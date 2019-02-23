using System;

namespace LocalChat.Domain.Server
{
    /// <summary>
    /// Represents a chaat server.
    /// </summary>
    public interface IChatServer
    {
        /// <summary>
        /// Occurs after receiving a message.
        /// </summary>
        event EventHandler<MessageReceiveEventArgs> MessageReceive;

        /// <summary>
        /// Starts a chat server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops a chat server.
        /// </summary>
        void Stop();

        /// <summary>
        /// Logins new client and starts processing its messages.
        /// </summary>
        void Process(object chatClient);

        /// <summary>
        /// Broadcasts message to connected clients.
        /// </summary>
        void BroadcastMessage(string message);
    }
}
