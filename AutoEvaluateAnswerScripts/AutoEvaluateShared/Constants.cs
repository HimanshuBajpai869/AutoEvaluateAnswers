// <copyright file="Constants.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    /// <summary>
    /// Represents the constants required by the application.
    /// </summary>
    public class Constants
    {
        /// <summary>
        /// Gets the secret name of the key used to access Cognitive Services.
        /// </summary>
        public const string CognitiveServicesKeySecretName = "CognitiveServicesKey";

        /// <summary>
        /// Gets the Storage Account name which is used to store the scripts.
        /// </summary>
        public const string StorageAccountName = "autoevaluationsa";

        /// <summary>
        /// Gets the Storage Container Name which contains all the answer scripts.
        /// </summary>
        public const string StorageContainer = "studentsubmissions";

        /// <summary>
        /// Gets the secret name of the key used to access Storage Account Key.
        /// </summary>
        public const string StorageAccountKeySecretName = "StorageAccouneKey";

        /// <summary>
        /// Gets the secret name of the key used to access emdpoint for Science Answers.
        /// </summary>
        public const string AnswerScienceSecretName = "AnswerScienceKey";

        /// <summary>
        /// The DNS name of the Key Vault.
        /// </summary>
        public const string KeyVaultUri = "https://autoevaluationkv.vault.azure.net/";

        /// <summary>
        /// Gets the Service Bus End point.
        /// </summary>
        public const string ServiceBusEndpoint = "sb://autoevaluationservicebus.servicebus.windows.net/";

        /// <summary>
        /// Gets the Service Bus Queue Name.
        /// </summary>
        public const string ServiceBusQueueName = "autoevaluation-queue";

        /// <summary>
        /// Gets the Cognitive Service Uri.
        /// </summary>
        public const string CognitiveServiceUri = "https://autoevaluationcv.cognitiveservices.azure.com/";

        /// <summary>
        /// Gets the storage account table name which stores the score of the student.
        /// </summary>
        public const string EvaluationTableName = "StudentScore";

        /// <summary>
        /// Gets the key used for accessing the API used for Extracting Features from Text.
        /// </summary>
        public const string ExtractFeaturesKeySecretName = "ExtractFeaturesKey";

        /// <summary>
        /// LDA Service ID.
        /// </summary>
        public const string LDAServiceID = "a0217785b3fa49128a4f43f58d8c5735";

        /// <summary>
        /// Extract Features Service Id.
        /// </summary>
        public const string ExtractFeaturesServiceID = "4b5430cf2f9e4ae8ab92d672b2e202be";
    }
}
