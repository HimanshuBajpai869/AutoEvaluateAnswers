// <copyright file="IStorageHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;

    /// <summary>
    /// Represents the Storage Helper Interface.
    /// </summary>
    public interface IStorageHelper
    {
        /// <summary>
        /// Uploads the answer scripts to storage account blob.
        /// </summary>
        /// <param name="answerScriptFile">Answer Script File.</param>
        /// <param name="blobReferenceName">Blob container name.</param>
        /// <param name="classStandard">Class Standard.</param>
        void UploadAnswerScript(HttpPostedFileBase answerScriptFile, string blobReferenceName, string classStandard = null);

        /// <summary>
        /// Gets the SAS URL of the answer image.
        /// </summary>
        /// <param name="storageContainerRef">Container Name.</param>
        /// <param name="storageResourceId">File Name.</param>
        /// <returns>A task to be awaited.</returns>
        Uri GetFileSASUrl(string storageContainerRef, string storageResourceId);

        /// <summary>
        /// Save Score to Storage Table.
        /// </summary>
        /// <param name="evaluatedScoreEntity">The Evaluated Score Entity.</param>
        /// <returns>A task to be awaited.</returns>
        Task SaveScoreInTable(EvaluatedScoreEntity evaluatedScoreEntity);

        /// <summary>
        /// Get Evaluated Scores by Roll Number.
        /// </summary>
        /// <param name="rollNumber">Roll Number.</param>
        /// <param name="answerNumber">Answer Number.</param>
        /// <returns>Evaluated Score Entity.</returns>
        Task<EvaluatedScoreEntity> GetEvaluatedScoreByRollNo(string rollNumber, string answerNumber);

        /// <summary>
        /// Get Evaluated Scores.
        /// </summary>
        /// <returns>A task to be awaited.</returns>
        List<EvaluatedScoreEntity> GetScores();

        /// <summary>
        /// Gets the Score for the Student.
        /// </summary>
        /// <param name="rollNumber">Roll Number.</param>
        /// <returns>Details of the scores obtained by the student.</returns>
        List<EvaluatedScoreEntity> GetScoresForStudent(string rollNumber);
    }
}
