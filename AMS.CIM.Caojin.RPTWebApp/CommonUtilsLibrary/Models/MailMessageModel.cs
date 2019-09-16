using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilsLibrary.Models
{
    public class MailMessageModel
    {
        public string Subject { get; set; }

        public string Text { get; set; }

        //附件、图片暂不实现
    }
}
