// ReSharper disable InconsistentNaming
namespace ExpressNET.Routes;

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

public enum RequestMethodAllowAll
{
    GET,
    POST,
    PUT,
    HEAD,
    DELETE,
    PATCH,
    OPTIONS,
    CONNECT,
    TRACE,
    All
}

public class MethodConverter
{
    public static RequestMethod Convert(RequestMethodAllowAll requestMethodAllowAll)
    {
        RequestMethod parsed;
        Enum.TryParse(requestMethodAllowAll.ToString(), out parsed);
        return parsed;
    }
}