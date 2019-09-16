using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilsLibrary.Models
{
    [Serializable]
    public class MailUtilException:ApplicationException
    {
        private string error;
        private Exception innerException;

        public MailUtilException()
        { }

        public MailUtilException(string msg) : base(msg)
        {
            error = msg;
        }

        public MailUtilException(string msg, Exception innerException) : base(msg)
        {
            this.innerException = innerException;
            error = msg;
        }

        public string GetError()
        {
            return error;
        }
    }
}
