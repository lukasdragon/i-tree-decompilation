// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PercentGroundCoverByStrata
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

namespace i_Tree_Eco_v6.Reports
{
  public class PercentGroundCoverByStrata : DatabaseReport
  {
    public PercentGroundCoverByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleGroundCoverCompositionByStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable groundCover = this.GetGroundCover();
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.GroundCover];
      SortedList<int, SortedList<int, Tuple<double, double>>> sortedList = new SortedList<int, SortedList<int, Tuple<double, double>>>();
      List<int> intList = new List<int>();
      foreach (DataRow row in (InternalDataCollectionBase) groundCover.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        if (sortedList.Count != 1 || !(this.estUtil.ClassValues[Classifiers.Strata][(short) key1].Item1 == "Study Area"))
        {
          int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
          double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
          double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
          if (!intList.Contains(key2))
            intList.Add(key2);
          if (!sortedList.ContainsKey(key1))
            sortedList.Add(key1, new SortedList<int, Tuple<double, double>>());
          if (!sortedList[key1].ContainsKey(key2))
            sortedList[key1].Add(key2, Tuple.Create<double, double>(num1, num2));
        }
      }
      intList.Sort();
      int num3 = 6;
      int num4 = (int) Math.Ceiling((double) intList.Count / (double) num3);
      for (int index1 = 0; index1 < num4; ++index1)
      {
        RenderTable renderTable = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable);
        renderTable.SplitVertBehavior = SplitBehaviorEnum.SplitIfLarge;
        int count = 2;
        renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
        renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
        renderTable.Cells[1, 0].Text = v6Strings.Strata_SingularName;
        int num5 = -1;
        LineDef lineDef = new LineDef((Unit) "1pt", Color.Gray);
        for (int index2 = 0; index2 < intList.Count; ++index2)
        {
          if (index2 >= index1 * num3 && index2 < (index1 + 1) * num3)
          {
            renderTable.Cols[(index2 - index1 * num3) * 2 + 1].Style.Borders.Left = lineDef;
            renderTable.Cells[0, (index2 - index1 * num3) * 2 + 1].SpanCols = 2;
            renderTable.Cells[0, (index2 - index1 * num3) * 2 + 1].Text = this.estUtil.ClassValues[Classifiers.GroundCover][(short) intList[index2]].Item1;
            renderTable.Cells[1, (index2 - index1 * num3) * 2 + 1].Text = "%";
            renderTable.Cells[1, (index2 - index1 * num3) * 2 + 2].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
            if (renderTable.Cells[0, (index2 - index1 * num3) * 2 + 1].Text.ToUpper() == "SHRUB")
              num5 = (index2 - index1 * num3) * 2 + 1;
          }
        }
        int num6 = count;
        for (int index3 = 0; index3 < sortedList.Count; ++index3)
        {
          string str1 = this.estUtil.ClassValues[Classifiers.Strata][(short) sortedList.Keys[index3]].Item1;
          renderTable.Cells[num6, 0].Text = sortedList.Keys.Count != 1 || this.curYear.RecordStrata ? str1 : "Study Area";
          if (renderTable.Cells[num6, 0].Text == "Study Area")
          {
            renderTable.Cells[num6, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
            renderTable.Rows[num6].Style.Borders.Top = LineDef.Default;
            renderTable.Rows[num6].Style.FontBold = true;
          }
          for (int index4 = 0; index4 < intList.Count; ++index4)
          {
            if (index4 >= index1 * num3 && index4 < (index1 + 1) * num3 && sortedList.Values[index3].ContainsKey(intList[index4]))
            {
              TableCell cell1 = renderTable.Cells[num6, (index4 - index1 * num3) * 2 + 1];
              double num7 = sortedList.Values[index3][intList[index4]].Item1;
              string str2 = num7.ToString("N1");
              cell1.Text = str2;
              TableCell cell2 = renderTable.Cells[num6, (index4 - index1 * num3) * 2 + 2];
              num7 = sortedList.Values[index3][intList[index4]].Item2;
              string str3 = "±" + num7.ToString("N1");
              cell2.Text = str3;
              if ((index4 - index1 * num3) * 2 + 1 == num5 && !this.curYear.RecordPercentShrub)
                renderTable.Cells[num6, (index4 - index1 * num3) * 2 + 1].Text = renderTable.Cells[num6, (index4 - index1 * num3) * 2 + 2].Text = string.Empty;
            }
          }
          ++num6;
        }
        for (int row = 3; row < renderTable.Rows.Count; ++row)
        {
          for (int col = 2; col < renderTable.Cols.Count; ++col)
          {
            if (renderTable.Cells[row, col].Text == "0.0")
              renderTable.Cells[row, col].Text = "<0.1";
          }
        }
        renderTable.Width = Unit.Auto;
        renderTable.Cols[0].Width = (Unit) "12%";
        renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
        for (int index5 = 1; index5 < renderTable.Cols.Count; ++index5)
          renderTable.Cols[index5].Width = (Unit) ".65in";
        renderTable.UserCellGroups.Add(new UserCellGroup(new Rectangle(1, 0, renderTable.Cols.Count - 1, 1))
        {
          Style = {
            TextAlignHorz = AlignHorzEnum.Center
          }
        });
        renderTable.Style.Spacing.Bottom = (Unit) ".5cm";
        ReportUtil.FormatRenderTable(renderTable);
      }
    }

    private DataTable GetGroundCover() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CoverArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetSignificantMultipleFieldsData(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Cover, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.GroundCover
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.GroundCover]);

    public override void SetAlignment(C1PrintDocument C1doc)
    {
      C1doc.Style.FlowAlign = FlowAlignEnum.Center;
      C1doc.Style.TextAlignHorz = AlignHorzEnum.Left;
      C1doc.Style.FlowAlignChildren = FlowAlignEnum.Near;
    }
  }
}
