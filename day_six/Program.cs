using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


namespace day_six
{
    public class coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
        public int id { get; set; }
    }

    public class point
    {
        public int x { get; set; }
        public int y { get; set; }
        public coordinate closest_to { get; set; }
        public int total_distance_to_all_points {get;set;} = 0;
        public int ManhattanDist(int x, int y)
        {
            return Math.Abs(this.x - x) + Math.Abs(this.y - y);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt");
            int i = 1;

            List<coordinate> coords = input.Select(e => e.Split(", ", StringSplitOptions.None)).Select(e => new coordinate { x = Int32.Parse(e[0]), y = Int32.Parse(e[1]) }).ToList();

            foreach (var item in coords)
            {
                item.id = i;
                i++;
            }

            Part1(coords);
        }



        public static void Part1(List<coordinate> coords)
        {
            var maxX = coords.Max(c => c.x);
            var maxY = coords.Max(c => c.y);

            var grid = new int[maxX + 2, maxY + 2];
            var safeCount = 0;

            List<point> points = new List<point>();

            for (int x = 0; x <= maxX + 1; x++)
                for (int y = 0; y <= maxY + 1; y++)
                {
                    point p = new point();
                    p.x = x;
                    p.y = y;

                    List<Tuple<int, int>> distances = new List<Tuple<int, int>>();

                    foreach (var coord in coords)
                    {
                        distances.Add(new Tuple<int, int>(coord.id, p.ManhattanDist(coord.x, coord.y)) { });

                        p.total_distance_to_all_points += p.ManhattanDist(coord.x,coord.y);                        
                    }

                    if(p.total_distance_to_all_points < 10000) safeCount++;

                    var min_dist = distances.Select(e => e.Item2).Min();
                    List<Tuple<int, int>> min_points = distances.Where(e => e.Item2 == min_dist).ToList();

                    if (min_points.Count() == 1)
                    {
                        p.closest_to = coords.First(e => e.id == min_points.Select(f => f.Item1).First());
                    }
                    else
                    {
                        p.closest_to = null;
                    }

                    points.Add(p);
                }

            points.RemoveAll(g => g.closest_to == null);

            List<int> infinite_ids = points.Where(e => e.x == 0 || e.y == 0 || e.x == maxX + 1 || e.y == maxY + 1).Select(f => f.closest_to.id).Distinct().ToList();

            points.RemoveAll(e => infinite_ids.Contains(e.closest_to.id));
            
            Console.WriteLine("Part 1:");
            Console.WriteLine(points.GroupBy(e => e.closest_to.id).Max(f => f.Count()));
            
            Console.WriteLine("Part 2:");
            Console.WriteLine(safeCount);
        }
    }
}
