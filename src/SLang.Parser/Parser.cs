using System.Collections.Immutable;
using Pidgin;
using Pidgin.Expression;
using SLang.Parser.Expressions;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace SLang.Parser;

public class Parser
{
    private static Parser<char, T> Tok<T>(Parser<char, T> token)
            => Try(token).Before(SkipWhitespaces);
    private static Parser<char, string> Tok(string token)
        => Tok(String(token));

    private static Parser<char, T> Parenthesised<T>(Parser<char, T> parser)
        => parser.Between(Tok("("), Tok(")"));

    private static Parser<char, Func<IExpression, IExpression, IExpression>> Binary(Parser<char, BinaryOperationType> op)
        => op.Select<Func<IExpression, IExpression, IExpression>>(type => (l, r) => new BinaryOperationExpression(type, l, r));
    private static Parser<char, Func<IExpression, IExpression>> Unary(Parser<char, UnaryOperationType> op)
        => op.Select<Func<IExpression, IExpression>>(type => o => new UnaryOperationExpression(type, o));

    private static readonly Parser<char, Func<IExpression, IExpression, IExpression>> Add
        = Binary(Tok("+").ThenReturn(BinaryOperationType.Add));
    private static readonly Parser<char, Func<IExpression, IExpression, IExpression>> Substract
        = Binary(Tok("-").ThenReturn(BinaryOperationType.Subtract));
    private static readonly Parser<char, Func<IExpression, IExpression, IExpression>> Multiply
        = Binary(Tok("*").ThenReturn(BinaryOperationType.Multiply));
    private static readonly Parser<char, Func<IExpression, IExpression, IExpression>> Divizion
        = Binary(Tok("/").ThenReturn(BinaryOperationType.Divide));

    private static readonly Parser<char, Func<IExpression, IExpression>> Negate
        = Unary(Tok("-").ThenReturn(UnaryOperationType.Negate));
    private static readonly Parser<char, Func<IExpression, IExpression>> Complement
        = Unary(Tok("~").ThenReturn(UnaryOperationType.Complement));

    private static readonly Parser<char, IExpression> Identifier
        = Tok(Letter.Then(LetterOrDigit.ManyString(), (h, t) => h + t))
            .Select<IExpression>(name => new IdentifierExpression(name))
            .Labelled("identifier");

    private static readonly Parser<char, IExpression> Literal
        = Tok(Num)
            .Select<IExpression>(value => new LiteralExpression(value))
            .Labelled("integer literal");

    private static Parser<char, Func<IExpression, IExpression>> Call(Parser<char, IExpression> subExpr)
        => Parenthesised(subExpr.Separated(Tok(",")))
            .Select<Func<IExpression, IExpression>>(args => method => new CallExpression(method, args.ToImmutableArray()))
            .Labelled("function call");

    private static readonly Parser<char, IExpression> Expr = ExpressionParser.Build<char, IExpression>(
        expr => (
            OneOf(
                Identifier,
                Literal,
                Parenthesised(expr).Labelled("parenthesised expression")
            ),
            new[]
            {
                    Operator.PostfixChainable(Call(expr)),
                    Operator.Prefix(Negate).And(Operator.Prefix(Complement)),
                    Operator.InfixL(Multiply),
                    Operator.InfixL(Divizion),
                    Operator.InfixL(Add),
                    Operator.InfixL(Substract)
            }
        )
    ).Labelled("expression");

    public static IExpression ParseOrThrow(string input)
        => Expr.ParseOrThrow(input);
}
