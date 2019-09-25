using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    RealtimeClient client;
    // Start is called before the first frame update
    void Start()
    {        
        client = RealtimeClientStarter.Instance.Client;        
    }

    public void OnClick()
    {        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
