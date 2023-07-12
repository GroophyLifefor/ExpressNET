using System.Security.Cryptography;
using System.Text;

namespace ExpressNET;

public class Minimalizer
{
    private Dictionary<string, string> Hash2Minimalized { get; set; } = new();
    public void Minimalize(string contentType, ref string content)
    {
        if (contentType != "text/css") return;
        
        string key = Hash(content);
        if (Hash2Minimalized.ContainsKey(key))
            content = Hash2Minimalized[key];
        string str = CompressCss(content);
        Hash2Minimalized.Add(key, str);
        content = str;
    }
    private string CompressCss(string css) => css.Replace("\r\n", "").Replace("    ", "").Replace(" {", "{").Replace(": ", ":");
    private string Hash(string input)
    {
        using (SHA1Managed shA1Managed = new SHA1Managed())
        {
            byte[] hash = shA1Managed.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder stringBuilder = new StringBuilder(hash.Length * 2);
            foreach (byte num in hash)
                stringBuilder.Append(num.ToString("X2"));
            return stringBuilder.ToString();
        }
    }
}