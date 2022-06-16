using System.Diagnostics.CodeAnalysis;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;

namespace SlowLang.Interpreter.Values;

public class SlowBool : Value
{
    public bool Value;

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
}