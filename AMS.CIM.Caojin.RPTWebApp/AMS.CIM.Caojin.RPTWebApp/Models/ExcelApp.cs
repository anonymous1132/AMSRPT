using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using Caojin.Common;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public static class ExcelApp
    {
        private static Application app;
        public static Application App {
            get
            {
                if (app == null) { app = new ExcelHelper().ExcelApp(); }
                return app;
            }
            set
            {
                try { app.Quit(); } catch (Exception) { }
                app = value;
            }
        }
    }
}