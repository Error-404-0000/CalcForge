namespace CalcForge;

public static class Debugger
    {
        public static void WriteTree(IEnumerable<Token> tokens, string indent = "", bool isLast = true)
        {
            var tokenList = tokens.ToList();
            for (int i = 0; i < tokenList.Count; i++)
            {
                var token = tokenList[i];
                bool last = i == tokenList.Count - 1;

                string prefix = indent + (isLast ? "└── " : "├── ");
                string nextIndent = indent + (isLast ? "    " : "│   ");

                if (token.TokenTree == TokenTree.Single)
                {
                    if (token.TokenType == TokenType.Function)
                    {
                        var func = token.Value as FunctionData;
                        Console.WriteLine($"{prefix}[Function] {func.FunctionName} ({string.Join(", ", func.@params)})");
                    }
                    else
                    {
                        Console.WriteLine($"{prefix}[{token.TokenType}] {token.Value}");
                    }
                }
                else if (token.TokenTree == TokenTree.Group)
                {
                    Console.WriteLine($"{prefix}[Group]");
                    WriteTree(token.Value as IEnumerable<Token>, nextIndent, last);
                }
            }
        }
    }
   
  
