using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_eighteen
{
    public class point
    {
        public int x { get; set; }
        public int y { get; set; }
        public bool yard { get; set; } = false;
        public bool trees { get; set; } = false;
        public bool open { get; set; } = false;
        public point new_state { get; set; } = null;

        public void zero_out()
        {
            this.yard = false;
            this.trees = false;
            this.open = false;
        }

        public void apply_new_state(point p)
        {
            this.yard = p.yard;
            this.trees = p.trees;
            this.open = p.open;
            this.new_state = null;
        }
    }

    public class grouping
    {
        public int val { get; set; }
        public int first { get; set; }
        public int last { get; set; }
        public int count { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            List<point> points = new List<point>();

            for (int i = 0; i < input.Count(); i++)
            {
                for (int j = 0; j < input[0].Length; j++)
                {
                    var contents = input[j][i];

                    point p = new point()
                    {
                        x = j,
                        y = i,
                        yard = false,
                        trees = false,
                        open = false
                    };

                    if (contents == '#') p.yard = true;
                    if (contents == '.') p.open = true;
                    if (contents == '|') p.trees = true;

                    points.Add(p);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                points = perform_change(points);
            }

            // Console.Clear();
            // print_grid(points);

            var rv = get_rv(points);

            Console.WriteLine("Part 1:");
            Console.WriteLine(rv.ToString());

            Part2(points);
        }



        public static void Part2(List<point> points)
        {
            List<int> rvs = new List<int>();

            var period = 0;

            // for (int i = 0; i < 600; i++)
            // {
            //     points = perform_change(points);

            //     rvs.Add(get_rv(points));

            //     File.AppendAllText("rvs.txt", get_rv(points).ToString() + "\n");
            // }

            period = 28;
            
            //0-index-shifted target - # of elements before stabilization mod the period
            var part2 = ((1000000000-1) - 406) % period;

            //period - the above gets you the index of the number at the billionth position relative to the list of repeating elements
            Console.WriteLine(File.ReadAllLines("rvs.txt")[period - part2]);

        }

        public static List<point> get_adj(List<point> points, point origin)
        {
            int orig_x = origin.x;
            int orig_y = origin.y;

            return points.Where(e => (e.x == orig_x + 1 && e.y == orig_y) || (e.x == orig_x - 1 && e.y == orig_y) || (e.x == orig_x && e.y == orig_y + 1) || (e.x == orig_x && e.y == orig_y - 1) || (e.x == orig_x + 1 && e.y == orig_y + 1) || (e.x == orig_x - 1 && e.y == orig_y - 1) || (e.x == orig_x + 1 && e.y == orig_y - 1) || (e.x == orig_x - 1 && e.y == orig_y + 1)).ToList();

        }

        private static void print_grid(List<point> points)
        {
            for (int i = 0; i < points.Select(e => e.x).Max() + 1; i++)
            {
                for (int j = 0; j < points.Select(e => e.y).Max() + 1; j++)
                {
                    var point = points.First(e => e.x == i && e.y == j);

                    if (point.trees) Console.Write('|');
                    if (point.open) Console.Write('.');
                    if (point.yard) Console.Write('#');
                }
                Console.WriteLine();
            }
        }

        public static int get_rv(List<point> points)
        {
            return points.Where(e => e.trees).Count() * points.Where(e => e.yard).Count();
        }

        public static List<point> perform_change(List<point> points)
        {
            // Console.Clear();
            // print_grid(points);
            // Console.ReadLine();
            foreach (var point in points)
            {
                if (point.open)
                {
                    var adj = get_adj(points, point);

                    if (adj.Count(e => e.trees) >= 3)
                    {
                        point.new_state = new point()
                        {
                            x = point.x,
                            y = point.y,
                            trees = true,
                            open = false,
                            yard = false,
                            new_state = null
                        };
                    }
                    else
                    {
                        point.new_state = null;
                    }
                }

                if (point.trees)
                {
                    var adj = get_adj(points, point);

                    if (adj.Count(e => e.yard) >= 3)
                    {
                        point.new_state = new point()
                        {
                            x = point.x,
                            y = point.y,
                            trees = false,
                            open = false,
                            yard = true,
                            new_state = null
                        };
                    }
                    else
                    {
                        point.new_state = null;
                    }
                }

                if (point.yard)
                {
                    var adj = get_adj(points, point);

                    if (adj.Count(e => e.yard) < 1 || adj.Count(e => e.trees) < 1)
                    {
                        point.new_state = new point()
                        {
                            x = point.x,
                            y = point.y,
                            trees = false,
                            open = true,
                            yard = false,
                            new_state = null
                        };
                    }
                    else
                    {
                        point.new_state = null;
                    }
                }
            }

            points.Where(e => e.new_state != null).ToList().ForEach(e => e.apply_new_state(e.new_state));


            return points;
        }

    }
}
