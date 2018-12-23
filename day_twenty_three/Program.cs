using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_twenty_three {
    class Program {

        public class point {
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }

            public int ManhattanDist (int x, int y, int z) {
                return Math.Abs (this.x - x) + Math.Abs (this.y - y) + Math.Abs (this.z - z);
            }
        }
        public class nanobot {
            public point point { get; set; }
            public int radius { get; set; }
            public bool in_range { get; set; } = false;
        }
        static void Main (string[] args) {
            var inputs = File.ReadAllLines ("input.txt");

            List<nanobot> bots = new List<nanobot> ();

            foreach (var line in inputs) {
                var splits = line.Split (", ");

                var coords = splits[0].Replace ("pos=<", "").Replace (">", "");

                var x = coords.Split (",") [0];
                var y = coords.Split (",") [1];
                var z = coords.Split (",") [2];

                var radius = splits[1].Replace ("r=", "");

                nanobot bot = new nanobot () {
                    point = new point () {
                    x = Int32.Parse (x),
                    y = Int32.Parse (y),
                    z = Int32.Parse (z)
                    },
                    radius = Int32.Parse (radius)
                };

                bots.Add (bot);
            }

            var max_rad = bots.Max (e => e.radius);

            var max_bot = bots.First (e => e.radius == max_rad);

            foreach (var bot in bots) {
                bot.in_range = bot.point.ManhattanDist (max_bot.point.x, max_bot.point.y, max_bot.point.z) <= max_rad;
            }

            Console.WriteLine (bots.Where (e => e.in_range).Count ());

            var max_x = bots.Max(e=>e.point.x);
            var max_y = bots.Max(e=>e.point.y);
            var max_z = bots.Max(e=>e.point.z);

            List<point> points = new List<point>();

            for (int x = 0; x < max_x; x++)
            {
                for (int y = 0; y < max_y; y++)
                {
                    for (int z = 0; z < max_z; z++)
                    {
                        points.Add(new point(){
                            x = x,
                            y=y,
                            z=z
                        });
                    }
                }
            }

            // foreach (var point in points)
            // {
            //     foreach (var bot in bots)
            //     {
            //         if(bot.ManhattanDist(point.x,point.y,point.z)<= bot.radius)
            //         {
            //             point.bots_in_range.Add(bot);
            //         }
            //     }
            // }

            var best_point = new point();

            var count = 0;


            foreach (var point in points)
            {
                var temp = bots.Where(e=>e.point.ManhattanDist(point.x,point.y,point.z)<=e.radius).Count();

                if(temp == count)
                {
                    if(point.ManhattanDist(0,0,0) < best_point.ManhattanDist(0,0,0))
                    {
                        best_point = point;
                        count = temp;
                        continue;
                    }
                }

                if(temp > count)
                {
                    count = temp;
                    best_point= point;
                }
            }

            Console.WriteLine(best_point.ManhattanDist(0,0,0));

        }

    }
}