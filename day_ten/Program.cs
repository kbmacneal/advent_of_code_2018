using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace day_ten
{
    public class point
    {
        public int x { get; set; }
        public int y { get; set; }
        public velocity v { get; set; }

        public void Step()
        {
            x += this.v.d_x;
            y += this.v.d_y;
        }
    }
    public class velocity
    {
        public int d_x { get; set; }
        public int d_y { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var contents = System.IO.File.ReadAllLines("input.txt");

            List<point> points = new List<point>();

            foreach (var line in contents)
            {
                string[] splits = line.Split(">", StringSplitOptions.RemoveEmptyEntries);

                splits[0] = splits[0].Replace("position=<", "");

                splits[1] = splits[1].Replace("velocity=<", "");

                point p = new point()
                {
                    x = Int32.Parse(splits[0].Split(",")[0].Trim()),
                    y = Int32.Parse(splits[0].Split(",")[1].Trim()),
                    v = new velocity()
                    {
                        d_x = Int32.Parse(splits[1].Split(",")[0].Trim()),
                        d_y = Int32.Parse(splits[1].Split(",")[1].Trim())
                    }
                };

                points.Add(p);
            }


            Part1();

        }

        public static void Part1()
        {
            var regex = new Regex(@"position=<\s?(-?\d+), \s?(-?\d+)> velocity=<\s?(-?\d+), \s?(-?\d+)>");
            var points = System.IO.File.ReadAllLines("input.txt")
                .Select(x =>
                {
                    var match = regex.Match(x);
                    return (posx: int.Parse(match.Groups[1].Value), posy: int.Parse(match.Groups[2].Value),
                            velx: int.Parse(match.Groups[3].Value), vely: int.Parse(match.Groups[4].Value));
                })
                .ToList();

            var minX = points.Min(x => x.posx);
            var minY = points.Min(x => x.posy);
            var maxX = points.Max(x => x.posx);
            var maxY = points.Max(x => x.posy);
            var seconds = 0;
            while (true)
            {
                var temp = points.Select(x => x).ToList();
                for (int i = 0; i < points.Count; i++)
                {
                    var p = points[i];
                    points[i] = (p.posx + p.velx, p.posy + p.vely, p.velx, p.vely);
                }

                var newMinX = points.Min(x => x.posx);
                var newMinY = points.Min(x => x.posy);
                var newMaxX = points.Max(x => x.posx);
                var newMaxY = points.Max(x => x.posy);
                if ((newMaxX - newMinX) > (maxX - minX) ||
                    (newMaxY - newMinY) > (maxY - minY))
                {
                    Console.WriteLine(seconds);
                    for (var i = minY; i <= maxY; i++)
                    {
                        for (var j = minX; j <= maxX; j++)
                        {
                            Console.Write(temp.Any(x => x.posy == i && x.posx == j) ? '#' : '.');
                        }

                        Console.WriteLine();
                    }

                    Console.ReadLine();
                }

                minX = newMinX;
                minY = newMinY;
                maxX = newMaxX;
                maxY = newMaxY;
                seconds++;
            }
        }

    }
}
