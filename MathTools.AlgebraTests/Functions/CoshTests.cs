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
    public class CoshTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("cosh(3.4)/3.8");
            Assert.AreEqual(Math.Cosh(3.4) / 3.8, formula.Eval(), error);

            var formula2 = Formula.Cosh(3.4) / 3.8;
            Assert.AreEqual(Math.Cosh(3.4) / 3.8, formula2.Eval(), error);

            formula = Formula.Parse("3.4/cosh(3.8+1.9)");
            Assert.AreEqual(3.4 / Math.Cosh(3.8 + 1.9), formula.Eval(), error);

            formula2 = 3.4 / Formula.Cosh(3.8 + 1.9);
            Assert.AreEqual(3.4 / Math.Cosh(3.8 + 1.9), formula2.Eval(), error);

        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("cosh(3.4)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(null), error);

            formula = Formula.Parse("3.4/cosh(3.8+1.9)");
            Assert.AreEqual(0, formula.EvalDerivative(null), error);

            formula = Formula.Parse("x^4*cosh(x)");

            var vars = new Dictionary<string, double> { { "x", 20.0 } };
            var x = new Variable("x");

            Assert.AreEqual(32000 * (Math.Cosh(20) - 5 * Math.Sinh(20)), formula.EvalDerivative("x", vars), error);

            formula = Formula.Pow(x, 4) * Formula.Cosh(x);
            Assert.AreEqual(32000 * (Math.Cosh(20) - 5 * Math.Sinh(20)), formula.EvalDerivative("x", vars), error);

            formula = Formula.Pow("x", 4) * Formula.Cosh("x");
            Assert.AreEqual(32000 * (Math.Cosh(20) - 5 * Math.Sinh(20)), formula.EvalDerivative("x", vars), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("cosh(3.4)/3.8");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Cosh(3.4) / 3.8;
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("3.4/cosh(3.8+1.9)");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = 3.4 / Formula.Cosh(3.8 + 1.9);
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;
            {
                var formula = Formula.Parse("x^4*cosh(x)");
                var vars = new Dictionary<string, double> { { "x", 20.0 } };

                var dif = formula.Derive("x");

                Assert.AreEqual(formula.EvalDerivative("x", vars), dif.Eval(vars), error);

                dif = dif.Simplify();
                Console.WriteLine(dif.ToString());
                var dif2 = Formula.Parse(dif.ToString());

                Assert.AreEqual(formula.EvalDerivative("x", vars), dif2.Eval(vars), error);
            }

            {
                var x = new Variable("x");
                var formula = Formula.Pow(x, 4) * Formula.Cosh(x);
                var vars = new Dictionary<string, double> { { "x", 20.0 } };

                var dif = formula.Derive("x");

                Assert.AreEqual(formula.EvalDerivative("x", vars), dif.Eval(vars), error);

                dif = dif.Simplify();
                Console.WriteLine(dif.ToString());
                var dif2 = Formula.Parse(dif.ToString());

                Assert.AreEqual(formula.EvalDerivative("x", vars), dif2.Eval(vars), error);
            }
        }
    }
}