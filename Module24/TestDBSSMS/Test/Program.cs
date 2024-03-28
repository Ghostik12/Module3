using System.Data;
using TestDBSSMS;


namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var manager = new Manager();

            manager.Connect();
            manager.ShowData();

            Console.WriteLine("Введите логин для добавления:");
            var login = Console.ReadLine();

            Console.WriteLine("Введите имя для добавления:");
            var name = Console.ReadLine();

            manager.AddUser(login, name);

            Console.Write("Введите логие для удаления: ");
            var count = manager.DeleteUserByLogin(Console.ReadLine());
            Console.Write($"Количество удаленных строк: {count}");

            manager.Disconnect();
            
            Console.ReadKey();
        }
    }
}