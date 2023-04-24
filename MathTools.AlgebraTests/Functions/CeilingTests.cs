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
    public class CeilingTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("Ceiling(3.4)/3.8");
            Assert.AreEqual(Math.Ceiling(3.4) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/Ceiling(3.8+1.9)");
            Assert.AreEqual(3.4 / Math.Ceiling(3.8 + 1.9), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-10;

            {
                var formula = Formula.Parse("x^4*2/4*Ceiling(x)");
                var x = new Variable("x");
                var xv = 20.2;
                var dif = 2.0 * Formula.Pow(x, 3.0) * Formula.Ceiling(x);
                var difValue = 2.0 * Math.Pow(xv, 3.0) * Math.Ceiling(xv);
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

            var formula = Formula.Parse("10+Ceiling(3.4)*3.8+20");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("2+3.4/Ceiling(3.8+1.9)*20");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("x^4*Ceiling(x)+x/20*x^2");
            var x = 20.5;
            var value = (Math.Pow(x, 4.0) * Math.Ceiling(x)) + (x / 20.0 * Math.Pow(x, 2.0));

            Assert.AreEqual(value, formula.Eval(new { x}), error);

            var dif = formula.Derive("x");
            Assert.AreEqual(formula.EvalDerivative("x", new { x }), dif.Eval(new { x }), error);

            var dif2 = Formula.Parse(dif.Simplify().ToString());
            Console.WriteLine(dif2);
            Assert.AreEqual(formula.EvalDerivative("x", new { x }), dif2.Eval(new { x }), error);

            Assert.AreEqual(1.0, (4.0 * Math.Pow(x, 3.0) * Math.Ceiling(x) + 3.0 * Math.Pow(x, 2.0) / 20.0) / dif2.Eval(new { x }), error);
        }
    }
}