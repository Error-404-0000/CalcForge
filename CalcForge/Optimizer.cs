namespace CalcForge;
    public static class Optimizer
    {
    
            public static void OptimizeLeftZeroAddSub(List<Token> tokens)
            {
                if (tokens == null || tokens.Count < 2)
                    return;

                int tokenCount = tokens.Count;

                var lastToken = tokens[tokenCount - 1];
                var secondLastToken = tokens[tokenCount - 2];
            
                //0+
                if (lastToken.TokenType == TokenType.Operation &&
                    (lastToken.TokenOperation == TokenOperation.AddOperation ||
                     lastToken.TokenOperation == TokenOperation.SubtractOperation) &&
                    secondLastToken.TokenType == TokenType.Number &&
                    Convert.ToDouble(secondLastToken.Value) == 0)
                {
                    tokens.RemoveRange(tokenCount - 2, 2);
                }
                ////+0
                //else  if (secondLastToken.TokenType == TokenType.Operation &&
                //    (lastToken.TokenOperation == TokenOperation.AddOperation ||
                //     lastToken.TokenOperation == TokenOperation.SubtractOperation) &&
                //    lastToken.TokenType == TokenType.Number &&
                //    Convert.ToDouble(lastToken.Value) == 0)
                //{
                //    tokens.RemoveRange(tokenCount - 2, 2);
                //}
            }
        public static Token CollapseGroupIfPossible(Token groupToken)
        {
            if (groupToken.TokenTree == TokenTree.Group)
            {
                var groupList = (List<Token>)groupToken.Value;
                if (groupList.Count == 1)
                {
                   
                    var inner = groupList[0];
                    if (inner.TokenTree == TokenTree.Single && inner.TokenType == TokenType.Number)
                    {
                        return inner; // collapse it
                    }
                }
            }
            return groupToken;
        }

        public static Token OptimizeGroup(Token token)
        {
            if (token.TokenTree != TokenTree.Group || token.Value is not List<Token> innerTokens)
                return token;

            // Recursively optimize nested groups first
            var optimizedInner = innerTokens
                .Select(OptimizeGroup)
                .ToList();

            // Flatten nested group layers like [Group -> Group -> Number]
            while (optimizedInner.Count == 1 && optimizedInner[0].TokenTree == TokenTree.Group)
            {
                optimizedInner = optimizedInner[0].Value as List<Token>;
            }

            // If only one token and it's Single, return it directly
            if (optimizedInner.Count == 1 && optimizedInner[0].TokenTree == TokenTree.Single)
                return optimizedInner[0];

            // Re-wrap if still a group
            return new Token(
                TokenType.None,
                TokenOperation.None,
                TokenTree.Group,
                optimizedInner
            );
        }
        public static Token OptimizeGroup(List<Token> tokens)
        {
            return new Token(TokenType.None,TokenOperation.None,TokenTree.Group, tokens.Select(OptimizeGroup).ToList());
        }
    }
