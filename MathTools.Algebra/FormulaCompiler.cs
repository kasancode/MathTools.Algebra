using MathTools.Algebra.Functions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace MathTools.Algebra
{
    public static partial class FormulaCompiler
    {
        private const string NamespaceName = "MathTools.Algebra";
        private const string ClassName = "FormulaEmitter";
        private const string MethodName = "Eval";

        internal static void Emit(ILGenerator generator, Formula formula, List<string> variables)
        {
            switch (formula)
            {
                case Constant c:
                    generator.Emit(OpCodes.Ldc_R8, c.Value);
                    break;

                case Variable v:
                    var index = variables.IndexOf(v.Name);
                    if (index < 0)
                        throw new FormulaException("Compilation Error. Could not find variable.");
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, index);
                    generator.Emit(OpCodes.Ldelem_R8);
                    break;

                case Sum sum:
                    if (sum.SubFormulae.Count < 0)
                        break;

                    Emit(generator, sum.SubFormulae[0], variables);
                    if (!sum.Signs[0])
                    {
                        generator.Emit(OpCodes.Neg);
                    }

                    for (var i = 1; i < sum.SubFormulae.Count; i++)
                    {
                        var sub = sum.SubFormulae[i];
                        var sign = sum.Signs[i];
                        Emit(generator, sub, variables);
                        generator.Emit(sign ? OpCodes.Add : OpCodes.Sub);
                    }

                    break;

                case Product product:
                    if (product.SubFormulae.Count < 0)
                        break;

                    if (!product.Signs[0])
                    {
                        generator.Emit(OpCodes.Ldc_R8, 1.0);
                        Emit(generator, product.SubFormulae[0], variables);
                        generator.Emit(OpCodes.Div);
                    }
                    else
                    {
                        Emit(generator, product.SubFormulae[0], variables);
                    }

                    for (var i = 1; i < product.SubFormulae.Count; i++)
                    {
                        var sub = product.SubFormulae[i];
                        var sign = product.Signs[i];
                        Emit(generator, sub, variables);
                        generator.Emit(sign ? OpCodes.Mul : OpCodes.Div);
                    }

                    break;

                case Pow:
                    Emit(generator, formula.SubFormulae[0], variables);
                    Emit(generator, formula.SubFormulae[1], variables);
                    generator.Emit(OpCodes.Call, typeof(Math).GetMethod("Pow", new Type[] { typeof(double), typeof(double) }) ?? throw new Exception("Compilation Error. Could not get Math.Pow()"));
                    break;

                case Mod:
                    Emit(generator, formula.SubFormulae[0], variables);
                    Emit(generator, formula.SubFormulae[1], variables);
                    generator.Emit(OpCodes.Rem);
                    break;

                case If:
                    var toTrue = generator.DefineLabel();
                    var toEnd = generator.DefineLabel();
                    Emit(generator, formula.SubFormulae[0], variables);
                    generator.Emit(OpCodes.Ldc_R8, 0.0);
                    generator.Emit(OpCodes.Bge_S, toTrue);

                    Emit(generator, formula.SubFormulae[2], variables);
                    generator.Emit(OpCodes.Br_S, toEnd);

                    generator.MarkLabel(toTrue);
                    Emit(generator, formula.SubFormulae[1], variables);

                    generator.MarkLabel(toEnd);
                    break;

                case Parenthesis:
                    Emit(generator, formula.SubFormulae[0], variables);
                    break;

                default:
                    EmitFunctions(generator, formula, variables);
                    break;
            }
        }

        internal static MethodInfo CreateMethod(Formula formula)
        {
            var variables = formula.GetVariables();
            var assemblyName = Path.GetRandomFileName();
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(assemblyName),
                AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);

            var typeBuilder = moduleBuilder.DefineType(
                $"{NamespaceName}.{ClassName}",
                TypeAttributes.Public | TypeAttributes.AutoLayout | TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Abstract);

            var method = typeBuilder.DefineMethod(MethodName, MethodAttributes.Public | MethodAttributes.Static,
                typeof(double),
                new[] { typeof(double[]) });
            var generator = method.GetILGenerator();
            Emit(generator, formula, variables);
            generator.Emit(OpCodes.Ret);

            var type = typeBuilder.CreateType();

            return type.GetMethod(MethodName) ?? throw new FormulaException("Compilation Error. Could not get method.");
        }

        public delegate double EvalFunc(params double[] args);

        public static EvalFunc ToFunc(this Formula formula)
        {
            var method = CreateMethod(formula);

            if (method is null)
                throw new FormulaException("Compilation Error. Could not get method.");

            return (EvalFunc)Delegate.CreateDelegate(typeof(EvalFunc), method);
        }
    }
}
