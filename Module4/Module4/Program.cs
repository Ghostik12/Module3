using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace Module4 { 

    public class Module4
    {
        public static void Main()
        {
            //  string[] favcolor = new string[3];
            // for (int i = 0; i < favcolor.Length; i++)
            // {
            //     Console.WriteLine("Write your favorite color(number){0}", i + 1);
            //    favcolor[i] = Console.ReadLine();
            // }

            // foreach (var color in favcolor)
            // {
            //     switch (color)
            // {
            //  case "red":
            //    Console.BackgroundColor = ConsoleColor.Red;
            //    Console.ForegroundColor = ConsoleColor.Black;
            //    Console.WriteLine("Your color is red");
            //    break;

            // case "green":
            //    Console.BackgroundColor = ConsoleColor.Green;
            //    Console.ForegroundColor = ConsoleColor.Black;
            //    Console.WriteLine("Your color is green");
            //    break;

            // case "cyan":
            //  Console.BackgroundColor = ConsoleColor.Cyan;
            //  Console.ForegroundColor = ConsoleColor.Black;
            // Console.WriteLine("Your color is cyan");
            //  break;

            // default:
            //   Console.BackgroundColor = ConsoleColor.Yellow;
            //    Console.ForegroundColor = ConsoleColor.Black;
            //   Console.WriteLine("Color is yellow");
            //   break;
            // }
            // }
            //  Console.BackgroundColor = ConsoleColor.Black;
            //  Console.ForegroundColor = ConsoleColor.White;

            //Console.WriteLine("Write your name: ");
            //string name;
            //name = Console.ReadLine();
            //Console.WriteLine("Your name on letter: ");
            //for (int i = name.Length - 1; i >= 0; i--)
            //{

            //   Console.Write(name[i] + " ");
            //}
            //Console.WriteLine("Last letter your name: {0}", name.Substring(name.Length - 1));

            // int[] array = {2, 4, 5, 1, 3, 6, 8, 7 };
            //int s = 0;
            //for (int i = 0; i < array.Length; i++)
            //{
            //    s += array[i];
            //}
            //Console.Write(s);

            int[,] array = {{ -1, 4, 3, -5, 2 },{ 7, -1, 5, -6, 4} };
            var x = 0;
            
            for (int i = 0;  i <= array.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= array.GetUpperBound(1); j++)
                {
                    for (int k = j+1; k <= array.GetUpperBound(1); k++)
                    {
                        if (array[i, j] > array[i, k])
                        {
                            x = array[i, k];
                            array[i, k] = array[i, j];
                            array[i, j] = x;
                        }
                    }
                    Console.Write(array[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
