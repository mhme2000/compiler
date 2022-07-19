namespace CompilerApp;

public class Lex
{
    public readonly char[] ContentFile = string.Empty.ToCharArray();
    private string _buffer = string.Empty;
    private int _state;
    public int Position;
    public Lex(string inputFileName)
    {
        try
        {
            var currentDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
            if (projectDirectory == null) return;
            var path = Path.Combine(projectDirectory, inputFileName);
            ContentFile = File.ReadAllText(path).ToCharArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured: {ex}");
        }
    }

    public Token NextToken()
    {
        Token token;
        if (IsEof())
        {
            _state = 0;
            token = new Token() { Content = "$", Type = EnumTypeToken.EndOfChain };
            ClearBuffer();
            return token;
        }

        _state = 0;
        while (true)
        {
            if (IsEof())
            {
                _state = 0;
                token = new Token() { Content = _buffer, Type = EnumTypeToken.Identifier };
                ClearBuffer();
                return token;
            }
            var currentChar = GoNext();
            switch (_state)
            {
                case 0:
                    if (IsDigit(currentChar))
                    {
                        _state = 1;
                        _buffer += currentChar;
                    }
                    else if (IsOperator(currentChar))
                    {
                        _state = 2;
                        _buffer += currentChar;
                        token = new Token() { Content = _buffer, Type = EnumTypeToken.Operator };
                        ClearBuffer();
                        return token;
                    }
                    else if (IsBundler(currentChar))
                    {
                        _state = 3;
                        _buffer += currentChar;
                        token = new Token() { Content = _buffer, Type = EnumTypeToken.Bundler };
                        ClearBuffer();
                        return token;
                    }
                    else if (IsLetterE(currentChar))
                    {
                        _state = 4;
                        _buffer += currentChar;
                    }
                    else if (IsSpace(currentChar))
                    {
                        _state = 0;
                        ClearBuffer();
                    }
                    else if (IsLineBreak(currentChar))
                    {
                        _state = 0;
                        token = new Token() { Content = currentChar.ToString(), Type = EnumTypeToken.LineBreak };
                        ClearBuffer();
                        return token;
                    }
                    else
                    {
                        throw new Exception("Unexpected token");
                    }

                    break;
                case 1:
                    if (!IsDigit(currentChar) && !IsDot(currentChar))
                    {
                        _state = 0;
                        token = new Token() { Content = _buffer, Type = EnumTypeToken.Identifier };
                        ClearBuffer();
                        GoBack();
                        return token;
                    }
                    else if (IsDigit(currentChar))
                    {
                        _state = 1;
                        _buffer += currentChar;
                    }

                    else if (IsDot(currentChar) && !_buffer.Contains('.'))
                    {
                        _state = 7;
                        _buffer += currentChar;
                    }
                    else if (_buffer.Contains('.'))
                    {
                        throw new Exception("Unexpected token");
                    }
                    break;
                case 2:
                    _state = 0;
                    break;
                case 3:
                    _state = 0;
                    break;
                case 4:
                    if (!IsLetterX(currentChar))
                    {
                        throw new Exception("Unexpected token");
                    }
                    _state = 5;
                    _buffer += currentChar;
                    break;
                case 5:
                    if (!IsLetterP(currentChar))
                    {
                        throw new Exception("Unexpected token");
                    }
                    _state = 6;
                    _buffer += currentChar;
                    token = new Token() { Content = _buffer, Type = EnumTypeToken.Operator };
                    ClearBuffer();
                    return token;
                case 6:
                    _state = 0;
                    break;
                case 7:
                    if (IsDigit(currentChar))
                    {
                        _state = 1;
                        _buffer += currentChar;
                    }
                    else
                    {
                        throw new Exception("Unexpected token");
                    }
                    break;
            }
        }
    }

    private static bool IsDigit(char c)
    {
        return (c >= '0' && c <= '9');
    }

    private static bool IsLetterE(char c)
    {
        return (c == 'E' || c == 'e');
    }

    private static bool IsLetterX(char c)
    {
        return (c == 'X' || c == 'x');
    }

    private static bool IsLetterP(char c)
    {
        return (c == 'P' || c == 'p');
    }

    private static bool IsOperator(char c)
    {
        var operators = new List<char>()
        {
            '+', '-', '/', '*', '^'
        };
        return (operators.Contains(c));
    }

    private static bool IsBundler(char c)
    {
        var bundlers = new List<char>()
        {
            '(', ')', '[', ']'
        };
        return (bundlers.Contains(c));
    }

    private static bool IsSpace(char c)
    {
        return c == ' ' || c == '\t' || c == '\r';
    }

    private static bool IsLineBreak(char c)
    {
        return c == '\n' ;
    }

    private static bool IsDot(char c)
    {
        return c == '.';
    }

    private bool IsEof()
    {
        return Position == ContentFile.Length;
    }

    private char GoNext()
    {
        return ContentFile[Position++];
    }

    private void GoBack()
    {
        Position--;
    }

    private void ClearBuffer()
    {
        _buffer = string.Empty;
    }
}