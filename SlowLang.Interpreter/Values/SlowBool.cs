using System.Diagnostics.CodeAnalysis;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;

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