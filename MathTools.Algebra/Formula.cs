using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;
using MathTools.Algebra.Functions;

namespace MathTools.Algebra
{
    public abstract partial class Formula :
        INumber<Formula>,
        IComparable,
        IComparable<Formula>,
        IEquatable<Formula>,
        IMinMaxValue<Formula>,
        IAdditionOperators<Formula, double, Formula>,
        ISubtractionOperators<Formula, double, Formula>,
        IMultiplyOperators<Formula, double, Formula>,
        IDivisionOperators<Formula, double, Formula>
    {
        public static Formula Zero => new Constant(0.0);

        static Formula INumberBase<Formula>.Zero => Zero;

        public static Formula NaN => new Constant(double.NaN);

        public static Formula Pi => new Constant(double.Pi);

        public static Formula Epsilon => new Constant(double.Epsilon);

        public static Formula Tau => new Constant(double.Tau);

        public static Formula One => new Constant(1.0);

        internal List<Formula> SubFormulae { get; init; } = new();

        public static Formula MaxValue => new Constant(double.MaxValue);

        public static Formula MinValue => new Constant(double.MinValue);

        static int INumberBase<Formula>.Radix => 2;

        static Formula IAdditiveIdentity<Formula, Formula>.AdditiveIdentity => Zero;

        static Formula IMultiplicativeIdentity<Formula, Formula>.MultiplicativeIdentity => One;

        public virtual List<string> GetVariables()
        {
            List<string> allVarList = new();

            if (this.SubFormulae is null || this.SubFormulae.Count < 1)
                return allVarList;

            for (var i = 0; i < this.SubFormulae.Count; i++)
            {
                var tempVarList = this.SubFormulae[i].GetVariables();
                if (tempVarList is not null)
                {
                    if (allVarList is null)
                    {
                        allVarList = tempVarList;
                    }
                    else
                    {
                        foreach (var v in tempVarList)
                        {
                            if(allVarList.All(av => av != v ))
                                allVarList.Add(v);
                        }
                    }
                }
            }

            return allVarList;
        }

        internal virtual Formula SpecificSimplify() => this;

        public virtual Formula Simplify()
        {
            var formula = this.BasicSimplify();
            var type = formula.GetType();

            for (var i = 0; i < 10; i++)
            {
                formula = formula.SpecificSimplify();
                if (type == formula.GetType())
                    return formula;

                type = formula.GetType();
            }

            return formula;
        }

        public abstract Formula Derive(string variable);

        public virtual double Eval() => this.Eval(new Dictionary<string, double>());

        public virtual double Eval(params double[] variables) {
            var varList = this.GetVariables();
            if (varList.Count != variables.Length)
                throw new ArgumentException($"variables.Length must be {varList.Count}.");

            var varDict = varList.Zip(variables).ToDictionary(items => items.First, items=>items.Second);

            return this.Eval(varDict);
        }

        public abstract double Eval(Dictionary<string, double> variables);

        public virtual double Eval(object variables)
        {
            var type = variables.GetType();
            var variableDict = type.GetProperties()
                .Where(p => p.CanRead && p.PropertyType == typeof(double))
                .ToDictionary(p => p.Name, p => (double)(p.GetValue(variables) ?? double.NaN));

            return this.Eval(variableDict);
        }

        public virtual double EvalDerivative(string variable)
        {
            var diffFormula = this.Derive(variable);
            return diffFormula.Eval();
        }

        public virtual double EvalDerivative(string variable, Dictionary<string, double> variables)
        {
            return this
                .Derive(variable)
                .Simplify()
                .Eval(variables);
        }

        public virtual double EvalDerivative(string variable, object variables)
        {
            var type = variables.GetType();
            var variableDict = type.GetProperties()
                .Where(p => p.CanRead && p.PropertyType == typeof(double))
                .ToDictionary(p => p.Name, p => (double)(p.GetValue(variables) ?? double.NaN));

            return this.EvalDerivative(variable, variableDict);
        }

        public virtual bool HasVariable() => this.SubFormulae.Any(f => f.HasVariable());

        internal static bool containsDelimiter(ReadOnlySpan<char> str)
        {
            foreach (var c in str)
            {
                if (delimiters.Contains(c))
                    return true;
            }

            return false;
        }

        internal static char[] delimiters = new[] { '(', ')', '+', '-', '*', '/', '^', ',', '=', '>', '<' };

        public static Formula Parse(string text)
            => TryParse(text, out var formula)
                ? formula
                : throw new FormulaException("Syntax error.");

        public static Formula Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
            => internalTryParse(s.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, provider, new(), out var formula)
                ? formula
                : throw new FormulaException("Syntax error.");

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Formula result)
            => internalTryParse(s.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, provider, new(), out result);

