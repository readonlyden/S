using SLang.Core.Expressions;

namespace SLang.Core
{
    public class CompilationUnit
    {
        public IExpression Expression { get; set; }

        public CompilationUnit(IExpression expression) => Expression = expression;
    }
}
