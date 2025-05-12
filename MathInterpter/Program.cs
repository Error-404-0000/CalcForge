
string Expr = @"((((((2 + 0) + 3) * ((4 * 1) + (0 + 5))) + 2) / (((6 + 6) * 1) * (1 + 0 + 1))) +
((100 / 5) - ((3 + 2) + 0))) +
(((7 * 8) + (9 - 4 + 0)) * (((2 + 3 + 0) * 1) + 2)) -
(((10 + 0 + 5) * 1) * ((6 - 0 - 1))) +
((((50 + 50) / 2) * (((3 + 2) - 0) - (1 + 1)))) + 0 + 0 * 1234 + Add(Pow(20,Sin((20/1*90)+Add(60,70)+90)),20)".Replace("\r\n", "");


Evaluator evaluator = new Evaluator(Expr);
Console.WriteLine(evaluator.Evaluate());
Debugger.WriteTree(evaluator.OptimizedTokens);