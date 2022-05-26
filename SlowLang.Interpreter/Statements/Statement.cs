using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements;

/// <summary>
/// Represents a single statement in a SlowLang script
/// </summary>
public abstract class Statement
{
    protected static readonly ILogger Logger = Interpreter.LoggerFactory.CreateLogger("SusLang.Statements");


    private static bool isInitialized = false;


    protected virtual void Execute()
    {
    }

    protected virtual void OnParse(ref TokenList list)
    {
    }

    private static readonly List<StatementRegistration> Registrations = new();

    protected static void Register(StatementRegistration registration)
    {
        //Check if the registration is valid
        if (registration.Statement.BaseType == typeof(Statement))
        {
            Registrations.Add(registration);
            return;
        }

        Logger.LogWarning("A StatementRegistration exists, which doesn't refer to a subclass of Statement");
    }


    public static Statement[] Parse(TokenList list)
    {
        //If the Parser wasn't initialized yet, do it now
        if (!isInitialized)
            Initialize();

        List<Statement> statements = new();
        while (list.List.Count > 0)
        {
            bool parsedSomething = false;
            foreach (StatementRegistration registration in Registrations)
            {
                //Check if the matching sequence in the current registration would fit int o list.List
                if(registration.Match.Length > list.List.Count)
                    continue;
                

                //Iterate through all elements and check if the TokenType matches
                for (int i = 0; i < registration.Match.Length; i++)
                {
                    //If it doesn't match, jump to the next block and go to the next element in the foreach statement
                    if (list.List[i].Type != registration.Match[i])
                        goto next;
                }

                //Only if the complete match list of the current registration matches:
                statements.Add((Activator.CreateInstance(registration.Statement) as Statement)!);
                list.List.RemoveRange(0, registration.Match.Length);
                parsedSomething = true;
                
                break;

                next: ;
            }

            if (!parsedSomething)
            {
                Logger.LogCritical("Couldn't parse {stmt}", list.List[0].ToString());
                break;
            }
        }

        return statements.ToArray();
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
            if (type == typeof(Statement))
                continue;

            //Get a static method called OnInitialize inside of them
            MethodInfo? initMethod = type.GetMethod("OnInitialize");

            //If it exists, call it
            if (initMethod is null)
                Logger.LogWarning("Statement subclass " + type.Name + " doesn't have an Initialize method");
            else
                initMethod.Invoke(null, null);
        }

        isInitialized = true;
    }


    public override string ToString()
    {
        return this.GetType().Name;
    }
}