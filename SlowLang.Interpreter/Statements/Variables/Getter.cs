using Microsoft.Extensions.Logging;
using SlowLang.Engine;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Statements.StatementRegistrations;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;

namespace SlowLang.Interpreter.Statements.Variables;

public class Getter : Statement
{
    public string VariableName = null!;

    public static void OnInitialize()
    {

        StatementRegistration.Create<Getter>(
                tokenList => Interpreter.Variables.ContainsKey(tokenList.Peek().RawContent), //Check if a variable with that name exists
                TokenType.Keyword)
            .Register();
    }

    protected override bool CutTokensManually() => true;

    protected override bool OnParse(ref TokenList list)
    {
        VariableName = list.Peek().RawContent;

        if (Interpreter.Variables.ContainsKey(VariableName))
            return true;
        
        Logger.LogCritical(
            "Variable getter parsed which tries to " +
            $"access not declared variable {VariableName}. " + 
            ("Parsing of the getter cancelled, now trying to parse {token}", list.Peek()) +
            "differently"
            );
        return false;
    }

    public override Value Execute()
    {
        if (!Interpreter.Variables.ContainsKey(VariableName))
            LoggingManager.LogError("There is no variable called " + VariableName, LineNumber);

        //Interpreter.LogError makes the process exit so this will only run if the variable exists
        return Interpreter.Variables[VariableName];
    }
}