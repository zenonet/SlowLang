using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter.Statements;

/// <summary>
/// Represents a block of code
/// </summary>
public class CodeBlock : Statement
{
    /// <summary>
    /// A list of all statements this CodeBlock contains
    /// </summary>
    private readonly Statement[] statements;

    public CodeBlock(Statement[] statements)
    {
        this.statements = statements;
    }

    public override Value Execute()
    {
        statements.Execute();
        return SlowVoid.I;
    }
}