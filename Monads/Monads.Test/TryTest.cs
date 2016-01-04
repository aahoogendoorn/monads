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
                .Get();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestFlatMap()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .FlatMap(e => e.IsNameKees())
                .Get();

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
            Assert.AreEqual(result.Get().Name, "Jan");
        }

        [TestMethod]
        public void TestFilter()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Kees"))
                .Filter(e => e.IsNameKees().Get());

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public void TestFilterNegative()
        {
            var result = Try<Employee>.Invoke(() => repo.Create("Frits"))
                .Filter(e => e.IsNameKees().Get());

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
                .Filter(e => e.IsNameKees().Get())
                .GetOrElse(repo.Create("Hans"));

            Assert.AreEqual(result.Name, "Hans");
        }
    }
}
