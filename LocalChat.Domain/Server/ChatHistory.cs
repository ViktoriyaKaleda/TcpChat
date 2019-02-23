using System;
using System.Collections.Generic;

namespace LocalChat.Domain.Server
{
    /// <summary>
    /// Represents a chat hystory.
    /// </summary>
    public class ChatHistory
    {     
        public Queue<Message> MessagesHistory { get; } = new Queue<Message>();

        private readonly int _messagesHistoryCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatHistory"/>.
        /// </summary>
        /// <param name="messagesHistoryCount">Count of the stored messages in history.</param>
        public ChatHistory(int messagesHistoryCount)
        {
            if (messagesHistoryCount < 0)
                throw new ArgumentOutOfRangeException(nameof(messagesHistoryCount), "Non-negative number required.");

            _messagesHistoryCount = messagesHistoryCount;
        }

        /// <summary>
        /// Adds a new message in the message's history.
        /// </summary>
        /// <param name="message"></param>
        public void AddNewMessage(Message message)
        {
            MessagesHistory.Enqueue(message);
            if (MessagesHistory.Count > _messagesHistoryCount)
            {
                MessagesHistory.Dequeue();
            }
        }
    }
}
