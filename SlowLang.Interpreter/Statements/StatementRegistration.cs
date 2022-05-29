using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements;

public struct StatementRegistration
{
    public readonly Type Statement;
    public readonly TokenType[] Match;

    internal StatementRegistration(Type statement, TokenType[] match)
    {
        Match = match;
        Statement = statement;
    }
    
    public static StatementRegistration Create<T>(params TokenType[] match) where T : Statement
    {
        return new StatementRegistration(typeof(T), match);
    }
    
    
}