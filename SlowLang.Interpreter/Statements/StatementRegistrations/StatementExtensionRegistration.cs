using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements.StatementRegistrations;

public struct StatementExtensionRegistration
{
    public readonly Statement BaseStatement;
    public readonly Statement ExtensionStatement;
    public readonly CustomParser? CustomParser;
    public readonly TokenType[] Match;

    public StatementExtensionRegistration(Statement extensionStatement, Statement baseStatement, CustomParser customParser, params TokenType[] match)
    {
        Match = match;
        CustomParser = customParser;
        ExtensionStatement = extensionStatement;
        BaseStatement = baseStatement;
    }
    
    public StatementExtensionRegistration(Statement extensionStatement, Statement baseStatement, params TokenType[] match)
    {
        Match = match;
        CustomParser = null;
        ExtensionStatement = extensionStatement;
        BaseStatement = baseStatement;
    }
}