using CalcForge.Attributes;
using CalcForge.Func;
using CalcForge.Tokenilzer;
using CalcForge.TokenObjects;
using System.Reflection;
namespace CalcForge.Evalutor;

public partial class Evaluator
{
    private  List<Token> _tokens;
    public IReadOnlyList<Token> OptimizedTokens => _tokens;
    private string StringInput;
    public Evaluator(string Input)
    {
       var splitTokens =  Tokenizer.Tokenize(StringInput = Input);
       _tokens = Parser.Parser.Parse(splitTokens);
    }

    

    public double Evaluate()
    {
        var tokenClones = _tokens.Select(token => token.New()).ToList();
        if (tokenClones.Count > 2)
        {
          
            if (tokenClones[^2].TokenOperation is TokenOperation.MultiplyOperation &&
                tokenClones[^1] is Token  and { TokenType: TokenType.Number, Value: "0" } )
                return 0;
            
        }
         var SubTokens = Posu(tokenClones);
         SubTokens = EvaluateFunctions(SubTokens).ToList();
        do
        {
            SubTokens = EvaluateGroups(SubTokens).ToList();
            SubTokens = EvaluateFunctions(SubTokens).ToList();
        }
        while (SubTokens.Any(x =>
            x.TokenTree == TokenTree.Group || x.TokenType == TokenType.Function));

        SubTokens =Posu(SubTokens);
        return eval(SubTokens.ToArray());
    }

    public List<Token> Posu(List<Token> SubTokens)
    {
        bool didChange;

        do
        {
            didChange = false;

            for (int i = 1; i < SubTokens.Count - 1; i++)
            {
                var token = SubTokens[i];

                if (token.TokenType != TokenType.Operation)
                    continue;

                if (token.TokenOperation is not (TokenOperation.MultiplyOperation or TokenOperation.DivideOperation or TokenOperation.PowerOperation))
                    continue;

                var left = SubTokens[i - 1];
                var right = SubTokens[i + 1];

                if (left.TokenType is  TokenType.CurlybracketStart or TokenType.CurlybracketEnds or TokenType.Function ||
                    right.TokenType is TokenType.CurlybracketEnds or TokenType.CurlybracketEnds or TokenType.Function) 
                    continue; // skip unsafe evaluation

                double value = eval([left, token, right]);
                SubTokens = Inject(SubTokens, i - 1, 3,
                    new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, value)).ToList();

                didChange = true;
                break; // restart loop with updated list
            }

        } while (didChange);

