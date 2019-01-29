using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Caojin.Common;
using Microsoft.Office.Interop.Excel;

namespace AMS.CIM.Caojin.RPTWebApp.Models
{
    public class ReqRpt018Chart1ExcelWorkSpace : ExcelWorkSpaceBase
    {
        public ReqRpt018Chart1ExcelWorkSpace(string filepath, int sheetno = 1) : base(filepath, ExcelApp.App, sheetno)
        {

        }


        public override void DoWork()
        {
            if (!PostModel.Entities.Any()) { throw new Exception("没有数据！"); }
            int currentRow = 1;
            worksheet.Cells[currentRow,1]= PostModel.Date;
            worksheet.Cells[currentRow, 2] = "PR";
            worksheet.Cells[currentRow, 3] = "SBY";
            worksheet.Cells[currentRow, 4] = "EN";
            worksheet.Cells[currentRow, 5] = "SD";
            worksheet.Cells[currentRow, 6] = "UD";
            worksheet.Cells[currentRow, 7] = "NST";
            worksheet.Cells[currentRow, 8] = "UPm(%)";
            worksheet.Cells[currentRow, 9] = "UUm(%)";
            worksheet.Cells[currentRow, 10] = "UPm Target";
            //currentRow++;
            for (int i = 0; i < PostModel.Entities.Count; i++)
            {
                currentRow++;
                worksheet.Cells[currentRow, 1] = PostModel.Entities[i].EqpID;
                worksheet.Cells[currentRow, 2] = PostModel.Entities[i].PR;
                worksheet.Cells[currentRow, 3] = PostModel.Entities[i].SB;
                worksheet.Cells[currentRow, 4] = PostModel.Entities[i].EN;
                worksheet.Cells[currentRow, 5] = PostModel.Entities[i].SD;
                worksheet.Cells[currentRow, 6] = PostModel.Entities[i].UD;
                worksheet.Cells[currentRow, 7] = PostModel.Entities[i].NS;
                worksheet.Cells[currentRow, 8] = PostModel.Entities[i].UP;
                worksheet.Cells[currentRow, 9] = PostModel.Entities[i].UU;
                worksheet.Cells[currentRow, 10] = 0.90;
            }
            Range range = worksheet.Range[worksheet.Cells[2,2], worksheet.Cells[currentRow,10]];
            //设置百分比
            excelHelper.SetRangeValueStyleNumber(range, "0.00%");

            var chart = excelHelper.AddShapeLevelChart(worksheet, range,700,350,XlRowCol.xlColumns);
            excelHelper.SetChartStyle(chart, XlChartType.xlColumnStacked100);
            excelHelper.SetChartXYTitle(chart,"","",PostModel.Date);
            //设置系列名称
            for (int i = 1; i <= 9; i++)
            {
                excelHelper.SetChartSeriesName(chart, worksheet.Cells[1, i + 1],i);
            }
            //设置x轴坐标显示value
            excelHelper.SetChartSeriesCollectionXValues(chart,worksheet.Range[worksheet.Cells[2,1],worksheet.Cells[currentRow,1]]);
            
            //设置系列7，8，9使用y副坐标,并改成折线图
            for (int i = 7; i <= 9; i++)
            {
                excelHelper.SetChartSeriesCollectionY(chart, i);
                excelHelper.SetChartSeriesCollectionChartType(chart,i,XlChartType.xlLine);
            }
            //设置y轴最大最小值，数据格式
            excelHelper.SetChartYMaxMinValue(chart, 1, 0, XlAxisGroup.xlPrimary);
            excelHelper.SetChartYMaxMinValue(chart, 1, 0, XlAxisGroup.xlSecondary);
            excelHelper.SetChartYThickLabelNumberFormat(chart, XlAxisGroup.xlPrimary, "0.00%");
            excelHelper.SetChartYThickLabelNumberFormat(chart, XlAxisGroup.xlSecondary, "0.00%");
            //PR绿色
            excelHelper.SetChartSeriesCollectionFillColor(chart,1,3329330);
            //SB黄色
            excelHelper.SetChartSeriesCollectionFillColor(chart, 2, 65535);
            //EN蓝色
            excelHelper.SetChartSeriesCollectionFillColor(chart, 3,16711680);
            //SD粉红色
            excelHelper.SetChartSeriesCollectionFillColor(chart,4,12957183);
            //UD红色
            excelHelper.SetChartSeriesCollectionFillColor(chart, 5, 255);
            //NS灰色
            excelHelper.SetChartSeriesCollectionFillColor(chart, 6, 13421772);
            //UPm天蓝色
            excelHelper.SetChartSeriesCollectionBorderColor(chart,7, 33);
            //UUm橘黄色
            excelHelper.SetChartSeriesCollectionBorderColor(chart, 8, 44);
            //UPm Target红色虚线
            excelHelper.SetChartSeriesCollectionBorderColor(chart, 9, 3);
            excelHelper.SetChartSeriesCollectionBorderLineStyle(chart,9,XlLineStyle.xlDash);

            //设置标签可见
            excelHelper.SetChartDataLabels(chart,true);

            //设置边框线条
            Range allRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[currentRow, 10]];
            excelHelper.SetRangeBodersThickness(allRange, XlBorderWeight.xlThin);
            excelHelper.SetFontHVCenter(allRange);

            Save();
            Quit();
        }

        public ReqRpt018Chart1PostModel PostModel { get; set; } = new ReqRpt018Chart1PostModel();


    }
}