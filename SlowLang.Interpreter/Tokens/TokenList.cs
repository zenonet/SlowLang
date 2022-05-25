using System.Collections;
using SlowLang.Interpreter.Statements;

namespace SlowLang.Interpreter.Tokens;

public class TokenList : IEnumerable<Token>
{
    internal readonly List<Token> List = new ();

    /// <summary>
    /// Adds a Token to the top of the list
    /// </summary>
    /// <param name="token">The token to add</param>
    public void Add(Token token) => List.Add(token);
    
    /// <summary>
    /// Gets the first token in the list but doesn't remove it
    /// </summary>
    /// <returns>The first token</returns>
    /// <param name="offset">Offsets the index to peek at</param>
    public Token Peek(int offset = 0) => List[offset];

    /// <summary>
    /// Gets the first token and removes it
    /// </summary>
    /// <returns>Offsets the index to get at (Not recommended)</returns>
    public Token Pop(int offset = 0)
    {
        Token first = List[offset];
        List.RemoveAt(offset);
        return first;
    }


    #region Contracts

    public IEnumerator<Token> GetEnumerator() => List.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString()
    {
        string output = "";
        List.ForEach(x => output += "\n" + x);
        return output;
    }

    public static implicit operator List<Token>(TokenList tokenList) => tokenList.List;
    
    #endregion
}