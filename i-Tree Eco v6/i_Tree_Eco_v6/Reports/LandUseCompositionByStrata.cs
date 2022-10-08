// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.LandUseCompositionByStrata
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Util;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class LandUseCompositionByStrata : DatabaseReport
  {
    public LandUseCompositionByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleLandUseCompositionByStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable compositionByStrata = this.GetLandUseCompositionByStrata();
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Landuse];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Strata];
      SortedList<string, SortedList<int, double>> sortedList1 = new SortedList<string, SortedList<int, double>>();
      List<int> intList = new List<int>();
      if (compositionByStrata != null)
      {
        foreach (DataRow row in (InternalDataCollectionBase) compositionByStrata.Rows)
        {
          string key1 = this.estUtil.ClassValues[Classifiers.Strata][(short) ReportUtil.ConvertFromDBVal<int>(row[classifierName2])].Item1;
          int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
          double num = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
          if (!intList.Contains(key2))
            intList.Add(key2);
          if (!sortedList1.ContainsKey(key1))
            sortedList1.Add(key1, new SortedList<int, double>());
          if (!sortedList1[key1].ContainsKey(key2))
            sortedList1[key1].Add(key2, num);
        }
      }
      intList.Sort();
      int num1 = 4;
      int num2 = (int) Math.Ceiling((double) sortedList1.Count / (double) num1);
      string str = ReportBase.plotInventory ? ReportUtil.FormatHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.PercentOfArea) : ReportUtil.FormatHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.PercentOfTrees);
      for (int index1 = 0; index1 < num2; ++index1)
      {
        int num3 = index1 * num1;
        int num4 = Math.Min(sortedList1.Count - 1, (index1 + 1) * num1 - 1);
        RenderTable renderTable = new RenderTable();
        if (index1 > 0)
          renderTable.BreakBefore = BreakEnum.Page;
        C1doc.Body.Children.Add((RenderObject) renderTable);
        int count = 3;
        renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
        renderTable.Rows[0].Style.FontSize = 14f;
        renderTable.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.LandUse;
        renderTable.Cells[0, 1].Text = v6Strings.Strata_SingularName;
        for (int index2 = 0; index2 < intList.Count; ++index2)
          renderTable.Cells[index2 + count, 0].Text = this.estUtil.ClassValues[Classifiers.Landuse][(short) intList[index2]].Item1;
        renderTable.Cells[renderTable.Rows.Count, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        renderTable.Rows[renderTable.Rows.Count - 1].Style.Borders.Top = LineDef.Default;
        renderTable.Rows[renderTable.Rows.Count - 1].Style.FontBold = true;
        for (int index3 = 0; index3 < sortedList1.Count; ++index3)
        {
          if (index3 >= num3 && index3 <= num4)
          {
            int col = index3 - num3 + 1;
            if (sortedList1.Count == 1)
            {
              renderTable.Cells[1, col].Text = this.curYear.RecordStrata ? sortedList1.Keys[index3] : i_Tree_Eco_v6.Resources.Strings.StudyArea;
              renderTable.Cells[2, col].Text = str;
            }
            else
            {
              renderTable.Cells[1, col].Text = sortedList1.Keys[index3];
              renderTable.Cells[2, col].Text = str;
            }
            SortedList<int, double> sortedList2 = sortedList1.Values[index3];
            for (int index4 = 0; index4 < intList.Count; ++index4)
            {
              int key = intList[index4];
              renderTable.Cells[index4 + count, col].Text = !sortedList2.ContainsKey(key) ? 0M.ToString("N1") : sortedList2[key].ToString("N1");
            }
            renderTable.Cells[renderTable.Rows.Count - 1, col].Text = Enumerable.Sum(sortedList2.Values).ToString("N1");
          }
        }
        renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
        renderTable.Cols[0].Style.Borders.Right = new LineDef((Unit) "1pt", Color.Gray);
        renderTable.Cells[0, 1].SpanCols = renderTable.Cols.Count - 1;
        renderTable.Cells[0, 1].Style.TextAlignHorz = AlignHorzEnum.Center;
        renderTable.Cols[0].Width = (Unit) "20%";
        for (int index5 = 1; index5 < renderTable.Cols.Count; ++index5)
          renderTable.Cols[index5].Width = (Unit) "1.5in";
        renderTable.Width = Unit.Auto;
        renderTable.Style.Spacing.Bottom = (Unit) ".1in";
        ReportUtil.FormatRenderTable(renderTable);
      }
    }

    private DataTable GetLandUseCompositionByStrata() => this.GetEstimatedValues2().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", 12).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatedValues2() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValues2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Landuse, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Landuse
    })], this.estUtil.ClassifierNames[Classifiers.Landuse], this.estUtil.ClassifierNames[Classifiers.Strata]);
  }
}
