using Godot;
using System;
using Godot.Collections;
using Google.Protobuf;

public static class MessageFactory
{
    public static NetworkMessage CreatePlayerJoinRequestMessage(string playerId)
    {
        var joinMsg = new PlayerJoinRequest
        {
            PlayerId = playerId,
            GameId = "newGame"
        };

        return new NetworkMessage
        {
            Type = MessageType.PlayerJoinRequest,
            PlayerJoinRequest = joinMsg
        };
    }

    public static NetworkMessage CreatePlayerMoveMessage(string playerId, PlayerColor playerColor, int row, int col)
    {
        var moveMsg = new PlayerMove
        {
            PlayerId = playerId,
            PlayerColor = playerColor,
            TileRow = row,
            TileCol = col
        };

        return new NetworkMessage
        {
            Type = MessageType.PlayerMove,
            PlayerMove = moveMsg
        };
    }
}