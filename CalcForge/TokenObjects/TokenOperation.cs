namespace CalcForge.TokenObjects;

using CalcForge.Attributes;
using CalcForge.Compiler;

public enum TokenOperation
{

    None = 0,
    #region Operations
    [Compliable("add {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "add", 0x02)]
    [ValueContainer(false, "+")]
    AddOperation,

    [Compliable("sub {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "sub", 0x03)]
    [ValueContainer(false, "-")]
    SubtractOperation,

    [Compliable("mul {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "mul", 0x04)]
    [ValueContainer(false, "*")]
    MultiplyOperation,

    [Compliable("mov rdx,{left}\ndiv {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "div", 0x05)]
    [ValueContainer(false, "/")]
    DivideOperation,

    [Compliable("pow {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "pow", 0x06)]
    [ValueContainer(false, "^")]
    PowerOperation,

    [Compliable("shl {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "shl", 0x07)]
    [ValueContainer(false, "<")]
    ShiftLeftOperation,

    [Compliable("shr {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "shr", 0x08)]
    [ValueContainer(false, ">")]
    ShiftRightOperation,

    [Compliable("or {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "or", 0x09)]
    [ValueContainer(false, "|")]
    OROperation,

    [Compliable("and {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "and", 0x0A)]
    [ValueContainer(false, "&")]
    ANDOperation,

    [Compliable("mod {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "mod", 0x0B)]
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
    Divide,
    #endregion


    #region Conditions

    [ValueContainer(false,"eq")]
    eq,
    [ValueContainer(false, "neq")]
    neq,
    [ValueContainer(false, "lt")]
    lt,
    [ValueContainer(false, "gt")]
    gt,
    [ValueContainer(false, "if")]
    If,
    [ValueContainer(false, "else")]
    @else,




    #endregion
}
