# MathTools.Algebra



Example : 

```c#
var formula = Formula.Parse("x^4*exp(x)");
var value = formula.Eval(new { x = 0.2 });

var derivative = formula.Derive("x").Simplify();
var text = derivative.ToString(); // 4*x^3*Exp(x)+x^4*Exp(x)
var derivedValue = derivative.Eval(new { x = 0.2 });
```



```c#
var x = new Variable("x");
var formula = Formula.Pow(x, 4) * Formula.Exp(x);
var value = formula.Eval(new { x = 0.2 });

var derivative = formula.Derive("x").Simplify();
var text = derivative.ToString(); // 4*x^3*Exp(x)+x^4*Exp(x)
var derivedValue = derivative.Eval(new { x = 0.2 });
```



