using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Statements;
using SlowLang.Interpreter.Tokens;
using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter;

public static class Interpreter
{
    public static TextWriter? OutputStream;
    
    
    
    
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

    private static readonly ILogger Logger = LoggerFactory.CreateLogger("SlowLang.Interpreter");

    internal static readonly ILogger ErrorLogger = LoggerFactory.CreateLogger("SlowLang.Errors");


    internal static void LogError(string errorMessage)
    {
        ErrorLogger.LogError(errorMessage);
        Environment.Exit(0);
    }

    public static void RunScript(string code)
    {
        OutputStream ??= Console.Out;
        
        TokenList tokenList = Lexer.Lex(code);

        StandardLib.Import();

        Statement[] statements = Statement.ParseMultiple(tokenList);
        statements.Execute();
    }
}