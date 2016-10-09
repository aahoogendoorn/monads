using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Monads.Test
{
    [TestClass]
    public class TryTest
    {
        private readonly EmployeeRepository repo = new EmployeeRepository();

        [TestMethod]
        public void TestMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .Map(e => e.Name)
                .Map(s => s.EndsWith("s"))
                .Value;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFlatMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.IsNameKees())
                .Value;

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFailureOnMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .Map(e => e.WillThrowException());

            Assert.IsTrue(result.IsFailure);
        }

        [TestMethod]
        public void TestRecoverOnFlatMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.WillThrowException())
                .Recover(ex => repo.Create("Jaap"));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Name, "Jaap");
        }

        [TestMethod]
        public void TestSpecificRecoverOnFlatMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.WillThrowTryException())
                .Recover<TryException>(e => repo.Create("Jaap"))
                .Recover(ex => repo.Create("Jan"));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Get().Name, "Jaap");
        }

        [TestMethod]
        public void TestRecoverSkippingSpecificRecoverOnFlatMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.WillThrowException())
                .Recover<TryException>(e => repo.Create("Jaap"))
                .Recover(ex => repo.Create("Jan"));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Name, "Jan");
        }

        [TestMethod]
        public void TestFilter()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .Filter(e => e.IsNameKees().Value);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void TestFilterNegative()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Value);

            Assert.IsTrue(result.IsFailure);
        }


        [TestMethod]
        public void TestGetOrElse()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .GetOrElse(repo.Create("Hans"));

            Assert.AreEqual(result.Name, "Kees");
        }

        [TestMethod]
        public void TestGetOrElseFails()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Value)
                .GetOrElse(repo.Create("Hans"));

            Assert.AreEqual(result.Name, "Hans");
        }
<<<<<<< HEAD
        
        [TestMethod]
        public void TestOrElse()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .OrElse(() => repo.Create("Hans"))
                .Get();

            Assert.AreEqual(result.Name, "Kees");
        }

        [TestMethod]
        public void TestOrElseFails()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Value)
                .OrElse(() => repo.Create("Hans"))
                .Get();

            Assert.AreEqual(result.Name, "Hans");
=======


        [TestMethod]
        public void TestDoSuccess()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Frits"))
                .Set(e => e.Name = "Walter Franssen is de beste")
                .Set(e => e.Age = new DateTime(1979, 4, 15))
                .Get();

            Assert.AreEqual("Walter Franssen is de beste", result.Name);
            Assert.AreEqual(new DateTime(1979, 4, 15), result.Age);
>>>>>>> f775e7bb0ac0ce37f151db7b5677ee1a958ed04c
        }
    }
}
