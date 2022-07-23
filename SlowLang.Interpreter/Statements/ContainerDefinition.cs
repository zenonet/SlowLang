using Microsoft.Extensions.Logging;
using SlowLang.Engine;
using SlowLang.Engine.Statements;
using SlowLang.Engine.Statements.StatementRegistrations;
using SlowLang.Engine.Tokens;
using SlowLang.Engine.Values;

namespace SlowLang.Interpreter.Statements;

public class ContainerDefinition : Statement
{
    public static readonly List<ContainerDefinition> ContainerDefinitions = new();


    public static void OnInitialize()
    {
        StatementRegistration.Create<ContainerDefinition>(
            x => x.Peek().RawContent == "container",
            TokenType.Keyword, TokenType.Keyword, TokenType.OpeningCurlyBrace
        ).Register();
    }

    public string Name;
    public Dictionary<string, Value> Fields;

    protected override void OnParse(ref TokenList list)
    {
        //Remove container keyword
        list.Pop();

        //Get container definition name
        Name = list.Pop().RawContent;

        //Remove opening curly brace
        list.Pop();

        //Get the tokens between the braces
        TokenList? betweenBrackets = list.FindBetweenBraces(TokenType.OpeningCurlyBrace, TokenType.ClosingCurlyBrace, Logger);

        //Null-check
        if (betweenBrackets is null)
            LoggingManager.LogError("Invalid brace pattern");

        Fields = GetFields(ref betweenBrackets);
        
        //Remove the closing brace
        list.Pop();
        
        ContainerDefinitions.Add(this);
        ;
    }

    private static Dictionary<string, Value> GetFields(ref TokenList list)
    {
        Dictionary<string, Value> fields = new();
        while (list.List.Count > 0)
        {
            Type? type = Value.ParseTypeKeyword(list.Peek());

            if (type is null)
                LoggingManager.LogError($"Unable to parse {list.Peek()}");

            list.Pop();

            string fieldName = list.Pop().RawContent;

            Value? value;

            //If there is a default value
            if (list.Peek().Type is TokenType.Equals)
            {
                //Remove the equals
                list.Pop();

                //Parse the default value
                value = Value.Parse(list);

                //Null-check
                if (value is null)
                    LoggingManager.LogError($"Unable to parse {list.Peek().RawContent} (Only constants are allow there)");

                //If the 2 types don't match
                if (type != value!.ConvertImplicitly(type!).GetType())
                    //Throw an error
                    LoggingManager.LogError($"Default value {list.Peek().RawContent} has to be of the same type as the field itself");

                //Remove the default value constant
                list.Pop();
            }
            else
            {
                value = (Value?) Activator.CreateInstance(type!);
                if (value is null)
                    Logger.LogCritical("Activator couldn't create an instance of {name}", type.FullName);
            }


            //Trim the optional semicolon
            list.TrimStart(TokenType.Semicolon);

            fields.Add(fieldName, value!);
        }

        return fields;
    }
}