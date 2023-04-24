using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathTools.Algebra.Functions.Tests
{
    [TestClass()]
    public class AbsTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("abs(-0.4)/3.8");
            Assert.AreEqual(Math.Abs(-0.4) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/abs(0.8)");
            Assert.AreEqual(3.4 / Math.Abs(0.8), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-6;

            var formula = Formula.Parse("x^4*abs(x)");

            Assert.AreEqual(
                0.008,
                formula.EvalDerivative("x", new { x = 0.2 }),
                error);

            var dif = formula.Derive("x").Simplify();
            Assert.AreEqual(
                0.008,
                dif.Eval(new { x = 0.2 }),
                error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("x^4*abs(x)");

            formula = formula.Simplify();
            var dif = formula.Derive("x");
            dif = dif.Simplify();

            var vars = new { x = 0.2 };
            Assert.AreEqual(formula.EvalDerivative("x", vars), dif.Eval(vars), error);

            Console.WriteLine(dif.ToString());
            var dif2 = Formula.Parse(dif.ToString() ?? throw new Exception("`dif.ToString()` is null."));

            Assert.AreEqual(formula.EvalDerivative("x", vars), dif2.Eval(vars), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            {
                var formula = Formula.Parse("Abs(0.4)/3.8");
                Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
            }
            {
                var formula = Formula.Parse("Abs(-0.4)/3.8");
                Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
            }
            {
                var formula = Formula.Parse("3.4/Abs(-0.8)");
                Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
            }
            {
                var formula = Formula.Parse("3.4/Abs(0.8)");
                Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
            }
        }
    }
}