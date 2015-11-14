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
        public void Test2Opt()
        {
            const int a = 0, b = 1, d = 3, c = 2;
            double[][] distances;
            var nodes = CreteNodesAndDistancesForSIngle2Opt(out distances);

            Solver.TwoOpt(nodes[0], nodes[2], distances);

            Assert.That(nodes[a].Next.Index, Is.EqualTo(c));   
            Assert.That(nodes[b].Next.Index, Is.EqualTo(d));   
            Assert.That(nodes[c].Next.Index, Is.EqualTo(b));   
            Assert.That(nodes[d].Next, Is.Null);
               
            Assert.That(nodes[a].Previous, Is.Null);   
            Assert.That(nodes[b].Previous.Index, Is.EqualTo(c));   
            Assert.That(nodes[c].Previous.Index, Is.EqualTo(a));   
            Assert.That(nodes[d].Previous.Index, Is.EqualTo(b));   
        }

        private static Node[] CreteNodesAndDistancesForSIngle2Opt(out double[][] distances)
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

            const int a = 0, b = 1, d = 3, c = 2;
            distances = new double[nodes.Length][];
            distances[a] = new double[nodes.Length];
            distances[b] = new double[nodes.Length];
            distances[c] = new double[nodes.Length];
            distances[d] = new double[nodes.Length];

            distances[a][b] = 1;
            distances[b][d] = 1;
            distances[d][c] = 1;

            distances[b][a] = 1;
            distances[d][b] = 1;
            distances[c][d] = 1;

            distances[a][d] = 0.5;
            distances[d][a] = 0.5;
            distances[b][c] = 0.5;
            distances[c][b] = 0.5;
            return nodes;
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

            var path = nodes.GetPath();
            var res = path.ToList();
            Assert.That(res[0], Is.EqualTo(0));
            for (var i = 1; i < res.Count; i++)
            {
                Assert.That(res[i], Is.EqualTo(i));
            }
        }

        [Test]
        public void TestGetPathOpted()
        {
            double[][] distances;
            var nodes = CreteNodesAndDistancesForSIngle2Opt(out distances);

            Solver.TwoOpt(nodes[0], nodes[2], distances);

            var path = nodes.GetPath();
            var res = path.ToList();
            var expectedPath = new List<int> {0, 2, 1, 3};
            Assert.That(res.Zip(expectedPath, (a, b) => a == b).All(x => x), Is.True);
        }
    }
}
