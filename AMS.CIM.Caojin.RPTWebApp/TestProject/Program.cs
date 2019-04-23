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
            string text = DateTime.Now.ToString("MMM", new System.Globalization.CultureInfo("en-us"));
            Console.WriteLine(text);
            Console.ReadKey();
        }


    }
}
