namespace CalcForge;

[AttributeUsage(AttributeTargets.Field)]
//this tells the parser which method to use and where it is
public class FuncMataAttribute(Type type, string Name):Attribute
{
    public Type Type { get; } = type;
    public string Name { get; } = Name;
}