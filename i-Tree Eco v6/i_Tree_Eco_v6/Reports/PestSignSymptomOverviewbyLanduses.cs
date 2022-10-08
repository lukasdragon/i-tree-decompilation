// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PestSignSymptomOverviewbyLanduses
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Util;
using EcoIpedReportGenerator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class PestSignSymptomOverviewbyLanduses : BasicReport
  {
    private bool _isSample;

    public PestSignSymptomOverviewbyLanduses(bool isSample)
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSignsAndSymptomsTotalsByStratum;
      this._isSample = isSample;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.RenderHeader(C1doc);
      DataTable table = staticData.DsData.Tables["Landuse_PestPest"];
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Rows[0].Style.TextAlignVert = AlignVertEnum.Bottom;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      renderTable.Width = (Unit) "95%";
      if (!this._isSample)
        renderTable.Cols[2].Visible = false;
      renderTable.Cols[0].Width = (Unit) "17%";
      renderTable.Cols[1].Width = (Unit) "15%";
      renderTable.Cols[2].Width = (Unit) "10%";
      renderTable.Cols[3].Width = (Unit) "13%";
      renderTable.Cols[4].Width = (Unit) "13%";
      renderTable.Cols[5].Width = (Unit) "17%";
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = this._isSample ? i_Tree_Eco_v6.Resources.Strings.PopulationEstimate : i_Tree_Eco_v6.Resources.Strings.TreeCount;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfStratum;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllTrees;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPestAffectedTrees;
      Dictionary<int, double> dictionary = new Dictionary<int, double>();
      double num1 = (double) table.Compute("SUM(EstimateValue)", "PestPest = " + staticData.PestPestAffectedClassValueOrder.ToString() + " OR PestPest =" + staticData.PestPestNoneClassValueOrder.ToString());
      double num2 = (double) table.Compute("SUM(EstimateValue)", "PestPest = " + staticData.PestPestAffectedClassValueOrder.ToString());
      foreach (DataRowView dataRowView in new DataView(table, "PestPest = " + staticData.PestPestAffectedClassValueOrder.ToString() + " OR PestPest =" + staticData.PestPestNoneClassValueOrder.ToString(), string.Empty, DataViewRowState.CurrentRows))
      {
        int int32 = Convert.ToInt32(dataRowView["Strata"]);
        if (!dictionary.ContainsKey(int32))
          dictionary.Add(int32, 0.0);
        dictionary[int32] += (double) dataRowView["EstimateValue"];
      }
      int row = 1;
      foreach (DataRowView dataRowView in new DataView(table, "PestPest=" + staticData.PestPestAffectedClassValueOrder.ToString(), string.Empty, DataViewRowState.CurrentRows))
      {
        int int32 = Convert.ToInt32(dataRowView["Strata"]);
        renderTable.Cells[row, 0].Text = staticData.GetClassValueName(Classifiers.Strata, Convert.ToInt16(dataRowView["Strata"]));
        renderTable.Cells[row, 1].Text = Math.Round((double) dataRowView["EstimateValue"], 0).ToString("#,0");
        if (this._isSample)
          renderTable.Cells[row, 2].Text = Math.Round((double) dataRowView["EstimateStandardError"], 0).ToString("#,0");
        renderTable.Cells[row, 3].Text = ((double) dataRowView["EstimateValue"] / dictionary[int32] * 100.0).ToString("0.00");
        renderTable.Cells[row, 4].Text = ((double) dataRowView["EstimateValue"] / num1 * 100.0).ToString("0.00");
        renderTable.Cells[row, 5].Text = ((double) dataRowView["EstimateValue"] / num2 * 100.0).ToString("0.00");
        ++row;
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.RenderFooter(C1doc);
    }
  }
}
