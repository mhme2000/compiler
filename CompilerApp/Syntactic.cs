using System.Collections;
using System.Globalization;

namespace CompilerApp;
public sealed class Syntactic
{
    #region private_variables
    private static Lex _lexScanner = null!;

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
    
    private static readonly List<string> Terminals = new()
    {
        "+", "-", "*", "/", "exp", "[", "]", "^", "(", ")", "id", "$"
    };
    #endregion
    
    #region private_methods
    private static double ResolveExpression(string operating1, string operator1, string? operating2 = null)
    {
        return operator1 switch
        {
            "+" => Convert.ToDouble(operating1) + Convert.ToDouble(operating2),
            "-" => Convert.ToDouble(operating1) - Convert.ToDouble(operating2),
            "*" => Convert.ToDouble(operating1) * Convert.ToDouble(operating2),
            "/" => Convert.ToDouble(operating1) / Convert.ToDouble(operating2),
            "^" => Math.Pow(Convert.ToDouble(operating1), Convert.ToDouble(operating2)),
            "exp" => Math.Exp(Convert.ToDouble(operating1)),
            _ => double.NaN
        };
    }
    private static string Calculate(List<ExpressionItem> expressionForCalculate)
    {
        var maxLevel = expressionForCalculate.Max(t => t.Level);
        for (var currentLevel = maxLevel; currentLevel >= 0; currentLevel--)
        {
            if (expressionForCalculate.Count == 1)
            {
                return expressionForCalculate[0].Value;
            }

            var quantityOperatorsInCurrentLevel = expressionForCalculate
                .FindAll(t => t.Level == currentLevel && t.Type == EnumTypeToken.Operator).Count;
            if (quantityOperatorsInCurrentLevel == 0) continue;
            var operatorsInCurrentLevel = expressionForCalculate
                .FindAll(t => t.Level == currentLevel && t.Type == EnumTypeToken.Operator);
            foreach (var currentOperator in operatorsInCurrentLevel)
            {
                var positionOperator = expressionForCalculate.FindIndex(t => t.Id == currentOperator.Id);
                if (currentOperator.Value == "exp")
                {
                    var result = ResolveExpression(expressionForCalculate[positionOperator + 1].Value,
                        expressionForCalculate[positionOperator].Value);
                    expressionForCalculate.RemoveAt(positionOperator + 1);
                    expressionForCalculate[positionOperator] = new ExpressionItem()
                    {
                        Id = Guid.NewGuid(),
                        Value = result.ToString(CultureInfo.InvariantCulture),
                        Type = EnumTypeToken.Identifier
                    };
                }
                else
                {
                    var result = ResolveExpression(expressionForCalculate[positionOperator - 1].Value,
                        expressionForCalculate[positionOperator].Value,
                        expressionForCalculate[positionOperator + 1].Value);
                    expressionForCalculate.RemoveRange(positionOperator - 1, 2);
                    expressionForCalculate[positionOperator - 1] = new ExpressionItem()
                    {
                        Id = Guid.NewGuid(),
                        Value = result.ToString(CultureInfo.InvariantCulture),
                        Type = EnumTypeToken.Identifier
                    };
                }

            }
        }
        return string.Empty;
    }
    
    private static string? NextExpectedToken(EnumTypeToken currentSymbolType)
    {
        switch (currentSymbolType)
        {
            case EnumTypeToken.Identifier:
                return "'operator' or ')' or ']' or 'LineBreak' or 'EndChain'";
            case EnumTypeToken.Operator:
                return "'Identifier' or '(' or '[' or 'exp'";
            case EnumTypeToken.Bundler:
                return "'Identifier' or 'operator'";
            case EnumTypeToken.LineBreak:
                return "'Identifier' or '(' or 'exp'";
            case EnumTypeToken.EndOfChain:
                return null;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentSymbolType), currentSymbolType, null);
        }
    }
    #endregion
    
    public Syntactic(string inputFileName)
    {
        _lexScanner = new Lex(inputFileName);
    }
    
    public void CheckSyntax()
    {
        var expression = new List<ExpressionItem>();
        var tokenPosition = -1;
        var stack = new Stack<StackItem>();
        var token = _lexScanner.NextToken();
        var nextExpectedToken = NextExpectedToken(token.Type);
        var lineOfCode = 1;
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
        while (stack.Peek().Content != "$" || token.Type != EnumTypeToken.EndOfChain)
        {
            var symbolInExpression = token.Type == EnumTypeToken.Identifier ? "id" : token.Content;
            
            if (token.Type == EnumTypeToken.LineBreak)
            {
                if (expression.Count > 0)
                {
                    Console.WriteLine(Calculate(expression));
                    expression.Clear();
                    stack.Push(new StackItem()
                    {
                        Content = "E",
                        Level = 0,
                    });
                    tokenPosition = -1;
                    lineOfCode++;
                }
                token = _lexScanner.NextToken();
            }
            else if (stack.Peek().Content == symbolInExpression)
            {
                if (token.Type == EnumTypeToken.Identifier || token.Type == EnumTypeToken.Operator)
                {
                    expression.Add(new ExpressionItem()
                    {
                        Id = Guid.NewGuid(),
                        Value = token.Content,
                        Level = stack.Peek().Level,
                        Type = token.Type
                    });
                    tokenPosition++;
                }

                stack.Pop();
                token = _lexScanner.NextToken();
            }
            else if (Terminals.Contains(stack.Peek().Content))
            {
                throw new Exception($"Syntactic Error in line {lineOfCode}, '{stack.Peek().Content}' was expected, but found '{token.Content}'");
            }
            else
            {
                var rules = HashtableSyntactic[new KeyHashtable(stack.Peek().Content, symbolInExpression)];
                if (rules == null) throw new Exception($"Syntactic Error in line {lineOfCode}, {nextExpectedToken} was expected, but found '{token.Content}'");
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
                        if (token.Content != "$" || stack.Peek().Content != "$")
                        {
                            expression[tokenPosition].Level -= 1;
                        }
                    }
                }
            }
        }

        if (_lexScanner.Position < _lexScanner.ContentFile.Length)
        {
            throw new Exception($"Syntactic Error in line {lineOfCode}, {nextExpectedToken} was expected, but found '{token.Content}'");
        }

        Console.WriteLine(Calculate(expression));
        Console.WriteLine("Build success!");
    }
}
