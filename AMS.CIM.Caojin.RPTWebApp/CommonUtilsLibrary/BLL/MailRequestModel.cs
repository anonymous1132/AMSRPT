using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtilsLibrary.Models;

namespace CommonUtilsLibrary.BLL
{
    public class MailRequestModel
    {
        public MailMessageModel Message { get; set; }

        public MailUtilParaModel UtilPara { get; set; }
    }
}
