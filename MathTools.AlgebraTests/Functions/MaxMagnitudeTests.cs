using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathTools.Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTools.Algebra.Functions.Tests
{
    [TestClass()]
    public class MaxMagnitudeTests
    {
        [TestClass()]
        public class MaxTests
        {
            [TestMethod()]
            public void EvalTest()
            {
                var error = 1e-10;

                var formula = Formula.Parse("MaxMagnitude(-3.4,2.0)/3.8");
                Assert.AreEqual(Math.MaxMagnitude(-3.4, 2.0) / 3.8, formula.Eval(), error);

                formula = Formula.Parse("3.4/MaxMagnitude(-3.8+1.9,5.0)");
                Assert.AreEqual(3.4 / Math.MaxMagnitude(-3.8 + 1.9, 5.0), formula.Eval(), error);
            }

            [TestMethod()]
            public void EvalDerivativeTest()
            {
                var error = 1e-10;

                void check(double xv)
                {
                    var formula = Formula.Parse("x^4*2/4*MaxMagnitude(-x,x*2)");
                    var x = new Variable("x");
                    var dif = Formula.If(Formula.Abs(-x) - Formula.Abs(x * 2),
                        -5.0 * Formula.Pow(x, 4.0) / 2,
                        5.0 * Formula.Pow(x, 4.0));
                    var difValue = Math.Abs(-xv) - Math.Abs(xv * 2) >= 0.0
                        ? -5.0 * Math.Pow(xv, 4.0) / 2
                        : 5.0 * Math.Pow(xv, 4.0);
                    var dif2 = formula.Derive("x").Simplify();
                    Console.WriteLine(dif);
                    Console.WriteLine(dif2);
                    Assert.AreEqual(dif.Eval(new { x = xv }), formula.EvalDerivative("x", new { x = xv }), error);
                    Assert.AreEqual(difValue, formula.EvalDerivative("x", new { x = xv }), error);
                }

                check(20.5);
                check(-10.5);
            }

            [TestMethod()]
            public void SimplifyTest()
            {
                var error = 1e-10;

                var formula = Formula.Parse("10+MaxMagnitude(3.4,3.0)*3.8+20");
                Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

                formula = Formula.Parse("2+3.4/MaxMagnitude(3.8+1.9,5.0)*20");
                Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
            }

            [TestMethod()]
            public void GetDifferentialExpressionTest()
            {
                var error = 1e-10;

                void check(double x)
                {
                    var formula = Formula.Parse("x^4*MaxMagnitude(-x,Exp(x))");
                    var value = (Math.Pow(x, 4.0) * Math.MaxMagnitude(-x, Math.Exp(x)));

                    Assert.AreEqual(value, formula.Eval(new { x }), error);

                    var dif = Formula.Parse(formula.Derive("x").Simplify().ToString());
                    Console.WriteLine(dif);
                    Assert.AreEqual(formula.EvalDerivative("x", new { x }), dif.Eval(new { x }), error);

                    Assert.AreEqual(
                        Math.Abs(-x) - Math.Abs(Math.Exp(x)) >= 0.0
                        ? -5.0 * Math.Pow(x, 4.0)
                        : Math.Exp(x) * Math.Pow(x, 3.0) * (x + 4.0),
                        dif.Eval(new { x }),
                        error);
                }

                check(20.5);
                check(-10.5);
                check(0.0);
            }
        }
    }
}