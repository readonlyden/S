namespace SLang.Parser.Expressions;

public class UnaryOperationExpression : IExpression
{
    public UnaryOperationType Type { get; }
    public IExpression Expr { get; }

    public UnaryOperationExpression(UnaryOperationType type, IExpression expr)
    {
        Type = type;
        Expr = expr;
    }

    public bool Equals(IExpression? other)
        => other is UnaryOperationExpression u
        && this.Type == u.Type
        && this.Expr.Equals(u.Expr);
}
