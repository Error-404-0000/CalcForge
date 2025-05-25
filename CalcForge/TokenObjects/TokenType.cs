using CalcForge.Attributes;

namespace CalcForge.TokenObjects;
public enum TokenType
{
    None,
    [ValueContainer(true, "Pow", "Add", "Subtract", "Multiply", "Divide", "Abs", "Floor", "Ceiling", "Round", "Max", "Min", "Sqrt", "Log", "Log10", "Exp", "Sin", "Cos", "Tan", "Truncate","Print")]
    Function,
    Number,
    [ValueContainer(true,"+","-","/","*","^","&","<",">","|","%")]
    Operation,
    [ValueContainer(false, "(")]
    ParenthesisOpen,
    [ValueContainer(false, ")")]
    ParenthesisClose,
    [ValueContainer(true, "eq","neq","gt","lt","if","else")]
    Conditions,
    [ValueContainer(false, "{")]

    CurlybracketStart,
    [ValueContainer(false, "}")]

    CurlybracketEnds

}
