using System.Diagnostics.CodeAnalysis;

namespace ExpressNET;

internal class ExpectionHelper
{
    public static object? ThrowIfNull(object obj, string message) =>
        obj is null ? throw new ArgumentNullException(message) : null;
}