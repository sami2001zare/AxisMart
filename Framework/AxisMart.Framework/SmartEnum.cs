using System.Collections.Concurrent;
using System.Reflection;

namespace AxisMart.Framework;

public abstract class SmartEnum<TEnum, TValue> where TEnum : SmartEnum<TEnum, TValue>
    where TValue : notnull
{
    private static readonly ConcurrentDictionary<TValue, TEnum> _items = new();

    public TValue Value { get; }
    public string Name { get; }

    protected SmartEnum(TValue value, string name)
    {
        Value = value;
        Name = name;
    }

    static SmartEnum()
    {
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var item = (TEnum)field.GetValue(null)!;
            Register(item);
        }
    }

    public static TEnum FromValue(TValue value)
    {
        return _items[value];
    }

    public static bool TryFromValue(TValue value, out TEnum result)
    {
        return _items.TryGetValue(value, out result!);
    }

    public static IEnumerable<TEnum> GetAll()
    {
        return _items.Values;
    }

    protected static TEnum Register(TEnum item)
    {
        return _items.GetOrAdd(item.Value, item);
    }

    public override string ToString()
    {
        return Name;
    }
}
