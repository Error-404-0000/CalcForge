using CalcForge;
using CalcForge.Compiler;

var e = new Evaluator("((((((2 + 0) + 3) * ((4 * 1) + (0 + 5))) + 2) / (((6 + 6) * 1) * (1 + 0 + 1))) +((100 / 5) - ((3 + 2) + 0))) +(((7 * 8) + (9 - 4 + 0)) * (((2 + 3 + 0) * 1) + 2)) -(((10 + 0 + 5) * 1) * ((6 - 0 - 1))) +((((50 + 50) / 2) * (((3 + 2) - 0) - (1 + 1)))) + 0 + 0 * 1234 ");
Console.WriteLine(Compile.AsString(e.OptimizedTokens.ToArray(), "MASM"));
Compile.BuildFile(e.OptimizedTokens.ToArray(), "MASM", ".asm", "C:\\Users\\Demon\\source\\repos\\CalcForge\\CalcForge.Test\\math");