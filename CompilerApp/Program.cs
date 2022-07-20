namespace CompilerApp;
public static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting compiler...");
        var fileName = args.Length > 0 ? args[0] : @"input.txt";
        var syntactic = new Syntactic(fileName);
        syntactic.CheckSyntax();
    }
}