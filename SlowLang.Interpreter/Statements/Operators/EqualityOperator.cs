using SlowLang.Engine.Statements;
using SlowLang.Engine.Statements.StatementRegistrations;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;

namespace SlowLang.Interpreter.Statements.Operators;

public class EqualityOperator : Operator
{
    public static void OnInitialize()
    {
        StatementExtensionRegistration
            .CreateStatementExtensionRegistration<Statement, EqualityOperator>(
                TokenType.Equals,
                TokenType.Equals
            )
            .Register();
    }

    protected override bool CutTokensManually() => true;

    public override void OnParse(ref TokenList list, Statement statement)
    {
        LeftOperand = statement;
        
        //Remove the 2 equals signs
        list.Pop();
        list.Pop();

        RightOperand = Statement.Parse(ref list);
    }

    public override Value Execute()
    {
        throw new NotImplementedException("EqualityComparison hasn't been implemented yet");
    }
}