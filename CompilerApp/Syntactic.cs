using System.Collections;

namespace CompilerApp;

public class Syntactic
{
    private static readonly Lex Tokenizer = new("input.txt");

    private static readonly Hashtable HashtableSyntactic = new()
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

    private double ResolveExpression(string operating1, string operator1, string operating2 = null)
    {
        switch (operator1)
        {
            case "+":
                return Convert.ToDouble(operating1) + Convert.ToDouble(operating2);
            case "-":
                return Convert.ToDouble(operating1) - Convert.ToDouble(operating2);
            case "*":
                return Convert.ToDouble(operating1) * Convert.ToDouble(operating2);
            case "/":
                return Convert.ToDouble(operating1) / Convert.ToDouble(operating2);
            case "^":
                return Math.Pow(Convert.ToDouble(operating1), Convert.ToDouble(operating2));
            case "exp":
                return Math.Exp(Convert.ToDouble(operating1));
        }

        return double.Epsilon;
    }

    public void Check()
    {
        var expression = new List<StackItem>() { };
        var numberToken = -1;
        var stack = new Stack<StackItem>();
        var symbol = Tokenizer.NextToken();
        stack.Push(new StackItem()
        {
            Content = "$",
            Level = 0,
        });
        stack.Push(new StackItem()
        {
            Content = "E",
            Level = 0,
        });
        while (stack.Peek().Content != "$" || symbol.Type != EnumTypeToken.EndOfChain)
        {
            var symbolInExpression = symbol.Type == EnumTypeToken.Identifier ? "id" : symbol.Content;
            if (symbol.Type == EnumTypeToken.LineBreak)
            {
                Console.WriteLine(Calculate(expression));
                expression.Clear();
                stack.Push(new StackItem()
                {
                    Content = "E",
                    Level = 0,
                });
                numberToken = -1;
                symbol = Tokenizer.NextToken();
            }
            else if (stack.Peek().Content == symbolInExpression)
            {
                if (symbol.Type == EnumTypeToken.Identifier || symbol.Type == EnumTypeToken.Operator)
                {
                    expression.Add(new StackItem()
                    {
                        Id = Guid.NewGuid(),
                        Content = symbol.Content,
                        Level = stack.Peek().Level,
                        Type = symbol.Type
                    });
                    numberToken++;
                }

                stack.Pop();
                symbol = Tokenizer.NextToken();
            }
            else if (Terminals.Contains(stack.Peek().Content))
            {
                throw new Exception("Syntax Error");
            }
            else
            {
                var rules = HashtableSyntactic[new KeyHashtable(stack.Peek().Content, symbolInExpression)];
                if (rules == null) throw new Exception("Syntax Error");
                var fatherLevel = stack.Peek().Level;
                stack.Pop();
                var rulesSeparated = rules.ToString()?.Split(' ');
                if (rulesSeparated == null) continue;
                for (var i = rulesSeparated.Length - 1; i >= 0; i--)
                {
                    if (rulesSeparated[i] != "&")
                    {
                        stack.Push(new StackItem()
                        {
                            Content = rulesSeparated[i],
                            Level = fatherLevel + 1,
                        });
                    }
                    else
                    {
                        if (symbol.Content != "$" || stack.Peek().Content != "$")
                        {
                            expression[numberToken].Level -= 1;
                        }

                    }
                }
            }
        }

        if (Tokenizer.Position < Tokenizer.ContentFile.Length)
        {
            throw new Exception("Syntax Error");
        }

        Console.WriteLine(Calculate(expression));
        Console.WriteLine("Build success!");
    }

    private string Calculate(List<StackItem> expressionForCalculate)
    {
        var maxLevel = expressionForCalculate.Max(t => t.Level);
        var listRemove = new List<string>
        {
            "(", ")", "[", "]"
        };
        expressionForCalculate.RemoveAll(t => listRemove.Contains(t.Content));
        for (var actualLevel = maxLevel; actualLevel >= 0; actualLevel--)
        {
            if (expressionForCalculate.Count == 1)
            {
                return expressionForCalculate[0].Content;
            }
            else
            {
                var quantityOperatorsInActualLevel = expressionForCalculate
                    .FindAll(t => t.Level == actualLevel && t.Type == EnumTypeToken.Operator).Count;
                if (quantityOperatorsInActualLevel == 0) continue;
                else
                {
                    var operatorsInActualLevel = expressionForCalculate
                        .FindAll(t => t.Level == actualLevel && t.Type == EnumTypeToken.Operator);
                    foreach (var actualOperator in operatorsInActualLevel)
                    {
                        var positionOperator = expressionForCalculate.FindIndex(t => t.Id == actualOperator.Id);
                        if (actualOperator.Content == "exp")
                        {
                            var result = ResolveExpression(expressionForCalculate[positionOperator + 1].Content,
                                expressionForCalculate[positionOperator].Content);
                            expressionForCalculate.RemoveAt(positionOperator + 1);
                            expressionForCalculate[positionOperator] = new StackItem()
                            {
                                Id = Guid.NewGuid(),
                                Content = result.ToString(),
                                Type = EnumTypeToken.Identifier
                            };
                        }
                        else
                        {
                            var result = ResolveExpression(expressionForCalculate[positionOperator - 1].Content,
                                expressionForCalculate[positionOperator].Content,
                                expressionForCalculate[positionOperator + 1].Content);
                            expressionForCalculate.RemoveRange(positionOperator - 1, 2);
                            expressionForCalculate[positionOperator - 1] = new StackItem()
                            {
                                Id = Guid.NewGuid(),
                                Content = result.ToString(),
                                Type = EnumTypeToken.Identifier
                            };
                        }

                    }
                }
            }
        }

        return "";
    }
}
