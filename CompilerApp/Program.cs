public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Começou");
        Syntax.Check();
    }
    // private static void Main(string[] args)
    // {
    //     Console.WriteLine("Começou");
    //     var tokenizer = new Lex("input.txt");
    //     Token token = null;
    //     do
    //     {
    //         token = tokenizer.nextToken();
    //         if (token != null)
    //         {
    //             Console.WriteLine(token.Content + "-" + token.Type);
    //         }
    //     } while (token != null);
    // }
}