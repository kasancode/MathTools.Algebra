using System.Text;

namespace MathTools.Algebra.Functions
{
    public class Mod : Formula
    {
        public Mod(Formula f1, Formula f2)
        {
            this.SubFormulae = new() { f1, f2 };
        }
        public Mod(Formula f1, double v2) : this(f1, new Constant(v2)) { }
        public Mod(double v1, Formula f2) : this(new Constant(v1), f2) { }
        public Mod(double v1, double v2) : this(new Constant(v1), new Constant(v2)) { }

        public override double Eval(Dictionary<string, double> variables)
            => this.SubFormulae[0].Eval(variables) % this.SubFormulae[1].Eval(variables);

        public override Formula Derive(string variable)
            => this.SubFormulae[0].Derive(variable)
                - (this.SubFormulae[1].Derive(variable)
                * Floor(this.SubFormulae[0] / this.SubFormulae[1]));

        internal override Formula SpecificSimplify()
        {
            if (this is { SubFormulae: [_, Constant { Value: 0.0 }] })
            {
                // Mod(f(x), 0) -> NaN
                return NaN;
            }

            return base.SpecificSimplify();
        }

        internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
        {
            void toStr(Formula sub)
            {
                if (sub is Sum { SubFormulae.Count: > 1 } or Product { SubFormulae.Count: > 1 } or Product { Signs: [false, ..] })
                {
                    builder.Append('(');
                    sub.BuildString(builder, format, formatProvider);
                    builder.Append(')');
                }
                else
                {
                    sub.BuildString(builder, format, formatProvider);
                }
            };

            toStr(this.SubFormulae[0]);
            for (var i = 1; i < this.SubFormulae.Count; i++)
            {
                builder.Append('%');
                toStr(this.SubFormulae[i]);
            }
        }
    }
}
