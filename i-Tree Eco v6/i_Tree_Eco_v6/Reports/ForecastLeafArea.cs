// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastLeafArea
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.v6;
using System;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class ForecastLeafArea : ForecastSimpleReport
  {
    public ForecastLeafArea()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleLeafAreaOverTime;
      this.ForecastedYear_DataValue_FromCohortResults(CohortResultDataType.LeafArea);
    }

    public override void InitDocument(C1PrintDocument C1doc)
    {
      base.InitDocument(C1doc);
      double num = 0.0;
      if (this.results.Count > 0)
        num = this.results.Average<CohortResult>((Func<CohortResult, double>) (x => x.DataValue));
      if (this.curYear.Unit == YearUnit.English)
        num *= 0.09290304;
      if (num > 100.0 * this.tenThousands)
      {
        this.dataValueHeading = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.TotalLeafArea, ReportBase.HaUnits());
        this.SetConvertRatio(Conversions.SqFtSqMToAcreHectare);
        this.dataValueTableFormatString = "N1";
      }
      else if (num > this.tenThousands)
      {
        this.dataValueHeading = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.TotalLeafArea, ReportBase.HaUnits());
        this.SetConvertRatio(Conversions.SqFtSqMToAcreHectare);
        this.dataValueTableFormatString = "N2";
      }
      else
      {
        this.dataValueHeading = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.TotalLeafArea, this.SquareMeterUnits());
        this.SetConvertRatio(Conversions.SqFtSqMToSqFtSqM);
        this.dataValueTableFormatString = "N0";
      }
    }
  }
}
