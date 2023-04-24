using System.Text;

namespace MathTools.Algebra.Functions
{
    public class Sum : Formula
    {
        internal List<bool> Signs { get; init; }

        public Sum(List<Formula> formulae, List<bool> signs)
        {
            if (formulae.Count != signs.Count)
            {
                throw new ArgumentException($"fromula.Count must be same signs.Count.");
            }

            if (formulae.Count < 1)
            {
                throw new ArgumentException($"fromula.Count must be >= 1.");
            }

            var subFormulae = new List<Formula>();
            var subSigns = new List<bool>();

            for (var i = 0; i < formulae.Count; i++)
            {
                var formula = formulae[i];
                var sign = signs[i];
                if (formula is Sum sum)
                {
                    subFormulae.AddRange(sum.SubFormulae);
                    subSigns.AddRange(sum.Signs.Select(s => s == sign));
                }
                else
                {
                    subFormulae.Add(formula);
                    subSigns.Add(sign);
                }
            }

            this.SubFormulae = subFormulae;
            this.Signs = subSigns;
        }

        public override double Eval(Dictionary<string, double> variables)
        {
            var sum = 0.0;
            for (var i = 0; i < this.SubFormulae.Count; i++)
            {
                var formula = this.SubFormulae[i];

                sum += this.Signs[i]
                    ? formula.Eval(variables)
                    : -formula.Eval(variables);
            }

            return sum;
        }

        public override Formula Derive(string val)
        {
            var difs = this.SubFormulae.Select(s => s.Derive(val));
            return new Sum(difs.ToList(), this.Signs);
        }

        internal override Formula SpecificSimplify()
        {
            var constant = 0.0;
            var checkSubs = new List<Formula>();
            var checkSigns = new List<double>();
            var texts = new List<string>();

            for (var i = 0; i < this.SubFormulae.Count; i++)
            {
                var sub = this.SubFormulae[i];
                var sign = this.Signs[i];

                if (sub.HasVariable())
                {
                    var text = sub.ToString();

                    var index = texts.IndexOf(text);
                    if (index >= 0)
                    {
                        checkSigns[index] += sign ? 1 : -1;
                    }
                    else if (sub is Product { SubFormulae: [Constant c, ..] } subProduct)
                    {
                        var tempProduct = new Product(subProduct.SubFormulae.Skip(1).ToList(), subProduct.Signs.Skip(1).ToList());

                        var tempText = tempProduct.ToString();
                        var index2 = texts.IndexOf(tempText);
                        if (index2 >= 0)
                        {
                            checkSigns[index2] += sign ? c.Eval() : -c.Eval();
                        }
                        else
                        {
                            texts.Add(tempText);
                            checkSubs.Add(tempProduct);
                            checkSigns.Add(sign ? c.Eval() : -c.Eval());
                        }
                    }
                    else
                    {
                        texts.Add(text);
                        checkSubs.Add(sub);
                        checkSigns.Add(sign ? 1 : -1);
                    }
                }
                else
                {
                    // a + f(x) + b -> f(x) + (a + b)
                    constant += sign
                        ? sub.Eval()
                        : -sub.Eval();
                }
            }

            var newSubs = new List<Formula>();
            var newSigns = new List<bool>();

            foreach (var (sub, sign) in checkSubs.Zip(checkSigns))
            {
                if (sign == 1 || sign == -1)
                {
                    newSubs.Add(sub);
                    newSigns.Add(sign > 0);
                }
                else if (sign != 0)
                {
                    newSubs.Add(
                        new Product(new List<Formula> { new Constant(Math.Abs(sign)), sub },
                        new List<bool> { true, true }));
                    newSigns.Add(sign > 0);
                }
            }

            if (newSubs.Count == 0)
            {
                // sum(a) -> a
                return new Constant(constant);
            }

            if (newSubs.Count == 1 && constant == 0.0 && newSigns[0])
            {
                // f(x) + 0 -> f(x)
                return newSubs[0];
            }

            if (constant != 0.0)
            {
                newSubs.Add(new Constant(Math.Abs(constant)));
                newSigns.Add(constant > 0);
            }

            return new Sum(newSubs, newSigns);
        }

        internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
        {
            if (this.SubFormulae.Count < 1)
            {
                builder.Append('0');
                return;
            }

            void toStr(Formula sub) => sub.BuildString(builder, format, formatProvider);
            ;

            if (!this.Signs[0])
            {
                builder.Append('-');
            }

            toStr(this.SubFormulae[0]);
            for (var i = 1; i < this.SubFormulae.Count; i++)
            {
                builder.Append(this.Signs[i] ? '+' : '-');
                toStr(this.SubFormulae[i]);
            }
        }
    }
}
