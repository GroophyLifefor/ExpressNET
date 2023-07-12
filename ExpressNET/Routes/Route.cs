namespace ExpressNET.Routes;

public class Route
{
    public string path { get; set; }
    public RequestMethodAllowAll requestMethod { get; set; }
    public Action<
        Request,  // Request
        Response, // Response
        Action,   // Next
        int       // Index of calling route
    > action { get; set; }

    public bool firstCall { get; set; } = false;
}