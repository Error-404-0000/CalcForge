using System.Reflection;

public partial class Evaluator
{
    public readonly List<Token> OptimizedTokens;
    private string StringInput;
    public Evaluator(string Input)
    {
       var splitTokens =  Tokenizer.Tokenize(StringInput = Input);
       OptimizedTokens = Parser.Parse(splitTokens);
    }

    

    public double Evaluate()
    {
        var tokenClones = OptimizedTokens.Select(token => token.New()).ToList();
        if (tokenClones.Count > 2)
        {
          
            if (tokenClones[^2].TokenOperation is TokenOperation.MultiplyOperation &&
                tokenClones[^1] is Token  and { TokenType: TokenType.Number, Value: "0" } )
                return 0;
            
        }
        var SubTokens = EvaluateFunctions(tokenClones).ToList();
        do
        {
            SubTokens = EvaluateGroups(SubTokens).ToList();
            SubTokens = EvaluateFunctions(SubTokens).ToList();
        }
        while (SubTokens.Any(x =>
            x.TokenTree == TokenTree.Group || x.TokenType == TokenType.Function));

        return Posu(SubTokens);
       
    }
    public double Posu(List<Token> SubTokens)
    {
        while (SubTokens.Any(x => x.TokenType == TokenType.Operation && x.TokenOperation is TokenOperation.MultiplyOperation or TokenOperation.DivideOperation or TokenOperation.PowerOperation))
        {
            if (SubTokens.Any(x => x.TokenType == TokenType.Operation && x.TokenOperation is TokenOperation.MultiplyOperation or TokenOperation.DivideOperation or TokenOperation.PowerOperation))
            {
                var indexofHighProOpr = SubTokens.FindIndex(x => x.TokenType == TokenType.Operation && x.TokenOperation is TokenOperation.MultiplyOperation or TokenOperation.DivideOperation
                or TokenOperation.PowerOperation);
                //Left Value <Opr> Right Value
                if (SubTokens.Count() - 1 > indexofHighProOpr && SubTokens.Count() + 1 > indexofHighProOpr)
                {
                    double value = eval([SubTokens[indexofHighProOpr - 1], SubTokens[indexofHighProOpr], SubTokens[indexofHighProOpr + 1]]);
                    SubTokens = Inject(SubTokens, indexofHighProOpr - 1, count: 3, new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, value)).ToList();
                }
            }
        }
        if (SubTokens.Count is 1 && SubTokens[0].TokenTree is TokenTree.Single)
            return Convert.ToDouble(SubTokens[0].Value);
        return eval(SubTokens.ToArray());
        
    }
    private Token[] EvaluateFunctions(List<Token> tokens)
    {
        if (tokens.Count == 0)
            return tokens.ToArray();
        while(tokens.Any(x=>x.TokenType == TokenType.Function))
        {
            var FunctionIndex = tokens.FindIndex(x => x.TokenType == TokenType.Function);
            var Function = tokens[FunctionIndex];
            FunctionData? funcd = Function.Value as FunctionData;
            if (funcd is null)
            {
                throw new Exception("Invalid Function Call");
            }
            var FuncMata = (FuncMata)typeof(TokenOperation).GetField(Enum.GetName(Function.TokenOperation)).GetCustomAttribute<FuncMata>(true);
            if (FuncMata == null)
                throw new Exception($"Unable to Find mate for {funcd.FunctionName}");
            MethodInfo func = FuncMata.Type.GetMethods().Where(x=>x.Name == FuncMata.Name && x.GetParameters().Length== funcd.PARMS?.Length).FirstOrDefault();
            if (func == null)
                throw new Exception($"Method was not resolved [Parameters Missed Match/Count,Invalid FunctionName] :: {funcd.FunctionName}");
            int parmleng = 0;
            object[] parms = new object[parmleng=func.GetParameters().Count()];
            if(parmleng!= funcd.PARMS.Count())
            {
                throw new ArgumentException($"Parameters count missed match. was given {funcd.PARMS.Count()} but expected {parmleng}.");
            }
            for (int i = 0; i < func.GetParameters().Count(); i++)
            {
                var splitTokens = Tokenizer.Tokenize(funcd.PARMS[i]);
                var Tokens = Parser.Parse(splitTokens);
                while(Tokens.Any(x=>x.TokenType is TokenType.Function))
                {
                    var funin = Tokens.FindIndex(x => x.TokenType is TokenType.Function);
                    if (funin != -1)
                    {
                        Tokens = Inject(Tokens, funin, 1, new Token(TokenType.None, TokenOperation.None, TokenTree.Group, EvaluateFunctions(Tokens).ToList())).ToList();

                    }
                    
                    else
                    {
                        break;
                    }
                }
                var elvresult = EvaluateGroups(Tokens).ToList();
                if(elvresult.Any(x=>x.TokenType==TokenType.Function))
                {
                    elvresult = EvaluateFunctions(elvresult).ToList();
                }
                parms[i] = Convert.ChangeType(Posu(elvresult), func.GetParameters()[i].ParameterType);
                
            }
            var result = func.Invoke(FuncMata.Type,parms);
            tokens = Inject(tokens, FunctionIndex, 1, new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, Convert.ChangeType(result,typeof(decimal)))).ToList();


        }
        return tokens.ToArray();
    }
    private Token[] EvaluateGroups( List<Token> tokens)
    {
        
        if(tokens.Count is 0) return [];
        while(tokens.Any(x => x.TokenTree == TokenTree.Group))
        {
            var GroupIndex = tokens.FindIndex(x=>x.TokenTree == TokenTree.Group);
            if(GroupIndex is not -1)
            {
               
                var currentGroup = ((List<Token>)tokens[GroupIndex].Value)
                    .Select(t => t.New())
                    .ToList();
                while (currentGroup.Count>1)
                { 
                    if(currentGroup.Any(x => x.TokenTree == TokenTree.Group))
                    {
                        var index = currentGroup.FindIndex(x => x.TokenTree == TokenTree.Group);
                        currentGroup[index] = EvaluateGroups(
                            [currentGroup[index].New()]
                        )?.FirstOrDefault()??new Token(TokenType.Number,TokenOperation.None,TokenTree.Single,0);

                    }
                    else if (currentGroup.Any(x => x.TokenType == TokenType.Operation && x.TokenOperation is  TokenOperation.PowerOperation or TokenOperation.MultiplyOperation or TokenOperation.DivideOperation ))
                    {
                        var indexofHighProOpr = currentGroup.FindIndex(x => x.TokenType == TokenType.Operation && x.TokenOperation is TokenOperation.PowerOperation or TokenOperation.MultiplyOperation or TokenOperation.DivideOperation
                        ); if (indexofHighProOpr is -1)
                            continue;
                        //Left Value <Opr> Right Value
                        if (currentGroup.Count - 1 > indexofHighProOpr && currentGroup.Count + 1 > indexofHighProOpr)
                        {
                            if(currentGroup[indexofHighProOpr - 1].TokenType is TokenType.Function)
                            {
                                currentGroup[indexofHighProOpr - 1] = EvaluateFunctions((List<Token>)[currentGroup[indexofHighProOpr - 1]])[0];
                            }
                            if (currentGroup[indexofHighProOpr + 1].TokenType is TokenType.Function)
                            {
                                currentGroup[indexofHighProOpr + 1] = EvaluateFunctions((List<Token>)[currentGroup[indexofHighProOpr +1]])[0];
                            }
                            double value = eval([currentGroup[indexofHighProOpr - 1], currentGroup[indexofHighProOpr], currentGroup[indexofHighProOpr + 1]]);
                            currentGroup = Inject(currentGroup, indexofHighProOpr - 1, count: 3, new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, value)).ToList();
                        }
                    }
                    else if (currentGroup.Any(x => x.TokenType == TokenType.Operation))
                    {
                        var indexofHighProOpr = currentGroup.FindIndex(x => x.TokenType == TokenType.Operation );
                        if (indexofHighProOpr is -1)
                            return tokens.ToArray();
                        //Left Value <Opr> Right Value
                        if (currentGroup.Count - 1 > indexofHighProOpr && currentGroup.Count + 1 > indexofHighProOpr)
                        {
                            if (currentGroup[indexofHighProOpr - 1].TokenType is TokenType.Function)
                            {
                                currentGroup[indexofHighProOpr - 1] = (EvaluateFunctions((List<Token>)[currentGroup[indexofHighProOpr - 1]]) ?? [new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, 0)])[0];
                            }
                            if (currentGroup[indexofHighProOpr + 1].TokenType is TokenType.Function)
                            {
                                currentGroup[indexofHighProOpr + 1] = (EvaluateFunctions((List<Token>)[currentGroup[indexofHighProOpr + 1]])??[new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, 0)])[0];
                            }
                            double value = eval([currentGroup[indexofHighProOpr - 1], currentGroup[indexofHighProOpr], currentGroup[indexofHighProOpr + 1]]);
                            currentGroup = Inject(currentGroup, indexofHighProOpr - 1, count: 3, new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, value)).ToList();
                        }
                    }


                   

                }
                if(currentGroup is not null and { Count: >0})
                {
                    tokens[GroupIndex] = currentGroup[0];
                }
                else
                {
                    tokens[GroupIndex] = new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, 0);
                }
                
            }
        }
        return tokens.ToArray();
    }
    private IEnumerable<Token> Inject(List<Token> tokens,int start,int count,Token Token)
    {
         tokens.RemoveRange(start, count);
        tokens.Insert(start, Token);
        return tokens;
    }
    /// <summary>
    /// Evaluate From LEFT->Right NO Math Rules
    /// </summary>
    /// <param name="Tokens"></param>
    /// <returns></returns>
    private double eval(Token[] Tokens)
    {
        double? left = null;
        TokenOperation operation = TokenOperation.None;
        foreach (var Token in Tokens)
        {
            if(Token.TokenType is TokenType.Number && left is null && operation is TokenOperation.None)
            {
                left = Convert.ToDouble(Token.Value);
            }
            else if (Token.TokenType is TokenType.Operation && left is not null)
            {
                operation = Token.TokenOperation;
            }
            else if (Token.TokenType is TokenType.Number && left != null && operation is not TokenOperation.None)
            {
                left = evalOperator(left.Value, operation, Convert.ToDouble(Token.Value));
                
                operation = TokenOperation.None;
            }
            else
            {
                throw new Exception($"Invalid token at :{Token.Value}. no left side value was set");
            }
        }
        return left.Value;
    }
    private double evalOperator(double left,TokenOperation operation, double right)
    {
        return operation switch
        {
            TokenOperation.AddOperation => left + right,
            TokenOperation.SubtractOperation => left - right,
            TokenOperation.MultiplyOperation => left * right,
            TokenOperation.DivideOperation => left / right,
            TokenOperation.OROperation => ((long)left) | ((long)right),
            TokenOperation.PowerOperation => System.Math.Pow(left, right),
            TokenOperation.ShiftRightOperation => ((int)left) >> ((int)right),
            TokenOperation.ShiftLeftOperation => ((int)left) << ((int)right),
            TokenOperation.ANDOperation => ((long)left) & ((long)right),
            TokenOperation.Reminder => left % right
        };
    }
   
  
}
