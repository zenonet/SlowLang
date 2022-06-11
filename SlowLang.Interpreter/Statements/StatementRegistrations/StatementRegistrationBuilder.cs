using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements.StatementRegistrations;

public class StatementRegistrationBuilder
{
    public static implicit operator StatementRegistration(StatementRegistrationBuilder input) => input.Build();

    private CustomParser? customParser;
    private TokenType[]? match;
    private Type? statement;

    /// <summary>
    /// Adds a custom parser to the StatementRegistration
    /// </summary>
    /// <param name="parser">A CustomParser delegate that should contain the custom parser</param>
    public StatementRegistrationBuilder AddCustomParser(CustomParser parser)
    {
        customParser = parser;
        return this;
    }

    /// <summary>
    /// Adds a sequence of TokenTypes which need to get matched before the Statement gets instantiated
    /// </summary>
    /// <param name="match">The sequence of TokenTypes</param>
    public StatementRegistrationBuilder AddMatchSequence(params TokenType[] match)
    {
        this.match = match;
        return this;
    }
    
    /// <summary>
    /// Adds a reference to a Statement that should get instantiated when all the requirements of this StatementRegistration are fulfilled 
    /// </summary>
    /// <typeparam name="T">The statement to instantiate</typeparam>
    public StatementRegistrationBuilder AddStatementReference<T>() where  T : Statement
    {
        statement = typeof(T);
        return this;
    }

    /// <summary>
    /// Builds this StatementRegistrationBuilder to a StatementRegistration
    /// </summary>
    /// <returns>The built StatementRegistration</returns>
    public StatementRegistration Build()
    {
        if (statement is null)
            throw new(
                $"A StatementRegistration needs to have a StatementReference selected. Select one using StatementRegistrationBuilder.{nameof(AddStatementReference)}");
        return new StatementRegistration(statement, match, customParser);
    }

    /// <summary>
    /// Builds the StatementRegistration and registers it
    /// </summary>
    public void Register() => Build().Register();
}