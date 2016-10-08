# Monads
During a recent Java project, we created a small library that executes the behaviour of the Scala `Try` classes (including `Success` and `Failure`). I really started to appreciate the style of programming, where we concatenate series of `Map()` and `FlatMap()` method, using the power of the `Try` monad, and thus avoiding abundant try-catch blocks and null checks.
Hence, I decided to port this library to C#. 

I've only added my first commit to this repository, but will add more updates as the library slowly grows. The unit tests I've added demonstrate the use of the Try classes and methods fairly well already.

## Try and succeced
When programming using the `Try` class, you can actually avoid most try-catch blocks in code, because the monad `Try` will wrap possible exceptions, and just return whether the actions and functions you've called fail or succeed.

In the (simple) code example below an instance of the class `Employee` is created, the name of the employee is fetched in the `Map()` statement where `e` is really the instance of `Employee` that was created above, and we check whether it ends with the character s. This will return a `Try<bool>`, and we use the property `Value` to get the actual value from the `Try`. 

```c#
    var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
        .Map(e => e.Name)
        .Map(s => s.EndsWith("s"))
        .Value;

    Assert.IsTrue(result);
```
Getting this value will only return a valid value IF all of the statements above have executed succesfully - that is, if we have an instance of the `Success` class in the end. If not, we have an instance of `Failure` on our hands and getting the value will throw an exception.

## Try and fail
In the next example, some more of the power of `Try` is exposed. Here one of the statements fails (deliberately). 

```c#
    var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
        .Map(e => e.WillThrowException());

    Assert.IsTrue(result.IsFailure);
```
Now, the first `Invoke()` call return an employee, but the second statement `Map()` throw an exception. But instead of crashing your programming, the result will be an instance of the `Failure` class, holding the exception that was thrown. If one of the statements in your code returns an instance of `Failure` all following statements are being ignored.

Although this example seems trivial, once you get used to programming with `Try` you will soon realize, it is actually quite powerful in building more robust code, that also becomes much easier to test too.

## Recovering from failure
The next thing you might want to do is to recover from failure and continue. To this aim there's the `Recover()` methods.

```c#
    var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
        .FlatMap(e => e.WillThrowException())
        .Recover(ex => repo.Create("Jaap"));

    Assert.IsTrue(result.IsSuccess);
    Assert.AreEqual(result.Value.Name, "Jaap");
```

In this code example the second statement throws and will return an instance of `Failure`. What the `Recover()` method in the next statement will do is help you get back on track. It will create a new employee (with the name Jaap) and allow you to continue. Quite often, recover statements appear at the end of a block of statements, for instance to recover from REST calls that do not return anything useful.
