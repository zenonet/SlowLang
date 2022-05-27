﻿using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter;

/// <summary>
/// Provides some static methods useful for parsing
/// </summary>
public static class ParsingUtility
{
    /// <summary>
    /// Tracks nested braces and finds the first closing one that wasn't opened in code
    /// </summary>
    /// <param name="input">The code to look through</param>
    /// <param name="openingBracket">The TokenType which starts a new brace pair</param>
    /// <param name="closingBracket">The TokenType which ends a brace pair</param>
    /// <param name="logger">A logger to use when logging errors</param>
    /// <returns>Everything between the braces</returns>
    public static TokenList FindBetweenBraces(TokenList input, TokenType openingBracket, TokenType closingBracket, ILogger logger)
    {
        int openedBraces = 1;
        TokenList codeBlock = null;

        //Tracks all curly braces until it finds the matching one
        for (var i = 0; i < input.List.Count; i++)
        {
            if (input[i].Type == openingBracket)
                openedBraces++;

            if (input[i].Type is TokenType.ClosingBrace)
                openedBraces--;

            if (openedBraces == 0)
            {
                codeBlock = input.List.GetRange(0, i).AsTokenList();
                break;
            }
        }

        //If there was no matching curly brace, throw Compiler error
        if (codeBlock is null)
        {
            logger.LogError("Invalid brace-pattern");
        }

        return codeBlock;
    }


    /// <summary>
    /// Converts a collection of Tokens into a TokenList
    /// </summary>
    /// <param name="tokens">The collection of tokens</param>
    /// <returns>The TokenList</returns>
    public static TokenList AsTokenList(this IEnumerable<Token> tokens) => new (tokens);
}
