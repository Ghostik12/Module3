using System;

namespace Module9
{
    class Program
    {
        static void Main(string[] args)
        {
            Exception ex = new Exception("Произошло исключение");
            ex.Data.Add("Дата создания исключения", DateTime.Now);
            ex.HelpLink = "https://yandex.ru";

            try
            {
                throw new RankException("Fail");
            }
            catch (Exception exception)
            { 
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.GetType());
            }
            finally
            {
                Console.WriteLine("Сработал finally");
            }
            Sum sum = Minus;
            sum += Plus;

            sum -= Plus;
            sum(9, 4);
            //Strin str = Str;
            //str.Invoke();
            //SumFiguer SM = Figuer;
            //int sm = SM(10, 15);
            //Console.WriteLine(sm);
            //CheckLenght cL = Check;
            //bool chec = cL("Check");
            //Console.WriteLine(chec);

            Action<string> action = Str;
            action("srt");
            Func<int, int, int> func = Figuer; 
            int result = func(10, 15);
            Console.WriteLine(result);
            Predicate<string> predicate = Check;
            bool chec = Check("Mamba");
            Console.WriteLine(chec);

            ShowMassege sh = (string massege) => Console.WriteLine($"{massege}");
            sh.Invoke("Hello world");

            RandomNumber rn = () => new Random().Next(0, 100);
            int random = rn.Invoke();
            Console.WriteLine(random);

            CarDelegate carDelegate = LexusHead;
            ChildInfo ch = GetParentInfo;
            ch.Invoke(new Child());
        }

        delegate int RandomNumber();
        delegate void ShowMassege(string massege);
        public delegate void Sum(int a, int b);
        //public delegate void Strin();
        //public delegate int SumFiguer(int a, int b);
        //public delegate bool CheckLenght(string s);

        public delegate Car CarDelegate();
        delegate void ChildInfo(Child childInfo);

        static void Minus (int a, int b)
        {
            Console.WriteLine(a - b);
        }

        static void Plus(int a, int b)
        {
            Console.WriteLine(a + b);
        }

        static void Str(string str)
        {
            Console.WriteLine(str);
        }

        private static int Figuer(int a, int b) 
        {
            return a + b;
        }

        static bool Check(string check)
        {
            if (check.Length < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static Car CarHead()
        {
            return null;
        }
        public static Lexus LexusHead()
        {
            return null;
        }

        public static Parent ParentHead()
        {
            return null;
        }

        public static Child ChildHead()
        {
            return null;
        }

        public static void GetParentInfo(Parent p)
        {
            Console.WriteLine(p.GetType());
        }
    }

    class Car
    {

    }

    class Lexus : Car
    {

    }

    class Parent
    {

    }

    class Child : Parent
    {

    }
}