// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PestSignSymptomDetailsCompletebySpeciesSample
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
  internal class PestSignSymptomDetailsCompletebySpeciesSample : BasicReport
  {
    private short CurSpecies;
    private PestData _pestData;

    public PestSignSymptomDetailsCompletebySpeciesSample(short curSpecies)
    {
      this.ReportTitle = curSpecies == (short) -1 ? i_Tree_Eco_v6.Resources.Strings.ReportTitleSignsAndSymptomsDetailsBySpecies : string.Format("{0}: {1}", (object) i_Tree_Eco_v6.Resources.Strings.ReportTitleSignsAndSymptomsDetailsForSpecies, staticData.UseScientificName ? (object) staticData.GetClassValueName1(Classifiers.Species, curSpecies) : (object) staticData.GetClassValueName(Classifiers.Species, curSpecies));
      this.CurSpecies = curSpecies;
      this._pestData = new PestData();
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.RenderHeader(C1doc);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cols[0].Width = (Unit) "13%";
      renderTable.Cols[1].Width = (Unit) "12%";
      renderTable.Cols[2].Width = (Unit) "14%";
      renderTable.Cols[4].Width = (Unit) "10%";
      renderTable.Cols[5].Width = (Unit) "5%";
      renderTable.Cols[6].Width = (Unit) "7%";
      renderTable.Cols[7].Width = (Unit) "7%";
      renderTable.Cols[8].Width = (Unit) "8%";
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[3].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.SignSymptomTypeLocation;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SignSymptom;
      renderTable.Cells[0, 3].Text = v6Strings.Condition_SingularName;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PopulationEstimate;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfSpecies;
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllTrees;
      renderTable.Cells[0, 8].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPestAffectedTrees;
      double estAllTreeCount1 = this._pestData.GetEstAllTreeCount();
      double estTotalAffected1 = this._pestData.GetEstTotalAffected();
      Dictionary<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> completeDictionary = this._pestData.GetEstSpeciesSignAndSymptomDetailsCompleteDictionary();
      double estTotalAffected2 = this._pestData.GetEstTotalAffected();
      double estAllTreeCount2 = this._pestData.GetEstAllTreeCount();
      int num = count;
      foreach (KeyValuePair<int, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> keyValuePair1 in completeDictionary)
      {
        if (this.CurSpecies == (short) -1 || keyValuePair1.Key == (int) this.CurSpecies)
        {
          short int16 = Convert.ToInt16(keyValuePair1.Key);
          string str = staticData.UseScientificName ? staticData.GetClassValueName1(Classifiers.Species, int16) : staticData.GetClassValueName(Classifiers.Species, int16);
          renderTable.Cells[num, 0].Text = str;
          double estTotalSpecies = this._pestData.GetEstTotalSpecies(Convert.ToInt16(keyValuePair1.Key));
          foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, PestStats>>> keyValuePair2 in keyValuePair1.Value)
          {
            renderTable.Cells[num, 1].Text = keyValuePair2.Key;
            foreach (KeyValuePair<string, Dictionary<string, PestStats>> keyValuePair3 in keyValuePair2.Value)
            {
              renderTable.Cells[num, 2].Text = keyValuePair3.Key;
              foreach (KeyValuePair<string, PestStats> keyValuePair4 in keyValuePair3.Value)
              {
                renderTable.Cells[num, 3].Text = keyValuePair4.Key;
                renderTable.Cells[num, 4].Text = Math.Round(keyValuePair4.Value.PopEst, 0).ToString("#,0");
                renderTable.Cells[num, 5].Text = Math.Round(keyValuePair4.Value.StdErr, 0).ToString("#");
                renderTable.Cells[num, 6].Text = (keyValuePair4.Value.PopEst / estTotalSpecies * 100.0).ToString("0.00");
                renderTable.Cells[num, 7].Text = (keyValuePair4.Value.PopEst / estAllTreeCount2 * 100.0).ToString("0.00");
                renderTable.Cells[num, 8].Text = (keyValuePair4.Value.PopEst / estTotalAffected2 * 100.0).ToString("0.00");
                ++num;
              }
            }
          }
          PestStats speciesAffectedStats = this._pestData.GetEstSpeciesAffectedStats(keyValuePair1.Key);
          renderTable.Cells[num, 3].Text = i_Tree_Eco_v6.Resources.Strings.Total;
          renderTable.Cells[num, 4].Text = Math.Round(speciesAffectedStats.PopEst, 0).ToString("#,0");
          renderTable.Cells[num, 6].Text = (speciesAffectedStats.PopEst / estTotalSpecies * 100.0).ToString("0.00");
          renderTable.Cells[num, 7].Text = (speciesAffectedStats.PopEst / estAllTreeCount1 * 100.0).ToString("0.00");
          renderTable.Cells[num, 8].Text = (speciesAffectedStats.PopEst / estTotalAffected1 * 100.0).ToString("0.00");
          renderTable.Rows[num].Style.FontBold = true;
          renderTable.Rows[num].Style.Borders.Top = renderTable.Rows[num].Style.Borders.Bottom = LineDef.Default;
          ++num;
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.RenderFooter(C1doc);
    }
  }
}
