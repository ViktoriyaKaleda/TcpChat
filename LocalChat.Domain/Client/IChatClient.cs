using System;

namespace LocalChat.Domain.Client
{
    /// <summary>
    /// Represents a chat client.
    /// </summary>
    public interface IChatClient
    {
        /// <summary>
        /// Occurs after receiving a message.
        /// </summary>
        event EventHandler<MessageReceiveEventArgs> MessageReceive;

        /// <summary>
        /// Trys to set a username to a chat client.
        /// </summary>
        /// <param name="username">A username string.</param>
        /// <returns>True if client are logged in, false if username already exists or empty.</returns>
        bool TrySetUsername(string username);

        /// <summary>
        /// Starts processing incoming messages.
        /// </summary>
        void Process();

        /// <summary>
        /// Sends a message to a server.
        /// </summary>
        /// <param name="message">A message string.</param>
        void SendMessage(string message);

        /// <summary>
        /// Closes a chat client.
        /// </summary>
        void Close();
    }
}
