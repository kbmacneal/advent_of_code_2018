using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_twenty
{
    public class room
    {
        public int x { get; set; }
        public int y { get; set; }
        public int depth { get; set; }

        public room(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public room() { }

        public room(int x, int y, int depth)
        {
            this.x = x;
            this.y = y;
            this.depth = depth;
        }

    }
    class Program
    {
        static string[] directions = new string[] { "N", "E", "S", "W" };

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")[0];

            List<room> rooms = new List<room>();

            int x = 0;
            int y = 0;

            var origin = new room(x, y);

            rooms.Add(origin);

            int prev_x = x;
            int prev_y = y;

            

            var depth = 0;


            traverse(input.Substring(1),rooms,origin,0);
        }

        public static object traverse(string instructions, List<room> rooms, room origin, int depth)
        {
            int x = origin.x;
            int y = origin.y;

            foreach (var item in instructions.Split(""))
            {
                if (directions.Contains(item))
                {
                    switch (item)
                    {
                        case "N":
                            y++;
                            break;
                        case "S":
                            y--;
                            break;
                        case "E":
                            x++;
                            break;
                        case "W":
                            x--;
                            break;
                        default:
                            break;
                    }

                    if (rooms.Where(e => e.x == x && e.y == y).Count() > 0)
                    {
                        rooms.First(e => e.x == x && e.y == y).depth = depth;
                    }
                    else
                    {
                        depth++;
                        rooms.Add(new room(x, y, depth));
                    }

                }
                else if (item == "|")
                {
                    x = origin.x; y = origin.y; depth = 0;

                }
                else if (item == "(")
                {
                    traverse(instructions,rooms,new room(x,y),depth);
                }
                else if(item == ")")
                {
                    return null; 
                }
                else if(item == "$")
                {
                    return rooms;
                }
                else
                {
                    Console.WriteLine("Invalid Char: " + item);
                    return null; 
                }
            }

            return rooms.Max(e=>e.depth);

        }
    }
}
