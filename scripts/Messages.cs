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
            PlayerId = playerId
        };

        return new NetworkMessage
        {
            Type = MessageType.PlayerJoinRequest,
            PlayerJoinRequest = joinMsg
        };
    }

    public static NetworkMessage CreatePlayerMoveMessage(string playerId, PlayerColor playerColor, Vector2I tileCoords)
    {
        var moveMsg = new PlayerMove
        {
            PlayerId = playerId,
            PlayerColor = playerColor,
            TileX = (uint)tileCoords.X,
            TileY = (uint)tileCoords.Y
        };

        return new NetworkMessage
        {
            Type = MessageType.PlayerJoinRequest,
            PlayerMove = moveMsg
        };
    }
}