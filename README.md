# TcpLocalChat

This solution contains realization of server and client applications for TCP chat using Sockets (System.Net.Sockets).

#### [Server (console application)](https://github.com/ViktoriyaKaleda/TcpChat/tree/master/LocalChat.Server.Console):

- Accepts connections from clients.
- When connected, it recognizes a username of a connected client and requires it to be unique.
- Receives message strings from clients and sends them to other connected clients.
- Stores the history of the last N messages that are sent to clients upon first connection.
- At the end of the application sends a notification to clients and correctly closes all connections.

#### Client ([console](https://github.com/ViktoriyaKaleda/TcpChat/tree/master/LocalChat.Client.Console) and [WPF](https://github.com/ViktoriyaKaleda/TcpChat/tree/master/LocalChat.Client.Wpf) versions):

- The console application asks after start: start a normal chat or either start a chatbot. Chatbot:
  - Connects with a new random name to the server.
  - Sends a random number of random messages with a random delay to the server.
  - Receives all messages from the server that are displayed on the screen.
  - Disconnects from server.
- Normal mode just looks like a usual chat app.
- WPF application is written using _MVVM pattern_, the design implemented using MaterialDesignThemes package.

  <img src="https://github.com/ViktoriyaKaleda/TcpChat/blob/master/screenshot.png" width="70%" height="70%" />
  
- Both console and GUI apps use [LocalChat.Domain](https://github.com/ViktoriyaKaleda/TcpChat/tree/master/LocalChat.Domain) library. As TCP operates on streams of data, not packets, so to get single “Send” results in a single “Receive” functionality, _length prefixing_ (prepends each message with the length of that message) is used.

The client and the server implemented using:

- the “For each client - own processing flow” scheme for the server
- Task Parallel Library

### Run

- Clone or download the repository
- Run the server console application
- Run clients (console and/or GUI versions)
