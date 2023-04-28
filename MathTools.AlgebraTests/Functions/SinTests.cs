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
    public class SinTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("sin(3.4)/3.8");
            Assert.AreEqual(Math.Sin(3.4) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/sin(3.8+1.9)");
            Assert.AreEqual(3.4 / Math.Sin(3.8 + 1.9), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("SIN(3.4)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("3.4/SIN(3.8+1.9)");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("x^4*sin(x)");

            Assert.AreEqual(32000 * (Math.Sin(20) + 5 * Math.Cos(20)), formula.EvalDerivative("x", new { x = 20.0 }), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("SIN(3.4)/3.8");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("3.4/SIN(3.8+1.9)");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("x^4*sin(x)");

            var dif = formula.Derive("x");
            Assert.AreEqual(formula.EvalDerivative("x", new { x = 20.0 }), dif.Eval(new { x = 20.0 }), error);

            var dif2 = Formula.Parse(dif.Simplify().ToString());
            Assert.AreEqual(formula.EvalDerivative("x", new { x = 20.0 }), dif2.Eval(new { x = 20.0 }), error);
        }
    }
}