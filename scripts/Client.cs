using Godot;
using System;
using System.Runtime.CompilerServices;
using Godot.Collections;
using Google.Protobuf;

public partial class Client : Node
{
    [Export] private Game _game;

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
                    NetworkMessage msg = NetworkMessage.Parser.ParseFrom(packet);
                    HandleMessage(msg);
                }
                break;

            case WebSocketPeer.State.Closing:
                GD.Print("Connection is closing...");
                break;

            case WebSocketPeer.State.Closed:
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

    private void HandleMessage(NetworkMessage msg)
    {
        GD.Print($"Handling message: {msg}");

        switch (msg.Type)
        {
            case MessageType.PlayerMove:
                _game.SetMoving(false);
                break;
        }
    }

    public bool IsConnected()
    {
        return connected;
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
