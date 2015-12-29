# Monads
During a recent Java project, we created a small library that executes the behaviour of the Scala `Try` classes (including `Success` and `Failure`). I really started to appreciate the style of programming, where we concatenate series of `Map()` and `FlatMap()` method, using the power of the `Try` monad, and thus avoiding abundant try-catch blocks and null checks.
Hence, I decided to port this library to C#. 

I've only added my first commit to this repository, but will add more updates as the library slowly grows. The unit tests I've added demonstrate the use of the Try classes and methods fairly well already.

