using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements.Variables;

public class Getter : Statement
{
    
    
    public static void OnInitialize()
    {
        Logger.LogInformation("Now initializing Getter");
        Statement.Register(StatementRegistration.Create<Getter>(
            TokenType.Keyword,
            TokenType.Equals
        ));
    }
}