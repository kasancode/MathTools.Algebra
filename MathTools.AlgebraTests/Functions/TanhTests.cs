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
    public class TanhTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("tanh(3.4)/3.8");
            Assert.AreEqual(Math.Tanh(3.4) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/tanh(3.8+1.9)");
            Assert.AreEqual(3.4 / Math.Tanh(3.8 + 1.9), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-8;

            var formula = Formula.Parse("tanh(3.4)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("3.4/tanh(3.8+1.9)");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("x^4*tanh(x)");

            Assert.AreEqual(32000 * (5 / Math.Pow(Math.Cosh(20), 2) + Math.Sinh(20) / Math.Cosh(20)), formula.EvalDerivative("x", new { x = 20.0 }), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("tanh(3.4)/3.8");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("3.4/tanh(3.8+1.9)");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }


        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-8;

            var formula = Formula.Parse("x^4*tanh(x)/exp(x^2)");
            var dif = formula.Derive("x");

            Assert.AreEqual(formula.EvalDerivative("x", new { x = 20.0 }), dif.Eval(new { x = 20.0 }), error);

            var dif2 = Formula.Parse(dif.Simplify().ToString());
            Console.WriteLine(dif2);

            Assert.AreEqual(formula.EvalDerivative("x", new { x = 20.0 }), dif2.Eval(new { x = 20.0 }), error);

            Assert.AreEqual(-1.21957999918747881570e-167, dif2.Eval(new { x = 20.0 }), error);
        }
    }
}