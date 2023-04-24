using MathTools.Algebra;
using MathTools.Algebra.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathTools.AlgebraTests
{
    [TestClass()]
    public class FormulaTests
    {
        [TestMethod()]
        public void ParseTest()
        {
            {
                var formula = Formula.Parse("1 + 2 + 3 * 4");
                Console.WriteLine($"{formula}={formula.Eval()}");
                Assert.AreEqual(1.0 + 2.0 + 3.0 * 4.0, formula.Eval());
            }

            {
                var formula = Formula.Parse("( 1 + 2 + 3 ) * 4");
                Console.WriteLine($"{formula}={formula.Eval()}");
                Assert.AreEqual((1.0 + 2.0 + 3.0) * 4.0, formula.Eval());
            }

            {
                var formula = Formula.Parse("1-2+3/4");
                Console.WriteLine($"{formula}={formula.Eval()}");
                Assert.AreEqual(1.0 - 2.0 + 3.0 / 4.0, formula.Eval());
            }

            {
                var formula = Formula.Parse("1+2^3-4");
                Console.WriteLine($"{formula}={formula.Eval()}");
                Assert.AreEqual(1.0 + Math.Pow(2.0, 3.0) - 4.0, formula.Eval());
            }

            {
                var formula = Formula.Parse("(1+2+3)^4");
                Console.WriteLine($"{formula}={formula.Eval()}");
                Assert.AreEqual(Math.Pow(1.0 + 2.0 + 3.0, 4.0), formula.Eval());
            }

            {
                var formula = Formula.Parse("1-log(2+3)^4");
                Console.WriteLine($"{formula}={formula.Eval()}");
                Assert.AreEqual(1.0 - Math.Pow(Math.Log(2.0 + 3.0), 4.0), formula.Eval());
            }

            {
                var formula = Formula.Parse("1-log(2+3)^4");
                formula = 1 - formula * 2;
                Console.WriteLine($"{formula}={formula.Eval()}");
                Assert.AreEqual(1.0 - (1.0 - Math.Pow(Math.Log(2.0 + 3.0), 4.0)) * 2.0, formula.Eval());
            }

            {
                var formula = Formula.Parse("1-log(2+x1)^4");
                var vars = new Dictionary<string, double> { { "x1", 3.0 } };
                Console.WriteLine($"{formula}={formula.Eval(vars)}");
                Assert.AreEqual(1.0 - Math.Pow(Math.Log(2.0 + 3.0), 4.0), formula.Eval(vars));
            }

            {
                var formula = Formula.Parse("x1-Sin(2+x1)^x2");
                var vars = new Dictionary<string, double> {
                    { "x1", 3.0 },
                    { "x2", 5.0 }
                };
                Console.WriteLine($"{formula}={formula.Eval(vars)}");
                Assert.AreEqual(3.0 - Math.Pow(Math.Sin(2.0 + 3.0), 5.0), formula.Eval(vars));
            }

            {
                var formula = Formula.Parse("x1-Sin(2+x1)^x2");
                var vars = new Dictionary<string, double> {
                    { "x1", 3.0 },
                    { "x2", 5.0 }
                };
                Console.WriteLine($"{formula}={formula.Eval(vars)}");
                Assert.AreEqual(3.0 - Math.Pow(Math.Sin(2.0 + 3.0), 5.0), formula.Eval(vars));
            }

            {
                var formula = Formula.Parse("[1]-Sin(2+[1])^[2]");
                var vars = new Dictionary<string, double> {
                    { "[1]", 3.0 },
                    { "[2]", 5.0 }
                };
                Console.WriteLine($"{formula}={formula.Eval(vars)}");
                Assert.AreEqual(3.0 - Math.Pow(Math.Sin(2.0 + 3.0), 5.0), formula.Eval(vars));
            }
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            {
                // f(x) +- a * g(x) -> f(x) - a * g(x)
                // f(x) -- a * g(x) -> f(x) + a * g(x)
                var formula = Formula.Parse("x+-a*y--a*x*y");
                Assert.AreEqual("x-a*y+a*x*y", formula.Simplify().ToString());
            }

            {
                // f(x) +- 1 * g(x) -> f(x) - g(x)
                // f(x) -- 1 * g(x) -> f(x) + g(x)
                var formula = Formula.Parse("x+-1*y--1*x");
                Assert.AreEqual("2*x-y", formula.Simplify().ToString());
            }

            {
                // f(x) - f(x) -> 0
                var formula = Formula.Parse("Sin(x)-Sin(x)");
                Assert.AreEqual("0", formula.Simplify().ToString());
            }

            {
                // a + f(x) + b -> f(x) + (a + b)
                var formula = Formula.Parse("4+Sin(x)-3");
                Assert.AreEqual("Sin(x)+1", formula.Simplify().ToString());
            }

            {
                // sum(f(x)) -> f(x)
                var formula = new Sum(new List<Formula> { new Variable("x") }, new List<bool> { true });
                Assert.IsTrue(formula.Simplify() is Variable);
                Assert.AreEqual("x", formula.Simplify().ToString());
            }

            {
                // f(x) + 0 -> f(x)
                var formula = Formula.Parse("Sin(x)-3+3");
                Assert.IsTrue(formula.Simplify() is Sin);
                Assert.AreEqual("Sin(x)", formula.Simplify().ToString());
            }

            {
                // f(x) + a
                var formula = Formula.Parse("3+Sin(x)+3");
                Assert.AreEqual("Sin(x)+6", formula.Simplify().ToString());
            }

            {
                // f(x) +- a -> f(x) - a
                var formula = Formula.Parse("Sin(x)+-3");
                Assert.AreEqual("Sin(x)-3", formula.Simplify().ToString());
            }

            {
                // f(x) / f(x) -> 1
                var formula = Formula.Parse("Sin(x)/Sin(x)");
                Assert.AreEqual("1", formula.Simplify().ToString());
            }

            {
                // a * f(x) * b -> (a * b) * f(x)
                var formula = Formula.Parse("6*Sin(x)/3");
                Assert.AreEqual("2*Sin(x)", formula.Simplify().ToString());
            }

            {
                // product(a) -> a
                var formula = new Product(new List<Formula> { new Constant(3.0) }, new List<bool> { true });
                Assert.IsTrue(formula.Simplify() is Constant);
                Assert.AreEqual("3", formula.Simplify().ToString());
            }

            {
                // 0 * f(x) -> 0
                var formula = Formula.Parse("0*Sin(x)/3");
                Assert.AreEqual("0", formula.Simplify().ToString());
            }

            {
                // 1 * f(x) -> f(x)
                var formula = Formula.Parse("3*Sin(x)/3");
                Assert.IsTrue(formula.Simplify() is Sin);
                Assert.AreEqual("Sin(x)", formula.Simplify().ToString());
            }

            {
                // if(a>=0, f(x), g(x)) -> f(x)
                var formula = Formula.Parse("If(1,Sin(x),Cos(x))");
                Assert.IsTrue(formula.Simplify() is Sin);
                Assert.AreEqual("Sin(x)", formula.Simplify().ToString());
            }

            {
                // if(a>=0, f(x), g(x)) -> f(x)
                var formula = Formula.Parse("If(0,Sin(x),Cos(x))");
                Assert.IsTrue(formula.Simplify() is Sin);
                Assert.AreEqual("Sin(x)", formula.Simplify().ToString());
            }

            {
                // if(a<0, f(x), g(x)) -> g(x)
                var formula = Formula.Parse("If(-1,Sin(x),Cos(x))");
                Assert.IsTrue(formula.Simplify() is Cos);
                Assert.AreEqual("Cos(x)", formula.Simplify().ToString());
            }

            {
                // Pow(0, f(x)) -> 0
                var formula = Formula.Parse("0^Sin(x)");
                Assert.AreEqual("0", formula.Simplify().ToString());
            }

            {
                // Pow(1, f(x)) -> 1
                var formula = Formula.Parse("1^Sin(x)");
                Assert.AreEqual("1", formula.Simplify().ToString());
            }

            {
                // Pow(f(x), 0) -> 1
                var formula = Formula.Parse("Sin(x)^0");
                Assert.AreEqual("1", formula.Simplify().ToString());
            }

            {
                // Pow(f(x), 1) -> f(x)
                var formula = Formula.Parse("Sin(x)^1");
                Assert.IsTrue(formula is Pow);
                Assert.IsTrue(formula.Simplify() is Sin);
                Assert.AreEqual("Sin(x)", formula.Simplify().ToString());
            }

            {
                // f(x) % 0 -> NaN
                var formula = Formula.Parse("x%0");
                Assert.IsTrue(double.IsNaN(formula.Simplify().Eval()));
            }

            {
                var formula = Formula.Parse("x*x^2/x^3");
                Assert.IsTrue(formula is Product);
                Assert.IsTrue(formula.Simplify() is Constant);
                Assert.AreEqual("1", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("x*x^2/x^5");
                Assert.IsTrue(formula is Product);
                Assert.IsTrue(formula.Simplify() is Product);
                Assert.AreEqual("1/x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Log(x)*Log(x)^2/Log(x)^5");
                Assert.IsTrue(formula is Product);
                Assert.IsTrue(formula.Simplify() is Product);
                Assert.AreEqual("1/Log(x)^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("x*Sqrt(x)");
                Assert.IsTrue(formula is Product);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^1.5", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("x/Sqrt(x)");
                Assert.IsTrue(formula is Product);
                Assert.IsTrue(formula.Simplify() is Sqrt);
                Assert.AreEqual("Sqrt(x)", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("x^1.5/x");
                Assert.IsTrue(formula is Product);
                Assert.IsTrue(formula.Simplify() is Sqrt);
                Assert.AreEqual("Sqrt(x)", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Sin(Asin(x^2))");
                Assert.IsTrue(formula is Sin);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Sinh(Asinh(x^2))");
                Assert.IsTrue(formula is Sinh);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Cos(Acos(x^2))");
                Assert.IsTrue(formula is Cos);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Cosh(Acosh(x^2))");
                Assert.IsTrue(formula is Cosh);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Tan(Atan(x^2))");
                Assert.IsTrue(formula is Tan);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Tanh(Atanh(x^2))");
                Assert.IsTrue(formula is Tanh);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Exp(Log(x^2))");
                Assert.IsTrue(formula is Exp);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Log(Exp(x^2))");
                Assert.IsTrue(formula is Log);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Log10(10^(x^2))");
                Assert.IsTrue(formula is Log10);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("10^Log10(x^2)");
                Assert.IsTrue(formula is Pow);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("a^(log(x^2)/log(a))");
                Assert.IsTrue(formula is Pow);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("x^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("(Sin(x)^2)^3");
                Assert.IsTrue(formula is Pow);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("Sin(x)^6", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Sqrt(Sin(x))^4");
                Assert.IsTrue(formula is Pow);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("Sin(x)^2", formula.Simplify().ToString());
            }

            {
                var formula = Formula.Parse("Sin(x)^2^3");
                Assert.IsTrue(formula is Pow);
                Assert.IsTrue(formula.Simplify() is Pow);
                Assert.AreEqual("Sin(x)^6", formula.Simplify().ToString());
            }
        }

        [TestMethod()]
        public void GreaterThanTest()
        {
            {
                var formula1 = Formula.Parse("1+2");
                var formula2 = Formula.Parse("3*5");
                Assert.IsFalse(formula1 > formula2);
            }

            {
                var formula1 = Formula.Parse("20+2*Exp(x)");
                var formula2 = Formula.Parse("-3+2*Exp(x)");
                Assert.IsTrue(formula1 > formula2);
            }

            {
                var formula1 = Formula.Parse("-3+2*Exp(x)");
                var formula2 = Formula.Parse("-3+2*Exp(x)");
                Assert.IsFalse(formula1 > formula2);
            }

            {
                var formula1 = Formula.Parse("1+2*x");
                var formula2 = Formula.Parse("3*5");
                Assert.ThrowsException<UnresolvedResultException>(() => formula1 > formula2);
            }
        }

        [TestMethod()]
        public void GreaterEqualTest()
        {
            {
                var formula1 = Formula.Parse("1+2");
                var formula2 = Formula.Parse("3*5");
                Assert.IsFalse(formula1 >= formula2);
            }

            {
                var formula1 = Formula.Parse("20+2*Exp(x)");
                var formula2 = Formula.Parse("-3+2*Exp(x)");
                Assert.IsTrue(formula1 >= formula2);
            }

            {
                var formula1 = Formula.Parse("-3+2*Exp(x)");
                var formula2 = Formula.Parse("-3+2*Exp(x)");
                Assert.IsTrue(formula1 >= formula2);
            }

            {
                var formula1 = Formula.Parse("1+2*x");
                var formula2 = Formula.Parse("3*5");
                Assert.ThrowsException<UnresolvedResultException>(() => formula1 >= formula2);
            }
        }

        [TestMethod()]
        public void LowerThanTest()
        {
            {
                var formula1 = Formula.Parse("1+2");
                var formula2 = Formula.Parse("3*5");
                Assert.IsTrue(formula1 < formula2);
            }

            {
                var formula1 = Formula.Parse("20+2*Exp(x)");
                var formula2 = Formula.Parse("-3+2*Exp(x)");
                Assert.IsFalse(formula1 < formula2);
            }

            {
                var formula1 = Formula.Parse("-3+2*Exp(x)");
                var formula2 = Formula.Parse("-3+2*Exp(x)");
                Assert.IsFalse(formula1 < formula2);
            }

            {
                var formula1 = Formula.Parse("1+2*x");
                var formula2 = Formula.Parse("3*5");
                Assert.ThrowsException<UnresolvedResultException>(() => formula1 < formula2);
            }
        }

        [TestMethod()]
        public void LowerEqualTest()
        {
            {
                var formula1 = Formula.Parse("1+2");
                var formula2 = Formula.Parse("3*5");
                Assert.IsTrue(formula1 <= formula2);
            }

            {
                var formula1 = Formula.Parse("20+2*Exp(x)");
                var formula2 = Formula.Parse("-3+2*Exp(x)");
                Assert.IsFalse(formula1 <= formula2);
            }

            {
                var formula1 = Formula.Parse("-3+2*Exp(x)");
                var formula2 = Formula.Parse("-3+2*Exp(x)");
                Assert.IsTrue(formula1 <= formula2);
            }

            {
                var formula1 = Formula.Parse("1+2*x");
                var formula2 = Formula.Parse("3*5");
                Assert.ThrowsException<UnresolvedResultException>(() => formula1 <= formula2);
            }
        }
    }
}