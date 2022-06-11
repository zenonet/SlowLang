using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements.StatementRegistrations;

public readonly struct StatementRegistration
{
    public readonly Type Statement;
    public readonly TokenType[] Match;

    public readonly CustomParser? CustomParser = null;

    internal StatementRegistration(Type statement, TokenType[]? match, CustomParser? customParser = null)
    {
        Match = match ?? Array.Empty<TokenType>();
        Statement = statement;
        CustomParser = customParser;
    }
    
    /// <summary>
    /// Creates a Statement Registration from a simple TokenType[] array
    /// </summary>
    /// <param name="match">The sequence of Tokens that needs to match</param>
    /// <typeparam name="T">The statement subclass that should get created when the token sequence gets matched</typeparam>
    /// <returns>A new StatementRegistration with the settings </returns>
    public static StatementRegistration Create<T>(params TokenType[] match) where T : Statement
    {
        return new StatementRegistration(typeof(T), match);
    }
    
    
    public static StatementRegistration Create<T>(CustomParser customParser, params TokenType[] match) where T : Statement
    {
        return new StatementRegistration(typeof(T), match, customParser);
    }


    public void Register()
    {
        Statements.Statement.Register(this);
    }

    /// <summary>
    /// Creates a new StatementRegistrationBuilder and sets the StatementReference
    /// </summary>
    /// <typeparam name="T">The Statement to add add the reference for</typeparam>
    /// <returns>The preconfigured builder</returns>
    public static StatementRegistrationBuilder Builder<T>() where T : Statement
    {
        return new StatementRegistrationBuilder().AddStatementReference<T>();
    }
    
    public static StatementRegistrationBuilder Builder()
    {
        return new StatementRegistrationBuilder();
    }

    public override string ToString() => Statement.Name + "-Registration";
}

/// <summary>
/// Used to define a custom Statement parser
/// </summary>
public delegate bool CustomParser(TokenList tokenList);