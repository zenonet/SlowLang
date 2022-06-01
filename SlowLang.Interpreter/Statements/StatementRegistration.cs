using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements;

public struct StatementRegistration
{
    public readonly Type Statement;
    public readonly TokenType[] Match;

    public CustomParser? CustomParser = null;


    internal StatementRegistration(Type statement, TokenType[] match, CustomParser? customParser = null)
    {
        Match = match;
        Statement = statement;
        CustomParser = customParser;
    }
    
    public static StatementRegistration Create<T>(params TokenType[] match) where T : Statement
    {
        return new StatementRegistration(typeof(T), match);
    }
    
    public static StatementRegistration CreateWithCustomParser<T>(CustomParser customParser, params TokenType[] match) where T : Statement
    {
        return new StatementRegistration(typeof(T), match, customParser);
    }
    
    
}

public delegate bool CustomParser(TokenList tokenList);