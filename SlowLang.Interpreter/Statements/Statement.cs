using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements;

/// <summary>
/// Represents a single statement in a SlowLang script
/// </summary>
public abstract class Statement
{
    protected static readonly ILogger Logger = Interpreter.LoggerFactory.CreateLogger("SusLang.Statements");

    protected virtual void Execute()
    {
    }

    protected virtual void OnParse()
    {
    }

    protected static readonly List<StatementRegistration> Registrations = new();

    public static void Parse(TokenList list)
    {
        while (list.List.Count > 0)
        {
            foreach (StatementRegistration registration in Registrations)
            {
            }
        }
    }

    /// <summary>
    /// Calls the static OnInitialize() method on every inheritor of Statement
    /// </summary>
    public static void Initialize()
    {
        Assembly? asm = Assembly.GetAssembly(typeof(Statement));
        if (asm is null)
        {
            Logger.LogCritical("Couldn't get the current Assembly");
            return;
        }

        //Thanks to 'Yahoo Serious' answer here: https://stackoverflow.com/questions/857705/get-all-derived-types-of-a-type
        
        //Iterate through all types which inherit from Statement
        foreach (
            Type type in from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
            from assemblyType in domainAssembly.GetTypes()
            where typeof(Statement).IsAssignableFrom(assemblyType)
            select assemblyType)
        {
            
            //Ignore the Statement class itself
            if(type == typeof(Statement))
                continue;
            
            //Get a static method called OnInitialize inside of them
            MethodInfo? initMethod = type.GetMethod("OnInitialize");
            
            //If it exists, call it
            if(initMethod is null)
                Logger.LogWarning("Statement subclass " + type.Name + " doesn't have an Initialize method");
            else
                initMethod.Invoke(null, null);
        }
    }
}