using System.Text;

namespace MathTools.Algebra
{
    public class Variable : Formula
    {
        public string Name { get; }

        public Variable(string name)
        {
            Name = name;
        }

        public override double Eval()
            => throw new FormulaException("Varialbes value is not set.");

        public override double Eval(Dictionary<string, double> variables)
        {
            return variables.TryGetValue(Name, out var value)
                ? value
                : throw new FormulaException($"variables does not contain variable `{Name}` value.");
        }

        public override bool HasVariable() => true;

        public override double EvalDerivative(string variable)
            => Name == variable ? 1.0 : 0.0;

        public override Formula Derive(string variable)
            => Name == variable ? One : Zero;

        public override double EvalDerivative(string variable, Dictionary<string, double> variables)
            => EvalDerivative(variable);

        public override List<string> GetVariables() => new() { Name };

        public override Formula Simplify() => new Variable(Name);

        internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            => builder.Append(Name.ToString(formatProvider));
    }
}
