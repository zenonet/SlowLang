using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements;

/// <summary>
/// Represents a call to a function in a SlowLang script
/// </summary>
public class FunctionCall : Statement
{
    public static void OnInitialize()
    {
        Logger.LogInformation("Now initializing FunctionCall");
        Statement.Register(StatementRegistration.Create<FunctionCall>(
            TokenType.Keyword,
            TokenType.OpeningBrace
        ));
    }
    
    protected override void OnParse(ref TokenList list)
    {
        
    }
}