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
            //GetCatalogs();
            //try
            //{
            //    DirectoryInfo dirInfo = new DirectoryInfo(@"C:\\" /* Или С:\\ для Windows */ );
            //    if (dirInfo.Exists)
            //    {
            //        Console.WriteLine(dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length);
            //    }

            //    DirectoryInfo difnew = new DirectoryInfo (@"C:/newDirectory");
            //    if (!difnew.Exists)
            //    {
            //        difnew.Create();
            //    }

            //    Console.WriteLine(dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length);

            //    difnew.Delete(true);

            //    Console.WriteLine(dirInfo.GetDirectories().Length + dirInfo.GetFiles().Length);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //DirectoryInfo dir1 = new DirectoryInfo(@"C:/newDirectory");
            //string dirs = "C:/newDirectoryNew";

            //if (dir1.Exists && !Directory.Exists(dirs))
            //{
            //    dir1.MoveTo(dirs);
            //}

            //try
            //{
            //    DirectoryInfo dirInfo = new DirectoryInfo(@"C:/User/Ghostman/Desktop/testFolder");
            //    string dirStr = "C:/User/Ghostman/Desktop/Basket";
            //    if (!dirInfo.Exists)
            //    {
            //        dirInfo.Create();
            //    }
            //    if(dirInfo.Exists && !Directory.Exists(dirStr))
            //    {
            //        dirInfo.MoveTo(dirStr);
            //    }
            //}
            //catch (Exception e) 
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

        //public static void GetCatalogs()
        //{
        //    string dirName = @"C:\\";

        //    if (Directory.Exists(dirName))
        //    {
        //        Console.WriteLine("Papki");
        //        string[] dirs = Directory.GetDirectories(dirName);

        //        foreach (string dir in dirs)
        //        {
        //            Console.WriteLine(dir);
        //        }

        //        Console.WriteLine();
        //        Console.WriteLine("Files");
        //        string[] fils = Directory.GetFiles(dirName);

        //        foreach(string fil in fils)
        //        {
        //            Console.WriteLine(fil);
        //        }
        //    }
        //}
    }
}