﻿using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;
using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter.Statements.Variables;

public class Getter : Statement
{
    public string VariableName;

    public static void OnInitialize()
    {
        Logger.LogInformation("Now initializing Getter");

        Statement.Register(StatementRegistration.CreateWithCustomParser<Getter>(
            tokenList => Value.Variables.ContainsKey(tokenList.Peek().RawContent), //Check if a variable with that name exists
            TokenType.Keyword
        ));
    }

    protected override bool CutTokensManually() => true;

    protected override void OnParse(ref TokenList list)
    {
        VariableName = list.Pop().RawContent;
    }

    public override Value Execute()
    {
        if (!Value.Variables.ContainsKey(VariableName))
            Interpreter.LogError("There is no variable called " + VariableName);

        //Interpreter.LogError makes the process exit so this will only run if the variable exists
        return Value.Variables[VariableName];
    }
}