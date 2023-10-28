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
            string str = "myFriends.dat";

            var person = new Contact("Seva", 325342525, "fgfgfgdfgf");
            BinaryFormatter bf = new BinaryFormatter();

            using (var fs = new FileStream(str, FileMode.OpenOrCreate))
            {
                bf.Serialize(fs, person);
            }
        }

        [Serializable]
        class Contact
        {

            public string Name { get; set; }
            public long PhoneNumber { get; set; }
            public string Email { get; set; }

            public Contact(string name, long phoneNumber, string email)
            {
                Name = name;
                PhoneNumber = phoneNumber;
                Email = email;
            }
        }
    }
}