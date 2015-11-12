using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelingSalesmanSolver
{
    public class Node
    {
        public int Index { get; set; }
        public Node Next { get; set; }
        public Node Previous { get; set; }
        public bool Backwards { get; set; }

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
            Backwards = !Backwards;
        }
    }

    public class Solver
    {

        public void Solve()
        {
            var coordinates = GraphGenerator.GetCoordinates();
            var distances = CalculateDistances(coordinates);
            var path = Greedy(coordinates, distances);
            var r = new Random(4711);
        }

        private static void TwoOpt(Node[] path, double[][] distances, Random r)
        {
            var a = path[r.Next(path.Length)];
            var c = path[r.Next(path.Length)];
            var b = a.Next;
            var d = c.Next;

            var ab = distances[a.Index][b.Index];
            var cd = distances[c.Index][d.Index];
            var ad = distances[a.Index][d.Index];
            var bc = distances[b.Index][c.Index];

            var abcd = ab + cd;
            var adbc = ad + bc;

            if (adbc > abcd) return;

            a.Next = d;
            d.Previous = a;

            c.Next = b;
            b.Previous = c;

            a.ToggleDirection();
            b.ToggleDirection();
            c.ToggleDirection();
            d.ToggleDirection();
        }

        private static Node[] Greedy(IReadOnlyCollection<Coordinate> coordinates, double[][] distances)
        {
            var r = new Random(5);
            var current = r.Next(coordinates.Count);
            var visited = new List<int>();
            var path = new Node[coordinates.Count];
            path[current] = new Node(current);
            for (var i = 1; i < coordinates.Count; i++)
            {
                var closest = distances[i].IndexOfMin();
                path[closest] = new Node(i, path[current]);
                path[current].Next = path[closest];
                current = closest;
                visited.Add(i);
            }
            return path;
        }

        private static double[][] CalculateDistances(IReadOnlyList<Coordinate> coordinates)
        {
            var distances = new double[coordinates.Count][];
            for (var i = 0; i < coordinates.Count; i++)
            {
                distances[i] = new double[coordinates.Count];
                for (var j = i; j < coordinates.Count; j++)
                {
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
        public static int IndexOfMin(this double[] src)
        {
            var min = double.MaxValue;
            var minIndex = -1;
            for(var i = 0; i < src.Length; i++)
            {
                if (!(src[i] < min)) continue;
                min = src[i];
                minIndex = i;
            }
            return minIndex;
        }

        public static IEnumerable<double> SkipIndices(this double[] src, IReadOnlyCollection<int> indices)
        {
            for (var i = 0; i < src.Length; i++)
            {
                if(indices.Contains(i)) continue;
                yield return i;
            }
        } 
    }
}
