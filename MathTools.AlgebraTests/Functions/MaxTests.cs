using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathTools.Algebra.Functions.Tests
{
    [TestClass()]
    public class MaxTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("Max(3.4,2.0)/3.8");
            Assert.AreEqual(Math.Max(3.4, 2.0) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/Max(3.8+1.9,5.0)");
            Assert.AreEqual(3.4 / Math.Max(3.8 + 1.9, 5.0), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-10;

            void check(double xv)
            {
                var formula = Formula.Parse("x^4*2/4*Max(x,x*2)");
                var x = new Variable("x");
                var dif = Formula.If(x, 5.0 * Formula.Pow(x, 4.0), 5.0 / 2.0 * Formula.Pow(x, 4.0));
                var difValue = xv >= 0.0 ? 5.0 * Math.Pow(xv, 4.0) : 5.0 / 2.0 * Math.Pow(xv, 4.0);
                var dif2 = formula.Derive("x").Simplify();
                Console.WriteLine(dif);
                Console.WriteLine(dif2);
                Assert.AreEqual(dif.Eval(new { x = xv }), formula.EvalDerivative("x", new { x = xv }), error);
                Assert.AreEqual(difValue, formula.EvalDerivative("x", new { x = xv }), error);
            }

            check(20.5);
            check(-10.5);
            check(0.0);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("10+Max(3.4,3.0)*3.8+20");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("2+3.4/Max(3.8+1.9,5.0)*20");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            void check(double x)
            {
                var formula = Formula.Parse("x^4*Max(x,Exp(x))+x/20*x^2");
                var value = (Math.Pow(x, 4.0) * Math.Max(x, Math.Exp(x))) + (x / 20.0 * Math.Pow(x, 2.0));

                Assert.AreEqual(value, formula.Eval(new { x }), error);

                var dif = Formula.Parse(formula.Derive("x").Simplify().ToString());
                Console.WriteLine(dif);
                Assert.AreEqual(formula.EvalDerivative("x", new { x }), dif.Eval(new { x }), error);

                Assert.AreEqual(
                    Math.Exp(x) >= x
                    ? (Math.Exp(x) * Math.Pow(x, 4.0)) + (4.0 * Math.Exp(x) * Math.Pow(x, 3.0)) + (3.0 / 20.0 * Math.Pow(x, 2.0))
                    : (5.0 * Math.Pow(x, 4.0)) + (3.00 / 20.0 * Math.Pow(x, 2.0)),
                    dif.Eval(new { x }),
                    error);
            }

            check(20.5);
            check(-10.5);
            check(0.0);
        }
    }
}