using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module7
{
    abstract class Delivery
    {
        private string Address;

        public Delivery(string address)
        {
            Address = Address;
        }

        public void DisplayAddress()
        {
            Console.WriteLine(Address);
        }
    }
}
