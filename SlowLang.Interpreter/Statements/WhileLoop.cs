using SlowLang.Engine;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Statements.StatementRegistrations;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;
using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter.Statements;

public class WhileLoop : Statement
{
    private Statement? condition;
    private Statement[]? codeBlock;

    public static void OnInitialize()
    {
        StatementRegistration.Create<WhileLoop>(
                tokenList => tokenList.Peek().RawContent == "while",
                TokenType.Keyword, TokenType.OpeningBrace
            )
            .Register();
    }


    protected override bool CutTokensManually() => true;

    protected override bool OnParse(ref TokenList list)
    {
        list.Pop(); //Remove the 'while' keyword
        list.Pop(); //Remove the opening bracket

        //Get the condition
        TokenList? rawCondition =
            ParsingUtility.FindBetweenBraces(list, TokenType.OpeningBrace, TokenType.ClosingBrace, Logger);

        if (rawCondition is null)
        {
            LoggingManager.LogError("Missing closing brace");
            throw new Exception();
        }

        //Remove the condition from the TokenList
        list.RemoveRange(..rawCondition.List.Count);

        condition = Statement.Parse(ref rawCondition);

        list.Pop(); //Remove closing brace

        //Check if the next token is an opening curly brace. If not, throw an error
        if (list.Peek().Type != TokenType.OpeningCurlyBrace)
            LoggingManager.LogError("Unexpected token " + list.Peek().RawContent, LineNumber);

        list.Pop(); //Remove opening curly brace

        TokenList? rawCodeBlock =
            list.FindBetweenBraces(TokenType.OpeningCurlyBrace, TokenType.ClosingCurlyBrace, Logger);

        //Error handling
        if (rawCodeBlock is null)
        {
            LoggingManager.LogError("Invalid curly brace pattern", LineNumber);
            throw new Exception();
        }

        //Remove the CodeBlock from the TokenList
        list.RemoveRange(..rawCodeBlock.List.Count);

        //Remove the closing curly brace
        list.Pop();


        codeBlock = ParseMultiple(rawCodeBlock);

        return true;
    }

    public override Value Execute()
    {
        while (condition!.Execute().ConvertImplicitly<SlowBool>().Value)
        {
            codeBlock?.Execute();
        }

        return SlowVoid.I;
    }
}