using System;
using System.Collections.Generic;

namespace Module4 { 

    public class Module4
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Write your favorite color");

            var color = Console.ReadLine();
            
            if (color == "red")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Your color red");
            }
            else if (color == "green") 
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Your color green");
            }
            else if (color == "blue") 
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Your color blue");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (color == "yellow")
            {
                Console.BackgroundColor= ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Your color yellow");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Your color cyan");
            }
        }
    }


}
