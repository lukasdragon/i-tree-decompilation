// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PestSignSymptomDetailsSummarybyLanduses
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
  internal class PestSignSymptomDetailsSummarybyLanduses : BasicReport
  {
    private int _curStrata;
    private bool _isSample;
    private PestData _pestData = new PestData();

    public PestSignSymptomDetailsSummarybyLanduses(short CurStrata, bool isSample)
    {
      this._curStrata = (int) CurStrata;
      this._isSample = isSample;
      this.ReportTitle = string.Format("{0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.SignsAndSymptomsSummaries, CurStrata == (short) -1 ? (object) i_Tree_Eco_v6.Resources.Strings.ByStratum : (object) string.Format("{0}: {1}", (object) i_Tree_Eco_v6.Resources.Strings.ForStratum, (object) staticData.GetClassValueName(Classifiers.Strata, CurStrata)));
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.RenderHeader(C1doc);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      LineDef lineDef = new LineDef((Unit) "1pt", Color.Black);
      renderTable.Rows[0].Style.Borders.Bottom = lineDef;
      renderTable.Rows[0].Style.TextAlignVert = AlignVertEnum.Bottom;
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      renderTable.Cols[2].Width = (Unit) "20%";
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.SignSymptomTypeLocation;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SignSymptom;
      renderTable.Cells[0, 3].Text = this._isSample ? i_Tree_Eco_v6.Resources.Strings.PopulationEstimate : i_Tree_Eco_v6.Resources.Strings.TreeCount;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfStratum;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllTrees;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPestAffectedTrees;
      Dictionary<int, Dictionary<string, Dictionary<string, PestStats>>> summaryDictionary = this._pestData.GetEstLanduseSignAndSymptomDetailsSummaryDictionary();
      double estTotalAffected = this._pestData.GetEstTotalAffected();
      double estAllTreeCount = this._pestData.GetEstAllTreeCount();
      int row = 1;
      foreach (KeyValuePair<int, Dictionary<string, Dictionary<string, PestStats>>> keyValuePair1 in summaryDictionary)
      {
        if (this._curStrata == -1 || this._curStrata == keyValuePair1.Key)
        {
          double estTotalLanduse = this._pestData.GetEstTotalLanduse(keyValuePair1.Key);
          string classValueName = staticData.GetClassValueName(Classifiers.Strata, Convert.ToInt16(keyValuePair1.Key));
          renderTable.Cells[row, 0].Text = classValueName;
          foreach (KeyValuePair<string, Dictionary<string, PestStats>> keyValuePair2 in keyValuePair1.Value)
          {
            renderTable.Cells[row, 1].Text = keyValuePair2.Key;
            foreach (KeyValuePair<string, PestStats> keyValuePair3 in keyValuePair2.Value)
            {
              renderTable.Cells[row, 2].Text = keyValuePair3.Key;
              TableCell cell1 = renderTable.Cells[row, 3];
              double num = Math.Round(keyValuePair3.Value.PopEst, 0);
              string str1 = num.ToString("#,0");
              cell1.Text = str1;
              TableCell cell2 = renderTable.Cells[row, 4];
              num = keyValuePair3.Value.PopEst / estTotalLanduse * 100.0;
              string str2 = num.ToString("0.00");
              cell2.Text = str2;
              TableCell cell3 = renderTable.Cells[row, 5];
              num = keyValuePair3.Value.PopEst / estAllTreeCount * 100.0;
              string str3 = num.ToString("0.00");
              cell3.Text = str3;
              TableCell cell4 = renderTable.Cells[row, 6];
              num = keyValuePair3.Value.PopEst / estTotalAffected * 100.0;
              string str4 = num.ToString("0.00");
              cell4.Text = str4;
              ++row;
            }
          }
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.RenderFooter(C1doc);
    }
  }
}
