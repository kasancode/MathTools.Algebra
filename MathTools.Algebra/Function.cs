using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathTools.Algebra.Functions;

namespace MathTools.Algebra
{
    public partial class Formula
    {
        public static Formula Mod(Formula v1, Formula v2) => new Mod(v1, v2);
        public static Formula Mod(double v1, Formula v2) => new Mod(v1, v2);
        public static Formula Mod(Formula v1, double v2) => new Mod(v1, v2);
        public static Formula Mod(double v1, double v2) => new Mod(v1, v2);
        public static Formula Pow(Formula v1, Formula v2) => new Pow(v1, v2);
        public static Formula Pow(double v1, Formula v2) => new Pow(v1, v2);
        public static Formula Pow(Formula v1, double v2) => new Pow(v1, v2);
        public static Formula Pow(double v1, double v2) => new Pow(v1, v2);
        public static Formula Exp(Formula v1) => new Exp(v1);
        public static Formula Exp(double v1) => new Exp(v1);
        public static Formula Sqrt(Formula v1) => new Sqrt(v1);
        public static Formula Sqrt(double v1) => new Sqrt(v1);
        public static Formula Log(Formula v1) => new Log(v1);
        public static Formula Log(double v1) => new Log(v1);
        public static Formula Log10(Formula v1) => new Log10(v1);
        public static Formula Log10(double v1) => new Log10(v1);
        public static Formula Sin(Formula v1) => new Sin(v1);
        public static Formula Sin(double v1) => new Sin(v1);
        public static Formula Cos(Formula v1) => new Cos(v1);
        public static Formula Cos(double v1) => new Cos(v1);
        public static Formula Tan(Formula v1) => new Tan(v1);
        public static Formula Tan(double v1) => new Tan(v1);
        public static Formula Sinh(Formula v1) => new Sinh(v1);
        public static Formula Sinh(double v1) => new Sinh(v1);
        public static Formula Cosh(Formula v1) => new Cosh(v1);
        public static Formula Cosh(double v1) => new Cosh(v1);
        public static Formula Tanh(Formula v1) => new Tanh(v1);
        public static Formula Tanh(double v1) => new Tanh(v1);
        public static Formula Asin(Formula v1) => new Asin(v1);
        public static Formula Asin(double v1) => new Asin(v1);
        public static Formula Acos(Formula v1) => new Acos(v1);
        public static Formula Acos(double v1) => new Acos(v1);
        public static Formula Atan(Formula v1) => new Atan(v1);
        public static Formula Atan(double v1) => new Atan(v1);
        public static Formula Asinh(Formula v1) => new Asinh(v1);
        public static Formula Asinh(double v1) => new Asinh(v1);
        public static Formula Acosh(Formula v1) => new Acosh(v1);
        public static Formula Acosh(double v1) => new Acosh(v1);
        public static Formula Atanh(Formula v1) => new Atanh(v1);
        public static Formula Atanh(double v1) => new Atanh(v1);
        public static Formula Abs(Formula v1) => new Abs(v1);
        public static Formula Abs(double v1) => new Abs(v1);
        public static Formula If(Formula v1, Formula v2, Formula v3) => new If(v1, v2, v3);
        public static Formula If(double v1, Formula v2, Formula v3) => new If(v1, v2, v3);
        public static Formula If(Formula v1, double v2, Formula v3) => new If(v1, v2, v3);
        public static Formula If(double v1, double v2, Formula v3) => new If(v1, v2, v3);
        public static Formula If(Formula v1, Formula v2, double v3) => new If(v1, v2, v3);
        public static Formula If(double v1, Formula v2, double v3) => new If(v1, v2, v3);
        public static Formula If(Formula v1, double v2, double v3) => new If(v1, v2, v3);
        public static Formula If(double v1, double v2, double v3) => new If(v1, v2, v3);
        public static Formula Min(Formula v1, Formula v2) => new Min(v1, v2);
        public static Formula Min(double v1, Formula v2) => new Min(v1, v2);
        public static Formula Min(Formula v1, double v2) => new Min(v1, v2);
        public static Formula Min(double v1, double v2) => new Min(v1, v2);
        public static Formula Max(Formula v1, Formula v2) => new Max(v1, v2);
        public static Formula Max(double v1, Formula v2) => new Max(v1, v2);
        public static Formula Max(Formula v1, double v2) => new Max(v1, v2);
        public static Formula Max(double v1, double v2) => new Max(v1, v2);
        public static Formula Ceiling(Formula v1) => new Ceiling(v1);
        public static Formula Ceiling(double v1) => new Ceiling(v1);
        public static Formula Floor(Formula v1) => new Floor(v1);
        public static Formula Floor(double v1) => new Floor(v1);
        public static Formula MinMagnitude(Formula v1, Formula v2) => new MinMagnitude(v1, v2);
        public static Formula MinMagnitude(double v1, Formula v2) => new MinMagnitude(v1, v2);
        public static Formula MinMagnitude(Formula v1, double v2) => new MinMagnitude(v1, v2);
        public static Formula MinMagnitude(double v1, double v2) => new MinMagnitude(v1, v2);
        public static Formula MaxMagnitude(Formula v1, Formula v2) => new MaxMagnitude(v1, v2);
        public static Formula MaxMagnitude(double v1, Formula v2) => new MaxMagnitude(v1, v2);
        public static Formula MaxMagnitude(Formula v1, double v2) => new MaxMagnitude(v1, v2);
        public static Formula MaxMagnitude(double v1, double v2) => new MaxMagnitude(v1, v2);

