namespace SlowLang.Interpreter.Statements;

/// <summary>
/// Represents a single statement in a SlowLang script
/// </summary>
public abstract class Statement
{
    public virtual void Execute()
    {
    }

    public virtual void Parse()
    {
    }
}