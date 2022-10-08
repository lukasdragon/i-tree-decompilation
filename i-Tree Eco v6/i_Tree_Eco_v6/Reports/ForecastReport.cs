// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastReport
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  public class ForecastReport : DatabaseReport
  {
    protected double thousand = 1000.0;
    protected double tenThousands = 10000.0;
    protected double million = 1000000.0;
    protected Forecast _forecast;
    protected string dataValueHeading = i_Tree_Eco_v6.Resources.Strings.DataValue;
    protected string dataValueChartFormatString = "N0";
    protected string dataValueTableFormatString = "N1";
    protected double convertRatio = 1.0;
    protected double unitArea = 1.0;
    protected double studyArea = 1.0;
    protected bool isUnitArea;
    protected bool isPercent;
    protected bool addTotalRow;
    protected IList<CohortResult> results;

    public ForecastReport() => this._forecast = this.curInputISession.Get<Forecast>((object) ReportBase.m_ps.InputSession.ForecastKey);

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
    }

    public override void SetHeaderText(ref StringBuilder sb)
    {
      sb.Append(Environment.NewLine);
      sb.Append(string.Format("{0}, ", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtReportHeader, (object) v6Strings.Project_SingularName, (object) this.project.Name)));
      sb.Append(string.Format("{0}, ", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtReportHeader, (object) v6Strings.Series_SingularName, (object) this.series.Id)));
      sb.Append(string.Format("{0}, ", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtReportHeader, (object) v6Strings.Year_SingularName, (object) this.curYear.Id)));
      sb.Append(string.Format("{0}{1}", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtReportHeader, (object) i_Tree_Eco_v6.Resources.Strings.Forecast, (object) this._forecast.Title), (object) Environment.NewLine));
      sb.Append(string.Format("{0}", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtReportHeader, (object) i_Tree_Eco_v6.Resources.Strings.Generated, (object) DateTime.Now.ToShortDateString())));
    }

    public virtual double CalculateValue(double value) => value * this.convertRatio;

    public void SetConvertRatio(Conversions c)
    {
      if (this.curYear.Unit == YearUnit.English)
      {
        if (ReportBase.EnglishUnits)
        {
          switch (c)
          {
            case Conversions.KgLbToMetricTonShortTon:
              this.convertRatio = 0.0005;
              break;
            case Conversions.KgLbToKgLb:
              this.convertRatio = 1.0;
              break;
            case Conversions.SqFtSqMToAcreHectare:
              this.convertRatio = 2.29568E-05;
              break;
            case Conversions.SqFtSqMToSqFtSqM:
              this.convertRatio = 1.0;
              break;
            case Conversions.SqInSqCmToSqFtSqM:
              this.convertRatio = 0.00694444;
              break;
            default:
              this.convertRatio = 1.0;
              break;
          }
        }
        else
        {
          switch (c)
          {
            case Conversions.KgLbToMetricTonShortTon:
              this.convertRatio = 0.00045359236;
              break;
            case Conversions.KgLbToKgLb:
              this.convertRatio = 0.45359236;
              break;
            case Conversions.SqFtSqMToAcreHectare:
              this.convertRatio = 9.2903044E-06;
              break;
            case Conversions.SqFtSqMToSqFtSqM:
              this.convertRatio = 0.09290304;
              break;
            case Conversions.SqInSqCmToSqFtSqM:
              this.convertRatio = 0.00064516;
              break;
            case Conversions.KgHectareToLbAcre:
              this.convertRatio = 1.12085;
              break;
            case Conversions.InToCm:
              this.convertRatio = 2.54;
              break;
            default:
              this.convertRatio = 1.0;
              break;
          }
        }
      }
      else if (ReportBase.EnglishUnits)
      {
        switch (c)
        {
          case Conversions.KgLbToMetricTonShortTon:
            this.convertRatio = 0.00110231;
            break;
          case Conversions.KgLbToKgLb:
            this.convertRatio = 2.20462261;
            break;
          case Conversions.SqFtSqMToAcreHectare:
            this.convertRatio = 0.000247105;
            break;
          case Conversions.SqFtSqMToSqFtSqM:
            this.convertRatio = 10.76391041660208;
            break;
          case Conversions.SqInSqCmToSqFtSqM:
            this.convertRatio = 0.00107639;
            break;
          case Conversions.KgHectareToLbAcre:
            this.convertRatio = 0.892179;
            break;
          case Conversions.InToCm:
            this.convertRatio = 0.393701;
            break;
          default:
            this.convertRatio = 1.0;
            break;
        }
      }
      else
      {
        switch (c)
        {
          case Conversions.KgLbToMetricTonShortTon:
            this.convertRatio = 0.001;
            break;
          case Conversions.KgLbToKgLb:
            this.convertRatio = 1.0;
            break;
          case Conversions.SqFtSqMToAcreHectare:
            this.convertRatio = 0.0001;
            break;
          case Conversions.SqFtSqMToSqFtSqM:
            this.convertRatio = 1.0;
            break;
          case Conversions.SqInSqCmToSqFtSqM:
            this.convertRatio = 0.0001;
            break;
          default:
            this.convertRatio = 1.0;
            break;
        }
      }
    }
  }
}
