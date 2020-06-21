// <copyright file="KeyVaultHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using System.Threading.Tasks;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;

    /// <summary>
    /// Represents the Key Vault Helper.
    /// </summary>
    public class KeyVaultHelper : IKeyVaultHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultHelper"/> class.
        /// </summary>
        public KeyVaultHelper()
        {
            var tokenProvider = new AzureServiceTokenProvider();
            this.KeyVaultClientInstance = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(tokenProvider.KeyVaultTokenCallback));
        }

        /// <summary>
        /// Gets Key Vault Client.
        /// </summary>
        private KeyVaultClient KeyVaultClientInstance { get; }

        /// <summary>
        /// Gets the Secret from the Key Vault.
        /// </summary>
        /// <param name="secretName">Secret Name.</param>
        /// <returns>Secret Value.</returns>
        public async Task<string> GetSecretFromKeyVault(string secretName)
        {
            var secret = await this.KeyVaultClientInstance.GetSecretAsync(Constants.KeyVaultUri, secretName).ConfigureAwait(false);
            return secret.Value;
        }
    }
}
