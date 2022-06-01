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
    protected Statement[] Statements;

    public override Value Execute()
    {
        Statements.Execute();
        return SlowVoid.I;
    }
}