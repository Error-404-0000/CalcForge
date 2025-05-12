public enum TokenType
{
    None,
    [ValueContainerAttribute(true, "Pow", "Add", "Subtract", "Multiply", "Divide", "Abs", "Floor", "Ceiling", "Round", "Max", "Min", "Sqrt", "Log", "Log10", "Exp", "Sin", "Cos", "Tan", "Truncate","Print")]
    Function,
    Number,
    [ValueContainerAttribute(true,"+","-","/","*","^","&","<",">","|","%")]
    Operation,
    [ValueContainerAttribute(false, "(")]
    ParenthesisOpen,
    [ValueContainerAttribute(false, ")")]
    ParenthesisClose
}
