using System.Text;

namespace MathTools.Algebra
{
    public class Constant : Formula
    {
        public double Value { get; }

        public Constant(double value)
        {
            Value = value;
        }

        public override Formula Derive(string? variable) => new Constant(0.0);

        public override double Eval() => Value;

        public override double Eval(Dictionary<string, double> variables) => Eval();

        public override double EvalDerivative(string? variable) => 0.0;

        public override double EvalDerivative(string variable, Dictionary<string, double> variables) => 0.0;

        public override List<string> GetVariables() => new();

        public override bool HasVariable() => false;

        public override Formula Simplify() => new Constant(Value);

        internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
        {
            switch (format)
            {
                case "C#":
                    builder.Append(Value.ToString());
                    break;
                default:
                    builder.Append(Value.ToString(format, formatProvider));
                    break;
            }
        }
    }
}
