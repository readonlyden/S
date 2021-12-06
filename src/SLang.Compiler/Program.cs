using SLang.CodeGen.IL;
using SLang.Parser;

if (!args.Any())
{
    Console.WriteLine("File not specified");
    return;
}

var parser = new Parser();
var syntaxTree = parser.Parse(File.ReadAllText(args[0]));

var codeGen = new CodeGen();

codeGen.GenerateCode(syntaxTree, "output.dll");