        internal static Formula CreateFunction(ReadOnlySpan<char> funcName, List<Formula> subFuncs)
        {
            var lowerName = new Span<char>(funcName.ToArray());
            funcName.ToLower(lowerName, null);
            lowerName = lowerName.Trim();

            return lowerName switch
            {
                "mod" => new Mod(subFuncs[0], subFuncs[1]),
                "pow" => new Pow(subFuncs[0], subFuncs[1]),
                "exp" => new Exp(subFuncs[0]),
                "sqrt" => new Sqrt(subFuncs[0]),
                "log" => new Log(subFuncs[0]),
                "log10" => new Log10(subFuncs[0]),
                "sin" => new Sin(subFuncs[0]),
                "cos" => new Cos(subFuncs[0]),
                "tan" => new Tan(subFuncs[0]),
                "sinh" => new Sinh(subFuncs[0]),
                "cosh" => new Cosh(subFuncs[0]),
                "tanh" => new Tanh(subFuncs[0]),
                "asin" => new Asin(subFuncs[0]),
                "acos" => new Acos(subFuncs[0]),
                "atan" => new Atan(subFuncs[0]),
                "asinh" => new Asinh(subFuncs[0]),
                "acosh" => new Acosh(subFuncs[0]),
                "atanh" => new Atanh(subFuncs[0]),
                "abs" => new Abs(subFuncs[0]),
                "if" => new If(subFuncs[0], subFuncs[1], subFuncs[2]),
                "min" => new Min(subFuncs[0], subFuncs[1]),
                "max" => new Max(subFuncs[0], subFuncs[1]),
                "ceiling" => new Ceiling(subFuncs[0]),
                "floor" => new Floor(subFuncs[0]),
                "minmagnitude" => new MinMagnitude(subFuncs[0], subFuncs[1]),
                "maxmagnitude" => new MaxMagnitude(subFuncs[0], subFuncs[1]),
                _ => throw new NotSupportedException($"The function `{funcName}` is not Supported.")
            };
        }

        internal Formula BasicSimplify()
        {
            if (!this.HasVariable())
            {
                return new Constant(this.Eval());
            }

            var optSubs = this.SubFormulae.Select(f => f.Simplify()).ToList();
            Formula opt = this switch {
                Sum sum => new Sum(optSubs, sum.Signs),
                Product product => new Product(optSubs, product.Signs),
                Parenthesis => optSubs[0],
                Mod _ => new Mod(optSubs[0], optSubs[1]),
                Pow _ => new Pow(optSubs[0], optSubs[1]),
                Exp _ => new Exp(optSubs[0]),
                Sqrt _ => new Sqrt(optSubs[0]),
                Log _ => new Log(optSubs[0]),
                Log10 _ => new Log10(optSubs[0]),
                Sin _ => new Sin(optSubs[0]),
                Cos _ => new Cos(optSubs[0]),
                Tan _ => new Tan(optSubs[0]),
                Sinh _ => new Sinh(optSubs[0]),
                Cosh _ => new Cosh(optSubs[0]),
                Tanh _ => new Tanh(optSubs[0]),
                Asin _ => new Asin(optSubs[0]),
                Acos _ => new Acos(optSubs[0]),
                Atan _ => new Atan(optSubs[0]),
                Asinh _ => new Asinh(optSubs[0]),
                Acosh _ => new Acosh(optSubs[0]),
                Atanh _ => new Atanh(optSubs[0]),
                Abs _ => new Abs(optSubs[0]),
                If _ => new If(optSubs[0], optSubs[1], optSubs[2]),
                Min _ => new Min(optSubs[0], optSubs[1]),
                Max _ => new Max(optSubs[0], optSubs[1]),
                Ceiling _ => new Ceiling(optSubs[0]),
                Floor _ => new Floor(optSubs[0]),
                MinMagnitude _ => new MinMagnitude(optSubs[0], optSubs[1]),
                MaxMagnitude _ => new MaxMagnitude(optSubs[0], optSubs[1]),
                _ => throw new NotSupportedException($"The function is not Supported.")
            };

            if (opt.HasVariable())
            {
                return opt;
            }
            return new Constant(opt.Eval());
        }
    }

