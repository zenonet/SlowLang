﻿namespace SlowLang.Interpreter.Tokens;

public enum TokenType
{
    String,
    Keyword,
    Int,
    Float,
    Bool,
    
    OpeningCurlyBrace,
    ClosingCurlyBrace,
    
    OpeningBrace,
    ClosingBrace,
    
    Equals,
    
    Semicolon,
    Comma,
}