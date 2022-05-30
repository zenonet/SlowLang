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

    public Value[] Parameters = null!;

    public FunctionCall(FunctionDefinition? reference, Value[] parameters)
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

        List<Value> parameters = new();
        foreach (TokenList tokenList in betweenBraces.Split(TokenType.Comma))
        {
            parameters.Add(Value.Parse(tokenList)!);
        }

        this.Parameters = parameters.ToArray();
        
        //Remove the rest of the Function call from the TokenList + 1 (The closing bracket) 
        list.List.RemoveRange(0, betweenBraces.List.Count + 1);

        if (list.Peek() != null && list.Peek().Type is TokenType.Semicolon)
            list.Pop();

    }

    public override Value Execute() => 
        Reference?.OnInvoke.Invoke(Parameters)!;
}