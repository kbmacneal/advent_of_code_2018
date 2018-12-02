using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_one {
    class Program {
        static void Main (string[] args) {
            string input = "input.txt";

            int[] vals = System.IO.File.ReadAllLines (input).ToList ().Select (e => Int32.Parse (e)).ToArray ();

            Console.WriteLine ("Solution to Part 1:");
            Console.WriteLine (vals.Sum ());

            List<int> running_count = new List<int> ();

            int val = 0;

            running_count.Add (val);

            bool found = false;

            while (!found) {
                foreach (var item in vals) {
                    val = item + val;

                    if (running_count.Contains (val)) {
                        found = true;
                        Console.WriteLine ("Solution for Part 2:");
                        Console.WriteLine (val);
                        break;
                    }

                    running_count.Add (val);
                }
            }

        }
    }
}