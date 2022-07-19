namespace CompilerApp;
public class Token
{
    public string Content { get; init; } = null!;
    public EnumTypeToken Type { get; init; }
}