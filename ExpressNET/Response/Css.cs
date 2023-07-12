namespace ExpressNET.Css;

internal class Minify
{
    public string Minimalize(string css) => css.Replace("\r\n", "").Replace("    ", "").Replace(" {", "{").Replace(": ", ":");
}