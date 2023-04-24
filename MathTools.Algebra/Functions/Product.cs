using System.Text;

namespace MathTools.Algebra.Functions
{
    public class Product : Formula
    {
        internal List<bool> Signs { get; init; }

        public Product(List<Formula> formulae, List<bool> signs)
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
                if (formula is Product product)
                {
                    subFormulae.AddRange(product.SubFormulae);
                    subSigns.AddRange(product.Signs.Select(s => s == sign));
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
            var product = 1.0;
            for (var i = 0; i < this.SubFormulae.Count; i++)
            {
                var formula = this.SubFormulae[i];

                if (this.Signs[i])
                {
                    product *= formula.Eval(variables);
                }
                else
                {
                    product /= formula.Eval(variables);
                }
            }

            return product;
        }

        public override Formula Derive(string val)
        {
            var difs = this.SubFormulae.Select(s => s.Derive(val)).ToList();
            var newSubs = new List<Formula>();
            var newSigns = new List<bool>();

            for (var i = 0; i < this.SubFormulae.Count; i++)
            {
                var products = new List<Formula>();
                var signs = new List<bool>();
                for (var j = 0; j < this.SubFormulae.Count; j++)
                {
                    if (i == j)
                    {
                        if (this.Signs[j])
                        {
                            products.Add(difs[j]);
                            signs.Add(true);
                        }
                        else
                        {
                            products.Add(difs[j] / new Pow(this.SubFormulae[j], 2.0));
                            signs.Add(true);
                        }
                    }
                    else
                    {
                        products.Add(this.SubFormulae[j]);
                        signs.Add(this.Signs[j]);
                    }
                }

                newSubs.Add(new Product(products, signs));
                newSigns.Add(this.Signs[i]);
            }

            return new Sum(newSubs, newSigns);
        }

        internal override Formula SpecificSimplify()
        {
            var constant = 1.0;
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
                    else if (sub is Pow { SubFormulae: [var b, Constant c] })
                    {
                        var tempText = b.ToString();
                        var index2 = texts.IndexOf(tempText);
                        if (index2 >= 0)
                        {
                            checkSigns[index2] += sign ? c.Eval() : -c.Eval();
                        }
                        else
                        {
                            texts.Add(tempText);
                            checkSubs.Add(b);
                            checkSigns.Add(sign ? c.Eval() : -c.Eval());
                        }
                    }
                    else if (sub is Sqrt { SubFormulae: [var s] })
                    {
                        var tempText = s.ToString();
                        var index2 = texts.IndexOf(tempText);
                        if (index2 >= 0)
                        {
                            checkSigns[index2] += sign ? 0.5 : -0.5;
                        }
                        else
                        {
                            texts.Add(tempText);
                            checkSubs.Add(s);
                            checkSigns.Add(sign ? 0.5 : -0.5);
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
                    // a * f(x) * b -> (a * b) * f(x)
                    if (sign)
                    {
                        constant *= sub.Eval();
                    }
                    else
                    {
                        constant /= sub.Eval();
                    }
                }
            }

            if (constant == 0.0)
            {
                // 0 * f(x) -> 0
                return Zero;
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
                else if (sign == 0.5 || sign == -0.5)
                {
                    newSubs.Add(new Sqrt(sub));
                    newSigns.Add(sign > 0);
                }
                else if (sign != 0)
                {
                    newSubs.Add(new Pow(sub, Math.Abs(sign)));
                    newSigns.Add(sign > 0);
                }
            }

            if (newSubs.Count == 0)
            {
                // product(a) -> a
                return new Constant(constant);
            }

            if (newSubs.Count == 1 && constant == 1.0 && newSigns[0])
            {
                // 1 * f(x) -> f(x)
                return newSubs[0];
            }

            if (constant != 1.0)
            {
                newSubs.Insert(0, new Constant(constant));
                newSigns.Insert(0, true);
            }

            return new Product(newSubs, newSigns);
        }

        internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
        {
            if (this.SubFormulae.Count < 1)
            {
                builder.Append('1');
                return;
            }

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

            if (!this.Signs[0])
            {
                builder.Append("1/");
            }

            toStr(this.SubFormulae[0]);
            for (var i = 1; i < this.SubFormulae.Count; i++)
            {
                builder.Append(this.Signs[i] ? '*' : '/');
                toStr(this.SubFormulae[i]);
            }
        }
    }
}
