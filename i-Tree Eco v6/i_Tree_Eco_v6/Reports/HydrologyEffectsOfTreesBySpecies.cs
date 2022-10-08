// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.HydrologyEffectsOfTreesBySpecies
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
  public class HydrologyEffectsOfTreesBySpecies : HydrologyEffects
  {
    public HydrologyEffectsOfTreesBySpecies()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleHydrologyEffectsOfTreesBySpecies;
      this.classifier = Classifiers.Species;
      this.unifier = "Total";
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.ClipPage = true;
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
      this.Header(renderTable);
      this.GetData();
      this.outList = this.avoidedRunoff.ToList<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>();
      this.outList.Sort((Comparison<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>) ((x, y) => y.Value.AvoidedRunoffValue.CompareTo(x.Value.AvoidedRunoffValue)));
      int headerRows = this.headerRows;
      for (int index = 0; index < this.outList.Count; ++index)
      {
        Tuple<string, string> tuple = this.estUtil.ClassValues[this.classifier][(short) this.outList[index].Key];
        renderTable.Cells[headerRows, 0].Text = ReportBase.ScientificName ? tuple.Item2 : tuple.Item1;
        HydrologyEffects.TotalRunoffObject values = this.outList[index].Value;
        this.PopulateHydrologyRow(renderTable, headerRows, values);
        ++headerRows;
      }
      this.BodyStylinig(C1doc, renderTable);
      renderTable.Width = (Unit) "100%";
      renderTable.Cols[0].Width = (Unit) "18%";
      renderTable.Cols[1].Width = (Unit) "10%";
      renderTable.Cols[2].Width = (Unit) "10%";
      renderTable.Cols[3].Width = (Unit) "12%";
      renderTable.Cols[4].Width = (Unit) "10%";
      renderTable.Cols[5].Width = (Unit) "10%";
      renderTable.Cols[6].Width = (Unit) "10%";
      renderTable.Cols[7].Width = (Unit) "10%";
      renderTable.Cols[8].Width = (Unit) "10%";
      this.Totals(renderTable);
      renderTable.Cells[headerRows, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      renderTable.Rows[headerRows].Style.FontBold = true;
      this.Note(C1doc);
    }
  }
}
