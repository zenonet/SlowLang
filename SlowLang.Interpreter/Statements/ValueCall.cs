using Microsoft.Extensions.Logging;
using SlowLang.Engine;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Statements.StatementRegistrations;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;

namespace SlowLang.Interpreter.Statements;

public class ValueCall : Statement
{
    private Value? value;

    public static void OnInitialize()
    {
        Logger.LogInformation("Now initializing ValueCall");

        StatementRegistration.Create<ValueCall>(TokenType.String).Register();
        StatementRegistration.Create<ValueCall>(TokenType.Int).Register();
        StatementRegistration.Create<ValueCall>(TokenType.Float).Register();
        StatementRegistration.Create<ValueCall>(TokenType.Bool).Register();
    }


    protected override bool CutTokensManually() => true;

    protected override void OnParse(ref TokenList tokenList)
    {
        value = Value.Parse(tokenList.List.Take(..1).AsTokenList());
        tokenList.Pop();
    }

    public override Value Execute()
    {
        return value!;
    }
}