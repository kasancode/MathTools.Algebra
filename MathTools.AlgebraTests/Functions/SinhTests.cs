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
    public class SinhTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("sinh(3.4)/3.8");
            Assert.AreEqual(Math.Sinh(3.4) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/sinh(3.8+1.9)");
            Assert.AreEqual(3.4 / Math.Sinh(3.8 + 1.9), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-8;

            var formula = Formula.Parse("sinh(3.4)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(null), error);

            formula = Formula.Parse("3.4/sinh(3.8+1.9)");
            Assert.AreEqual(0, formula.EvalDerivative(null), error);

            formula = Formula.Parse("x^4*sinh(x)");
            Assert.AreEqual(
                32000 * (Math.Sinh(20) + 5 * Math.Cosh(20)),
                formula.EvalDerivative("x", new { x = 20.0 }), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("sinh(3.4)/3.8");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("3.4/sinh(3.8+1.9)");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;
            var vars = new { x = 20.0 };

            var formula = Formula.Parse("x^4*sinh(x)");
            var dif = formula.Derive("x");

            Assert.AreEqual(formula.EvalDerivative("x", vars), dif.Eval(vars), error);

            var dif2 = Formula.Parse(dif.Simplify().ToString());
            Assert.AreEqual(formula.EvalDerivative("x", vars), dif2.Eval(vars), error);
        }
    }
}