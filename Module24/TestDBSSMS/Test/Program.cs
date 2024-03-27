using TestDBSSMS;


namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var con = new MainConnector();

            var result = con.ConnectAsync();

            if (result.Result)
            {
                Console.WriteLine("Подключено успешно!");
            }
            else
            {
                Console.WriteLine("Ошибка подключения!");
            }

            Console.ReadKey();

        }
    }
}