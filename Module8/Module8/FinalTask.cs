using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FinalTask
{
    class FinalTask
    {
        public static void Main(string[] args)
        {
            string path = @"C:\\Users\\Ghosman\\Desktop\\Students";
            string path2 = @"C:\\Users\\Ghosman\\Desktop\\Students.dat";
            CreateDirOrNo(path);
            ReadValues(path2, path);
        }

        private static FileInfo fileInfo;

        public static void CreateDirOrNo(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            try
            {
                if (!dir.Exists)
                {
                    dir.Create();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void WriteToFile(Student students, string path3)
        {
            if (!File.Exists($"{path3}{students.Group}.txt"))
            {
                File.Create($"{path3}{students.Group}.txt");
                using (StreamWriter sw = File.CreateText(path3))
                {
                    sw.WriteLine(students);
                }
            }
        }

        static void ReadValues(string path2, string path3)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                if (File.Exists(path2))
                {
                    using (FileStream fs = new FileStream(path2, FileMode.OpenOrCreate))
                    {
                        Student[] person = (Student[])formatter.Deserialize(fs);

                        foreach (Student student in person)
                        {
                            fileInfo = new FileInfo($"{path3}{student.Group}.txt");
                            WriteToFile(student, path3);
                        }
                    }
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Student(string name, string group, DateTime dateOfBirth)
        {
            Name = name;
            Group = group;
            DateOfBirth = dateOfBirth;
        }
    }
}