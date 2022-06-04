﻿using SlowLang.Interpreter.Statements;
using SlowLang.Interpreter.Values;
using static SlowLang.Interpreter.Statements.FunctionDefinition;

namespace SlowLang.Interpreter;

public static class StandardLib
{
    public static void Import()
    {
        DefinedFunctions.Add(new FunctionDefinition(
            "print",
            (parameters) =>
            {
                SlowLang.Interpreter.Interpreter.OutputStream?.WriteLine(((SlowString)parameters[0]).Value);
                return SlowVoid.I;
            }
        ));
        
        DefinedFunctions.Add(new FunctionDefinition(
            "getInput",
            _ => new SlowString(Interpreter.InputStream!.ReadLine()!)));
    }
}