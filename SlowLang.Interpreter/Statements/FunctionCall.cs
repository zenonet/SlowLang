using Microsoft.Extensions.Logging;
using SlowLang.Engine;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Statements.StatementRegistrations;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;


namespace SlowLang.Interpreter.Statements;

/// <summary>
/// Represents a call to a function in a SlowLang script
/// </summary>
public class FunctionCall : Statement
{
    public FunctionDefinition? Reference;

    public Statement[] Parameters = null!;

    public FunctionCall(FunctionDefinition? reference, Statement[] parameters)
    {
        Reference = reference;
        Parameters = parameters;
    }

    public FunctionCall()
    {
    }

    public static void OnInitialize()
    {
        Logger.LogInformation("Now initializing FunctionCall");
        StatementRegistration.Create<FunctionCall>(TokenType.Keyword, TokenType.OpeningBrace).Register();
    }

    protected override bool CutTokensManually() => true;

    protected override void OnParse(ref TokenList list)
    {
        //Find the FunctionDefinition this FunctionCall belongs to
        string name = list.Pop().RawContent;
        Reference = FunctionDefinition.GetFunctionDefinition(name);
        if (Reference is null)
        {
            LoggingManager.LogError($"There is no function called {name} defined", LineNumber);
        }

        //Cut the opening bracket
        list.Pop();

        //Find everything between the braces
        TokenList? betweenBraces = ParsingUtility.FindBetweenBraces(list, TokenType.OpeningBrace, TokenType.ClosingBrace, Logger);

        if (betweenBraces is null)
            LoggingManager.LogError("Closing brace missing", LineNumber);

        //Remove the parameter list
        list.RemoveRange(..betweenBraces!.List.Count);
        //Remove the closing brace
        list.Pop();

        List<Statement> parameters = new();
        while (betweenBraces.List.Count > 0)
        {
            //Parse the parameter
            parameters.Add(Parse(ref betweenBraces));

            if (betweenBraces.StartsWith(TokenType.Comma))
                betweenBraces.Pop();
        }

        this.Parameters = parameters.ToArray();
    }

    public override Value Execute()
    {
        List<Value> executedParameters = new();

        foreach (Statement parameter in Parameters)
        {
            Value v = parameter.Execute();

            if (v.IsVoid)
                LoggingManager.LogError($"{parameter} doesn't have a return value", LineNumber);

            executedParameters.Add(v);
        }

        return Reference?.OnInvoke.Invoke(executedParameters.ToArray())!;
    }
}