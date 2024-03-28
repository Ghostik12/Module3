using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDBSSMS;

namespace Test
{
    public class Manager
    {
        private MainConnector connector;
        private DbExecutor dbExecutor;
        private Table userTable;

        public Manager()
        {
            connector = new MainConnector();

            userTable = new Table();
            userTable.Name = "NetworkUser";
            userTable.ImportantField = "Login";
            userTable.Fields.Add("Id");
            userTable.Fields.Add("Login");
            userTable.Fields.Add("Name");
        }

        public void Connect()
        {
            var result = connector.ConnectAsync();

            if (result.Result)
            {
                Console.WriteLine("Подключено успешно!");

                dbExecutor = new DbExecutor(connector);
            }
            else
            {
                Console.WriteLine("Ошибка подключения!");
            }
        }

        public void Disconnect()
        {
            Console.WriteLine("Отключаем БД!");
            connector.DisconnectAsync();
        }

        public void ShowData()
        {
            Console.WriteLine("Получаем данные таблицы " + userTable.Name);

            var data = dbExecutor.SelectAll(userTable.Name);

            Console.WriteLine("Количество строк в " + userTable.Name + ": " + data.Rows.Count);

            Console.WriteLine();
            foreach (DataColumn column in data.Columns)
            {
                Console.Write($"{column.ColumnName}\t");
            }

            Console.WriteLine();

            foreach (DataRow row in data.Rows)
            {

                var cells = row.ItemArray;
                foreach (var cell in cells)
                {
                    Console.Write($"{cell}\t");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        public int DeleteUserByLogin(string value)
        {
            return dbExecutor.DeleteByColumn(userTable.Name, userTable.ImportantField, value);
        }

        public void AddUser(string login, string name)
        {
            dbExecutor.AddingUserProc(name, login);
        }

        public int UpdateUserByLogin(string value, string newvalue)
        {
            return dbExecutor.UpdateByColumn(userTable.Name, userTable.ImportantField, value, userTable.Fields[2], newvalue);
        }
    }
}
