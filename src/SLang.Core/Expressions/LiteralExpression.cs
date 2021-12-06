namespace SLang.Core.Expressions;

public class LiteralExpression : IExpression
{
    public int Value { get; }

    public LiteralExpression(int value)
    {
        Value = value;
    }

    public bool Equals(IExpression? other)
        => other is LiteralExpression l && this.Value == l.Value;
}
