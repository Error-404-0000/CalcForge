public enum TokenOperation
{
    None = 0,
    [ValueContainer(false,"+")]
    AddOperation,
    [ValueContainer(false,"-")]
    SubtractOperation,
    [ValueContainer(false, "*")]
    MultiplyOperation,
    [ValueContainer(false, "/")]
    DivideOperation,
    [ValueContainer(false, "^")]
    PowerOperation,
    [ValueContainer(false,"<")]
    ShiftLeftOperation,
    [ValueContainer(false, ">")]
    ShiftRightOperation,
    [ValueContainer(false, "|")]
    OROperation,
    [ValueContainer(false, "&")]
    ANDOperation,
    [ValueContainer(false, "%")]
    Reminder,
    [ValueContainer(false, "Pow")]
    [FuncMata(typeof(Math), nameof(Math.Pow))]
    Pow,

    [ValueContainer(false, "Abs")]
    [FuncMata(typeof(Math), nameof(Math.Abs))]
    Abs,

    [ValueContainer(false, "Sin")]
    [FuncMata(typeof(Math), nameof(Math.Sin))]
    Sin,

    [ValueContainer(false, "Cos")]
    [FuncMata(typeof(Math), nameof(Math.Cos))]
    Cos,

    [ValueContainer(false, "Tan")]
    [FuncMata(typeof(Math), nameof(Math.Tan))]
    Tan,

    [ValueContainer(false, "Sqrt")]
    [FuncMata(typeof(Math), nameof(Math.Sqrt))]
    Sqrt,

    [ValueContainer(false, "Log")]
    [FuncMata(typeof(Math), nameof(Math.Log))]
    Log,
    [ValueContainer(false, "Print")]
    [FuncMata(typeof(Math), nameof(Math.Print))]
    Print,
    [ValueContainer(false, "Log10")]
    [FuncMata(typeof(Math), nameof(Math.Log10))]
    Log10,

    [ValueContainer(false, "Exp")]
    [FuncMata(typeof(Math), nameof(Math.Exp))]
    Exp,

    [ValueContainer(false, "Floor")]
    [FuncMata(typeof(Math), nameof(Math.Floor))]
    Floor,

    [ValueContainer(false, "Ceiling")]
    [FuncMata(typeof(Math), nameof(Math.Ceiling))]
    Ceiling,

    [ValueContainer(false, "Round")]
    [FuncMata(typeof(Math), nameof(Math.Round))]
    Round,

    [ValueContainer(false, "Truncate")]
    [FuncMata(typeof(Math), nameof(Math.Truncate))]
    Truncate,

    [ValueContainer(false, "Max")]
    [FuncMata(typeof(Math), nameof(Math.Max))]
    Max,

    [ValueContainer(false, "Min")]
    [FuncMata(typeof(Math), nameof(Math.Min))]
    Min,

    [ValueContainer(false, "Add")]
    [FuncMata(typeof(Math), nameof(Math.Add))]
    Add,

    [ValueContainer(false, "Subtract")]
    [FuncMata(typeof(Math), nameof(Math.Subtract))]
    Subtract,

    [ValueContainer(false, "Multiply")]
    [FuncMata(typeof(Math), nameof(Math.Multiply))]
    Multiply,

    [ValueContainer(false, "Divide")]
    [FuncMata(typeof(Math), nameof(Math.Divide))]
    Divide
}
