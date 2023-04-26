using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MathTools.Algebra.Tests
{
    [TestClass()]
    public class FormulaCompilerTests
    {
        [TestMethod()]
        public void ToFuncTest()
        {
            {
                var formula = Formula.Parse("x1-Sin(2+x1)^x2");
                var func = formula.ToFunc();
                var vars = new Dictionary<string, double> {
                    { "x1", 3.0 },
                    { "x2", 5.0 }
                };
                Assert.AreEqual(formula.Eval(vars), func(3.0, 5.0));
            }

            {
                var formula = Formula.Parse("if(x1, -Sin(2+x1)^x2, x2^Sin(x1))*x2");
                var func = formula.ToFunc();

                var check = (double x1, double x2) =>
                {
                    var vars = new Dictionary<string, double> { { "x1", x1 }, { "x2", x2 } };
                    Assert.AreEqual(formula.Eval(vars), func(x1, x2));
                };

                check(0.3, 5.0);
                check(-0.3, 5.0);
                check(0.0, -5.0);
                check(-0.5, 3.0);
            }
        }

        [TestMethod()]
        public void PerformanceTest()
        {
            var text = "Log(x1)/Exp(2+x1)^x2";

            void doNomal(int loop)
            {
                var random = new Random();
                var func = (double x1, double x2) => Math.Log(x1) / Math.Pow(Math.Exp(2 + x1), x2);
                for (var i = 0; i < loop; i++)
                {
                    var value = func(random.NextDouble(), random.NextDouble());
                }
            }

            void doIl(int loop)
            {
                var random = new Random();
                var formula = Formula.Parse(text).Derive("x1").Simplify();
                var func = formula.ToFunc();
                for (var i = 0; i < loop; i++)
                {
                    var value = func(random.NextDouble(), random.NextDouble());
                }
            }

            void doNoname(int loop)
            {
                var random = new Random();
                var formula = Formula.Parse(text).Derive("x1").Simplify();

                for (var i = 0; i < loop; i++)
                {
                    var value = formula.Eval(new { x1 = random.NextDouble(), x2 = random.NextDouble() });
                }
            }

            void doDict(int loop)
            {
                var random = new Random();
                var formula = Formula.Parse(text).Derive("x1").Simplify();

                for (var i = 0; i < loop; i++)
                {
                    var dict = new Dictionary<string, double>
                    {
                        {"x1", random.NextDouble() },
                        {"x2", random.NextDouble() },
                    };
                    var value = formula.Eval(dict);
                }
            }

            void doParams(int loop)
            {
                var random = new Random();
                var formula = Formula.Parse(text).Derive("x1").Simplify();

                for (var i = 0; i < loop; i++)
                {
                    var value = formula.Eval(random.NextDouble(), random.NextDouble());
                }
            }

            void doMethod(string method, int loop)
            {
                switch (method)
                {
                    case "normal":
                        doNomal(loop);
                        break;
                    case "il":
                        doIl(loop);
                        break;
                    case "params":
                        doParams(loop);
                        break;
                    case "noname":
                        doNoname(loop);
                        break;
                    case "dict":
                        doDict(loop);
                        break;
                }
            }

            var watch = new Stopwatch();
#if DEBUG
            var loops = new[] { 1, 10, 100, 1000, 10000, 100000 };
#else
            var loops = new[] { 1, 10, 100, 1000, 10000, 100000, 1000000 };
#endif
            var methods = new[] {"normal", "noname", "dict", "params", "il" };

            var results = new Dictionary<(string, int), double>();

            foreach (var method in methods)
            {
                // first time is slow
                watch.Restart();
                doMethod(method, 1);
                watch.Stop();
                results.Add((method, 0), watch.ElapsedMilliseconds);
            }

            foreach (var method in methods)
            {
                foreach (var loop in loops)
                {
                    watch.Restart();
                    doMethod(method, loop);
                    watch.Stop();
                    results.Add((method, loop), watch.ElapsedMilliseconds);
                }
            }

            Console.WriteLine($"method,loop,msec");
            foreach (var (key, value) in results)
            {
                Console.WriteLine($"{key.Item1},{key.Item2},{value}");
            }
        }
    }
}