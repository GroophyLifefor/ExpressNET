using ExpressNET.Routes;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming

namespace ExpressNET;

public class Request
{
    private Dictionary<string, string> requestHeaders { get; } = new();
    public string? RequestRaw { get; }
    public RequestMethod Method { get; }
    public string? HTTP_PROTOCOL { get; }
    public string? Host { get; }
    public string Connection { get; }
    public string[]? sec_ch_ua { get; }
    public string? sec_ch_ua_mobile { get; }
    public string? sec_ch_ua_platform { get; }
    public string? User_Agent { get; }
    public string? Url { get; }
    public string? IfNoneMatch { get; }
    public string? IfModifiedSince { get; }
    public string? CacheControl { get; }
    public string? BaseUrl { get; }
    public string? Path { get; }
    public string? ContentEncoding { get; }
    public string? Prefer { get; }
    public string? Preferer { get; }
    public bool isFresh { get; }
    public bool isXHR { get; } // XML Http Request
    internal bool isValidClient => !string.IsNullOrEmpty(Url);
    public Dictionary<string, string> Params { get; } = new();
    public Dictionary<string, string> Cookies { get; } = new();
    public List<string> Accepts { get; } = new();
    public List<string> AcceptEncodings { get; } = new();
    public List<string> AcceptLanguages { get; } = new();
    public List<string> AcceptCharsets { get; } = new();

    public Request(string requestString)
    {
        if (string.IsNullOrEmpty(requestString)) return;

        RequestRaw = requestString;
        
        string[] splitRequestString = requestString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        
        string?[] requestParts = splitRequestString[0].Split(' ');
        Method = requestParts[0]?.ToUpper() switch
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
        HTTP_PROTOCOL = requestParts[2];
        BaseUrl = "/" + Url!.Trim('/').Split('/').First();
        Path = "/" + Url!.Trim('/').Split('/').Last().Split('?').First();
        for (int i = 1; i < splitRequestString.Length; i++)
        {
            string name = splitRequestString[i].Substring(0, splitRequestString[i].IndexOf(':')).Trim();
            string? value = splitRequestString[i].Substring(splitRequestString[i].IndexOf(':')+1).Trim();

            requestHeaders.TryAdd(name, value);
            
            switch (name)
            {
                case "Host":
                    Host = value;
                    break;
                case "Connection":
                    Connection = value;
                    break;
                case "sec-ch-ua":
                    sec_ch_ua = value.Split(',');
                    break;
                case "sec-ch-ua-mobile":
                    sec_ch_ua_mobile = value;
                    break;
                case "sec-ch-ua-platform":
                    sec_ch_ua_platform = value;
                    break;
                case "User-Agent":
                    User_Agent = value;
                    break;
                case "If-None-Match":
                    IfNoneMatch = value;
                    break;
                case "If-Modified-Since":
                    IfModifiedSince = value;
                    break;
                case "Cookie":
                    string[] _cookies = value.Split(';');
                    foreach (var cookie in _cookies)
                    {
                        string cookieName = cookie.Substring(0, cookie.IndexOf('=')).Trim();
                        string cookieValue =  cookie.Substring(cookie.IndexOf('=')+1).Trim();
                        
                        if (!Cookies.TryAdd(cookieName, cookieValue))
                        {
                            Console.WriteLine("Somethings gone wrong when cookies listing but server still running.");
                        }
                    }
                    break;
                case "Cache-Control":
                    CacheControl = value;
                    isFresh = CacheControl != "no-cache";
                    break;
                case "X-Requested-With":
                    isXHR = true;
                    break;
                case "Accept":
                    ApplyAccepts(value, Accepts);
                    break;
                case "Accept-Encoding":
                    ApplyAccepts(value, AcceptEncodings);
                    break;
                case "Accept-Language":
                    ApplyAccepts(value, AcceptLanguages);
                    break;
                case "Accept-Charset":
                    ApplyAccepts(value, AcceptCharsets);
                    break;
                case "Content-Encoding":
                    ContentEncoding = value;
                    break;
                case "Prefer":
                    Prefer = value;
                    break;
                case "Preferer":
                    Preferer = value;
                    break;
            }
        }
    }

    private void ApplyAccepts(string value, List<string> list)
    {
        string[] acceptFormats = value.Split(';');
        for (int j = 0; j < acceptFormats.Length; j++)
        {
            var acceptFormat = acceptFormats[j];
            var formats = acceptFormat.Split(',', StringSplitOptions.TrimEntries);

            for (int k = 0; k < formats.Length; k++)
            {
                var format = formats[k];
                if (format.StartsWith('q')) continue;
                            
                list.Add(format);
            }
        }
    }

    /// <summary>
    /// Checks if the specified content types are acceptable
    /// </summary>
    /// <param name="value">ContentType</param>
    /// <returns>Is acceptable</returns>
    public bool accepts(string value)
        => Accepts.Any(
            accept =>
                accept == "*/*" ||
                accept == value ||
                accept == type(value));

    /// <summary>
    /// Checks if the specified encodings are acceptable
    /// </summary>
    /// <param name="value">Encoding</param>
    /// <returns>Is acceptable</returns>
    public bool acceptsEncodings(string value)
        => AcceptEncodings.Any(
            accept =>
                accept == value);
    
    /// <summary>
    /// Checks if the specified languages are acceptable
    /// </summary>
    /// <param name="value">Language</param>
    /// <returns>Is acceptable</returns>
    public bool acceptsLanguages(string value)
        => AcceptLanguages.Any(
            accept =>
                accept == value);
    
    /// <summary>
    /// Checks if the specified charset are acceptable
    /// </summary>
    /// <param name="value">Charset</param>
    /// <returns>Is acceptable</returns>
    public bool acceptsCharsets(string value)
        => AcceptCharsets.Any(
            accept =>
                accept == value);

    public string? get(string headerName)
        => requestHeaders.ContainsKey(headerName) ? requestHeaders[headerName] : null;

    internal void SetParams(string path)
    {
        Params.Clear();
        
        string[] spPath = path.Split('/');
        string[]? spURL = Url?.Split('/');
        
        if (spURL is null) return;

        for (int i = 0; i < Math.Min(spPath.Length, spURL.Length); i++)
        {
            var _path = spPath[i];
            var _url = spURL[i];

            if (!_path.StartsWith(':')) continue;

            if (!Params.TryAdd(_path.TrimStart(':'), _url))
            {
                Console.WriteLine("Somethings gone wrong when params listing but server still running.");
            }
        }
    }
    
    // ReSharper disable StringLiteralTypo
    /// <summary>
    /// Sets the Content-Type HTTP header to the MIME type as determined by the specified type. If type contains the “/” character, then it sets the Content-Type to the exact value of type
    /// </summary>
    /// <param name="ext">File name or extension</param>
    /// <returns>returns determined specific type</returns>
    private string type(string ext)
    {
        ext = ext.Split('.').Last();
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