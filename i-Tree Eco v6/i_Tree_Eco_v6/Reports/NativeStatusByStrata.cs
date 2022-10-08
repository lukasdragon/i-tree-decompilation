// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.NativeStatusByStrata
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
  internal class NativeStatusByStrata : DatabaseReport
  {
    private bool plusFound;

    public NativeStatusByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleNativeStatusByStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable stratumAndContinent = this.GetLeafAreaByStratumAndContinent();
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Continent];
      SortedList<int, SortedList<int, double>> sortedList1 = new SortedList<int, SortedList<int, double>>();
      List<int> intList = new List<int>();
      foreach (DataRow row in (InternalDataCollectionBase) stratumAndContinent.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        if (sortedList1.Keys.Count != 1 || !(this.estUtil.ClassValues[Classifiers.Strata][(short) key1].Item1 == "Study Area"))
        {
          int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
          double num = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
          if (!sortedList1.ContainsKey(key1))
            sortedList1.Add(key1, new SortedList<int, double>());
          SortedList<int, double> sortedList2 = sortedList1[key1];
          if (!sortedList2.ContainsKey(key2))
            sortedList2.Add(key2, num);
          if (!intList.Contains(key2))
            intList.Add(key2);
        }
      }
      intList.Sort();
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.PlaceOfNativeRange;
      renderTable.Cells[0, 1].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cells[1, 0].Text = v6Strings.Strata_SingularName;
      this.plusFound = false;
      for (int index = 0; index < intList.Count; ++index)
      {
        renderTable.Cells[1, index + 1].Text = this.estUtil.ClassValues[Classifiers.Continent][(short) intList[index]].Item1;
        if (this.estUtil.ClassValues[Classifiers.Continent][(short) intList[index]].Item1.EndsWith("+"))
          this.plusFound = true;
      }
      renderTable.Cells[0, 1].SpanCols = renderTable.Cols.Count - 1;
      for (int index1 = 0; index1 < sortedList1.Count; ++index1)
      {
        string str = this.estUtil.ClassValues[Classifiers.Strata][(short) sortedList1.Keys[index1]].Item1;
        renderTable.Cells[index1 + 2, 0].Text = sortedList1.Keys.Count != 1 || this.curYear.RecordStrata ? str : "Study Area";
        if (renderTable.Cells[index1 + 2, 0].Text == "Study Area")
        {
          renderTable.Cells[index1 + 2, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          renderTable.Rows[index1 + 2].Style.Borders.Top = LineDef.Default;
          renderTable.Rows[index1 + 2].Style.FontBold = true;
        }
        for (int index2 = 0; index2 < intList.Count; ++index2)
        {
          if (sortedList1[sortedList1.Keys[index1]].ContainsKey(intList[index2]))
            renderTable.Cells[index1 + 2, index2 + 1].Text = sortedList1[sortedList1.Keys[index1]][intList[index2]].ToString("N1");
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.Note(C1doc);
    }

    private DataTable GetLeafAreaByStratumAndContinent() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery()
    {
      List<Classifiers> source = new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Continent
      };
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetMultipleFieldsData2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, source)], source.Select<Classifiers, string>((Func<Classifiers, string>) (c => this.estUtil.ClassifierNames[c])).ToArray<string>());
    }

    protected override string ReportMessage() => this.plusFound ? i_Tree_Eco_v6.Resources.Strings.NoteNativeStatus : string.Empty;
  }
}
