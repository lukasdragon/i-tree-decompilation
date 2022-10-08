// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.SpeciesCompositionByDBH
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
  internal class SpeciesCompositionByDBH : SpieciesDistributionBase
  {
    public SpeciesCompositionByDBH() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSpeciesDistributionbyDBHClass;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      SortedList<int, SortedList<int, Tuple<double, double>>> dbhData = this.GetDBHData();
      double num1 = Math.Ceiling((double) this._dbhColumnIndexes.Count / 6.0);
      for (int index1 = 0; (double) index1 < num1; ++index1)
      {
        int startDbhIdex = index1 * 6;
        int endDbhIndex = Math.Min(this._dbhColumnIndexes.Count - 1, (index1 + 1) * 6 - 1);
        RenderTable renderTable = this.RenderTableAndHeader(C1doc);
        renderTable.SplitHorzBehavior = SplitBehaviorEnum.Never;
        renderTable.Cols[0].Visible = false;
        renderTable.Cols[1].Width = (Unit) "20%";
        this.HeaderDBH(renderTable, startDbhIdex, endDbhIndex);
        int num2 = 3;
        for (int index2 = 0; index2 < dbhData.Count; ++index2)
        {
          Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][(short) dbhData.Keys[index2]];
          renderTable.Cells[num2, 1].Text = ReportBase.ScientificName ? tuple.Item2 : tuple.Item1;
          if (renderTable.Cells[num2, 0].Text == "Total")
          {
            renderTable.Cells[num2, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
            renderTable.Rows[num2].Style.FontBold = true;
          }
          this.SpeciesDBHDataIntoTable(dbhData.Values[index2], renderTable, startDbhIdex, endDbhIndex, num2);
          ++num2;
        }
        ReportUtil.FormatRenderTable(renderTable);
      }
    }

    public override void SetAlignment(C1PrintDocument C1doc)
    {
      C1doc.Style.FlowAlign = FlowAlignEnum.Near;
      C1doc.Style.FlowAlignChildren = FlowAlignEnum.Near;
      C1doc.Style.TextAlignHorz = AlignHorzEnum.Center;
    }

    protected override void SetLayout(C1PrintDocument C1doc)
    {
      base.SetLayout(C1doc);
      C1doc.ClipPage = true;
      C1doc.PageLayout.PageSettings.Landscape = false;
    }
  }
}
