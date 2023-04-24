using System.Text;

namespace MathTools.Algebra.Functions
{
    public class Parenthesis : Formula
    {
        public Parenthesis(Formula formula)
        {
            this.SubFormulae = new() { formula };
        }
        public Parenthesis(double value) : this(new Constant(value)) { }

        public override double Eval(Dictionary<string, double> variables)
            => this.SubFormulae[0].Eval(variables);

        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable);

        internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
        {
            builder.Append('(');
            this.SubFormulae[0].BuildString(builder, format, formatProvider);
            builder.Append(')');
        }
    }
}
