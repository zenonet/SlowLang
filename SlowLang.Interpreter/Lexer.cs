﻿using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using SlowLang.Interpreter.Tokens;

namespace SlowLang.Interpreter;

public static class Lexer
{
    private static readonly ILogger Logger = Interpreter.LoggerFactory.CreateLogger("SlowLang.Lexer");

    private static readonly string NewLine = Environment.NewLine;

    private static readonly Dictionary<string, TokenType> TokenDefinitions = new()
    {
        {"\".*?\"", TokenType.String},
        
        {@"\(", TokenType.OpeningBrace},
        {@"\)", TokenType.ClosingBrace},
        
        {@"\{", TokenType.OpeningCurlyBrace},
        {@"\}", TokenType.ClosingCurlyBrace},
        
        {@"\d+", TokenType.Int},
        {@"\d+.?\d*(?:f|F)", TokenType.Float},
        {@"(?:(?:t|T)(?:rue|RUE))|(?:(?:f|F)(?:alse|ALSE))", TokenType.Bool},
        
        
        {@";", TokenType.Semicolon},
        {@",", TokenType.Comma},
        {@"\s*=\s*", TokenType.Equals},
        
        
        {@"\w*", TokenType.Keyword}, //Needs to be the last one
    };
    
    public static TokenList Lex(string code)
    {
        TokenList tokenList = new TokenList();
        int lineNumber = 1;
        while (code.Length > 0)
        {
            while (code.StartsWith(NewLine))
            {
                code = code[NewLine.Length..];
                lineNumber++;
            }

            //Iterate through all defined tokens
            foreach (KeyValuePair<string,TokenType> tokenDefinition in TokenDefinitions)
            {
                //Try to match them at the start of code
                Match match = Regex.Match(code, "^" + tokenDefinition.Key);
                
                if(!match.Success)
                    continue;
                
                //If the current tokenDefinition got matched successfully, add it to the TokenList
                tokenList.Add(new Token(match.Value, tokenDefinition.Value, lineNumber));
                
                //Remove the matched region from code
                code = code.Substring(match.Value.Length);

                //And break out of the foreach loop
                break;
            }
        }

        Logger.LogInformation("Lexed:" + tokenList);
        return tokenList;
    }
}