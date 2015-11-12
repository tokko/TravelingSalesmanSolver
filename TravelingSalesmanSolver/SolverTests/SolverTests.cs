using System.Collections.Generic;
using System.Linq;
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

        [Test]
        public void TestSkipIndices()
        {
            var list = new List<double> {3, 6, 1, 5, 3, 9, 19}.ToArray();
            var skipList = new List<int> {1, 3};
            
            var res = list.SkipIndices(skipList).ToList();

            Assert.That(res.Count(), Is.EqualTo(list.Count() - skipList.Count));
            Assert.That(res.Contains(list[skipList[0]]), Is.False);
            Assert.That(res.Contains(list[skipList[1]]), Is.False);
        }
    }
}
