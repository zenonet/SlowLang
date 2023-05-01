using System.Diagnostics.CodeAnalysis;
using SlowLang.Engine;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;
using SlowLang.Interpreter.Statements.Operators;
using Zenonet.Utility;

namespace SlowLang.Interpreter.Values;

public class SlowBool : Value
{
    [MainDataField]
    public bool Value;

    
    public static string GetKeyword() => "bool";
    public SlowBool(bool value)
    {
        Value = value;
    }

    public override Value ApplyOperator(Subtype<Operator> @operator, Value rightOperand)
    {
        if (@operator.Type == typeof(EqualityOperator))
        {
            if(rightOperand is SlowInt rightInt)
            {
                return new SlowBool(Value  == (rightInt.Value != 1));
            }

        }
        return null;
    }

    public static bool TryParse(ref TokenList tokenList, [MaybeNullWhen(false)] out Value val)
    {
        if (tokenList.Peek().Type is TokenType.Bool)
        {
            val = new SlowBool(bool.Parse(tokenList.Pop().RawContent));
            return true;
        }

        val = null;
        return false;
    }

    public override bool TryConvertImplicitly(Type targetType, out Value output)
    {
        if (targetType == typeof(SlowInt))
        {
            output = new SlowInt(Value ? 1 : 0);
            return true;
        }
        if (targetType == typeof(SlowString))
        {
            output = new SlowString(Value.ToString());
            return true;
        }
        
        output = null!;
        return false;
    }
}