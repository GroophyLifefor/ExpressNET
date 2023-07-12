using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ExpressNET.Javascript;

internal class Minify
{
    /// <summary>
    /// First time can up to five second for minify
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public string Minimalize(string input)
        => MinifyJavaScript(input);

    static string MinifyJavaScript(string jsCode)
    {
        string url = "https://closure-compiler.appspot.com/compile";

        // Prepare the request parameters
        string parameters = $"js_code={Uri.EscapeDataString(jsCode)}&compilation_level=WHITESPACE_ONLY&output_format=text&output_info=compiled_code";
        byte[] requestData = Encoding.UTF8.GetBytes(parameters);

        // Create the HTTP request
        WebRequest request = WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = requestData.Length;

        // Write the request data to the request stream
        using (Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(requestData, 0, requestData.Length);
        }

        // Get the response from the server
        using (WebResponse response = request.GetResponse())
        using (Stream responseStream = response.GetResponseStream())
        using (StreamReader reader = new StreamReader(responseStream))
        {
            string minifiedCode = reader.ReadToEnd();
            return minifiedCode;
        }
    }
}