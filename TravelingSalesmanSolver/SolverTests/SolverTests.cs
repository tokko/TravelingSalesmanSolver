using System.Collections.Generic;
using NUnit.Framework;
using TravelingSalesmanSolver;

namespace SolverTests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void TestGetPath()
        {

        }

        [Test]
        public void TestGetIndexOfMin()
        {
            var list = new List<double> {3, 6, 1, 5, 3, 9, 19};

            var min = list.IndexOfMin();

            Assert.That(min, Is.EqualTo(2));
        }
    }
}
