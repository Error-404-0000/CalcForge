using CalcForge;
namespace CalcForge;
public enum TokenOperation
{
    None = 0,
    [ValueContainerAttribute(false,"+")]
    AddOperation,
    [ValueContainerAttribute(false,"-")]
    SubtractOperation,
    [ValueContainerAttribute(false, "*")]
    MultiplyOperation,
    [ValueContainerAttribute(false, "/")]
    DivideOperation,
    [ValueContainerAttribute(false, "^")]
    PowerOperation,
    [ValueContainerAttribute(false,"<")]
    ShiftLeftOperation,
    [ValueContainerAttribute(false, ">")]
    ShiftRightOperation,
    [ValueContainerAttribute(false, "|")]
    OROperation,
    [ValueContainerAttribute(false, "&")]
    ANDOperation,
    [ValueContainerAttribute(false, "%")]
    Reminder,
    [ValueContainerAttribute(false, "Pow")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Pow))]
    Pow,

    [ValueContainerAttribute(false, "Abs")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Abs))]
    Abs,

    [ValueContainerAttribute(false, "Sin")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Sin))]
    Sin,

    [ValueContainerAttribute(false, "Cos")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Cos))]
    Cos,

    [ValueContainerAttribute(false, "Tan")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Tan))]
    Tan,

    [ValueContainerAttribute(false, "Sqrt")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Sqrt))]
    Sqrt,

    [ValueContainerAttribute(false, "Log")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Log))]
    Log,
    [ValueContainerAttribute(false, "Print")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Print))]
    Print,
    [ValueContainerAttribute(false, "Log10")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Log10))]
    Log10,

    [ValueContainerAttribute(false, "Exp")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Exp))]
    Exp,

    [ValueContainerAttribute(false, "Floor")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Floor))]
    Floor,

    [ValueContainerAttribute(false, "Ceiling")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Ceiling))]
    Ceiling,

    [ValueContainerAttribute(false, "Round")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Round))]
    Round,

    [ValueContainerAttribute(false, "Truncate")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Truncate))]
    Truncate,

    [ValueContainerAttribute(false, "Max")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Max))]
    Max,

    [ValueContainerAttribute(false, "Min")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Min))]
    Min,

    [ValueContainerAttribute(false, "Add")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Add))]
    Add,

    [ValueContainerAttribute(false, "Subtract")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Subtract))]
    Subtract,

    [ValueContainerAttribute(false, "Multiply")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Multiply))]
    Multiply,

    [ValueContainerAttribute(false, "Divide")]
    [FuncMataAttribute(typeof(Math), nameof(Math.Divide))]
    Divide
}
