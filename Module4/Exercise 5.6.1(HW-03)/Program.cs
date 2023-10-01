using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace Exercise5
{
    class Program
    {
        static void Main(string[] args)
        {
            Korteg();
        }

        static (string Name, string lastName, int Age) Korteg() 
        {
            (string Name, string lastName, int Age) korte;
            int countPets; 
            int favColors;
            string age;
            int intage;
            string str;

            do
            {
                Console.WriteLine("Write your name: ");
                age = Console.ReadLine();
            } while (CheckStr(age, out str));
            korte.Name = str;

            do
            {
                Console.WriteLine("Write your last name: ");
                age = Console.ReadLine();
            } while (CheckStr(age, out str));
            korte.lastName = str;
            
            do
            {
                Console.WriteLine("Write your age: ");
                age = Console.ReadLine();
            } while (CheckNum(age, out intage));

            korte.Age = intage;
            Console.WriteLine("You have a pet? (yes or no)");

            var has = Console.ReadLine();
            string[] petsName = new string[0];
            string[] favcolor = new string[0];

            if (has == "yes") 
            {
                do
                {

                    Console.WriteLine("How many pets do you have?");
                    age = Console.ReadLine();
                } while (CheckNum(age, out intage));
                countPets = intage;
                PetsName(countPets, out petsName);
            }
            do 
            {
                Console.WriteLine("How many colors do you like?");
                age = Console.ReadLine();
            } while(CheckNum(age, out intage));
            favColors = intage;

            FavColors(favColors, out favcolor);
            Show(korte.Name, korte.lastName, korte.Age, petsName, favcolor);
            return korte;
        }

        static string[] PetsName(in int countPets, out string[] petsName) 
        {
            petsName = new string[countPets];
            for (int i = 0; i < countPets; i++)
            {
                Console.WriteLine("Write name pets: ");
                petsName[i] = Console.ReadLine();
            }
            return petsName;
        }

        static string[] FavColors (in int favColors, out string[] favcolor)
        {
            favcolor = new string[favColors];
            for (int i = 0; i < favcolor.Length; i++)
            {
                Console.WriteLine("Write your favorite color(number){0}", i + 1);
                favcolor[i] = Console.ReadLine();
            }
            return favcolor;
        }

        static bool CheckStr(string str, out string corrnumber)
        {
            bool check = true;
            if (check = int.TryParse(str, out int intnum))
            {
                corrnumber = "";
                return true;
            }
            corrnumber = str;
            return false;
        }

        static bool CheckNum(string number, out int corrnumber)
        {
            if (int.TryParse(number, out int intnum))
            {
                if (intnum > 0)
                {
                    corrnumber = intnum;
                    return false;
                }
            }
            corrnumber = 0;
            return true;
        }
        
        static void Show(in string Name, string lastname, int age, string[] petsName, string[] favColors)
        {
            Console.WriteLine("\n" + "Your name and lastname: {0}", Name + " " + lastname);
            Console.WriteLine("Your age: {0}", age);
            Console.Write("Name your pet(s): ");
            for (int i = 0; i < petsName.Length; i++)
            {
                Console.Write(petsName[i] + ", ");
            }
            Console.WriteLine();
            Console.Write("Your favorite color: ");
            for (int i = 0;i < favColors.Length; i++)
            {
                Console.Write(favColors[i] + ", ");
            }
            Console.WriteLine();
        }
    }
}
