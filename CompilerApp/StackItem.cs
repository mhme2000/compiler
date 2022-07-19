namespace CompilerApp;
[Serializable]
public class StackItem 
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Level { get; set; }
    public EnumTypeToken Type { get; set; }
    
}