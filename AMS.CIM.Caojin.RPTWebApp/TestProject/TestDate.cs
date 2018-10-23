using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class TestDate
    {
        public static void Test()
        {
            DateTime dt = DateTime.Now.Date;
            Console.WriteLine(dt.ToString());
        }
    }
}
