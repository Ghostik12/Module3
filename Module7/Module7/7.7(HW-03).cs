using System;
using System.Collections.Generic;
using System.Data;
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

    class HomeDelivery : Delivery
    {
        private string Name;
        private DateTime DateDelivery;

        public HomeDelivery (string address, string name, DateTime datedelivery) : base(address) 
        { 
            Name = name;
            DateDelivery = datedelivery;
        }
    }

    class PickPointDelivery : Delivery
    {
        private string Name;
        private DateTime DateDelivery;
        public PickPointDelivery(string address, string name, DateTime datedelivery) : base(address)
        {
            Name = name;
            DateDelivery = datedelivery;
        }
    }

    class ShopDelivery : Delivery
    {
        private string Name;
        private DateTime DateDelivery;
        public ShopDelivery(string address, string name, DateTime datedelivery) : base(address)
        {
            Name = name;
            DateDelivery = datedelivery;
        }
    }

    class BasketOrder
    {
        public List<string> Products;
    }

    class Product
    {
        public string Name;
        public int Count;
    }

    class Order<TDelivery> where TDelivery : Delivery
    {
        public TDelivery Delivery;

        public int Number;

        public string Description;

        public Product Product;

        public BasketOrder BasketOrder { get; set; }  
    }
}
