using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.CIM.Caojin.RPTLibrary;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt024MainViewModel
    {
        public ReqRpt024MainViewModel()
        {
        }

        public static Dictionary<string, string> Departments { get; set; } = new Dictionary<string, string>()
        {
            { "BadWafer", "WCOUT_PRO,WCOUT_ENG"},
            { "C:CMP", "SCRAP_CMP"},
            { "D:DIF","SCRAP_DIF"},
            { "E:ETH","SCRAP_ETH"},
            { "H:PIE","SCRAP_PIE"},
            { "I:WAT","SCRAP_WAT"},
            { "L:PFA","SCRAP_PFA"},
            { "M:DCM", "SCRAP_DCM"},
            { "P:PHO","SCRAP_PHO" },
            { "Q:QRA","SCRAP_QRA"},
            { "R:WRC", "SCRAP_WRC"},
            { "T:TF(PVD+CVD)","SCRAP_TF"},
            { "V:PRD","SCRAP_PRD"},
            { "W:WET","SCRAP_WET"},
            { "WD:WET&DIF","SCRAP_WD"}
        };

        public Dictionary<string, string> DepartmentOptions { get { return Departments; } }

        //public void GetData()
        //{
        //    Departments.Add("BadWafer", "WCOUT_PRO,WCOUT_ENG");
        //    Departments.Add("C:CMP", "SCRAP_CMP");
        //    Departments.Add("D:DIF","SCRAP_DIF");
        //    Departments.Add("E:ETH","SCRAP_ETH");
        //    Departments.Add("H:PIE","SCRAP_PIE");
        //    Departments.Add("I:WAT","SCRAP_WAT");
        //    Departments.Add("L:PFA","SCRAP_PFA");
        //    Departments.Add("M:DCM", "SCRAP_DCM");
        //    Departments.Add("P:PHO","SCRAP_PHO");
        //    Departments.Add("Q:QRA","SCRAP_QRA");
        //    Departments.Add("R:WRC", "SCRAP_WRC");
        //    Departments.Add("T:TF(PVD+CVD)","SCRAP_TF");
        //    Departments.Add("V:PRD","SCRAP_PRD");
        //    Departments.Add("W:WET","SCRAP_WET");
        //    Departments.Add("WD:WET&DIF","SCRAP_WD");
        //}


    }
}