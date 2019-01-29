using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;
using Microsoft.Office.Interop.Excel;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018Chart2ExcelWorkSpace : ExcelWorkSpaceBase
    {
        public ReqRpt018Chart2ExcelWorkSpace(string filepath, int sheetno = 1) : base(filepath, ExcelApp.App, sheetno)
        {

        }

        public override void DoWork()
        {
            if (!PostModel.Entities.Any()) { throw new Exception("没有数据！"); }
            worksheet.Cells[1, 1] = PostModel.EqpID;
            worksheet.Cells[2, 1] = "UPm(%)";
            worksheet.Cells[3, 1] = "UUm(%)";
            worksheet.Cells[4, 1] = "SD(%)";
            worksheet.Cells[5, 1] = "UD(%)";
            worksheet.Cells[6, 1] = "EN(%)";
            worksheet.Cells[7, 1] = "UPm Target";
            int totalColumn = PostModel.Entities.Count;
            for (int i = 0; i < totalColumn; i++) {
                worksheet.Cells[1, i+2] = PostModel.Entities[i].Date;
                worksheet.Cells[2, i + 2] = PostModel.Entities[i].UPm;
                worksheet.Cells[3, i + 2] = PostModel.Entities[i].UUm;
                worksheet.Cells[4, i + 2] = PostModel.Entities[i].SD;
                worksheet.Cells[5, i + 2] = PostModel.Entities[i].UD;
                worksheet.Cells[6, i + 2] = PostModel.Entities[i].EN;
                worksheet.Cells[7, i + 2] = 0.9;
            }
            Range range = worksheet.Range[worksheet.Cells[2, 2], worksheet.Cells[7, totalColumn+1]];
            //设置百分比
            excelHelper.SetRangeValueStyleNumber(range, "0.00%");

            var chart = excelHelper.AddShapeLevelChart(worksheet, range,700,350, XlRowCol.xlRows);
            excelHelper.SetChartStyle(chart, XlChartType.xlColumnClustered);
            excelHelper.SetChartXYTitle(chart, "", "", PostModel.EqpID);
            //设置系列名称
            for (int i = 1; i <= 6; i++)
            {
                excelHelper.SetChartSeriesName(chart, worksheet.Cells[1+i, 1], i);
            }
            //设置x轴坐标显示value
            excelHelper.SetChartSeriesCollectionXValues(chart, worksheet.Range[worksheet.Cells[1, 2], worksheet.Cells[1, totalColumn+ 1]]);

            //设置系列1，2，6使用y副坐标,并改成折线图
            foreach(var i in new int[] { 1,2,6} )
            {
                excelHelper.SetChartSeriesCollectionY(chart, i);
                excelHelper.SetChartSeriesCollectionChartType(chart, i, XlChartType.xlLine);
            }
            //设置y轴最大最小值，数据格式
            excelHelper.SetChartYMaxMinValue(chart,1,0,XlAxisGroup.xlPrimary);
            excelHelper.SetChartYMaxMinValue(chart, 1, 0, XlAxisGroup.xlSecondary);
            excelHelper.SetChartYThickLabelNumberFormat(chart,XlAxisGroup.xlPrimary,"0.00%");
            excelHelper.SetChartYThickLabelNumberFormat(chart, XlAxisGroup.xlSecondary, "0.00%");
            //Upm天蓝色
            excelHelper.SetChartSeriesCollectionBorderColor(chart, 1, 33);
            //UUm橘黄色
            excelHelper.SetChartSeriesCollectionBorderColor(chart, 2, 44);
            //SD粉红色
            excelHelper.SetChartSeriesCollectionFillColor(chart,3, 12957183);
            //UD红色
            excelHelper.SetChartSeriesCollectionFillColor(chart, 4,255);
            //EN蓝色
            excelHelper.SetChartSeriesCollectionFillColor(chart, 5, 16711680);
            //UPm Target红色虚线
            excelHelper.SetChartSeriesCollectionBorderColor(chart, 6, 3);
            excelHelper.SetChartSeriesCollectionBorderLineStyle(chart, 6, XlLineStyle.xlDash);

            //设置标签可见
            excelHelper.SetChartDataLabels(chart, true);

            //设置边框线条
            Range allRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[7, totalColumn + 1]];
            excelHelper.SetRangeBodersThickness(allRange,XlBorderWeight.xlThin);
            excelHelper.SetFontHVCenter(allRange);

            //设置背景色为黄色
            excelHelper.SetRangeBackground(worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, totalColumn + 1]],6);
            excelHelper.SetRangeBackground(worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[7, 1]], 6);
            Save();
            Quit();
        }

        public ReqRpt018Chart2PostModel PostModel { get; set; }
    }
}