using System.Security.Cryptography;
using System.Text;

namespace ExpressNET;

public class Minimalizer
{
    private Dictionary<string, string> Hash2Minimalized { get; } = new();
    public void Minimalize(string? contentType, ref string content)
    {
        switch (contentType)
        {
            case "text/css":
            {
                string key = Hash(content);
                if (Hash2Minimalized.ContainsKey(key))
                {
                    content = Hash2Minimalized[key];
                    return;
                }
                
                // Minify css
                string str = new Css.Minify().Minimalize(content);
                
                Hash2Minimalized.Add(key, str);
                content = str;
            }
                break;
            case "text/javascript":
            {
                string key = Hash(content);
                if (Hash2Minimalized.ContainsKey(key))
                {
                    content = Hash2Minimalized[key];
                    return;
                }

                // Minify javascript
                string str = new Javascript.Minify().Minimalize(content);
                
                Hash2Minimalized.Add(key, str);
                content = str;
            }
                break;
            default:
                return;
        }
        
        
    }
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