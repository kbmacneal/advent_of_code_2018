using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_sixteen {

    public class command {
        public int opcode { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
    }
    public class input {
        public int[] before_registers { get; set; }
        public command command { get; set; }
        public int[] after_registers { get; set; }
        public List<string> possible_opcodes { get; set; } = new List<string> ();
    }
    class Program {
        static void Main (string[] args) {
            var input = File.ReadAllText ("input.txt").Split ("\n", StringSplitOptions.RemoveEmptyEntries);

            var last_index = input.ToList ().FindLastIndex (e => e.StartsWith ("After:"));

            var part_one_inputs = input.AsSpan ().Slice (0, last_index);

            var part_two_inputs = input.AsSpan ().Slice (last_index);

            List<input> inputs = new List<input> ();

            while (part_one_inputs.ToArray ().Length > 0) {
                var instruction = part_one_inputs.ToArray ().ToList ().Take (3);

                input i = new input ();

                i.before_registers = instruction.ToArray () [0].Replace ("Before: [", "").Replace ("]", "").Split (", ").ToList ().Select (e => Int32.Parse (e)).ToArray ();

                var split = instruction.ToArray () [1].Split (" ").Select (e => Int32.Parse (e)).ToArray ();

                command reg = new command () {
                    opcode = split[0],
                    a = split[1],
                    b = split[2],
                    c = split[3]
                };

                i.command = reg;

                i.after_registers = instruction.ToArray () [2].Replace ("After:  [", "").Replace ("]", "").Split (", ").ToList ().Select (e => Int32.Parse (e)).ToArray ();

                inputs.Add (i);

                part_one_inputs.ToArray ().ToList ().RemoveRange (0, 3);
            }

            string[] operations = new string[] {
                "addr",
                "addi",
                "mulr",
                "muli",
                "banr",
                "bani",
                "borr",
                "bori",
                "setr",
                "seti",
                "gtir",
                "gtri",
                "gtrr",
                "eqir",
                "eqri",
                "eqrr"
            };

            int part_one_count = 0;

            foreach (var i in inputs) {
                foreach (var op in operations) {
                    switch (op) {
                        case "addr":
                            if (i.after_registers[i.command.c] == i.before_registers[i.command.a] + i.before_registers[i.command.b]) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "addi":
                            if (i.after_registers[i.command.c] == i.before_registers[i.command.a] + i.command.b) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "mulr":
                            if (i.after_registers[i.command.c] == i.before_registers[i.command.a] * i.before_registers[i.command.b]) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "muli":
                            if (i.after_registers[i.command.c] == i.before_registers[i.command.a] * i.command.b) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "banr":
                            if (i.after_registers[i.command.c] == (i.before_registers[i.command.a] & i.before_registers[i.command.b])) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "bani":
                            if (i.after_registers[i.command.c] == (i.before_registers[i.command.a] & i.command.b)) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "borr":
                            if (i.after_registers[i.command.c] == (i.before_registers[i.command.a] | i.before_registers[i.command.b])) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "bori":
                            if (i.after_registers[i.command.c] == (i.before_registers[i.command.a] | i.command.b)) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "setr":
                            if (i.after_registers[i.command.c] == i.before_registers[i.command.a]) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "seti":
                            if (i.after_registers[i.command.c] == i.command.a) {
                                part_one_count++;
                                i.possible_opcodes.Add (op);
                            }
                            break;
                        case "gtir":
                            if (i.after_registers[i.command.c] == 1) {
                                if (i.command.a > i.before_registers[i.command.b]) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            } else if (i.after_registers[i.command.c] == 0) {
                                if (i.command.a <= i.before_registers[i.command.b]) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            }
                            break;
                        case "gtri":
                            if (i.after_registers[i.command.c] == 1) {
                                if (i.before_registers[i.command.a] > i.command.b) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            } else if (i.after_registers[i.command.c] == 0) {
                                if (i.before_registers[i.command.a] <= i.command.b) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            }
                            break;
                        case "gtrr":
                            if (i.after_registers[i.command.c] == 1) {
                                if (i.before_registers[i.command.a] > i.before_registers[i.command.b]) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            } else if (i.after_registers[i.command.c] == 0) {
                                if (i.before_registers[i.command.a] <= i.before_registers[i.command.b]) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            }
                            break;
                        case "eqir":
                            if (i.after_registers[i.command.c] == 1) {
                                if (i.command.a == i.before_registers[i.command.b]) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            } else if (i.after_registers[i.command.c] == 0) {
                                if (i.command.a != i.before_registers[i.command.b]) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            }
                            break;
                        case "eqri":
                            if (i.after_registers[i.command.c] == 1) {
                                if (i.before_registers[i.command.a] == i.command.b) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            } else if (i.after_registers[i.command.c] == 0) {
                                if (i.before_registers[i.command.a] != i.command.b) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            }
                            break;
                        case "eqrr":
                            if (i.after_registers[i.command.c] == 1) {
                                if (i.before_registers[i.command.a] == i.before_registers[i.command.b]) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            } else if (i.after_registers[i.command.c] == 0) {
                                if (i.before_registers[i.command.a] != i.before_registers[i.command.b]) {
                                    part_one_count++;
                                    i.possible_opcodes.Add (op);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            Console.WriteLine(inputs.Where(e=>e.possible_opcodes.Count() >2).Count());

        }
    }
}