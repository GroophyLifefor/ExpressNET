using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using ExpressNET.Routes;

namespace ExpressNET.Server;

internal class NetworkListener
{
    private JobFixer listenFixer { get; set; } = new JobFixer();
    private Action<short> _listenAction = null;
    private List<(TcpListener listener, int port)> listenerList = new();
    private Minimalizer _minimalizer = new();
    private RouteHandler _routeHandler;

    public NetworkListener(RouteHandler routeHandler)
        => _routeHandler = routeHandler;

    public NetworkListener(RouteHandler routeHandler, int port)
    {
        _routeHandler = routeHandler;
        addListen(port);
    }
    public void addListen(int port)
    {
        listenerList.Add((new TcpListener(IPAddress.Any, port), port));
    }

    public async void Listen(string path, Action<int>? action)
    {
        if (listenerList.Count == 0)
            return;
        
        // if path was null or empty path will be "*". That means listen everything
        path = string.IsNullOrEmpty(path) ? "*" : path;
        
        listenFixer.SetState(State.InProgress);
        
        for (int i = 0; i < listenerList.Count; i++)
        {
            var listener = listenerList[i].listener;
            var port = listenerList[i].port;
            
            // call before action
            action?.Invoke(port);

            listener.Start();

            await Task.Run(function: async () =>
            {
                while (true)
                {
                    // If server was trying to stop.
                    if (listenFixer.isJobFixed) break;

                    // Accept to connect request
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    // handle client request and response
                    await HandleClient(path, client);

                    // closes client if not closed.
                    if (GetState(client) != TcpState.Closed)
                        client.Close();
                }
                
                listener.Stop();
                listenFixer.SetState(State.Done);
            });
        }
        
    }
    
    private async Task HandleClient(string path, TcpClient client)
    {
        NetworkStream networkStream = client.GetStream();
        
        // Get requestString from stream.
        byte[] buffer = new byte[client.ReceiveBufferSize];
        int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
        string requestString = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
        
        // Create initial request and response
        Request request = new Request(requestString); 
        Response response = new Response(request, client, _minimalizer);
        
        if (!request.isValidClient) return; 

        if (!WildCardRegex(path, request.Url)) return;

        await _routeHandler.Route(request, response);

        client.Close();
    }
    
    private TcpState GetState(TcpClient tcpClient)
    {
        var foo = IPGlobalProperties.GetIPGlobalProperties()
            .GetActiveTcpConnections()
            .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
        return foo != null ? foo.State : TcpState.Unknown;
    }
    
    private static bool WildCardRegex(string match ,string? value) {
        return match is "*" ? true : match is null ? false : value is null ? false : Regex.IsMatch(match, "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$"); 
    }

    public void StopListening() => listenFixer.FixJob();


}