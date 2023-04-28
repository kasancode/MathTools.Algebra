using System.Text;

namespace MathTools.Algebra.Functions
{
    public partial class Pow : Formula
    {
        public override double Eval(Dictionary<string, double> variables)
            => Math.Pow(this.SubFormulae[0].Eval(variables), this.SubFormulae[1].Eval(variables));

        public override Formula Derive(string variable)
        {
            var fx = this.SubFormulae[0];
            var dfx = this.SubFormulae[0].Derive(variable);
            var gx = this.SubFormulae[1];
            var dgx = this.SubFormulae[1].Derive(variable);
            return Pow(fx, gx - 1.0) * ((gx * dfx) + (fx * Log(fx) * dgx));
        }

        internal override Formula SpecificSimplify()
        {
            if (this.SubFormulae[0] is Constant c1)
            {
                if (c1.Value == 0.0)
                    // Pow(0, f(x)) -> 0
                    return Zero;

                if (c1.Value == 1.0)
                    // Pow(1, f(x)) -> 1
                    return new Constant(1.0);
            }

            if (this.SubFormulae[1] is Constant c2)
            {
                if (c2.Value == 0.0)
                    // Pow(f(x), 0) -> 1
                    return new Constant(1.0);

                if (c2.Value == 1.0)
                    // Pow(f(x), 1) -> f(x)
                    return this.SubFormulae[0];
            }

            if (this.SubFormulae is [Pow pow, var f1])
            {
                // Pow(Pow(f(x), g(x)), h(x)) -> Pow(f(x), g(x) * h(x))
                return Pow(pow.SubFormulae[0], (f1 * pow.SubFormulae[1]).Simplify());
            }

            if (this.SubFormulae is [Sqrt sqrt, var f2])
            {
                // Pow(Sqrt(f(x)), g(x)) -> Pow(f(x), g(x) * 0.5)
                return Pow(sqrt.SubFormulae[0], (f2 * 0.5).Simplify());
            }

            if (this.SubFormulae is [Constant { Value: 10.0 }, Log10 log10])
            {
                // Pow(10.0, Log10(f(x)) -> f(x)
                return log10.SubFormulae[0];
            }

            if (this.SubFormulae is
                [var a1, Product
                {
                    Signs: [true, false],
                    SubFormulae: [
                        Log { SubFormulae: [var f3] },
                        Log { SubFormulae: [var a2] }]
                }] &&
                a1 == a2)
            {
                // Pow(a, Log(f(x))/Log(a)) -> f(x)
                return f3;
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

            switch (format)
            {
                case "C#":
                    builder.Append("Math.Pow(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default:
                    toStr(this.SubFormulae[0]);
                    for (var i = 1; i < this.SubFormulae.Count; i++)
                    {
                        builder.Append('^');
                        toStr(this.SubFormulae[i]);
                    }
                    break;

            }

        }
    }
}