    namespace Functions
    {

        public partial class Exp
        {
            public Exp(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Exp(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Exp(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Exp(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Exp(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Sqrt
        {
            public Sqrt(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Sqrt(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Sqrt(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Sqrt(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Sqrt(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Log
        {
            public Log(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Log(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Log(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Log(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Log(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Log10
        {
            public Log10(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Log10(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Log10(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Log10(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Log10(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Sin
        {
            public Sin(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Sin(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Sin(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Sin(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Sin(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Cos
        {
            public Cos(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Cos(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Cos(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Cos(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Cos(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Tan
        {
            public Tan(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Tan(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Tan(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Tan(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Tan(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Sinh
        {
            public Sinh(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Sinh(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Sinh(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Sinh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Sinh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Cosh
        {
            public Cosh(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Cosh(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Cosh(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Cosh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Cosh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Tanh
        {
            public Tanh(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Tanh(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Tanh(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Tanh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Tanh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Asin
        {
            public Asin(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Asin(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Asin(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Asin(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Asin(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Acos
        {
            public Acos(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Acos(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Acos(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Acos(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Acos(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Atan
        {
            public Atan(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Atan(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Atan(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Atan(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Atan(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Asinh
        {
            public Asinh(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Asinh(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Asinh(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Asinh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Asinh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Acosh
        {
            public Acosh(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Acosh(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Acosh(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Acosh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Acosh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Atanh
        {
            public Atanh(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Atanh(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Atanh(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Atanh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Atanh(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Abs
        {
            public Abs(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Abs(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Abs(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Abs(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Abs(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Min
        {
            public Min(Formula v1, Formula v2)
            {
                this.SubFormulae = new List<Formula> { v1, v2 };
            }

            public Min(double v1, Formula v2) : this(new Constant(v1), v2) { }

            public Min(Formula v1, double v2) : this(v1, new Constant(v2)) { }

            public Min(double v1, double v2) : this(new Constant(v1), new Constant(v2)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Min(this.SubFormulae[0].Eval(variables), this.SubFormulae[1].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Min(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Min(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Max
        {
            public Max(Formula v1, Formula v2)
            {
                this.SubFormulae = new List<Formula> { v1, v2 };
            }

            public Max(double v1, Formula v2) : this(new Constant(v1), v2) { }

            public Max(Formula v1, double v2) : this(v1, new Constant(v2)) { }

            public Max(double v1, double v2) : this(new Constant(v1), new Constant(v2)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Max(this.SubFormulae[0].Eval(variables), this.SubFormulae[1].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Max(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Max(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Ceiling
        {
            public Ceiling(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Ceiling(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Ceiling(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Ceiling(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Ceiling(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class Floor
        {
            public Floor(Formula v1)
            {
                this.SubFormulae = new List<Formula> { v1 };
            }

            public Floor(double v1) : this(new Constant(v1)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.Floor(this.SubFormulae[0].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.Floor(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("Floor(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class MinMagnitude
        {
            public MinMagnitude(Formula v1, Formula v2)
            {
                this.SubFormulae = new List<Formula> { v1, v2 };
            }

            public MinMagnitude(double v1, Formula v2) : this(new Constant(v1), v2) { }

            public MinMagnitude(Formula v1, double v2) : this(v1, new Constant(v2)) { }

            public MinMagnitude(double v1, double v2) : this(new Constant(v1), new Constant(v2)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.MinMagnitude(this.SubFormulae[0].Eval(variables), this.SubFormulae[1].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.MinMagnitude(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("MinMagnitude(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }

        public partial class MaxMagnitude
        {
            public MaxMagnitude(Formula v1, Formula v2)
            {
                this.SubFormulae = new List<Formula> { v1, v2 };
            }

            public MaxMagnitude(double v1, Formula v2) : this(new Constant(v1), v2) { }

            public MaxMagnitude(Formula v1, double v2) : this(v1, new Constant(v2)) { }

            public MaxMagnitude(double v1, double v2) : this(new Constant(v1), new Constant(v2)) { }

            public override double Eval(Dictionary<string, double> variables)
                => Math.MaxMagnitude(this.SubFormulae[0].Eval(variables), this.SubFormulae[1].Eval(variables));

            internal override void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider)
            {
                switch(format){
                case "C#" :
                    builder.Append("Math.MaxMagnitude(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;

                default :
                    builder.Append("MaxMagnitude(");
                    this.SubFormulae[0].BuildString(builder, format, formatProvider);
                    builder.Append(',');
                    this.SubFormulae[1].BuildString(builder, format, formatProvider);
                    builder.Append(')');
                    break;
                }
            }
        }
    }
}
