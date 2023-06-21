namespace ExpressNET;

public class Request
{
    public RequestMethod Method { get; private set; }
    public string Url { get; private set; }
    
    public Request(string requestString)
    {
        if (string.IsNullOrEmpty(requestString)) return;
        
        var requestParts = requestString.Split(' ');
        Method = requestParts[0].ToUpper() switch
        {
            "GET" => RequestMethod.GET,
            "POST" => RequestMethod.POST,
            "PUT" => RequestMethod.PUT,
            "HEAD" => RequestMethod.HEAD,
            "DELETE" => RequestMethod.DELETE,
            "PATCH" => RequestMethod.PATCH,
            "OPTIONS" => RequestMethod.OPTIONS,
            "CONNECT" => RequestMethod.CONNECT,
            "TRACE" => RequestMethod.TRACE
        };
        Url = requestParts[1];
    } 
}

public enum RequestMethod
{
    GET,
    POST,
    PUT,
    HEAD,
    DELETE,
    PATCH,
    OPTIONS,
    CONNECT,
    TRACE
}