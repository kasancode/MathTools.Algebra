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
    public class AsinhTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("asinh(2.4)/3.8");
            Assert.AreEqual(Math.Asinh(2.4) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/asinh(3.8)");
            Assert.AreEqual(3.4 / Math.Asinh(3.8), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-8;

            var formula = Formula.Parse("asinh(2.4)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("3.4/asinh(2.8)");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("x^4*asinh(x)");

            var x = 2.2;
            var vars = new Dictionary<string, double> { { "x", x } };

            Assert.AreEqual(
                Math.Pow(x,3)*(x/Math.Sqrt(Math.Pow(x,2)+1)+4*Math.Asinh(x)),
                formula.EvalDerivative("x", vars), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("asinh(2.4)/3.8");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("3.4/asinh(2.8)");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("x^4*asinh(x)");

            var vars = new Dictionary<string, double> { { "x", 0.2 } };

            var dif = formula.Derive("x");

            Assert.AreEqual(formula.EvalDerivative("x", vars), dif.Eval(vars), error);

            dif = dif.Simplify();
            Console.WriteLine(dif.ToString());

            var dif2 = Formula.Parse(dif.ToString());

            Assert.AreEqual(formula.EvalDerivative("x", vars), dif2.Eval(vars), error);
        }
    }
}