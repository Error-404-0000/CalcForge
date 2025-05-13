
using CalcForge;
// Bitwise and nested logic
Console.WriteLine(new Evaluator("(15 > 1) & 7").Evaluate());       // 7
Console.WriteLine(new Evaluator("(3 < 2) | (1 + 1)").Evaluate());   // 2

// Math with precedence
Console.WriteLine(new Evaluator("(2 + 3) * (4 + 5) ^ 2").Evaluate()); // 2025

// Trigonometry
Console.WriteLine(new Evaluator("Sin(90) + Cos(0)").Evaluate());

// Deep nesting
Console.WriteLine(new Evaluator("Add(1, Add(2, Add(3, Add(4, Add(5, 6)))))").Evaluate());
