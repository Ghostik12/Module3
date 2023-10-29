using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Module8
{
    class Program
    {
        public static void Main(string[] args)
        {
            string path = @"C:\\Users\\Ghosman\\Desktop\\Новая папка\\";
            long dirSize = 0;
            long dirSpace = 0;
            long dirSizeNow = 0;
            dirSize = GetSize(path, ref dirSize);
            Console.WriteLine($"Исходный размер папки: {dirSize} байт");
            DeleteFileAndDirectory(path);
            dirSpace = dirSize - GetSize(path, ref dirSpace);
            Console.WriteLine($"Освобожденно: {dirSpace}  байт");
            dirSizeNow = GetSize(path, ref dirSizeNow);
            Console.WriteLine($"Текущий размер папки: {dirSizeNow} байт");
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
        public static void DeleteFileAndDirectory(string path)
        {

            DirectoryInfo dir = new DirectoryInfo(path);
            DirectoryInfo[] dirs = dir.GetDirectories();
            FileInfo[] fileInfo = dir.GetFiles();
            try
            {
                if (Directory.Exists(path))
                {
                    foreach (var file1 in fileInfo)
                    {
                        if ((DateTime.Now - file1.LastWriteTime) > TimeSpan.FromMinutes(2))
                            file1.Delete();
                    }
                    foreach (var file in dirs)
                    {
                        DeleteFileAndDirectory(file.FullName);
                        if ((DateTime.Now - file.LastWriteTime) > TimeSpan.FromMinutes(2))
                            file.Delete(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}