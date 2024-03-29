﻿using SlowLang.Engine.Values;
using SlowLang.Interpreter.Statements;
using SlowLang.Interpreter.Values;
using static SlowLang.Interpreter.Statements.FunctionDefinition;

namespace SlowLang.Interpreter;

public static class StandardLib
{
    public static void Import()
    {
        DefinedFunctions.Add(new FunctionDefinition(
            "print",
            context =>
            {
                context.ExpectParameters(typeof(SlowString));
                
                Interpreter.OutputStream?.WriteLine(((SlowString) context.Parameters[0]).Value);
                return SlowVoid.I;
            }
        ));

        DefinedFunctions.Add(new FunctionDefinition(
                "getTime",
                _ => new SlowString(DateTime.Now.ToShortTimeString())
            )
        );

        DefinedFunctions.Add(new FunctionDefinition(
                "getInput",
                _ => new SlowString(Interpreter.InputStream!.ReadLine()!)
            )
        );

        DefinedFunctions.Add(new FunctionDefinition(
                "getDate",
                _ => new SlowString(DateTime.Now.ToShortDateString())
            )
        );
    }
}