using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace Caojin.Common
{
    public class ExcelHelper
    {
        public ExcelHelper()
        { }

        public Excel.Application ExcelApp()
        {
            //设置程序运行语言
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            //创建Application
            Excel.Application xlApp = new Excel.Application();
            //设置是否显示警告窗体
            xlApp.DisplayAlerts = false;
            //设置是否显示Excel
            xlApp.Visible = false;
            //禁止刷新屏幕
            xlApp.ScreenUpdating = false;
            //屏蔽关闭前告警
            xlApp.AlertBeforeOverwriting = false;
            //根据路径path打开
            return xlApp;
        }

        public Excel.Workbook GetWorkbook(string filePath, Excel.Application app)
        {
            //根据路径path打开
            Excel.Workbook xlsWorkBook = app.Workbooks.Open(filePath, System.Type.Missing, System.Type.Missing, System.Type.Missing,
            System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing,
            System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);

            return xlsWorkBook;
        }

        public Excel.Workbook MakeNewWorkbook(Excel.Application app)
        {   
            return app.Workbooks.Add();
        }

        //按sheet名获取
        public Excel.Worksheet GetWorksheet(Excel.Workbook workbook, string sheetname)
        {
            return (Worksheet)workbook.Worksheets[sheetname];
        }

        //按sheet排序获取
        public Excel.Worksheet GetWorksheet(Excel.Workbook workbook, int sheetno)
        {
            return (Worksheet)workbook.Worksheets[sheetno];
        }

        public int GetTotalRowCount(Excel.Worksheet sheet)
        {
            return sheet.UsedRange.Rows.Count;
        }

        public int GetTotalColumCount(Excel.Worksheet worksheet)
        {
            return worksheet.UsedRange.Columns.Count;
        }

        //删除行
        public void DeleteRow(Excel.Worksheet sheet, int rowno)
        {
            Range deleteRng = (Range)sheet.Rows[rowno, System.Type.Missing];
            deleteRng.Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
        }

        //删除列
        public void DeleteColumn(Excel.Worksheet sheet, int colno)
        {
            ((Excel.Range)sheet.Cells[1, colno]).EntireColumn.Delete(0);
        }

        //设置背景色，红色colorindex=3
        public void SetRangeBackground(Range range, int colorIndex = 3)
        {
            range.Interior.ColorIndex = colorIndex;
        }

        //获取range值
        public string GetCellValue(Range range)
        {
            return Convert.ToString(range.Value2);

        }

        //设置字号
        public void SetFontSize(Range range, int size)
        {
            range.Font.Size = size;
        }
        //设置字体
        public void SetFontStyle(Range range, string name)
        {
            range.Font.Name = name;
        }

        //是否设置粗体
        public void SetBoldValue(Range range, bool isbold = true)
        {
            range.Font.Bold = isbold;
        }

        //设置水平垂直居中
        public void SetFontHVCenter(Range range)
        {
            range.HorizontalAlignment = XlVAlign.xlVAlignCenter;
        }

        //设置水平靠左
        public void SetFontHVLeft(Range range)
        {
            range.HorizontalAlignment = XlHAlign.xlHAlignLeft;
        }

        //设置区域边框
        public void SetRangeBodersStyle(Range range, int linestyle)
        {
            range.Borders.LineStyle = linestyle;
        }

        //设置边框的线条
        public void SetRangeBodersThickness(Range range, XlBorderWeight weight)
        {
            range.Borders.Weight = weight;
        }

        //设置区域单元格为数字格式,小数点后面保留1位
        public void SetRangeValueStyleNumber(Range range, string format = "0.0")
        {
            range.NumberFormat = format;
        }

        //设置区域单元格为文本格式
        public void SetRangeValueStyleText(Range range)
        {
            range.NumberFormat = "@";
        }


        //设置区域复制
        public void Copy(Range sRange, Range dRang)
        {
            sRange.Select();
            sRange.Copy(Type.Missing);
            dRang.Select();
            dRang.Parse(Missing.Value, Missing.Value);
        }

        #region Chart About
        /// <summary>
        /// 插入sheet级别的图表
        /// </summary>
        /// <param name="workbook">工作簿</param>
        /// <param name="range">数据所在坐标范围</param>
        /// <param name="plotBy">指定绘制数据的方式，是以行为数据源（列为该数据源数据）还是以列为数据源（行为该数据源数据</param>
        /// <returns>Chart对象</returns>
        public Chart AddSheetLevelChart(Excel.Workbook workbook,Range range, Excel.XlRowCol plotBy = Excel.XlRowCol.xlColumns)
        {
            Chart chart = workbook.Charts.Add();
            chart.SetSourceData(range,plotBy);
            return chart;
        }

        /// <summary>
        /// 插入sheet子图表
        /// </summary>
        /// <param name="sheet">表对象</param>
        /// <param name="range">数据范围对象</param>
        /// <param name="plotBy">指定绘制数据的方式，是以行为数据源（列为该数据源数据）还是以列为数据源（行为该数据源数据</param>
        /// <returns>Chart对象</returns>
        public Chart AddShapeLevelChart(Excel.Worksheet sheet,Range range, Excel.XlRowCol plotBy = Excel.XlRowCol.xlColumns)
        {
            Shape shape= sheet.Shapes.AddChart();
            Chart chart = shape.Chart;
            chart.SetSourceData(range,plotBy);
            return chart;
        }

        public Chart AddShapeLevelChart(Excel.Worksheet sheet, Range range,float width,float height ,Excel.XlRowCol plotBy = Excel.XlRowCol.xlColumns)
        {
            Shape shape = sheet.Shapes.AddChart();
            shape.Width = width;
            shape.Height = height;
            Chart chart = shape.Chart;
            chart.SetSourceData(range, plotBy);
            return chart;
        }

        public Chart AddShapeLevelChart(Worksheet sheet)
        {
            Shape shape = sheet.Shapes.AddChart();
            Chart chart = shape.Chart;
            return chart;
        }

        public Chart AddShapeLevelChart(Worksheet sheet,float width,float height)
        {
            Shape shape = sheet.Shapes.AddChart();
            shape.Width = width;
            shape.Height = height;
            Chart chart = shape.Chart;
            return chart;
        }

        /// <summary>
        /// 设置Chart类型
        /// </summary>
        /// <param name="chart">Chart对象</param>
        /// <param name="chartType">Chart类型，如柱形图等</param>
        public void SetChartStyle(Chart chart,XlChartType chartType)
        {
            chart.ChartType = chartType;
        }

        /// <summary>
        /// 设置图标的x,y坐标标题、图表标题
        /// </summary>
        /// <param name="chart">Chart</param>
        /// <param name="x">x轴标题，空字符串则不设置标题</param>
        /// <param name="y">y轴标题，空字符串则不设置标题</param>
        /// <param name="title">图表标题，空字符串则不设置标题</param>
        public void SetChartXYTitle(Chart chart, string x,string y,string title)
        {
            Axis xAxis = chart.Axes(XlAxisType.xlCategory,XlAxisGroup.xlPrimary);
            Axis yAxis= chart.Axes(XlAxisType.xlValue, XlAxisGroup.xlPrimary);     
            xAxis.HasTitle = string.IsNullOrEmpty(x)?false: true;
            if (xAxis.HasTitle) { xAxis.AxisTitle.Text = x; }
           
            yAxis.HasTitle = string.IsNullOrEmpty(y) ? false : true;
            if (yAxis.HasTitle) { yAxis.AxisTitle.Text = y; }

            chart.HasTitle = string.IsNullOrEmpty(title) ? false : true;
            if (chart.HasTitle) { chart.ChartTitle.Text = title; }
        }

        public void SetChartYMaxMinValue(Chart chart,double max,double min,XlAxisGroup yType)
        {
            Axis yAxis = chart.Axes(XlAxisType.xlValue,yType);
            yAxis.MinimumScaleIsAuto = false;
            yAxis.MaximumScaleIsAuto = false;
            yAxis.MinimumScale = min;
            yAxis.MaximumScale = max;
        }
        /// <summary>
        ///设置chart Y轴 label显示数字的格式 
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="yType"></param>
        /// <param name="format">0.00%为百分比</param>
        public void SetChartYThickLabelNumberFormat(Chart chart,XlAxisGroup yType,string format) {
            Axis yAxis = chart.Axes(XlAxisType.xlValue, yType);
            yAxis.TickLabels.NumberFormatLocal = format;
        }

        /// <summary>
        /// 设置系列的y坐标为副坐标
        /// </summary>
        /// <param name="chart">chart</param>
        /// <param name="seriesIndex">index</param>
        public void SetChartSeriesCollectionY(Chart chart,int seriesIndex)
        {
            // chart.SeriesCollection(seriesIndex).Axes(XlAxisType.xlCategory, XlAxisGroup.xlSecondary);
            //Axis ysAxis = chart.Axes(XlAxisType.xlValue, XlAxisGroup.xlSecondary);
            //  ysAxis.HasTitle = string.IsNullOrEmpty(y) ? false : true;
            // ysAxis.AxisTitle.Text = y;
            Excel.Series se = (Excel.Series)chart.SeriesCollection(seriesIndex);
            se.AxisGroup = XlAxisGroup.xlSecondary;
        }
        /// <summary>
        /// 设置指定系列的图标类型
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="seriesIndex"></param>
        /// <param name="chartType"></param>
        public void SetChartSeriesCollectionChartType(Chart chart,int seriesIndex,XlChartType chartType)
        {
            Excel.Series se = (Excel.Series)chart.SeriesCollection(seriesIndex);
            se.ChartType = chartType;
        }
        /// <summary>
        /// 设置x轴显示数据
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="range"></param>
        public void SetChartSeriesCollectionXValues(Chart chart,Range range)
        {
            var c = chart.SeriesCollection().Count;
            for(int i=1;i<=c;i++)
            {
                chart.SeriesCollection(i).XValues = range;
            }
           
        }
        /// <summary>
        /// 增加新的系列数据至图表
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="seriesName"></param>
        /// <param name="valueRange"></param>
        public void ChartAddNewSeries(Chart chart,string seriesName,Range valueRange)
        {
            chart.SeriesCollection().NewSeries();
            var c = chart.SeriesCollection().Count;
            chart.SeriesCollection().Name =seriesName ;
            chart.SeriesCollection().Values = valueRange;
        }
        /// <summary>
        /// 增加新的系列数据至图标
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="nameRange"></param>
        /// <param name="valueRange"></param>
        public void ChartAddNewSeries(Chart chart, Range nameRange, Range valueRange)
        {
            chart.SeriesCollection().NewSeries();
            var c = chart.SeriesCollection().Count;
            chart.SeriesCollection(c).Name = nameRange;
            chart.SeriesCollection(c).Values = valueRange;
        }
        /// <summary>
        /// 设置系列的名称
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="nameRange"></param>
        /// <param name="seriesIndex"></param>
        public void SetChartSeriesName(Chart chart, Range nameRange,int seriesIndex)
        {
            chart.SeriesCollection(seriesIndex).Name = nameRange;
        }

        public void SetChartSeriesName(Chart chart, string name, int seriesIndex)
        {
            chart.SeriesCollection(seriesIndex).Name = name;
        }

        /// <summary>
        /// 改图例颜色（Line型）
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="seriesIndex">系列号</param>
        /// <param name="colorIndex">色号</param>
        public void SetChartSeriesCollectionBorderColor(Chart chart,int seriesIndex,int colorIndex)
        {
            chart.SeriesCollection(seriesIndex).Border.ColorIndex = colorIndex;
        }

        public void SetChartSeriesCollectionBorderLineStyle(Chart chart,int seriesIndex,XlLineStyle lineStyle)
        {
            chart.SeriesCollection(seriesIndex).Border.LineStyle = lineStyle;
        }
        /// <summary>
        /// 改图例颜色（柱型）
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="seriesIndex"></param>
        /// <param name="rgb"></param>
        public void SetChartSeriesCollectionFillColor(Chart chart, int seriesIndex, int rgb)
        {
            chart.SeriesCollection(seriesIndex).Format.Fill.ForeColor.RGB = rgb;
        }

        /// <summary>
        /// 设置标签是否可见
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="show"></param>
        public void SetChartDataLabels(Chart chart,bool show)
        {
            if (show) { chart.ApplyDataLabels(); } else { chart.ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowNone); }
        }

        public void SetChartDataLabels(Chart chart, int seriesIndex,bool show)
        {
            var type = show ? XlDataLabelsType.xlDataLabelsShowLabel : XlDataLabelsType.xlDataLabelsShowNone;
            chart.SeriesCollection(seriesIndex).ApplyDataLabels(type);
        }
        #endregion

        public void Save(Excel.Workbook workbook, string filePath)
        {
            workbook.SaveAs(filePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
        }

        //关闭excel
        public void QuitExcel(Excel.Application app)
        {
            app.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
            app = null;
        }



    }
}
