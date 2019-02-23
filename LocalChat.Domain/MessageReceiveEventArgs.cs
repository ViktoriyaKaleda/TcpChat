using System;

namespace LocalChat.Domain
{
    /// <summary>
    /// Represents an argument for message receive event.
    /// </summary>
    public class MessageReceiveEventArgs : EventArgs
    {
        public Message Message { get; set; }
    }
}
