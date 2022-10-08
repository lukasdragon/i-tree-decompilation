// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PestSignSymptomDetailsByStratum
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Util;
using EcoIpedReportGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class PestSignSymptomDetailsByStratum : BasicReport
  {
    private PestData _pestData;
    private int CurLanduse;

    public PestSignSymptomDetailsByStratum(int curLanduse)
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSignsAndSymptomsDetailsByStratum;
      this._pestData = new PestData();
      this.CurLanduse = curLanduse;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.RenderHeader(C1doc);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      LineDef lineDef = new LineDef((Unit) "1pt", Color.Black);
      renderTable.Cols[3].Width = (Unit) "20%";
      renderTable.Cols[4].Width = (Unit) "9%";
      renderTable.Cols[5].Width = (Unit) "8%";
      renderTable.Cols[6].Width = (Unit) "8%";
      renderTable.Cols[7].Width = (Unit) "8%";
      renderTable.Cols[8].Width = (Unit) "12%";
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[3].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Rows[0].Style.TextAlignVert = AlignVertEnum.Bottom;
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TypeSlashLocation;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SignSymptom;
      renderTable.Cells[0, 3].Text = v6Strings.Condition_SingularName;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PopulationEstimate;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfStratum;
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllStrata;
      renderTable.Cells[0, 8].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPestAffectedTrees;
      double estAllTreeCount1 = this._pestData.GetEstAllTreeCount();
      double estTotalAffected1 = this._pestData.GetEstTotalAffected();
      Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> completeDictionary = this._pestData.GetEstLanduseSignAndSymptomDetailsCompleteDictionary();
      double estTotalAffected2 = this._pestData.GetEstTotalAffected();
      double estAllTreeCount2 = this._pestData.GetEstAllTreeCount();
      int num1 = 1;
      foreach (KeyValuePair<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> keyValuePair1 in completeDictionary)
      {
        if (this.CurLanduse == -1 || keyValuePair1.Key == this.CurLanduse)
        {
          string classValueName = staticData.GetClassValueName(Classifiers.Strata, Convert.ToInt16(keyValuePair1.Key));
          renderTable.Cells[num1, 0].Text = classValueName;
          double estTotalLanduse = this._pestData.GetEstTotalLanduse(keyValuePair1.Key);
          double num2;
          foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, PestStats>>> keyValuePair2 in keyValuePair1.Value)
          {
            renderTable.Cells[num1, 1].Text = keyValuePair2.Key;
            foreach (KeyValuePair<string, Dictionary<string, PestStats>> keyValuePair3 in keyValuePair2.Value)
            {
              renderTable.Cells[num1, 2].Text = keyValuePair3.Key;
              foreach (KeyValuePair<string, PestStats> keyValuePair4 in keyValuePair3.Value)
              {
                renderTable.Cells[num1, 3].Text = keyValuePair4.Key;
                TableCell cell1 = renderTable.Cells[num1, 4];
                num2 = Math.Round(keyValuePair4.Value.PopEst, 0);
                string str1 = num2.ToString("#,0");
                cell1.Text = str1;
                renderTable.Cells[num1, 5].Text = keyValuePair4.Value.StdErr.ToString("#,0");
                TableCell cell2 = renderTable.Cells[num1, 6];
                num2 = keyValuePair4.Value.PopEst / estTotalLanduse * 100.0;
                string str2 = num2.ToString("0.00");
                cell2.Text = str2;
                TableCell cell3 = renderTable.Cells[num1, 7];
                num2 = keyValuePair4.Value.PopEst / estAllTreeCount2 * 100.0;
                string str3 = num2.ToString("0.00");
                cell3.Text = str3;
                TableCell cell4 = renderTable.Cells[num1, 8];
                num2 = keyValuePair4.Value.PopEst / estTotalAffected2 * 100.0;
                string str4 = num2.ToString("0.00");
                cell4.Text = str4;
                ++num1;
              }
            }
          }
          PestStats landuseAffectedStats = this._pestData.GetEstLanduseAffectedStats(keyValuePair1.Key);
          renderTable.Cells[num1, 0].Text = classValueName;
          renderTable.Cells[num1, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreesAffected;
          renderTable.Cells[num1, 2].Text = i_Tree_Eco_v6.Resources.Strings.AllSymptoms;
          renderTable.Cells[num1, 4].Text = landuseAffectedStats.PopEst.ToString("#,0");
          TableCell cell5 = renderTable.Cells[num1, 6];
          num2 = landuseAffectedStats.PopEst / estTotalLanduse * 100.0;
          string str5 = num2.ToString("0.00");
          cell5.Text = str5;
          TableCell cell6 = renderTable.Cells[num1, 7];
          num2 = landuseAffectedStats.PopEst / estAllTreeCount1 * 100.0;
          string str6 = num2.ToString("0.00");
          cell6.Text = str6;
          TableCell cell7 = renderTable.Cells[num1, 8];
          num2 = landuseAffectedStats.PopEst / estTotalAffected1 * 100.0;
          string str7 = num2.ToString("0.00");
          cell7.Text = str7;
          renderTable.Rows[num1].Style.FontBold = true;
          renderTable.Rows[num1].Style.Borders.Top = lineDef;
          renderTable.Rows[num1].Style.Borders.Bottom = lineDef;
          ++num1;
        }
      }
      double estTotalAffected3 = this._pestData.GetEstTotalAffected();
      renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.TreesAffected;
      renderTable.Cells[num1, 4].Text = estTotalAffected3.ToString("#,0");
      renderTable.Rows[num1].Style.Borders.Top = lineDef;
      renderTable.Rows[num1].Style.FontBold = true;
      ReportUtil.FormatRenderTable(renderTable);
      this.RenderFooter(C1doc);
    }
  }
}
