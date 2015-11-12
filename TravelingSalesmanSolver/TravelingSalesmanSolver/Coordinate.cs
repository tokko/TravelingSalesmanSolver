
using System;
using System.Collections.Generic;
using System.Linq;

namespace TravelingSalesmanSolver
{
    public class Coordinate
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Coordinate(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class GraphGenerator
    {
        public static List<Coordinate> GetCoordinates()
        {
            var r = new Random(1337);
            return Enumerable.Range(0, 2000).Select(x => new Coordinate(r.NextDouble()*100, r.NextDouble()*100)).ToList();
        } 
    }
}
