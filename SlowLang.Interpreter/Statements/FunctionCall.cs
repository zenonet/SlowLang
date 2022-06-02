using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;
using SlowLang.Interpreter.Values;

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
    public FunctionCall(){}

    public static void OnInitialize()
    {
        Logger.LogInformation("Now initializing FunctionCall");
        Statement.Register(StatementRegistration.Create<FunctionCall>(
            TokenType.Keyword,
            TokenType.OpeningBrace
        ));
    }
    
    protected override bool CutTokensManually() => true;

    protected override void OnParse(ref TokenList list)
    {
        //Find the FunctionDefinition this FunctionCall belongs to
        Reference = FunctionDefinition.GetFunctionDefinition(list.Pop().RawContent);
        
        //Cut the opening bracket
        list.Pop();
        
        //Find everything between the braces
        TokenList betweenBraces = ParsingUtility.FindBetweenBraces(list, TokenType.OpeningBrace, TokenType.ClosingBrace, Logger);

        //Remove the parameter list
        list.RemoveRange(..betweenBraces.List.Count);
        //Remove the closing brace
        list.Pop();
        
        List<Statement> parameters = new();
        while (betweenBraces.List.Count > 0)   
        {
            //Parse the parameter
            parameters.Add(Parse(ref betweenBraces));

            if (betweenBraces.Peek() != null && betweenBraces.Peek().Type is TokenType.Comma)
                betweenBraces.Pop();
        }
        this.Parameters = parameters.ToArray();
        

        if (list.Peek() != null! && list.Peek().Type is TokenType.Semicolon)
            list.Pop();
    }

    public override Value Execute()
    {
        List<Value> executedParameters = new();
        
        foreach (Statement parameter in Parameters)
        {
            Value v = parameter.Execute();
            
            if(v == SlowVoid.I)
                Interpreter.LogError($"{parameter} doesn't have a return value");

            executedParameters.Add(v);
        }

        return Reference?.OnInvoke.Invoke(executedParameters.ToArray())!;

    }
}