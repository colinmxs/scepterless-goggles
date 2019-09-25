public class RealtimeClientStarter
{
    public RealtimeClient Client { get; set; }

    private static RealtimeClientStarter instance; 

    public RealtimeClientStarter()
    {
        Client = new RealtimeClient(true);
    }

    public static RealtimeClientStarter Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = new RealtimeClientStarter();
            }
            return instance;
        }
    }
}