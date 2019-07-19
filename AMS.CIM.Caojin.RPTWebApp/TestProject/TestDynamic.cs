using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestProject
{
    public class TestDynamic
    {
        public static void Test()
        {
            string path = "App\\db2Config.json";
            string text = System.IO.File.ReadAllText(path);
            //dynamic obj = JToken.Parse(text) as dynamic;
            dynamic obj = JsonConvert.DeserializeObject(text);
            Console.WriteLine(obj.UID);
            Console.ReadLine();
        }
    }
}
