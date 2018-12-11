using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace day_eleven
{
    class Program
    {
        public class fuel_cell
        {
            public int x { get; set; }
            public int y { get; set; }
            public int power_level { get; set; }

            public int get_power_level(int input)
            {
                int temp = (((this.x + 10) * this.y) + input) * (this.x + 10);

                int hundredDigit = (int)Math.Abs(temp / 100 % 10);

                return hundredDigit - 5;
            }

        }

        public class result
        {
            public fuel_cell top_left { get; set; }
            public int total_power { get; set; }
            public int size {get;set;}

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }
        static void Main(string[] args)
        {
            var input = 7347;
            // var input = 18;

            List<fuel_cell> cells = new List<fuel_cell>();

            for (int i = 1; i <= 300; i++)
            {
                for (int j = 1; j <= 300; j++)
                {
                    fuel_cell cell = new fuel_cell()
                    {
                        x = i,
                        y = j
                    };

                    cell.power_level = cell.get_power_level(input);

                    cells.Add(cell);
                }
            }

            // List<fuel_cell> test = cells.Where(e=>e.power_level <=0).ToList();

            int max_power = 0;
            int max_power_part_two = 0;
            result max_result = new result();
            result max_part_two = new result();

            foreach (var cell in cells)
            {
                var temp = get_three_by_three_power_rating(cell,cells,3);

                if(temp > max_power)
                {
                    max_result = new result(){
                        top_left = cell,
                        total_power = temp,
                        size=3
                    };
                    max_power = temp;
                }
            }

            Console.WriteLine(max_result.ToString());

            foreach (var cell in cells)
            {

                for (int i = 3; i < 300; i++)
                {
                    var part_2 = get_three_by_three_power_rating(cell, cells, i);

                    if (part_2 == 0)
                    {
                        continue;
                    }

                    if (part_2 > max_power_part_two)
                    {
                        max_part_two = new result()
                        {
                            top_left = cell,
                            total_power = part_2,
                            size = i
                        };
                        max_power_part_two = part_2;
                    }
                }


            }

            
            //if this doesnt work, the answer is 1,1,300
            Console.WriteLine(max_part_two.ToString());

        }

        public static int get_three_by_three_power_rating(fuel_cell top_left, List<fuel_cell> grid, int search_size)
        {
            int total_power = 0;

            int start_x = top_left.x;
            int start_y = top_left.y;

            for (int i = 0; i < search_size; i++)
            {
                for (int j = 0; j < search_size; j++)
                {
                    var temp = grid.FirstOrDefault(e => e.x == start_x + i && e.y == start_y + j);

                    if (temp != null)
                    {
                        total_power += temp.power_level;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            return total_power;
        }
    }
}
