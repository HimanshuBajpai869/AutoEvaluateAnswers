// <copyright file="StorageHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Represents Storage Helper Class.
    /// </summary>
    public class StorageHelper : IStorageHelper
    {
        /// <summary>
        /// Cloud Blob Client Instance.
        /// </summary>
        private readonly CloudBlobClient cloudBlobClient;

        /// <summary>
        /// Cloud Table Client.
        /// </summary>
        private readonly CloudTableClient cloudTableClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="StorageHelper"/> class.
        /// </summary>
        /// <param name="storageAccountName">Storage Account Name.</param>
        /// <param name="storageAccessKey">Storage Account Key.</param>
        public StorageHelper(string storageAccountName, string storageAccessKey)
        {
            if (!string.IsNullOrEmpty(storageAccountName) && !string.IsNullOrEmpty(storageAccessKey))
            {
                var credentials = new StorageCredentials(storageAccountName, storageAccessKey);
                var cloudStorageAccount = new CloudStorageAccount(credentials, useHttps: true);
                this.cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                this.cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            }
        }

        /// <summary>
        /// Uploads the answer scripts to storage account blob.
        /// </summary>
        /// <param name="answerScriptFile">Answer Script File.</param>
        /// <param name="blobReferenceName">Blob container name.</param>
        /// <param name="classStandard">Class Standard.</param>
        public void UploadAnswerScript(HttpPostedFileBase answerScriptFile, string blobReferenceName, string classStandard = null)
        {
            try
            {
                // TODO: We can create container classwise.
                CloudBlobContainer cloudBlobContainer = this.cloudBlobClient.GetContainerReference(Constants.StorageContainer);
                if (cloudBlobContainer.CreateIfNotExists())
                {
                    cloudBlobContainer.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
                }

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobReferenceName);
                cloudBlockBlob.Properties.ContentType = answerScriptFile.ContentType;
                cloudBlockBlob.UploadFromStream(answerScriptFile.InputStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the SAS URL of the answer image.
        /// </summary>
        /// <param name="storageContainerRef">Container Name.</param>
        /// <param name="storageResourceId">File Name.</param>
        /// <returns>A task to be awaited.</returns>
        public Uri GetFileSASUrl(string storageContainerRef, string storageResourceId)
        {
            Uri storageSASUrl = null;
            var storageSASUrlExpiryInHours = 3;
            CloudBlobContainer container = this.cloudBlobClient.GetContainerReference(storageContainerRef);
            if (container.Exists())
            {
                var sasPolicy = new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessStartTime = DateTime.Now,
                    SharedAccessExpiryTime = DateTime.Now.AddHours(storageSASUrlExpiryInHours),
                };

                var resourceId = $"{storageResourceId}";
                CloudBlockBlob blob = container.GetBlockBlobReference(resourceId);
                if (blob.Exists())
                {
                    string sasToken = blob.GetSharedAccessSignature(sasPolicy);
                    storageSASUrl = new Uri(blob.Uri, sasToken);
                }
            }

            return storageSASUrl;
        }

        /// <summary>
        /// Save Score to Storage Table.
        /// </summary>
        /// <param name="evaluatedScoreEntity">The Evaluated Score Entity.</param>
        /// <returns>A task to be awaited.</returns>
        public async Task SaveScoreInTable(EvaluatedScoreEntity evaluatedScoreEntity)
        {
            try
            {
                var table = this.cloudTableClient.GetTableReference(Constants.EvaluationTableName);
                await table.CreateIfNotExistsAsync().ConfigureAwait(false);

                TableOperation insertOperation = TableOperation.InsertOrReplace(evaluatedScoreEntity);
                await table.ExecuteAsync(insertOperation).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Evaluated Scores.
        /// </summary>
        /// <returns>A task to be awaited.</returns>
        public List<EvaluatedScoreEntity> GetScores()
        {
            var query = new TableQuery<EvaluatedScoreEntity>();
            var scoreTableReference = this.cloudTableClient.GetTableReference(Constants.EvaluationTableName);
            var scores = scoreTableReference.ExecuteQuery(query);
            return scores.ToList();
        }

        /// <summary>
        /// Get Evaluated Scores by Roll Number.
        /// </summary>
        /// <param name="rollNumber">Roll Number.</param>
        /// <param name="answerNumber">Answer Number.</param>
        /// <returns>Evaluated Score Entity.</returns>
        public async Task<EvaluatedScoreEntity> GetEvaluatedScoreByRollNo(string rollNumber, string answerNumber)
        {
            TableOperation tableOperation = TableOperation.Retrieve<EvaluatedScoreEntity>(answerNumber, rollNumber);
            var scoreTableReference = this.cloudTableClient.GetTableReference(Constants.EvaluationTableName);
            TableResult tableResult = await scoreTableReference.ExecuteAsync(tableOperation);
            return tableResult.Result as EvaluatedScoreEntity;
        }

        /// <summary>
        /// Gets the Score for the Student.
        /// </summary>
        /// <param name="rollNumber">Roll Number.</param>
        /// <returns>Details of the scores obtained by the student.</returns>
        public List<EvaluatedScoreEntity> GetScoresForStudent(string rollNumber)
        {
            var condition = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rollNumber);
            var query = new TableQuery<EvaluatedScoreEntity>().Where(condition);
            var scoreTableReference = this.cloudTableClient.GetTableReference(Constants.EvaluationTableName);
            var scores = scoreTableReference.ExecuteQuery(query);
            return scores.ToList();
        }
    }
}