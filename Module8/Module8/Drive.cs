using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module8
{
    class Drive
    {
        public string Name { get; }
        public long Capacity { get; }
        public long FreeSpace { get; }

        public Drive(string name, long capacity, long freeSpace)
        {
            Name = name;
            Capacity = capacity;
            FreeSpace = freeSpace;
        }

        Dictionary<string, Folder> Folders = new Dictionary<string, Folder>();

        public void CreateFolder(string name)
        {
            Folders.Add(name, new Folder());
        }
    }
}
