using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtilsLibrary.Utils;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string test = "";
            List<string> list = test.Split('|').ToList();
            Console.WriteLine(list.Count());
            Console.ReadKey();
        }


    }
}
