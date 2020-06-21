// <copyright file="Output1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the output of the service.
    /// </summary>
    public class Output1
    {
        /// <summary>
        /// Gets or sets the Input Answer.
        /// </summary>
        public string Answers { get; set; }

        /// <summary>
        /// Gets or sets the answer after preprocessing.
        /// </summary>
        [JsonProperty("Preprocessed Answers")]
        public string PreprocessedAnswers { get; set; }

        /// <summary>
        /// Gets or sets the score for topic one.
        /// </summary>
        public string Topic1 { get; set; }

        /// <summary>
        /// Gets or sets the score for topic two.
        /// </summary>
        public string Topic2 { get; set; }

        /// <summary>
        /// Gets or sets the score for topic three.
        /// </summary>
        public string Topic3 { get; set; }

        /// <summary>
        /// Gets or sets the score for topic four.
        /// </summary>
        public string Topic4 { get; set; }

        /// <summary>
        /// Gets or sets the score for topic five.
        /// </summary>
        public string Topic5 { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of Key Features.
        /// </summary>
        [JsonProperty("Key Phrases")]
        public string KeyFeatures { get; set; }
    }
}
