// <copyright file="SubmissionController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace EvaluateMVCApp
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using AutoEvaluateShared;

    /// <summary>
    /// Represents Submission Controller.
    /// </summary>
    public class SubmissionController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubmissionController"/> class.
        /// </summary>
        public SubmissionController()
        {
            this.KeyVaultHelper = this.KeyVaultHelper ?? new KeyVaultHelper();
            this.ServiceBusHelper = this.ServiceBusHelper ?? new ServiceBusHelper();
            this.ServiceBusHelper.RegisterMessageReceivedCallback(this.ProcessRequest, default(CancellationToken));

            if (this.StorageHelper == null)
            {
                var storageKey = this.KeyVaultHelper.GetSecretFromKeyVault(Constants.StorageAccountKeySecretName).GetAwaiter().GetResult();
                this.StorageHelper = this.StorageHelper ?? new StorageHelper(Constants.StorageAccountName, storageKey);
            }

            if (this.ComputerVisionHelper == null)
            {
                var cognitiveServiceKey = this.KeyVaultHelper.GetSecretFromKeyVault(Constants.CognitiveServicesKeySecretName).GetAwaiter().GetResult();
                this.ComputerVisionHelper = this.ComputerVisionHelper ?? new ComputerVisionHelper(Constants.CognitiveServiceUri, cognitiveServiceKey);
            }

            if (this.MachineLearningServiceHelper == null)
            {
                this.MachineLearningServiceHelper = new MachineLearningServiceHelper();
            }
        }

        /// <summary>
        /// Gets or sets the Key Vault Helper.
        /// </summary>
        public IKeyVaultHelper KeyVaultHelper { get; set; }

        /// <summary>
        /// Gets or sets the Storage Helper.
        /// </summary>
        public IStorageHelper StorageHelper { get; set; }

        /// <summary>
        /// Gets or sets the Service Bus Helper.
        /// </summary>
        public IServiceBusHelper ServiceBusHelper { get; set; }

        /// <summary>
        /// Gets or sets the Computer Vision Helper.
        /// </summary>
        public IComputerVisionHelper ComputerVisionHelper { get; set; }

        /// <summary>
        /// Gets or sets the Machine Learning Service Helper.
        /// </summary>
        public IMachineLearningServiceHelper MachineLearningServiceHelper { get; set; }

        /// <summary>
        /// Represents the GET Upload Submission Action.
        /// </summary>
        /// <returns>Action Result.</returns>
        [HttpGet]
        public ActionResult UploadSubmission()
        {
            return this.View();
        }

        /// <summary>
        /// Represents the POST Upload Submission Action.
        /// </summary>
        /// <param name="student">Student Model.</param>
        /// <returns>Action Result.</returns>
        [HttpPost]
        public ActionResult UploadSubmission(Student student)
        {
            var allowedExtensions = new[]
            {
                ".jpg", ".png", ".jpeg", ".PNG", ".JPG", ".JPEG",
            };
            var fileName = Path.GetFileName(student.ImageFile.FileName);
            var extension = Path.GetExtension(student.ImageFile.FileName);

            if (allowedExtensions.Contains(extension))
            {
                // Generate the File Name.
                var submissionFileName = student.RollNo + "_" + student.StudentName + "_" + student.Standard + "_" + student.Question + "_" + DateTime.UtcNow.ToString("s") + extension;

                // Upload answer scripts to storage account container.
                this.StorageHelper.UploadAnswerScript(student.ImageFile, submissionFileName, student.Standard.ToString());

                // Prepare the Evaluate Score Entity.
                var evaluateScoreEntity = new EvaluatedScoreEntity(student.RollNo.ToString(), student.Question.ToString())
                {
                    RollNo = student.RollNo.ToString(),
                    Question = student.Question.ToString(),
                    Class = student.Standard.ToString(),
                    StudentName = student.StudentName,
                    Subject = student.Subject,
                    EvaluatedScore = "Evaluating",
                };

                // Save the Evaluate Score Entity to the Storage Table.
                this.StorageHelper.SaveScoreInTable(evaluateScoreEntity).ConfigureAwait(false);

                // Send the Request to Service Bus.
                this.ServiceBusHelper.SendRequest(submissionFileName).GetAwaiter().GetResult();

                return this.RedirectToAction("Evaluation", "Evaluation");
            }
            else
            {
                this.ViewBag.ValidationMessage = "Please upload only Image file.";
            }

            return this.View();
        }

        /// <summary>
        /// Process Request Call Back Method.
        /// </summary>
        /// <param name="serviceBusMessageLockToken">Service Bus Lock Token.</param>
        /// <param name="fileName">File Name.</param>
        /// <returns>A task to be awaited.</returns>
        public async Task ProcessRequest(string serviceBusMessageLockToken, string fileName)
        {
            // Complete the Request in Service Bus.
            await this.ServiceBusHelper.CompleteRequest(serviceBusMessageLockToken).ConfigureAwait(false);

            // Get the LDA API Key from Key Vault.
            var ldaApiKey = this.KeyVaultHelper.GetSecretFromKeyVault(Constants.AnswerScienceSecretName).GetAwaiter().GetResult();

            // Get the Extract Features API from Key Vault.
            var extractFeaturesApiKey = this.KeyVaultHelper.GetSecretFromKeyVault(Constants.ExtractFeaturesKeySecretName).GetAwaiter().GetResult();

            // Get SAS Url of the image from the container.
            var imageUrl = this.StorageHelper.GetFileSASUrl(Constants.StorageContainer, fileName);

            // Pass the SAS URL as input cognitive service and get the text in the image.
            var textFromImage = await this.ComputerVisionHelper.BatchReadFileUrl(imageUrl.ToString()).ConfigureAwait(false);

            // Pass the extracted text to the LDA Model.
            var ldaResponse = await this.MachineLearningServiceHelper.ExecuteMLWebService(true, ldaApiKey, textFromImage).ConfigureAwait(false);

            // Pass the extracted text to the Extract Features Model.
            var featuresResponse = await this.MachineLearningServiceHelper.ExecuteMLWebService(false, extractFeaturesApiKey, textFromImage).ConfigureAwait(false);

            // Evaluate the LDA Model Response.
            var markedScore = 2;
            var ldaOutput = ldaResponse.Results.Outputs.FirstOrDefault();
            var score = decimal.Parse(ldaOutput.Topic1, System.Globalization.NumberStyles.Float);
            score += decimal.Parse(ldaOutput.Topic2, System.Globalization.NumberStyles.Float);
            score += decimal.Parse(ldaOutput.Topic3, System.Globalization.NumberStyles.Float);
            score += decimal.Parse(ldaOutput.Topic4, System.Globalization.NumberStyles.Float);
            score += decimal.Parse(ldaOutput.Topic5, System.Globalization.NumberStyles.Float);

            var averageScoreFromLDA = score / 5;

            // Evaluate the Features Model Response.
            var featuresOutput = featuresResponse.Results.Outputs.FirstOrDefault();

            // Allocate Final Scores based on both LDA and Features Model.
            if (averageScoreFromLDA > 0.8M || featuresOutput.KeyFeatures.Split(',').Count() >= 4)
            {
                markedScore = 5;
            }

            // Save the scores in the Storage Table.
            // 1_Himanshu_2020-06-09T12:05:39.jpeg
            var splitInput = fileName.Split('_');

            var evaluateScoreEntity = await this.StorageHelper.GetEvaluatedScoreByRollNo(splitInput[0], splitInput[3]);
            if (evaluateScoreEntity != null)
            {
                evaluateScoreEntity.EvaluatedScore = markedScore.ToString();
            }

            await this.StorageHelper.SaveScoreInTable(evaluateScoreEntity).ConfigureAwait(false);
        }
    }
}