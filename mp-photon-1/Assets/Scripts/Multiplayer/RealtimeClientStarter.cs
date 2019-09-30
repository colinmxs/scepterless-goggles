public class RealtimeClientStarter
{
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

    public RealtimeClient Client { get; set; }    
}