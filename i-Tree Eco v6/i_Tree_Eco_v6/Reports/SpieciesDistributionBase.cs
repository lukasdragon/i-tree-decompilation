// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.SpieciesDistributionBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Util;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;

namespace i_Tree_Eco_v6.Reports
{
  internal class SpieciesDistributionBase : DatabaseReport
  {
    protected List<short> _speciesList = new List<short>();
    protected Dictionary<short, int> _dbhColumnIndexes;
    protected SortedList<short, string> _convertedDbhRanges;
    protected string _percentFormat = "N1";
    protected const int _headerRows = 3;

    public DataTable GetMultipleFieldsData(
      List<Classifiers> fields,
      EstimateTypeEnum estType,
      Units primaryUnit,
      Units secondaryUnit,
      Units tertiaryUnit)
    {
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetMultipleFieldsData(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, fields)], string.Join<Classifiers>(", ", (IEnumerable<Classifiers>) fields)).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("equType", 1).SetParameter<int>(nameof (estType), this.estUtil.EstTypes[estType]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(primaryUnit, secondaryUnit, tertiaryUnit)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
    }

    protected virtual void GetDBHColumnIndexesAndRanges()
    {
      this._convertedDbhRanges = this.GetConvertedDbhRanges();
      this.GetDBHColumnIndexes(this._convertedDbhRanges);
    }

    protected int GetStudyAreaCVO() => (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area");

    protected int GetTotalTreeCVO() => (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total");

    protected virtual SortedList<short, string> GetConvertedDbhRanges() => this.estUtil.ConvertDBHRangesToEnglish(ReportBase.EnglishUnits);

    protected virtual void GetDBHColumnIndexes(SortedList<short, string> convertedDbhRanges)
    {
      this._dbhColumnIndexes = new Dictionary<short, int>();
      for (int index = 0; index < convertedDbhRanges.Count; ++index)
        this._dbhColumnIndexes.Add(convertedDbhRanges.Keys[index], index * 2);
    }

    protected SortedList<int, SortedList<int, SortedList<int, Tuple<double, double>>>> GetStrataDBHData()
    {
      SortedList<int, SortedList<int, SortedList<int, Tuple<double, double>>>> strataDbhData = new SortedList<int, SortedList<int, SortedList<int, Tuple<double, double>>>>();
      DataTable multipleFieldsData = this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species,
        Classifiers.CDBH
      }, EstimateTypeEnum.NumberofTrees, Units.Percent, Units.None, Units.None);
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      string classifierName3 = this.estUtil.ClassifierNames[Classifiers.CDBH];
      string columnName1 = "EstimateValue";
      string columnName2 = "EstimateStandardError";
      foreach (DataRow row in (InternalDataCollectionBase) multipleFieldsData.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        int key3 = ReportUtil.ConvertFromDBVal<int>(row[classifierName3]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row[columnName1]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row[columnName2]);
        if (!this._speciesList.Contains((short) key2))
          this._speciesList.Add((short) key2);
        if (!strataDbhData.ContainsKey(key1))
          strataDbhData.Add(key1, new SortedList<int, SortedList<int, Tuple<double, double>>>());
        if (!strataDbhData[key1].ContainsKey(key2))
          strataDbhData[key1].Add(key2, new SortedList<int, Tuple<double, double>>());
        if (!strataDbhData[key1][key2].ContainsKey(key3))
          strataDbhData[key1][key2].Add(key3, Tuple.Create<double, double>(num1, num2));
      }
      this.GetDBHColumnIndexesAndRanges();
      return strataDbhData;
    }

    protected SortedList<int, SortedList<int, Tuple<double, double>>> GetDBHData()
    {
      SortedList<int, SortedList<int, Tuple<double, double>>> dbhData = new SortedList<int, SortedList<int, Tuple<double, double>>>();
      DataTable multipleFieldsData = this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Species,
        Classifiers.CDBH
      }, EstimateTypeEnum.NumberofTrees, Units.Percent, Units.None, Units.None);
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Species];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.CDBH];
      string columnName1 = "EstimateValue";
      string columnName2 = "EstimateStandardError";
      foreach (DataRow row in (InternalDataCollectionBase) multipleFieldsData.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row[columnName1]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row[columnName2]);
        if (!dbhData.ContainsKey(key1))
          dbhData.Add(key1, new SortedList<int, Tuple<double, double>>());
        if (!dbhData[key1].ContainsKey(key2))
          dbhData[key1].Add(key2, Tuple.Create<double, double>(num1, num2));
      }
      this.GetDBHColumnIndexesAndRanges();
      return dbhData;
    }

    protected virtual void SpeciesDBHDataIntoTable(
      SortedList<int, Tuple<double, double>> curSpeciesDic,
      RenderTable rTable,
      int startDbhIdex,
      int endDbhIndex,
      int curRow)
    {
      for (int index = 0; index < curSpeciesDic.Count; ++index)
      {
        short key = (short) curSpeciesDic.Keys[index];
        int num = this._convertedDbhRanges.Keys.IndexOf(key);
        if (num >= startDbhIdex && num <= endDbhIndex)
        {
          int col = this._dbhColumnIndexes[key] - startDbhIdex * 2 + 2;
          Tuple<double, double> tuple = curSpeciesDic.Values[index];
          rTable.Cells[curRow, col].Text = tuple.Item1.ToString(this._percentFormat);
          rTable.Cells[curRow, col + 1].Text = tuple.Item2.ToString(this._percentFormat);
        }
      }
    }

    protected void HeaderDBH(RenderTable rTable, int startDbhIdex, int endDbhIndex)
    {
      rTable.Cells[0, 2].SpanCols = (endDbhIndex - startDbhIdex) * 2 + 1;
      rTable.Cells[0, 2].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.DBHClass, ReportBase.CmUnits());
      int num1 = 0;
      for (int index = startDbhIdex; index <= endDbhIndex; ++index)
      {
        int num2 = num1 * 2 + 2;
        rTable.Cells[1, num2].SpanCols = 2;
        rTable.Cells[1, num2].Text = this._convertedDbhRanges.Values[index];
        rTable.Cells[2, num2].Text = "%";
        rTable.Cells[2, num2 + 1].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
        rTable.ColGroups[num2, 2].SplitBehavior = SplitBehaviorEnum.Never;
        ++num1;
      }
    }

    protected RenderTable RenderTableAndHeader(C1PrintDocument C1doc)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.BreakBefore = BreakEnum.Page;
      renderTable.Width = Unit.Auto;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      renderTable.RowGroups[0, 3].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, 3].Style.FontSize = 12f;
      renderTable.RowGroups[0, 3].Style.TextAlignHorz = AlignHorzEnum.Center;
      ReportUtil.FormatRenderTableHeader(renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[1, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      return renderTable;
    }
  }
}
