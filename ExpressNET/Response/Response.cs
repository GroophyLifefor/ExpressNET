using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExpressNET;

public class Response
{
    private StreamWriter streamWriter;
    private NetworkStream networkStream;
    private Minimalizer _minimalizer;
    private Request _request;
    private Dictionary<string, string> Headers = new();
    private List<string> _appendToHeader = new();
    private DateTime __lastModified;
    private DateTime _lastModified
    {
        get
        {
            return __lastModified;
        }
        set
        {
            __lastModified = value;
            LastModified = __lastModified.ToString("r");
        }
    }

    public string HTTP_PROTOCOL { get; set; } = "HTTP/1.1";
    private int StatusCode { get; set; } = 200;

    public string Date
    {
        get => Headers["Date"];
        set => Headers["Date"] = value;
    }
    
    private string Server
    {
        get => Headers["Server"];
        set => Headers["Server"] = value;
    }

    private string ServerGitHub
    {
        get => Headers["Server-Github"];
        set => Headers["Server-Github"] = value;
    }
    
    public string AccessControlAllowOrigin
    {
        get => Headers["Access-Control-Allow-Origin"];
        set => Headers["Access-Control-Allow-Origin"] = value;
    }
    
    public string Connection
    {
        get => Headers["Connection"];
        set => Headers["Connection"] = value;
    }
    
    public string? ContentType
    {
        get => Headers["Content-Type"];
        set => Headers["Content-Type"] = value;
    }
    
    public string KeepAlive
    {
        get => Headers["Keep-Alive"];
        set => Headers["Keep-Alive"] = value;
    }
    
    public string LastModified
    {
        get => Headers["Last-Modified"];
        set => Headers["Last-Modified"] = value;
    }
    
    public Response(Request request, TcpClient client, Minimalizer minimalizer)
    {
        _request = request;
        _minimalizer = minimalizer;
        networkStream = client.GetStream();
        streamWriter = new StreamWriter(networkStream);

        Date = DateTime.UtcNow.ToString("r");
        _lastModified = DateTime.UtcNow;
        Server = "ExpressNET";
        ServerGitHub = "https://github.com/GroophyLifefor/ExpressNET";
        AccessControlAllowOrigin = "*";
        Connection = "Keep-Alive";
        ContentType = "text/html";
        KeepAlive = "timeout=5, max=999";
    }

    /// <summary>
    /// Sends response with empty body.
    /// </summary>
    private void HTTPResponse()
    {
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
        interpolatedStringHandler.AppendFormatted(HTTP_PROTOCOL);
        interpolatedStringHandler.AppendLiteral(" ");
        interpolatedStringHandler.AppendFormatted(StatusCode);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        streamWriter.WriteLine(stringAndClear);
        foreach ((string key, string str) in Headers)
            streamWriter.WriteLine(key + ": " + str);
        foreach (var append in _appendToHeader)
            streamWriter.WriteLine(append);
        FlushAndClose();
    }
    
    /// <summary>
    /// Sends response with given content
    /// </summary>
    /// <param name="content">Body of response</param>
    private void HTTPResponse(string content)
    {
        if (CheckETag(content.Length))
        {
            HTTPResponse();
            return;
        }
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
        interpolatedStringHandler.AppendFormatted(HTTP_PROTOCOL);
        interpolatedStringHandler.AppendLiteral(" ");
        interpolatedStringHandler.AppendFormatted(StatusCode);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        streamWriter.WriteLine(stringAndClear);
        foreach ((string key, string str) in Headers)
            streamWriter.WriteLine(key + ": " + str);
        foreach (var append in _appendToHeader)
            streamWriter.WriteLine(append);
        streamWriter.WriteLine("Content-Length: " + content.Length + "\r\n");
        streamWriter.Write(content);
        FlushAndClose();
    }
    
    /// <summary>
    /// Sends response with given content
    /// </summary>
    /// <param name="content">Body of response</param>
    private void HTTPResponse(byte[] content)
    {
        if (CheckETag(content.Length))
        {
            HTTPResponse();
            return;
        }
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
        interpolatedStringHandler.AppendFormatted(HTTP_PROTOCOL);
        interpolatedStringHandler.AppendLiteral(" ");
        interpolatedStringHandler.AppendFormatted(StatusCode);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        streamWriter.WriteLine(stringAndClear);
        foreach ((string key, string str) in Headers)
            streamWriter.WriteLine(key + ": " + str);
        foreach (var append in _appendToHeader)
            streamWriter.WriteLine(append);
        streamWriter.WriteLine("Content-Length: " + content.Length + "\r\n");
        streamWriter.Flush();
        networkStream.Write(content, 0, content.Length);
        FlushAndClose();
    }