        if (SubTokens.Count == 1 && SubTokens[0].TokenTree == TokenTree.Single)
        {
            return [new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, Convert.ToDouble(SubTokens[0].Value))];
        }

        return SubTokens;
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
            var FuncMata = typeof(TokenOperation).GetField(Enum.GetName(Function.TokenOperation)).GetCustomAttribute<FuncMataAttribute>(true);
            if (FuncMata == null)
                throw new Exception($"Unable to Find mate for {funcd.FunctionName}");
            MethodInfo func = FuncMata.Type.GetMethods().Where(x=>x.Name == FuncMata.Name && x.GetParameters().Length== funcd.@params?.Length).FirstOrDefault();
            if (func == null)
                throw new Exception($"Method was not resolved [Parameters Missed Match/Count,Invalid FunctionName] :: {funcd.FunctionName}");
            int parmleng = 0;
            object[] parms = new object[parmleng=func.GetParameters().Count()];
            if(parmleng!= funcd.@params.Count())
            {
                throw new ArgumentException($"Parameters count missed match. was given {funcd.@params.Count()} but expected {parmleng}.");
            }
            for (int i = 0; i < func.GetParameters().Count(); i++)
            {
                var splitTokens = Tokenizer.Tokenize(funcd.@params[i]);
                var Tokens = Parser.Parser.Parse(splitTokens);
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
                parms[i] = Convert.ChangeType(eval(Posu(elvresult).ToArray()), func.GetParameters()[i].ParameterType);
                
            }
            var result = func.Invoke(FuncMata.Type,parms);
            tokens = Inject(tokens, FunctionIndex, 1, new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, Convert.ToDouble(result))).ToList();


        }
        return tokens.ToArray();
    }
    private readonly static string[] ConditionValuesContainersValue = ValueContainerAttribute.GetContainerValues(typeof(TokenType), "if");
    public Token[] EvaluateConditions(Token[] tokens)
    {
        if (tokens.Count() <3)
            return tokens;
        else if (tokens[0].TokenType is not TokenType.Number)
            return tokens;
        if (tokens[1].TokenOperation is not TokenOperation.If )
        {
            throw new Exception($"Invalid token, expected a If as the starter Condtion,but rec: {tokens[0].Value}");
        }
        var conditionType = FindAnyRemoveAny(tokens[2..], ConditionValuesContainersValue.Where(x=>x is not "if" or "else").ToArray());
        if (conditionType.wasSuc)
        {
            if(conditionType.foundToken.TokenType is TokenType.Conditions)
            {
                var EndindexOfB = EndOfCurlyBraces(string.Join("", (tokens[3..] ?? []).Select(x=>x.Value)));
              //  tokens = tokens[(3+EndindexOfB+1)..];

                var result = Posu(EvaluateGroups((tokens.Skip(2).Take(1) ?? []).ToList()).ToList());
                var tokenli = EndOfCurlyBraces(tokens[3..] ?? []);
                if (tokenli == -1)
                {
                    throw new InvalidOperationException($"Expected a group block '{{...}}' but found token of'{tokens[3].TokenType}'.");

                }
                if (Compare(Convert.ToDouble(tokens[0].Value), conditionType.foundToken.TokenOperation, eval(result.ToArray())))
                {

                    var first_Token = tokens[0];

                    tokens = tokens[4..];
                    var newT = tokens.ToList();

                    newT.RemoveAt(EndindexOfB-3);
                    tokens = [first_Token,..newT.ToArray()];

                    
                }
                else
                {
                    var first_Token = tokens[0];
                    
                    var newT = tokens.ToList();

                    newT.RemoveRange(1,EndindexOfB+1);
                    tokens = newT.ToArray();

                }

            }
        }
        return tokens;
       


    }
    private bool Compare(double num, TokenOperation operation, double right)
    {
        return operation switch
        {
            TokenOperation.eq => num == right,
            TokenOperation.neq => num != right,
            TokenOperation.lt => num < right,
            TokenOperation.gt => num > right,
            _ => throw new InvalidOperationException($"Unsupported comparison operator: {operation}")
        };
    }

    private Token[] EvaluateGroups( List<Token> tokens)
    {
        
        if(tokens.Count is 0) return [];
        while(tokens.Any(x => x.TokenTree == TokenTree.Group))
        {
            if (tokens.Any(x => x.TokenType == TokenType.Conditions))
            {
                int condIndex = tokens.FindIndex(x => x.TokenType == TokenType.Conditions);
                if (condIndex <= 0)
                    throw new InvalidOperationException("Expected a condition operator with a token before it.");

                int start = condIndex - 1;

                var sub = tokens.Skip(start).ToArray();

                var resultTokens = EvaluateConditions(sub);

                tokens.RemoveRange(start, tokens.Count - start);
                tokens.AddRange(resultTokens);

            }
            var GroupIndex = tokens.FindIndex(x=>x.TokenTree == TokenTree.Group);

            if(GroupIndex is not -1)
            {
                
                var currentGroup = ((List<Token>)tokens[GroupIndex].Value)
                    .Select(t => t.New())
                    .ToList();
                while (currentGroup.Count>1)
                {
                    if (tokens.Any(x => x.TokenType == TokenType.Conditions))
                    {
                        int condIndex = tokens.FindIndex(x => x.TokenType == TokenType.Conditions);
                        if (condIndex <= 0)
                            throw new InvalidOperationException("Expected a condition operator with a token before it.");

                        int start = condIndex - 1;

                        var sub = tokens.Skip(start).ToArray();
                        var resultTokens = EvaluateConditions(sub);
                        tokens.RemoveRange(start, tokens.Count - start);
                        tokens.AddRange(resultTokens);
                        GroupIndex = -1;
                        break;
                    }
                    
                    else if (currentGroup.Any(x => x.TokenTree == TokenTree.Group))
                    {
                        var index = currentGroup.FindIndex(x => x.TokenTree == TokenTree.Group);
                        currentGroup[index] = EvaluateGroups(
                            [currentGroup[index].New()]
                        )?.FirstOrDefault() ?? new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, 0);

                    }
                    //else if (currentGroup.Any(x => x.TokenType == TokenType.Conditions))
                    //{
                    //    int condIndex = currentGroup.FindIndex(x => x.TokenType == TokenType.Conditions);
                    //    if (condIndex <= 0)
                    //            throw new InvalidOperationException("Expected a condition operator with a token before it.");

                    //    int start = condIndex - 1;

                    //    var sub = currentGroup.Skip(start).ToArray();

                    //    var resultTokens = EvaluateConditions(sub);

                    //    currentGroup.RemoveRange(start, currentGroup.Count - start);
                    //    currentGroup.AddRange(resultTokens);

                    //}
                    else if (currentGroup.Any(x => x.TokenType == TokenType.Operation && x.TokenOperation is TokenOperation.PowerOperation or TokenOperation.MultiplyOperation or TokenOperation.DivideOperation))
                    {
                        var indexofHighProOpr = currentGroup.FindIndex(x => x.TokenType == TokenType.Operation && x.TokenOperation is TokenOperation.PowerOperation or TokenOperation.MultiplyOperation or TokenOperation.DivideOperation
                        ); if (indexofHighProOpr is -1)
                            continue;
                        //Left Value <Opr> Right Value
                        if (currentGroup.Count - 1 > indexofHighProOpr && currentGroup.Count + 1 > indexofHighProOpr)
                        {
                            if (currentGroup[indexofHighProOpr - 1].TokenType is TokenType.Function)
                            {
                                currentGroup[indexofHighProOpr - 1] = EvaluateFunctions([currentGroup[indexofHighProOpr - 1]])[0];
                            }
                            if (currentGroup[indexofHighProOpr + 1].TokenType is TokenType.Function)
                            {
                                currentGroup[indexofHighProOpr + 1] = EvaluateFunctions([currentGroup[indexofHighProOpr + 1]])[0];
                            }
                            double value = eval([currentGroup[indexofHighProOpr - 1], currentGroup[indexofHighProOpr], currentGroup[indexofHighProOpr + 1]]);
                            currentGroup = Inject(currentGroup, indexofHighProOpr - 1, count: 3, new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, value)).ToList();
                        }
                    }


                    else if (currentGroup.Any(x => x.TokenType == TokenType.Operation))
                    {
                        var indexofHighProOpr = currentGroup.FindIndex(x => x.TokenType == TokenType.Operation);
                        if (indexofHighProOpr is -1)
                            return tokens.ToArray();
                        //Left Value <Opr> Right Value
                        if (currentGroup.Count - 1 > indexofHighProOpr && currentGroup.Count + 1 > indexofHighProOpr)
                        {
                            if (currentGroup[indexofHighProOpr - 1].TokenType is TokenType.Function)
                            {
                                currentGroup[indexofHighProOpr - 1] = (EvaluateFunctions([currentGroup[indexofHighProOpr - 1]]) ?? [new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, 0)])[0];
                            }
                            if (currentGroup[indexofHighProOpr + 1].TokenType is TokenType.Function)
                            {
                                currentGroup[indexofHighProOpr + 1] = (EvaluateFunctions([currentGroup[indexofHighProOpr + 1]]) ?? [new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, 0)])[0];
                            }
                            double value = eval([currentGroup[indexofHighProOpr - 1], currentGroup[indexofHighProOpr], currentGroup[indexofHighProOpr + 1]]);
                            currentGroup = Inject(currentGroup, indexofHighProOpr - 1, count: 3, new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, value)).ToList();
                        }
                    }

                    else break;




                }
                

                if (currentGroup is not null and { Count: >0} && GroupIndex !=-1)
                {
                    tokens[GroupIndex] = currentGroup[0];
                }
                else if( GroupIndex != -1)
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
        return left??0;
    }
    private double evalOperator(double left,TokenOperation operation, double right)
    {
        return operation switch
        {
            TokenOperation.AddOperation => left + right,
            TokenOperation.SubtractOperation => left - right,
            TokenOperation.MultiplyOperation => left * right,
            TokenOperation.DivideOperation => left / right,
            TokenOperation.OROperation => (long)left | (long)right,
            TokenOperation.PowerOperation => System.Math.Pow(left, right),
            TokenOperation.ShiftRightOperation => (int)left >> (int)right,
            TokenOperation.ShiftLeftOperation => (int)left << (int)right,
            TokenOperation.ANDOperation => (long)left & (long)right,
            TokenOperation.Reminder => left % right
        };
    }
    public static (bool wasSuc, Token[] newTokens, Token? foundToken) FindAnyRemoveAny(Token[] tokens, string[] values)
    {
        var list = tokens.ToList();

        // Drill down to the first usable token
        int index = 0;
        Token first = list.FirstOrDefault() ?? throw new InvalidOperationException("Empty token list.");
        List<Token> trail = list;

        while (first.TokenTree == TokenTree.Group && first.Value is List<Token> inner)
        {
            if (inner.Count == 0)
            {
                // Empty group, remove and skip
                trail.RemoveAt(0);
                if (trail.Count == 0)
                    throw new InvalidOperationException("Empty group found with no usable token.");

                first = trail[0];
                continue;
            }

            if (inner.Count == 1)
            {
                // Keep drilling
                trail = inner;
                first = inner[0];
            }
            else
            {
                // Multiple tokens in group — check first real one
                first = inner[0];
                trail = inner;
                break;
            }
        }

        // Validate that first usable token matches expected values
        string opName = first.TokenOperation.ToString();
        if (first.TokenType == TokenType.Function && first.Value is FunctionData fdata)
            opName = fdata.FunctionName;

        if (string.IsNullOrWhiteSpace(opName) || opName == "None" || !values.Contains(opName))
            throw new InvalidOperationException($"First usable token '{opName}' is not valid or allowed.");

        // Remove the found token from its owning list
        trail.Remove(first);
        return (true, list.ToArray(), first);
    }




    public static int EndOfCurlyBraces(string value)
    {
        int openBraces = 0;
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == '{')
                openBraces++;
            else if (value[i] == '}')
                openBraces--;
            if (openBraces == 0)
                return i;

        }
        return -1;
    }
    public static int EndOfCurlyBraces(Token[] value)
    {
        int openBraces = 0;
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i].TokenType is TokenType.CurlybracketStart )
                openBraces++;
            else if (value[i].TokenType is TokenType.CurlybracketEnds)
                openBraces--;
            if (openBraces == 0)
                return i;

        }
        return -1;
    }

}
