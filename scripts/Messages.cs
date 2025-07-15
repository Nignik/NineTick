using Godot;
using System;
using Godot.Collections;
using Google.Protobuf;

public static class MessageFactory
{
    public static NetworkMessage CreatePlayerJoinRequestMessage(string playerId)
    {
        var joinReq = new PlayerJoinRequest { Id = playerId };
        var request = new NetworkMessage
        {
            Type = MessageType.PlayerJoinRequest,
            PlayerJoinRequest = joinReq
        }; 

        return request;
    }
}