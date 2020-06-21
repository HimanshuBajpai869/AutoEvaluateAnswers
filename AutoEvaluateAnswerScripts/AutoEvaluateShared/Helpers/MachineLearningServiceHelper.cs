// <copyright file="MachineLearningServiceHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the Machine Learning Service Helper class.
    /// </summary>
    public class MachineLearningServiceHelper : IMachineLearningServiceHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MachineLearningServiceHelper"/> class.
        /// </summary>
        /// <param name="apiKey">Machine Learning Key.</param>
        public MachineLearningServiceHelper()
        {
        }

        /// <summary>
        /// Execute Machine Learning Web Service.
        /// </summary>
        /// <param name="isLDAService">Flag indicating if LDA service has to be called.</param>
        /// <param name="apiKey">ML Service Key.</param>
        /// <param name="inputAnswer">Answer to validate.</param>
        /// <returns>A task to be awaited.</returns>
        public async Task<MLServiceResult> ExecuteMLWebService(bool isLDAService, string apiKey, string inputAnswer)
        {
            var serviceID = isLDAService ? Constants.LDAServiceID : Constants.ExtractFeaturesServiceID;

            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, List<Dictionary<string, string>>>()
                    {
                        {
                            "input1",
                            new List<Dictionary<string, string>>()
                            {
                                new Dictionary<string, string>()
                                {
                                            {
                                                "Answers", inputAnswer
                                            },
                                },
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>() { },
                };

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri($"https://ussouthcentral.services.azureml.net/workspaces/ee632bd67d7b47cb878120875af36cdd/services/{serviceID}/execute?api-version=2.0&format=swagger");
                HttpResponseMessage response = await client.PostAsJsonAsync(string.Empty, scoreRequest).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var resultFromJson = JsonConvert.DeserializeObject<MLServiceResult>(result);
                    return resultFromJson;
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new Exception(responseContent);
                }
            }
        }
    }
}
