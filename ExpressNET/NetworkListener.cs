using System.Net;
using System.Net.Sockets;

namespace ExpressNET;

internal class NetworkListener
{
    private short? port { get; set; } = null;
    private JobFixer listenFixer { get; set; } = new JobFixer();
    private List<Route> routes { get; set; } = new List<Route>();
    internal string staticDirectory { get; set; } = null;

    public NetworkListener(short? port)
    {
        this.port = port;
    }

    public void Listen()
    {
        ExpectionHelper.ThrowIfNull(port, "Port not defined.");

        TcpListener listener = new TcpListener(IPAddress.Any, port!.Value);
        listener.Start();
        listenFixer.SetState(State.InProgress);
        while (true)
        {
            if (listenFixer.isJobFixed) break;
            TcpClient client = listener.AcceptTcpClient();
            HandleClient(client);
            client.Close();
        }

        listener.Stop();
        listenFixer.SetState(State.Done);
    }

    public void ListenWithAction(Action<short> action)
    {
        ExpectionHelper.ThrowIfNull(port, "Port not defined.");

        TcpListener listener = new TcpListener(IPAddress.Any, port!.Value);
        listener.Start();
        listenFixer.SetState(State.InProgress);
        action(port!.Value);
        while (true)
        {
            if (listenFixer.isJobFixed) break;
            TcpClient client = listener.AcceptTcpClient();
            HandleClient(client);
        }

        listener.Stop();
        listenFixer.SetState(State.Done);
    }

    public void StopListening() => listenFixer.FixJob();

    public void ChangePort(short? port)
    {
        switch (listenFixer.state)
        {
            // Change port if already server not up.
            case State.NotStarted:
            case State.Done:
                this.port = port;
                break;
            // Firstly shutdown server then change port and re-up server.
            case State.InProgress:
                StopListening();
                this.port = port;
                Listen();
                break;
        }
    }

    internal void addRoute(Route route) => routes.Add(route);

    private void HandleClient(TcpClient client)
    {
        NetworkStream networkStream = client.GetStream();
        StreamReader reader = new StreamReader(networkStream);

        var requestString = reader.ReadLine();
        Console.WriteLine(requestString);
        Request request = new Request(requestString);
        Response response = new Response(client);

        if (string.IsNullOrEmpty(requestString))
        {
            client.Close();
            return;
        }

        bool found = false;
        for (int i = 0; i < routes.Count; i++)
        {
            if (routes[i].path == request.Url)
            {
                routes[i].action(request, response);
                found = true;
            }
        }
        
        if (found) return;

        if (string.IsNullOrEmpty(staticDirectory))
        {
            client.Close();
            return;
        }

        string pathInDriver = $"{Directory.GetCurrentDirectory()}{staticDirectory}{request.Url}";

        if (request.Url.IndexOf("favicon") != -1 && File.Exists(pathInDriver))
        {
            response.setHeader("Cache-Control", "public, max-age=0");
            response.setHeader("Connection", "keep-alive");
            response.setHeader("Access-Control-Allow-Origin", "*");
            response.sendBinary(pathInDriver);
            
        } else 
        
        if (File.Exists(pathInDriver))
        {
            response.sendFile(pathInDriver);
        }

        client.Close();
    }
}