using System;
using System.Collections;    
using Photon;
using global::Photon.Realtime;
using ExitGames.Client.Photon;

public class RealtimePlayer : Player
{
    public float xPosition;
    public float yPosition;
    public static bool isSendReliable;

    public enum EventKey : byte { PlayerPositionX = 0, PlayerPositionY = 1, PlayerName = 2 }

    public enum EventCode : byte { PlayerInfo = 0, PlayerMove = 1 }
        
    public RealtimePlayer(string nickName, int actorNr, bool isLocal) : base(nickName, actorNr, isLocal)
    {
        // actorNumbers in-game start with 1. any local creation of players gets randomized here
        if (actorNr < 1)
        {
            this.xPosition = default;
            this.yPosition = default;
        }
    }

    internal void SendPlayerInfo(LoadBalancingPeer peer)
    {
        if (peer == null)
        {
            return;
        }

        // Setting up the content of the event. Here we want to send a player's info: nickName and color.
        Hashtable evInfo = new Hashtable();
        evInfo.Add((byte)EventKey.PlayerName, this.NickName);

        // The event's code must be of type byte, so we have to cast it. We do this above as well, to get routine ;)
        peer.OpRaiseEvent((byte)EventCode.PlayerInfo, evInfo, null, SendOptions.SendReliable);
    }    

    internal void SendPlayerLocation(LoadBalancingPeer peer)
    {
        if (peer == null)
        {
            return;
        }

        Hashtable eventContent = new Hashtable();
        eventContent.Add((byte)EventKey.PlayerPositionX, (byte)this.xPosition);
        eventContent.Add((byte)EventKey.PlayerPositionY, (byte)this.yPosition);

        peer.OpRaiseEvent((byte)EventCode.PlayerMove, eventContent, new RaiseEventOptions {Receivers = ReceiverGroup.All}, new SendOptions() { DeliveryMode = DeliveryMode.UnreliableUnsequenced });
    }

    internal void SetInfo(Hashtable customEventContent)
    {
        this.NickName = (string)customEventContent[(byte)EventKey.PlayerName];
    }

    internal void SetPosition(Hashtable evData)
    {
        this.xPosition = (byte)evData[(byte)EventKey.PlayerPositionX];
        this.yPosition = (byte)evData[(byte)EventKey.PlayerPositionY];
    }

    public override string ToString()
    {
        return this.ActorNumber + "'" + this.NickName + "':" + this.xPosition + ":" + this.yPosition + " PlayerProps: " + SupportClass.DictionaryToString(this.CustomProperties);
    }
}