// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastDbhDistrib
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using i_Tree_Eco_v6.Forms.Resources;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class ForecastDbhDistrib : ForecastReport
  {
    private SortedList<double, string> listDBHRangeStartToLabel = new SortedList<double, string>();
    private Dictionary<string, int> yearDBHRangeStartToTreeNumber = new Dictionary<string, int>();
    private Dictionary<int, int> yearToTotalTreeNumberOfTheYear = new Dictionary<int, int>();
    private string DBHFormat = "N0";

    public ForecastDbhDistrib() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleYearlyDBHDistributions;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.initialize();
      for (short yr = 0; (int) yr <= this.yearToTotalTreeNumberOfTheYear.Count - 1; ++yr)
        this.addDataForYear(C1doc, yr, g);
    }

    private void initialize()
    {
      this.SetConvertRatio(Conversions.InToCm);
      this.results = this.curInputISession.CreateCriteria<CohortResult>().Add((ICriterion) Restrictions.Eq("DataType", (object) CohortResultDataType.CDBHDistribution)).Add((ICriterion) Restrictions.Eq("Forecast", (object) this._forecast)).Add((ICriterion) Restrictions.IsNull("Stratum")).AddOrder(Order.Asc("ForecastedYear")).AddOrder(Order.Asc("DBHRangeStart")).List<CohortResult>();
      if (this.convertRatio != 1.0)
      {
        foreach (CohortResult result in (IEnumerable<CohortResult>) this.results)
        {
          result.DBHRangeStart *= this.convertRatio;
          result.DBHRangeEnd *= this.convertRatio;
        }
        this.DBHFormat = "N1";
      }
      this.listDBHRangeStartToLabel.Clear();
      this.yearDBHRangeStartToTreeNumber.Clear();
      this.yearToTotalTreeNumberOfTheYear.Clear();
      foreach (CohortResult result in (IEnumerable<CohortResult>) this.results)
      {
        double num;
        if (!this.listDBHRangeStartToLabel.ContainsKey(result.DBHRangeStart))
        {
          if (result.DBHRangeEnd - result.DBHRangeStart > 100.0)
          {
            SortedList<double, string> rangeStartToLabel = this.listDBHRangeStartToLabel;
            double dbhRangeStart = result.DBHRangeStart;
            num = result.DBHRangeStart;
            string str = "> " + num.ToString(this.DBHFormat);
            rangeStartToLabel.Add(dbhRangeStart, str);
          }
          else
          {
            SortedList<double, string> rangeStartToLabel = this.listDBHRangeStartToLabel;
            double dbhRangeStart = result.DBHRangeStart;
            num = result.DBHRangeStart;
            string str1 = num.ToString(this.DBHFormat);
            num = result.DBHRangeEnd;
            string str2 = num.ToString(this.DBHFormat);
            string str3 = str1 + " - " + str2;
            rangeStartToLabel.Add(dbhRangeStart, str3);
          }
        }
        int forecastedYear;
        if (this.yearToTotalTreeNumberOfTheYear.ContainsKey(result.ForecastedYear))
        {
          Dictionary<int, int> treeNumberOfTheYear = this.yearToTotalTreeNumberOfTheYear;
          forecastedYear = result.ForecastedYear;
          treeNumberOfTheYear[forecastedYear] += (int) result.DataValue;
        }
        else
          this.yearToTotalTreeNumberOfTheYear.Add(result.ForecastedYear, (int) result.DataValue);
        Dictionary<string, int> startToTreeNumber = this.yearDBHRangeStartToTreeNumber;
        forecastedYear = result.ForecastedYear;
        string str4 = forecastedYear.ToString();
        num = result.DBHRangeStart;
        string str5 = num.ToString();
        string key = str4 + "_" + str5;
        int dataValue = (int) result.DataValue;
        startToTreeNumber.Add(key, dataValue);
      }
    }

    private void addDataForYear(C1PrintDocument C1doc, short yr, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      renderTable.Width = (Unit) "50%";
      renderTable.Cells[0, 0].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.DBHRange, ReportBase.CentimeterUnits());
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeNumberInRange;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.TreePercent;
      renderTable.Rows[0].Style.FontBold = true;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
      renderTable.Cols[2].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
      renderTable.Cells[0, 1].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      c1Chart.BackColor = Color.White;
      c1Chart.Style.Font = new Font("Calibri", 14f);
      c1Chart.ChartArea.AxisX.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.DBHRange, ReportBase.CentimeterUnits());
      c1Chart.ChartArea.AxisY.Text = i_Tree_Eco_v6.Resources.Strings.TreeNumber;
      c1Chart.ChartArea.AxisY2.Visible = true;
      c1Chart.ChartArea.AxisY2.Text = i_Tree_Eco_v6.Resources.Strings.PercentOfTreesV;
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      c1Chart.ChartGroups.Group1.ChartType = Chart2DTypeEnum.XYPlot;
      string str1 = yr == (short) 0 ? string.Format(ForecastRes.DbhDistribChartHeader, (object) ForecastRes.CurrentStr, (object) "") : string.Format(ForecastRes.DbhDistribChartHeader, (object) "", (object) string.Format(": {0} {1}", (object) v6Strings.Year_SingularName, (object) yr.ToString()));
      c1Chart.Header.Text = str1;
      ReportUtil.SetChartOptions(c1Chart, false);
      c1Chart.Legend.Visible = false;
      c1Chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels;
      c1Chart.ChartArea.AxisX.AnnotationRotation = -45;
      c1Chart.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      ChartGroup group0 = c1Chart.ChartGroups.Group0;
      ChartGroup group1 = c1Chart.ChartGroups.Group1;
      ChartDataSeries chartDataSeries1 = group0.ChartData.SeriesList.AddNewSeries();
      ChartDataSeries chartDataSeries2 = group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries2.LineStyle.Pattern = LinePatternEnum.None;
      chartDataSeries2.SymbolStyle.Shape = SymbolShapeEnum.None;
      chartDataSeries1.LineStyle.Color = ReportUtil.GetColor(1);
      chartDataSeries1.Label = i_Tree_Eco_v6.Resources.Strings.TreeNumbers;
      chartDataSeries2.Label = i_Tree_Eco_v6.Resources.Strings.Percent;
      int num1 = this.yearToTotalTreeNumberOfTheYear[(int) yr];
      int row = 0;
      for (int index = 0; index < this.listDBHRangeStartToLabel.Count; ++index)
      {
        string str2 = yr.ToString();
        KeyValuePair<double, string> keyValuePair = this.listDBHRangeStartToLabel.ElementAt<KeyValuePair<double, string>>(index);
        string str3 = keyValuePair.Key.ToString();
        string key = str2 + "_" + str3;
        int num2;
        if (this.yearDBHRangeStartToTreeNumber.ContainsKey(key))
        {
          num2 = this.yearDBHRangeStartToTreeNumber[key];
          chartDataSeries1.X.Add((object) index);
          ValueLabelsCollection valueLabels = c1Chart.ChartArea.AxisX.ValueLabels;
          // ISSUE: variable of a boxed type
          __Boxed<int> val = (ValueType) index;
          keyValuePair = this.listDBHRangeStartToLabel.ElementAt<KeyValuePair<double, string>>(index);
          string text = keyValuePair.Value;
          valueLabels.Add((object) val, text);
          chartDataSeries1.Y.Add((object) num2);
          chartDataSeries2.X.Add((object) index);
          chartDataSeries2.Y.Add((object) ((double) num2 / (double) num1 * 100.0));
        }
        else
        {
          num2 = 0;
          chartDataSeries1.X.Add((object) index);
          ValueLabelsCollection valueLabels = c1Chart.ChartArea.AxisX.ValueLabels;
          // ISSUE: variable of a boxed type
          __Boxed<int> val = (ValueType) index;
          keyValuePair = this.listDBHRangeStartToLabel.ElementAt<KeyValuePair<double, string>>(index);
          string text = keyValuePair.Value;
          valueLabels.Add((object) val, text);
          chartDataSeries1.Y.Add((object) num2);
          chartDataSeries2.X.Add((object) index);
          chartDataSeries2.Y.Add((object) ((double) num2 / (double) num1 * 100.0));
        }
        ++row;
        TableCell cell = renderTable.Cells[row, 0];
        keyValuePair = this.listDBHRangeStartToLabel.ElementAt<KeyValuePair<double, string>>(index);
        string str4 = keyValuePair.Value;
        cell.Text = str4;
        renderTable.Cells[row, 1].Text = num2.ToString();
        renderTable.Cells[row, 2].Text = ((double) num2 / (double) num1 * 100.0).ToString("#0.00");
      }
      c1Chart.ChartArea.AxisY.Max = chartDataSeries1.MaxY * 1.15;
      c1Chart.ChartArea.AxisY2.Max = chartDataSeries2.MaxY * 1.15;
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 0.8, 0.79);
      C1doc.Body.Children.Add(chartRenderObject);
      ReportUtil.FormatRenderTable(renderTable);
      C1doc.Body.Children.Add((RenderObject) renderTable);
      if ((int) yr == this.yearToTotalTreeNumberOfTheYear.Count - 1)
        return;
      renderTable.BreakAfter = BreakEnum.Page;
    }
  }
}
