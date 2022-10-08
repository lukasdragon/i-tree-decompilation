// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastBasalArea
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.v6;

namespace i_Tree_Eco_v6.Reports
{
  public class ForecastBasalArea : ForecastSimpleReport
  {
    public ForecastBasalArea()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleBasalAreaOverTime;
      this.ForecastedYear_DataValue_FromCohortResults(CohortResultDataType.BasalArea);
    }

    public override void InitDocument(C1PrintDocument C1doc)
    {
      base.InitDocument(C1doc);
      this.dataValueHeading = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.TotalBasalArea, this.SquareMeterUnits());
      this.SetConvertRatio(Conversions.SqInSqCmToSqFtSqM);
    }
  }
}
