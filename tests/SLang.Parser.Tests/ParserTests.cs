using SLang.Core.Expressions;
using Xunit;

namespace SLang.Parser.Tests;

public class ParserTests
{
    [Fact]
    public void Parse_ShouldParseLiteralsInBinaryExpression()
    {
        string input = "2 + 2";

        var expression = Parser.ParseOrThrow(input);

        Assert.IsType<BinaryOperationExpression>(expression);

        if (expression is BinaryOperationExpression binaryOperation)
        {
            Assert.Equal(BinaryOperationType.Add, binaryOperation.Type);
            Assert.IsType<LiteralExpression>(binaryOperation.Left);
            Assert.IsType<LiteralExpression>(binaryOperation.Right);
        }
    }
}
