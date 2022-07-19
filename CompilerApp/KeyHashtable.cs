namespace CompilerApp;
public class KeyHashtable
{
    public KeyHashtable(string nonTerminalSymbol, string terminalSymbol)
    {
        NonTerminalSymbol = nonTerminalSymbol;
        TerminalSymbol = terminalSymbol;
    }
    private string NonTerminalSymbol { get; }
    private string TerminalSymbol { get; }

    public override bool Equals(object? compareObject)
    {
        if (compareObject is not KeyHashtable compareKey)
            return false;
        return NonTerminalSymbol == compareKey.NonTerminalSymbol && TerminalSymbol == compareKey.TerminalSymbol;
    }

    public override int GetHashCode()
    {
        return 10 * NonTerminalSymbol.GetHashCode() + TerminalSymbol.GetHashCode();
    }
}