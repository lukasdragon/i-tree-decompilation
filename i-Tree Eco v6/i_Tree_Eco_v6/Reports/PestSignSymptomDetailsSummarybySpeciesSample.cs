// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PestSignSymptomDetailsSummarybySpeciesSample
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
  internal class PestSignSymptomDetailsSummarybySpeciesSample : BasicReport
  {
    private int _curSpecies;
    private PestData _pestData;

    public PestSignSymptomDetailsSummarybySpeciesSample(short curSpecies)
    {
      this.ReportTitle = curSpecies == (short) -1 ? i_Tree_Eco_v6.Resources.Strings.ReportTitleSignsAndSymptomsSummariesBySpecies : string.Format("{0}: {1}", (object) i_Tree_Eco_v6.Resources.Strings.ReportTitleSignsAndSymptomsSummariesForSpecies, staticData.UseScientificName ? (object) staticData.GetClassValueName1(Classifiers.Species, curSpecies) : (object) staticData.GetClassValueName(Classifiers.Species, curSpecies));
      this._pestData = new PestData();
      this._curSpecies = (int) curSpecies;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.RenderHeader(C1doc);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cols[1].Width = (Unit) "14%";
      renderTable.Cols[3].Width = (Unit) "12%";
      renderTable.Cols[4].Width = (Unit) "10%";
      renderTable.Cols[5].Width = (Unit) "12%";
      renderTable.Cols[6].Width = (Unit) "12%";
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
      DataTable table = staticData.DsData.Tables["Species_PestPest"];
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.SignSymptomTypeLocation;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SignSymptom;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.PopulationEstimate;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfSpecies;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllTrees;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPestAffectedTrees;
      double estAllTreeCount = this._pestData.GetEstAllTreeCount();
      double estTotalAffected1 = this._pestData.GetEstTotalAffected();
      Dictionary<int, Dictionary<string, Dictionary<string, PestStats>>> summaryDictionary = this._pestData.GetEstSpeciesSignAndSymptomDetailsSummaryDictionary();
      double estTotalAffected2 = this._pestData.GetEstTotalAffected();
      int num1 = count;
      foreach (KeyValuePair<int, Dictionary<string, Dictionary<string, PestStats>>> keyValuePair1 in summaryDictionary)
      {
        if (this._curSpecies == -1 || keyValuePair1.Key == this._curSpecies)
        {
          double estTotalSpecies = this._pestData.GetEstTotalSpecies(Convert.ToInt16(keyValuePair1.Key));
          short int16 = Convert.ToInt16(keyValuePair1.Key);
          string str1 = staticData.UseScientificName ? staticData.GetClassValueName1(Classifiers.Species, int16) : staticData.GetClassValueName(Classifiers.Species, int16);
          renderTable.Cells[num1, 0].Text = str1;
          foreach (KeyValuePair<string, Dictionary<string, PestStats>> keyValuePair2 in keyValuePair1.Value)
          {
            renderTable.Cells[num1, 1].Text = keyValuePair2.Key;
            foreach (KeyValuePair<string, PestStats> keyValuePair3 in keyValuePair2.Value)
            {
              renderTable.Cells[num1, 2].Text = keyValuePair3.Key;
              renderTable.Cells[num1, 3].Text = keyValuePair3.Value.PopEst.ToString("#,0");
              renderTable.Cells[num1, 4].Text = (keyValuePair3.Value.PopEst / estTotalSpecies * 100.0).ToString("0.00");
              TableCell cell1 = renderTable.Cells[num1, 5];
              double num2 = keyValuePair3.Value.PopEst / estAllTreeCount * 100.0;
              string str2 = num2.ToString("0.00");
              cell1.Text = str2;
              TableCell cell2 = renderTable.Cells[num1, 6];
              num2 = keyValuePair3.Value.PopEst / estTotalAffected2 * 100.0;
              string str3 = num2.ToString("0.00");
              cell2.Text = str3;
              ++num1;
            }
          }
          PestStats speciesAffectedStats = this._pestData.GetEstSpeciesAffectedStats(keyValuePair1.Key);
          renderTable.Cells[num1, 2].Text = i_Tree_Eco_v6.Resources.Strings.Total;
          renderTable.Cells[num1, 3].Text = speciesAffectedStats.PopEst.ToString("#,0");
          renderTable.Cells[num1, 4].Text = (speciesAffectedStats.PopEst / estTotalSpecies * 100.0).ToString("0.00");
          renderTable.Cells[num1, 5].Text = (speciesAffectedStats.PopEst / estAllTreeCount * 100.0).ToString("0.00");
          renderTable.Cells[num1, 6].Text = (speciesAffectedStats.PopEst / estTotalAffected1 * 100.0).ToString("0.00");
          renderTable.Rows[num1].Style.FontBold = true;
          renderTable.Rows[num1].Style.Borders.Top = LineDef.Default;
          renderTable.Rows[num1].Style.Borders.Bottom = LineDef.Default;
          ++num1;
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.RenderFooter(C1doc);
    }
  }
}
