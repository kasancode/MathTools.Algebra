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
    public class Atan2Tests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("atan2(0.4, 0.5)/3.8");
            Assert.AreEqual(Math.Atan2(0.4, 0.5) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/atan2(0.8, 0.2)");
            Assert.AreEqual(3.4 / Math.Atan2(0.8, 0.2), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-6;

            var formula = Formula.Parse("atan2(0.4, 0.3)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("3.4/atan2(0.8, 0.5)");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("x^4*atan2(x, 0.2)");
            var vars = new Dictionary<string, double> { { "x", 0.2 } };

            Assert.AreEqual(0.0211327, formula.EvalDerivative("x", vars), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("atan2(0.4,0.2)/3.8");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("3.4/atan2(0.8,0.3)");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("x^4*atan2(x,0.5)");
            var vars = new Dictionary<string, double> { { "x", 0.2} };
            var dif = formula.Derive("x");

            Assert.AreEqual(formula.EvalDerivative("x", vars), dif.Eval(vars), error);

            dif = dif.Simplify();
            Console.WriteLine(dif.ToString());
            var dif2 = Formula.Parse(dif.ToString());

            Assert.AreEqual(formula.EvalDerivative("x", vars), dif2.Eval(vars), error);
        }
    }
}