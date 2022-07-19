namespace CompilerApp;
public static class Program
{
    private static void Main()
    {
        Console.WriteLine("Starting compiler...");
        var syntactic = new Syntactic("input.txt");
        syntactic.CheckSyntax();
    }
}