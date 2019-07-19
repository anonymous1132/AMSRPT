using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class TestMultiTask
    {
        public TestMultiTask()
        {
     
        }

        public void Test()
        {
            Console.WriteLine("start");
            while (Active)
            {
                Console.WriteLine(++i);
                System.Threading.Thread.Sleep(2000);
            }
            Console.WriteLine("end");
        }

        public bool Active = true;

        int i = 0;
    }
}
