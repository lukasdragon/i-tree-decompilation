// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.BioemissionsOfTreesBySpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class BioemissionsOfTreesBySpecies : DatabaseReport
  {
    public BioemissionsOfTreesBySpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleVOCEmissionsOfTreesBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Species];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.VOCs];
      DataTable dataTable = this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Species,
        Classifiers.VOCs
      })], classifierName1, classifierName2).SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.VOCEmissions]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.None, Units.Year)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("classifier", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      List<KeyValuePair<int, string>> keyValuePairList = new List<KeyValuePair<int, string>>();
      SortedList<int, double> source = new SortedList<int, double>();
      int num1 = 0;
      foreach (KeyValuePair<short, Tuple<string, string>> keyValuePair in this.estUtil.ClassValues[Classifiers.VOCs])
      {
        if (keyValuePair.Value.Item1 != "VOC Other")
          keyValuePairList.Add(new KeyValuePair<int, string>((int) keyValuePair.Key, keyValuePair.Value.Item1));
        else
          num1 = (int) keyValuePair.Key;
        source.Add((int) keyValuePair.Key, 0.0);
      }
      SortedList<string, SortedList<int, double>> sortedList1 = new SortedList<string, SortedList<int, double>>();
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        string key2 = ReportBase.ScientificName ? this.estUtil.ClassValues[Classifiers.Species][(short) key1].Item2 : this.estUtil.ClassValues[Classifiers.Species][(short) key1].Item1;
        int key3 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        if (key3 != num1)
        {
          double d = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
          if (!sortedList1.ContainsKey(key2))
            sortedList1.Add(key2, new SortedList<int, double>());
          sortedList1[key2].Add(key3, EstimateUtil.ConvertToEnglish(d, Units.Kilograms, ReportBase.EnglishUnits));
          source[key3] += EstimateUtil.ConvertToEnglish(d, Units.Kilograms, ReportBase.EnglishUnits);
        }
      }
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
      KeyValuePair<int, string> keyValuePair1;
      for (int index = 0; index <= keyValuePairList.Count; ++index)
      {
        if (index < keyValuePairList.Count)
        {
          TableCell cell = renderTable.Cells[0, index + 1];
          keyValuePair1 = keyValuePairList[index];
          string str = ReportUtil.FormatInlineHeaderUnitsStr(keyValuePair1.Value, ReportUtil.GetValuePerYrStr(ReportBase.KgUnits()));
          cell.Text = str;
        }
        else
          renderTable.Cells[0, index + 1].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.TotalVOCs, ReportUtil.GetValuePerYrStr(ReportBase.KgUnits()));
      }
      for (int index1 = 0; index1 < sortedList1.Count; ++index1)
      {
        renderTable.Cells[index1 + 1, 0].Text = sortedList1.Keys[index1];
        for (int index2 = 0; index2 <= keyValuePairList.Count; ++index2)
        {
          if (index2 < keyValuePairList.Count)
          {
            SortedList<int, double> sortedList2 = sortedList1.Values[index1];
            keyValuePair1 = keyValuePairList[index2];
            int key4 = keyValuePair1.Key;
            if (sortedList2.ContainsKey(key4))
            {
              TableCell cell = renderTable.Cells[index1 + 1, index2 + 1];
              SortedList<int, double> sortedList3 = sortedList1.Values[index1];
              keyValuePair1 = keyValuePairList[index2];
              int key5 = keyValuePair1.Key;
              string str = sortedList3[key5].ToString("N1");
              cell.Text = str;
              continue;
            }
          }
          renderTable.Cells[index1 + 1, index2 + 1].Text = sortedList1.Values[index1].Sum<KeyValuePair<int, double>>((Func<KeyValuePair<int, double>, double>) (p => p.Value)).ToString("N1");
        }
      }
      int count = renderTable.Rows.Count;
      renderTable.Rows[count].Style.FontBold = true;
      renderTable.Cells[count, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      renderTable.Rows[count].Style.Borders.Top = LineDef.Default;
      double num2;
      for (int index = 0; index <= keyValuePairList.Count; ++index)
      {
        if (index < keyValuePairList.Count)
        {
          TableCell cell = renderTable.Cells[count, index + 1];
          SortedList<int, double> sortedList4 = source;
          keyValuePair1 = keyValuePairList[index];
          int key = keyValuePair1.Key;
          num2 = sortedList4[key];
          string str = num2.ToString("N1");
          cell.Text = str;
        }
        else
        {
          TableCell cell = renderTable.Cells[count, index + 1];
          num2 = source.Sum<KeyValuePair<int, double>>((Func<KeyValuePair<int, double>, double>) (p => p.Value));
          string str = num2.ToString("N1");
          cell.Text = str;
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
    }
  }
}
