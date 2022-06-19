﻿using Microsoft.Extensions.Logging;
using SlowLang.Engine;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Tokens;

namespace SlowLang.Interpreter;

public static class Interpreter
{
    public static TextWriter? OutputStream;
    public static TextReader? InputStream;
    
    
    
    private static readonly Dictionary<string, TokenType> TokenDefinitions = new()
    {
        {"\".*?\"", TokenType.String},
        
        {@"\(", TokenType.OpeningBrace},
        {@"\)", TokenType.ClosingBrace},
        
        {@"\{", TokenType.OpeningCurlyBrace},
        {@"\}", TokenType.ClosingCurlyBrace},
        
        {@"\d+", TokenType.Int},
        {@"\d+.?\d*(?:f|F)", TokenType.Float},
        {@"(?:(?:t|T)(?:rue|RUE))|(?:(?:f|F)(?:alse|ALSE))", TokenType.Bool},
        
        
        {@";", TokenType.Semicolon},
        {@",", TokenType.Comma},
        {@"\s*=\s*", TokenType.Equals},
        
        
        {@"\w*", TokenType.Keyword}, //Needs to be the last one
    };
    
    
    
    /// <summary>
    /// The LoggerFactory used for all Loggers in the Project
    /// </summary>
    public static readonly ILoggerFactory LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
    {
        builder
            .ClearProviders()
            .AddSimpleConsole()
            ;
    });
    
    public static void RunScript(string code)
    {
        LoggingManager.SetLoggerFactory(LoggerFactory);
        
        OutputStream ??= Console.Out;
        InputStream ??= Console.In;
        
        Lexer.DefineTokens(TokenDefinitions);
        TokenList tokenList = Lexer.Lex(code);

        StandardLib.Import();

        Statement[] statements = Statement.ParseMultiple(tokenList);
        statements.Execute();
    }
}