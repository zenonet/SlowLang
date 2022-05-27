using System.Diagnostics.CodeAnalysis;
using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter.Values;

public class SlowString : Value
{
    public string Value;

    public SlowString(string value)
    {
        Value = value;
    }

    public static bool TryParse(ref TokenList tokenList, [MaybeNullWhen(false)]out Value val)
    {
        if (tokenList.Peek().Type is TokenType.String)
        {
            val = new SlowString(
                tokenList.Pop()     //Get the String token
                    .RawContent     //Get its raw content
                    .TrimStart('"') //Remove the " in front
                    .TrimEnd('"')); //Remove the " at the end
            return true;
        }

        val = null;
        return false;
    }
}