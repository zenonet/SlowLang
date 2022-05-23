namespace SlowLang.Interpreter.Tokens;

public class Token
{
    public string RawContent;
    public TokenType Type;

    public Token(string rawContent, TokenType type)
    {
        RawContent = rawContent;
        Type = type;
    }
}