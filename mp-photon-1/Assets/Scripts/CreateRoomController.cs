using UnityEngine;
using UnityEngine.UI;

public class CreateRoomController : MonoBehaviour
{
    public Button CreateRoomButton;
    public InputField CreateRoomInput;

    // Start is called before the first frame update
    void Start()
    {
        CreateRoomButton.onClick.AddListener(() => 
        {
            var roomName = CreateRoomInput.text;
            var joined = RealtimeClientStarter.Instance.Client.OpJoinRoom(
                new Photon.Realtime.EnterRoomParams
                {
                    CreateIfNotExists = true,
                    RoomName = roomName
                });
        });
    }

    // Update is called once per frame
    void Update()
    {
    }
}
