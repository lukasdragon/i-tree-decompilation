// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.HydrologyEffectsOfTreesByStratum
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class HydrologyEffectsOfTreesByStratum : HydrologyEffects
  {
    public HydrologyEffectsOfTreesByStratum()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleHydrologyEffectsOfTreesByStratum;
      this.classifier = Classifiers.Strata;
      this.unifier = "Study Area";
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      this.Header(renderTable);
      this.GetData();
      this.outList = this.avoidedRunoff.ToList<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>();
      this.outList.Sort((Comparison<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>) ((x, y) => y.Value.AvoidedRunoffValue.CompareTo(x.Value.AvoidedRunoffValue)));
      int headerRows = this.headerRows;
      for (int index = 0; index < this.outList.Count; ++index)
      {
        KeyValuePair<int, HydrologyEffects.TotalRunoffObject> keyValuePair = this.outList[index];
        HydrologyEffects.TotalRunoffObject values = keyValuePair.Value;
        SortedList<short, Tuple<string, string>> classValue1 = this.estUtil.ClassValues[this.classifier];
        keyValuePair = this.outList[index];
        int key1 = (int) (short) keyValuePair.Key;
        if (!(classValue1[(short) key1].Item1 == this.unifier))
        {
          TableCell cell = renderTable.Cells[headerRows, 0];
          SortedList<short, Tuple<string, string>> classValue2 = this.estUtil.ClassValues[this.classifier];
          keyValuePair = this.outList[index];
          int key2 = (int) (short) keyValuePair.Key;
          string str = classValue2[(short) key2].Item1;
          cell.Text = str;
          this.PopulateHydrologyRow(renderTable, headerRows, values);
          ++headerRows;
        }
        else
          break;
      }
      this.BodyStylinig(C1doc, renderTable);
      renderTable.Width = (Unit) "100%";
      renderTable.Cols[0].Width = (Unit) "13%";
      renderTable.Cols[1].Width = (Unit) "10%";
      renderTable.Cols[2].Width = (Unit) "10%";
      renderTable.Cols[3].Width = (Unit) "12%";
      renderTable.Cols[4].Width = (Unit) "11%";
      renderTable.Cols[5].Width = (Unit) "11%";
      renderTable.Cols[6].Width = (Unit) "11%";
      renderTable.Cols[7].Width = (Unit) "11%";
      renderTable.Cols[8].Width = (Unit) "11%";
      if (this.outList.Count > 1)
      {
        this.Totals(renderTable);
        renderTable.Cells[headerRows, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        renderTable.Rows[headerRows].Style.FontBold = true;
      }
      else if (!this.curYear.RecordStrata)
      {
        renderTable.Cells[headerRows - 1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
        renderTable.Rows[headerRows - 1].Style.FontBold = true;
      }
      this.Note(C1doc);
    }
  }
}
