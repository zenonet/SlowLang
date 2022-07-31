using System.Diagnostics.CodeAnalysis;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;

namespace SlowLang.Interpreter.Values;

public class SlowInt : Value
{
    [MainDataField]
    public int Value;

    public SlowInt(int value)
    {
        Value = value;
    }

    public SlowInt()
    {
    }

    public static string GetKeyword() => "int";

    public static bool TryParse(ref TokenList tokenList, [MaybeNullWhen(false)] out Value val)
    {
        if (tokenList.Peek().Type is TokenType.Int)
        {
            val = new SlowInt(int.Parse(tokenList.Pop().RawContent));
            return true;
        }

        val = null;
        return false;
    }

    public override bool TryConvertImplicitly(Type type, out Value output)
    {
        if (type == typeof(SlowBool))
        {
            output = Value > 0 ? new SlowBool(true) : new SlowBool(false);
            return true;
        }
        if (type == typeof(SlowString))
        {
            output = new SlowString(Value.ToString());
            return true;
        }


        output = null!;
        return false;
    }
}