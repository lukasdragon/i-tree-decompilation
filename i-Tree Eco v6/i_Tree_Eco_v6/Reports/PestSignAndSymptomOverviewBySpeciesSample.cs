// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PestSignAndSymptomOverviewBySpeciesSample
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Util;
using EcoIpedReportGenerator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class PestSignAndSymptomOverviewBySpeciesSample : BasicReport
  {
    public PestSignAndSymptomOverviewBySpeciesSample() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSignAndSymptomTotalsBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.RenderHeader(C1doc);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      DataTable table = staticData.DsData.Tables["Species_PestPest"];
      renderTable.Rows[0].Style.TextAlignVert = AlignVertEnum.Bottom;
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      renderTable.Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Width = (Unit) "17%";
      renderTable.Cols[2].Width = (Unit) "10%";
      renderTable.Cols[3].Width = (Unit) "13%";
      renderTable.Cols[4].Width = (Unit) "13%";
      renderTable.Cols[5].Width = (Unit) "19%";
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.PopulationEstimate;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfSpecies;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllTrees;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPestAffectedTrees;
      Dictionary<int, double> dictionary = new Dictionary<int, double>();
      double num1 = (double) table.Compute("SUM(EstimateValue)", "PestPest=" + staticData.PestPestAffectedClassValueOrder.ToString() + " OR PestPest=" + staticData.PestPestNoneClassValueOrder.ToString());
      double num2 = (double) table.Compute("SUM(EstimateValue)", "PestPest=" + staticData.PestPestAffectedClassValueOrder.ToString());
      foreach (DataRowView dataRowView in new DataView(table, "PestPest=" + staticData.PestPestAffectedClassValueOrder.ToString() + " OR PestPest=" + staticData.PestPestNoneClassValueOrder.ToString(), string.Empty, DataViewRowState.CurrentRows))
      {
        int int32 = Convert.ToInt32(dataRowView["Species"]);
        if (!dictionary.ContainsKey(int32))
          dictionary.Add(int32, 0.0);
        dictionary[int32] += (double) dataRowView["EstimateValue"];
      }
      DataView dataView = new DataView(table, "PestPest=" + staticData.PestPestAffectedClassValueOrder.ToString(), "Species", DataViewRowState.CurrentRows);
      dataView.Sort = "Species";
      int row = 1;
      foreach (DataRowView dataRowView in dataView)
      {
        renderTable.Cells[row, 0].Text = staticData.UseScientificName ? staticData.GetClassValueName1(Classifiers.Species, Convert.ToInt16(dataRowView["Species"])) : staticData.GetClassValueName(Classifiers.Species, Convert.ToInt16(dataRowView["Species"]));
        TableCell cell1 = renderTable.Cells[row, 1];
        double num3 = Math.Round((double) dataRowView["EstimateValue"], 0);
        string str1 = num3.ToString("#,0");
        cell1.Text = str1;
        TableCell cell2 = renderTable.Cells[row, 2];
        num3 = Math.Round((double) dataRowView["EstimateStandardError"], 0);
        string str2 = num3.ToString("#,0");
        cell2.Text = str2;
        TableCell cell3 = renderTable.Cells[row, 3];
        num3 = (double) dataRowView["EstimateValue"] / dictionary[Convert.ToInt32(dataRowView["Species"])] * 100.0;
        string str3 = num3.ToString("0.00");
        cell3.Text = str3;
        TableCell cell4 = renderTable.Cells[row, 4];
        num3 = (double) dataRowView["EstimateValue"] / num1 * 100.0;
        string str4 = num3.ToString("0.00");
        cell4.Text = str4;
        TableCell cell5 = renderTable.Cells[row, 5];
        num3 = (double) dataRowView["EstimateValue"] / num2 * 100.0;
        string str5 = num3.ToString("0.00");
        cell5.Text = str5;
        ++row;
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.RenderFooter(C1doc);
    }
  }
}
