using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_sixteen
{

    public class Operation
    {
        public Operation(Action<int, int, int> action, string name)
        {
            Action = action;
            this.name = name;
        }
        public Action<int, int, int> Action { get; set; }
        public int OpCode { get; set; } = -1;
        public string name { get; set; }
    }

    class Program
    {
        static int[] reg = new int[] { 0, 0, 0, 0 };
        static void Main(string[] args)
        {
            var ops = new List<Operation>();

            //addr
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] + reg[b];
            }, "addr"));
            //addi
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] + b;
            }, "addi"));
            //mulr
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] * reg[b];
            }, "mulr"));
            //muli
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] * b;
            }, "muli"));
            //banr
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] & reg[b];
            }, "banr"));
            //bani
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] & b;
            }, "bani"));
            //borr
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] | reg[b];
            }, "borr"));
            //bori
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] | b;
            }, "bori"));
            //setr
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a];
            }, "setr"));
            //seti
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = a;
            }, "seti"));
            //gtir
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = a > reg[b] ? 1 : 0;
            }, "gtir"));
            //gtri
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] > b ? 1 : 0;
            }, "gtri"));
            //gtrr
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] > reg[b] ? 1 : 0;
            }, "gtrr"));
            //eqir
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = a == reg[b] ? 1 : 0;
            }, "eqir"));
            //eqri
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] == b ? 1 : 0;
            }, "eqri"));
            //eqrr
            ops.Add(new Operation((a, b, c) =>
            {
                reg[c] = reg[a] == reg[b] ? 1 : 0;
            }, "eqrr"));

            var initial_ip = 2;
            var ip = initial_ip;

            //part 1
            reg = new int[] { 0, 0, 0, 0, 0, 0 };
            var part2 = false;

            //part 2
            // reg = new int[] { 1, 0, 0, 0, 0, 0 };
            // var part2 = true;

            if (part2)
            {
                Console.WriteLine(sum_factors(10551315));

                return;
            }

            var input = File.ReadAllLines("input.txt").ToList().Skip(1).ToArray();

            ip = reg[initial_ip];

            while (ip < input.Count())
            {
                reg[initial_ip] = ip;

                var line = input[ip];

                var inputs = line.Split(' ');
                var name = inputs[0];
                var a = int.Parse(inputs[1]);
                var b = int.Parse(inputs[2]);
                var c = int.Parse(inputs[3]);
                var op = ops.First(x => x.name == name);


                op.Action(a, b, c);

                ip = reg[initial_ip];

                ip++;

                if(part2)
                {
                    var str = String.Join(" | ", reg) + "\n";

                    File.AppendAllText("reg.txt", str);
                }
                
            }

            Console.WriteLine(reg[0]);

        }

        public static int sum_factors(int number)
        {
            var factors = new List<int>();

            for (int div = 1; div <= number; div++)
            {
                if(number%div ==0) factors.Add(div);
            }
            return factors.Sum();
        }
    }
}