    /// <summary>
    /// Check ETag is valid.
    /// if has valid ETag set status code as 304-Not Modified-.
    /// </summary>
    /// <param name="length">Content lenght</param>
    /// <returns>returns true if has valid ETag, returns false if has not valid ETag.</returns>
    private bool CheckETag(int length)
    {
        var etag = GenerateETag(((DateTimeOffset)_lastModified).ToUnixTimeSeconds(), length);
        if (_request.IfNoneMatch != null && _request.IfNoneMatch.Equals(etag))
        {
            status(304);
            return true;
        }
        setHeader("ETag", etag);
        return false;
    }

    /// <summary>
    /// Flushes and closes streams.
    /// </summary>
    private void FlushAndClose()
    {
        networkStream.Flush();
        streamWriter.Close();
    }

    /// <summary>
    /// Sets the HTTP status for the response.
    /// </summary>
    /// <param name="code">Specific status code</param>
    /// <returns>return this</returns>
    public Response status(int code)
    {
        ExceptionHelper.ThrowIfNull(code, "status code must be a number.");
        StatusCode = code;
        return this;
    }

    /// <summary>
    /// Response a status code to client without content.
    /// </summary>
    /// <param name="code">Specific status code</param>
    public void sendStatus(int code)
        => status(code).end();

    /// <summary>
    /// Sets the cookie when response delivered.
    /// Supports to multi-cookie.
    /// </summary>
    /// <returns></returns>
    public Response setCookie(
        string name, 
        string value, 
        string path = "/", 
        string expires = "", 
        string domain = "",
        string priority = "",
        bool isSecure = false,
        bool isHttpOnly = true)
    {
        string append =
            $"Set-Cookie: {name}={value}; path={path}; {(string.IsNullOrEmpty(expires) ? "" : $"expires={expires};")} {(string.IsNullOrEmpty(domain) ? "" : $"domain={domain};")} {(string.IsNullOrEmpty(priority) ? "" : $"priority={priority};")} {(isSecure ? "Secure;" : "")} {(isHttpOnly ? "HttpOnly;" : "")}";
        
        _appendToHeader.Add(append);
        return this;
    }

    /// <summary>
    /// Appends the specified value to the HTTP response header field. If the header is not already set, it creates the header with the specified value.
    /// </summary>
    /// <example>
    /// response.setHeader("Set-Cookie", "foo=bar; Path=/; HttpOnly");
    /// </example>
    /// <param name="name">Header name</param>
    /// <param name="value">Header value</param>
    public void setHeader(string name, string value)
        => Headers[name] = value;

    /// <summary>
    /// Sends the HTTP response as string with html format.
    /// </summary>
    /// <param name="value">Response data as string</param>
    /// <param name="ContentType">ContentType whatever you want an other type, you can help from response.type() function.</param>
    public void send(string value, string? ContentType = "text/html")
    {
        this.ContentType = ContentType;
        HTTPResponse(value);
    }
    
    /// <summary>
    /// Sends the HTTP response with html format.
    /// </summary>
    /// <param name="value">Response data as byte array</param>
    /// <param name="ContentType">ContentType whatever you want an other type, you can help from response.type() function.</param>
    private void send(byte[] value, string? ContentType = "text/html")
    {
        this.ContentType = ContentType;
        HTTPResponse(value);
    }

    /// <summary>
    /// Transfers the file at the given path. Sets the Content-Type response HTTP header field based on the filename’s extension. Unless the root option is set in the options object, path must be an absolute path to the file.
    /// </summary>
    /// <param name="filePaths">Path of file.</param>
    /// <param name="contentType">ContentType whatever you want an other type, you can help from response.type() function.</param>
    public void sendFile(string filePaths, string? contentType = null)
    {
        GetMostTruthPath(ref filePaths);
        SetContentType(filePaths, contentType);
        SetLastModified(filePaths);

        string content = File.ReadAllText(filePaths);
        Minimalize(ContentType, ref content);
        
        send(content, ContentType);
    }

    /// <summary>
    /// Ends the response process without content.
    /// You can set status code.
    /// <example>
    /// response.status(404).end();
    /// </example>
    /// </summary>
    public void end()
        => HTTPResponse();

