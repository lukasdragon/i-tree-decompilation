// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.SpeciesCompositionByDBHByStrataHorizontal
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
  internal class SpeciesCompositionByDBHByStrataHorizontal : SpieciesDistributionBase
  {
    public SpeciesCompositionByDBHByStrataHorizontal() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSpeciesDistributionbyDBHClassandStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.ClipPage = true;
      SortedList<int, SortedList<int, SortedList<int, Tuple<double, double>>>> strataDbhData = this.GetStrataDBHData();
      RenderTable renderTable = this.RenderTableAndHeader(C1doc);
      this.HeaderDBH(renderTable, 0, this._dbhColumnIndexes.Count - 1);
      int num = 3;
      for (int index1 = 0; index1 < strataDbhData.Count; ++index1)
      {
        if (strataDbhData.Count == 2 && index1 == 1)
        {
          if (!this.curYear.RecordStrata)
            renderTable.Cells[3, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
        }
        else
        {
          SortedList<int, SortedList<int, Tuple<double, double>>> sortedList1 = strataDbhData.Values[index1];
          short key1 = (short) strataDbhData.Keys[index1];
          renderTable.Cells[num, 0].Text = this.estUtil.ClassValues[Classifiers.Strata][key1].Item1;
          for (int index2 = 0; index2 < sortedList1.Count; ++index2)
          {
            SortedList<int, Tuple<double, double>> sortedList2 = sortedList1.Values[index2];
            short key2 = (short) sortedList1.Keys[index2];
            if (((int) key1 != this.GetStudyAreaCVO() || (int) key2 == this.GetTotalTreeCVO()) && sortedList2.Any<KeyValuePair<int, Tuple<double, double>>>((Func<KeyValuePair<int, Tuple<double, double>>, bool>) (p => p.Value.Item1 > 0.0 || p.Value.Item2 > 0.0)))
            {
              Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][key2];
              renderTable.Cells[num, 1].Text = ReportBase.ScientificName ? tuple.Item2 : tuple.Item1;
              if (renderTable.Cells[num, 1].Text == "Total")
              {
                renderTable.Cells[num, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
                renderTable.Rows[num].Style.Borders.Top = LineDef.Default;
                renderTable.Rows[num].Style.Borders.Bottom = LineDef.Default;
                renderTable.Rows[num].Style.FontBold = true;
              }
              if ((int) key1 == this.GetStudyAreaCVO() && (int) key2 == this.GetTotalTreeCVO())
                renderTable.Cells[num, 1].Text = string.Empty;
              this.SpeciesDBHDataIntoTable(sortedList2, renderTable, 0, this._dbhColumnIndexes.Count - 1, num);
              ++num;
            }
          }
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
    }
  }
}
