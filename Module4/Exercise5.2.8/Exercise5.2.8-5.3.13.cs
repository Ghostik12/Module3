using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module4
{
    public class Class1
    {
        //static void Main(string[] ars)
        //{ 
        //    var someName = "Nastya ";
        //    var someAge = 5;
        //    var num = 6;

        //    Console.Write(someName);
        //    Console.WriteLine(someAge);

        //    GetName(out someName);
        //    GetAge(someAge);

        //    Console.WriteLine(someName);
        //    Console.WriteLine(someAge);
        //    int[] sorteddesc = new int[2];
        //    int[] sortedasc = new int[3];
        //    var array = GetArrayFromConsole(ref num);
        //    ShowArray(array, sorteddesc, sortedasc, true);
        //}
        //static int[] GetArrayFromConsole(ref int num)
        //    {
        //        var result = new int[num];

        //        for (int i = 0; i < result.Length; i++)
        //        {
        //            Console.Write("Write letter {0}: ", i + 1);
        //            result[i] = int.Parse(Console.ReadLine());
        //        }
        //        return result;
        //    }

        //static int[] SortArray(in int[] array, out int[] sorteddesc, out int[] sortedasc)
        //    {
        //        sorteddesc = SortArrayDesc(array);
        //        sortedasc = SortArrayAsc(array);
        //        //for (int i = 0; i < sorteddesc.Length; i++)
        //        //{
        //        //    Console.Write(sorteddesc);
        //        //}
        //        //for (int i = 0; i < sortedasc.Length; i++)
        //        //{
        //        //    Console.Write(sortedasc);
        //        //}
        //        return array;
        //    }

        //static int[] SortArrayDesc(in int[] array)
        //{
        //    for (int i = 0; i < array.GetUpperBound(0) + 1; i++)
        //    {
        //        for (int j = i + 1; j < array.GetUpperBound(0) + 1; j++)
        //            if (array[i] > array[j])
        //            {
        //                var x = array[j];
        //                array[j] = array[i];
        //                array[i] = x;
        //            }
        //    }
        //    return array;
        //}

        //static int[] SortArrayAsc(in int[] array)
        //{
        //    for (int i = 0; i < array.GetUpperBound(0) + 1; i++)
        //    {
        //        for (int j = i + 1; j < array.GetUpperBound(0) + 1; j++)
        //            if (array[i] < array[j])
        //            {
        //                var x = array[j];
        //                array[j] = array[i];
        //                array[i] = x;
        //            }
        //    }
        //    return array;
        //}

        //static void ShowArray(int[] array, int[] sorteddesc, int[] sortedasc, bool sort = false)
        //{
        //    var x = array;

        //    if (sort) 
        //    {
        //        x = SortArray(array, out sorteddesc, out sortedasc);
        //    }
        //    foreach (var item in x)
        //    {
        //        Console.Write("{0} ", item);
        //    }

        //}

        //static void GetName(out string name)
        //{
        //    Console.WriteLine("Write name: ");
        //    name = Console.ReadLine();
        //}
        //static void GetAge(double age)
        //{
        //    Console.WriteLine("Write age: ");
        //    age = Convert.ToDouble(Console.ReadLine());
        //}

        static void Main(string[] args)
        {
            //Console.WriteLine("Write: ");
            //var str = Console.ReadLine();
            //Console.WriteLine("Write deep(letter): ");
            var deep = int.Parse(Console.ReadLine());
            var x = byte.Parse(Console.ReadLine());
            Factorial(deep);
            Console.WriteLine(PowerUp(deep, x));
            //Echo(str, deep);
        }

        //static void Echo (string phrase, int deep)
        //{
        //    var modif = phrase;
        //    if (modif.Length > 2 )
        //    {
        //        modif = modif.Remove(0, 2);
        //        Console.BackgroundColor = (ConsoleColor)deep;
        //        Console.WriteLine("..." + modif);
        //    }
        //    //Console.BackgroundColor = (ConsoleColor)deep;
        //    //Console.WriteLine("..." + modif);

        //    if (deep > 1) 
        //    {
        //        Echo(modif, deep - 1);
        //    }

        //    Console.BackgroundColor = ConsoleColor.Black;
        //}

        static decimal Factorial(int x)
        {
            if (x == 0 || x == 1)
            {
                return 1;
            }
            else
            {
                return x * Factorial(x - 1);
            }
        }

        private static int PowerUp(int N, byte pow)
        {
            if (pow == 0)
            {
                return 1;
            }
            else
            {

                if (pow == 1)
                {
                    return N;
                }
                else
                {
                    return N * PowerUp(N, --pow);
                }
            }
        }
    }
}