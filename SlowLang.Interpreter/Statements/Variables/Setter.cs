﻿using Microsoft.Extensions.Logging;
using SlowLang.Engine;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Statements.StatementRegistrations;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;

namespace SlowLang.Interpreter.Statements.Variables;

public class Setter : Statement
{
    private Statement? value;
    private string? varName;

    public static void OnInitialize()
    {
        StatementRegistration.Create<Setter>(
                TokenType.Keyword,
                TokenType.Equals)
            .Register();
    }


    protected override bool CutTokensManually() => true;


    protected override bool OnParse(ref TokenList list)
    {
        //Get the variable name
        varName = list.Pop().RawContent;
        //Remove the equals
        list.Pop();

        value = Statement.Parse(ref list, false);

        if (value == null)
            return false;
        
        if (list.StartsWith(TokenType.Semicolon))
            list.Pop();
        else
            Logger.LogCritical("Missing semicolon after setter statement");

        //Register the variable but don't give it any value yet
        if (!Interpreter.Variables.ContainsKey(varName))
        {
            Interpreter.Variables.Add(varName, SlowVoid.I);
            Logger.LogInformation($"Declared a variable called {varName}");
        }

        return true;
    }

    public override Value Execute()
    {
        Value val = value!.Execute();

        if (val.IsVoid)
            LoggingManager.LogError($"{value} doesn't have a return value", LineNumber);

        Interpreter.Variables[varName!] = val;
        return SlowVoid.I;
    }
}