// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.Chart.HourlyVOCResults
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util;

namespace i_Tree_Eco_v6.Reports.Chart
{
  internal class HourlyVOCResults : VOCDateValueChart
  {
    public HourlyVOCResults(string item, string category, string title)
      : base(item, category, Units.Grams)
    {
      this.metricUnits = ReportUtil.GetValuePerHrStr(i_Tree_Eco_v6.Resources.Strings.UnitGramsAbbr);
      this.englishUnits = ReportUtil.GetValuePerHrStr(i_Tree_Eco_v6.Resources.Strings.UnitOuncesAbbr);
      this.XAxisTitle = i_Tree_Eco_v6.Resources.Strings.Date;
      this.TableXTitle = ReportUtil.GetValuePerValueStr(i_Tree_Eco_v6.Resources.Strings.Date, i_Tree_Eco_v6.Resources.Strings.Time);
      this.MainTitle = title;
    }
  }
}
