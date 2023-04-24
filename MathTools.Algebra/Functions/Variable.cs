using System.Text;

namespace MathTools.Algebra.Functions
{
    public class Variable : Formula
    {
        public string Name { get; }

        public Variable(string name)
        {
            this.Name = name;
        }

        public override double Eval()
            => throw new FormulaException("Varialbes value is not set.");

        public override double Eval(Dictionary<string, double> variables)
        {
            return variables.TryGetValue(this.Name, out var value)
                ? value
                : throw new FormulaException($"variables does not contain variable `{this.Name}` value.");
        }

        public override bool HasVariable() => true;

        public override double EvalDerivative(string variable)
            => this.Name == variable ? 1.0 : 0.0;

        public override Formula Derive(string variable)
            => this.Name == variable ? One : Zero;

        public override double EvalDerivative(string variable, Dictionary<string, double> variables)
            => this.EvalDerivative(variable);

        public override List<string> GetVariables() => new() { this.Name };

        public override Formula Simplify() => new Variable(this.Name);

        internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            => builder.Append(this.Name.ToString(formatProvider));
    }
}
