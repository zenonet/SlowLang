﻿using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;
using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter.Statements;

public class ValueCall : Statement
{
    private Value? value;
    
    public static void OnInitialize()
    {
        Logger.LogInformation("Now initializing ValueCall");
        
        Statement.Register(StatementRegistration.Create<ValueCall>(
            TokenType.String
            ));
        
        Statement.Register(StatementRegistration.Create<ValueCall>(
            TokenType.Int
            ));
        
        Statement.Register(StatementRegistration.Create<ValueCall>(
            TokenType.Float
            ));
    }



    protected override void OnParse(ref TokenList list)
    {
        value = Value.Parse(list.Take(..1).AsTokenList());
    }
}