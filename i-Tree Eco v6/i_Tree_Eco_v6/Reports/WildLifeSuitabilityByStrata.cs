// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.WildLifeSuitabilityByStrata
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using System.Collections.Generic;

namespace i_Tree_Eco_v6.Reports
{
  public class WildLifeSuitabilityByStrata : WildLifeSuitability
  {
    public WildLifeSuitabilityByStrata()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleWildlifeSuitabilityByStratum;
      this.reportBy = v6Strings.Strata_SingularName;
    }

    protected override void PopulateReportBody(RenderTable rTable)
    {
      int tableRow = this.PopulateReportData(rTable, this.SuitabilityIndexByStrata, this.headerRows);
      this.PopulateReportDataCombined(rTable, this.SuitabilityIndexByStrataForTotal, tableRow, i_Tree_Eco_v6.Resources.Strings.StudyArea);
    }

    protected int PopulateReportData(
      RenderTable rTable,
      List<StrataWildLifeSuitability> list,
      int tableRow)
    {
      string unifier = string.Empty;
      foreach (StrataWildLifeSuitability plot in list)
      {
        ++tableRow;
        if (unifier.Equals(plot.StrataDesc))
        {
          this.PopulateHydrologyRow(rTable, tableRow, (WildLifeSuitabilityBase) plot, string.Empty);
        }
        else
        {
          unifier = plot.StrataDesc;
          this.PopulateHydrologyRow(rTable, tableRow, (WildLifeSuitabilityBase) plot, unifier);
        }
      }
      return tableRow;
    }

    protected int PopulateReportDataCombined(
      RenderTable rTable,
      List<StrataWildLifeSuitability> list,
      int tableRow,
      string unifier = "")
    {
      this.PopulateHydrologyRow(rTable, tableRow, (WildLifeSuitabilityBase) list[0], unifier);
      for (int index = 1; index < list.Count; ++index)
      {
        ++tableRow;
        this.PopulateHydrologyRow(rTable, tableRow, (WildLifeSuitabilityBase) list[index], string.Empty);
      }
      return tableRow;
    }
  }
}
