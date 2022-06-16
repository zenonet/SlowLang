using Microsoft.Extensions.Logging;
using SlowLang.Engine;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Tokens;

namespace SlowLang.Interpreter;

public static class Interpreter
{
    public static TextWriter? OutputStream;
    public static TextReader? InputStream;
    
    
    
    
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
        
        TokenList tokenList = Lexer.Lex(code);

        StandardLib.Import();

        Statement[] statements = Statement.ParseMultiple(tokenList);
        statements.Execute();
    }
}