    /// <summary>
    /// Generates ETag header
    /// </summary>
    /// <param name="lastModified">Last modified date as unix timestamp</param>
    /// <param name="size">Content size</param>
    /// <returns>ETag value</returns>
    private string GenerateETag(long lastModified, long size)
        => $"\"{lastModified:X}-{size:X}\"";

    /// <summary>
    /// Sets ContentType header from path.
    /// </summary>
    /// <param name="path">File path</param>
    /// <param name="contentType">Specific ContentType</param>
    private void SetContentType(string path, string? contentType = null)
        => ContentType = type(contentType ?? new FileInfo(path).Extension);

    /// <summary>
    /// Sets last modified date
    /// </summary>
    /// <param name="path">File path</param>
    private void SetLastModified(string path)
        => _lastModified = File.GetLastWriteTime(path);

    /// <summary>
    /// Minimalize CSS files for lowest cost.
    /// </summary>
    /// <param name="contentType">Specific ContentType</param>
    /// <param name="content">Content</param>
    private void Minimalize(string? contentType, ref string content)
        => _minimalizer.Minimalize(contentType, ref content);
    
    /// <summary>
    /// Finds possible paths from an part path.
    /// </summary>
    /// <param name="path">File path</param>
    /// <exception cref="FileNotFoundException">if was not pass "exist cases".</exception>
    private void GetMostTruthPath(ref string path)
    {
        path = path.Replace('/', '\\');
        if (!File.Exists(path)) path = Directory.GetCurrentDirectory() + path;
        if (!File.Exists(path)) throw new FileNotFoundException("The file specified in path was not found.");
    }
    
