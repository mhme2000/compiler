using CompilerApp;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting compiler...");
        var syntactic = new Syntactic();
        syntactic.Check();
    }
}