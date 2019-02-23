using LocalChat.Domain.Client;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LocalChat.Domain.Chatbot
{
    /// <summary>
    /// Represents a chatbot which can send a random number of rendom messages with random pauses between them to server, receives all messages from the server.
    /// </summary>
    public class Chatbot
    {
        private readonly Random _random = new Random();
        private readonly ChatClient _chatClient = new ChatClient("127.0.0.1", 8005);
        private readonly int _minMessageCount;
        private readonly int _maxMessageCount;
        private readonly int _minMessageDelayTime;
        private readonly int _maxMessageDelayTime;

        private readonly List<string> Usernames = new List<string>
        {
            "Vasya",
            "Petya",
            "Sasha",
            "Tanya",
            "Vanya",
            "Lena"
        };

        private readonly List<string> Messages = new List<string>
        {
            "quia et suscipit",
            "suscipit recusandae consequuntur expedita et cum",
            "est rerum tempore vitae",
            "et iusto sed quo iure",
            "ullam et saepe reiciendis voluptatem adipisci",
            "repudiandae veniam quaerat sunt sed",
            "ut aspernatur corporis harum nihil",
            "dolore placeat quibusdam ea quo vitae",
            "dignissimos aperiam dolorem qui eum",
            "consectetur animi nesciunt iure dolore",
            "quo et expedita modi cum officia",
            "delectus reiciendis molestiae occaecati",
            "itaque id aut magnam",
            "aut dicta possimus sint mollitia",
            "fuga et accusamus dolorum perferendis illo",
            "reprehenderit quos placeat",
            "suscipit nam nisi quo aperiam aut",
            "eos voluptas et aut odit natus earum",
            "eveniet quo quis",
            "illum quis cupiditate provident sit magnam",
        };

        public EventHandler<string> ChtatbotInternalMessageSend;

        /// <summary>
        /// Initializes a new instance of the <see cref="Chatbot"/> with defaults values for boundaries value of messages count and delay time.
        /// </summary>
        public Chatbot()
        {
            _minMessageCount = 1;
            _maxMessageCount = 20;
            _minMessageDelayTime = 500;
            _maxMessageDelayTime = 3000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Chatbot"/>.
        /// </summary>
        /// <param name="minMessageCount">A min value for sent messages count.</param>
        /// <param name="maxMessageCount">A max value for sent messages count.</param>
        /// <param name="minMessageDelayTime">A min value for messages delay.</param>
        /// <param name="maxMessageDelayTime">A max value for messages delay.</param>
        public Chatbot(int minMessageCount, int maxMessageCount, int minMessageDelayTime, int maxMessageDelayTime)
        {
            _minMessageCount = minMessageCount;
            _maxMessageCount = maxMessageCount;
            _minMessageDelayTime = minMessageDelayTime;
            _maxMessageDelayTime = maxMessageDelayTime;
        }

        /// <summary>
        /// Logins with a random username, sends a random number of random messages with random pauses between them to server, receives all messages from the server, closes connection.
        /// </summary>
        public void Start()
        {
            int messagesCount = _random.Next(1, _maxMessageCount);

            string username;
            do
            {
                username = Usernames[_random.Next(Usernames.Count - 1)];
            }
            while (_chatClient.TrySetUsername(username) != true);

            ChtatbotInternalMessageSend?.Invoke(this, $"You logged in as '{username}'.");

            Thread clientThread = new Thread(new ThreadStart(_chatClient.Process));
            clientThread.Start();

            for (int i = 0; i < messagesCount; i++)
            {
                string message = Messages[_random.Next(Messages.Count - 1)];
                _chatClient.SendMessage(message);
                Thread.Sleep(_random.Next(_minMessageDelayTime, _maxMessageDelayTime));
            }

            ChtatbotInternalMessageSend?.Invoke(this, $"{DateTime.Now.ToShortTimeString()} {username}: This was my last message! Close the chat ...");

            _chatClient.Close();
            
        }

        /// <summary>
        /// Subscribes on <see cref="ChatClient.MessageReceive"/> event.
        /// </summary>
        /// <param name="eventHandler">An event handler.</param>
        public void SubscribeOnMessageReceiveEvent(EventHandler<MessageReceiveEventArgs> eventHandler)
        {
            _chatClient.MessageReceive += eventHandler;
        }
    }
}
