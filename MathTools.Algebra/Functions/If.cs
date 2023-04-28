using System.Text;

namespace MathTools.Algebra.Functions
{
    public partial class If : Formula
    {
        public override double Eval(Dictionary<string, double> variables)
        {
            var condition = this.SubFormulae[0].Eval(variables);
            return condition >= 0
                ? this.SubFormulae[1].Eval(variables)
                : this.SubFormulae[2].Eval(variables);
        }

        public override Formula Derive(string variable)
            => If(
                this.SubFormulae[0],
                this.SubFormulae[1].Derive(variable),
                this.SubFormulae[2].Derive(variable));

        internal override Formula SpecificSimplify()
        {
            if (!this.SubFormulae[0].HasVariable())
            {
                // if(a>=0, f(x), g(x)) -> f(x)
                // if(a<0, f(x), g(x)) -> g(x)
                return this.SubFormulae[0].Eval() >= 0.0
                     ? this.SubFormulae[1]
                     : this.SubFormulae[2];
            }

            return base.SpecificSimplify();
        }

        internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
        {
            switch (format)
            {
                case "C#":
                    builder.Append('(');
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(">=0.0 ? ");
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(':');
                    this.SubFormulae[2].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default:
                    builder.Append("If(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[2].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
            }
        }
    }
}
