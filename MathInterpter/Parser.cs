
public static partial class Parser
{
    public static List<Token> Parse(string[] StringTokens)
    {
        List<Token> tokens = new List<Token>();
        for (int i = 0; i < StringTokens.Length; i++)
        {

            if (Tokenizer.IsNumber(StringTokens[i]))
            {
                tokens.Add(new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, StringTokens[i]));
            }
            else
            {
                var GetTokenType = ValueContainer.GetContainerValue(typeof(TokenType), StringTokens[i]);
                if (GetTokenType is (_, null))
                    throw new Exception($"{string.Join("", StringTokens)}  : Invalid TokenType '{StringTokens[i]}'");


                if (Tokenizer.IsFunctionName(StringTokens[i]) && (TokenType)Enum.Parse(typeof(TokenType), GetTokenType.Value) is not TokenType.Function)
                {
                    throw new Exception($"Invalid FunctionName {StringTokens[i]}");
                }
                if ((TokenType)Enum.Parse(typeof(TokenType), GetTokenType.Value) is TokenType.Function)
                {
                    var IndexOfEndParms = FindOpenedParenthesisEnd(StringTokens[(i + 1)..]);
                    if (IndexOfEndParms == -1)
                        throw new InvalidOperationException($"Invalid parm given to {StringTokens[i]}");
                    string[] Parms = GroupParms(string.Join("", StringTokens.ToArray().Skip(i + 2).Take(IndexOfEndParms - 1)));

                    var GetTokenOperation = ValueContainer.GetContainerValue(typeof(TokenOperation), StringTokens[i]);
                    if (GetTokenOperation is (_, null))
                        throw new Exception($"{string.Join("", StringTokens)}  : Invalid TokenOperation {StringTokens[i]}");

                    FunctionData function = new FunctionData(StringTokens[i], Parms);
                    tokens.Add(new Token((TokenType)Enum.Parse(typeof(TokenType), GetTokenType.Value),
                         (TokenOperation)Enum.Parse(typeof(TokenOperation), GetTokenOperation.Value),
                         TokenTree.Single, function));
                    i += IndexOfEndParms + 1;


                }
                else if ((TokenType)Enum.Parse(typeof(TokenType), GetTokenType.Value) is TokenType.ParenthesisOpen)
                {
                    var EndParenthesis = FindOpenedParenthesisEnd(StringTokens.Skip(i).ToArray());
                    if (EndParenthesis == -1)
                        throw new InvalidOperationException($"No matching closing parenthesis found starting at position {i} in the input: {StringTokens[i]}");
                    string[] GroupPart = StringTokens.ToArray().Skip(i + 1).Take(EndParenthesis - 1).ToArray();
                    var NewGroup = Parse(GroupPart);
                    //IF GROUP COUNT IS 0 ,LET JUST ADD IT FLAT DIRECTLY

                    tokens?.Add(Optimizer.CollapseGroupIfPossible(Optimizer.OptimizeGroup(NewGroup)));
                    i += EndParenthesis;

                }
                else if ((TokenType)Enum.Parse(typeof(TokenType), GetTokenType.Value) is TokenType.ParenthesisClose)
                {
                    // This should never happen if there is already an open parenthesis
                    throw new InvalidOperationException($"Unexpected closing parenthesis at position {i} in the input: {StringTokens[i]}");
                }

                else
                {

                    if (GetTokenType.haveNext)
                    {

                        var GetTokenOperation = ValueContainer.GetContainerValue(typeof(TokenOperation), StringTokens[i]);
                        if (GetTokenOperation is (_, null))
                            throw new Exception($"{string.Join("", StringTokens)}  : Invalid TokenOperation {StringTokens[i]}");


                        tokens.Add(new Token((TokenType)Enum.Parse(typeof(TokenType), GetTokenType.Value),
                      (TokenOperation)Enum.Parse(typeof(TokenOperation), GetTokenOperation.Value),
                      TokenTree.Single, StringTokens[i]));

                    }
                    else
                        tokens.Add(new Token((TokenType)Enum.Parse(typeof(TokenType), GetTokenType.Value),
                           TokenOperation.None,
                           TokenTree.Single, StringTokens[i]));
                }

            }
            int TokenCount = 0;
            //self-Optimize BOTH SIZE DONT MATTER WHEN 0
            if ((TokenCount = tokens?.Count ?? 0) >= 2)
            {
                //
                //catching /0 as soon as posible
                if (tokens[TokenCount - 2].TokenOperation is TokenOperation.DivideOperation)
                {
                    if (tokens[TokenCount - 1] is Token v1 and { TokenType: TokenType.Number } && Convert.ToDouble(v1.Value) is 0)
                        throw new DivideByZeroException($"at {i}");
                    // for 0/100  =replace with 0
                    else if (TokenCount > 3 && tokens[TokenCount - 3] is Token v2 and { TokenType: TokenType.Number } && Convert.ToDouble(v2.Value) == 0)
                    {
                        tokens.RemoveRange(TokenCount - 3, 3);
                        tokens.Add(new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, 0));
                        continue;
                    }
                    else if (TokenCount >= 3 && tokens[TokenCount - 1] is Token v22 and { TokenType: TokenType.Number } && Convert.ToDouble(v22.Value) == 1)
                    {
                        tokens.RemoveRange(TokenCount - 2, 2);
                        continue;
                    }
                }
                else if (TokenCount >= 3)
                {

                    if (tokens[TokenCount - 2].TokenOperation is TokenOperation.MultiplyOperation)
                    {
                        // for 0*100 or 100*0 or any eq based on 0 =remove the whole part            <<0>> = 0 ,<<0000>> = 0;
                        if ((tokens[TokenCount - 1] is Token v1 && v1.TokenType is TokenType.Number && Convert.ToDouble(v1.Value) == 0)
                        || (tokens[TokenCount - 3] is Token v2 && v2.TokenType is TokenType.Number && Convert.ToDouble(v2.Value) == 0))
                        {
                            tokens.RemoveRange(TokenCount - 3, 3);
                            tokens.Add(new Token(TokenType.Number, TokenOperation.None, TokenTree.Single, 0));

                        }
                        else if ((tokens[TokenCount - 1] is Token v11 && v11.TokenType is TokenType.Number && Convert.ToDouble(v11.Value) == 1)
                         || (tokens[TokenCount - 3] is Token v22 && v22.TokenType is TokenType.Number && Convert.ToDouble(v22.Value) == 2))
                        {
                            tokens.RemoveRange(TokenCount - 2, 2);


                        }
                        continue;
                    }
                }
            }
            Optimizer.OptimizeLeftZeroAddSub(tokens??[]);


        }
        //doing this last in case of +0
        int TokenCount_ = tokens.Count;
        if ((TokenCount_ = tokens.Count) >= 2)
        {
            //0+
            if (tokens[TokenCount_ - 2].TokenType is TokenType.Operation)
            {
                if (tokens[TokenCount_ - 2].TokenOperation is TokenOperation.AddOperation or TokenOperation.SubtractOperation)
                {
                    if (tokens[TokenCount_ - 1].TokenType is TokenType.Number && Convert.ToDouble(tokens[TokenCount_ - 1].Value) == 0)
                        tokens.RemoveRange(TokenCount_ - 2, 2);
                }
            }


        }
        return tokens;
    }
   

    

    public static string[] GroupParms(string value)
    {
        List<string> parms = new List<string>();
        int OpenedParenthesis = 0;
        string currentValue = null;
        for (int i = 0; i < value.Length; i++)
        {
            if (OpenedParenthesis == 0 && value[i] == ',')
            {
                parms.Add(currentValue);
                currentValue = null;
            }

            else if (value[i] == '(')
            {
                OpenedParenthesis++;
                currentValue += value[i];
            }
            else if (value[i] == ')')
            {
                OpenedParenthesis--;
                currentValue += value[i];
            }
            else
            {
                currentValue += value[i];
            }

        }
        if (currentValue != null)
        {
            parms.Add(currentValue);
        }
        return parms.ToArray();
    }


    public static int FindOpenedParenthesisEnd(string[] StartParenthesis)
    {
        if (StartParenthesis is null || StartParenthesis.Length == 0)
            return -1;
        int startPr = 0;
        int endPr = 0;
        for (int i = 0; i < StartParenthesis.Length; i++)
        {

            if (StartParenthesis[i] == "(")
                startPr++;
            else if (StartParenthesis[i] == ")")
                startPr--;
            if (startPr == 0)
            {
                endPr = i;
                break;
            }

        }
        return startPr != 0 ? -1 : endPr;
    }
}