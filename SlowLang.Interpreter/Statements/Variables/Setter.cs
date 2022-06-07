using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Statements.StatementRegistrations;
using SlowLang.Interpreter.Tokens;
using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter.Statements.Variables;

public class Setter : Statement
{
    private Statement value;
    private string varName;

    public static void OnInitialize()
    {
        Logger.LogInformation("Now initializing Setter");
        Statement.Register(StatementRegistration.Create<Setter>(
            TokenType.Keyword,
            TokenType.Equals
        ));
    }


    protected override bool CutTokensManually() => true;


    protected override void OnParse(ref TokenList list)
    {
        //Get the variable name
        varName = list.Pop().RawContent;
        //Remove the equals
        list.Pop();

        value = Statement.Parse(ref list);
        
        if (list.Peek() != null! && list.Peek().Type is TokenType.Semicolon)
            list.Pop();
        else
            Logger.LogCritical("Missing semicolon after setter statement");
        
        //Register the variable but don't give it any value yet
        Value.Variables.Add(varName, SlowVoid.I);
    }

    public override Value Execute()
    {
        Value val = value.Execute();
        
        if(val == SlowVoid.I)
            Interpreter.LogError($"{value} doesn't have a return value", LineNumber);
        
        Value.Variables[varName] = val;
        return SlowVoid.I;
    }
}