using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_sixteen {

    public class Operation {
        public Operation (Action<int, int, int> action) {
            Action = action;
        }
        public Action<int, int, int> Action { get; set; }
        public int OpCode { get; set; } = -1;
    }

    class Program {
        static int[] reg = new int[] { 0, 0, 0, 0 };
        static void Main (string[] args) {
            var ops = new List<Operation> ();

            //addr
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] + reg[b];
            }));
            //addi
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] + b;
            }));
            //mulr
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] * reg[b];
            }));
            //muli
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] * b;
            }));
            //banr
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] & reg[b];
            }));
            //bani
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] & b;
            }));
            //borr
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] | reg[b];
            }));
            //bori
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] | b;
            }));
            //setr
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a];
            }));
            //seti
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = a;
            }));
            //gtir
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = a > reg[b] ? 1 : 0;
            }));
            //gtri
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] > b ? 1 : 0;
            }));
            //gtrr
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] > reg[b] ? 1 : 0;
            }));
            //eqir
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = a == reg[b] ? 1 : 0;
            }));
            //eqri
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] == b ? 1 : 0;
            }));
            //eqrr
            ops.Add (new Operation ((a, b, c) => {
                reg[c] = reg[a] == reg[b] ? 1 : 0;
            }));

            var input1 = File.ReadAllLines ("input1.txt");

            var input2 = File.ReadAllLines ("input2.txt");

            int three_or_more = 0;

            for (int i = 0; i < input1.Length; i += 4) {
                var bf = input1[i];
                var before = new int[] {
                    int.Parse (bf.Substring (9, 1)),
                    int.Parse (bf.Substring (12, 1)),
                    int.Parse (bf.Substring (15, 1)),
                    int.Parse (bf.Substring (18, 1))
                };
                var af = input1[i + 2];
                var after = new int[] {
                    int.Parse (af.Substring (9, 1)),
                    int.Parse (af.Substring (12, 1)),
                    int.Parse (af.Substring (15, 1)),
                    int.Parse (af.Substring (18, 1))
                };
                var p = input1[i + 1].Split (' ');
                var o = int.Parse (p[0]);
                var a = int.Parse (p[1]);
                var b = int.Parse (p[2]);
                var c = int.Parse (p[3]);

                var matches = FindOpCodes (ops, before, a, b, c, o, after);

                if (matches > 2) three_or_more++;
            }

            Console.WriteLine (three_or_more);

            reg = new int[] {0,0,0,0};

            for (int i = 0; i < input1.Length; i += 4) {
                var bf = input1[i];
                var before = new int[] {
                    int.Parse (bf.Substring (9, 1)),
                    int.Parse (bf.Substring (12, 1)),
                    int.Parse (bf.Substring (15, 1)),
                    int.Parse (bf.Substring (18, 1))
                };
                var af = input1[i + 2];
                var after = new int[] {
                    int.Parse (af.Substring (9, 1)),
                    int.Parse (af.Substring (12, 1)),
                    int.Parse (af.Substring (15, 1)),
                    int.Parse (af.Substring (18, 1))
                };
                var p = input1[i + 1].Split (' ');
                var o = int.Parse (p[0]);
                var a = int.Parse (p[1]);
                var b = int.Parse (p[2]);
                var c = int.Parse (p[3]);

                MatchOperations (ops, before, a, b, c, o, after);
            }
            
            foreach (var line in input2)
            {
                var inputs = line.Split(' ');
                var o = int.Parse(inputs[0]);
                var a = int.Parse(inputs[1]);
                var b = int.Parse(inputs[2]);
                var c = int.Parse(inputs[3]);
                var op = ops.First(x => x.OpCode == o);
                op.Action(a,b,c);
            }

            Console.WriteLine(reg[0]);

        }

        static int FindOpCodes(
            List<Operation> ops, 
            int[] before, 
            int a, int b, int c, int opCode,
            int[] after)
        {
            var count = 0;
            Operation lastMatch = new Operation(null); 
            ops.Where(x => x.OpCode == -1).ToList().ForEach(op =>
            {
                reg = (int[])before.Clone();
                op.Action(a, b, c);
                if (reg.SequenceEqual(after)) { 
                    count++;
                    lastMatch = op;
                }
            });
            return count;
        }

        static int MatchOperations (
            List<Operation> ops,
            int[] before,
            int a, int b, int c, int opCode,
            int[] after) {
            var count = 0;
            Operation lastMatch = new Operation (null);
            ops.Where (x => x.OpCode == -1).ToList ().ForEach (op => {
                reg = (int[]) before.Clone ();
                op.Action (a, b, c);
                if (reg.SequenceEqual (after)) {
                    count++;
                    lastMatch = op;
                }
            });
            if (count == 1) {
                lastMatch.OpCode = opCode;
            }
            return count;
        }
    }
}