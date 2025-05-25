using CacheLily;
namespace CalcForge.Attributes;
/// <summary>
///    Tells the parser how to turn a token into a enum value
/// </summary>
/// <param name="HaveNext">If the token have a Spifc Opr type</param>
/// <param name="Values">value containers</param>
[AttributeUsage(AttributeTargets.Field)]

public class ValueContainerAttribute(bool HaveNext = false, params string[] Values) : Attribute
{
    private class StringCachedObject : ICacheable
    {
        public int CacheCode { get; set; }
        public string[] Values { get; set; }
    }
    private static readonly Cache<StringCachedObject> cacheValues = new Cache<StringCachedObject>(200, 200, false);
    private static readonly Cache<CacheObject<(bool, string?)>> cacheValue = new Cache<CacheObject<(bool, string?)>>(200, 200, false);
    public bool HaveNext { get; } = HaveNext;
    public string[] Values { get; } = Values;
    public static (bool haveNext, string? Value) GetContainerValue(Type EnumType, string Value)
    {

        return cacheValue.Invoke(typeof(ValueContainerAttribute), _getContainerValue, EnumType, Value).Value;
    }
    public static (bool haveNext, string? Value) _getContainerValue(Type EnumType, string Value)
    {
        if (EnumType.IsEnum)
        {
            foreach (var Fields in EnumType.GetFields())
            {
                ValueContainerAttribute valueContainers = (ValueContainerAttribute)Fields.GetCustomAttributes(typeof(ValueContainerAttribute), true).FirstOrDefault(x => ((ValueContainerAttribute)x).Values.Contains(Value));
                if (valueContainers != null)
                    return (valueContainers.HaveNext, Fields.Name);

            }
            return (false, null);
        }
        else
            throw new Exception("Expected EnumType to be Enum : [GetContainerValue]");
    }
    public static string[] GetContainerValues(Type EnumType, string Value)
    {
        return cacheValues.Invoke(typeof(ValueContainerAttribute), _Getvaluecontainerobject, EnumType, Value).Values;
    }
    private static string[] _Getvaluecontainerobject(Type EnumType, string Value)
    {
        if (EnumType.IsEnum)
        {
            foreach (var Fields in EnumType.GetFields())
            {
                ValueContainerAttribute valueContainers = (ValueContainerAttribute)Fields.GetCustomAttributes(typeof(ValueContainerAttribute), true).FirstOrDefault(x => ((ValueContainerAttribute)x).Values.Contains(Value));
                if (valueContainers != null)
                    return valueContainers.Values;

            }
            return [];
        }
        else
            throw new Exception("Expected EnumType to be Enum : [GetContainerValue]");
    }
}
