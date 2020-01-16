using System;
using System.IO;
using System.Linq;

namespace Galilu
{
    class Program
    {
        static int Main(string[] args)
        {
            args = new string[] {".\\test.bc" };
            //args = new string[] { ".\\tests.bc" };
            //args = new string[] { ".\\Galilu.exe" };

            int errCode = 0;
            errCode = Validation(args);
            if (errCode > 0) return errCode;

            string newFile = $"{Path.GetDirectoryName(args[0])}{args[0]}.cs";
            Console.WriteLine($"New file: {newFile}");


            return errCode;
        }

        static int Validation(string[] args)
        {
            int errCode = 0;
            if (args.Length > 1 || args.Length == 0)
            {
                Console.WriteLine("Err: 1, Invalid number of arguments");
                errCode = 1;
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("Err: 2, File doesn't exist");
                errCode = 2;
            }
            else if (Path.GetExtension(args[0]) != ".bc")
            {
                Console.WriteLine("Err 3, File type not compatible");
                errCode = 3;
            }
            
            return errCode;
        }
    }
}
