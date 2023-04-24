using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathTools.Algebra.Functions.Tests
{
    [TestClass()]
    public class AddTests
    {
        [TestMethod()]
        public void AddTest()
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
                var sum = a + b;
                Assert.AreEqual(av + bv, sum.Eval());
            }
            {
                var sum = a + b + c;
                Assert.AreEqual(av + bv + cv, sum.Eval());
            }
            {
                var sum = 1.0 + b + 3.0;
                Assert.AreEqual(1.0 + bv + 3.0, sum.Eval());
            }
            {
                var sum = x + a + y + b;
                var vSum = xv + av + yv + bv;

                Assert.AreEqual(
                    vSum,
                    sum.Eval(new Dictionary<string, double>() {
                    {"x", xv},
                    {"y", yv}
                    }));
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

            var sum = a + b + c;

            Assert.AreEqual("1+2+3", sum.ToString());

            sum = x + a + y + b;
            Assert.AreEqual("x+1+y+2", sum.ToString());
        }
    }
}