# Eval() performance



```c#
var formula = Formula.Parse("Log(x1)/Exp(2+x1)^x2");

// Dictionary : Use Dictionary<string, double> for argumetns.
var dictValue = formula.Eval(new Dictionary<string, double>
{
    {"x1", 0.1 },
    {"x2", 0.2 },
});

// Anonymous class : Use anonymous class for arguments.
var anonyValue = formula.Eval(new { x1 = 0.1, x2 = 0.2 })

// params: Arguments order is depend on Formula.GetVariables();
var pramsValue = formula.Eval(0.1, 0.2);

// ToFunc() : Emit IL code for calculation.
var func = formula.ToFunc();
var toFuncValue = func(0.1, 0.2);
```



![performance](..\images\performance.png)
