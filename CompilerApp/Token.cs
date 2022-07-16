namespace CompilerApp;

public class Token
{
    public TypeToken Type { get; init; }
    public string Content { get; init; } = null!;

    public enum TypeToken
    {
        Identifier = 1,
        Operator = 2,
        Bundler = 3,
        LineBreak = 4,
        EndOfChain = 5,
    }
}