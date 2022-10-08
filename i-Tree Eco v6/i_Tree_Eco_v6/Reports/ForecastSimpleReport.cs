// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastSimpleReport
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using i_Tree_Eco_v6.Forms.Resources;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class ForecastSimpleReport : ForecastReport
  {
    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(this.CreateSimpleChart(), g, C1doc, 1.0, 0.79);
      C1doc.Body.Children.Add(chartRenderObject);
      C1doc.Body.Children.Add((RenderObject) this.CreateSimpleTable());
    }

    protected void ForecastedYear_DataValue_FromCohortResults(CohortResultDataType dataType) => this.results = this.curInputISession.CreateCriteria<CohortResult>().Add((ICriterion) Restrictions.Eq("DataType", (object) dataType)).Add((ICriterion) Restrictions.Eq("Forecast", (object) this._forecast)).Add((ICriterion) Restrictions.IsNull("Stratum")).AddOrder(Order.Asc("ForecastedYear")).List<CohortResult>();

    protected C1.Win.C1Chart.C1Chart CreateSimpleChart()
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      chart.ChartArea.AxisX.Text = v6Strings.Year_SingularName;
      chart.ChartArea.AxisY.Text = this.dataValueHeading;
      chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      ReportUtil.SetChartOptions(chart, false);
      chart.ChartArea.AxisX.AnnoFormat = FormatEnum.NumericManual;
      chart.ChartArea.AxisX.AutoMinor = false;
      if (this.results.Count < 10)
        chart.ChartArea.AxisX.UnitMajor = 1.0;
      chart.ChartArea.AxisX.AnnoFormatString = this.dataValueChartFormatString;
      chart.ChartArea.AxisY.Min = 0.0;
      ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chart.Legend.Visible = false;
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(1);
      foreach (CohortResult result in (IEnumerable<CohortResult>) this.results)
      {
        chartDataSeries.X.Add((object) result.ForecastedYear);
        chartDataSeries.Y.Add((object) this.CalculateValue(result.DataValue));
      }
      if (this.results.Count<CohortResult>((Func<CohortResult, bool>) (r => r.ForecastedYear == 0)) == 0)
      {
        chart.ChartArea.AxisX.AutoOrigin = false;
        chart.ChartArea.AxisX.Origin = 1.0;
      }
      return chart;
    }

    protected RenderTable CreateSimpleTable()
    {
      RenderTable rt = new RenderTable();
      rt.Cells[0, 0].Text = v6Strings.Year_SingularName;
      rt.Cells[0, 1].Text = this.dataValueHeading;
      rt.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      rt.Cols[1].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
      rt.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      rt.Rows[0].Style.FontBold = true;
      rt.Rows[0].Style.FontSize = 14f;
      rt.Width = (Unit) "40%";
      rt.Cols[0].Width = (Unit) "10%";
      double num1 = 0.0;
      int num2 = 0;
      foreach (CohortResult result in (IEnumerable<CohortResult>) this.results)
      {
        rt.Cells[result.ForecastedYear + 1, 0].Text = result.ForecastedYear == 0 ? ForecastRes.CurrentStr : result.ForecastedYear.ToString();
        rt.Cells[result.ForecastedYear + 1, 1].Text = this.CalculateValue(result.DataValue).ToString(this.dataValueTableFormatString);
        num2 = result.ForecastedYear + 1;
        num1 += this.CalculateValue(result.DataValue);
      }
      if (this.addTotalRow)
      {
        int num3 = num2 + 1;
        rt.Cells[num3, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        rt.Cells[num3, 1].Text = num1.ToString(this.dataValueTableFormatString);
        rt.Rows[num3].Style.FontBold = true;
      }
      ReportUtil.FormatRenderTable(rt);
      return rt;
    }

    public override double CalculateValue(double value)
    {
      if (this.isPercent)
        return Math.Min(value * this.convertRatio / this.studyArea * 100.0, 100.0);
      return this.isUnitArea ? value * this.convertRatio / this.studyArea : base.CalculateValue(value);
    }

    public void GetStudyArea()
    {
      if (!this.curYear.RecordStrata)
        return;
      this.studyArea = (double) Enumerable.Sum(this.curInputISession.QueryOver<Strata>().Where((System.Linq.Expressions.Expression<Func<Strata, bool>>) (r => (Guid?) r.Year.Guid == ReportBase.m_ps.InputSession.YearKey)).And((System.Linq.Expressions.Expression<Func<Strata, bool>>) (r => r.Size > 0.0f)).SelectList((Func<QueryOverProjectionBuilder<Strata>, QueryOverProjectionBuilder<Strata>>) (list => list.Select((System.Linq.Expressions.Expression<Func<Strata, object>>) (s => (object) s.Size)))).List<float>());
      if (ReportBase.EnglishUnits && this.curYear.Unit == YearUnit.Metric)
      {
        this.studyArea *= 2.47105;
      }
      else
      {
        if (ReportBase.EnglishUnits || this.curYear.Unit != YearUnit.English)
          return;
        this.studyArea *= 0.404686;
      }
    }
  }
}
