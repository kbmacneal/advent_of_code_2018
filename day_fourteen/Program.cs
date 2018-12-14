﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace day_fourteen
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = Int32.Parse("030121");

            // var input = 9;

            List<int> scoreboard = new List<int> { 3, 7 };

            var first = 0;
            var second = 1;

            while (scoreboard.Count() < input + 10)
            {
                var sum = scoreboard[first] + scoreboard[second];
                if (sum >= 10)
                {
                    scoreboard.Add(sum / 10);
                }
                scoreboard.Add(sum % 10);
                first = (first + scoreboard[first] + 1) % scoreboard.Count;
                second = (second + scoreboard[second] + 1) % scoreboard.Count;
            }

            foreach (var recipe in scoreboard.Skip(input).Take(10))
            {
                Console.Write(recipe);
            }
            Console.WriteLine();

            Part2();
        }

        public static void Part2()
        {
            var recipes = new List<int> { 3, 7 };
            var Input = "030121";
            var first = 0;
            var second = 1;
            while (true)
            {
                var s = new string(recipes.Skip(recipes.Count - Input.Length - 1).Select(x => x.ToString()[0]).ToArray());

                if (s.Contains(Input))
                {
                    s = new string(recipes.Select(x => x.ToString()[0]).ToArray());
                    Console.WriteLine(s.IndexOf(Input));
                    break;
                }

                var sum = recipes[first] + recipes[second];

                if (sum >= 10)
                {
                    recipes.Add(sum / 10);
                }

                recipes.Add(sum % 10);

                first = (first + recipes[first] + 1) % recipes.Count;
                
                second = (second + recipes[second] + 1) % recipes.Count;
            }


        }
    }
}