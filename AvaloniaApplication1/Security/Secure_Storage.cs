using System;

namespace AvaloniaApplication1.Security;

internal abstract class Secure_Storage
{
    protected static string Key => Security_Config.Encryption_Key;

    protected static void Wipe(ref string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            var chars = new char[value.Length];
            Array.Clear(chars, 0, chars.Length);
            value = new string(chars);
        }
        value = null;
    }
}