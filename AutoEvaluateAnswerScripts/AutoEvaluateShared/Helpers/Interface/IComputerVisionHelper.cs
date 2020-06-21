// <copyright file="IComputerVisionHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using System.Threading.Tasks;

    /// <summary>
    /// Computer Vision Helper Interface.
    /// </summary>
    public interface IComputerVisionHelper
    {
        /// <summary>
        /// Read Text from image URL.
        /// </summary>
        /// <param name="urlImage">URL of the image.</param>
        /// <returns>Image Text.</returns>
        Task<string> BatchReadFileUrl(string urlImage);
    }
}
