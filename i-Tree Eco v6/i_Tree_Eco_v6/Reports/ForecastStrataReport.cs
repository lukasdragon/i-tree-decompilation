// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastStrataReport
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
  public class ForecastStrataReport : ForecastReport
  {
    protected double maxValue;
    protected List<Strata> lstStrata;
    private List<C1.Win.C1Chart.C1Chart> lstStrataCharts = new List<C1.Win.C1Chart.C1Chart>();
    protected Dictionary<int, double> strataToAreas = new Dictionary<int, double>();
    protected string stackedBarChartTitle;

    protected virtual void GetStrataResultsMax()
    {
      if (this.results.Count == 0)
      {
        this.maxValue = 1.0;
      }
      else
      {
        this.lstStrata = this.results.Select<CohortResult, Strata>((Func<CohortResult, Strata>) (r => r.Stratum)).Distinct<Strata>().ToList<Strata>();
        if (this.isUnitArea || this.isPercent)
        {
          bool flag = true;
          foreach (Strata lstStratum in this.lstStrata)
          {
            Strata s = lstStratum;
            double unitAreaValue = this.CalculateUnitAreaValue(this.results.Where<CohortResult>((Func<CohortResult, bool>) (r => r.Stratum == s)).Max<CohortResult>((Func<CohortResult, double>) (r => r.DataValue)), (double) s.Size);
            if (flag)
            {
              this.maxValue = unitAreaValue;
              flag = false;
            }
            else if (unitAreaValue > this.maxValue)
              this.maxValue = unitAreaValue;
          }
        }
        else
          this.maxValue = this.CalculateValue(this.results.Max<CohortResult>((Func<CohortResult, double>) (x => x.DataValue)));
      }
    }

    protected void Strata_ForecastedYear_DataValue_FromCohortResults(CohortResultDataType dataType) => this.results = this.curInputISession.CreateCriteria<CohortResult>().Add((ICriterion) Restrictions.Eq("DataType", (object) dataType)).Add((ICriterion) Restrictions.Eq("Forecast", (object) this._forecast)).Add((ICriterion) Restrictions.IsNotNull("Stratum")).AddOrder(Order.Asc("Stratum")).AddOrder(Order.Asc("ForecastedYear")).List<CohortResult>();

    protected C1.Win.C1Chart.C1Chart CreateStackedBarChart()
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart()
      {
        Header = {
          Text = this.stackedBarChartTitle
        },
        Style = {
          Font = new Font("Calibri", 14f)
        },
        ChartArea = {
          AxisX = {
            Text = v6Strings.Year_SingularName
          },
          AxisY = {
            Text = this.dataValueHeading
          }
        },
        ChartGroups = {
          Group0 = {
            ChartType = Chart2DTypeEnum.Bar,
            Stacked = true
          }
        }
      };
      chart.ChartArea.AxisX.AnnoFormat = FormatEnum.NumericManual;
      chart.ChartArea.AxisX.AnnoFormatString = this.dataValueChartFormatString;
      chart.Legend.Visible = true;
      chart.BackColor = Color.White;
      ReportUtil.SetChartOptions(chart, false);
      foreach (Strata lstStratum in this.lstStrata)
      {
        Strata s = lstStratum;
        ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries.Label = s.Description;
        foreach (CohortResult cohortResult in (IEnumerable<CohortResult>) this.results.Where<CohortResult>((Func<CohortResult, bool>) (r => r.Stratum == s)).ToList<CohortResult>())
        {
          chartDataSeries.X.Add((object) cohortResult.ForecastedYear);
          if (this.isUnitArea || this.isPercent)
            chartDataSeries.Y.Add((object) this.CalculateUnitAreaValue(cohortResult.DataValue, (double) s.Size));
          else
            chartDataSeries.Y.Add((object) this.CalculateValue(cohortResult.DataValue));
        }
      }
      chart.ChartArea.AxisY.Min = 0.0;
      if (this.results.Count<CohortResult>((Func<CohortResult, bool>) (r => r.ForecastedYear == 0)) == 0)
      {
        chart.ChartArea.AxisX.AutoOrigin = false;
        chart.ChartArea.AxisX.Origin = 1.0;
      }
      return chart;
    }

    protected RenderTable CreateForecastStrataTable()
    {
      RenderTable rt = new RenderTable();
      rt.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      rt.Cells[0, 1].Text = v6Strings.Year_SingularName;
      rt.Cells[0, 1].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      rt.Cells[0, 2].Text = this.dataValueHeading;
      rt.Rows[0].Style.FontBold = true;
      rt.UserCellGroups.Add(new UserCellGroup(new Rectangle(1, 0, 1, 1))
      {
        Style = {
          TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center
        }
      });
      rt.Width = (Unit) "40%";
      rt.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      rt.Cols[1].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      rt.Cols[2].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
      rt.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      int num1 = 0;
      int num2 = -1;
      double num3 = 0.0;
      double num4 = 0.0;
      foreach (CohortResult result in (IEnumerable<CohortResult>) this.results)
      {
        ++num1;
        if (result.Stratum.Id != num2)
        {
          if (this.addTotalRow && num2 != -1)
          {
            rt.Cells[num1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
            rt.Cells[num1, 2].Text = num3.ToString(this.dataValueTableFormatString);
            rt.Rows[num1].Style.FontBold = true;
            num3 = 0.0;
            ++num1;
          }
          num2 = result.Stratum.Id;
          rt.Cells[num1, 0].Text = this.curYear.RecordStrata ? result.Stratum.Description : i_Tree_Eco_v6.Resources.Strings.StudyArea;
        }
        rt.Cells[num1, 1].Text = result.ForecastedYear == 0 ? ForecastRes.CurrentStr : result.ForecastedYear.ToString();
        if (this.isUnitArea || this.isPercent)
        {
          double unitAreaValue = this.CalculateUnitAreaValue(result.DataValue, (double) result.Stratum.Size);
          rt.Cells[num1, 2].Text = unitAreaValue.ToString(this.dataValueTableFormatString);
          num3 += unitAreaValue;
          num4 += unitAreaValue;
        }
        else
        {
          double num5 = this.CalculateValue(result.DataValue);
          rt.Cells[num1, 2].Text = num5.ToString(this.dataValueTableFormatString);
          num3 += num5;
          num4 += num5;
        }
      }
      if (this.addTotalRow && num2 != -1)
      {
        int num6 = num1 + 1;
        rt.Cells[num6, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        rt.Cells[num6, 2].Text = num3.ToString(this.dataValueTableFormatString);
        rt.Rows[num6].Style.FontBold = true;
        int num7 = num6 + 1;
        rt.Cells[num7, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        rt.Cells[num7, 2].Text = num4.ToString(this.dataValueTableFormatString);
        rt.Rows[num7].Style.FontBold = true;
      }
      ReportUtil.FormatRenderTable(rt);
      return rt;
    }

    protected C1.Win.C1Chart.C1Chart CreatePerStrataChart(string ChartHeader, int StrataId)
    {
      List<CohortResult> list = this.results.Where<CohortResult>((Func<CohortResult, bool>) (r => r.Stratum.Id == StrataId)).ToList<CohortResult>();
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      chart.Header.Text = ChartHeader;
      chart.Style.Font = new Font("Calibri", 14f);
      chart.ChartArea.AxisX.Text = v6Strings.Year_SingularName;
      chart.ChartArea.AxisY.Text = this.dataValueHeading;
      chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      ReportUtil.SetChartOptions(chart, false);
      chart.ChartArea.AxisY.SetMinMax((object) 0, (object) this.maxValue);
      chart.ChartArea.AxisX.AnnoFormat = FormatEnum.NumericManual;
      chart.ChartArea.AxisX.AutoMinor = false;
      if (list.Count < 10)
        chart.ChartArea.AxisX.UnitMajor = 1.0;
      chart.ChartArea.AxisX.AnnoFormatString = this.dataValueChartFormatString;
      chart.Legend.Visible = false;
      chart.BackColor = Color.White;
      ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(1);
      foreach (CohortResult cohortResult in (IEnumerable<CohortResult>) list)
      {
        chartDataSeries.X.Add((object) cohortResult.ForecastedYear);
        if (this.isUnitArea || this.isPercent)
          chartDataSeries.Y.Add((object) this.CalculateUnitAreaValue(cohortResult.DataValue, (double) cohortResult.Stratum.Size));
        else
          chartDataSeries.Y.Add((object) this.CalculateValue(cohortResult.DataValue));
      }
      if (this.results.Count<CohortResult>((Func<CohortResult, bool>) (r => r.ForecastedYear == 0)) == 0)
      {
        chart.ChartArea.AxisX.AutoOrigin = false;
        chart.ChartArea.AxisX.Origin = 1.0;
      }
      return chart;
    }

    protected bool StackedBarChartNeeded() => this.lstStrata.Count > 1;

    public virtual double CalculateUnitAreaValue(double value, double strataArea)
    {
      if (strataArea <= 0.0)
        strataArea = 1.0;
      else if (ReportBase.EnglishUnits && this.curYear.Unit == YearUnit.Metric)
        strataArea *= 2.47105;
      else if (!ReportBase.EnglishUnits && this.curYear.Unit == YearUnit.English)
        strataArea *= 0.404686;
      return this.isPercent ? Math.Min(value * this.convertRatio / strataArea * 100.0, 100.0) : value * this.convertRatio / strataArea;
    }
  }
}
