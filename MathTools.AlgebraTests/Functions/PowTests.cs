using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathTools.Algebra.Functions.Tests
{
    [TestClass()]
    public class PowTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("3.4^3.8");
            Assert.AreEqual(Math.Pow(3.4, 3.8), formula.Eval(), error);

            formula = Formula.Parse("3.4^3.8^1.9");
            Assert.AreEqual(Math.Pow(Math.Pow(3.4, 3.8), 1.9), formula.Eval(), error);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            {
                var formula = Formula.Parse("x^-2");
                Assert.AreEqual("x^-2", formula.ToString());
            }

            {
                var formula = Formula.Parse("x^-y");
                Assert.AreEqual("x^-y", formula.ToString());
            }

            {
                var formula = Formula.Parse("x^(1/y)");
                Assert.AreEqual("x^(1/y)", formula.ToString());
            }

            {
                var x = new Variable("x");
                var y = new Variable("y");

                var formula = Formula.Pow(x, -y);
                Assert.AreEqual("x^-y", formula.ToString());
            }

            {
                var x = new Variable("x");
                var y = new Variable("y");

                var formula = Formula.Pow(x, 1 / y);
                Assert.AreEqual("x^(1/y)", formula.ToString());
            }
        }
    }
}