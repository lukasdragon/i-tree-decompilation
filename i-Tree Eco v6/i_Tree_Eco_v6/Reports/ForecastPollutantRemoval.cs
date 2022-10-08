// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastPollutantRemoval
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Forms.Resources;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class ForecastPollutantRemoval : ForecastReport
  {
    private Dictionary<int, string> _pollutantNames;
    private string _pollutantName;
    private int _pollutantId;
    private string _polStr;

    public string PollutantName
    {
      get => this._pollutantName;
      set => this._pollutantName = value;
    }

    public ForecastPollutantRemoval(string name, string polStr = null)
    {
      this._pollutantNames = new Dictionary<int, string>();
      this._pollutantName = name;
      this._polStr = polStr;
      if (this._pollutantName == "All")
        this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleValueForRemovalOfPollutantsOverTime;
      else
        this.ReportTitle = string.Format(i_Tree_Eco_v6.Resources.Strings.ReportTitlePollutantRemovalAmountAndValueOverTime, (object) this._pollutantName);
      RetryExecutionHandler.Execute((System.Action) (() =>
      {
        using (ISession session = Program.Session.LocSp.OpenSession())
        {
          this._pollutantId = session.QueryOver<Pollutant>().Where((System.Linq.Expressions.Expression<Func<Pollutant, bool>>) (p => p.Name == this._pollutantName)).Select((System.Linq.Expressions.Expression<Func<Pollutant, object>>) (p => (object) p.Id)).Cacheable().SingleOrDefault<int>();
          this._pollutantNames = session.QueryOver<Pollutant>().Cacheable().List().ToDictionary<Pollutant, int, string>((Func<Pollutant, int>) (p => p.Id), (Func<Pollutant, string>) (p => p.Name));
        }
      }));
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.SetConvertRatio(Conversions.KgLbToKgLb);
      IList<ForecastPollutantRemoval.Result> source1 = this.loadData(this._pollutantName, this.convertRatio);
      source1.OrderBy<ForecastPollutantRemoval.Result, int>((Func<ForecastPollutantRemoval.Result, int>) (r => r.PollutantId)).Select<ForecastPollutantRemoval.Result, int>((Func<ForecastPollutantRemoval.Result, int>) (r => r.PollutantId)).Distinct<int>().ToList<int>();
      if (this._pollutantName == "All")
      {
        C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
        IEnumerable<\u003C\u003Ef__AnonymousType0<short, double>> source2 = source1.OrderBy<ForecastPollutantRemoval.Result, short>((Func<ForecastPollutantRemoval.Result, short>) (r => r.Year)).GroupBy<ForecastPollutantRemoval.Result, short>((Func<ForecastPollutantRemoval.Result, short>) (r => r.Year)).Select(p => new
        {
          Year = p.Key,
          Value = p.Sum<ForecastPollutantRemoval.Result>((Func<ForecastPollutantRemoval.Result, double>) (s => s.Value))
        });
        c1Chart.ChartArea.AxisX.Text = v6Strings.Year_SingularName;
        c1Chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CombinedPollutantRemovalValue, this.CurrencySymbol);
        c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
        c1Chart.BackColor = Color.White;
        c1Chart.Style.Font = new Font("Calibri", 14f);
        ReportUtil.SetChartOptions(c1Chart, false);
        c1Chart.ChartArea.AxisX.AnnoFormat = FormatEnum.NumericManual;
        c1Chart.ChartArea.AxisX.AutoMinor = false;
        if (source2.Count() < 10)
          c1Chart.ChartArea.AxisX.UnitMajor = 1.0;
        c1Chart.ChartArea.AxisX.AnnoFormatString = "N0";
        c1Chart.ChartArea.AxisX.AutoMax = true;
        ChartDataSeries chartDataSeries = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries.LineStyle.Pattern = LinePatternEnum.None;
        chartDataSeries.SymbolStyle.Shape = SymbolShapeEnum.Tri;
        chartDataSeries.SymbolStyle.Size = 10;
        chartDataSeries.SymbolStyle.Color = Color.Black;
        chartDataSeries.LineStyle.Color = ReportUtil.GetColor(1);
        foreach (var data in source2)
        {
          chartDataSeries.X.Add((object) data.Year);
          chartDataSeries.Y.Add((object) data.Value);
        }
        RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 1.0, 0.79);
        C1doc.Body.Children.Add(chartRenderObject);
        RenderTable renderTable = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable);
        renderTable.Width = (Unit) "40%";
        renderTable.Cols[0].Width = (Unit) "10%";
        renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        int count = 2;
        renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
        renderTable.Cells[0, 0].Text = v6Strings.Year_SingularName;
        renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.CombinedPollutantRemovalValue;
        renderTable.Cells[1, 1].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
        int num1 = count;
        double num2 = 0.0;
        foreach (var data in source2)
        {
          renderTable.Cells[num1, 0].Text = data.Year == (short) 0 ? ForecastRes.CurrentStr : data.Year.ToString();
          renderTable.Cells[num1, 1].Text = data.Value.ToString("N0");
          num2 += data.Value;
          ++num1;
        }
        renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        renderTable.Cells[num1, 1].Text = num2.ToString("N0");
        renderTable.Rows[num1].Style.FontBold = true;
        ReportUtil.FormatRenderTable(renderTable);
        this.Note(C1doc);
      }
      else
      {
        IList<ForecastPollutantRemoval.Result> list = (IList<ForecastPollutantRemoval.Result>) source1.Where<ForecastPollutantRemoval.Result>((Func<ForecastPollutantRemoval.Result, bool>) (r => r.PollutantId == this._pollutantId)).OrderBy<ForecastPollutantRemoval.Result, short>((Func<ForecastPollutantRemoval.Result, short>) (r => r.Year)).ToList<ForecastPollutantRemoval.Result>();
        C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
        string str1 = ReportUtil.FormatInlineHeaderUnitsStr(string.Format(i_Tree_Eco_v6.Resources.Strings.PollutantRemoved, (object) this._polStr), ReportBase.KgUnits());
        c1Chart.ChartArea.AxisX.Text = v6Strings.Year_SingularName;
        c1Chart.ChartArea.AxisY.Text = str1;
        c1Chart.ChartArea.AxisY2.Visible = true;
        c1Chart.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.PollutantRemovalValue, this.CurrencySymbol);
        c1Chart.ChartArea.AxisY2.Rotation = RotationEnum.Rotate270;
        c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.XYPlot;
        c1Chart.ChartGroups.Group1.ChartType = Chart2DTypeEnum.Bar;
        c1Chart.BackColor = Color.White;
        c1Chart.Style.Font = new Font("Calibri", 14f);
        ReportUtil.SetChartOptions(c1Chart, false);
        c1Chart.Legend.Visible = false;
        c1Chart.ChartArea.AxisX.AnnoFormat = FormatEnum.NumericManual;
        c1Chart.ChartArea.AxisX.AutoMinor = false;
        if (list.Count < 10)
          c1Chart.ChartArea.AxisX.UnitMajor = 1.0;
        c1Chart.ChartArea.AxisX.AnnoFormatString = "N0";
        ChartGroup group1 = c1Chart.ChartGroups.Group1;
        ChartDataSeries chartDataSeries1 = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        ChartDataSeries chartDataSeries2 = group1.ChartData.SeriesList.AddNewSeries();
        chartDataSeries1.Label = str1;
        chartDataSeries2.Label = i_Tree_Eco_v6.Resources.Strings.PollutantRemovalValue;
        chartDataSeries1.LineStyle.Pattern = LinePatternEnum.None;
        chartDataSeries1.SymbolStyle.Shape = SymbolShapeEnum.None;
        chartDataSeries2.LineStyle.Color = ReportUtil.GetColor(1);
        foreach (ForecastPollutantRemoval.Result result in (IEnumerable<ForecastPollutantRemoval.Result>) list)
        {
          chartDataSeries1.X.Add((object) result.Year);
          chartDataSeries1.Y.Add((object) result.Amount);
          chartDataSeries2.X.Add((object) result.Year);
          chartDataSeries2.Y.Add((object) result.Value);
        }
        c1Chart.ChartArea.AxisY.Min = 0.0;
        c1Chart.ChartArea.AxisY.Max = chartDataSeries1.MaxY * 1.15;
        c1Chart.ChartArea.AxisY2.Min = 0.0;
        c1Chart.ChartArea.AxisY2.Max = chartDataSeries2.MaxY * 1.15;
        RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 1.0, 0.79);
        C1doc.Body.Children.Add(chartRenderObject);
        RenderTable renderTable = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable);
        renderTable.Width = (Unit) "60%";
        renderTable.Cols[0].Width = (Unit) "10%";
        renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        int count = 2;
        renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
        renderTable.Cells[0, 0].Text = v6Strings.Year_SingularName;
        renderTable.Cells[0, 1].Text = string.Format(i_Tree_Eco_v6.Resources.Strings.PollutantRemoved, (object) this._polStr);
        renderTable.Cells[1, 1].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.KgUnits());
        renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.PollutantRemovalValue;
        renderTable.Cells[1, 2].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
        int num3 = count;
        double num4 = 0.0;
        double num5 = 0.0;
        foreach (ForecastPollutantRemoval.Result result in (IEnumerable<ForecastPollutantRemoval.Result>) list)
        {
          renderTable.Cells[num3, 0].Text = result.Year == (short) 0 ? ForecastRes.CurrentStr : result.Year.ToString();
          TableCell cell1 = renderTable.Cells[num3, 1];
          double amount = result.Amount;
          string str2 = amount.ToString("N1");
          cell1.Text = str2;
          TableCell cell2 = renderTable.Cells[num3, 2];
          amount = result.Value;
          string str3 = amount.ToString("N2");
          cell2.Text = str3;
          num4 += result.Amount;
          num5 += result.Value;
          ++num3;
        }
        renderTable.Cells[num3, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        renderTable.Cells[num3, 1].Text = num4.ToString("N1");
        renderTable.Cells[num3, 2].Text = num5.ToString("N2");
        renderTable.Rows[num3].Style.FontBold = true;
        ReportUtil.FormatRenderTable(renderTable);
        RenderTable ro = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) ro);
        ro.Style.FontSize = 12f;
        ro.Style.Spacing.Top = (Unit) "1ls";
        ro.Width = (Unit) "100%";
        ro.Cols[0].Width = (Unit) "100%";
        ro.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        ro.Cells[0, 0].Text = this.PollutionRemoval_kg_lb_Footer(this._pollutantName);
      }
    }

    protected override string ReportMessage() => this.PollutionRemoval_kg_lb_Footer() + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteZeroValuesMeaning + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.WR_PM25Pm10;

    private IList<ForecastPollutantRemoval.Result> loadData(
      string pollutantName,
      double convertRatio)
    {
      PollutantResult pra = (PollutantResult) null;
      Forecast fa = (Forecast) null;
      IQueryOver<PollutantResult, PollutantResult> queryOver = this.curInputISession.QueryOver<PollutantResult>((System.Linq.Expressions.Expression<Func<PollutantResult>>) (() => pra)).JoinAlias((System.Linq.Expressions.Expression<Func<object>>) (() => pra.Forecast), (System.Linq.Expressions.Expression<Func<object>>) (() => fa)).Where((System.Linq.Expressions.Expression<Func<bool>>) (() => (Guid?) fa.Guid == ReportBase.m_ps.InputSession.ForecastKey)).Select((IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pra.PollutantId)), (IProjection) Projections.Group((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pra.ForecastedYear)), (IProjection) Projections.Sum((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pra.Amount)), (IProjection) Projections.Sum((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pra.Value))).OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pra.PollutantId)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pra.ForecastedYear)).Asc;
      if (pollutantName != "All")
        queryOver = queryOver.Where((System.Linq.Expressions.Expression<Func<bool>>) (() => pra.PollutantId == this._pollutantId));
      double convertToMetricTonRatio = 0.001;
      if (this.curYear.Unit == YearUnit.English)
        convertToMetricTonRatio = 0.000453592;
      return (IList<ForecastPollutantRemoval.Result>) queryOver.List<object[]>().Select<object[], ForecastPollutantRemoval.Result>((Func<object[], ForecastPollutantRemoval.Result>) (record => new ForecastPollutantRemoval.Result()
      {
        PollutantId = Convert.ToInt32(record[0]),
        Year = Convert.ToInt16(record[1]),
        Amount = convertRatio * Convert.ToDouble(record[2]),
        Value = Convert.ToInt32(record[0]) == 1 ? convertToMetricTonRatio * Convert.ToDouble(record[2]) * this.customizedCoDollarsPerTon : (Convert.ToInt32(record[0]) == 2 ? convertToMetricTonRatio * Convert.ToDouble(record[2]) * this.customizedNO2DollarsPerTon : (Convert.ToInt32(record[0]) == 3 ? convertToMetricTonRatio * Convert.ToDouble(record[2]) * this.customizedO3DollarsPerTon : (Convert.ToInt32(record[0]) == 5 ? convertToMetricTonRatio * Convert.ToDouble(record[2]) * this.customizedPM25DollarsPerTon : (Convert.ToInt32(record[0]) == 6 ? convertToMetricTonRatio * Convert.ToDouble(record[2]) * this.customizedSO2DollarsPerTon : (Convert.ToInt32(record[0]) == 8 ? convertToMetricTonRatio * Convert.ToDouble(record[2]) * this.customizedPM10DollarsPerTon : 0.0)))))
      })).ToList<ForecastPollutantRemoval.Result>();
    }

    private struct Result
    {
      public int PollutantId;
      public short Year;
      public double Amount;
      public double Value;
    }
  }
}
