using System;
using System.Collections.Generic;

namespace Module3
{

    public class modul3
    {
        static void Main(string[] args)
        {
            string name = "Seva";
            ushort age = 26;
            bool pet = true;
            double footsize = 25.6;

            Console.WriteLine("My name is "+ name);
            Console.WriteLine("My age " + age);
            Console.WriteLine("Do I have a pet? " + pet);
            Console.WriteLine("My shoe size " + footsize);

            Console.WriteLine("IntMin {0} ", int.MinValue);
            Console.WriteLine("IntMax {0} ", int.MaxValue);
        }
    }
}