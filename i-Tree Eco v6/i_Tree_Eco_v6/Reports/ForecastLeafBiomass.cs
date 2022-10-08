// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastLeafBiomass
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.v6;
using System;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class ForecastLeafBiomass : ForecastSimpleReport
  {
    public ForecastLeafBiomass()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleLeafBiomassOverTime;
      this.ForecastedYear_DataValue_FromCohortResults(CohortResultDataType.LeafBiomass);
    }

    public override void InitDocument(C1PrintDocument C1doc)
    {
      base.InitDocument(C1doc);
      double num = 0.0;
      if (this.results.Count > 0)
        num = this.results.Average<CohortResult>((Func<CohortResult, double>) (x => x.DataValue));
      if (this.curYear.Unit == YearUnit.English)
        num *= 0.45359236;
      if (num > 100.0 * this.thousand)
      {
        this.dataValueHeading = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.TotalLeafBiomass, ReportBase.TonnesUnits());
        this.SetConvertRatio(Conversions.KgLbToMetricTonShortTon);
        this.dataValueTableFormatString = "N1";
      }
      else if (num > this.thousand)
      {
        this.dataValueHeading = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.TotalLeafBiomass, ReportBase.TonnesUnits());
        this.SetConvertRatio(Conversions.KgLbToMetricTonShortTon);
        this.dataValueTableFormatString = "N2";
      }
      else
      {
        this.dataValueHeading = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.TotalLeafBiomass, ReportBase.KilogramsUnits());
        this.SetConvertRatio(Conversions.KgLbToKgLb);
        this.dataValueTableFormatString = "N1";
      }
    }
  }
}
