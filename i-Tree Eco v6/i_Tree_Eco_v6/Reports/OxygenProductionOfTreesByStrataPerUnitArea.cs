// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.OxygenProductionOfTreesByStrataPerUnitArea
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util;
using System;

namespace i_Tree_Eco_v6.Reports
{
  internal class OxygenProductionOfTreesByStrataPerUnitArea : OxygenProductionOfTreesBase
  {
    public OxygenProductionOfTreesByStrataPerUnitArea()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleOxygenProductionOfTreesByStratumPerUnitArea;
      this.header = i_Tree_Eco_v6.Resources.Strings.OxygenProductionDensity;
      this.headerUnits = ReportUtil.GetValuePerValueStr(ReportUtil.GetValuePerYrStr(ReportBase.KgUnits()), ReportBase.HaUnits());
      this.units = Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.Year, Units.Hectare);
    }
  }
}
