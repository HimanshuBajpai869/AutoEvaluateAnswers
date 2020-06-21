// <copyright file="ServiceBusHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
namespace AutoEvaluateShared
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Primitives;

    /// <summary>
    /// Represents the Service Bus Helper.
    /// </summary>
    public class ServiceBusHelper : IServiceBusHelper
    {
        /// <summary>
        /// Queue Client.
        /// </summary>
        private QueueClient queueClient;

        /// <summary>
        /// Flag indicating if the Service Bus is registered.
        /// </summary>
        private bool isRegistered = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusHelper"/> class.
        /// </summary>
        public ServiceBusHelper()
        {
            var tokenProvider = TokenProvider.CreateManagedIdentityTokenProvider();
            this.queueClient = new QueueClient(Constants.ServiceBusEndpoint, Constants.ServiceBusQueueName, tokenProvider);
        }

        /// <summary>
        /// Sends the request to Service Bus.
        /// </summary>
        /// <param name="fileName">File Name.</param>
        /// <returns>A task to be awaited.</returns>
        public async Task SendRequest(string fileName)
        {
            await this.queueClient.SendAsync(this.CreateMessageForQueueClient(fileName)).ConfigureAwait(false);
        }

        /// <summary>
        /// Complete the request in the Queue Client.
        /// </summary>
        /// <param name="lockToken">Lock Token.</param>
        /// <returns>A task to be awaited.</returns>
        public async Task CompleteRequest(string lockToken)
        {
            await this.queueClient.CompleteAsync(lockToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Registers callback for message received from the Service Bus Queue.
        /// </summary>
        /// <param name="callback">Callback Delegate.</param>
        /// <param name="cancellationToken">Cancellation Token.</param>
        public void RegisterMessageReceivedCallback(Func<string, string, Task> callback, CancellationToken cancellationToken)
        {
            if (!this.isRegistered)
            {
                var messageHandlerOptions = new MessageHandlerOptions(this.ExceptionReceivedHandler)
                {
                    AutoComplete = false,
                    MaxConcurrentCalls = 10,
                };

                this.queueClient.RegisterMessageHandler(
                         async (message, token) =>
                         {
                             if (!cancellationToken.IsCancellationRequested)
                             {
                                 using (CancellationTokenRegistration ctr = token.Register(() => cancellationToken.ThrowIfCancellationRequested()))
                                 {
                                     var fileName = CreateRequestFromServiceBus(message);
                                     await callback(message.SystemProperties.LockToken, fileName).ConfigureAwait(false);
                                 }
                             }
                         }, messageHandlerOptions);
                this.isRegistered = true;
            }
        }

        /// <summary>
        /// Create Request to be sent for Service Bus.
        /// </summary>
        /// <param name="message">Service Bus Message.</param>
        /// <returns>Message Id.</returns>
        private static string CreateRequestFromServiceBus(Message message)
        {
            return message.MessageId;
        }

        /// <summary>
        /// Exception Received Handler.
        /// </summary>
        /// <param name="exceptionReceivedEventArgs">Exception Received Event Args.</param>
        /// <returns>A task to be awaited.</returns>
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            throw exceptionReceivedEventArgs.Exception;
        }

        /// <summary>
        /// Create Message to be send to Queue Client.
        /// </summary>
        /// <param name="fileName">File Name.</param>
        /// <returns>Message instance.</returns>
        private Message CreateMessageForQueueClient(string fileName)
        {
            return new Message()
            {
                MessageId = fileName,
            };
        }
    }
}
