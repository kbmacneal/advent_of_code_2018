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
        }
        public class nanobot {
            public point point { get; set; }
            public int radius { get; set; }
            public bool in_range { get; set; } = false;

            public int ManhattanDist (int x, int y, int z) {
                return Math.Abs (this.point.x - x) + Math.Abs (this.point.y - y) + Math.Abs (this.point.z - z);
            }
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
                bot.in_range = bot.ManhattanDist (max_bot.point.x, max_bot.point.y, max_bot.point.z) <= max_rad;
            }

            Console.WriteLine (bots.Where (e => e.in_range).Count ());

            var max_x = bots.Max (e => e.point.x);
            var max_y = bots.Max (e => e.point.y);
            var max_z = bots.Max (e => e.point.z);

            List<(int, int, int)> points = new List<(int,int,int)> ();

            for (int x = 0; x < max_x; x++) {
                for (int y = 0; y < max_y; y++) {
                    for (int z = 0; z < max_z; z++) {
                        points.Add ((x,y,z));
                    }
                }
            }

            var best_point = (0,0,0);

            var count = 0;

            foreach (var point in points) {
                var temp = bots.Count (e => e.ManhattanDist (point.Item1, point.Item2, point.Item3) <= e.radius);

                if (temp == count) {
                    if (dist_from_0 (point.Item1, point.Item2, point.Item3) < dist_from_0 (best_point.Item1, best_point.Item2, best_point.Item3)) {
                        best_point = point;
                        count = temp;
                        continue;
                    }
                }

                if (temp > count) {
                    count = temp;
                    best_point = point;
                }
            }

            Console.WriteLine (dist_from_0 (best_point.Item1, best_point.Item2, best_point.Item3));

        }

        public static int dist_from_0 (int x, int y, int z) {
            return Math.Abs (x) + Math.Abs (y) + Math.Abs (z);

        }

    }
}