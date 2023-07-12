namespace ExpressNET;

internal abstract class ExceptionHelper
{
    public static object? ThrowIfNull(object obj, string message) =>
        obj is null ? throw new ArgumentNullException(message) : null;
}