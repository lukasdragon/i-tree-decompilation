// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.SpeciesCompositionByDBHHorizontal
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  internal class SpeciesCompositionByDBHHorizontal : SpieciesDistributionBase
  {
    public SpeciesCompositionByDBHHorizontal() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSpeciesDistributionbyDBHClass;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.ClipPage = true;
      SortedList<int, SortedList<int, Tuple<double, double>>> dbhData = this.GetDBHData();
      RenderTable renderTable = this.RenderTableAndHeader(C1doc);
      renderTable.Cols[0].Visible = false;
      this.HeaderDBH(renderTable, 0, this._dbhColumnIndexes.Count - 1);
      int num = 3;
      for (int index = 0; index < dbhData.Count; ++index)
      {
        SortedList<int, Tuple<double, double>> sortedList = dbhData.Values[index];
        short key = (short) dbhData.Keys[index];
        if (sortedList.Any<KeyValuePair<int, Tuple<double, double>>>((Func<KeyValuePair<int, Tuple<double, double>>, bool>) (p => p.Value.Item1 > 0.0 || p.Value.Item2 > 0.0)))
        {
          Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][key];
          renderTable.Cells[num, 1].Text = ReportBase.ScientificName ? tuple.Item2 : tuple.Item1;
          if (renderTable.Cells[num, 1].Text == "Total")
          {
            renderTable.Cells[num, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
            renderTable.Rows[num].Style.FontBold = true;
          }
          this.SpeciesDBHDataIntoTable(sortedList, renderTable, 0, this._dbhColumnIndexes.Count - 1, num);
        }
        ++num;
      }
      ReportUtil.FormatRenderTable(renderTable);
    }
  }
}
