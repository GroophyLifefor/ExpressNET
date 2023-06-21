namespace ExpressNET;

public class Express
{
    private short? _port = null;
    public short? port
    {
        get
        {
            return _port;
        }
        set
        {
            _port = value;
            _listener.ChangePort(_port);
        }
    }

    private NetworkListener _listener { get; set; }

    public Express()
    {
        _listener = new NetworkListener(port);
    }
    
    public void route(string path, Action<Request, Response> action)
    {
        _listener.addRoute(new Route()
        {
            path = path,
            action = action
        });
    }
    
    public void staticDir(string directory)
    {
        _listener.staticDirectory = $"/{directory.TrimStart('.').TrimStart('/')}";
    }

    public void listen()
    {
        _listener.Listen();
    }
    
    public void listen(Action<short> action)
    {
        _listener.ListenWithAction(action);
    }
}