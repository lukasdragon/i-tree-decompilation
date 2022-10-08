// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.RelativePerformanceIndex
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class RelativePerformanceIndex : RelativePerformanceIndexData
  {
    public RelativePerformanceIndex() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleRelativePerformanceIndexBySpecies;

    public override object GetData() => (object) this.GetStats();

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable data = (DataTable) this.GetData();
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      int num1 = 2;
      renderTable.RowGroups[0, num1].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, num1].Style.FontSize = 12f;
      ReportUtil.FormatRenderTableHeader(renderTable);
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      int index = 1 + this.healthClasses.Count + 2;
      renderTable.Cols[index].Style.TextAlignHorz = AlignHorzEnum.Left;
      List<ColumnFormat> columnsFormat = this.ColumnsFormat(data);
      this.ApplyHeader(renderTable, columnsFormat);
      Style style = ReportUtil.SetDefaultReportFormatting(C1doc, renderTable, this.curYear, num1);
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

    protected void ApplyHeader(RenderTable rTable, List<ColumnFormat> columnsFormat)
    {
      int row1 = 0;
      foreach (ColumnFormat columnFormat in columnsFormat)
        rTable.Cells[row1, columnFormat.ColNum].Text = columnFormat.HeaderText;
      int row2 = row1 + 1;
      int num = 1;
      for (int index = 0; index < this.healthClasses.Count; ++index)
        rTable.Cells[row2, num + index].Text = ReportUtil.FormatHeaderUnitsStr("%");
    }

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList1 = new List<ColumnFormat>();
      SortedDictionary<string, HealthRptClass> sortedDictionary = new SortedDictionary<string, HealthRptClass>((IDictionary<string, HealthRptClass>) this.curYear.HealthRptClasses.ToDictionary<HealthRptClass, string, HealthRptClass>((Func<HealthRptClass, string>) (x => x.Id.ToString()), (Func<HealthRptClass, HealthRptClass>) (x => x)));
      columnFormatList1.Add(new ColumnFormat()
      {
        HeaderText = v6Strings.Tree_Species,
        ColName = data.Columns[0].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 0
      });
      int num1 = 0;
      int index1 = num1 + 1;
      int count = sortedDictionary.Count;
      int num2 = num1 + count;
      string empty = string.Empty;
      short num3 = 1;
      while (num1 < num2)
      {
        string columnName = data.Columns[index1].ColumnName;
        string description = sortedDictionary[columnName].Description;
        List<ColumnFormat> columnFormatList2 = columnFormatList1;
        ColumnFormat columnFormat = new ColumnFormat();
        columnFormat.HeaderText = this.GetColumnWithUnitsHeader(description);
        columnFormat.ColName = columnName;
        columnFormat.Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue1);
        int num4;
        num1 = num4 = num1 + 1;
        columnFormat.ColNum = num4;
        columnFormatList2.Add(columnFormat);
        ++index1;
        ++num3;
      }
      int num5 = num2;
      int num6;
      columnFormatList1.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.RPI,
        ColName = data.Columns[num6 = num5 + 1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = num6
      });
      int num7;
      columnFormatList1.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.NumberOfTrees,
        ColName = data.Columns[num7 = num6 + 1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatCount),
        ColNum = num7
      });
      int index2 = num7 + 1;
      if (this.series.IsSample)
        columnFormatList1.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.StandardError,
          ColName = data.Columns[index2].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatSE),
          ColNum = index2
        });
      int num8;
      columnFormatList1.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.PercentOfTrees,
        ColName = data.Columns[num8 = index2 + 1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue1),
        ColNum = num8
      });
      return columnFormatList1;
    }

    private string GetColumnWithUnitsHeader(string header) => ReportBase.csvExport ? string.Format("{0}(%)", (object) header) : header;
  }
}
