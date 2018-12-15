using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_fifteen {
    class Program {

        public class coordinate {
            public int x { get; set; }
            public int y { get; set; }

            public coordinate (int x, int y) {
                this.x = x;
                this.y = y;
            }
        }
        public class unit {
            public int atk_power { get; set; } = 3;
            public int hp { get; set; } = 200;
            public coordinate location { get; set; }

            public class elf : unit { }
            public class goblin : unit { }
        }

        public class node {
            public coordinate location;
            public bool wall { get; set; }
            public object occupant { get; set; }
        }

        static void Main (string[] args) {

            string[] input = System.IO.File.ReadAllLines ("input.txt");

            List<node> nodes = new List<node> ();
            List<unit.elf> elves = new List<unit.elf> ();
            List<unit.goblin> goblins = new List<unit.goblin> ();
            bool battle_joined = true;

            for (int i = 0; i < input[0].Length; i++) {
                for (int j = 0; j < input.Count (); j++) {
                    switch (input[i][j]) {
                        case '#':
                            nodes.Add (new node () {
                                location = new coordinate (i, j),
                                    wall = true,
                                    occupant = null
                            });
                            break;
                        case '.':
                            nodes.Add (new node () {
                                location = new coordinate (i, j),
                                    wall = false,
                                    occupant = null
                            });
                            break;
                        case 'E':
                            var elf = new unit.elf () {
                                location = new coordinate (i, j)
                            };
                            elves.Add (elf);
                            nodes.Add (new node () {
                                location = new coordinate (i, j),
                                    wall = false,
                                    occupant = elf
                            });
                            break;
                        case 'G':
                            var goblin = new unit.goblin () {
                                location = new coordinate (i, j)
                            };
                            goblins.Add (goblin);
                            nodes.Add (new node () {
                                location = new coordinate (i, j),
                                    wall = false,
                                    occupant = goblin
                            });
                            break;
                        default:
                            break;
                    }
                }
            }

            while (battle_joined) {
                
            }

        }
        //get adjacent squares
        //if there is an enemy, attack the one with the lowest hp       
        //get all enemies and their adjacent squares
        //Dijkstra's algorithm
        //generate a coord > Dictionary wrappers to determine next position to be in
        //determine direction to move and move
        //outcome: number of rounds multiplied by the total hp of remaining units

        public List<char> shortest_path (char start, char finish, Dictionary<char, Dictionary<char, int>> vertices) {
            var previous = new Dictionary<char, char> ();
            var distances = new Dictionary<char, int> ();
            var nodes = new List<char> ();

            List<char> path = null;

            foreach (var vertex in vertices) {
                if (vertex.Key == start) {
                    distances[vertex.Key] = 0;
                } else {
                    distances[vertex.Key] = int.MaxValue;
                }

                nodes.Add (vertex.Key);
            }

            while (nodes.Count != 0) {
                nodes.Sort ((x, y) => distances[x] - distances[y]);

                var smallest = nodes[0];
                nodes.Remove (smallest);

                if (smallest == finish) {
                    path = new List<char> ();
                    while (previous.ContainsKey (smallest)) {
                        path.Add (smallest);
                        smallest = previous[smallest];
                    }

                    break;
                }

                if (distances[smallest] == int.MaxValue) {
                    break;
                }

                foreach (var neighbor in vertices[smallest]) {
                    var alt = distances[smallest] + neighbor.Value;
                    if (alt < distances[neighbor.Key]) {
                        distances[neighbor.Key] = alt;
                        previous[neighbor.Key] = smallest;
                    }
                }
            }

            return path;
        }

    }
}