using System;

namespace LocalChat.Domain
{
    /// <summary>
    /// Represents a chat message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets a message username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets a message text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a message created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/>.
        /// </summary>
        /// <param name="message">A message text. Acceptable formats: ":{Text}", "{Username}:{Text}", "{Username}:{Text}:DateTime:{CreatedDate}", 
        /// ":{Text}:DateTime:{CreatedDate}".</param>
        /// <param name="createdDate">A created date time.</param>
        public Message(string message, DateTime createdDate)
        {
            int index = message.IndexOf(':');
            if (index == -1)
                Username = "";
            else
                Username = message.Substring(0, index);

            Text = message.Substring(message.IndexOf(':') + 1);
            CreatedDate = createdDate;

            // a case when the message contains created date time
            string dateTimeSeparator = ":DateTime:";
            index = Text.LastIndexOf(dateTimeSeparator);
            if (index != -1)
            {
                var dateTimeString = Text.Substring(index + dateTimeSeparator.Length, Text.Length - index - dateTimeSeparator.Length);
                Text = Text.Substring(0, index);
                CreatedDate = Convert.ToDateTime(dateTimeString);
            }
        }

        public override string ToString()
        {
            return $"{Username}: {Text} {CreatedDate.ToShortTimeString()}";
        }
    }
}
