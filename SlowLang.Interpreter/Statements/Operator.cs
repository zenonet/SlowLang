using SlowLang.Engine;
using SlowLang.Engine.Statements;

namespace SlowLang.Interpreter.Statements;

public abstract class Operator : StatementExtension
{
    public Statement LeftOperand = null!;
    public Statement RightOperand = null!;

    protected Operator(Statement leftOperand, Statement rightOperand)
    {
        LeftOperand = leftOperand;
        RightOperand = rightOperand;
    }

    protected Operator()
    {
        
    }
}