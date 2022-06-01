using System.Reflection;
using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;
using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter.Statements;

/// <summary>
/// Represents a single statement in a SlowLang script
/// </summary>
public abstract class Statement
{
    protected static readonly ILogger Logger = Interpreter.LoggerFactory.CreateLogger("SlowLang.Statements");


    private static bool isInitialized = false;

    /// <summary>
    /// If this returns true, the Statement will have to cut itself from the token list
    /// </summary>
    protected virtual bool CutTokensManually() => false;

    public virtual Value Execute() => SlowVoid.I;

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


    public static Statement Parse(ref TokenList list)
    {
        //If the Parser wasn't initialized yet, do it now
        if (!isInitialized)
            Initialize();

        Statement? statement = null;
        foreach (StatementRegistration registration in Registrations)
        {
            //Check if the matching sequence in the current registration would fit int o list.List
            if (registration.Match.Length > list.List.Count)
                continue;


            //Iterate through all elements and check if the TokenType matches
            for (int i = 0; i < registration.Match.Length; i++)
            {
                //If it doesn't match:
                if (list.List[i].Type != registration.Match[i])
                    goto next; //Jump over all of the parsing stuff and continue with the next StatementRegistration
            }


            //If the StatementRegistration has a customParser defined:
            if (registration.CustomParser != null)
            {
                //Execute the custom Parser
                bool result = registration.CustomParser.Invoke(list);
                if(!result) //And if it couldn't parse the TokenList, jump over the parsing stuff and continue with the next StatementRegistration
                    goto next;
            }
            
            
            //Instantiate the matching subclass
            statement = (Activator.CreateInstance(registration.Statement) as Statement)!;
            

            //Remove the tokens that match from the token list
            if(!statement.CutTokensManually())
                list.List.RemoveRange(0, registration.Match.Length);
                
            //Invoke its OnParse() callback
            statement.OnParse(ref list);
            break;

            next: ;
        }

        if (statement != null)
            return statement;
        
        
        Interpreter.LogError($"Couldn't parse {list.Peek()}");
        return null!;
    }
    
    public static Statement[] ParseMultiple(TokenList list)
    {
        //If the Parser wasn't initialized yet, do it now
        if (!isInitialized)
            Initialize();

        List<Statement> statements = new();
        while (list.List.Count > 0)
        {
            statements.Add(Parse(ref list));
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
        
        //Iterate through all types which inherit from Statement
        foreach (Type type in ParsingUtility.GetAllInheritors(typeof(Statement)))
        {
            
            //Ignore abstract Statement inheritors
            if(type.IsAbstract)
                continue;
            
            //Get a static method called OnInitialize inside of them
            MethodInfo? initMethod = type.GetMethod("OnInitialize");

            //If it exists, call it
            if (initMethod is null)
                Logger.LogWarning("Statement subclass " + type.Name + " doesn't have an Initialize method");
            else
                initMethod.Invoke(null, null);
        }

        //Sort registrations
        Registrations.Sort(((x, y) => y.Match.Length - x.Match.Length));
        
        isInitialized = true;
    }


    public override string ToString()
    {
        return this.GetType().Name;
    }
}