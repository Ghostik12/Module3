using System;
using System.Collections.Generic;
using System.Drawing;
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
            long dirSize = 0;
            dirSize = GetSize(path, ref dirSize);
            if (dirSize == 0) 
            {
                Console.WriteLine($"Каталог {path} пуст");
            }
            else
            {
                Console.WriteLine($"Размер каталога {path} составляет {dirSize} Байт");
            }
        }

        public static long GetSize(string path, ref long size)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] str = dir.GetFiles();
            DirectoryInfo[] dirs = dir.GetDirectories();
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
                        GetSize(file.FullName, ref size);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return size;
        }
    }
}