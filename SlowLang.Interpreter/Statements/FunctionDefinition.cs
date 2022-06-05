using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter.Statements;

/// <summary>
/// Represents the definition of a function in a SlowLang script
/// </summary>
public class FunctionDefinition
{
    /// <summary>
    /// Contains all functions that can be called in a SlowLang script
    /// </summary>
    public static readonly List<FunctionDefinition> DefinedFunctions = new();


    public string Identifier;
    public Type[] Parameters;

    public readonly Func<Value[], Value> OnInvoke;
    
    public FunctionDefinition(string identifier, Func<Value[], Value> onInvoke, params Type[] parameters)
    {
        Identifier = identifier;
        OnInvoke = onInvoke;
        Parameters = parameters;
    }

    public static FunctionDefinition? GetFunctionDefinition(string name)
    {
        try
        {
            return DefinedFunctions.First(fun => fun.Identifier == name);
        }
        catch (Exception)
        {
            return null;
        }
    }
}