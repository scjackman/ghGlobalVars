using System;
using System.Collections.Generic;

public static class GlobalState
{
    // Global dictionary
    private static readonly Dictionary<string, object> _data = new Dictionary<string, object>();

    // Method for setting a value to the dictionary
    public static void Set(string key, object value)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        lock (_data) // thread-safety
        {
            _data[key] = value;
        }
    }

    // Method for safely trying to get a value from the dictionary
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

    // Get all data
    public static Dictionary<string, object> GetAll()
    {
        lock (_data)
        {
            return new Dictionary<string, object>(_data);
        }
    }

    // Clear all data
    public static void Clear()
    {
        lock (_data)
        {
            _data.Clear();
        }
    }
}
