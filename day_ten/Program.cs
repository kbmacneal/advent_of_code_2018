using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace day_ten
{
    class Program
    {
        static void Main(string[] args)
        {
            Parts();
        }

        public static void Parts()
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
                //if there the x dimension or y dimension has increased, that means that we have past the point of convergence (which you could think of as the global minimum distance between all points) so we must have our answer?
                if ((newMaxX - newMinX) > (maxX - minX) || (newMaxY - newMinY) > (maxY - minY))
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
