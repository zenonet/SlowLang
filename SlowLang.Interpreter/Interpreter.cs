using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace SlowLang.Interpreter;

public static class Interpreter
{
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

    private static readonly ILogger Logger = LoggerFactory.CreateLogger("SusLang.Interpreter");

    public static void RunScript(string code)
    {
        Lexer.Lex(code);
    }
}