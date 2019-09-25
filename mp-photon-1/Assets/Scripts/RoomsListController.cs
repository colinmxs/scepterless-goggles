using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RoomsListController : MonoBehaviour
{
    private bool updateNeeded = false;
    public GameObject RoomButtonPrefab;
    //private Thread updateThread;
    Dictionary<string, RoomInfo> Rooms;
    List<Button> RoomButtons = new List<Button>();


    private void Awake()
    {
        for (int i = 0; i < 20; i++)
        {
            var button = Instantiate(RoomButtonPrefab, transform).GetComponent<Button>();            
            RoomButtons.Add(button);
        }        
    }

    // Start is called before the first frame update
    void Start()
    {
        RealtimeClientStarter.Instance.Client.OnCachedRoomListUpdate += UpdateRoomsList;
        Rooms = RealtimeClientStarter.Instance.Client.CachedRoomList;
    }

    // Update is called once per frame
    void Update()
    {
        if (updateNeeded)
        {
            foreach (var button in RoomButtons)
            {
                button.interactable = false;
                button.onClick = null;
                var text = button.GetComponentInChildren<Text>();
                text.text = "";
            }

            var i = 0;

            foreach (var room in Rooms.Take(20))
            {
                var roomButton = RoomButtons[i];
                roomButton.interactable = true;
                roomButton.onClick.AddListener(() => RealtimeClientStarter.Instance.Client.OpJoinRoom(new EnterRoomParams { RoomName = room.Key }));
                var text = roomButton.GetComponentInChildren<Text>();
                text.text = room.Value.Name;
                i++;
            }
        }
        updateNeeded = false;
    }

    
    private void UpdateRoomsList()
    {
        Debug.Log("UpdateRoomsList()");
        updateNeeded = true;
    }
}
