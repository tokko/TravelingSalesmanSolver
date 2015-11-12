using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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

        [Test]
        public void TestGetpath()
        {
            var nodes = new Node[10];
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = new Node(i);
                nodes[i] = node;
                if (i <= 0) continue;
                nodes[i - 1].Next = node;
                node.Previous = nodes[i - 1];
            }

            var res = nodes.GetPath().ToList();
            Assert.That(res[0], Is.EqualTo(0));
            for (var i = 1; i < res.Count; i++)
            {
                Assert.That(res[i], Is.EqualTo(i));
            }
        }
    }
}
