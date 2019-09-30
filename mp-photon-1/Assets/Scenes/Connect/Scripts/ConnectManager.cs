using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;

public class ConnectManager : MonoBehaviour
{
    public UIController UIController;
    private RealtimeClient client;
    private Dictionary<string, RoomInfo> rooms;
    private bool redrawNeeded = false;

    private bool IsInRoom => client.InRoom;

    private bool ConnectPanelIsActive => UIController.ConnectPanel.activeSelf;

    public void LeaveRoom()
    {
        client.OpLeaveRoom(false);
    }

    public void ReadyUp()
    {
        client.ToggleReady();
    }

    private void Awake()
    {
        client = RealtimeClientStarter.Instance.Client;
        rooms = client.CachedRoomList;
        client.OnUpdate += () => redrawNeeded = true;
        UIController.CreateRoom += CreateRoom;
        UIController.JoinRoom += JoinRoom;
    }

    private void CreateRoom()
    {
        var roomName = UIController.CreateRoomInput.text;
        JoinRoom(roomName);
    }

    private void JoinRoom(string roomName)
    {
        var joined = RealtimeClientStarter.Instance.Client.OpJoinRoom(
            new Photon.Realtime.EnterRoomParams
            {
                CreateIfNotExists = true,
                RoomName = roomName,
                RoomOptions = new Photon.Realtime.RoomOptions
                {
                    MaxPlayers = 4
                }
            });
    }

    private void Update()
    {
        if (IsInRoom && (ConnectPanelIsActive || redrawNeeded))
        {
            UIController.ActivateRoomPanel(client.CurrentRoom.Name);
            var playerNames = GetPlayerNames();
            UIController.SetPlayerNames(playerNames);
        }
        else if (!IsInRoom && (!ConnectPanelIsActive || redrawNeeded))
        {
            UIController.ActivateConnectPanel();
            UIController.ClearRoomButtonText();
            foreach (var room in rooms)
            {
                UIController.AddRoomButton(room.Value.Name, room.Value.PlayerCount, room.Value.MaxPlayers);
            }
        }
    }

    private string[] GetPlayerNames()
    {
        var players = RealtimeClientStarter.Instance.Client.CurrentRoom.Players;
        var playerNames = players.Values.Select(player => 
        {
            var name = player.NickName;
            if (((RealtimePlayer)player).PlayerReady)
            {
                name += " (ready)";
            }

            return name;
        });
        return playerNames.ToArray();
    }
}