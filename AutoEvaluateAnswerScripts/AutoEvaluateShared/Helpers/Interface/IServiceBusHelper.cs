// <copyright file="IServiceBusHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents Sefvice Bus Helper Interface.
    /// </summary>
    public interface IServiceBusHelper
    {
        /// <summary>
        /// Send the mesasge to Service Bus Queue.
        /// </summary>
        /// <param name="fileName">File Name.</param>
        /// <returns>A Task to be awaited.</returns>
        Task SendRequest(string fileName);

        /// <summary>
        /// Complete the Service Bus Queue Message.
        /// </summary>
        /// <param name="lockToken">Lock Token.</param>
        /// <returns>A task to be awaited.</returns>
        Task CompleteRequest(string lockToken);

        /// <summary>
        /// Registers callback for message received from the Service Bus Queue.
        /// </summary>
        /// <param name="callback">Callback Delegate.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        void RegisterMessageReceivedCallback(Func<string, string, Task> callback, CancellationToken cancellationToken);
    }
}
