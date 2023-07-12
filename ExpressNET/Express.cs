// ReSharper disable InconsistentNaming

using ExpressNET.Routes;
using ExpressNET.Server;

namespace ExpressNET;

public class Express
{
    private RouteHandler _routeHandler = new();
    private NetworkListener _networkListener;

    public Express()
    {
        _networkListener = new (_routeHandler);
    }

    public Express(int port)
    {
        _networkListener = new (_routeHandler, port);
    }

    #region get
    /// <summary>
    /// Routes HTTP GET requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route get(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.GET, action);
    
    /// <summary>
    /// Routes HTTP GET requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route get(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.GET, action);
    
    /// <summary>
    /// Routes HTTP GET requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route get(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.GET, action);
    #endregion
    #region post
    /// <summary>
    /// Routes HTTP POST requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route post(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.POST, action);
    
    /// <summary>
    /// Routes HTTP POST requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route post(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.POST, action);
    
    /// <summary>
    /// Routes HTTP POST requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route post(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.POST, action);
    #endregion
    #region delete
    /// <summary>
    /// Routes HTTP DELETE requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route delete(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.DELETE, action);
    
    /// <summary>
    /// Routes HTTP DELETE requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route delete(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.DELETE, action);
    
    /// <summary>
    /// Routes HTTP DELETE requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route delete(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.DELETE, action);
    #endregion
    #region all
    /// <summary>
    /// Routes ALL HTTP requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route all(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.All, action);
    
    /// <summary>
    /// Routes ALL HTTP requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route all(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.All, action);
    
    /// <summary>
    /// Routes ALL HTTP requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route all(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.All, action);
    #endregion
    #region head
    /// <summary>
    /// Routes HTTP HEAD requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route head(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.HEAD, action);
    
    /// <summary>
    /// Routes HTTP HEAD requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route head(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.HEAD, action);
    
    /// <summary>
    /// Routes HTTP HEAD requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route head(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.HEAD, action);
    #endregion
    #region put
    /// <summary>
    /// Routes HTTP PUT requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route put(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.PUT, action);
    
    /// <summary>
    /// Routes HTTP PUT requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route put(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.PUT, action);
    
    /// <summary>
    /// Routes HTTP PUT requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route put(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.PUT, action);
    #endregion
    #region patch
    /// <summary>
    /// Routes HTTP PATCH requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route patch(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.PATCH, action);
    
    /// <summary>
    /// Routes HTTP PATCH requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route patch(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.PATCH, action);
    
    /// <summary>
    /// Routes HTTP PATCH requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route patch(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.PATCH, action);
    #endregion
    #region trace
    /// <summary>
    /// Routes HTTP TRACE requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route trace(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.TRACE, action);
    
    /// <summary>
    /// Routes HTTP TRACE requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route trace(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.TRACE, action);
    
    /// <summary>
    /// Routes HTTP TRACE requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route trace(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.TRACE, action);
    #endregion
    #region connect
    /// <summary>
    /// Routes HTTP CONNECT requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route connect(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.CONNECT, action);
    
    /// <summary>
    /// Routes HTTP CONNECT requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route connect(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.CONNECT, action);
    
    /// <summary>
    /// Routes HTTP CONNECT requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route connect(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.CONNECT, action);
    #endregion
    #region options
    /// <summary>
    /// Routes HTTP OPTIONS requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route options(string path, Action<Request, ExpressNET.Response> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.OPTIONS, action);
    
    /// <summary>
    /// Routes HTTP OPTIONS requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route options(string path, Action<Request, ExpressNET.Response, Action> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.OPTIONS, action);
    
    /// <summary>
    /// Routes HTTP OPTIONS requests to the specified path with the specified callback functions.
    /// </summary>
    /// <param name="path">The path for which the middleware function is invoked. Default was "/" (root path)</param>
    /// <param name="action">Callback functions</param>
    /// <returns>Route class which you configure deep options.</returns>
    public Route options(string path, Action<Request, ExpressNET.Response, Action, int> action)
        => _routeHandler.AddRoute(path, RequestMethodAllowAll.OPTIONS, action);
    #endregion
    
