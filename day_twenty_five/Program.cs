using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace day_twenty_five {
    class Program {
        public class point {
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }
            public int t { get; set; }
            public List<point> constellation { get; set; } = new List<point> ();
        }
        static void Main (string[] args) {
            var input = System.IO.File.ReadAllText ("input.txt");

           var points = input.Split(new string[] { "\n", "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
            var constellations = 0;
            var spaceTimePoints = new List<point>();
            for(var i =0; i < points.Count(); i +=4)
            {
                spaceTimePoints.Add(new point
                {
                    x = points[i],
                    y = points[i + 1],
                    z = points[i + 2],
                    t = points[i + 3]
                });
            }
            var allTried = new List<int>();
            while (true)
            {
                var tried = new List<int>();
                var pointsToCheck = new Queue<int>();
                for (var i = 0; i < spaceTimePoints.Count(); i++)
                {
                    if (!allTried.Contains(i))
                    {
                        pointsToCheck.Enqueue(i);
                        break;
                    }
                }
                while (pointsToCheck.Count > 0)
                {
                    var current = pointsToCheck.Dequeue();
                    if(allTried.Contains(current))
                    {
                        continue;
                    }
                    tried.Add(current);
                    allTried.Add(current);
                    for (var i = 0; i < spaceTimePoints.Count(); i++)
                    {
                        if (!tried.Contains(i) && !allTried.Contains(i))
                        {
                            if ((ManhattanDist(spaceTimePoints[i], spaceTimePoints[current])) <= 3)
                            {
                                pointsToCheck.Enqueue(i);
                            }
                        }
                    }
                }
                if(tried.Count() == 0)
                {
                    break;
                }
                constellations++;
            }
            // return constellations;
        

            Console.WriteLine ("Part 1:");
            Console.WriteLine (constellations);
        }

        public static int ManhattanDist (point point1, point point2) {
            int rtn = 0;

            rtn = Math.Abs (point1.x - point2.x) + Math.Abs (point1.y - point2.y) + Math.Abs (point1.z - point2.z) + Math.Abs (point1.t - point2.t);

            return rtn;
        }
    }
}