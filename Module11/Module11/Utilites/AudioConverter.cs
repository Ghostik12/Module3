using FFMpegCore;
using Module11.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module11.Utilites
{
    public static class AudioConverter
    {
        public static void TryConvert (string inputFIle, string outputFIle)
        {
            GlobalFFOptions.Configure(options => options.BinaryFolder = Path.Combine(DirectoryExtensions.GetSolutionRoot(), "FFmpeg-win64", "bin"));

            FFMpegArguments
                .FromFileInput(inputFIle)
                .OutputToFile(outputFIle, true, options => options
                .WithFastStart())
                .ProcessSynchronously();
        }

        private static string GetSolutionRoot()
        {
            var dir = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            var fullname = Directory.GetParent(dir).FullName;
            var progectRoot = fullname.Substring(0, fullname.Length - 4);
            return Directory.GetParent(progectRoot)?.FullName;
        }
    }
}
