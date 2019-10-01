using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject ConnectPanel;
    public GameObject RoomPanel;
    public GameObject RoomButtonPrefab;
    public GameObject RoomButtonContent;
    public Button CreateRoomButton;
    public InputField CreateRoomInput;
    public UnityAction CreateRoom;
    public UnityAction<string> JoinRoom;

    private readonly List<Button> roomButtons = new List<Button>();

    private Text playerNamesText;
    private Text roomNameText;
    private Text playCountdownText;

    public IEnumerable<Button> RoomButtons => roomButtons;

    public void ActivateRoomPanel(string roomName)
    {
        ConnectPanel.SetActive(false);
        RoomPanel.SetActive(true);
        SetRoomName(roomName);
    }

    public void ActivateConnectPanel()
    {
        ConnectPanel.SetActive(true);
        RoomPanel.SetActive(false);
    }
    
    public void SetRoomName(string roomName)
    {
        roomNameText.text = "Room Name: " + roomName;
    }

    public void SetPlayerNames(string[] playerNames)
    {
        var playerTextValue = "Players: ";
        var delimeter = ",  ";

        for (int i = 0; i < playerNames.Length; i++)
        {
            playerTextValue += playerNames[i];
            playerTextValue += delimeter;
        }

        if (playerNames.Length > 0)
        {
            playerTextValue = playerTextValue.Remove(playerTextValue.Length - delimeter.Length);
        }

        playerNamesText.text = playerTextValue;
    }

    public void AddRoomButton(string roomName, int playerCount, int maxPlayerCount)
    {
        var roomButton = roomButtons.FirstOrDefault(rb => !rb.interactable);
        if (roomButton != null)
        {
            roomButton.interactable = true;
            roomButton.onClick.AddListener(() =>
            {
                JoinRoom(roomName);
            });
            var text = roomButton.GetComponentInChildren<Text>();
            text.text = roomName + " | Players: " + playerCount + "/" + maxPlayerCount;
        }
    }

    internal void ClearRoomButtonText()
    {
        foreach (var button in RoomButtons)
        {
            button.interactable = false;
            button.onClick.RemoveAllListeners();
            var text = button.GetComponentInChildren<Text>();
            text.text = string.Empty;
        }
    }

    internal void SetPlayCountdownText(string text)
    {
        playCountdownText.text = text;
    }

    private void Awake()
    {
        for (int i = 0; i < 20; i++)
        {
            var button = Instantiate(RoomButtonPrefab, transform).GetComponent<Button>();
            button.transform.SetParent(RoomButtonContent.transform);
            roomButtons.Add(button);
        }
    }

    private void Start()
    {
        var textChillens = RoomPanel.GetComponentsInChildren<Text>();
        playerNamesText = textChillens[1];
        roomNameText = textChillens[0];
        playCountdownText = textChillens[2];
        CreateRoomButton.onClick.AddListener(CreateRoom);
    }
}
