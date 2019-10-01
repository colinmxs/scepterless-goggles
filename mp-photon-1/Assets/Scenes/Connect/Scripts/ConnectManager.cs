using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectManager : MonoBehaviour
{
    public UIController UIController;
    public RealtimeInGamePlayer InGamePlayer;
    private RealtimeClient client;
    private Dictionary<string, RoomInfo> rooms;
    private bool redrawNeeded = false;

    private bool IsInRoom => client.InRoom;

    private bool ConnectPanelIsActive => UIController.ConnectPanel.activeSelf;

    private bool StartCondition => client.CurrentRoom.Players.Values.All(p => ((RealtimePlayer)p).PlayerReady);

    public void LeaveRoom()
    {
        client.OpLeaveRoom(false);
    }

    public void ReadyUp()
    {
        client.ToggleReady();
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

    private void CreateRoom()
    {
        var roomName = UIController.CreateRoomInput.text;
        JoinRoom(roomName);
    }

    private void JoinRoom(string roomName)
    {
        var joined = RealtimeClientStarter.Instance.Client.OpJoinRoom(
            new EnterRoomParams
            {
                CreateIfNotExists = true,
                RoomName = roomName,
                RoomOptions = new Photon.Realtime.RoomOptions
                {
                    MaxPlayers = 4
                }
            });
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
    
    private IEnumerator StartCountdown()
    {
        int timeLeft = 3;
        while (true)
        {
            if (IsInRoom && StartCondition && timeLeft > 0)
            {
                UIController.SetPlayCountdownText("Play starting in " + timeLeft + " seconds...");
                yield return new WaitForSeconds(1);
                timeLeft--;
            }
            else if (IsInRoom && !StartCondition && timeLeft != 3)
            {
                UIController.SetPlayCountdownText("Ready up to play!");
                yield return null;
                timeLeft = 3;
            }

            if (IsInRoom && StartCondition && timeLeft == 0)
            {
                UIController.SetPlayCountdownText("Loading the game!");
                var inGamePlayer = Instantiate(InGamePlayer);
                inGamePlayer.player = (RealtimePlayer)client.LocalPlayer;                
                inGamePlayer.client = client;
                SceneManager.LoadScene("Game");
                yield return null;
            }

            yield return null;
        }
    }

    private void Start()
    {
        StartCoroutine(StartCountdown());
    }

    private void Awake()
    {
        client = RealtimeClientStarter.Instance.Client;
        rooms = client.CachedRoomList;
        client.OnUpdate += () => redrawNeeded = true;
        UIController.CreateRoom += CreateRoom;
        UIController.JoinRoom += JoinRoom;
    }
}