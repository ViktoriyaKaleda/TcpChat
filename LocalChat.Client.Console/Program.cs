using LocalChat.Domain;
using LocalChat.Domain.Chatbot;

namespace LocalChat.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //new LocalChatConsoleManager().StartChating();

            var chatbot = new Chatbot();
            chatbot.SubscribeOnMessageReceiveEvent(new System.EventHandler<MessageReceiveEventArgs>(PrintReceivedMessageToConsole));
            chatbot.ChtatbotInternalMessageSend += PrintChatbotMessageToConsole;
            chatbot.Start();
            System.Console.ReadLine();
        }

        private static void PrintReceivedMessageToConsole(object sender, MessageReceiveEventArgs messageReceiveEventArgs)
        {
            var message = messageReceiveEventArgs.Message;
            System.Console.WriteLine($"{message.CreatedDate.ToShortTimeString()} {message.Username}: {message.Text}");
        }

        private static void PrintChatbotMessageToConsole(object sender, string message)
            => System.Console.WriteLine(message);
    }
}
