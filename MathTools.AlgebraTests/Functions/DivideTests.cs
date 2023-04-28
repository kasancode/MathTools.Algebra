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
    public class DivideTests
    {
        [TestMethod()]
        public void EvalTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("3.4/3.8");
            Assert.AreEqual(3.4 / 3.8, formula.Eval(), error);

            formula = Formula.Parse("3.4/3.8/1.9");
            Assert.AreEqual(3.4 / 3.8 / 1.9, formula.Eval(), error);

            formula = Formula.Parse("3.4+3.8/1.9");
            Assert.AreEqual(3.4 + 3.8 / 1.9, formula.Eval(), error);

            formula = Formula.Parse("3.4-3.8/1.9");
            Assert.AreEqual(3.4 - 3.8 / 1.9, formula.Eval(), error);

        }

        [TestMethod()]
        public void EvalDerivativeTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("3.4/3.8");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);

            formula = Formula.Parse("3.4/3.8/1.9");
            Assert.AreEqual(0, formula.EvalDerivative(""), error);
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            var error = 1e-10;

            var formula = Formula.Parse("3.4/3.8");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);

            formula = Formula.Parse("3.4/3.8/1.9");
            Assert.AreEqual(formula.Eval(), formula.Simplify().Eval(), error);
        }
    }
}