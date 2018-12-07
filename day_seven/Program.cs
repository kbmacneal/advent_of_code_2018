using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace day_seven
{
    class Program
    {
        static void Main(string[] args)
        {
            var contents = System.IO.File.ReadAllLines("input.txt");

            var dependencies = new List<(string pre, string post)>();

            contents.ToList().ForEach(e => dependencies.Add((e.Split(" must be finished before step ")[0].Replace("Step ", ""), e.Split(" must be finished before step ")[1].Replace(" can begin.", ""))));

            var allSteps = dependencies.Select(x => x.pre).Concat(dependencies.Select(x => x.post)).Distinct().OrderBy(x => x).ToList();

            var result = string.Empty;

            while (allSteps.Any())
            {
                //find the new root node
                var valid = allSteps.Where(s => !dependencies.Any(d => d.post == s)).First();

                result += valid;

                allSteps.Remove(valid);
                dependencies.RemoveAll(d => d.pre == valid);
            }

            Console.WriteLine("Part 1:");
            Console.WriteLine(result);

            Part2(contents);
        }

        public static void Part2(string[] inputs)
        {
            var dependencies = new List<(string pre, string post)>();

            inputs.ToList().ForEach(e => dependencies.Add((e.Split(" must be finished before step ")[0].Replace("Step ", ""), e.Split(" must be finished before step ")[1].Replace(" can begin.", ""))));

            var allSteps = dependencies.Select(x => x.pre).Concat(dependencies.Select(x => x.post)).Distinct().OrderBy(x => x).ToList();
            
            var workers = new List<int>(5) { 0, 0,0,0,0 };
            var currentSecond = 0;
            var doneList = new List<(string step, int finish)>();

            while (allSteps.Any() || workers.Any(w => w > currentSecond))
            {
                doneList.Where(d => d.finish <= currentSecond).ToList().ForEach(x => dependencies.RemoveAll(d => d.pre == x.step));
                doneList.RemoveAll(d => d.finish <= currentSecond);

                var valid = allSteps.Where(s => !dependencies.Any(d => d.post == s)).ToList();

                for (var w = 0; w < workers.Count && valid.Any(); w++)
                {
                    if (workers[w] <= currentSecond)
                    {
                        workers[w] = GetWorkTime(valid.First()) + currentSecond;
                        allSteps.Remove(valid.First());
                        doneList.Add((valid.First(), workers[w]));
                        valid.RemoveAt(0);
                    }
                }

                currentSecond++;
            }

            Console.WriteLine(currentSecond.ToString());
        }

        private static int GetWorkTime(string v)
        {
            return (v[0] - 'A') + 61;
            // return (v[0] - 'A') + 1;
        }
    }
}

