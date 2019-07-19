using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using CommonUtilsLibrary.Utils;
using System.Threading;
using Newtonsoft.Json;
using System.Diagnostics;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Test25.Test();
            //  TestCT.RunCT();
            Console.WriteLine("OK");
            Console.ReadLine();
        }

        static TestMultiTask test = new TestMultiTask();


        static void GetPutIn()
        {
            var text = Console.ReadLine();
            if (text == "")
            {
                test.Active = false;
                Console.WriteLine("stoped");
            }
            else {
                GetPutIn();
            }

        }


        static void WriteSeqValue()
        {
            string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string fullPath = Path.GetDirectoryName(exePath);
            string path = Path.Combine(fullPath, "App", "task1", ".seq.json");
            Console.WriteLine(path);
        }
    }

    class TestJson
    {
        public DateTime dt { get; set; }
     }
}
