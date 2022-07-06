using System.Collections;

public class Syntax
{
    private static Lex _tokenizer = new Lex("input.txt");
    private static Hashtable _hashtableSyntatic = new Hashtable
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
    private List<string> _nonTerminals = new List<string>
    {
        "E", "L", "T", "TL", "P", "PL", "F"
    };

    private static List<string> _terminals = new List<string>
    {
        "+", "-", "*", "/", "exp", "[", "]", "^", "(", ")", "id", "$"
    };

    public static void Check()
    {
        var stack = new Stack<string>();
        var symbol = _tokenizer.nextToken();
        stack.Push("$");
        stack.Push("E");
        while (stack.Peek() != "$")
        {
            var symbolInExpression = symbol.Type == Token.TypeToken.Identifier ? "id" : symbol.Content;
            if (stack.Peek() == symbolInExpression)
            {
                stack.Pop();
                if (_tokenizer.position <= _tokenizer.contentFile.Length)
                {
                    symbol = _tokenizer.nextToken();
                }
                
            }
            else if (_terminals.Contains(stack.Peek()))
            {
                throw new Exception("Syntax Error");
            }
            else
            {
                var rules = _hashtableSyntatic[new KeyHashtable(stack.Peek(), symbolInExpression)];
                if (rules == null) throw new Exception("Syntax Error");;
                stack.Pop();
                var rulesSeparated = rules.ToString().Split(' ');
                for (int i = rulesSeparated.Length - 1; i >= 0; i--)
                {
                    if (rulesSeparated[i] != "&"){
                        stack.Push(rulesSeparated[i]);
                    }
                    
                }
            }
        }

        if (_tokenizer.position < _tokenizer.contentFile.Length)
        {
            throw new Exception("Syntax Error");;
        }
        Console.WriteLine("Deu boa");
    }
}