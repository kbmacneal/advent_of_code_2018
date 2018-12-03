using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_three {
    public class rectangle {
        public int id { get; set; }
        public int from_left { get; set; }
        public int from_top { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int area { get; set; }
        public List<point> constituent_points = new List<point> ();
    }

    public class point {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int claimed_by { get; set; } = 0;
        public List<int> claim_ids { get; set; } = new List<int> ();
    }

    class Program {
        static void Main (string[] args) {

            string input = "input.txt";

            int point_id = 0;

            string[] vals = System.IO.File.ReadAllLines (input);

            List<point> grid = new List<point> ();
            List<rectangle> rects = new List<rectangle> ();

            foreach (var item in vals) {
                string[] splits = item.Split (" ");

                rectangle rect = new rectangle () {
                    id = Int32.Parse (splits[0].Replace ("#", "")),
                    from_left = Int32.Parse (splits[2].Replace (":", "").Split (",") [0]),
                    from_top = Int32.Parse (splits[2].Replace (":", "").Split (",") [1]),
                    width = Int32.Parse (splits[3].Split ("x") [0]),
                    height = Int32.Parse (splits[3].Split ("x") [1]),
                    area = Int32.Parse (splits[3].Split ("x") [0]) * Int32.Parse (splits[3].Split ("x") [1])
                };

                rects.Add (rect);

                //generate the pointlist
                int starting_x = rect.from_left;
                int starting_y = rect.from_top;

                //#1 @ 1,3: 4x4
                // #2 @ 3,1: 4x4
                // #3 @ 5,5: 2x2

                for (int i = 0; i < rect.width; i++) {
                    for (int j = 0; j < rect.height; j++) {
                        int temp_x = starting_x + i;
                        int temp_y = starting_y + j;
                        if (grid.Where (e => e.x == temp_x && e.y == temp_y).Count () > 0) {
                            grid.First (e => {
                                bool v = e.x == temp_x && e.y == temp_y;
                                return v;
                            }).claimed_by++;

                            grid.First (e => {
                                bool v = e.x == temp_x && e.y == temp_y;
                                return v;
                            }).claim_ids.Add (rect.id);

                        } else {
                            point point = new point () {
                                x = temp_x,
                                y = temp_y,
                                claimed_by = 1,
                                id = point_id++
                            };

                            grid.Add (point);

                            rect.constituent_points.Add (point);
                        }
                    }
                }
            }

            Console.WriteLine ("Solution to Part 1");
            Console.WriteLine (grid.Where (e => e.claimed_by > 1).Count ().ToString ());

            Console.WriteLine ("Solution to Part 2");
            Console.WriteLine (Part2());

        }

        private static int Part2 () {
            string input = "input.txt";

            int point_id = 0;

            string[] vals = System.IO.File.ReadAllLines (input);

            var grid = new Dictionary<int, Dictionary<int, int>> ();

            int overlaps = 0;

            foreach (var line in vals) {
                var parts = line.Split (' ');

                var coords = parts[2].Remove (parts[2].Length - 1, 1).Split (',');
                var xCoord = int.Parse (coords[0]);
                var yCoord = int.Parse (coords[1]);

                var size = parts[3].Split ('x');
                var xSize = int.Parse (size[0]);
                var ySize = int.Parse (size[1]);

                for (int x = xCoord; x < xCoord + xSize; ++x) {
                    for (int y = yCoord; y < yCoord + ySize; ++y) {
                        if (!grid.TryGetValue (x, out var gridDictY)) {
                            gridDictY = new Dictionary<int, int> ();
                            grid[x] = gridDictY;
                        }

                        if (!gridDictY.TryGetValue (y, out var gridAtLocation)) {
                            gridAtLocation = 0;
                        }

                        ++gridAtLocation;
                        gridDictY[y] = gridAtLocation;
                    }
                }
            }

            // Pass over each claim again, and check if it was overlapped by any other claim
            foreach (var line in vals) {
                var parts = line.Split (' ');

                var claimID = int.Parse (parts[0].Remove (0, 1)); // Remove #

                var coords = parts[2].Remove (parts[2].Length - 1, 1).Split (',');
                var xCoord = int.Parse (coords[0]);
                var yCoord = int.Parse (coords[1]);

                var size = parts[3].Split ('x');
                var xSize = int.Parse (size[0]);
                var ySize = int.Parse (size[1]);

                bool isCandidate = true;

                for (int x = xCoord; x < xCoord + xSize; ++x) {
                    for (int y = yCoord; y < yCoord + ySize; ++y) {
                        if (grid.TryGetValue (x, out var gridDictY)) {
                            if (gridDictY.TryGetValue (y, out var gridAtLocation)) {
                                if (gridAtLocation > 1) {
                                    isCandidate = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (isCandidate) {
                    return claimID;
                }
            }

            return -1;
        }

    }

}