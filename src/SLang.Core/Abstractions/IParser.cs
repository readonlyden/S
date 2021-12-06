namespace SLang.Core.Abstractions;

public interface IParser
{
    CompilationUnit Parse(string code);
}
