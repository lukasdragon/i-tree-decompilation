// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PrimaryPestImpactsByStrata
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
  internal class PrimaryPestImpactsByStrata : BasicReport
  {
    private int _curStrata;
    private bool _isSample;

    public PrimaryPestImpactsByStrata(short curStrata, bool isSample)
    {
      this.ReportTitle = curStrata == (short) -1 ? i_Tree_Eco_v6.Resources.Strings.ReportTitlePrimaryPestImpactsByStratum : string.Format("{0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.ReportTitlePrimaryPestImpactsForStratum, (object) staticData.GetClassValueName(Classifiers.Strata, curStrata));
      this._curStrata = (int) curStrata;
      this._isSample = isSample;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.RenderHeader(C1doc);
      DataTable table = staticData.DsData.Tables["Landuse_PestPest"];
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      LineDef lineDef = new LineDef((Unit) "1pt", Color.Black);
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Rows[0].Style.TextAlignVert = AlignVertEnum.Bottom;
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      if (!this._isSample)
        renderTable.Cols[3].Visible = false;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.PrimaryPest;
      renderTable.Cells[0, 2].Text = this._isSample ? i_Tree_Eco_v6.Resources.Strings.PopulationEstimate : i_Tree_Eco_v6.Resources.Strings.TreeCount;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfStratum;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllTrees;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPestAffectedTrees;
      Dictionary<int, Dictionary<int, PestStats>> dictionary1 = new Dictionary<int, Dictionary<int, PestStats>>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        int int32_1 = Convert.ToInt32(row["Strata"]);
        int int32_2 = Convert.ToInt32(row["PestPest"]);
        if (!dictionary1.ContainsKey(int32_1))
          dictionary1.Add(int32_1, new Dictionary<int, PestStats>());
        if (!dictionary1[int32_1].ContainsKey(int32_2))
          dictionary1[int32_1].Add(int32_2, new PestStats());
        dictionary1[int32_1][int32_2] = new PestStats((int) Math.Round((double) row["EstimateValue"], 0), (double) row["EstimateStandardError"], 0.0, 0.0, 0.0);
      }
      double a1 = (double) table.Compute("SUM(EstimateValue)", "PestPest = " + staticData.PestPestAffectedClassValueOrder.ToString());
      double num1 = (double) table.Compute("SUM(EstimateValue)", "PestPest = " + staticData.PestPestNoneClassValueOrder.ToString() + " OR PestPest = " + staticData.PestPestAffectedClassValueOrder.ToString());
      double a2 = 0.0;
      int num2 = 1;
      foreach (KeyValuePair<int, Dictionary<int, PestStats>> keyValuePair1 in dictionary1)
      {
        double popEst = keyValuePair1.Value[staticData.PestPestAffectedClassValueOrder].PopEst;
        double num3 = keyValuePair1.Value[staticData.PestPestNoneClassValueOrder].PopEst + keyValuePair1.Value[staticData.PestPestAffectedClassValueOrder].PopEst;
        renderTable.Cells[num2, 0].Text = staticData.GetClassValueName(Classifiers.Strata, Convert.ToInt16(keyValuePair1.Key));
        renderTable.Cells[num2, 0].Style.FontBold = true;
        foreach (KeyValuePair<int, PestStats> keyValuePair2 in keyValuePair1.Value)
        {
          if (keyValuePair2.Key != staticData.PestPestNoneClassValueOrder && keyValuePair2.Key != staticData.PestPestAffectedClassValueOrder && (keyValuePair1.Key == this._curStrata || this._curStrata == -1))
          {
            short int16 = Convert.ToInt16(keyValuePair2.Key);
            renderTable.Cells[num2, 1].Text = staticData.UseScientificName ? staticData.GetClassValueName1(Classifiers.PestPest, int16) : staticData.GetClassValueName(Classifiers.PestPest, int16);
            renderTable.Cells[num2, 2].Text = Math.Round(keyValuePair2.Value.PopEst, 0).ToString("#,0");
            double num4;
            if (this._isSample)
            {
              TableCell cell = renderTable.Cells[num2, 3];
              num4 = Math.Round(keyValuePair2.Value.StdErr, 0);
              string str = num4.ToString("#,0");
              cell.Text = str;
            }
            TableCell cell1 = renderTable.Cells[num2, 4];
            num4 = keyValuePair2.Value.PopEst / num3 * 100.0;
            string str1 = num4.ToString("0.00");
            cell1.Text = str1;
            TableCell cell2 = renderTable.Cells[num2, 5];
            num4 = keyValuePair2.Value.PopEst / num1 * 100.0;
            string str2 = num4.ToString("0.00");
            cell2.Text = str2;
            TableCell cell3 = renderTable.Cells[num2, 6];
            num4 = keyValuePair2.Value.PopEst / a1 * 100.0;
            string str3 = num4.ToString("0.00");
            cell3.Text = str3;
            a2 += keyValuePair2.Value.PopEst / a1 * 100.0;
            ++num2;
          }
        }
      }
      renderTable.Rows[num2].Style.Borders.Top = lineDef;
      renderTable.Rows[num2].Style.Borders.Bottom = lineDef;
      renderTable.Rows[num2].Style.FontBold = true;
      renderTable.Cells[num2, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      renderTable.Cells[num2, 1].Text = string.Empty;
      if (this._curStrata == -1)
      {
        renderTable.Cells[num2, 2].Text = Math.Round(a1).ToString("#,0");
        if (this._isSample)
          renderTable.Cells[num2, 3].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        renderTable.Cells[num2, 4].Text = (a1 / num1 * 100.0).ToString("#.00");
        renderTable.Cells[num2, 5].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        renderTable.Cells[num2, 6].Text = Math.Round(a2).ToString(".00");
      }
      else if (dictionary1.ContainsKey(this._curStrata))
      {
        Dictionary<int, PestStats> dictionary2 = dictionary1[this._curStrata];
        PestStats pestStats = dictionary2[staticData.PestPestAffectedClassValueOrder];
        double num5 = dictionary2[staticData.PestPestNoneClassValueOrder].PopEst + pestStats.PopEst;
        double popEst = pestStats.PopEst;
        renderTable.Cells[num2, 2].Text = Math.Round(pestStats.PopEst, 0).ToString("#,0");
        double num6;
        if (this._isSample)
        {
          TableCell cell = renderTable.Cells[num2, 3];
          num6 = Math.Round(pestStats.StdErr, 0);
          string str = num6.ToString("#,0");
          cell.Text = str;
        }
        TableCell cell4 = renderTable.Cells[num2, 4];
        num6 = Math.Round(popEst / num5 * 100.0, 2);
        string str4 = num6.ToString("0.00");
        cell4.Text = str4;
        TableCell cell5 = renderTable.Cells[num2, 5];
        num6 = Math.Round(pestStats.PopEst / num1 * 100.0, 2);
        string str5 = num6.ToString("0.##");
        cell5.Text = str5;
        TableCell cell6 = renderTable.Cells[num2, 6];
        num6 = Math.Round(a2);
        string str6 = num6.ToString(".00");
        cell6.Text = str6;
      }
      else
      {
        renderTable.Cells[num2, 2].Text = "0";
        if (this._isSample)
          renderTable.Cells[num2, 3].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        renderTable.Cells[num2, 4].Text = "0";
        renderTable.Cells[num2, 5].Text = "0";
        renderTable.Cells[num2, 6].Text = "0";
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.RenderFooter(C1doc);
    }
  }
}
