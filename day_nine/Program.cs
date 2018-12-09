using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

class MainClass {
    static int players = 471;
    static int marbles = 72026* 100;

    static long[] scores = new long[players];
    static LinkedList<int> placed = new LinkedList<int> ();
    static LinkedListNode<int> current = placed.AddFirst (0);

    static void next () {
        current = current.Next ?? placed.First;
    }
    static void previous () {
        current = current.Previous ?? placed.Last;
    }

    public static void Main (string[] args) {
        for (int m = 0; m < marbles; ++m) {
            if (((m + 1) % 23) == 0) {
                previous ();
                previous ();
                previous ();
                previous ();
                previous ();
                previous ();
                previous ();

                var j = m % players;
                scores[j] += m + 1 + current.Value;

                var tmp = current;
                next ();
                placed.Remove (tmp);
            } else {
                next ();
                current = placed.AddAfter (current, m + 1);
            }
        }
        Console.WriteLine (scores.Max ());
    }
}