using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection.Metadata;

namespace Module4 { 

    public class Module4
    {
        //public static void Main()
        //{
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

        //int[,] array = {{ -1, 4, 3, -5, 2 },{ 7, -1, 5, -6, 4} };
        //var x = 0;

        //for (int i = 0;  i <= array.GetUpperBound(0); i++)
        //{
        //for (int j = 0; j <= array.GetUpperBound(1); j++)
        //{
        //for (int k = j+1; k <= array.GetUpperBound(1); k++)
        //{
        //if (array[i, j] > array[i, k])
        //{
        //x = array[i, k];
        //array[i, k] = array[i, j];
        //array[i, j] = x;
        //}
        //}
        //Console.Write(array[i, j] + " ");
        //}
        //Console.WriteLine();
        //}

        

        //(string? Name, string? LastName, string? Login, int LoginLenght, bool HasPet, int Age, string?[] favcolors) User;

        //for (int i = 0; i < 3; i++)
        //{

        //Console.WriteLine("Write your name: ");
        //User.Name = Console.ReadLine();
        //Console.WriteLine("Write your lastname");
        //User.LastName = Console.ReadLine();
        //Console.WriteLine("Write login");
        //User.Login = Console.ReadLine();
        //User.LoginLenght = User.Login.Length;
        //Console.WriteLine("Do you have a pet?");
        //var result = Console.ReadLine();

        //if (result == "Yes")
        //{
        //    User.HasPet = true;
        //}
        //else
        //{
        //    User.HasPet = false;
        //}
        //Console.WriteLine("How old are you");
        //User.Age = Convert.ToInt32(Console.ReadLine());

        //User.favcolors = new string[3];
        //Console.WriteLine("Three yout favorite colors: ");
        //for (int k = 0; k < User.favcolors.Length; k++)
        //{
        //    User.favcolors[k] = Console.ReadLine();
        //}
        //}

        //(string Name, string[] Dishes) User;

        //Console.Write("Write your name: ");
        //User.Name = Console.ReadLine();
        //User.Dishes = new string[5];

        //for (int i = 0; i < User.Dishes.Length; i++)
        //{
        //    Console.Write("Write your five favorite dishes {0}: ", i + 1);
        //    User.Dishes[i] = Console.ReadLine();
        //}
        //}

        //_________________________________

        //static void Main()
        //{
        //    GetArrayFromConsole();
        //}

        //static int[] GetArrayFromConsole()
        //{
            //var result = new int[5];

            //for (int i = 0; i < result.Length; i++)
            //{
            //    Console.Write("Write letter {0}: ", i + 1);
            //    result[i] = int.Parse(Console.ReadLine());
            //}
            //for (int i = 0; i < result.Length; i++)
            //{
            //    for (int j = i + 1; j < result.Length; j++)
            //        if (result[i] > result[i + 1])
            //        {
            //            var x = result[j];
            //            result[j] = result[i];
            //            result[i] = x;
            //        }
            //}
            //for (int i = 0; i < result.Length; i++)
            //{
            //    Console.WriteLine(result[i]);
            //}
            //return result;
        // }
        //________________________________

        static void Main ()
        {
            var (name, age, color) = ("Евгения", 27, " ");

            Console.WriteLine("Ваше имя: {0}", name);
            Console.WriteLine("Ваш возраст: {0}", age);

            Console.Write("Write your name: ");
            name = Console.ReadLine();
            Console.Write("Write your age: ");
            age = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Your name {0}", name);
            Console.WriteLine("Your age {0}", age);
            
            
            var favcolors = new string[3];

            for (int i = 0; i < favcolors.Length; i++)
            {
                favcolors[i] = ShowColor(name, age, color);
            }
            Console.WriteLine("Your favorite colors: ");
            foreach (var i in favcolors)
            {
                Console.WriteLine(i);
            }
            Console.ReadKey();
        }

        static string ShowColor(string name, int age, string color)
        {
            Console.WriteLine("{0}, {1} years \nWrite your favorite color: ", name, age);
            color = Console.ReadLine();

            switch (color)
            {
                    case "red":
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Your color is red");
                        break;

                    case "green":
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Your color is green");
                        break;

                    case "cyan":
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Your color is cyan");
                        break;

                    default:
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Color is yellow");
                        break;
                    
            }
            return color;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
