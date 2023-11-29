using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module11.Extensions
{
    public static class DirectoryExtensions
    {
        public static string GetSolutionRoot()
        {
            var dir = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            var fullname = Directory.GetParent(dir).FullName;
            var progectRoot = fullname.Substring(0, fullname.Length - 4);
            return Directory.GetParent(progectRoot)?.FullName;
        }
    }
}
