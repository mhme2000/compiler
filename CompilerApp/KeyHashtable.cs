public class KeyHashtable
{
    public KeyHashtable(string nonTerminalSymbol, string terminalSymbol)
    {
        NonTerminalSymbol = nonTerminalSymbol;
        TerminalSymbol = terminalSymbol;
    }
    private string NonTerminalSymbol { get; }
    private string TerminalSymbol { get; }

    public override bool Equals(object? otherObject)
    {
        if (otherObject is not KeyHashtable otherKey)
            return false;
        return NonTerminalSymbol == otherKey.NonTerminalSymbol && TerminalSymbol == otherKey.TerminalSymbol;
    }

    public override int GetHashCode()
    {
        return 10 * NonTerminalSymbol.GetHashCode() + TerminalSymbol.GetHashCode();
    }
}