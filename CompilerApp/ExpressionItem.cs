namespace CompilerApp;
public class ExpressionItem
{
    public Guid Id { get; init; }
    public string Value { get; init; } = string.Empty;
    public int Level { get; set; }
    public EnumTypeToken Type { get; init; }
}