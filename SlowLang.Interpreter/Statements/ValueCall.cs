using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Statements.StatementRegistrations;
using SlowLang.Interpreter.Tokens;
using SlowLang.Interpreter.Values;

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

    protected override void OnParse(ref TokenList list)
    {
        value = Value.Parse(list.Take(..1).AsTokenList());
        list.Pop();
    }

    public override Value Execute()
    {
        return value!;
    }
}