        public static Formula Parse(string s, IFormatProvider? provider)
            => internalTryParse(s.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, provider, new(), out var formula)
                ? formula
                : throw new FormulaException("Syntax error.");

        public static bool TryParse(string text, [MaybeNullWhen(false)] out Formula formula)
            => internalTryParse(text.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, null, new(), out formula);

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Formula result)
            => internalTryParse(s?.Trim() ?? "", NumberStyles.Float | NumberStyles.AllowThousands, provider, new(), out result);

        public static Formula Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
            => internalTryParse(s.Trim(), style, provider, new(), out var formula)
                ? formula
                : throw new FormulaException("Syntax error.");

        public static Formula Parse(string s, NumberStyles style, IFormatProvider? provider)
            => internalTryParse(s.Trim(), style, provider, new(), out var formula)
                ? formula
                : throw new FormulaException("Syntax error.");

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Formula result)
            => internalTryParse(s.Trim(), style, provider, new(), out result);

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Formula result)
            => internalTryParse(s?.Trim() ?? "", style, provider, new(), out result);

        internal static bool internalTryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, List<Variable> variables, [MaybeNullWhen(false)] out Formula formula)
        {
            if (s.Length < 1)
            {
                formula = Zero;
                return true;
            }

            var opPosition = -1;
            var funcNameLen = -1;
            var parent = 0;

            var text = s.Trim();

            for (var index = 0; index < text.Length; index++)
            {
                if (parent > 0)
                {
                    switch (text[index])
                    {
                        case '(':
                            parent++;
                            break;
                        case ')':
                            parent--;
                            break;
                    }

                    continue;
                }
                else
                {
                    switch (text[index])
                    {
                        case '(':
                            funcNameLen = index;
                            parent++;
                            break;
                        case '+':
                        case '-':
                            if (opPosition < 0 || (index > 0 && !(text[index - 1] is '+' or '-' or '*' or '/' or '^' or '%')))
                            {
                                opPosition = index;
                            }

                            break;
                        case '*':
                        case '/':
                        case '%':
                            if (opPosition < 0 || !(text[opPosition] is '+' or '-'))
                            {
                                opPosition = index;
                            }

                            break;
                        case '^':
                            if (opPosition < 0 || !(text[opPosition] is '+' or '-' or '*' or '/' or '%'))
                            {
                                opPosition = index;
                            }

                            break;
                    }
                }
            }

            if (parent > 0)
                throw new FormulaException("Syntax error. Missing ')'.");

            if (parent < 0)
                throw new FormulaException("Syntax error. Too many ')'.");

            if (opPosition < 0)
            {
                // not operator

                if (text[0] == '(')
                {
                    if (text[^1] != ')')
                    {
                        formula = Zero;
                        return false;
                    }

                    if (internalTryParse(text[1..^1], style, provider, variables, out var subFormula))
                    {
                        formula = new Parenthesis(subFormula);
                        return true;
                    }

                    formula = Zero;
                    return false;
                }
                else if (char.IsNumber(text[0]) || text[0] == '-' || text[0] == '+')
                {
                    var val = double.Parse(text, style, provider);
                    formula = new Constant(val);
                    return true;
                }
                else if (char.IsLetter(text[0]) || text[0] == '[')
                {
                    if (funcNameLen > 0)
                    {
                        // function
                        var funcName = text[..funcNameLen];

                        var subText = text[(funcNameLen + 1)..^1];
                        var subFormulae = new List<Formula>();
                        var last = 0;
                        for (var i = 0; i < subText.Length; i++)
                        {
                            if (subText[i] == ',')
                            {
                                if (internalTryParse(subText[last..i], style, provider, variables, out var sf))
                                {
                                    subFormulae.Add(sf);
                                    last = i + 1;
                                }
                                else
                                {
                                    formula = Zero;
                                    return false;
                                }
                            }
                        }

                        if (internalTryParse(subText[last..], style, provider, variables, out var subFormula))
                        {
                            subFormulae.Add(subFormula);
                        }
                        else
                        {
                            formula = Zero;
                            return false;
                        }

                        if (CreateFunction(funcName, subFormulae) is { } f)
                        {
                            formula = f;
                            return true;
                        }

                        formula = Zero;
                        return false;
                    }
                    // variable
                    if (!containsDelimiter(text))
                    {
                        foreach (var v in variables)
                        {
                            if (v.Name == text)
                            {
                                formula = v;
                                return true;
                            }
                        }

                        var variable = new Variable(text.ToString());
                        variables.Add(variable);
                        formula = variable;
                        return true;
                    }
                }

                formula = Zero;
                return false;

            }
            else
            {
                // operator

                if (!internalTryParse(text[0..opPosition], style, provider, variables, out var left)
                    || !internalTryParse(text[(opPosition + 1)..], style, provider, variables, out var right))
                {
                    formula = Zero;
                    return false;
                }

                if (opPosition == 0)
                {
                    if (text[0] == '+')
                    {
                        formula = right;
                        return true;
                    }
                    else if (text[0] == '-')
                    {
                        if (right.HasVariable())
                        {
                            formula = -right;
                            return true;
                        }
                        else
                        {
                            formula = new Constant(-right.Eval());
                            return true;
                        }
                    }
                    else
                    {
                        formula = Zero;
                        return false;
                    }
                }

                if (opPosition == text.Length - 1)
                {
                    formula = Zero;
                    return false;
                }

                formula = text[opPosition] switch
                {
                    '+' => new Sum(new List<Formula> { left, right }, new List<bool> { true, true }),
                    '-' => new Sum(new List<Formula> { left, right }, new List<bool> { true, false }),
                    '*' => new Product(new List<Formula> { left, right }, new List<bool> { true, true }),
                    '/' => new Product(new List<Formula> { left, right }, new List<bool> { true, false }),
                    '%' => new Mod(left, right),
                    '^' => new Pow(left, right),
                    _ => Zero
                };

                return formula is not Constant c || c.Value != 0.0;
            }
        }

        public int CompareTo(object? value) => value == null ? 1 : value is Formula f ? this.CompareTo(f) : throw new ArgumentException("Argument must be Formula.");

        public int CompareTo(Formula? other)
        {
            if (other == null)
            {
                return 1;
            }

            if (!this.HasVariable() && other.HasVariable())
            {
                return this.Eval().CompareTo(other.Eval());
            }

            var comp = (this - other).Simplify();
            if (comp.HasVariable())
            {
                throw new Exception("Formulae have variables, can not compare.");
            }

            var value = comp.Eval();
            return value < 0.0
                ? -1
                : value > 0.0 ? 1 : value == 0.0 ? 0 : double.IsNaN(this.Eval()) ? double.IsNaN(other.Eval()) ? 0 : -1 : 1;
        }

        public bool Equals([NotNullWhen(true)] Formula? other)
            => this.Simplify().ToString() == (other?.Simplify().ToString() ?? "");

        /// <inheritdoc cref="INumberBase{TSelf}.IsCanonical(TSelf)" />
        static bool INumberBase<Formula>.IsCanonical(Formula value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsComplexNumber(TSelf)" />
        static bool INumberBase<Formula>.IsComplexNumber(Formula value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsEvenInteger(TSelf)" />
        public static bool IsEvenInteger(Formula value) =>
             IsInteger(value) && (double.Abs(value.Eval() % 2) == 0);

        public static bool IsFinite(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsFinite(value.Eval());

        /// <inheritdoc cref="INumberBase{TSelf}.IsImaginaryNumber(TSelf)" />
        static bool INumberBase<Formula>.IsImaginaryNumber(Formula value) => false;

        static bool INumberBase<Formula>.IsInfinity(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsInfinity(value.Eval());

        /// <inheritdoc cref="INumberBase{TSelf}.IsInteger(TSelf)" />
        public static bool IsInteger(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsInteger(value.Eval());

        public static bool IsNaN(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsNaN(value.Eval());

        public static bool IsNegative(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsNegative(value.Eval());

        public static bool IsNegativeInfinity(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsNegativeInfinity(value.Eval());

        public static bool IsNormal(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsNormal(value.Eval());

        public static bool IsOddInteger(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsOddInteger(value.Eval());

        public static bool IsPositive(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsPositive(value.Eval());

        public static bool IsPositiveInfinity(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsPositiveInfinity(value.Eval());

        public static bool IsRealNumber(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsRealNumber(value.Eval());

        public static bool IsSubnormal(Formula value)
            => value.Simplify().HasVariable()
                ? throw new UnresolvedResultException()
                : double.IsSubnormal(value.Eval());

        static bool INumberBase<Formula>.IsZero(Formula value)
        {
            var f = value.Simplify();
            return f.HasVariable()
                ? throw new UnresolvedResultException()
                : f.Eval() == 0;
        }

        public static Formula MaxMagnitudeNumber(Formula x, Formula y) => MaxMagnitude(x, y);

        public static Formula MinMagnitudeNumber(Formula x, Formula y) => MinMagnitude(x, y);

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        {
            var result = this.ToString(format.ToString(), provider);

            if (result?.TryCopyTo(destination) ?? false)
            {
                charsWritten = result.Length;
                return true;
            }
            else
            {
                charsWritten = 0;
                return false;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            this.BuildString(builder, null, null);

            return builder.ToString();

        }

        public virtual string ToString(string? format, IFormatProvider? formatProvider)
        {
            var builder = new StringBuilder();
            this.BuildString(builder, format, formatProvider);

            return builder.ToString();
        }

        internal abstract void BuildString(StringBuilder builder, string? format, IFormatProvider? formatProvider);

        static bool INumberBase<Formula>.TryConvertFromChecked<TOther>(TOther value, out Formula result) => TryConvertFrom(value, out result);

        static bool TryConvertFrom<TOther>(TOther value, out Formula result) where TOther : INumberBase<TOther>
        {
            if (typeof(TOther).IsSubclassOf(typeof(Formula)))
            {
                result = value as Formula ?? Zero;
                return true;
            }
            else if (typeof(TOther) == typeof(double))
            {
                var actualValue = (double)(object)value;
                result = checked(new Constant(actualValue));
                return true;
            }
            else if (typeof(TOther) == typeof(short))
            {
                var actualValue = (short)(object)value;
                result = checked(new Constant(actualValue));
                return true;
            }
            else if (typeof(TOther) == typeof(long))
            {
                var actualValue = (long)(object)value;
                result = checked(new Constant(actualValue));
                return true;
            }
            else if (typeof(TOther) == typeof(nint))
            {
                var actualValue = (nint)(object)value;
                result = checked(new Constant(actualValue));
                return true;
            }
            else if (typeof(TOther) == typeof(sbyte))
            {
                var actualValue = (sbyte)(object)value;
                result = checked(new Constant(actualValue));
                return true;
            }
            else if (typeof(TOther) == typeof(float))
            {
                var actualValue = (float)(object)value;
                result = checked(new Constant(actualValue));
                return true;
            }
            else
            {
                result = Zero;
                return false;
            }
        }

        static bool INumberBase<Formula>.TryConvertToChecked<TOther>(Formula formula, [MaybeNullWhen(false)] out TOther result)
        {
            var opt = formula.Simplify();
            if (opt.HasVariable())
            {
                result = default;
                return false;
            }

            var value = opt.Eval();

            if (typeof(TOther) == typeof(byte))
            {
                var actualResult = checked((byte)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                var actualResult = checked((char)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualResult = checked((decimal)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                var actualResult = checked((ushort)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                var actualResult = checked((uint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualResult = checked((ulong)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualResult = checked((UInt128)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualResult = checked((nuint)value);
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool TryConvertTo<TOther>(Formula formula, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
        {
            var opt = formula.Simplify();
            if (opt.HasVariable())
            {
                result = default;
                return false;
            }

            var value = opt.Eval();

            if (typeof(TOther) == typeof(byte))
            {
                var actualResult = (byte)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(char))
            {
                var actualResult = (char)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(decimal))
            {
                var actualResult = (decimal)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ushort))
            {
                var actualResult = (ushort)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(uint))
            {
                var actualResult = (uint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(ulong))
            {
                var actualResult = (ulong)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(UInt128))
            {
                var actualResult = (UInt128)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else if (typeof(TOther) == typeof(nuint))
            {
                var actualResult = (nuint)value;
                result = (TOther)(object)actualResult;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        static bool INumberBase<Formula>.TryConvertFromSaturating<TOther>(TOther value, out Formula result) => TryConvertFrom(value, out result);

        static bool INumberBase<Formula>.TryConvertToSaturating<TOther>(Formula value, [MaybeNullWhen(false)] out TOther result) => TryConvertTo(value, out result);

        static bool INumberBase<Formula>.TryConvertFromTruncating<TOther>(TOther value, out Formula result) => TryConvertFrom(value, out result);

        static bool INumberBase<Formula>.TryConvertToTruncating<TOther>(Formula value, [MaybeNullWhen(false)] out TOther result) => TryConvertTo(value, out result);

        /// <inheritdoc cref="IAdditionOperators{TSelf, TOther, TResult}.op_Addition(TSelf, TOther)" />
        public static Formula operator +(Formula left, Formula right) => new Sum(new List<Formula> { left, right }, new List<bool> { true, true });
        /// <inheritdoc cref="IAdditionOperators{TSelf, TOther, TResult}.op_Addition(TSelf, TOther)" />
        public static Formula operator +(double left, Formula right) => new Sum(new List<Formula> { new Constant(left), right }, new List<bool> { true, true });
        /// <inheritdoc cref="IAdditionOperators{TSelf, TOther, TResult}.op_Addition(TSelf, TOther)" />
        public static Formula operator +(Formula left, double right) => new Sum(new List<Formula> { left, new Constant(right) }, new List<bool> { true, true });

        public static Formula operator -(Formula left, Formula right) => new Sum(new List<Formula> { left, right }, new List<bool> { true, false });
        public static Formula operator -(double left, Formula right) => new Sum(new List<Formula> { new Constant(left), right }, new List<bool> { true, false });
        public static Formula operator -(Formula left, double right) => new Sum(new List<Formula> { left, new Constant(right) }, new List<bool> { true, false });

        public static Formula operator *(Formula left, Formula right) => new Product(new List<Formula> { left, right }, new List<bool> { true, true });
        public static Formula operator *(double left, Formula right) => new Product(new List<Formula> { new Constant(left), right }, new List<bool> { true, true });
        public static Formula operator *(Formula left, double right) => new Product(new List<Formula> { left, new Constant(right) }, new List<bool> { true, true });

        public static Formula operator /(Formula left, Formula right) => new Product(new List<Formula> { left, right }, new List<bool> { true, false });
        public static Formula operator /(double left, Formula right) => new Product(new List<Formula> { new Constant(left), right }, new List<bool> { true, false });
        public static Formula operator /(Formula left, double right) => new Product(new List<Formula> { left, new Constant(right) }, new List<bool> { true, false });

        public static bool operator >(Formula left, Formula right)
        {
            var opt = (left - right).Simplify();

            return opt.HasVariable()
                ? throw new UnresolvedResultException()
                : opt.Eval() > 0.0;
        }

        public static bool operator >=(Formula left, Formula right)
        {
            var opt = (left - right).Simplify();
            return opt.HasVariable()
                ? throw new UnresolvedResultException()
                : opt.Eval() >= 0.0;
        }

        public static bool operator <(Formula left, Formula right)
        {
            var opt = (left - right).Simplify();
            return opt.HasVariable()
                ? throw new UnresolvedResultException()
                : opt.Eval() < 0.0;
        }

        public static bool operator <=(Formula left, Formula right)
        {
            var opt = (left - right).Simplify();
            return opt.HasVariable()
                ? throw new UnresolvedResultException()
                : opt.Eval() <= 0.0;
        }

        public static Formula operator %(Formula left, Formula right) => Mod(left, right);

        public static Formula operator %(Formula left, double right) => Mod(left, right);

        public static Formula operator %(double left, Formula right) => Mod(left, right);

        /// <inheritdoc cref="IDecrementOperators{TSelf}.op_Decrement(TSelf)" />
        static Formula IDecrementOperators<Formula>.operator --(Formula value) => value - 1.0;

        public static bool operator ==(Formula? left, Formula? right)
        {
            var optLeft = left?.Simplify().ToString() ?? "";
            var optRight = right?.Simplify().ToString() ?? "";

            return optLeft == optRight;
        }

        public static bool operator !=(Formula? left, Formula? right) => !(left == right);

        /// <inheritdoc cref="IDecrementOperators{TSelf}.op_Increment(TSelf)" />
        static Formula IIncrementOperators<Formula>.operator ++(Formula value) => value + 1.0;

        /// <inheritdoc cref="IUnaryNegationOperators{TSelf, TResult}.op_UnaryNegation(TSelf)" />
        public static Formula operator -(Formula value) => new Sum(new List<Formula> { value }, new List<bool> { false });

        /// <inheritdoc cref="IUnaryPlusOperators{TSelf, TResult}.op_UnaryPlus(TSelf)" />
        public static Formula operator +(Formula value) => value;

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return ReferenceEquals(this, obj)
                ? true
                : obj is Formula f
                    ? this.Equals(f)
                    : false;
        }

        public override int GetHashCode() => this.ToString().GetHashCode();

    }
}
