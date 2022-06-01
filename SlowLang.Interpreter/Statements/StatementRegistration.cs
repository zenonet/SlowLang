using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements;

public struct StatementRegistration
{
    public readonly Type Statement;
    public readonly TokenType[] Match;

    public Func<TokenList, bool>? CustomParser = null;

    internal StatementRegistration(Type statement, TokenType[] match, Func<TokenList, bool>? customParser = null)
    {
        Match = match;
        Statement = statement;
        CustomParser = customParser;
    }
    
    public static StatementRegistration Create<T>(params TokenType[] match) where T : Statement
    {
        return new StatementRegistration(typeof(T), match);
    }
    
    public static StatementRegistration CreateWithCustomParser<T>(Func<TokenList, bool>? customParser, params TokenType[] match) where T : Statement
    {
        return new StatementRegistration(typeof(T), match, customParser);
    }
    
    
}