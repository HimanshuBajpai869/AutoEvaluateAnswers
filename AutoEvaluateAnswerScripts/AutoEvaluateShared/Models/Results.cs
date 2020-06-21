// <copyright file="Results.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the Results of the Service.
    /// </summary>
    public class Results
    {
        /// <summary>
        /// Gets or sets the Output of the Service.
        /// </summary>
        [JsonProperty("output1")]
        public Output1[] Outputs { get; set; }
    }
}
