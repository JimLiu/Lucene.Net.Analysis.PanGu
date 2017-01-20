using Dos.PanGu.Dict;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanGu.DictionaryConverter
{

    class Program
    {

        static void Main(string[] args)
        {
            var currentPath = Environment.CurrentDirectory;
            var resDir = new DirectoryInfo(Path.Combine(currentPath, "Resources"));
            var dict = new WordDictionary();

            dict.Load(Path.Combine(resDir.FullName, "Dict.dct"));
            dict.

            Console.WriteLine("Press any key to continue...");
        }

    }

}