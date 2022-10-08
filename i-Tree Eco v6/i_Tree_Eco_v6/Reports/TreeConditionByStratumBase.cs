// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.TreeConditionByStratumBase
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
  internal class TreeConditionByStratumBase : DatabaseReport
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
        renderTable.Cols[0].Style.FontBold = true;
        renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
        renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
        renderTable.BreakAfter = BreakEnum.None;
        renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
        renderTable.Width = Unit.Auto;
        renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
        Style style = ReportUtil.SetDefaultReportFormatting(C1doc, renderTable, this.curYear, num1);
        List<ColumnFormat> columnsFormat = this.ColumnsFormat(data);
        this.ApplyHeader(renderTable, columnsFormat);
        int valueOrderFromName1 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area");
        int valueOrderFromName2 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total");
        int num2 = num1;
        string empty = string.Empty;
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          string str1 = row[0].ToString();
          string str2 = row[1].ToString();
          foreach (ColumnFormat columnFormat in columnsFormat)
          {
            renderTable.Cells[num2, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
            if (empty == str1)
              renderTable.Cells[num2, 0].Text = string.Empty;
          }
          if (str2 == "Total")
          {
            renderTable.Rows[num2].Style.FontBold = true;
            renderTable.Rows[num2].Style.Borders.Bottom = renderTable.Rows[num2].Style.Borders.Top = LineDef.Default;
            renderTable.Cells[num2, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
            if (str1 == "Study Area")
            {
              renderTable.Cells[num2, 1].Text = string.Empty;
              renderTable.Cells[num2, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
            }
          }
          if ((num2 - num1) % 2 == 0)
            renderTable.Rows[num2].Style.Parent = style;
          empty = row[0].ToString();
          ++num2;
        }
      }
    }

    public override object GetData() => (object) this.PivotOnDiebacks(this.GetMultipleFieldsData().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>());

    private IQuery GetMultipleFieldsData()
    {
      List<Classifiers> source = new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species,
        this.diebackClass
      };
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetMultipleFieldsData2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, source)], source.Select<Classifiers, string>((Func<Classifiers, string>) (f => this.estUtil.ClassifierNames[f])).ToArray<string>());
    }

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList = new List<ColumnFormat>();
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = v6Strings.Strata_SingularName,
        ColName = data.Columns[0].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 0
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = v6Strings.Tree_Species,
        ColName = data.Columns[1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 1
      });
      int num1 = 1;
      int index1 = 2;
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
      rTable.Cells[row, 1].Text = columnsFormat[1].HeaderText;
      int num1 = 1;
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

    public DataTable PivotOnDiebacks(DataTable data)
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      string classifierName3 = this.estUtil.ClassifierNames[this.diebackClass];
      return this.populatePivotTable(data, classifierName1, classifierName2, classifierName3);
    }

    private DataTable populatePivotTable(
      DataTable data,
      string strataColumnName,
      string speciesColumnName,
      string diebackColumnName)
    {
      DataTable pivotTable = this.createPivotTable(strataColumnName, speciesColumnName);
      foreach (IGrouping<object, DataRow> source in this.groupBy(data, strataColumnName))
      {
        IEnumerable<IGrouping<object, DataRow>> orderedAndGrouped = (IEnumerable<IGrouping<object, DataRow>>) this.groupBy(this.getSpeciesInStrata(data, source.ToList<DataRow>()), speciesColumnName).OrderBy<IGrouping<object, DataRow>, short>((Func<IGrouping<object, DataRow>, short>) (x => Convert.ToInt16(x.Key)));
        this.appendData(pivotTable, orderedAndGrouped, diebackColumnName, strataColumnName, speciesColumnName, Convert.ToInt16(source.Key));
      }
      return pivotTable;
    }

    protected DataTable getSpeciesInStrata(DataTable data, List<DataRow> strataGoupRow)
    {
      DataTable speciesInStrata = data.Clone();
      speciesInStrata.Columns.RemoveAt(0);
      speciesInStrata.Columns[0].DataType = typeof (short);
      foreach (DataRow dataRow in strataGoupRow)
      {
        object[] array = ((IEnumerable<object>) dataRow.ItemArray).Skip<object>(1).ToArray<object>();
        speciesInStrata.Rows.Add(array);
      }
      return speciesInStrata;
    }

    protected void appendData(
      DataTable data,
      IEnumerable<IGrouping<object, DataRow>> orderedAndGrouped,
      string diebackColumnName,
      string strataColumnName,
      string speciesColumnName,
      short strata)
    {
      foreach (IGrouping<object, DataRow> grouping in orderedAndGrouped)
      {
        DataRow dataRow = data.Rows.Add();
        short int16_1 = Convert.ToInt16(grouping.Key);
        dataRow[speciesColumnName] = ReportBase.ScientificName ? (object) this.estUtil.ClassValues[Classifiers.Species][int16_1].Item2 : (object) this.estUtil.ClassValues[Classifiers.Species][int16_1].Item1;
        foreach (DataRow row in (IEnumerable<DataRow>) grouping)
        {
          string str = row.Field<int>(diebackColumnName).ToString();
          short int16_2 = Convert.ToInt16(strata);
          dataRow[strataColumnName] = this.curYear.RecordStrata ? (object) this.estUtil.ClassValues[Classifiers.Strata][int16_2].Item1 : (object) i_Tree_Eco_v6.Resources.Strings.StudyArea;
          dataRow[str] = (object) row.Field<double>("EstimateValue");
          dataRow[this.GetSEColumnName(str)] = (object) row.Field<double>("EstimateStandardError");
        }
      }
    }

    protected IEnumerable<IGrouping<object, DataRow>> groupBy(
      DataTable data,
      string gorupByColumn)
    {
      return data.AsEnumerable().GroupBy<DataRow, object>((Func<DataRow, object>) (x => x.Field<object>(gorupByColumn)));
    }

    private DataTable createPivotTable(string strataColumnName, string speciesColumnName)
    {
      DataTable pivotTable = new DataTable();
      pivotTable.Columns.Add(strataColumnName, typeof (string));
      pivotTable.Columns.Add(speciesColumnName, typeof (string));
      foreach (int key in (IEnumerable<short>) this.diebackOdersAndDescriptions.Keys)
      {
        string str = key.ToString();
        pivotTable.Columns.Add(str, typeof (double));
        string seColumnName = this.GetSEColumnName(str);
        pivotTable.Columns.Add(seColumnName, typeof (double));
      }
      return pivotTable;
    }

    protected SortedList<short, string> getSortedDiebackOdersWithDescriptions()
    {
      SortedList<short, string> withDescriptions = new SortedList<short, string>();
      foreach (KeyValuePair<short, Tuple<string, string>> keyValuePair in this.estUtil.ClassValues[this.diebackClass])
      {
        string translatedName = this.getTranslatedName(keyValuePair.Value.Item1);
        withDescriptions.Add(keyValuePair.Key, translatedName);
      }
      return withDescriptions;
    }

    protected virtual string getTranslatedName(string name) => name;
  }
}
