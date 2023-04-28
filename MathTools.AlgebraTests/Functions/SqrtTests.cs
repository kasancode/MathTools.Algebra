using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathTools.Algebra.Functions.Tests
{
    [TestClass()]
    public class SqrtTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            {
                var formula = Formula.Parse("sqrt(0.4)/3.8");
                Assert.AreEqual(Math.Sqrt(0.4) / 3.8, formula.Eval(), error);
            }
            {
                var formula = Formula.Sqrt(0.4) / 3.8;
                Assert.AreEqual(Math.Sqrt(0.4) / 3.8, formula.Eval(), error);
            }
            {
                var formula = Formula.Parse("3.4/sqrt(0.8)");
                Assert.AreEqual(3.4 / Math.Sqrt(0.8), formula.Eval(), error);
            }
            {
                var formula = 3.4 / Formula.Sqrt(0.8);
                Assert.AreEqual(3.4 / Math.Sqrt(0.8), formula.Eval(), error);
            }
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-6;

            var vars = new { x = 0.2 };
            var formula = Formula.Parse("sqrt(0.4)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("3.4/sqrt(0.8)");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("x^4*sqrt(x)");
            Assert.AreEqual(0.0160997, formula.EvalDerivative("x", vars), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("sqrt(0.4)/3.8");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("3.4/sqrt(0.8)");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;
            var vars = new { x = 0.2 };

            var formula = Formula.Parse("x^4*sqrt(x)");

            var dif = formula.Derive("x");

            Assert.AreEqual(formula.EvalDerivative("x", vars), dif.Eval(vars), error);

            Assert.AreEqual(formula.EvalDerivative("x", vars), dif.Simplify().Eval(vars), error);

            var dif2 = Formula.Parse(dif.Simplify().ToString());
            Assert.AreEqual(formula.EvalDerivative("x", vars), dif2.Eval(vars), error);
        }
    }
}