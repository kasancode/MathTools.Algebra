using Microsoft.VisualStudio.TestTools.UnitTesting;
using MathTools.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

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
                Console.WriteLine($"{formula}={formula.Eval(vars)}");
                Assert.AreEqual(3.0 - Math.Pow(Math.Sin(2.0 + 3.0), 5.0), formula.Eval(vars));
                Assert.AreEqual(3.0 - Math.Pow(Math.Sin(2.0 + 3.0), 5.0), func(new[] { 3.0, 5.0 }));
            }
        }

        [TestMethod()]
        public void PerformanceTest()
        {
            void doFunc(int loop)
            {
                var random = new Random();
                var formula = Formula.Parse("Log(x1)/Exp(2+x1)^x2").Derive("x1").Simplify();
                var func = formula.ToFunc();
                for (var i = 0; i < loop; i++)
                {
                    var value = func(new[] { random.NextDouble(), random.NextDouble() });
                }
            }

            void doNoname(int loop)
            {
                var random = new Random();
                var formula = Formula.Parse("Log(x1)/Exp(2+x1)^x2").Derive("x1").Simplify();
                for (var i = 0; i < loop; i++)
                {
                    var value = formula.Eval(new { x1 = random.NextDouble(), x2 = random.NextDouble() });
                }
            }

            void doDict(int loop)
            {
                var random = new Random();
                var formula = Formula.Parse("Log(x1)/Exp(2+x1)^x2").Derive("x1").Simplify();
                var dict = new Dictionary<string, double>
                {
                    {"x1", 0.0 },
                    {"x2", 0.0 },
                };

                for (var i = 0; i < loop; i++)
                {
                    dict["x1"] = random.NextDouble();
                    dict["x2"] = random.NextDouble();
                    var value = formula.Eval(dict);
                }
            }

            void doParams(int loop)
            {
                var random = new Random();
                var formula = Formula.Parse("Log(x1)/Exp(2+x1)^x2").Derive("x1").Simplify();

                for (var i = 0; i < loop; i++)
                {
                    var value = formula.Eval(random.NextDouble(), random.NextDouble());
                }
            }

            void doMethod(string method, int loop)
            {
                switch (method)
                {
                    case "func":
                        doFunc(loop);
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
            var loops = new[] { 1, 10, 100, 1000, 10000, 100000, /*1000000*/ };
            var methods = new[] { "noname", "dict", "params", "func" };

            var results = new Dictionary<(string, int), double>();

            foreach (var method in methods)
            {
                // first time is slow
                // dummy
                doMethod(method, 1);
                foreach (var loop in loops)
                {
                    watch.Restart();
                    doMethod(method, loop);
                    watch.Stop();
                    results.Add((method, loop), watch.ElapsedMilliseconds);
                }
            }

            Console.WriteLine($"method,loop,msec");
            foreach (var (key,value) in results)
            {
                Console.WriteLine($"{key.Item1},{key.Item2},{value}");
            }
        }
    }
}