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
            DeleteFileAndDirectory(path);
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