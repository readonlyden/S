namespace SLang.Core.Expressions;

public class BinaryOperationExpression : IExpression
{
    public BinaryOperationType Type { get; }
    public IExpression Left { get; }
    public IExpression Right { get; }

    public BinaryOperationExpression(BinaryOperationType type, IExpression left, IExpression right)
    {
        Type = type;
        Left = left;
        Right = right;
    }

    public bool Equals(IExpression? other)
        => other is BinaryOperationExpression b
        && Type == b.Type
        && Left.Equals(b.Left)
        && Right.Equals(b.Right);
}
