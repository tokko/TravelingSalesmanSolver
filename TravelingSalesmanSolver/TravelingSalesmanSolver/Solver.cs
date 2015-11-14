using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TravelingSalesmanSolver
{
    [DebuggerDisplay("{Index}")]
    public class Node
    {
        public int Index { get; set; }
        public Node Next { get; set; }
        public Node Previous { get; set; }
        public bool Toggle { get; set; }

        public Node(int index)
        {
            Index = index;
        }

        public Node(int index, Node previous) : this(index)
        {
            Previous = previous;
        }

        public void ToggleDirection()
        {
            Toggle = !Toggle;
        }
    }

    public class Solver
    {

        public static List<int> Solve()
        {
            var coordinates = GraphGenerator.GetCoordinates();
            var distances = CalculateDistances(coordinates);
            var path = RandomApprox(coordinates.Count);
            RandomTwoOpt(path, distances);
            return path.GetPath().ToList();
        }

        public static void RandomTwoOpt(Node[] path, double[][] distances)
        {
            var r = new Random(4711);
            for (var i = 0; i < 20*Math.Pow(10, 6); i++)
            {
                var a = path[r.Next(path.Length)];
                var c = path[r.Next(path.Length)];
                TwoOpt(a, c, distances);
            }

        }

        public static void TwoOpt(Node a, Node c, double[][] distances)
        {
            //if either of seeds is end of path
            if (a == null || c == null) return;
            if (a.Next == null || c.Next == null) return;

            var b = a.Next;
            var d = c.Next;

            var ab = distances[a.Index][b.Index];
            var cd = distances[c.Index][d.Index];
            var ad = distances[a.Index][d.Index];
            var bc = distances[b.Index][c.Index];

            var abcd = ab + cd;
            var adbc = ad + bc;

            if (adbc > abcd) return;

            c.ToggleDirection();
            b.Next?.ToggleDirection();

            a.Next = c;
            c.Next = c.Previous;
            c.Previous = a;
            
            b.Previous = b.Next;
            b.Next = d;
            d.Previous = b;

           
        }

        public static Node[] RandomApprox(int n)
        {
            var nodes = new Node[n];
            var visited = new HashSet<int>();
            var r = new Random(4711);
            for (var i = 0; i < n; i++)
            {
                int index;
                while (!visited.Add(index = r.Next(n)));
                nodes[i] = new Node(index);
                if (i <= 0) continue;
                nodes[i].Previous = nodes[i - 1];
                nodes[i - 1].Next = nodes[i];
            }
            nodes[0].Previous = nodes.Last();
            nodes.Last().Next = nodes[0];
            return nodes;
        }

        public static Node[] Greedy(IReadOnlyCollection<Coordinate> coordinates, double[][] distances)
        {
            var r = new Random(5);
            var current = r.Next(coordinates.Count);
            var visited = new HashSet<int>();
            var path = new List<Node> {new Node(current)};
            while(path.Count < coordinates.Count)
            {
                var i = current;
                var closest = distances[i].SkipIndices(visited).ToList().IndexOfMin();
                var closestNode = new Node(i, path.Last());
                path.Last().Next = closestNode;
                path.Add(closestNode);
                current = closest;
                visited.Add(i);
            }
            return path.ToArray();
        }

        public static double[][] CalculateDistances(IReadOnlyList<Coordinate> coordinates)
        {
            var distances = new double[coordinates.Count][];
            for (var i = 0; i < coordinates.Count; i++)
            {
                distances[i] = new double[coordinates.Count];
                for (var j = i; j < coordinates.Count; j++)
                {
                    distances[j] = new double[coordinates.Count];
                    var d = FastAbs(coordinates[i].X - coordinates[j].X) + FastAbs(coordinates[i].Y - coordinates[j].Y);
                    distances[i][j] = d;
                    distances[j][i] = d;
                }
            }
            return distances;
        }

        private static double FastAbs(double d)
        {
            return d < 0 ? -d : d;
        }

    }

    public static class Extensions
    {

        public static IEnumerable<int> GetPath(this Node[] src)
        {
            var current = src.First();
            var backwards = current.Toggle;
            src.Last().Next = null;
            src.First().Previous = null;
            while (current != null)
            {
                yield return current.Index;
                current = backwards ? current.Previous : current.Next;
                if (current != null && current.Toggle)
                    backwards = !backwards;
            }
        } 

        public static int IndexOfMin(this List<double> src)
        {
            var min = double.MaxValue;
            var minIndex = -1;
            for(var i = 0; i < src.Count; i++)
            {
                if (src[i] >= min) continue;
                min = src[i];
                minIndex = i;
            }
            return minIndex;
        }

        public static IEnumerable<double> SkipIndices(this double[] src, HashSet<int> indices)
        {
            return src.Where((t, i) => !indices.Contains(i));
        }
    }
}
