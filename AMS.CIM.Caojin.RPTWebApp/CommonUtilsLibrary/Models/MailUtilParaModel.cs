using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtilsLibrary.Models
{
    public class MailUtilParaModel
    {
        private string _mailAddress;
        public string MailAddress
        {
            get
            {
                if (string.IsNullOrEmpty(_mailAddress)) throw new MailUtilException("MailAddress Invalid Value");
                return _mailAddress;
            }
            set { _mailAddress = value; }
        }

        private string _serverHost;
        public string ServerHost
        {
            get
            {
                if (string.IsNullOrEmpty(_serverHost)) throw new MailUtilException("ServerHost Invalid Value");
                return _serverHost;
            }
            set
            {
                _serverHost = value;
            }
        }

        private Int32 _serverPort=465;
        public Int32 ServerPort
        {
            get
            {
                if (_serverPort < 0 || _serverPort > 65535) throw new MailUtilException("ServerPort Invalid Value");
                return _serverPort;
            }
            set
            {
                _serverPort = value;
            }
        }

        private string _logonUserName;
        public string LogonUserName
        {
            get
            {
                if (string.IsNullOrEmpty(_logonUserName)) throw new MailUtilException("LogonUserName Invalid Value");
                return _logonUserName;
            }
            set
            {
                _logonUserName = value;
            }
        }

        public string Password { get; set; }

        //private List<string> _revicerAddressList=new List<string>();
        //public List<string> ReciverAddressList
        //{
        //    get
        //    {
        //      //  if (_revicerAddressList.Count < 1) throw new MailUtilException("ReciverAddressList Invalid Value");
        //        return _revicerAddressList;
        //    }
        //    set
        //    {
        //        _revicerAddressList = value;
        //    }
        //}

        private string _reciverAddresses;
        public string ReciverAddresses
        {
            get
            {
                if (string.IsNullOrEmpty(_reciverAddresses)) throw new Exception("ReciverAddresses Invalid Value");
                return _reciverAddresses;
            }
            set
            {
                _reciverAddresses = value;
            }
        }

        /// <summary>
        /// 0:ssl
        /// 1:tls
        /// 2:auto
        /// -1:none
        /// </summary>
        public int SSLType { get; set; } = 0;
    }

}
