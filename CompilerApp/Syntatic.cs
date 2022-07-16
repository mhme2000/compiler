using System.Collections;

namespace CompilerApp;

public class Syntatic
{
    private static readonly Lex Tokenizer = new ("input.txt");
    private static readonly Hashtable HashtableSyntatic = new()
    {
        { new KeyHashtable("E", "+"), null },
        { new KeyHashtable("E", "-"), null },
        { new KeyHashtable("E", "*"), null },
        { new KeyHashtable("E", "/"), null },
        { new KeyHashtable("E", "exp"), "T EL" },
        { new KeyHashtable("E", "["), null },
        { new KeyHashtable("E", "]"), null },
        { new KeyHashtable("E", "^"), null },
        { new KeyHashtable("E", "("), "T EL" },
        { new KeyHashtable("E", ")"), null },
        { new KeyHashtable("E", "id"), "T EL" },
        { new KeyHashtable("E", "$"), null },
        { new KeyHashtable("EL", "+"), "+ T EL" },
        { new KeyHashtable("EL", "-"), "- T EL" },
        { new KeyHashtable("EL", "*"), null },
        { new KeyHashtable("EL", "/"), null },
        { new KeyHashtable("EL", "exp"), null },
        { new KeyHashtable("EL", "["), null },
        { new KeyHashtable("EL", "]"), null },
        { new KeyHashtable("EL", "^"), null },
        { new KeyHashtable("EL", "("), null },
        { new KeyHashtable("EL", ")"), "&" },
        { new KeyHashtable("EL", "id"), null },
        { new KeyHashtable("EL", "$"), "&" },
        { new KeyHashtable("T", "+"), null },
        { new KeyHashtable("T", "-"), null },
        { new KeyHashtable("T", "*"), null },
        { new KeyHashtable("T", "/"), null },
        { new KeyHashtable("T", "exp"), "P TL" },
        { new KeyHashtable("T", "["), null },
        { new KeyHashtable("T", "]"), null },
        { new KeyHashtable("T", "^"), null },
        { new KeyHashtable("T", "("), "P TL" },
        { new KeyHashtable("T", ")"), null },
        { new KeyHashtable("T", "id"), "P TL" },
        { new KeyHashtable("T", "$"), null },
        { new KeyHashtable("TL", "+"), "&" },
        { new KeyHashtable("TL", "-"), "&" },
        { new KeyHashtable("TL", "*"), "* P TL" },
        { new KeyHashtable("TL", "/"), "/ P TL" },
        { new KeyHashtable("TL", "exp"), null },
        { new KeyHashtable("TL", "["), null },
        { new KeyHashtable("TL", "]"), null },
        { new KeyHashtable("TL", "^"), null },
        { new KeyHashtable("TL", "("), null },
        { new KeyHashtable("TL", ")"), "&" },
        { new KeyHashtable("TL", "id"), null },
        { new KeyHashtable("TL", "$"), "&" },
        { new KeyHashtable("P", "+"), null },
        { new KeyHashtable("P", "-"), null },
        { new KeyHashtable("P", "*"), null },
        { new KeyHashtable("P", "/"), null },
        { new KeyHashtable("P", "exp"), "exp [ F ]" },
        { new KeyHashtable("P", "["), null },
        { new KeyHashtable("P", "]"), null },
        { new KeyHashtable("P", "^"), null },
        { new KeyHashtable("P", "("), "F PL" },
        { new KeyHashtable("P", ")"), null },
        { new KeyHashtable("P", "id"), "F PL" },
        { new KeyHashtable("P", "$"), null },
        { new KeyHashtable("PL", "+"), "&" },
        { new KeyHashtable("PL", "-"), "&" },
        { new KeyHashtable("PL", "*"), "&" },
        { new KeyHashtable("PL", "/"), "&" },
        { new KeyHashtable("PL", "exp"), null },
        { new KeyHashtable("PL", "["), null },
        { new KeyHashtable("PL", "]"), null },
        { new KeyHashtable("PL", "^"), "^ F PL" },
        { new KeyHashtable("PL", "("), null },
        { new KeyHashtable("PL", ")"), "&" },
        { new KeyHashtable("PL", "id"), null },
        { new KeyHashtable("PL", "$"), "&" },
        { new KeyHashtable("F", "+"), null },
        { new KeyHashtable("F", "-"), null },
        { new KeyHashtable("F", "*"), null },
        { new KeyHashtable("F", "/"), null },
        { new KeyHashtable("F", "exp"), null },
        { new KeyHashtable("F", "["), null },
        { new KeyHashtable("F", "]"), null },
        { new KeyHashtable("F", "^"), null },
        { new KeyHashtable("F", "("), "( E )" },
        { new KeyHashtable("F", ")"), null },
        { new KeyHashtable("F", "id"), "id" },
        { new KeyHashtable("F", "$"), null }
    };
    private static readonly List<string> NonTerminals = new()
    {
        "E", "L", "T", "TL", "P", "PL", "F"
    };

    private static readonly List<string> Terminals = new()
    {
        "+", "-", "*", "/", "exp", "[", "]", "^", "(", ")", "id", "$"
    };
    public static void Check()
    {
        var stack = new Stack<string>();
        var symbol = Tokenizer.NextToken();
        stack.Push("$");
        stack.Push("E");
        while (stack.Peek() != "$" || symbol.Type != Token.TypeToken.EndOfChain)
        {
            var symbolInExpression = symbol.Type == Token.TypeToken.Identifier ? "id" : symbol.Content;
            if (symbol.Type == Token.TypeToken.LineBreak)
            {
                stack.Push("E");
                symbol = Tokenizer.NextToken();
            }
            else if (stack.Peek() == symbolInExpression)
            {
                stack.Pop();
                if (Tokenizer.Position <= Tokenizer.ContentFile.Length)
                {
                    symbol = Tokenizer.NextToken();
                }

            }
            else if (Terminals.Contains(stack.Peek()))
            {
                throw new Exception("Syntax Error");
            }
            else
            {
                var rules = HashtableSyntatic[new KeyHashtable(stack.Peek(), symbolInExpression)];
                if (rules == null) throw new Exception("Syntax Error");
                stack.Pop();
                var rulesSeparated = rules.ToString()?.Split(' ');
                if (rulesSeparated == null) continue;
                for (var i = rulesSeparated.Length - 1; i >= 0; i--)
                {
                    if (rulesSeparated[i] != "&")
                    {
                        stack.Push(rulesSeparated[i]);
                    }
                }
            }
        }

        if (Tokenizer.Position < Tokenizer.ContentFile.Length)
        {
            throw new Exception("Syntax Error"); ;
        }
        Console.WriteLine("Build success!");
    }
}