// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.Chart.HourlyHydroResults
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.Properties;
using Eco.Util;

namespace i_Tree_Eco_v6.Reports.Chart
{
  internal class HourlyHydroResults : DateValueChart
  {
    public HourlyHydroResults(string colName, string type, string title)
    {
      this.item1 = colName;
      this.item2 = this.item1 + "Volume";
      this.curUnit1 = Units.Centimeters;
      this.curUnit2 = Units.CubicMeter;
      this.valueMultiplier1 = 100.0;
      this.valueMultiplier2 = 1.0;
      this.category = type;
      this.metricUnits1 = ReportUtil.GetValuePerHrStr(i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr);
      this.englishUnits1 = ReportUtil.GetValuePerHrStr(i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr);
      this.metricUnits2 = ReportUtil.GetValuePerHrStr(i_Tree_Eco_v6.Resources.Strings.UnitCubicMetersAbbr);
      this.englishUnits2 = ReportUtil.GetValuePerHrStr(i_Tree_Eco_v6.Resources.Strings.UnitCubicFeetAbbr);
      string date = i_Tree_Eco_v6.Resources.Strings.Date;
      string depth = i_Tree_Eco_v6.Resources.Strings.Depth;
      string volume = i_Tree_Eco_v6.Resources.Strings.Volume;
      this.MainTitle = ReportUtil.GetValueByValue(title, type == "T" ? v6Strings.Tree_PluralName : v6Strings.Shrub_PluralName);
      this.XAxisTitle = date;
      this.YAxisTitle = depth;
      this.YAxisTitle2 = volume;
      this.TableXTitle = ReportUtil.GetValuePerValueStr(date, i_Tree_Eco_v6.Resources.Strings.Time);
      this.TableYTitle = depth;
      this.TableY2Title = volume;
    }
  }
}
