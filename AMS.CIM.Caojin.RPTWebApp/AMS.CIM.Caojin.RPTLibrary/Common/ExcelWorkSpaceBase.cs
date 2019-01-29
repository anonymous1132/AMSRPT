using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace Caojin.Common
{
    public abstract class ExcelWorkSpaceBase
    {
        public ExcelWorkSpaceBase(string filepath,Application app,int sheetno=1)
        {
            _filePath = filepath;
            try
            {
                if (System.IO.File.Exists(_filePath)) { workbook = excelHelper.GetWorkbook(_filePath, app); }
                else { workbook = excelHelper.MakeNewWorkbook(app); }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("RPC"))
                {
                    app = excelHelper.ExcelApp();
                    if (System.IO.File.Exists(_filePath)) { workbook = excelHelper.GetWorkbook(_filePath, app); }
                    else { workbook = excelHelper.MakeNewWorkbook(app); }
                }
            }
            finally
            {
                this.app = app;
                worksheet = excelHelper.GetWorksheet(workbook, sheetno);
            }

        }

        public ExcelWorkSpaceBase(string filepath, Application app, string sheetName)
        {
            _filePath = filepath;
            workbook = excelHelper.GetWorkbook(_filePath, app);
            worksheet = excelHelper.GetWorksheet(workbook, sheetName);
        }

        ~ExcelWorkSpaceBase()
        {
            try
            { Quit(); }
            catch (Exception)
            { }
        }

        protected string _filePath { get; set; }
        protected ExcelHelper excelHelper = new ExcelHelper();
        protected Workbook workbook { get; set; }
        protected Worksheet worksheet { get; set; }
        protected Application app { get; set; }
        public void Save()
        { excelHelper.Save(workbook, _filePath); }

        public abstract void DoWork();

        protected virtual void Quit()
        {
            workbook.Close();
            //app.Quit();
           // excelHelper.QuitExcel(app);
        }
    }
}
