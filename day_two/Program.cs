using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_two {
    class helper_return {
        public bool result { get; set; }
        public string of_what { get; set; }
    }
    class Program {
        static void Main (string[] args) {
            string input = "input.txt";

            string[] vals = System.IO.File.ReadAllLines (input);

            int twos = 0;
            int threes = 0;

            foreach (var val in vals) {
                if (contains_x_of_char (val, 2).result) {
                    twos++;
                }

                if (contains_x_of_char (val, 3).result) {
                    threes++;
                }
            }

            Console.WriteLine ("Solution to Part 1:");
            Console.WriteLine (twos * threes);

            foreach (var item in vals) {
                foreach (var compare in vals) {
                    if (item != compare) {
                        if (part_two_comparison (item, compare) != null) {
                            Console.WriteLine ("Solution to Part 2:");

                            string rtn = part_two_comparison (item, compare);

                            Console.WriteLine(rtn);

                            return;
                        }
                    }
                }
            }

        }

        public static helper_return contains_x_of_char (string val, int target) {
            char[] splits = val.ToCharArray ();

            helper_return rtn = new helper_return { result = false, of_what = null };

            foreach (char item in splits) {
                if (splits.Where (e => e == item).Count () == target) {
                    rtn.result = true;
                    rtn.of_what = item.ToString ();
                    return rtn;
                }
            }

            return rtn;
        }

        public static string part_two_comparison (string a, string b) {
            char[] a_chars = a.ToCharArray ();

            char[] b_chars = b.ToCharArray ();

            string rtn = null;

            int diff_sum = 0;

            for (int i = 0; i < a_chars.Length; i++) {
                if (a_chars[i] != b_chars[i]) diff_sum++;
            }

            if (diff_sum == 1) {
                for (int i = 0; i < a_chars.Length; i++) {
                    if (a_chars[i] != b_chars[i]) {
                        List<char> a_char_list = a_chars.ToList ();
                        a_char_list.RemoveAt (i);
                        rtn = string.Join("",a_char_list);
                    }
                }

            }

            return rtn;
        }
    }
}