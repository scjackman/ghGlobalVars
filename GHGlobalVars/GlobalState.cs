using System;
using System.Collections.Generic;

public static class GlobalState
{
    // The global dictionary
    private static readonly Dictionary<string, object> _data = new Dictionary<string, object>();

    // Set a value
    public static void Set(string key, object value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        lock (_data) // thread-safety
        {
            _data[key] = value;
        }
    }

    // Try to get a value safely
    public static bool TryGet<T>(string key, out T value)
    {
        value = default;
        if (key == null) return false;

        lock (_data)
        {
            if (_data.TryGetValue(key, out var obj))
            {
                if (obj is T t)
                {
                    value = t;
                    return true;
                }
            }
        }
        return false;
    }

    // Optional: remove a key
    public static bool Remove(string key)
    {
        if (key == null) return false;
        lock (_data)
        {
            return _data.Remove(key);
        }
    }

    // Optional: clear all data
    public static void Clear()
    {
        lock (_data)
        {
            _data.Clear();
        }
    }
}
