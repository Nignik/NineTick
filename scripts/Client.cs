using Godot;
using System;
using System.Runtime.CompilerServices;
using Godot.Collections;
using Google.Protobuf;

public partial class Client : Node
{
    private GameLogic _game;
    private WebSocketPeer _socket;
    private bool _connected = false;
    private string _serverUrl = "ws://localhost:3000";
    private string _playerId = "maks";

    [Signal] public delegate void PlayerMoveSignalEventHandler(int row, int col, int color);

    public override void _Ready()
    {
        _socket = new WebSocketPeer();
        ConnectToServer();
    }

    public override void _ExitTree()
    {
        if (_connected)
        {
            _socket.Close();
        }
    }

    public override void _Process(double delta)
    {
        _socket.Poll();

        WebSocketPeer.State state = _socket.GetReadyState();

        switch (state)
        {
            case WebSocketPeer.State.Open:
                if (!_connected)
                {
                    _connected = true;
                    GD.Print("Connected to WebSocket server!");

                    NetworkMessage msg = MessageFactory.CreatePlayerJoinRequestMessage(_playerId);
                    SendMessage(msg);
                }

                while (_socket.GetAvailablePacketCount() > 0)
                {
                    byte[] packet = _socket.GetPacket();
                    NetworkMessage msg = NetworkMessage.Parser.ParseFrom(packet);
                    HandleMessage(msg);
                }
                break;

            case WebSocketPeer.State.Closing:
                GD.Print("Connection is closing...");
                break;

            case WebSocketPeer.State.Closed:
                _connected = false;
                break;

            case WebSocketPeer.State.Connecting:
                break;
        }
    }

    public void SendMessage(NetworkMessage msg)
    {
        _socket.Send(msg.ToByteArray());
    }

    private void HandleMessage(NetworkMessage msg)
    {
        GD.Print($"Handling message: {msg}");

        switch (msg.Type)
        {
            case MessageType.PlayerJoinApproved:
                PlayerJoinApproved joinApprovedMsg = msg.PlayerJoinApproved;
                GD.Print($"Player join for {_playerId} has been approved");
                if (joinApprovedMsg.PlayerColor == PlayerColor.White)
                    _game.SetMoving(true);

                break;
            case MessageType.PlayerMove:
                PlayerMove moveMsg = msg.PlayerMove;
                GD.Print($"Player {moveMsg.PlayerId} the move: {moveMsg.TileRow}, {moveMsg.TileCol}");
                if (_playerId != moveMsg.PlayerId)
                {
                    EmitSignal(SignalName.PlayerMoveSignal, moveMsg.TileRow, moveMsg.TileCol, (int)moveMsg.PlayerColor);
                 
                }
                
                break;
        }
    }

    public bool IsConnected()
    {
        return _connected;
    }

    private void ConnectToServer()
    {
        Error error = _socket.ConnectToUrl(_serverUrl);

        if (error != Error.Ok)
        {
            GD.PrintErr($"Failed to connect to WebSocket server: {error}");
            return;
        }

        GD.Print($"Attempting to connect to {_serverUrl}");
    }
}
