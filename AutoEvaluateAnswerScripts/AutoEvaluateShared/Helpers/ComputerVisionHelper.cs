// <copyright file="ComputerVisionHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoEvaluateShared
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
    using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

    /// <summary>
    /// Represents Helper Class for accessing Computer Vision APIs.
    /// </summary>
    public class ComputerVisionHelper : IComputerVisionHelper
    {
        /// <summary>
        /// Computer Vision Client.
        /// </summary>
        private ComputerVisionClient computerVisionClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerVisionHelper"/> class.
        /// </summary>
        /// <param name="endpoint">Computer Vision Endpoint.</param>
        /// <param name="key">computer Vision Access Key.</param>
        public ComputerVisionHelper(string endpoint, string key)
        {
            this.computerVisionClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                {
                    Endpoint = endpoint,
                };
        }

        /// <summary>
        /// Read Text from image URL.
        /// </summary>
        /// <param name="urlImage">URL of the image.</param>
        /// <returns>Image Text.</returns>
        public async Task<string> BatchReadFileUrl(string urlImage)
        {
            // Read text from URL
            var textHeaders = await this.computerVisionClient.BatchReadFileAsync(urlImage).ConfigureAwait(false);

            // After the request, get the operation location (operation ID)
            string operationLocation = textHeaders.OperationLocation;

            var readOperationResult = await this.GetOperationResult(operationLocation).ConfigureAwait(false);
            return this.GetTextFromResult(readOperationResult);
        }

        /// <summary>
        /// Gets the result of the Read Operation.
        /// </summary>
        /// <param name="operationLocation">operation ID.</param>
        /// <returns>Read Operation Result.</returns>
        private async Task<ReadOperationResult> GetOperationResult(string operationLocation)
        {
            // Retrieve the URI where the recognized text will be stored from the Operation-Location header.
            // We only need the ID and not the full URL
            const int numberOfCharsInOperationId = 36;
            string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

            // Extract the text
            // Delay is between iterations and tries a maximum of 10 times.
            int i = 0;
            int maxRetries = 10;
            ReadOperationResult results;
            do
            {
                results = await this.computerVisionClient.GetReadOperationResultAsync(operationId);
                await Task.Delay(1000);
                if (i == 9)
                {
                    throw new Exception("Server timed out.");
                }
            }
            while ((results.Status == TextOperationStatusCodes.Running ||
                results.Status == TextOperationStatusCodes.NotStarted) && i++ < maxRetries);

            return results;
        }

        /// <summary>
        /// Get the Text from the Result.
        /// </summary>
        /// <param name="readOperationResult">Read Operation Result.</param>
        /// <returns>Text String.</returns>
        private string GetTextFromResult(ReadOperationResult readOperationResult)
        {
            // Display the found text.
            var imageContent = string.Empty;
            var textRecognitionLocalFileResults = readOperationResult.RecognitionResults;
            foreach (TextRecognitionResult recResult in textRecognitionLocalFileResults)
            {
                foreach (Line line in recResult.Lines)
                {
                    imageContent += line.Text + " ";
                }
            }

            return imageContent;
        }
    }
}
