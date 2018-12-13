using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day_thirteen
{
    class Program
    {
        static void Main(string[] args)
        {
            Solve();
        }

        public static void Solve()
        {
            var lines = System.IO.File.ReadAllLines("input.txt");
            var maxLine = lines.Max(x => x.Length);
            var grid = new char[lines.Length, maxLine];
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    grid[i, j] = lines[i][j];
                }
            }
            var cartSymbols = new[] { '^', 'v', '>', '<' };
            var carts = new List<(int x, int y, char dir, char turn, bool crashed)>();
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (!cartSymbols.Contains(grid[y, x]))
                    {
                        continue;
                    }
                    carts.Add((x, y, grid[y, x], 'l', false));
                    if (grid[y, x] == '^' || grid[y, x] == 'v')
                    {
                        grid[y, x] = '|';
                    }
                    else
                    {
                        grid[y, x] = '-';
                    }
                }
            }

            var turns = new Dictionary<(char dir, char gridSymbol), char>
    {
        { ('<', '/'), 'v' },
        { ('^', '/'), '>' },
        { ('>', '/'), '^' },
        { ('v', '/'), '<' },
        { ('<', '\\'), '^' },
        { ('^', '\\'), '<' },
        { ('>', '\\'), 'v' },
        { ('v', '\\'), '>' },
    };
            var intersections = new Dictionary<(char dir, char turn), (char dir, char turn)>
    {
        { ('<', 'l'), ('v', 's') },
        { ('<', 's'), ('<', 'r') },
        { ('<', 'r'), ('^', 'l') },
        { ('^', 'l'), ('<', 's') },
        { ('^', 's'), ('^', 'r') },
        { ('^', 'r'), ('>', 'l') },
        { ('>', 'l'), ('^', 's') },
        { ('>', 's'), ('>', 'r') },
        { ('>', 'r'), ('v', 'l') },
        { ('v', 'l'), ('>', 's') },
        { ('v', 's'), ('v', 'r') },
        { ('v', 'r'), ('<', 'l') },
    };
            var done = false;
            while (!done)
            {
                //for (int y = 0; y < lines.Length; y++)
                //{
                //    for (int x = 0; x < lines[y].Length; x++)
                //    {
                //        var c = carts.FirstOrDefault(cart => !cart.crashed && cart.x == x && cart.y == y);
                //        if (c != default)
                //        {
                //            Console.Write(c.dir);
                //        }
                //        else
                //        {
                //            Console.Write(grid[y,x]);
                //        }
                //    }
                //    Console.WriteLine();
                //}
                if (carts.Count(x => !x.crashed) == 1)
                {
                    Console.WriteLine(carts.First(x => !x.crashed));
                    done = true;
                    continue;
                }
                var orderedCarts = carts.OrderBy(x => x.y).ThenBy(x => x.x).ToList();
                for (var i = 0; i < orderedCarts.Count; i++)
                {
                    var cart = orderedCarts[i];
                    if (cart.crashed)
                    {
                        continue;
                    }
                    var (x, y) = GetNextPoint(cart.x, cart.y, cart.dir);
                    //part1
                    //if (orderedCarts.Any(c => c.x == x && c.y == y))
                    //{
                    //    Console.WriteLine((x, y));
                    //    done = true;
                    //}
                    var crashedCartIndex = orderedCarts.FindIndex(c => !c.crashed && c.x == x && c.y == y);
                    if (crashedCartIndex >= 0)
                    {
                        orderedCarts[i] = (x, y, cart.dir, cart.turn, true);
                        orderedCarts[crashedCartIndex] = (x, y, cart.dir, cart.turn, true);
                        continue;
                    }

                    var gridSymbol = grid[y, x];
                    if (gridSymbol == '\\' || gridSymbol == '/')
                    {
                        orderedCarts[i] = (x, y, turns[(cart.dir, gridSymbol)], cart.turn, cart.crashed);
                    }
                    else if (gridSymbol == '+')
                    {
                        var afterInter = intersections[(cart.dir, cart.turn)];
                        orderedCarts[i] = (x, y, afterInter.dir, afterInter.turn, cart.crashed);
                    }
                    else
                    {
                        orderedCarts[i] = (x, y, cart.dir, cart.turn, cart.crashed);
                    }
                }

                carts = orderedCarts;
            }
        }

        private static (int x, int y) GetNextPoint(int x, int y, char dir)
        {
            switch (dir)
            {
                case '^':
                    return (x, y - 1);
                case 'v':
                    return (x, y + 1);
                case '>':
                    return (x + 1, y);
                case '<':
                    return (x - 1, y);
            }
            throw new ArgumentException();
        }
    }
}