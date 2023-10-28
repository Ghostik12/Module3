using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module8
{
    class Program
    {
        public static void Main(string[] args)
        {
            string str = "C:\\Users\\Ghosman\\Desktop\\BinaryFile.bin";
            WriteValues(str);
            ReadValues(str);
        }

        static void ReadValues(string str)
        {
            string st;
            if (File.Exists(str)) 
            { 
                using (BinaryReader reader = new BinaryReader(File.Open(str, FileMode.Open)))
                {
                    st = reader.ReadString();
                    Console.WriteLine(st);
                }
            }
        }

        static void WriteValues(string str)
        {
            string st;
            if (File.Exists(str))
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open(str, FileMode.Create)))
                {
                    writer.Write($"Файл измене {DateTime.Now} на компьютере {Environment.OSVersion}");
                }
            }
        }
    }
}