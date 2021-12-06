namespace SLang.Core.Expressions;

public class IdentifierExpression : IExpression
{
    public string Name { get; }

    public IdentifierExpression(string name)
    {
        Name = name;
    }

    public bool Equals(IExpression? other)
        => other is IdentifierExpression otherIdentifier && Name == otherIdentifier.Name;
}
