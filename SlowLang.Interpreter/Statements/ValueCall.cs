using Microsoft.Extensions.Logging;
using SlowLang.Engine;
using SlowLang.Engine.Initialization;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Statements.StatementRegistrations;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;

namespace SlowLang.Interpreter.Statements;

public class ValueCall : Statement, IInitializable
{
    private Value? value;

    public static void Initialize()
    {
        StatementRegistration.Create<ValueCall>(TokenType.String).Register();
        StatementRegistration.Create<ValueCall>(TokenType.Int).Register();
        StatementRegistration.Create<ValueCall>(TokenType.Float).Register();
        StatementRegistration.Create<ValueCall>(TokenType.Bool).Register();
    }


    protected override bool CutTokensManually() => true;

    protected override bool OnParse(ref TokenList tokenList)
    {
        value = Value.Parse(ref tokenList);

        if (value == null)
        {
            Logger.LogError("Unable to parse {token}", tokenList.Peek());
            return false;
        }
        
        return true;
    }

    public override Value Execute()
    {
        return value!;
    }
}