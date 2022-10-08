// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.WildLifeSuitabilityByPlots
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;

namespace i_Tree_Eco_v6.Reports
{
  public class WildLifeSuitabilityByPlots : WildLifeSuitability
  {
    public WildLifeSuitabilityByPlots()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleWildlifeSuitabilityByPlot;
      this.reportBy = i_Tree_Eco_v6.Resources.Strings.PlotID;
    }

    protected override void PopulateReportBody(RenderTable rTable)
    {
      rTable.Cols[0].Width = (Unit) "6%";
      int headerRows = this.headerRows;
      string empty = string.Empty;
      foreach (PlotWildLifeSuitability suitabilityIndexByPlot in this.SuitabilityIndexByPlots)
      {
        ++headerRows;
        if (empty.Equals(suitabilityIndexByPlot.PlotID.ToString()))
        {
          this.PopulateHydrologyRow(rTable, headerRows, (WildLifeSuitabilityBase) suitabilityIndexByPlot, string.Empty);
        }
        else
        {
          empty = suitabilityIndexByPlot.PlotID.ToString();
          this.PopulateHydrologyRow(rTable, headerRows, (WildLifeSuitabilityBase) suitabilityIndexByPlot, empty);
        }
      }
    }
  }
}
