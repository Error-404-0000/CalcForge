using CalcForge.TokenObjects;

namespace CalcForge.Compiler;

public static class Compile
{
    public static string AsString(Token[] tokens, string profile)
    {
        var lines = new List<string>();
        var masmRegisters = new[] { "eax", "ebx", "ecx", "edx", "esi", "edi", "r8", "r9", "r10", "r11", "r12", "r13", "r14", "r15" };
        var regPool = new Stack<string>(masmRegisters.Reverse());
        var liveRegs = new HashSet<string>();
        EmitRecursive(tokens, lines, regPool, liveRegs, out _, profile);
        return string.Join("\n", lines);
    }

    public static void BuildFile(Token[] tokens, string profile, string extension, string pathWithoutExtension)
    {
        var asm = AsString(tokens, profile);
        var fullPath = Path.ChangeExtension(pathWithoutExtension, extension);
        File.WriteAllText(fullPath, asm);
    }

    private static void EmitRecursive(Token[] tokens, List<string> output, Stack<string> regPool, HashSet<string> liveRegs, out string lastResult, string profile)
    {
        int i = 0;
        lastResult = null;

        while (i < tokens.Length)
        {
            Token token = tokens[i];

            if (token.TokenTree == TokenTree.Group && token.Value is List<Token> groupTokens)
            {
                EmitRecursive(groupTokens.ToArray(), output, regPool, liveRegs, out var groupResult, profile);
                lastResult = groupResult;
                i++;
                continue;
            }

            var field = token.TokenOperation.GetType().GetField(token.TokenOperation.ToString());
            var attr = field?.GetCustomAttributes(typeof(CompliableAttribute), false)
                             .FirstOrDefault() as CompliableAttribute;
            var emitAttr = TokenEmitResolver.ResolveEmitAttribute(token.TokenOperation, profile);

            if (attr == null || emitAttr == null)
            {
                i++;
                continue;
            }

            var operands = new List<string>();
            string leftOp = null, rightOp = null;

            foreach (var need in attr.Needs)
            {
                switch (need)
                {
                    case CompileNeeds.Left:
                        leftOp = lastResult ?? (i - 1 >= 0 && tokens[i - 1].TokenType == TokenType.Number ? tokens[i - 1].Value.ToString() : "0");
                        operands.Add(leftOp);
                        break;
                    case CompileNeeds.Right:
                        string right = "0";
                        if (i + 1 < tokens.Length)
                        {
                            var rightToken = tokens[i + 1];
                            if (rightToken.TokenTree == TokenTree.Group && rightToken.Value is List<Token> subGroup)
                            {
                                EmitRecursive(subGroup.ToArray(), output, regPool, liveRegs, out var subResult, profile);
                                right = subResult;
                            }
                            else if (rightToken.TokenType == TokenType.Number)
                            {
                                right = rightToken.Value.ToString();
                            }
                        }
                        rightOp = right;
                        operands.Add(right);
                        break;
                    case CompileNeeds.Value:
                        operands.Add(token.Value.ToString());
                        break;
                }
            }

            if (regPool.Count == 0)
                throw new InvalidOperationException("Register pool exhausted. Consider implementing a smarter allocator.");

            string resultReg = regPool.Pop();

            // Emit valid MASM
            if (operands.Count == 2)
            {
                output.Add($"mov {resultReg}, {operands[0]}");
                output.Add($"{emitAttr.Name} {resultReg}, {operands[1]}");
            }
            else if (operands.Count == 1)
            {
                output.Add($"{emitAttr.Name} {resultReg}, {operands[0]}");
            }
            else
            {
                output.Add($"{emitAttr.Name} {string.Join(", ", operands)}");
            }

            liveRegs.Add(resultReg);

            if (leftOp != null && liveRegs.Remove(leftOp))
                regPool.Push(leftOp);

            if (rightOp != null && liveRegs.Remove(rightOp))
                regPool.Push(rightOp);

            lastResult = resultReg;
            i += attr.Needs.Contains(CompileNeeds.Right) ? 2 : 1;
        }
    }
}