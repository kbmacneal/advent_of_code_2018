using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_fifteen {
    class Program {

        public enum type {
            Elf,
            Goblin
        }

        public class coordinate {
            public int x { get; set; }
            public int y { get; set; }

            public coordinate (int x, int y) {
                this.x = x;
                this.y = y;
            }
        }

        public class occupant {
            public int atk_power { get; set; } = 3;
            public int hp { get; set; } = 200;
            public coordinate location { get; set; }
        }

        public class node {
            public coordinate location;
            public bool wall { get; set; }
            public occupant occupant { get; set; }
            public type occupant_type { get; set; }
            public class movement_node : node {
                public bool visited = false;
            }
        }

        static void Main (string[] args) {

            string[] input = System.IO.File.ReadAllLines ("input.txt");

            List<node> nodes = new List<node> ();
            List<occupant> elves = new List<occupant> ();
            List<occupant> goblins = new List<occupant> ();
            bool battle_joined = true;

            for (int i = 0; i < input[0].Length; i++) {
                for (int j = 0; j < input.Count (); j++) {
                    switch (input[i][j]) {
                        case '#':
                            nodes.Add (new node () {
                                location = new coordinate (j, i),
                                    wall = true,
                                    occupant = null
                            });
                            break;
                        case '.':
                            nodes.Add (new node () {
                                location = new coordinate (j, i),
                                    wall = false,
                                    occupant = null
                            });
                            break;
                        case 'E':
                            var elf = new occupant () {
                                location = new coordinate (j, i)
                            };
                            elves.Add (elf);
                            nodes.Add (new node () {
                                location = new coordinate (j, i),
                                    wall = false,
                                    occupant = elf
                            });
                            break;
                        case 'G':
                            var goblin = new occupant () {
                                location = new coordinate (j, i)
                            };
                            goblins.Add (goblin);
                            nodes.Add (new node () {
                                location = new coordinate (j, i),
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

                var agents = nodes.Where (e => !e.wall && e.occupant != null);

                foreach (var agent in agents.OrderBy (e => e.location.y).ThenBy (e => e.location.x)) {

                    var adjacent = get_adjacent (nodes, agent);
                    if (adjacent.Where (e => e.occupant != null).Where (e => nameof (e.occupant) != nameof (agent.occupant)).Count () > 0) {
                        //if we have a target, attack it then continue
                        attack_adjacent (agent, nodes);
                        continue;
                    }
                    //we have no target at the start of the turn, so we move

                    var enemies = nodes.Where (e => nameof (e.occupant) != nameof (agent.occupant));
                }

            }

        }

        public static List<node.movement_node> shortest_path (node actor, List<node> valid_nodes) {
            List<node.movement_node> shortest = new List<node.movement_node> ();

            Dictionary<node.movement_node, int> unvisited_set = new Dictionary<node.movement_node, int> ();

            foreach (var node in valid_nodes) {
                if (node == actor) {
                    unvisited_set.Add (node as node.movement_node, 0);
                } else {
                    unvisited_set.Add (node as node.movement_node, Int32.MaxValue);
                }
            }

            foreach (var node in unvisited_set) {
                
            }

            return shortest;
            // For the current node, consider all of its unvisited neighbors and calculate their tentative distances through the current node. Compare the newly calculated tentative distance to the current assigned value and assign the smaller one. For example, if the current node A is marked with a distance of 6, and the edge connecting it with a neighbor B has length 2, then the distance to B through A will be 6 + 2 = 8. If B was previously marked with a distance greater than 8 then change it to 8. Otherwise, keep the current value.
            // When we are done considering all of the unvisited neighbors of the current node, mark the current node as visited and remove it from the unvisited set. A visited node will never be checked again.
            // If the destination node has been marked visited (when planning a route between two specific nodes) or if the smallest tentative distance among the nodes in the unvisited set is infinity (when planning a complete traversal; occurs when there is no connection between the initial node and remaining unvisited nodes), then stop. The algorithm has finished.
            // Otherwise, select the unvisited node that is marked with the smallest tentative distance, set it as the new "current node", and go back to step 3.
        }

        public static void attack_adjacent (node agent, List<node> nodes) {
            var adjacent = get_adjacent (nodes, agent);

            var target = adjacent.Where (e => e.occupant != null).Where (e => nameof (e.occupant) != nameof (agent.occupant)).OrderBy (e => e.occupant.hp).ThenBy (e => e.location.y).ThenBy (e => e.location.x).First ();

            target.occupant.hp -= agent.occupant.atk_power;
        }

        public static List<node> get_adjacent (List<node> nodes, node start) {
            return nodes.Where (e => !e.wall).Where (e => e.location.x == start.location.x + 1 || e.location.x == start.location.x - 1 || e.location.y == start.location.y + 1 || e.location.y == start.location.y - 1).ToList ();
        }
        //get all enemies and their adjacent squares
        //Dijkstra's algorithm
        //generate a coord > Dictionary wrappers to determine next position to be in
        //determine direction to move and move
        //outcome: number of rounds multiplied by the total hp of remaining units

    }
}