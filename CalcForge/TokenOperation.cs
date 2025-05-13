namespace CalcForge;
using CalcForge.Compiler;

public enum TokenOperation
{
    None = 0,

    [Compliable("add {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "add", 0x02)]
    [ValueContainerAttribute(false, "+")]
    AddOperation,

    [Compliable("sub {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "sub", 0x03)]
    [ValueContainerAttribute(false, "-")]
    SubtractOperation,

    [Compliable("mul {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "mul", 0x04)]
    [ValueContainerAttribute(false, "*")]
    MultiplyOperation,

    [Compliable("div {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "div", 0x05)]
    [ValueContainerAttribute(false, "/")]
    DivideOperation,

    [Compliable("pow {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "pow", 0x06)]
    [ValueContainerAttribute(false, "^")]
    PowerOperation,

    [Compliable("shl {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "shl", 0x07)]
    [ValueContainerAttribute(false, "<")]
    ShiftLeftOperation,

    [Compliable("shr {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "shr", 0x08)]
    [ValueContainerAttribute(false, ">")]
    ShiftRightOperation,

    [Compliable("or {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "or", 0x09)]
    [ValueContainerAttribute(false, "|")]
    OROperation,

    [Compliable("and {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "and", 0x0A)]
    [ValueContainerAttribute(false, "&")]
    ANDOperation,

    [Compliable("mod {left} {right}", CompileNeeds.Left, CompileNeeds.Right)]
    [Emit("MASM64", "mod", 0x0B)]
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
