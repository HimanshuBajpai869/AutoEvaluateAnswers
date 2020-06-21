// <copyright file="IMachineLearningServiceHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using System.Threading.Tasks;

    /// <summary>
    /// Machine Learning Service Helper Interface.
    /// </summary>
    public interface IMachineLearningServiceHelper
    {
        /// <summary>
        /// Execute Machine Learning Web Service.
        /// </summary>
        /// <param name="isLDAService">Flag indicating if LDA service has to be called.</param>
        /// <param name="apiKey">ML Service Key.</param>
        /// <param name="inputAnswer">Answer to validate.</param>
        /// <returns>A task to be awaited.</returns>
        Task<MLServiceResult> ExecuteMLWebService(bool isLDAService, string apiKey, string inputAnswer);
    }
}
