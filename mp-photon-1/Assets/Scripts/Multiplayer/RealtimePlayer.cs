using System.Collections;    
using ExitGames.Client.Photon;
using global::Photon.Realtime;

public class RealtimePlayer : Player
{
    public static bool IsSendReliable;
    public float XPosition;
    public float YPosition;

    public RealtimePlayer(string nickName, int actorNr, bool isLocal) : base(nickName, actorNr, isLocal)
    {
        // actorNumbers in-game start with 1. any local creation of players gets randomized here
        if (actorNr < 1)
        {
            this.XPosition = default;
            this.YPosition = default;
        }
    }

    public enum EventKey : byte
    {
        PlayerPositionX = 0,
        PlayerPositionY = 1,
        PlayerName = 2,
        PlayerReady = 3
    }

    public enum EventCode : byte
    {
        PlayerInfo = 0,
        PlayerMove = 1
    }

    public bool PlayerReady { get; private set; }
    
    public override string ToString()
    {
        return this.ActorNumber + "'" + this.NickName + "':" + this.XPosition + ":" + this.YPosition + " PlayerProps: " + SupportClass.DictionaryToString(this.CustomProperties);
    }

    internal void SendPlayerInfo(LoadBalancingPeer peer)
    {
        if (peer == null)
        {
            return;
        }

        // Setting up the content of the event. Here we want to send a player's info: nickName and color.
        Hashtable eventInfo = new Hashtable();
        eventInfo.Add((byte)EventKey.PlayerName, this.NickName);
        eventInfo.Add((byte)EventKey.PlayerReady, this.PlayerReady);

        // The event's code must be of type byte, so we have to cast it. We do this above as well, to get routine ;)
        peer.OpRaiseEvent((byte)EventCode.PlayerInfo, eventInfo, null, SendOptions.SendReliable);
    }

    internal void SendPlayerLocation(LoadBalancingPeer peer)
    {
        if (peer == null)
        {
            return;
        }

        Hashtable eventContent = new Hashtable();
        eventContent.Add((byte)EventKey.PlayerPositionX, (byte)this.XPosition);
        eventContent.Add((byte)EventKey.PlayerPositionY, (byte)this.YPosition);

        peer.OpRaiseEvent((byte)EventCode.PlayerMove, eventContent, new RaiseEventOptions { Receivers = ReceiverGroup.All }, new SendOptions() { DeliveryMode = DeliveryMode.UnreliableUnsequenced });
    }
    
    internal void SetInfo(Hashtable customEventContent)
    {
        this.NickName = (string)customEventContent[(byte)EventKey.PlayerName];
        this.PlayerReady = (bool)customEventContent[(byte)EventKey.PlayerReady];
    }

    internal void SetPosition(Hashtable eventData)
    {
        this.XPosition = (byte)eventData[(byte)EventKey.PlayerPositionX];
        this.YPosition = (byte)eventData[(byte)EventKey.PlayerPositionY];
    }

    internal void ToggleReady()
    {
        PlayerReady = !PlayerReady;
    }
}