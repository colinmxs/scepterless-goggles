using UnityEngine;

public class RealtimeInGamePlayer : MonoBehaviour
{
    public RealtimePlayer player;
    public RealtimeClient client;
    public bool IsLoaded;
    public bool IsReady;

    public void SendPlayerLocation(float x, float y)
    {
        player.XPosition = x;
        player.YPosition = y;
        player.SendPlayerLocation(client.LoadBalancingPeer);
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        IsLoaded = false;
        IsReady = false;
    }
}
