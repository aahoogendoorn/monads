using System;
using NUnit.Framework;

namespace Monads.Test
{
    [TestFixture]
    public class TryTest
    {
        private readonly EmployeeRepository repo = new EmployeeRepository();

        [Test]
        public void TestMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .Map(e => e.Name)
                .Map(s => s.EndsWith("s"))
                .Value;

            Assert.IsTrue(result);
        }

        [Test]
        public void TestFlatMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.IsNameKees())
                .Value;

            Assert.IsTrue(result);
        }

        [Test]
        public void TestFailureOnMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .Map(e => e.WillThrowException());

            Assert.IsTrue(result.IsFailure);
        }

        [Test]
        public void TestRecoverOnFlatMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.WillThrowException())
                .Recover(ex => repo.Create("Jaap"));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Name, "Jaap");
        }

        [Test]
        public void TestSpecificRecoverOnFlatMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.WillThrowTryException())
                .Recover<TryException>(e => repo.Create("Jaap"))
                .Recover(ex => repo.Create("Jan"));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Get().Name, "Jaap");
        }

        [Test]
        public void TestRecoverSkippingSpecificRecoverOnFlatMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.WillThrowException())
                .Recover<TryException>(e => repo.Create("Jaap"))
                .Recover(ex => repo.Create("Jan"));

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(result.Value.Name, "Jan");
        }

        [Test]
        public void TestFilter()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .Filter(e => e.IsNameKees().Value);

            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void TestFilterNegative()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Value);

            Assert.IsTrue(result.IsFailure);
        }

        [Test]
        public void TestGetOrElse()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .GetOrElse(repo.Create("Hans"));

            Assert.AreEqual(result.Name, "Kees");
        }

        [Test]
        public void TestGetOrElseFails()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Value)
                .GetOrElse(repo.Create("Hans"));

            Assert.AreEqual(result.Name, "Hans");
        }

        [Test]
        public void TestOrElse()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .OrElse(() => repo.Create("Hans"))
                .Get();

            Assert.AreEqual(result.Name, "Kees");
        }

        [Test]
        public void TestOrElseFails()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Value)
                .OrElse(() => repo.Create("Hans"))
                .Get();

            Assert.AreEqual(result.Name, "Hans");
        }

        [Test]
        public void TestDoSuccess()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Frits"))
                .Set(e => e.Name = "Walter")
                .Set(e => e.Age = new DateTime(1979, 4, 15))
                .Get();

            Assert.AreEqual("Walter", result.Name);
            Assert.AreEqual(new DateTime(1979, 4, 15), result.Age);
        }
    }
}
