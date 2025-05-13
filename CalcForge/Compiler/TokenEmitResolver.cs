namespace CalcForge.Compiler;

using System;

public static class TokenEmitResolver
{
    public static EmitAttribute? ResolveEmitAttribute(Enum tokenOperation, string targetProfile)
    {
        var field = tokenOperation.GetType().GetField(tokenOperation.ToString());
        if (field == null) return null;

        var emits = field.GetCustomAttributes(typeof(EmitAttribute), false) as EmitAttribute[];
        foreach (var emit in emits)
        {
            if (emit.TargetProfile.Equals(targetProfile, StringComparison.OrdinalIgnoreCase))
                return emit;
        }
        return null;
    }
}
