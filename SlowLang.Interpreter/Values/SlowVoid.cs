namespace SlowLang.Interpreter.Values;

/// <summary>
/// Used as the return type of void functions
/// </summary>
public class SlowVoid : Value
{
    public static readonly SlowVoid I = new ();
}