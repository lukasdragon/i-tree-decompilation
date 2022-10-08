// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PestSignAndSymptomOverviewBySpeciesInventory
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using EcoIpedReportGenerator;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class PestSignAndSymptomOverviewBySpeciesInventory : BasicReport
  {
    private PestData _pestData;

    public PestSignAndSymptomOverviewBySpeciesInventory()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSignAndSymptomTotalsBySpecies;
      this._pestData = new PestData();
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.RenderHeader(C1doc);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      renderTable.Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Rows[0].Style.TextAlignVert = AlignVertEnum.Bottom;
      renderTable.Cols[0].Width = (Unit) "25%";
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeCount;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfSpecies;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllTrees;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPestAffectedTrees;
      int allTreeCount = this._pestData.GetAllTreeCount();
      int affectedTreeCount = this._pestData.GetPestAffectedTreeCount();
      Dictionary<string, List<DataRow>> speciesDictionaryList1 = this._pestData.GetAffectedSpeciesDictionaryList();
      Dictionary<string, List<DataRow>> speciesDictionaryList2 = this._pestData.GetAllSpeciesDictionaryList();
      int row = 1;
      foreach (KeyValuePair<string, List<DataRow>> keyValuePair in speciesDictionaryList1)
      {
        renderTable.Cells[row, 0].Text = this.GetSpecies(keyValuePair.Key);
        renderTable.Cells[row, 1].Text = keyValuePair.Value.Count.ToString();
        TableCell cell1 = renderTable.Cells[row, 2];
        double num = (double) keyValuePair.Value.Count / (double) speciesDictionaryList2[keyValuePair.Key].Count;
        string str1 = num.ToString("0.00");
        cell1.Text = str1;
        TableCell cell2 = renderTable.Cells[row, 3];
        num = (double) keyValuePair.Value.Count / (double) allTreeCount;
        string str2 = num.ToString("0.00");
        cell2.Text = str2;
        TableCell cell3 = renderTable.Cells[row, 4];
        num = (double) keyValuePair.Value.Count / (double) affectedTreeCount;
        string str3 = num.ToString("0.00");
        cell3.Text = str3;
        ++row;
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.RenderFooter(C1doc);
    }
  }
}