    // ReSharper disable StringLiteralTypo
    /// <summary>
    /// Sets the Content-Type HTTP header to the MIME type as determined by the specified type. If type contains the “/” character, then it sets the Content-Type to the exact value of type
    /// </summary>
    /// <param name="ext">File name or extension</param>
    /// <returns>returns determined specific type</returns>
    public string type(string ext)
    {
        ext = ext.Split('.').Last().TrimEnd('.');
        return ext switch
        {
            "aac" => "audio/aac",
            "audio/aac" => "audio/aac",
            "abw" => "application/x-abiword",
            "application/x-abiword" => "application/x-abiword",
            "arc" => "application/x-freearc",
            "application/x-freearc" => "application/x-freearc",
            "avif" => "image/avif",
            "image/avif" => "image/avif",
            "avi" => "video/x-msvideo",
            "video/x-msvideo" => "video/x-msvideo",
            "azw" => "application/vnd.amazon.ebook",
            "application/vnd.amazon.ebook" => "application/vnd.amazon.ebook",
            "bin" => "application/octet-stream",
            "application/octet-stream" => "application/octet-stream",
            "bmp" => "image/bmp",
            "image/bmp" => "image/bmp",
            "bz" => "application/x-bzip",
            "application/x-bzip" => "application/x-bzip",
            "bz2" => "application/x-bzip2",
            "application/x-bzip2" => "application/x-bzip2",
            "cda" => "application/x-cdf",
            "application/x-cdf" => "application/x-cdf",
            "csh" => "application/x-csh",
            "application/x-csh" => "application/x-csh",
            "css" => "text/css",
            "text/css" => "text/css",
            "csv" => "text/csv",
            "text/csv" => "text/csv",
            "doc" => "application/msword",
            "application/msword" => "application/msword",
            "docx" => "application/vnd.openxmlformats",
            "eot" => "application/vnd.ms-fontobject",
            "application/vnd.ms-fontobject" => "application/vnd.ms-fontobject",
            "epub" => "application/epub+zip",
            "application/epub+zip" => "application/epub+zip",
            "gz" => "application/gzip",
            "application/gzip" => "application/gzip",
            "gif" => "image/gif",
            "image/gif" => "image/gif",
            "html" => "text/html",
            "text/html" => "text/html",
            "ico" => "image/vnd.microsoft.icon",
            "image/vnd.microsoft.icon" => "image/vnd.microsoft.icon",
            "ics" => "text/calendar",
            "text/calendar" => "text/calendar",
            "jar" => "application/java-archive",
            "application/java-archive" => "application/java-archive",
            "jpeg" => "image/jpeg",
            "jpg" => "image/jpeg",
            "image/jpeg" => "image/jpeg",
            "js" => "text/javascript",
            "text/javascript" => "text/javascript",
            "json" => "application/json",
            "application/json" => "application/json",
            "jsonld" => "application/ld+json",
            "application/ld+json" => "application/ld+json",
            "mid" => "audio/midi",
            "audio/midi" => "audio/midi",
            "midi" => "audio/midi",
            "mjs" => "text/javascript",
            "mp3" => "audio/mpeg",
            "audio/mpeg" => "audio/mpeg",
            "mp4" => "video/mp4",
            "video/mp4" => "video/mp4",
            "mpeg" => "video/mpeg",
            "video/mpeg" => "video/mpeg",
            "mpkg" => "application/vnd.apple.installer+xml",
            "application/vnd.apple.installer+xml" => "application/vnd.apple.installer+xml",
            "odp" => "application/vnd.oasis.opendocument.presentation",
            "application/vnd.oasis.opendocument.presentation" => "application/vnd.oasis.opendocument.presentation",
            "ods" => "application/vnd.oasis.opendocument.spreadsheet",
            "application/vnd.oasis.opendocument.spreadsheet" => "application/vnd.oasis.opendocument.spreadsheet",
            "odt" => "application/vnd.oasis.opendocument.text",
            "application/vnd.oasis.opendocument.text" => "application/vnd.oasis.opendocument.text",
            "oga" => "audio/ogg",
            "audio/ogg" => "audio/ogg",
            "ogv" => "video/ogg",
            "video/ogg" => "video/ogg",
            "ogx" => "application/ogg",
            "application/ogg" => "application/ogg",
            "opus" => "audio/opus",
            "audio/opus" => "audio/opus",
            "otf" => "font/otf",
            "font/otf" => "font/otf",
            "png" => "image/png",
            "image/png" => "image/png",
            "pdf" => "application/pdf",
            "application/pdf" => "application/pdf",
            "php" => "application/x-httpd-php",
            "application/x-httpd-php" => "application/x-httpd-php",
            "ppt" => "application/vnd.ms-powerpoint",
            "application/vnd.ms-powerpoint" => "application/vnd.ms-powerpoint",
            "pptx" => "application/vnd.openxmlformats",
            "rar" => "application/vnd.rar",
            "application/vnd.rar" => "application/vnd.rar",
            "rtf" => "application/rtf",
            "application/rtf" => "application/rtf",
            "sh" => "application/x-sh",
            "application/x-sh" => "application/x-sh",
            "svg" => "image/svg+xml",
            "image/svg+xml" => "image/svg+xml",
            "tar" => "application/x-tar",
            "application/x-tar" => "application/x-tar",
            "tif" => "image/tiff",
            "tiff" => "image/tiff",
            "image/tiff" => "image/tiff",
            "ts" => "video/mp2t",
            "video/mp2t" => "video/mp2t",
            "ttf" => "font/ttf",
            "font/ttf" => "font/ttf",
            "txt" => "text/plain",
            "text/plain" => "text/plain",
            "vsd" => "application/vnd.visio",
            "application/vnd.visio" => "application/vnd.visio",
            "wav" => "audio/wav",
            "audio/wav" => "audio/wav",
            "weba" => "audio/webm",
            "audio/webm" => "audio/webm",
            "webm" => "video/webm",
            "video/webm" => "video/webm",
            "webp" => "image/webp",
            "image/webp" => "image/webp",
            "woff" => "font/woff",
            "font/woff" => "font/woff",
            "woff2" => "font/woff2",
            "font/woff2" => "font/woff2",
            "xhtml" => "application/xhtml+xml",
            "application/xhtml+xml" => "application/xhtml+xml",
            "xls" => "application/vnd.ms-excel",
            "application/vnd.ms-excel" => "application/vnd.ms-excel",
            "xlsx" => "application/vnd.openxmlformats",
            "application/vnd.openxmlformats" => "application/vnd.openxmlformats",
            "xml" => "application/xml",
            "application/xml" => "application/xml",
            "xul" => "application/vnd.mozilla.xul+xml",
            "application/vnd.mozilla.xul+xml" => "application/vnd.mozilla.xul+xml",
            "zip" => "application/zip",
            "application/zip" => "application/zip",
            "3gp" => "video/3gpp",
            "video/3gpp" => "video/3gpp",
            "3g2" => "video/3gpp2",
            "video/3gpp2" => "video/3gpp2",
            "7z" => "application/x-7z-compressed",
            "application/x-7z-compressed" => "application/x-7z-compressed",
            _ => "text/plain"
        };
    }
    // ReSharper restore StringLiteralTypo

}