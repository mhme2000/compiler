public class Lex
{
    private char[] contentFile;
    private int state;
    private int position;
    private string buffer = null;
    public Lex(string inputFileName)
    {
        try
        {
            var currentDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
            var path = Path.Combine(projectDirectory, inputFileName);
            contentFile = File.ReadAllText(path).ToCharArray();
            Console.WriteLine(contentFile);
        }
        catch (Exception ex)
        {

        }


    }

    public Token nextToken()
    {
        Token token;
        char currentChar;
        if (isEOF())
        {
            return null;
        }

        state = 0;
        while (true)
        {
            currentChar = goNext();
            switch (state)
            {
                case 0:
                    if (isDigit(currentChar))
                    {
                        state = 1;
                        buffer += currentChar;
                    }
                    else if (isOperator(currentChar))
                    {
                        state = 2;
                        buffer += currentChar;
                        token = new Token() { Content = buffer, Type = Token.TypeToken.Operator };
                        clearBuffer();
                        return token;
                    }
                    else if (isBundler(currentChar))
                    {
                        state = 3;
                        buffer += currentChar;
                        token = new Token() { Content = buffer, Type = Token.TypeToken.Bundler };
                        clearBuffer();
                        return token;
                    }
                    else if (isLetterE(currentChar))
                    {
                        state = 4;
                        buffer += currentChar;
                    }
                    else if (isSpace(currentChar))
                    {
                        state = 0;
                        clearBuffer();
                    }
                    else
                    {
                        throw new Exception("Unexpected token");
                    }
                    break;
                case 1:
                    if (!isDigit(currentChar) && !isDot(currentChar))
                    {
                        state = 0;
                        token = new Token() { Content = buffer, Type = Token.TypeToken.Identifier };
                        clearBuffer();
                        goBack();
                        return token;
                    }
                    else if (isDot(currentChar))
                    {
                        state = 7;
                        buffer += currentChar;
                    }
                    else if (isDigit(currentChar))
                    {
                        state = 1;
                        buffer += currentChar;
                    }
                    break;
                case 2:
                    state = 0;
                    break;
                case 3:
                    state = 0;
                    break;
                case 4:
                    if (!isLetterX(currentChar))
                    {
                        throw new Exception("Unexpected token");
                    }
                    else
                    {
                        state = 5;
                        buffer += currentChar;
                    }
                    break;
                case 5:
                    if (!isLetterP(currentChar))
                    {
                        throw new Exception("Unexpected token");
                    }
                    else
                    {
                        state = 6;
                        buffer += currentChar;
                        token = new Token() { Content = buffer, Type = Token.TypeToken.Operator };
                        clearBuffer();
                        return token;
                    }
                    break;
                case 6:
                    state = 0;
                    break;
                case 7:
                    if (isDigit(currentChar))
                    {
                        state = 1;
                        buffer += currentChar;
                    }
                    else
                    {
                        throw new Exception("Unexpected token");
                    }

                    break;
            }
        }
    }

    private bool isDigit(char c)
    {
        return (c >= '0' && c <= '9');
    }

    private bool isLetterE(char c)
    {
        return (c == 'E' || c == 'e');
    }

    private bool isLetterX(char c)
    {
        return (c == 'X' || c == 'x');
    }

    private bool isLetterP(char c)
    {
        return (c == 'P' || c == 'p');
    }

    private bool isOperator(char c)
    {
        var operators = new List<char>()
        {
            '+', '-', '/', '*', '^'
        };
        return (operators.Contains(c));
    }

    private bool isBundler(char c)
    {
        var bundlers = new List<char>()
        {
            '(', ')', '[', ']'
        };
        return (bundlers.Contains(c));
    }

    private bool isSpace(char c)
    {
        return (c == ' ' || c == '\t' || c == '\n' || c == '\r');
    }

    private bool isDot(char c)
    {
        return c == '.';
    }

    private bool isEOF()
    {
        return position == contentFile.Length;
    }

    private char goNext()
    {
        return contentFile[position++];
    }

    private void goBack()
    {
        position--;
    }

    private void clearBuffer()
    {
        buffer = null;
    }
}