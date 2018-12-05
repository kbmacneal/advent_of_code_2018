using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace day_five
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("input.txt")[0].ToString();

            List<char> sequence = input.ToCharArray().ToList();

            Part1(sequence);
            Part2();
        }

        public static void Part1(List<char> sequence)
        {
            List<char> working_set = sequence;

            int num_changes = 0;
            do
            {
                num_changes = 0;
                for (int i = 0; i < working_set.Count()-1; i++)
                {
                    if(working_set[i] != working_set[i+1] && char.ToUpper(working_set[i]) == char.ToUpper(working_set[i+1]))
                    {
                        num_changes++;
                        working_set[i] = '_';
                        working_set[i+1]='_';
                    }
                }
                working_set.RemoveAll(e => e == '_');
            } while (num_changes > 0);

            Console.WriteLine("Part 1:");
            Console.WriteLine(working_set.Count());
        }

        public static void Part2()
        {            
            var input = System.IO.File.ReadAllLines("input.txt")[0].ToString();

            List<char> sequence = input.ToCharArray().ToList();

            List<char> working_set = sequence;

            List<char> distinct_chars = working_set.Select(e=>char.ToLower(e)).Distinct().OrderBy(e=>(int)e).ToList();

            Dictionary<char,int> results = new Dictionary<char, int>();

            foreach (var item in distinct_chars)
            {

                sequence = input.ToCharArray().ToList();

                sequence.RemoveAll(e=>e==item);
                sequence.RemoveAll(e=>e==char.ToUpper(item));

                results.Add(item,react_sequence(sequence).Count());
            }

            Console.WriteLine("Part 2");
            Console.WriteLine(results.Values.Min());
        }

        public static List<char> react_sequence(List<char> sequence)
        {
            int num_changes = 0;
            do
            {
                num_changes = 0;
                for (int i = 0; i < sequence.Count()-1; i++)
                {
                    if(sequence[i] != sequence[i+1] && char.ToUpper(sequence[i]) == char.ToUpper(sequence[i+1]))
                    {
                        num_changes++;
                        sequence[i] = '_';
                        sequence[i+1]='_';
                    }
                }
                sequence.RemoveAll(e => e == '_');
            } while (num_changes > 0);

            return sequence;
        }
    }
}
