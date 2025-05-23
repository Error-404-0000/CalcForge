﻿namespace CalcForge;
/// <summary>
///    Tells the parser how to turn a token into a enum value
/// </summary>
/// <param name="HaveNext">if the token have a Spifc Opr type</param>
/// <param name="Values">value containers</param>
[AttributeUsage(AttributeTargets.Field)]

public class ValueContainerAttribute(bool HaveNext=false,params string[] Values) : Attribute
{
    public bool HaveNext { get; } = HaveNext;
    public string[] Values { get; } = Values;
    public static (bool haveNext,string? Value) GetContainerValue(Type EnumType, string Value)
    {
        if (EnumType.IsEnum)
        {
            foreach (var Fields in EnumType.GetFields())
            {
                ValueContainerAttribute valueContainers = (ValueContainerAttribute)Fields.GetCustomAttributes(typeof(ValueContainerAttribute), true).FirstOrDefault(x=> ((ValueContainerAttribute)x).Values.Contains(Value));
                if(valueContainers != null)
                    return (valueContainers.HaveNext,Fields.Name);
            
            }
            return (false,null);
        }
        else
            throw new Exception("Expected EnumType to be Enum : [GetContainerValue]");
    }
}
