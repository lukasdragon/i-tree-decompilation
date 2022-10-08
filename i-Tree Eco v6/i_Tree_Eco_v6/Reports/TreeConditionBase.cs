// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.TreeConditionBase
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
  internal class TreeConditionBase : DatabaseReport
  {
    public SortedList<short, string> diebackOdersAndDescriptions;
    public Classifiers diebackClass;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable data = (DataTable) this.GetData();
      if (data.Rows.Count <= 0)
      {
        this.DisplayStandardMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgNoData);
      }
      else
      {
        C1doc.PageLayout.PageSettings.Landscape = false;
        RenderTable renderTable = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable);
        int num1 = 2;
        renderTable.RowGroups[0, num1].Header = TableHeaderEnum.Page;
        renderTable.RowGroups[0, num1].Style.FontSize = 12f;
        ReportUtil.FormatRenderTableHeader(renderTable);
        renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
        renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
        renderTable.BreakAfter = BreakEnum.None;
        renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
        renderTable.Width = Unit.Auto;
        renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
        Style style = ReportUtil.SetDefaultReportFormatting(C1doc, renderTable, this.curYear, num1);
        List<ColumnFormat> columnsFormat = this.ColumnsFormat(data);
        this.ApplyHeader(renderTable, columnsFormat);
        int num2 = num1;
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          foreach (ColumnFormat columnFormat in columnsFormat)
            renderTable.Cells[num2, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
          if ((num2 - num1) % 2 == 0)
            renderTable.Rows[num2].Style.Parent = style;
          if (row[0].ToString() == "Total")
          {
            renderTable.Rows[num2].Style.FontBold = true;
            renderTable.Rows[num2].Style.Borders.Top = LineDef.Default;
            renderTable.Cells[num2, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
          }
          ++num2;
        }
      }
    }

    public override object GetData()
    {
      DataTable pivotedData = this.PivotOnDiebacks(this.GetTreeConditions());
      this.AppendTotalRow(pivotedData);
      return (object) pivotedData;
    }

    private DataTable GetTreeConditions() => this.GetMultipleFieldsData2().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetMultipleFieldsData2()
    {
      List<Classifiers> source = new List<Classifiers>()
      {
        Classifiers.Species,
        this.diebackClass
      };
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetMultipleFieldsData2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, source)], source.Select<Classifiers, string>((Func<Classifiers, string>) (c => this.estUtil.ClassifierNames[c])).ToArray<string>());
    }

    private DataTable GetTotals() => this.GetEstimatesQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetSignificantTotalsEstimateValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Species,
      this.diebackClass
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[this.diebackClass]);

    public DataTable PivotOnDiebacks(DataTable data)
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Species];
      DataTable pivotTable = this.createPivotTable(classifierName1);
      IEnumerable<IGrouping<int, DataRow>> speciesGroups = this.GetSpeciesGroups(data, classifierName1);
      string classifierName2 = this.estUtil.ClassifierNames[this.diebackClass];
      foreach (IGrouping<int, DataRow> grouping in speciesGroups)
      {
        DataRow newRow = pivotTable.Rows.Add();
        short key = (short) grouping.Key;
        newRow[classifierName1] = ReportBase.ScientificName ? (object) this.estUtil.ClassValues[Classifiers.Species][key].Item2 : (object) this.estUtil.ClassValues[Classifiers.Species][key].Item1;
        foreach (DataRow row in (IEnumerable<DataRow>) grouping)
          this.AssignDieback(row, newRow, classifierName2);
      }
      return pivotTable;
    }

    protected IEnumerable<IGrouping<int, DataRow>> GetSpeciesGroups(
      DataTable data,
      string speciesColumnName)
    {
      return data.AsEnumerable().GroupBy<DataRow, int>((Func<DataRow, int>) (x => x.Field<int>(speciesColumnName)));
    }

    private DataTable createPivotTable(string speciesColumnName)
    {
      DataTable pivotTable = new DataTable();
      pivotTable.Columns.Add(speciesColumnName, typeof (object));
      foreach (int key in (IEnumerable<short>) this.diebackOdersAndDescriptions.Keys)
      {
        string str = key.ToString();
        pivotTable.Columns.Add(str, typeof (object));
        string seColumnName = this.GetSEColumnName(str);
        pivotTable.Columns.Add(seColumnName, typeof (object));
      }
      return pivotTable;
    }

    protected SortedList<short, string> GetSortedDiebackOdersWithDescriptions()
    {
      SortedList<short, string> withDescriptions = new SortedList<short, string>();
      foreach (KeyValuePair<short, Tuple<string, string>> keyValuePair in this.estUtil.ClassValues[this.diebackClass])
      {
        string translatedName = this.GetTranslatedName(keyValuePair.Value.Item1);
        withDescriptions.Add(keyValuePair.Key, translatedName);
      }
      return withDescriptions;
    }

    private void AppendTotalRow(DataTable pivotedData)
    {
      DataTable totals = this.GetTotals();
      if (totals.Rows.Count <= 0)
        return;
      DataRow newRow = pivotedData.Rows.Add();
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Species];
      newRow[classifierName1] = (object) "Total";
      string classifierName2 = this.estUtil.ClassifierNames[this.diebackClass];
      foreach (DataRow row in (InternalDataCollectionBase) totals.Rows)
        this.AssignDieback(row, newRow, classifierName2);
    }

    private void AssignDieback(DataRow row, DataRow newRow, string diebackColumnName)
    {
      string str = row[diebackColumnName].ToString();
      newRow[str] = row["EstimateValue"];
      newRow[this.GetSEColumnName(str)] = row["EstimateStandardError"];
    }

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList = new List<ColumnFormat>();
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = v6Strings.Tree_Species,
        ColName = data.Columns[0].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 0
      });
      int num1 = 0;
      int index1 = 1;
      int num2 = this.diebackOdersAndDescriptions.Count * 2 + num1;
      string empty = string.Empty;
      short index2 = 1;
      while (num1 < num2)
      {
        string columnHeader1 = this.GetColumnHeader(this.diebackOdersAndDescriptions, "%", index2);
        int num3 = num1 + 1;
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = columnHeader1,
          ColName = data.Columns[index1].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue1),
          FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue1),
          ColNum = num3
        });
        string columnHeader2 = this.GetColumnHeader(this.diebackOdersAndDescriptions, i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr, index2);
        num1 = num3 + 1;
        int index3 = index1 + 1;
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = columnHeader2,
          ColName = data.Columns[index3].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue1),
          FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue1),
          ColNum = num1
        });
        index1 = index3 + 1;
        ++index2;
      }
      return columnFormatList;
    }

    private string GetColumnHeader(
      SortedList<short, string> diebackOderAndDescription,
      string unit,
      short index)
    {
      return ReportBase.csvExport ? ReportUtil.FormatInlineHeaderUnitsStr(diebackOderAndDescription[index], unit) : unit;
    }

    private string GetSEColumnName(string diebackClass) => string.Format("{0}(1)", (object) diebackClass, (object) i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr);

    protected void ApplyHeader(RenderTable rTable, List<ColumnFormat> columnsFormat)
    {
      int row = 0;
      rTable.Cells[row, 0].Text = columnsFormat[0].HeaderText;
      int num1 = 0;
      foreach (KeyValuePair<short, string> odersAndDescription in this.diebackOdersAndDescriptions)
      {
        int num2 = 2;
        int num3 = num1 + (int) odersAndDescription.Key;
        rTable.Cells[0, num3].SpanCols = num2;
        rTable.Cells[0, num3].Text = odersAndDescription.Value;
        rTable.Cells[1, num3].Text = columnsFormat[num3].HeaderText;
        int num4 = num3 + 1;
        rTable.Cells[1, num4].Text = columnsFormat[num4].HeaderText;
        ++num1;
      }
    }

    protected virtual string GetTranslatedName(string name) => name;
  }
}
