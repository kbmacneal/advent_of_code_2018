using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace day_four
{
    public class event_record
    {
        public DateTime timestamp { get; set; }
        public int index { get; set; }
        public bool begin_shift { get; set; }
        public bool fall_asleep { get; set; }
        public bool wake_up { get; set; }
        public int guard_id { get; set; }

    }

    public class guard
    {
        public int id { get; set; }
        public int total_sleep { get; set; } = 0;
        public List<int> asleep_minute { get; set; } = new List<int>();
        public int maximal_minute { get; set; }=0;
        public int maximal_frequency { get; set; }=0;
    }
    class Program
    {
        static void Main(string[] args)
        {
            var contents = System.IO.File.ReadAllLines("input.txt");

            List<event_record> records = new List<event_record>();

            int guard_on_duty = 0;

            for (int i = 0; i < contents.Length; i++)
            {
                string currow = contents[i];

                if (currow.Contains("#"))
                {
                    guard_on_duty = Int32.Parse(currow.Split(" ")[3].Replace("#", ""));
                }
                else
                {
                    guard_on_duty = 0;
                }

                event_record record = new event_record()
                {
                    timestamp = pull_datetime_from_record(currow),
                    index = i,
                    begin_shift = currow.Contains("#"),
                    fall_asleep = currow.Contains("falls"),
                    wake_up = currow.Contains("wakes"),
                    guard_id = guard_on_duty
                };

                records.Add(record);
            }

            int guard_last_seen = 0;
            foreach (var record in records.OrderBy(e => e.timestamp))
            {
                if (record.guard_id != 0)
                {
                    guard_last_seen = record.guard_id;
                }
                else
                {
                    record.guard_id = guard_last_seen;
                }
            }

            Part1(records);


        }

        public static void Part1(List<event_record> records)
        {
            List<int> guards_list = records.Select(e => e.guard_id).Distinct().ToList();

            List<guard> guards = new List<guard>();

            foreach (var item in guards_list)
            {
                guard guard = new guard();

                guard.id = item;

                List<event_record> specific_list = records.Where(e => e.guard_id == item && e.begin_shift == false).OrderBy(e => e.timestamp).ToList();

                for (int i = 0; i < specific_list.Count(); i = i + 2)
                {
                    event_record currow = specific_list[i];
                    event_record next_row = specific_list[i + 1];

                    TimeSpan span = next_row.timestamp.Subtract(currow.timestamp);

                    guard.total_sleep += span.Minutes;

                    DateTime start = currow.timestamp;

                    while (start < next_row.timestamp)
                    {
                        guard.asleep_minute.Add(start.Minute);

                        start = start.AddMinutes(1);
                    }
                }

                guards.Add(guard);
            }

            Console.WriteLine("Sleepiest Guard");
            Console.WriteLine(guards.OrderByDescending(e => e.total_sleep).First().id);
            Console.WriteLine("Sleepiest Minute");
            var most = (from i in guards.First(e => e.id == guards.OrderByDescending(f => f.total_sleep).First().id).asleep_minute
                        group i by i into grp
                        orderby grp.Count() descending
                        select grp.Key).First();
            Console.WriteLine(most);
            Console.WriteLine("Multiplied");
            Console.WriteLine(guards.OrderByDescending(e => e.total_sleep).First().id * most);

            Part2(guards);
        }

        public static void Part2(List<guard> guards)
        {
            foreach (var guard in guards)
            {
                if(guard.asleep_minute.Count() == 0)continue;
                var most = guard.asleep_minute.GroupBy(i=>i).OrderByDescending(grp=>grp.Count()).Select(grp=>grp.Key).First();

                guard.maximal_minute = most;
                guard.maximal_frequency = guard.asleep_minute.Where(e=>e== most).Count();
            }

            var answer = guards.First(e=>e.maximal_frequency == guards.Max(f=>f.maximal_frequency));

            Console.WriteLine("Part Two Guard ID:");
            Console.WriteLine(answer.id);
            Console.WriteLine("Part Two Max Freq");
            Console.WriteLine(guards.Max(f=>f.maximal_frequency));
            Console.WriteLine("Part Two Max Freq Minute:");
            Console.WriteLine(answer.maximal_minute);
            Console.WriteLine("Multiplied");
            Console.WriteLine(answer.id * answer.maximal_minute);
        }

        // public static T MostCommon<T>(this IEnumerable<T> list)
        // {
        //     return list.GroupBy(i => i).OrderByDescending(grp => grp.Count())
        //       .Select(grp => grp.Key).First();
        // }

        public static DateTime pull_datetime_from_record(string record)
        {
            string raw_stamp = record.Split("]")[0].Replace("[", "");

            return DateTime.ParseExact(raw_stamp, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
