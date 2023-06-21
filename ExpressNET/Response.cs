using System.Net.Sockets;

namespace ExpressNET;

public class Response
{
    private StreamWriter streamWriter;
    private NetworkStream networkStream;
    private TcpClient _client;
    private int StatusCode { get; set; } = 200;
    private string Date { get; set; } = DateTime.UtcNow.ToString("r");
    private string Server { get; set; } = "ExpressNET";
    private List<string> tempHeader { get; set; } = new List<string>();

    public Response(TcpClient client)
    {
        networkStream = client.GetStream();
        streamWriter = new StreamWriter(networkStream);
        _client = client;
    }

    public void status(int code)
    {
        ExpectionHelper.ThrowIfNull(code, "status code must be a number.");

        StatusCode = code;
    }

    public void setHeader(string name, string value)
    {
        tempHeader.Add($"{name}: {value}");
    }

    public void send(string value, string ContentType = "text/html")
    {
        streamWriter.WriteLine($"HTTP/1.1 {StatusCode}");
        streamWriter.WriteLine($"Date: {Date}");
        streamWriter.WriteLine($"Server: {Server}");
        tempHeader.ForEach(x => streamWriter.WriteLine(x));
        streamWriter.WriteLine($"Content-Type: {ContentType}\r\n");
        streamWriter.WriteLine(value);
        streamWriter.Flush();

        streamWriter.Close();
        _client.Close();
    }

    public void sendFile(string filePaths, string ContentType = null)
    {
        send(File.ReadAllText(filePaths), ContentType is null ? getContentTypeFromExtention(filePaths.Split('.').Last()) : ContentType);
    }

    private string getContentTypeFromExtention(string ext)
    {
        ext = ext.TrimStart('.');
        return ext switch
        {
            "aac" => "audio/aac",
            "abw" => "application/x-abiword",
            "arc" => "application/x-freearc",
            "avif" => "image/avif",
            "avi" => "video/x-msvideo",
            "azw" => "application/vnd.amazon.ebook",
            "bin" => "application/octet-stream",
            "bmp" => "image/bmp",
            "bz" => "application/x-bzip",
            "bz2" => "application/x-bzip2",
            "cda" => "application/x-cdf",
            "csh" => "application/x-csh",
            "css" => "text/css",
            "csv" => "text/csv",
            "doc" => "application/msword",
            "docx" => "application/vnd.openxmlformats",
            "eot" => "application/vnd.ms-fontobject",
            "epub" => "application/epub+zip",
            "gz" => "application/gzip",
            "gif" => "image/gif",
            "html" => "text/html",
            "ico" => "image/vnd.microsoft.icon",
            "ics" => "text/calendar",
            "jar" => "application/java-archive",
            "jpeg" => "image/jpeg",
            "jpg" => "image/jpeg",
            "js" => "text/javascript",
            "json" => "application/json",
            "jsonld" => "application/ld+json",
            "mid" => "audio/midi",
            "midi" => "audio/midi",
            "mjs" => "text/javascript",
            "mp3" => "audio/mpeg",
            "mp4" => "video/mp4",
            "mpeg" => "video/mpeg",
            "mpkg" => "application/vnd.apple.installer+xml",
            "odp" => "application/vnd.oasis.opendocument.presentation",
            "ods" => "application/vnd.oasis.opendocument.spreadsheet",
            "odt" => "application/vnd.oasis.opendocument.text",
            "oga" => "audio/ogg",
            "ogv" => "video/ogg",
            "ogx" => "application/ogg",
            "opus" => "audio/opus",
            "otf" => "font/otf",
            "png" => "image/png",
            "pdf" => "application/pdf",
            "php" => "application/x-httpd-php",
            "ppt" => "application/vnd.ms-powerpoint",
            "pptx" => "application/vnd.openxmlformats",
            "rar" => "application/vnd.rar",
            "rtf" => "application/rtf",
            "sh" => "application/x-sh",
            "svg" => "image/svg+xml",
            "tar" => "application/x-tar",
            "tif" => "image/tiff",
            "tiff" => "image/tiff",
            "ts" => "video/mp2t",
            "ttf" => "font/ttf",
            "txt" => "text/plain",
            "vsd" => "application/vnd.visio",
            "wav" => "audio/wav",
            "weba" => "audio/webm",
            "webm" => "video/webm",
            "webp" => "image/webp",
            "woff" => "font/woff",
            "woff2" => "font/woff2",
            "xhtml" => "application/xhtml+xml",
            "xls" => "application/vnd.ms-excel",
            "xlsx" => "application/vnd.openxmlformats",
            "xml" => "application/xml",
            "xul" => "application/vnd.mozilla.xul+xml",
            "zip" => "application/zip",
            "3gp" => "video/3gpp",
            "3g2" => "video/3gpp2",
            "7z" => "application/x-7z-compressed",
            _ => "text/plain"
        };
    }
}