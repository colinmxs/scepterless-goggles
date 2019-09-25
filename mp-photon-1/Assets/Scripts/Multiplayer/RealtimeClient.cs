using System;
using System.Collections;
using System.Threading;
using global::Photon.Realtime;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class RealtimeClient : LoadBalancingClient, IConnectionCallbacks, ILobbyCallbacks, IInRoomCallbacks, IMatchmakingCallbacks
{
    internal int DispatchInterval = 10;                 
    internal int LastDispatch = Environment.TickCount;
    internal int SendInterval = 50;                     
    internal int LastSend = Environment.TickCount;
    internal int MoveInterval = 0;                    
    internal int LastMove = Environment.TickCount;
    private int UIUpdateInterval = 1000;
    internal int LastUIUpdate = Environment.TickCount;
    private readonly Thread UpdateThread;

    public Action OnUpdate { get; set; }

    public Action OnCachedRoomListUpdate { get; set; }

    public int ReceivedCountMeEvents { get; set; }

    public Dictionary<string, RoomInfo> CachedRoomList { get; set; }

    public RealtimeClient(bool createGameLoopThread) : base(ConnectionProtocol.Udp)
    {
        CachedRoomList = new Dictionary<string, RoomInfo>();

        //this.loadBalancingPeer.DebugOut = DebugLevel.INFO;
        //this.loadBalancingPeer.TrafficStatsEnabled = true;
        if (createGameLoopThread)
        {
            this.UpdateThread = new Thread(this.UpdateLoop);
            this.UpdateThread.IsBackground = true;
            this.UpdateThread.Start();
        }

        this.NickName = "Player_" + (SupportClass.ThreadSafeRandom.Next() % 1000);
        this.LocalPlayer.SetCustomProperties(new Hashtable() { { "class", "tank" + (SupportClass.ThreadSafeRandom.Next() % 99) } });

        this.AppId = "b6956e7b-6680-4455-ac70-69ed382bfc7c";
        this.NameServerHost = "ns.exitgames.com";
        this.AppVersion = "1.0";

        this.AddCallbackTarget(this);

        bool couldConnect = false;
        if (!string.IsNullOrEmpty(this.MasterServerAddress))
        {
            couldConnect = this.Connect();
        }
        else
        {
            couldConnect = this.ConnectToRegionMaster("usw");
        }
        if (!couldConnect)
        {
            this.DebugReturn(DebugLevel.ERROR, "Can't connect to: " + this.CurrentServerAddress);
        }
    }

    public override void OnStatusChanged(StatusCode statusCode)
    {
        base.OnStatusChanged(statusCode);

        if (statusCode == StatusCode.Disconnect)
        {
            this.ReceivedCountMeEvents = 0;
        }

        if (statusCode == StatusCode.Disconnect && this.DisconnectedCause != DisconnectCause.None)
        {
            DebugReturn(DebugLevel.ERROR, this.DisconnectedCause + " caused a disconnect. State: " + this.State + " statusCode: " + statusCode + ".");
        }

        if (this.OnUpdate != null)
        {
            this.OnUpdate();
        }
    }

    public override void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)RealtimePlayer.EventCode.PlayerInfo:

                var actorNr = (int)photonEvent.Sender;
                var player = (RealtimePlayer)this.CurrentRoom.GetPlayer(actorNr);

                if (player != null)
                {
                    player.SetInfo((Hashtable)photonEvent.CustomData);
                }
                else
                {
                    Console.Out.WriteLine("did not find player to set info: " + actorNr);
                }

                break;

            case (byte)RealtimePlayer.EventCode.PlayerMove:

                actorNr = (int)photonEvent.Sender;
                player = (RealtimePlayer)this.CurrentRoom.GetPlayer(actorNr);

                if (player != null)
                {
                    player.SetPosition((Hashtable)photonEvent.CustomData);
                }
                else
                {
                    Console.Out.WriteLine("did not find player to move: " + actorNr);
                }

                break;

            case EventCode.Join:

                ((RealtimePlayer)LocalPlayer).SendPlayerInfo(this.LoadBalancingPeer);
                break;
        }

        base.OnEvent(photonEvent);
        if (this.OnUpdate != null)
        {
            this.OnUpdate();
        }
    }

    protected override Player CreatePlayer(string actorName, int actorNumber, bool isLocal, Hashtable actorProperties)
    {
        RealtimePlayer tmpPlayer = null;
        if (this.CurrentRoom != null)
        {
            tmpPlayer = (RealtimePlayer)this.CurrentRoom.GetPlayer(actorNumber);
        }

        if (tmpPlayer == null)
        {
            tmpPlayer = new RealtimePlayer(actorName, actorNumber, isLocal);
            tmpPlayer.InternalCacheProperties(actorProperties);

            if (this.CurrentRoom != null)
            {
                this.CurrentRoom.StorePlayer(tmpPlayer);
            }
        }
        else
        {
            this.DebugReturn(DebugLevel.ERROR, "Player already listed: " + actorNumber);
        }

        return tmpPlayer;
    }

    private void SendPosition()
    {
        // dont move if player does not have a number or peer is not connected
        if (this.LocalPlayer == null || this.LocalPlayer.ActorNumber == 0)
        {
            return;
        }

        ((RealtimePlayer)this.LocalPlayer).SendPlayerLocation(this.LoadBalancingPeer);
    }

    private void SendPlayerInfo()
    {
        if (this.LocalPlayer == null || this.LocalPlayer.ActorNumber == 0)
        {
            return;
        }

        ((RealtimePlayer)this.LocalPlayer).SendPlayerInfo(this.LoadBalancingPeer);
    }
	
    private void UpdateLoop()
    {
        while (true)
        {
            this.Update();
            Thread.Sleep(10);
        }
    }

    private void Update()
    {
        if (Environment.TickCount - this.LastDispatch > this.DispatchInterval)
        {
            this.LastDispatch = Environment.TickCount;
            this.LoadBalancingPeer.DispatchIncomingCommands();
        }

        if (Environment.TickCount - this.LastSend > this.SendInterval)
        {
            this.LastSend = Environment.TickCount;
            this.LoadBalancingPeer.SendOutgoingCommands(); // will send pending, outgoing commands
        }

        if (this.MoveInterval != 0 && Environment.TickCount - this.LastMove > this.MoveInterval)
        {
            this.LastMove = Environment.TickCount;
            if (this.State == ClientState.Joined)
            {
                this.SendPosition();
            }
        }

        // Update call for windows phone UI-Thread
        if (Environment.TickCount - this.LastUIUpdate > this.UIUpdateInterval)
        {
            this.LastUIUpdate = Environment.TickCount;
            if (this.OnUpdate != null)
            {
                this.OnUpdate();
            }
        }
    }

    #region IConnectionCallbacks implementation

    public void OnConnected()
    {
    }

    public void OnConnectedToMaster()
    {
        this.OpJoinLobby(null);
    }

    public void OnDisconnected(DisconnectCause cause)
    {
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    #endregion

    #region ILobbyCallbacks implmentation

    public void OnJoinedLobby()
    {
    }

    public void OnLeftLobby()
    {
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (CachedRoomList.ContainsKey(info.Name))
                {
                    CachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (CachedRoomList.ContainsKey(info.Name))
            {
                CachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                CachedRoomList.Add(info.Name, info);
            }
        }

        OnCachedRoomListUpdate();
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
    }

    #endregion

    #region IInRoomCallbacks implementation

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
    }

    #endregion

    #region IMatchmakingCallbacks implementation

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnCreatedRoom()
    {
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public void OnJoinedRoom()
    {
        CachedRoomList.Clear();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
    }

    public void OnLeftRoom()
    {
    }

    #endregion
}