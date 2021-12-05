using System.Collections.Immutable;

namespace SLang.Parser.Expressions;

public class CallExpression : IExpression
{
    public IExpression Body { get; }
    public ImmutableArray<IExpression> Arguments { get; }

    public CallExpression(IExpression expr, ImmutableArray<IExpression> arguments)
    {
        Body = expr;
        Arguments = arguments;
    }

    public bool Equals(IExpression? other)
        => other is CallExpression c
        && Body.Equals(c.Body)
        && Arguments.SequenceEqual(c.Arguments);
}
