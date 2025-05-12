public record Token(TokenType TokenType,TokenOperation TokenOperation,TokenTree TokenTree,object Value)
{
    public Token New()
    {
        return new Token(
            TokenType,
            TokenOperation,
            TokenTree,
            CloneValue(Value)
        );
    }

    private dynamic CloneValue(dynamic value)
    {
        if (value is List<Token> tokenList)
        {
            return tokenList.Select(t => t.New()).ToList();
        }

        return value;
    }
}
