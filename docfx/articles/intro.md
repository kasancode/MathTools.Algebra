# Introduction

## Create `Formula` instance

There are some way to create `Fromula` instance.

1.  Parse from a string.

   ```c#
   var formula = Formula.Parse("x^4*exp(x)");
   ```

2. Using static methods of `Formula` .

   ```c#
   var formula = Formula.Pow("x", 4) * Formula.Exp("x");
   ```

3. Using `Variable` or `Constant` instances.

   ```c#
   var x = new Variable("x");
   var formula = Formula.Pow(x, 4) * Formula.Exp(x);
   ```

   

## Evaluate `Formula` instance

 `Formula.Eval()` methods return evaluated values.

If `Formula` instance has no variable, `Eval()` method can be used.

```c#
var formula = Formula.Parse("2^4*exp(3)");
var value = formula.Eval(); // OK

formula = Formula.Parse("x^4*exp(x)");
value = formula.Eval(); // Error
```

If `Formula` instance have some variables, `Eval()` method requires arguments. 

- `Eval(Dictionary<string, double>)`
- `Eval(object)`
- `Eval(params double[])`



### Dictionary type arguments

The example of `Eval(Dictionary<string, double>)` is :

 ```c#
 var formula = Formula.Parse("Log(x1)/Exp(2+x1)^x2");
 
 var value = formula.Eval(new Dictionary<string, double>
 {
     {"x1", 0.1 },
     {"x2", 0.2 },
 });
 ```

Dictionary type arguments can contain specific strings for name of variables.

 ```c#
var formula = Formula.Parse("Log([1])/Exp(2+[1])^[2]");

var value = formula.Eval(new Dictionary<string, double>
{
    {"[1]", 0.1 },
    {"[2]", 0.2 },
});
 ```



### Object type arguments

The example of `Eval(object)` is :

```c#
var formula = Formula.Parse("Log(x1)/Exp(2+x1)^x2");
var value = formula.Eval(new { x1 = 0.1, x2 = 0.2 });
var value2 = formula.Eval(new { x1 = 1, x2 = 2 }); // Error. propery type must be `double`
```

This method searches for properties of the argument object with the `double` type. Other number types such as `int`, `float`, etc. will be ignored.



### Params type arguments

The example of `Eval(parmas double[])` is :

```c#
var formula = Formula.Parse("Log(x1)/Exp(2+x1)^x2");
var value = formula.Eval(0.1, 0.2);
```

The arguments order is depend on `Formula.GetVariables()`



### `ToFunc()` extension method

The `ToFunc()` extension method creates an `EvalFunc` delegate. It is constructed using emitted IL codes. The arguments order is depend on `Formula.GetVariables()`

See [performance](./performance.md).

```c#
var formula = Formula.Parse("Log(x1)/Exp(2+x1)^x2");
var func = formula.ToFunc();
var value = func(0.1, 0.2);
```



## Simplify

The `Formula` instance can be simplified.

```c#
var formula = Formula.Parse("Log(x)*Log(x)^2/Log(x)^5");
var text = formula.Simplify().ToString(); // 1/Log(x)^2
```



## Derive

The `Formula` instance can be derived.

```c#
var formula = Formula.Parse("x^4*exp(x)");
var derivative = formula.Derive("x").Simplify();
var text = derivative.ToString(); // 4*x^3*Exp(x)+x^4*Exp(x)
var derivedValue = derivative.Eval(new { x = 0.2 });
```



