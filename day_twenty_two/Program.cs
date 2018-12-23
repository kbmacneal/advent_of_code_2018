using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_twenty_two {
    class Program {
        public enum type {
            rocky,
            wet,
            narrow,
            target,
            unset,
            origin
        }
        public class coordinate {
            public int x { get; set; }
            public int y { get; set; }

            public int geo_index { get; set; } = 0;
            public int erosion_level { get; set; } = 0;
            public type type { get; set; } = type.unset;

            public void SetGeoIndex (List<coordinate> coords) {

                if (this.x == 0 || this.y == 0) {
                    if (this.x == 0 && this.y == 0) {
                        this.geo_index = 0;
                    }
                    if (this.y == 0) {
                        this.geo_index = this.x * 16807;
                    }
                    if (this.x == 0) {
                        this.geo_index = this.y * 48271;
                    }
                } else if (this.x == max_x && this.y == max_y) {
                    this.geo_index = 0;
                } else {
                    var first = coords.First (e => e.x == this.x - 1 && e.y == this.y);
                    var second = coords.First (e => e.y == this.y - 1 && e.x == this.x);

                    this.geo_index = first.erosion_level * second.erosion_level;
                }

            }

            public void SetErosionLevel () {

                this.erosion_level = (this.geo_index + max_depth) % 20183;

                switch (this.erosion_level % 3) {
                    case 0:
                        this.type = type.rocky;
                        break;
                    case 1:
                        this.type = type.wet;
                        break;
                    case 2:
                        this.type = type.narrow;
                        break;
                    default:
                        break;
                }
            }

        }
        public static int max_depth = 10914;
        public static int max_x = 9;
        public static int max_y = 739;

        public static List<coordinate> coords = new List<coordinate> ();

        static void Main (string[] args) {

            var risk = 0;

            for (int i = 0; i <= max_x; i++) {
                for (int j = 0; j <= max_y; j++) {
                    coords.Add (new coordinate () {
                        x = i,
                            y = j
                    });
                }
            }

            for (int i = 0; i <= max_x; i++) {
                for (int j = 0; j <= max_y; j++) {

                    var coord = coords.First (e => e.x == i && e.y == j);

                    coord.SetGeoIndex (coords);
                    coord.SetErosionLevel ();

                    if (coord.type == type.narrow) risk += 2;
                    if (coord.type == type.wet) risk++;
                }
            }

            Console.WriteLine (risk);

            BfsSolve ();

        }
        private static void BfsSolve () {
            (int x, int y) [] neis = {
                (-1, 0), (0, 1), (1, 0), (0, -1) };

            Queue < (int x, int y, char tool, int switching, int minutes) > queue = new Queue < (int x, int y, char tool, int switching, int minutes) > ();
            HashSet < (int x, int y, char tool) > seen = new HashSet < (int x, int y, char tool) > ();
            queue.Enqueue ((0, 0, 'T', 0, 0));
            seen.Add ((0, 0, 'T'));
            while (queue.Count > 0) {
                (int x, int y, char tool, int switching, int minutes) = queue.Dequeue ();
                if (switching > 0) {
                    if (switching != 1 || seen.Add ((x, y, tool)))
                        queue.Enqueue ((x, y, tool, switching - 1, minutes + 1));
                    continue;
                }

                if ((x, y) == (max_x,max_y) && tool == 'T') {
                    Console.WriteLine (minutes);
                    break;
                }

                foreach ((int xo, int yo) in neis) {
                    (int nx, int ny) = (x + xo, y + yo);
                    if (nx < 0 || ny < 0)
                        continue;

                    if (GetAllowedTools (GetRegionType (nx, ny)).Contains (tool) && seen.Add ((nx, ny, tool)))
                        queue.Enqueue ((nx, ny, tool, 0, minutes + 1));
                }

                foreach (char otherTool in GetAllowedTools (GetRegionType (x, y)))
                    queue.Enqueue ((x, y, otherTool, 6, minutes + 1));
            }
        }

        private static readonly Dictionary < (int x, int y), int > s_erosionLevels = new Dictionary < (int x, int y), int > ();
        private static int ErosionLevel (int x, int y) {
            if (s_erosionLevels.TryGetValue ((x, y), out int val))
                return val;

            if ((x, y) == (0, 0))
                val = 0;
            else if ((x, y) == (max_x,max_y))
                val = 0;
            else if (y == 0)
                val = x * 16807;
            else if (x == 0)
                val = y * 48271;
            else
                val = ErosionLevel (x - 1, y) * ErosionLevel (x, y - 1);

            val += max_depth;
            val %= 20183;
            s_erosionLevels.Add ((x, y), val);
            return val;
        }

        private static char GetRegionType (int x, int y) {
            int erosionLevel = ErosionLevel (x, y);
            return "RWN" [erosionLevel % 3];
        }

        private static string GetAllowedTools (char region) {
            switch (region) {
                case 'R':
                    return "CT";
                case 'W':
                    return "CN";
                case 'N':
                    return "TN";
                default:
                    throw new Exception ("Unreachable");
            }
        }
    }
}