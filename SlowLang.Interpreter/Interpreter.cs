using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace SlowLang.Interpreter;

public static class Interpreter
{
    private static readonly ILogger Logger = new DebugLoggerProvider().CreateLogger("SusLang.Interpreter");
    public static void RunScript(string code)
    {
        Lexer.Lex(code);
    }
}