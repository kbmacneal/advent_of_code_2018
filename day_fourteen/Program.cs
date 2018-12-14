using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace day_fourteen
{
    class Program
    {
        static void Main(string[] args)
        {
            var string_input = "030121";
            var input = Int32.Parse(string_input);

            List<int> scoreboard = new List<int> { 3, 7 };

            var first = 0;
            var second = 1;

            int part_two;

            while (true)
            {
                var s = new string(scoreboard.Skip(scoreboard.Count - string_input.Length - 1).Select(x => x.ToString()[0]).ToArray());

                if (s.Contains(string_input))
                {
                    s = new string(scoreboard.Select(x => x.ToString()[0]).ToArray());
                    part_two = s.IndexOf(string_input);
                    break;
                }

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

            Console.WriteLine(part_two);
        }

    }
}
