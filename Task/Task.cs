using System;
using System.Collections.Generic;

namespace Module3
{

    public class modul3
    {
        
        
        public static void Main(string[] args)
        {

            Console.Write("Enter your Name: ");
            var name = Console.ReadLine();
            Console.Write("Enter your age: ");
            var age = Convert.ToByte(Console.ReadLine());
            Console.Write("When is your birthday: ");
            var date = Console.ReadLine();
            Console.WriteLine("Your name is {0}, age is {1} and date of birth {2}", name, age, date);
            Console.Write("What is your favorite day of week? ");
            var FavoriteDay = (Week)Convert.ToInt16(Console.ReadLine());
            Console.WriteLine("Your favorite day is: {0}", FavoriteDay);

        }
        enum Week 
        {
            Monday = 1,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sanday
        }
    }
}