namespace CalcForge.Compiler;

using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class EmitAttribute : Attribute
{
    public string TargetProfile { get; }
    public string Name { get; }
    public byte Opcode { get; }

    public EmitAttribute(string targetProfile, string name, byte opcode)
    {
        TargetProfile = targetProfile;
        Name = name;
        Opcode = opcode;
    }
}
