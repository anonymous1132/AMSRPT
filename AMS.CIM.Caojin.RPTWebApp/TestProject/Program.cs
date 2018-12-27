using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> testList = new List<string>();

            testList.ForEach(f=>Console.WriteLine(f));

            Console.ReadKey();
        }


        public static bool IsNatural_Number(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9\-_]+$");
            return reg1.IsMatch(str);
        }

    }
}