    /// <summary>
    /// Binds and listens for connections on the specified host and port. This method is identical to System.Net.Sockets's TcpListener.
    /// </summary>
    /// <param name="port">Listening port of server</param>
    public void listen(int port)
    {
        _networkListener.addListen(port);
        _networkListener.Listen("*", null);
    }

    /// <summary>
    /// Binds and listens for connections on the specified host and port. This method is identical to System.Net.Sockets's TcpListener.
    /// </summary>
    /// <param name="port">Listening port of server</param>
    /// <param name="path">Listening path of server</param>
    public void listen(int port, string path)
    {
        _networkListener.addListen(port);
        _networkListener.Listen(path, null);
    }
    
    /// <summary>
    /// Binds and listens for connections on the specified host and port. This method is identical to System.Net.Sockets's TcpListener.
    /// </summary>
    /// <param name="port">Listening port of server</param>
    /// <param name="path">Listening path of server</param>
    public void listen(string path, int port)
    {
        _networkListener.addListen(port);
        _networkListener.Listen(path, null);
    }
    
    /// <summary>
    /// Binds and listens for connections on the specified host and port. This method is identical to System.Net.Sockets's TcpListener.
    /// </summary>
    /// <param name="port">Listening port of server</param>
    /// <param name="action">Callback function when server was running</param>
    public void listen(int port, Action<int>? action)
    {
        _networkListener.addListen(port);
        _networkListener.Listen("*", action);
    }
    
    /// <summary>
    /// Binds and listens for connections on the specified host and port. This method is identical to System.Net.Sockets's TcpListener.
    /// </summary>
    /// <param name="port">Listening port of server</param>
    /// <param name="path">Listening path of server</param>
    /// <param name="action">Callback function when server was running</param>
    public void listen(int port, string path, Action<int> action)
    {
        _networkListener.addListen(port);
        _networkListener.Listen(path, action);
    }

    /// <summary>
    /// This is a built-in middleware function in Express. It serves static files and is based on serve-static.
    /// </summary>
    /// <param name="root">specifies the root directory from which to serve static assets.</param>
    /// <returns>Middleware function for serve static assets like css files.</returns>
    public static Action<Request, ExpressNET.Response, Action> Static(string root)
        => Static(root, new staticOptions());

    /// <summary>
    /// This is a built-in middleware function in Express. It serves static files and is based on serve-static.
    /// </summary>
    /// <param name="root">specifies the root directory from which to serve static assets.</param>
    /// <param name="options">Serve options of middleware.</param>
    /// <returns>Middleware function for serve static assets like css files.</returns>
    public static Action<Request, Response, Action> Static(string root, staticOptions options)
    {
        return (request, response, next) =>
        {
            if (request.Url == "/" || request.Url is null)
            {
                next();
                return;
            }

            string currentDir = Directory.GetCurrentDirectory();
            string rootPath = currentDir + FixSlash(root);
            string filePath = rootPath + FixSlash(request.Url);
            string extension = filePath.Split('.').Last();

            if (filePath.IndexOf('/') != -1)
                filePath = filePath.Replace("/", "\\");

            // check validate extensions option
            if (options.extentions.Count != 0)
            {
                if (!options.extentions.Any(x => x.TrimStart('.') == extension))
                {
                    next();
                    return;
                }
            }

            if (File.Exists(filePath))
                response.sendFile(filePath);
            else
                next();
        };
    }

    private static string FixSlash(string value)
        => $"\\{value.TrimStart('/', '\\')}";

    /// <summary>
    /// Mounts the specified middleware function or functions at the specified path: the middleware function is executed when the base of the requested path matches path
    /// </summary>
    /// <param name="middleware">Middleware function</param>
    public void use(Action<Request, ExpressNET.Response, Action> middleware)
        => all("*", middleware);

    /// <summary>
    /// Creates a while loop and end console when pressing Q key.
    /// </summary>
    public void DoNotEndMyConsole()
    { 
        Console.WriteLine("Press Q for exit.");
        do ; while (Console.ReadKey().Key != ConsoleKey.Q);
    }
}