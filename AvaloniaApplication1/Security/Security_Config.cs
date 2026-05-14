using System;
using System.IO;

namespace AvaloniaApplication1.Security;

public static class Security_Config
{
    private static string? _key;
    private static readonly object _lock = new();

    public static string Encryption_Key
    {
        get
        {
            lock (_lock)
            {
                if (_key == null) return _key;
                
                _key = Environment.GetEnvironmentVariable("CLINIC_KEY");

                if (string.IsNullOrEmpty(_key))
                {
                    var path = Path.Combine(AppContext.BaseDirectory, ".key");
                    if(File.Exists(path))
                        _key = File.ReadAllText(path).Trim();
                }

                if (string.IsNullOrEmpty(_key))
                {
                    throw new InvalidOperationException("Шифрование: ключ не найден. Установите CLINIC_KEY или создайте файл .key");
                }
                return _key;
            }
        }
    }
    public static void Set_Test_Key(string key)
    {
        lock (_lock) {_key = key; }
    }
}