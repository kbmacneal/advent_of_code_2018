using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_eight {
    class Program {
        static void Main (string[] args) {
            var input = System.IO.File.ReadAllText("input.txt");

            Part1(input);
            Part2(input);
        }

        public static void Part1 (string Input) {
            var numbers = Input.Split (' ').Select (int.Parse).ToList ();
            var i = 0;
            var root = ReadNode (numbers, ref i);
            Console.WriteLine (root.Sum ());
        }

        public static void Part2 (string Input) {
            var numbers = Input.Split (' ').Select (int.Parse).ToList ();
            var i = 0;
            var root = ReadNode (numbers, ref i);
            Console.WriteLine (root.Value ());
        }

        public static Node ReadNode (List<int> numbers, ref int i) {
            var node = new Node ();
            var children = numbers[i++];
            var metadata = numbers[i++];
            for (int j = 0; j < children; j++) {
                node.Nodes.Add (ReadNode (numbers, ref i));
            }

            for (int j = 0; j < metadata; j++) {
                node.Metadata.Add (numbers[i++]);
            }

            return node;
        }

        public class Node {
            public List<int> Metadata { get; set; } = new List<int> ();
            public List<Node> Nodes { get; set; } = new List<Node> ();

            public int Sum () {
                return Metadata.Sum () + Nodes.Sum (x => x.Sum ());
            }

            public int Value () {
                if (!Nodes.Any ()) {
                    return Metadata.Sum ();
                }

                var value = 0;
                foreach (var m in Metadata) {
                    if (m <= Nodes.Count) {
                        value += Nodes[m - 1].Value ();
                    }
                }

                return value;
            }
        }
    }
}