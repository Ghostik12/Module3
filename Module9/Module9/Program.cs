using System;

namespace Module9
{
    class Program
    {
        static void Main(string[] args)
        {
            NumberReader reader = new NumberReader();
            reader.NumberEnteredEvent += Sort;

            try
            {
                //reader.Read();
                while (true)
                {
                    reader.Read();
                    Exception[] exception = new Exception[5];
                    exception[0] = new ArgumentException();
                    exception[1] = new FormatException();
                    exception[2] = new RankException();
                    exception[3] = new TimeoutException();
                    exception[4] = new MyException();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {

            }
            catch (FormatException)
            {
                Console.WriteLine("Некорректное число");
            }
        }
        
        static void Sort(int number)
        {
            List<string> list = new List<string>();
            list.Add("Smirnov");
            list.Add("Ivanov");
            list.Add("Luzhaev");
            list.Add("Baranov");
            list.Add("Luzhaeva");

            Console.WriteLine("Список до сортировки");
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();

            if (number == 1)
            {
                list.Sort();
                foreach (string str in list)
                {
                    Console.WriteLine(str);
                }
            }
            else 
            {
                    var ln = list.OrderByDescending(x => x);
                    foreach (string str in ln)
                    { 
                        Console.WriteLine(str); 
                    }
            }
        }
    }

    public class MyException : Exception 
    { 
        public MyException() : base () 
        {

        }
    }

    class NumberReader
    {
        public delegate void NumberEnteredDelegate(int number);
        public event NumberEnteredDelegate NumberEnteredEvent;
        public void Read()
        {
            Console.WriteLine();
            Console.WriteLine("Need write 1 or 2");

            int number = Convert.ToInt32 (Console.ReadLine());

            if (number != 1 && number != 2) throw new FormatException();

            NumberEntered(number);
        }

        protected virtual void NumberEntered(int number)
        {
            NumberEnteredEvent?.Invoke(number);
        }
    }
}