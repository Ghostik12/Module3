﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module7
{
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
}