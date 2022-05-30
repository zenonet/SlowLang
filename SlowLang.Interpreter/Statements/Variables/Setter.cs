using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;
using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter.Statements.Variables;

public class Setter : Statement
{
    private Value? value;
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

        value = Value.Parse(list);
        
        if (list.Peek() != null! && list.Peek().Type is TokenType.Semicolon)
            list.Pop();
        else
            Interpreter.LogError("Missing semicolon after setter statement");
    }

    public override Value Execute()
    {
        Value.Variables[varName] = value!;
        return SlowVoid.I;
    }
}