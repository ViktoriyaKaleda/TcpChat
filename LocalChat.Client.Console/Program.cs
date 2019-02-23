using LocalChat.Domain;
using LocalChat.Domain.Chatbot;

namespace LocalChat.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string userInput;
            while(true)
            {
                System.Console.WriteLine("Press '1' to start a chatbot and '2' to start a normal chat: ");
                userInput = System.Console.ReadLine();
                if (userInput != "1" || userInput != "2")
                    System.Console.WriteLine("Invalid input. Try again, please.");
                else
                    break;
            }          

            if (userInput == "1")
            {
                var chatbot = new Chatbot();
                chatbot.SubscribeOnMessageReceiveEvent(new System.EventHandler<MessageReceiveEventArgs>(PrintReceivedMessageToConsole));
                chatbot.ChtatbotInternalMessageSend += PrintChatbotMessageToConsole;
                chatbot.Start();
                System.Console.ReadLine();
            }

            else
            {
                new LocalChatConsoleManager().StartChating();
            }                      
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
