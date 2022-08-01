using System.Reflection;
using SlowLang.Engine;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Statements.StatementRegistrations;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;
using SlowLang.Interpreter.Values;

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
        Value leftValue = LeftOperand.Execute();
        Value rightValue = RightOperand.Execute();

        if (leftValue.GetType() == rightValue.GetType())
        {
            //Get the mainDataField
            FieldInfo? mainDataField = leftValue
                .GetType()
                .GetFields()
                .FirstOrDefault(x =>
                    x.GetCustomAttribute(typeof(MainDataFieldAttribute)) != null
                );

            //If it isn't defined, throw an error
            if (mainDataField == null)
            {
                LoggingManager.LogError(
                    $"Can't use equality operator on 2 {leftValue.GetType()}s"
                );

                return null!;
            }

            return new SlowBool(
                mainDataField.GetValue(leftValue)!
                    .Equals( //The == operator doesn't work here because
                             //it would call object.Equals and not the overriden object.Equals
                        mainDataField.GetValue(rightValue)!)
            );
        }

        return base.Execute();
    }
}