namespace ExpressNET.Routes;

public class staticOptions
{
    /// <summary>
    /// Sets file extension fallbacks: If a file is not found, search for files with the specified extensions and serve the first one found.
    /// </summary>
    public List<string> extentions { get; set; } = new List<string>();
}