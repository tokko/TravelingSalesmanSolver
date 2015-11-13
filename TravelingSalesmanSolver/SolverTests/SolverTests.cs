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
        public void TestGetPath()
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

        [Test]
        public void Test2Opt()
        {
            var nodes = new Node[4];
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = new Node(i);
                nodes[i] = node;
                if (i <= 0) continue;
                nodes[i - 1].Next = node;
                node.Previous = nodes[i - 1];
            }

            var distances = new double[nodes.Length][];
            distances[0] = new[]{0, 1, 0, 0.5};
            distances[1] = new[]{1, 0, 0.5, 1};
            distances[2] = new[]{0, 0.5, 0, 1};
            distances[3] = new[]{0.5, 1, 1, 0};

            Solver.TwoOpt(nodes[0], nodes[3], distances);

            Assert.That(nodes[0].Next.Index, Is.EqualTo(2));   
            Assert.That(nodes[1].Next.Index, Is.EqualTo(3));   
            Assert.That(nodes[2].Next, Is.EqualTo(1));   
            Assert.That(nodes[3].Next.Index, Is.Null);
               
            Assert.That(nodes[0].Previous, Is.Null);   
            Assert.That(nodes[1].Previous.Index, Is.EqualTo(3));   
            Assert.That(nodes[2].Previous.Index, Is.EqualTo(1));   
            Assert.That(nodes[3].Previous.Index, Is.EqualTo(0));   
        }

        [Test]
        [Ignore]
        public void TestGetPathOpted()
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

            var distances = new double[nodes.Length][];
            distances[2] = new double[nodes.Length];
            distances[5] = new double[nodes.Length];
            distances[3] = new double[nodes.Length];
            distances[6] = new double[nodes.Length];
            distances[2][3] = 1;
            distances[3][2] = 1;
            distances[5][6] = 1;
            distances[6][5] = 1;

            Solver.TwoOpt(nodes[2], nodes[5], distances);

            var res = nodes.GetPath().ToList();
            Assert.That(res[0], Is.EqualTo(0));
            for (var i = 1; i < res.Count; i++)
            {
                Assert.That(res[i], Is.EqualTo(i));
            }
        }
    }
}
