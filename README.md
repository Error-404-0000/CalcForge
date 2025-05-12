# Evtor — Math Expression Evaluator

Evtor is a lightweight, extensible C# library for parsing and evaluating complex mathematical expressions. It builds an internal token tree (AST-style), supports custom functions via reflection, and offers constant-folding optimizations.

---

## 🚀 Features

* **Recursive descent parser** with full support for nested groups (`(` `)`) and multi-parameter functions.
* **Custom function mapping** via `[ValueContainer]` and `[FuncMata]` attributes on enums.
* **Operator precedence**: `^` (power), `*` / `/`, `+` / `-`, bitwise `&`, `|`, shifts `<` `>`.
* **Built-in functions**: `Add`, `Subtract`, `Multiply`, `Divide`, `Pow`, `Abs`, `Floor`, `Ceiling`, `Round`, `Max`, `Min`, `Sqrt`, `Log`, `Log10`, `Exp`, `Sin`, `Cos`, `Tan`, `Truncate`, `Print`.
* **Token optimization**: constant folding (`0+X`, `X*1`, `(2+3+4)`) and group simplification.
* **Tree visualization**: pretty-print tokens/groups with `WriteTree` or raw tokens with `WriteTokens`.

---

## 📦 Installation

1. Clone or include the `Evtor` source files in your project.
2. Target **.NET 8.0** or later.


---

## 💡 Quick Start

```csharp
// Simple usage
var evaluator = new Evaluator("Add(2, Multiply(3, (4+5)))");
Console.WriteLine(evaluator.Evaluate()); // 17

// View raw tokens
Evaluator.WriteTokens(evaluator.Tokens);

// View token tree
WriteTree(evaluator.Tokens);

// View optimized token tree
var optimized = evaluator.OptimizeConstants(evaluator.Tokens);
WriteTree(optimized);
```

---

## 🔢 Example with Complex Expression

```csharp
string Expr = @"((((((2 + 0) + 3) * ((4 * 1) + (0 + 5))) + 2) / (((6 + 6) * 1) * (1 + 0 + 1))) +
((100 / 5) - ((3 + 2) + 0))) +
(((7 * 8) + (9 - 4 + 0)) * (((2 + 3 + 0) * 1) + 2)) -
(((10 + 0 + 5) * 1) * ((6 - 0 - 1))) +
((((50 + 50) / 2) * (((3 + 2) - 0) - (1 + 1)))) + 0 + 0 * 1234 + Add(Pow(20,Sin((20/1*90)+Add(60,70)+90)),20)".Replace("\r\n", "");

Evaluator evaluator = new Evaluator(Expr);
Console.WriteLine(evaluator.Evaluate());
Debugger.WriteTree(evaluator.OptimizedTokens);
```

**Output:**

```
556.6377040595102
└── [Group] ... (token tree output here)
```

---

## 📗 More Examples

```csharp
// Bitwise and nested logic
Console.WriteLine(new Evaluator("(15 > 1) & 7").Evaluate());       // 7
Console.WriteLine(new Evaluator("(3 < 2) | (1 + 1)").Evaluate());   // 2

// Math with precedence
Console.WriteLine(new Evaluator("(2 + 3) * (4 + 5) ^ 2").Evaluate()); // 2025

// Trigonometry
Console.WriteLine(new Evaluator("Sin(90) + Cos(0)").Evaluate());

// Deep nesting
Console.WriteLine(new Evaluator("Add(1, Add(2, Add(3, Add(4, Add(5, 6)))))").Evaluate());
```

---

(Other sections of the README follow here...)
