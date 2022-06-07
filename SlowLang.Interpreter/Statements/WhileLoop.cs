using SlowLang.Interpreter.Statements.StatementRegistrations;
using SlowLang.Interpreter.Tokens;
using SlowLang.Interpreter.Values;

namespace SlowLang.Interpreter.Statements;

public class WhileLoop : Statement
{

    
    private Statement? condition;
    private Statement[]? codeBlock;
    
    public static void OnInitialize()
    {
        Statement.Register(StatementRegistration.Create<WhileLoop>(
            tokenList => tokenList.Peek().RawContent == "while",
            TokenType.Keyword,
            TokenType.OpeningBrace
        ));
    }


    protected override bool CutTokensManually() => true;

    protected override void OnParse(ref TokenList list)
    {
        list.Pop(); //Remove the 'while' keyword
        list.Pop(); //Remove the opening bracket

        //Get the condition
        TokenList rawCondition =
            ParsingUtility.FindBetweenBraces(list, TokenType.OpeningBrace, TokenType.ClosingBrace, Logger);
        
        
        //Remove the condition from the TokenList
        list.RemoveRange(..rawCondition.List.Count);
        
        condition = Statement.Parse(ref rawCondition);

        list.Pop(); //Remove closing brace
        
        //Check if the next token is an opening curly brace. If not, throw an error
        if(list.Peek().Type != TokenType.OpeningCurlyBrace)
            Interpreter.LogError("Unexpected token " + list.Peek().RawContent, LineNumber);

        list.Pop(); //Remove opening curly brace
        
        TokenList? rawCodeBlock =
            ParsingUtility.FindBetweenBraces(list, TokenType.OpeningCurlyBrace, TokenType.ClosingCurlyBrace, Logger);

        //Error handling
        if(rawCodeBlock is null)
            Interpreter.LogError("Invalid curly brace pattern", LineNumber);
        
        //Remove the CodeBlock from the TokenList
        list.RemoveRange(..rawCodeBlock.List.Count );

        //Remove the closing curly brace
        list.Pop();
        
        
        codeBlock = ParseMultiple(rawCodeBlock);

        ;
    }

    public override Value Execute()
    {
        while (((SlowBool) condition!.Execute()).Value)
        {
            codeBlock?.Execute();
        }
        
        return SlowVoid.I;
    }
}