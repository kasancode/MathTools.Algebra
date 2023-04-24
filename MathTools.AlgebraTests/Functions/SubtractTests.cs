using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathTools.Algebra.Functions.Tests
{
    [TestClass()]
    public class SubtractTests
    {
        [TestMethod()]
        public void SubtractTest()
        {
            var av = 1.0;
            var bv = 2.0;
            var cv = 3.0;
            var xv = 4.0;
            var yv = 5.0;

            var a = new Constant(av);
            var b = new Constant(bv);
            var c = new Constant(cv);
            var x = new Variable("x");
            var y = new Variable("y");

            {
                var result = a - b;
                Assert.AreEqual(av - bv, result.Eval());
            }
            {
                var result = a + b - c;
                Assert.AreEqual(av + bv - cv, result.Eval());
            }
            {
                var result = x - a + y - b;
                var vResult = xv - av + yv - bv;

                Assert.AreEqual(
                    vResult,
                    result.Eval(new Dictionary<string, double>() {
                    {"x", xv},
                    {"y", yv}
                    }));
            }
            {
                var formula = Formula.Parse("(1^1)*-1/1");
                Assert.AreEqual(-1.0, formula.Eval());
            }
            {
                var text = "(x^4)*-1*1/Sqrt(1-x^2)";
                var formula = Formula.Parse(text);
                Assert.AreEqual(text, formula.ToString());
            }
            {
                var text = "(x^(4-1))*(4*1+x*Log(x)*0)*Acos(x)+(x^4)*-1*1/Sqrt(1-x^2)";
                var formula = Formula.Parse(text);
                Assert.AreEqual(text, formula.ToString());
            }  
        }

        [TestMethod()]
        public void ToStringTest()
        {
            var aValue = 1.0;
            var bValue = 2.0;
            var cValue = 3.0;

            var a = new Constant(aValue);
            var b = new Constant(bValue);
            var c = new Constant(cValue);
            var x = new Variable("x");
            var y = new Variable("y");

            {
                var result = a + b - c;
                Assert.AreEqual("1+2-3", result.ToString());
            }
            {
                var result = x - a + y - b;
                Assert.AreEqual("x-1+y-2", result.ToString());
            }
            {
                var formula = x - aValue - (bValue - cValue);
                Assert.AreEqual(
                    5.0 - aValue - (bValue - cValue),
                    formula.Eval(new { x = 5.0 }));
            }
            {
                var b_c = b - c;
                var formula = x - aValue - b_c;
                Assert.AreEqual(
                    5.0 - aValue - (bValue - cValue),
                    formula.Eval(new { x = 5.0 }));

                Assert.AreEqual("x-1-2+3", formula.ToString());
            }
            {
                var formula = Formula.Parse("(x+1)^2-(x+1)^2");
                Assert.AreEqual("0", formula.Simplify().ToString());
            }
        }

        [TestMethod()]
        public void SimplifyTest()
        {
            {
                var formula = Formula.Parse("x+-2/x");
                Assert.AreEqual("x-2/x", formula.Simplify().ToString());
            }
            {
                var formula = Formula.Parse("x-x");
                Assert.AreEqual(0.0, formula.Simplify().Eval());
            }
            {
                var formula = Formula.Parse("x+-2*x");
                Assert.AreEqual("-x", formula.Simplify().ToString());
            }
        }
    }
}