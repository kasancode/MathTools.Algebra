using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathTools.Algebra.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace MathTools.Algebra.Functions.Tests
{
    [TestClass()]
    public class AcoshTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("acosh(3.4)/3.8");
            Assert.AreEqual(Math.Acosh(3.4) / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/acosh(3.8)");
            Assert.AreEqual(3.4 / Math.Acosh(3.8), formula.Eval(), error);
        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-6;

            var formula = Formula.Parse("acosh(3.4)/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("3.4/acosh(3.8)");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            var x = 2.2;
            formula = Formula.Parse("x^4*acosh(x)");
            var vars = new Dictionary<string, double> { { "x", x } };

            Console.WriteLine(formula.Derive("x").Simplify());

            Assert.AreEqual(
                Math.Pow(x, 3) * (x / (Math.Sqrt(x - 1) * Math.Sqrt(x + 1)) + 4 * Math.Acosh(x)),
                formula.EvalDerivative("x", vars), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("acosh(4.4)/3.8");
            var value = formula.Eval();
            formula = formula.Simplify();
            Assert.AreEqual(value, formula.Eval(), error);

            formula = Formula.Parse("3.4/acosh(4.8)");
            value = formula.Eval();
            formula = formula.Simplify();
            Assert.AreEqual(value, formula.Eval(), error);
        }

        [TestMethod()]
        public void GetDifferentialExpressionTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("x^4*acosh(x)");
            var vars = new Dictionary<string, double> { { "x", 3.2 } };

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