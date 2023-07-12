namespace ExpressNET.Routes;

public class RouteHandler
{
    private List<Route> Routes { get; } = new();

    public Route AddRoute(string path, RequestMethodAllowAll requestMethod, Action<Request, Response> action)
    {
        var r = new Route()
        {
            path = path,
            requestMethod = requestMethod,
            action = (request, response, next, _) =>
            {
                action(request, response);
                next();
            }
        };
        Routes.Add(r);
        return r;
    }
    
    public Route AddRoute(string path, RequestMethodAllowAll requestMethod, Action<Request, Response, Action> action)
    {
        var r = new Route()
        {
            path = path,
            requestMethod = requestMethod,
            action = (request, response, next, _) =>
            {
                action(request, response, next);
            }
        };
        Routes.Add(r);
        return r;
    }
    
    public Route AddRoute(string path, RequestMethodAllowAll requestMethod, Action<Request, Response, Action, int> action)
    {
        var r = new Route()
        {
            path = path,
            requestMethod = requestMethod,
            action = action
        };
        Routes.Add(r);
        return r;
    }

    public bool isPathMatch(string url, string path)
    {
        if (path == "*") return true;

        var parts = path.Split('/');
        var urlParts = url.Split('/');
        
        if (parts.Length != urlParts.Length) return false;
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].StartsWith(':')) continue;
            if (parts[i] != urlParts[i]) return false;
        }

        return true;
    }

    public Task Route(Request request, Response response)
    {
        // first call property
        List<int> called = new();
        for (int i = 0; i < Routes.Count; i++)
        {
            if (!Routes[i].firstCall) continue;
            bool isEnd;
            RouteAction(request, response, Routes[i], i, out isEnd);
            if (isEnd) return Task.CompletedTask;
            called.Add(i);
        }

        for (int i = 0; i < Routes.Count; i++)
        {
            if (called.Any(x => x == i)) continue;

            bool isEnd;
            RouteAction(request, response, Routes[i], i, out isEnd);
            if (isEnd) return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    private void RouteAction(Request request, Response response, Route route, int i, out bool isEnd)
    {
        if (route.requestMethod != RequestMethodAllowAll.All)
        {
            if (MethodConverter.Convert(route.requestMethod) != request.Method)
            {
                isEnd = false;
                return;
            }
        }
            
        if (isPathMatch(request.Url, route.path))
        {
            bool _ = true;
            Action next = () => _ = false;
            request.SetParams(route.path);
            route.action(request, response, next, i);
            if (_)
            {
                isEnd = true;
                return;
            }
        }

        isEnd = false;
    }
}