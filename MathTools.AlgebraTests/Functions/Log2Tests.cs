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
    public class Log2Tests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("log2(0.4)/3.8");
            Assert.AreEqual(Math.Log2(0.4) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/log2(0.8)");
            Assert.AreEqual(3.4 / Math.Log2(0.8), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-6;

            var formula = Formula.Parse("log2(0.4)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("3.4/log2(0.8)");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("x^4*log2(x)");

            Assert.AreEqual(-0.0627601, formula.EvalDerivative("x", new { x = 0.2 }), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("log2(0.4)/3.8");
            var value = formula.Eval();
            formula.Simplify();
            Assert.AreEqual(value, formula.Eval(), error);

            formula = Formula.Parse("3.4/log2(0.8)");
            value = formula.Eval();
            formula.Simplify();
            Assert.AreEqual(value, formula.Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("x^4*log2(x)");

            var dif = formula.Derive("x");

            Assert.AreEqual(formula.EvalDerivative("x", new { x = 0.2 }), dif.Eval(new { x = 0.2 }), error);

            dif = dif.Simplify();
            Console.WriteLine(dif.ToString());
            var dif2 = Formula.Parse(dif.ToString());

            Assert.AreEqual(formula.EvalDerivative("x", new { x = 0.2 }), dif2.Eval(new { x = 0.2 }), error);
        }
    }
}