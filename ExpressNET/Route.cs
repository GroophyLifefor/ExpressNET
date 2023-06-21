namespace ExpressNET;

public class Route
{
    public string path { get; set; }
    public Action<Request, Response> action { get; set; }
}