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
    public class AcosTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("acos(0.4)/3.8");
            Assert.AreEqual(Math.Acos(0.4) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/acos(0.8)");
            Assert.AreEqual(3.4 / Math.Acos(0.8), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-6;

            var formula = Formula.Parse("acos(0.4)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("3.4/acos(0.8)");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("x^4*acos(x)");
            var vars = new Dictionary<string, double> { { "x", 0.2 } };

            Assert.AreEqual(0.042189, formula.EvalDerivative("x", vars), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("acos(0.4)/3.8");
            var value = formula.Eval();
            formula = formula.Simplify();
            Assert.AreEqual(value, formula.Eval(), error);

            formula = Formula.Parse("3.4/acos(0.8)");
            value = formula.Eval();
            formula = formula.Simplify();
            Assert.AreEqual(value, formula.Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("x^4*acos(x)");
            var vars = new Dictionary<string, double> { { "x", 0.2 } };

            var dif = formula.Derive("x");

            Assert.AreEqual(formula.EvalDerivative("x", vars), dif.Eval(vars), error);

            {
                Console.WriteLine(dif.ToString());
                var dif2 = Formula.Parse(dif.ToString() ?? throw new Exception("`dif.ToString()` is null."));
                Console.WriteLine(dif2.ToString());

                Assert.AreEqual(formula.EvalDerivative("x", vars), dif2.Eval(vars), error);
            }
            {
                dif = dif.Simplify();
                Console.WriteLine(dif.ToString());
                var dif2 = Formula.Parse(dif.ToString() ?? throw new Exception("`dif.ToString()` is null."));

                Assert.AreEqual(formula.EvalDerivative("x", vars), dif2.Eval(vars), error);
            }
        }
    }
}