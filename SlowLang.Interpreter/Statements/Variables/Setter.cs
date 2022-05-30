using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Statements.Variables;

public class Setter : Statement
{
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
        string varName = list.Pop().RawContent;
        //Remove the equals
        list.Pop();

        Statement.ParseMultiple(list);

    }
}