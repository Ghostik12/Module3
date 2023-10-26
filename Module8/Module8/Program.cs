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
            string tempFile = Path.GetTempFileName();//используем генерацию имени файла.
            var fileInfo = new FileInfo(tempFile);//Создаем объект класса FileInfo.

            using (StreamWriter sw = fileInfo.CreateText())
            {
                sw.WriteLine("Seva");
                sw.WriteLine("Dima");
                sw.WriteLine("Nastya");
                sw.WriteLine(DateTime.Now);
            }

            using (StreamReader sr = fileInfo.OpenText())
            {
                string line = "";
                while((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }

            try
            {
                string tempFile2 = Path.GetTempFileName();
                var fileInfo2 = new FileInfo(tempFile2);

                fileInfo2.Delete(); //Убедимся, что файл назначения точно отсутствует

                fileInfo.CopyTo(tempFile2);//Копируем информацию
                Console.WriteLine($"{fileInfo} Copy to file {tempFile2}");

                fileInfo.Delete();//Удаляем ранее созданный файл.
                Console.WriteLine($"File {fileInfo} delete");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message );
            }
        }
    }
}