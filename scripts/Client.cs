using Godot;
using System;
using System.Runtime.CompilerServices;
using Godot.Collections;
using Google.Protobuf;

public partial class Client : Node
{
    private WebSocketPeer socket;
    private bool connected = false;
    private string serverUrl = "ws://localhost:3000";

    public override void _Ready()
    {
        socket = new WebSocketPeer();
        ConnectToServer();
    }

    public override void _ExitTree()
    {
        if (connected)
        {
            socket.Close();
        }
    }

    public override void _Process(double delta)
    {
        socket.Poll();

        WebSocketPeer.State state = socket.GetReadyState();

        switch (state)
        {
            case WebSocketPeer.State.Open:
                if (!connected)
                {
                    connected = true;
                    GD.Print("Connected to WebSocket server!");

                    NetworkMessage msg = MessageFactory.CreatePlayerJoinRequestMessage("maks");
                    SendMessage(msg);
                }

                while (socket.GetAvailablePacketCount() > 0)
                {
                    byte[] packet = socket.GetPacket();
                    string message = System.Text.Encoding.UTF8.GetString(packet);
                    GD.Print($"Received: {message}");
                    HandleMessage(message);
                }
                break;

            case WebSocketPeer.State.Closing:
                GD.Print("Connection is closing...");
                break;

            case WebSocketPeer.State.Closed:
                int code = socket.GetCloseCode();
                string reason = socket.GetCloseReason();
                GD.Print($"Connection closed with code: {code} reason: {reason}");
                connected = false;
                break;

            case WebSocketPeer.State.Connecting:
                break;
        }
    }

    public void SendMessage(NetworkMessage msg)
    {
        socket.Send(msg.ToByteArray());
    }

    public bool IsConnected()
    {
        return connected;
    }

    private void HandleMessage(string message)
    {
        // Handle incoming messages from your C++ server
        GD.Print($"Handling message: {message}");

        // You can parse JSON messages here if needed
        // Example with Godot's JSON parser:
        /*
        var json = new Json();
        Error parseResult = json.Parse(message);
        if (parseResult == Error.Ok)
        {
            Variant data = json.Data;
            // Handle the parsed data
            GD.Print($"Parsed JSON: {data}");
        }
        */
    }
    private void ConnectToServer()
    {
        Error error = socket.ConnectToUrl(serverUrl);

        if (error != Error.Ok)
        {
            GD.PrintErr($"Failed to connect to WebSocket server: {error}");
            return;
        }

        GD.Print($"Attempting to connect to {serverUrl}");
    }
}
