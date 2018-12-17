using System;
using System.IO;

namespace day_seventeen
{
    class Program
    {
        static void Main(string[] args)
        {
            Go();
        }

        public static char[,] grid;
        public static int maxY = 0;
        public static int minY = int.MaxValue;

        public static void Go()
        {
            var input = File.ReadAllLines("input.txt");
            var x = 2000;
            var y = 2000;

            grid = new char[x, y];

            foreach (var line in input)
            {
                var l = line.Split(new[] { '=', ',', '.' });

                if (l[0] == "x")
                {
                    x = int.Parse(l[1]);
                    y = int.Parse(l[3]);
                    var len = int.Parse(l[5]);
                    for (var a = y; a <= len; a++)
                    {
                        grid[x, a] = '#';
                    }
                }
                else
                {
                    y = int.Parse(l[1]);
                    x = int.Parse(l[3]);
                    var len = int.Parse(l[5]);
                    for (var a = x; a <= len; a++)
                    {
                        grid[a, y] = '#';
                    }
                }

                if (y > maxY)
                {
                    maxY = y;
                }

                if (y < minY)
                {
                    minY = y;
                }
            }

            var springX = 500;
            var springY = 0;

            // fill with water
            GoDown(springX, springY);

            // count spaces with water
            var t = 0;
            for (y = minY; y < grid.GetLength(1); y++)
            {
                for (x = 0; x < grid.GetLength(0); x++)
                {
                    //if (grid[x, y] == 'W' || grid[x, y] == '|') // Part 1
                     if (grid[x,y] == 'W') // Part 2
                    {
                        t++;
                    }
                }
            }

            Console.WriteLine(t);
        }

        private static bool SpaceTaken(int x, int y)
        {
            return grid[x, y] == '#' || grid[x, y] == 'W';
        }

        public static void GoDown(int x, int y)
        {
            grid[x, y] = '|';
            while (grid[x, y + 1] != '#' && grid[x, y + 1] != 'W')
            {

                y++;
                if (y > maxY)
                {
                    return;
                }
                grid[x, y] = '|';
            };

            do
            {
                bool goDownLeft = false;
                bool goDownRight = false;

                // find boundaries
                int minX;
                for (minX = x; minX >= 0; minX--)
                {
                    if (SpaceTaken(minX, y + 1) == false)
                    {
                        goDownLeft = true;
                        break;
                    }

                    grid[minX, y] = '|';

                    if (SpaceTaken(minX - 1, y))
                    {
                        break;
                    }

                }

                int maxX;
                for (maxX = x; maxX < grid.GetLength(0); maxX++)
                {
                    if (SpaceTaken(maxX, y + 1) == false)
                    {
                        goDownRight = true;

                        break;
                    }

                    grid[maxX, y] = '|';

                    if (SpaceTaken(maxX + 1, y))
                    {
                        break;
                    }

                }

                // handle water falling
                if (goDownLeft)
                {
                    if (grid[minX, y] != '|')
                        GoDown(minX, y);
                }

                if (goDownRight)
                {
                    if (grid[maxX, y] != '|')
                        GoDown(maxX, y);
                }

                if (goDownLeft || goDownRight)
                {
                    return;
                }

                // fill row
                for (int a = minX; a < maxX + 1; a++)
                {
                    grid[a, y] = 'W';
                }

                y--;
            }
            while (true);
        }
    }
}
