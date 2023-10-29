using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Module8
{
    class Program
    {
        public static void Main(string[] args)
        {
            string path = @"C:\\Users\\Ghosman\\Desktop\\Новая папка\\";
            GetSize(path);
        }

        public static void GetSize(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] str = dir.GetFiles();
            DirectoryInfo[] dirs = dir.GetDirectories();
            long size = 0;
            try
            {
                if (dir.Exists)
                {
                    foreach (var file in str)
                    {
                        size += file.Length;
                    }
                    foreach (var file in dirs)
                    {
                        GetSize(file.FullName);
                    }
                    Console.WriteLine($"Размер подкаталога в байтах: {size}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}