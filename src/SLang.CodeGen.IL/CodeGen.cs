using Mono.Cecil;
using Mono.Cecil.Cil;
using SLang.Core;
using SLang.Core.Abstractions;
using SLang.Core.Expressions;

namespace SLang.CodeGen.IL;

public class CodeGen : ICodeGenerator
{
    public Task GenerateCode(CompilationUnit syntaxTree, string outputFileName)
    {
        if (syntaxTree.Expression is not BinaryOperationExpression
            {
                Left: LiteralExpression left,
                Right: LiteralExpression right,
                Type: BinaryOperationType.Add
            })
        {
            throw new NotSupportedException();
        }

        var app = AssemblyDefinition.CreateAssembly(
            new AssemblyNameDefinition("HelloWorld", new Version(1, 0, 0, 0)), "HelloWorld", ModuleKind.Console);

        var module = app.MainModule;

        // create the program type and add it to the module
        var programType = new TypeDefinition("HelloWorld", "Program",
            Mono.Cecil.TypeAttributes.Class | Mono.Cecil.TypeAttributes.Public, module.TypeSystem.Object);

        module.Types.Add(programType);

        // add an empty constructor
        var ctor = new MethodDefinition(".ctor", Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.HideBySig
            | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.RTSpecialName, module.TypeSystem.Void);

        // define the 'Main' method and add it to 'Program'
        var mainMethod = new MethodDefinition("Main",
            Mono.Cecil.MethodAttributes.Public | Mono.Cecil.MethodAttributes.Static, module.TypeSystem.Void);

        programType.Methods.Add(mainMethod);

        // add the 'args' parameter
        var argsParameter = new ParameterDefinition("args",
            Mono.Cecil.ParameterAttributes.None, module.Import(typeof(string[])));

        mainMethod.Parameters.Add(argsParameter);

        // create the method body
        var il = mainMethod.Body.GetILProcessor();

        il.Append(il.Create(OpCodes.Ldc_I4, left.Value));
        il.Append(il.Create(OpCodes.Ldc_I4, right.Value));
        il.Append(il.Create(OpCodes.Add));

        var writeLineMethod = il.Create(OpCodes.Call,
            module.Import(typeof(Console).GetMethod("WriteLine", new[] { typeof(int) })!)!);

        // call the method
        il.Append(writeLineMethod);

        il.Append(il.Create(OpCodes.Nop));
        il.Append(il.Create(OpCodes.Ret));

        // set the entry point and save the module
        app.EntryPoint = mainMethod;
        app.Write("Generated.dll");
        return Task.CompletedTask;
    }
}
