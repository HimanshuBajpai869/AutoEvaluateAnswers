// <copyright file="IKeyVaultHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using System.Threading.Tasks;

    /// <summary>
    /// Key Vault Helper Interface.
    /// </summary>
    public interface IKeyVaultHelper
    {
        /// <summary>
        /// Gets the Secret from the Key Vault.
        /// </summary>
        /// <param name="secretName">Secret Name.</param>
        /// <returns>Secret Value.</returns>
        Task<string> GetSecretFromKeyVault(string secretName);
    }
}
