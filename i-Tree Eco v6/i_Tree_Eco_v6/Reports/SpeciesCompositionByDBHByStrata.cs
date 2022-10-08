// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.SpeciesCompositionByDBHByStrata
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Util;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class SpeciesCompositionByDBHByStrata : SpieciesDistributionBase
  {
    public SpeciesCompositionByDBHByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSpeciesDistributionbyDBHClassandStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.ClipPage = true;
      SortedList<int, SortedList<int, SortedList<int, Tuple<double, double>>>> strataDbhData = this.GetStrataDBHData();
      this._speciesList.Sort();
      RenderTable renderTable = new RenderTable();
      double num1 = Math.Ceiling((double) this._dbhColumnIndexes.Count / 7.0);
      for (int index1 = 0; (double) index1 < num1; ++index1)
      {
        int startDbhIdex = index1 * 7;
        int endDbhIndex = Math.Min(this._dbhColumnIndexes.Count - 1, (index1 + 1) * 7 - 1);
        int num2 = 3;
        for (int index2 = 0; index2 < strataDbhData.Count; ++index2)
        {
          if (strataDbhData.Count == 2 && index2 == 1)
          {
            if (!this.curYear.RecordStrata)
              renderTable.Cells[3, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          }
          else
          {
            SortedList<int, SortedList<int, Tuple<double, double>>> sortedList = strataDbhData.Values[index2];
            short key = (short) strataDbhData.Keys[index2];
            renderTable = this.RenderTableAndHeader(C1doc);
            renderTable.SplitHorzBehavior = SplitBehaviorEnum.Never;
            this.HeaderDBH(renderTable, startDbhIdex, endDbhIndex);
            renderTable.Cols[0].Width = (Unit) "14%";
            renderTable.Cols[1].Width = (Unit) "22%";
            renderTable.Cells[num2, 0].Text = this.estUtil.ClassValues[Classifiers.Strata][key].Item1;
            foreach (short species in this._speciesList)
            {
              Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][species];
              renderTable.Cells[num2, 1].Text = ReportBase.ScientificName ? tuple.Item2 : tuple.Item1;
              if (renderTable.Cells[num2, 1].Text == "Total")
              {
                renderTable.Cells[num2, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
                renderTable.Rows[num2].Style.Borders.Top = LineDef.Default;
                renderTable.Rows[num2].Style.Borders.Bottom = LineDef.Default;
                renderTable.Rows[num2].Style.FontBold = true;
              }
              if ((int) key == this.GetStudyAreaCVO())
              {
                if ((int) species == this.GetTotalTreeCVO())
                  renderTable.Cells[num2, 1].Text = string.Empty;
                else
                  continue;
              }
              if (sortedList.Keys.Contains((int) species))
                this.SpeciesDBHDataIntoTable(sortedList[(int) species], renderTable, startDbhIdex, endDbhIndex, num2);
              ++num2;
            }
            ReportUtil.FormatRenderTable(renderTable);
          }
        }
      }
    }

    public override void SetAlignment(C1PrintDocument C1doc)
    {
      C1doc.Style.FlowAlign = FlowAlignEnum.Near;
      C1doc.Style.FlowAlignChildren = FlowAlignEnum.Near;
      C1doc.Style.TextAlignHorz = AlignHorzEnum.Center;
    }
  }
}
