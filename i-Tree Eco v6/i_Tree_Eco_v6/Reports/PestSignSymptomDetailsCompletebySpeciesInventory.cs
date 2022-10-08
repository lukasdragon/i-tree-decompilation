// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PestSignSymptomDetailsCompletebySpeciesInventory
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using EcoIpedReportGenerator;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class PestSignSymptomDetailsCompletebySpeciesInventory : BasicReport
  {
    private PestData _pestData;
    private string _curSpecies;

    public PestSignSymptomDetailsCompletebySpeciesInventory(string curSpecies)
    {
      this.ReportTitle = string.IsNullOrEmpty(curSpecies) ? i_Tree_Eco_v6.Resources.Strings.ReportTitleSignsAndSymptomsSummariesBySpecies : string.Format("{0}: {1}", (object) i_Tree_Eco_v6.Resources.Strings.ReportTitleSignsAndSymptomsSummariesForSpecies, (object) this.GetSpecies(curSpecies));
      this._pestData = new PestData();
      this._curSpecies = curSpecies;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.RenderHeader(C1doc);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[3].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[0].Width = (Unit) "13%";
      renderTable.Cols[1].Width = (Unit) "12%";
      renderTable.Cols[2].Width = (Unit) "14%";
      renderTable.Cols[4].Width = (Unit) "10%";
      renderTable.Cols[5].Width = (Unit) "7%";
      renderTable.Cols[6].Width = (Unit) "7%";
      renderTable.Cols[7].Width = (Unit) "8%";
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.SignSymptomTypeLocation;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SignSymptom;
      renderTable.Cells[0, 3].Text = v6Strings.Condition_SingularName;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.TreeCount;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfSpecies;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllTrees;
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPestAffectedTrees;
      int allTreeCount = this._pestData.GetAllTreeCount();
      int affectedTreeCount = this._pestData.GetPestAffectedTreeCount();
      Dictionary<string, List<DataRow>> speciesDictionaryList1 = this._pestData.GetAffectedSpeciesDictionaryList();
      Dictionary<string, List<DataRow>> speciesDictionaryList2 = this._pestData.GetAllSpeciesDictionaryList();
      Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> dictionary1 = new Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>>();
      Dictionary<string, Dictionary<string, PestStats>> dictionary2 = new Dictionary<string, Dictionary<string, PestStats>>();
      Dictionary<string, PestStats> dictionary3 = new Dictionary<string, PestStats>();
      foreach (KeyValuePair<string, List<DataRow>> keyValuePair in speciesDictionaryList2)
      {
        dictionary3[keyValuePair.Key] = new PestStats();
        dictionary1[keyValuePair.Key] = this._pestData.CreateSSDCDetailsDictionary();
        dictionary2[keyValuePair.Key] = this._pestData.CreateSSSummaryTotalDictionary();
        if (speciesDictionaryList1.ContainsKey(keyValuePair.Key))
        {
          foreach (DataRow dataRow in speciesDictionaryList1[keyValuePair.Key])
          {
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            if ((int) dataRow["PestTSDieback"] > 0)
            {
              flag1 = true;
              ++dictionary1[keyValuePair.Key]["Tree Stress"]["Dieback"][this._pestData.DctTSDieback[(int) dataRow["PestTSDieback"]]].TreeCount;
            }
            if ((int) dataRow["PestTSEpiSprout"] > 0)
            {
              flag1 = true;
              ++dictionary1[keyValuePair.Key]["Tree Stress"]["Epicormic Sprouts"][this._pestData.DctTSEpiSprout[(int) dataRow["PestTSEpiSprout"]]].TreeCount;
            }
            if ((int) dataRow["PestTSWiltFoli"] > 0)
            {
              flag1 = true;
              ++dictionary1[keyValuePair.Key]["Tree Stress"]["Wilted Foliage"][this._pestData.DctTSWiltFoli[(int) dataRow["PestTSWiltFoli"]]].TreeCount;
            }
            if ((int) dataRow["PestTSEnvStress"] > 0)
            {
              flag1 = true;
              ++dictionary1[keyValuePair.Key]["Tree Stress"]["Environmental Stress"][this._pestData.DctTSEnvStress[(int) dataRow["PestTSEnvStress"]]].TreeCount;
            }
            if ((int) dataRow["PestTSHumStress"] > 0)
            {
              flag1 = true;
              ++dictionary1[keyValuePair.Key]["Tree Stress"]["Human caused Stress"][this._pestData.DctTSHumStress[(int) dataRow["PestTSHumStress"]]].TreeCount;
            }
            if (!string.IsNullOrEmpty(dataRow["PestTSNotes"].ToString()))
            {
              flag1 = true;
              ++dictionary1[keyValuePair.Key]["Tree Stress"]["Notes Present"][string.Empty].TreeCount;
            }
            if ((int) dataRow["PestFTChewFoli"] > 0)
            {
              flag2 = true;
              ++dictionary1[keyValuePair.Key]["Foliage/Twigs"]["Defoliation"][this._pestData.DctFTChewFoli[(int) dataRow["PestFTChewFoli"]]].TreeCount;
            }
            if ((int) dataRow["PestFTDiscFoli"] > 0)
            {
              flag2 = true;
              ++dictionary1[keyValuePair.Key]["Foliage/Twigs"]["Discolored Foliage"][this._pestData.DctFTDiscFoli[(int) dataRow["PestFTDiscFoli"]]].TreeCount;
            }
            if ((int) dataRow["PestFTAbnFoli"] > 0)
            {
              flag2 = true;
              ++dictionary1[keyValuePair.Key]["Foliage/Twigs"]["Abnormal Foliage"][this._pestData.DctFTAbnFoli[(int) dataRow["PestFTAbnFoli"]]].TreeCount;
            }
            if ((int) dataRow["PestFTInsectSigns"] > 0)
            {
              flag2 = true;
              ++dictionary1[keyValuePair.Key]["Foliage/Twigs"]["Insect Signs"][this._pestData.DctFTInsectSigns[(int) dataRow["PestFTInsectSigns"]]].TreeCount;
            }
            if ((int) dataRow["PestFTFoliAffect"] > 0)
            {
              flag2 = true;
              ++dictionary1[keyValuePair.Key]["Foliage/Twigs"]["% Foliage Affected"][this._pestData.DctFTFoliAffect[(int) dataRow["PestFTFoliAffect"]]].TreeCount;
            }
            if (!string.IsNullOrEmpty(dataRow["PestFTNotes"].ToString()))
            {
              flag2 = true;
              ++dictionary1[keyValuePair.Key]["Foliage/Twigs"]["Notes Present"][string.Empty].TreeCount;
            }
            if ((int) dataRow["PestBBInsectSigns"] > 0)
            {
              flag3 = true;
              ++dictionary1[keyValuePair.Key]["Branches/Bole"]["Insect Signs"][this._pestData.DctBBInsectSigns[(int) dataRow["PestBBInsectSigns"]]].TreeCount;
            }
            if ((int) dataRow["PestBBInsectPres"] > 0)
            {
              flag3 = true;
              ++dictionary1[keyValuePair.Key]["Branches/Bole"]["Insect Presence"][this._pestData.DctBBInsectPres[(int) dataRow["PestBBInsectPres"]]].TreeCount;
            }
            if ((int) dataRow["PestBBDiseaseSigns"] > 0)
            {
              flag3 = true;
              ++dictionary1[keyValuePair.Key]["Branches/Bole"]["Disease Signs"][this._pestData.DctBBDiseaseSigns[(int) dataRow["PestBBDiseaseSigns"]]].TreeCount;
            }
            if ((int) dataRow["PestBBProbLoc"] > 0)
            {
              flag3 = true;
              ++dictionary1[keyValuePair.Key]["Branches/Bole"]["Problem Location"][this._pestData.DctBBProbLoc[(int) dataRow["PestBBProbLoc"]]].TreeCount;
            }
            if ((int) dataRow["PestBBAbnGrowth"] > 0)
            {
              flag3 = true;
              ++dictionary1[keyValuePair.Key]["Branches/Bole"]["Loose Bark"][this._pestData.DctBBAbnGrowth[(int) dataRow["PestBBAbnGrowth"]]].TreeCount;
            }
            if (!string.IsNullOrEmpty(dataRow["PestBBNotes"].ToString()))
            {
              flag3 = true;
              ++dictionary1[keyValuePair.Key]["Branches/Bole"]["Notes Present"][string.Empty].TreeCount;
            }
            if (flag1)
              ++dictionary2[keyValuePair.Key]["Tree Stress"].TreeCount;
            if (flag2)
              ++dictionary2[keyValuePair.Key]["Foliage/Twigs"].TreeCount;
            if (flag3)
              ++dictionary2[keyValuePair.Key]["Branches/Bole"].TreeCount;
            if (flag1 | flag2 | flag3)
              ++dictionary3[keyValuePair.Key].TreeCount;
          }
        }
      }
      int num = 1;
      foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, Dictionary<string, PestStats>>>> keyValuePair1 in dictionary1)
      {
        if (ReportBase.m_ps.Species.ContainsKey(keyValuePair1.Key) && (string.IsNullOrEmpty(this._curSpecies) || keyValuePair1.Key == this._curSpecies))
        {
          renderTable.Cells[num, 0].Text = this.GetSpecies(keyValuePair1.Key);
          foreach (KeyValuePair<string, Dictionary<string, Dictionary<string, PestStats>>> keyValuePair2 in keyValuePair1.Value)
          {
            renderTable.Cells[num, 1].Text = keyValuePair2.Key;
            foreach (KeyValuePair<string, Dictionary<string, PestStats>> keyValuePair3 in keyValuePair2.Value)
            {
              renderTable.Cells[num, 2].Text = keyValuePair3.Key;
              foreach (KeyValuePair<string, PestStats> keyValuePair4 in keyValuePair3.Value)
              {
                renderTable.Cells[num, 3].Text = keyValuePair4.Key;
                renderTable.Cells[num, 4].Text = keyValuePair4.Value.TreeCount.ToString("#,0");
                renderTable.Cells[num, 5].Text = ((double) keyValuePair4.Value.TreeCount / (double) speciesDictionaryList2[keyValuePair1.Key].Count * 100.0).ToString("0.00");
                renderTable.Cells[num, 6].Text = ((double) keyValuePair4.Value.TreeCount / (double) allTreeCount * 100.0).ToString("0.00");
                renderTable.Cells[num, 7].Text = ((double) keyValuePair4.Value.TreeCount / (double) affectedTreeCount * 100.0).ToString("0.00");
                ++num;
              }
            }
          }
          renderTable.Cells[num, 3].Text = i_Tree_Eco_v6.Resources.Strings.Total;
          renderTable.Cells[num, 4].Text = dictionary3[keyValuePair1.Key].TreeCount.ToString("#,0");
          renderTable.Cells[num, 5].Text = ((double) dictionary3[keyValuePair1.Key].TreeCount / (double) speciesDictionaryList2[keyValuePair1.Key].Count * 100.0).ToString("0.00");
          renderTable.Cells[num, 6].Text = ((double) dictionary3[keyValuePair1.Key].TreeCount / (double) allTreeCount * 100.0).ToString("0.00");
          renderTable.Cells[num, 7].Text = ((double) dictionary3[keyValuePair1.Key].TreeCount / (double) affectedTreeCount * 100.0).ToString("0.00");
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
