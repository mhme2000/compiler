public class Token
{
    public TypeToken Type { get; set; }
    public string Content { get; set; }

    public enum TypeToken
    {
        Identifier = 1,
        Operator = 2,
        Bundler = 3,
    }
}