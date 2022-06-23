using SlowLang.Engine;
using SlowLang.Engine.Values;

namespace SlowLang.Interpreter.Statements;

public readonly struct FunctionCallContext
{
    public readonly Value[] Parameters;
    public readonly FunctionCall Caller;

    public FunctionCallContext(Value[] parameters, FunctionCall caller)
    {
        Parameters = parameters;
        Caller = caller;
    }

    public void ExpectParameters(params Type[] parameterTypes)
    {
        for (int i = 0; i < parameterTypes.Length; i++)
        {
            if (parameterTypes[i] != Parameters[i].GetType())
            {
                //TODO: Add implicit conversion
                LoggingManager.LogError($"Invalid type of parameter {Caller.Parameters[i]}");
            }
        }
    }
}