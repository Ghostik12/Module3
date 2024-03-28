using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Table
    {
        public string Name {  get; set; }
        public List<string> Fields { get; set; }
        public string ImportantField { get; set; }
        public Table()
        {
            Fields = new List<string>();
        }
    }
}
