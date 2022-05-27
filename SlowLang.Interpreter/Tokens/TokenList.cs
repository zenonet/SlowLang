using System.Collections;
using SlowLang.Interpreter.Statements;

namespace SlowLang.Interpreter.Tokens;

public class TokenList : IEnumerable<Token>
{
    internal readonly List<Token> List = new();

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

    
    /// <summary>
    /// Splits a TokenList at a separator
    /// </summary>
    /// <param name="separator">The separator to split at</param>
    /// <returns>An array of TokenLists</returns>
    public TokenList[] Split(TokenType separator)
    {
        List<TokenList> tokenLists = new();

        if(List.Count > 0)
            tokenLists.Add(new TokenList());
        
        foreach (Token token in List)
        {
            if (token.Type == separator)
            {
                tokenLists.Add(new TokenList());
                continue;
            }

            tokenLists.Last().Add(token);
        }

        return tokenLists.ToArray();
    }


    #region Contracts

    public IEnumerator<Token> GetEnumerator() => List.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString()
    {
        string output = "";
        List.ForEach(x => output += "\n" + x);
        //Trim the line break at the Start and return
        return output.TrimStart('\n');
    }

    public static implicit operator List<Token>(TokenList tokenList) => tokenList.List;


    public Token this[int index] => List[index];


    internal TokenList(IEnumerable<Token> tokens)
    {
        List = tokens.ToList();
    }

    public TokenList()
    {
    }

    #endregion
}