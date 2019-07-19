using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.Net.NetworkInformation;

namespace TestProject
{
    public class TestAD
    {
        public static void Test()
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
             string hostName = ipGlobalProperties.HostName;
             string domainName = ipGlobalProperties.DomainName;
            Console.WriteLine(domainName);
        }
    }
}
