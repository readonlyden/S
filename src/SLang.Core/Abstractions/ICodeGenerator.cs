namespace SLang.Core.Abstractions;

public interface ICodeGenerator
{
    Task GenerateCode(CompilationUnit syntaxTree, string outputFileName);
}
