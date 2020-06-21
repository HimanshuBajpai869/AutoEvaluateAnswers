// <copyright file="EvaluationController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace EvaluateMVCApp
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using AutoEvaluateShared;

    /// <summary>
    /// Represents Evaluation Controller.
    /// </summary>
    public class EvaluationController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationController"/> class.
        /// </summary>
        public EvaluationController()
        {
            this.KeyVaultHelper = this.KeyVaultHelper ?? new KeyVaultHelper();
            if (this.StorageHelper == null)
            {
                var storageKey = this.KeyVaultHelper.GetSecretFromKeyVault(Constants.StorageAccountKeySecretName).GetAwaiter().GetResult();
                this.StorageHelper = this.StorageHelper ?? new StorageHelper(Constants.StorageAccountName, storageKey);
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
        /// Represents the Evaluation Action.
        /// </summary>
        /// <returns>Action Result.</returns>
        public ActionResult Evaluation()
        {
            var scores = this.StorageHelper.GetScores();
            return this.View(scores);
        }

        /// <summary>
        /// Represents the Download Result Action.
        /// </summary>
        /// <param name="rollNumber">Roll Number of the student.</param>
        /// <returns>Action Result.</returns>
        public ActionResult ShowResult(string rollNumber)
        {
            var studentScores = this.StorageHelper.GetScoresForStudent(rollNumber);
            return this.View(studentScores);
        }
    }
}