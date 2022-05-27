using System.Diagnostics.CodeAnalysis;
using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Values;

public class SlowInt : Value
{
    public int Value;

    public SlowInt(int value)
    {
        Value = value;
    }
    
    public static bool TryParse(ref TokenList tokenList, [MaybeNullWhen(false)]out Value val)
    {
        if (tokenList.Peek().Type is TokenType.Int)
        {
            val = new SlowInt(int.Parse(tokenList.Pop().RawContent));
            return true;
        }

        val = null;
        return false;
    }
}