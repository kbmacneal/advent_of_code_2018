using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace day_twelve
{
    class Program
    {

        public static void Main(string[] args)
        {
            Part1(20);
            Part2();
        }
        public static void Part1(long num_generations)
        {
            var lines = File.ReadAllLines("input.txt");
            var state = new StringBuilder(lines[0].Substring(15).Trim());
            List<int> sums = new List<int>();
            List<string> sum_strings = new List<string>();
            var instructions = lines.Skip(2).Select(x =>
            {
                var instruction = x.Split(" => ");
                return (from: instruction[0], to: instruction[1][0]);
            }).ToDictionary(x => x.from, x => x.to);
            var minValue = 0;
            for (long i = 0; i < num_generations; i++)
            {
                var minPotted = state.ToString().IndexOf('#');
                for (int j = 0; j < 4 - minPotted; j++)
                {
                    state.Insert(0, '.');
                    minValue--;
                }

                var s = state.ToString();
                var maxPotted = state.ToString().LastIndexOf('#');
                for (int j = 0; j < 5 - (s.Length - maxPotted); j++)
                {
                    state.Append('.');
                }

                var currentState = state.ToString();
                for (int j = 0; j < currentState.Length - 5; j++)
                {
                    var sub = currentState.Substring(j, 5);
                    if (instructions.ContainsKey(sub))
                    {
                        state[j + 2] = instructions[sub];
                    }
                    else
                    {
                        state[j + 2] = '.';
                    }
                }

                var inside_sum = 0;
                for (int z = 0; z < state.Length; z++)
                {
                    if (state[z] == '#')
                    {
                        inside_sum += z + minValue;
                    }
                }

                int last = 0;
                if (sums.Count() > 0) last = sums.Last();
                sums.Add(inside_sum);

                sum_strings.Add(inside_sum.ToString() + " | " + (inside_sum - last).ToString());
            }

            File.WriteAllLines("gens.txt", sum_strings);

            var finalState = state.ToString();
            var sum = 0;
            for (int z = 0; z < finalState.Length; z++)
            {
                if (finalState[z] == '#')
                {
                    sum += z + minValue;
                }
            }

            Console.WriteLine(sum);
        }

        public static void Part2()
        {
            var i = 50000000000;

            Part1(200);
            Console.WriteLine(5011 + ((i - 167) * 26));
        }
    }
}

