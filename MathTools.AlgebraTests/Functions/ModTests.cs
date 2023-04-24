using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathTools.Algebra.Functions.Tests
{
    [TestClass()]
    public class ModTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("Sin(3.4)%3.8");
            Assert.AreEqual(Math.Sin(3.4) % 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4%sin(3.8+1.9)");
            Assert.AreEqual(3.4 % Math.Sin(3.8 + 1.9), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-10;

            {
                var formula = Formula.Parse("x^4*2%4*sin(x)");
                var x = new Variable("x");
                var xv = 20.0;
                var dif = (2.0 * Formula.Pow(x, 4.0) % 4.0 * Formula.Cos(x)) + (8.0 * Formula.Pow(x, 3.0) * Formula.Sin(x));
                var difValue = (2.0 * Math.Pow(xv, 4.0) % 4.0 * Math.Cos(xv)) + (8.0 * Math.Pow(xv, 3.0) * Math.Sin(xv));
                var dif2 = formula.Derive("x").Simplify();
                Console.WriteLine(dif);
                Console.WriteLine(dif2);
                Assert.AreEqual(dif.Eval(new { x = xv }), formula.EvalDerivative("x", new { x = xv }), error);
                Assert.AreEqual(difValue, formula.EvalDerivative("x", new { x = xv }), error);
            }
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("10+Sin(3.4)%3.8+20");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("2+3.4%Exp(3.8+1.9)*20");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("x^4*Exp(x)+x%20*x^2");
            var x = 20.0;
            var value = (Math.Pow(x, 4.0) * Math.Exp(x)) + (x % 20.0 * Math.Pow(x, 2.0));

            Assert.AreEqual(value, formula.Eval(new { x }), error);

            var dif = formula.Derive("x");
            Console.WriteLine(dif);
            Assert.AreEqual(formula.EvalDerivative("x", new { x }), dif.Eval(new { x }), error);

            var dif2 = Formula.Parse(dif.Simplify().ToString());
            Console.WriteLine(dif2);
            Assert.AreEqual(formula.EvalDerivative("x", new { x = 20.0 }), dif2.Eval(new { x = 20.0 }), error);

            Assert.AreEqual(1.0, 400.0 * (1 + (480 * Math.Exp(20.0))) / dif2.Eval(new { x = 20.0 }), error);
        }
    }
}