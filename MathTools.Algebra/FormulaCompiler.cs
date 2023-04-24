using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace MathTools.Algebra
{
    public static class FormulaCompiler
    {
        public static Func<double, double> Compile(this Formula formula) => throw new NotImplementedException();

        private const string NamespaceName = "MathTools.Algebra";
        private const string ClassName = "FormulaEmitter";
        private const string MethodName = "Eval";

        private const string CodeHeader = $$"""
            namespace {{NamespaceName}}
            {
                using System;
                public class {{ClassName}}
                {
                    public double {{MethodName}}
            """;

        private const string CodeFooter = """
                }
            }
            """;

        internal static Type CreateClass(Formula formula)
        {
            var variables = formula.GetVariables();

            var code = CodeHeader + $"({string.Join(", ", variables.Select(v => $"double {v}"))}) => {formula.ToString("C#", null)};" + CodeFooter;
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var assemblyName = Path.GetRandomFileName();
            var references = AppDomain.CurrentDomain
              .GetAssemblies()
              .Where(a => !a.IsDynamic)
              .Select(a => a.Location)
              .Where(s => !string.IsNullOrEmpty(s))
              .Select(s => MetadataReference.CreateFromFile(s));

            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var stream = new MemoryStream();
            var result = compilation.Emit(stream);

            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                throw new FormulaException("Compilation Error\n" + string.Join("\n", failures.Select(f => f.GetMessage())));
            }

            stream.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(stream.ToArray());

            return assembly.GetType($"{NamespaceName}.{ClassName}") ?? throw new FormulaException("Could not get type from assembly.");
        }

        public static Func<double[], double> ToFunc(this Formula formula)
        {
            var type = CreateClass(formula);
            var obj = Activator.CreateInstance(type);

            return (double[] vars) =>
            {
                var result = type.InvokeMember(
                    MethodName,
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    obj,
                    vars.Select(v => (object)v).ToArray());

                return result is null ? throw new FormulaException("Eval result is null.") : (double)result;
            };
        }
    }
}
