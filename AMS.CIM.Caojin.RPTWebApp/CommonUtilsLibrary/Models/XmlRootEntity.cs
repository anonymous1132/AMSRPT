using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilsLibrary.Models
{
    public class XmlRootEntity
    {
        public string XmlVersion { get; set; } = "1.0";

        public string XmlEncoding { get; set; } = "utf-16";

        public string RootName { get; set; }


    }
}
