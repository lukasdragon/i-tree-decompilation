// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.CommentsBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class CommentsBase : DatabaseReport
  {
    public virtual bool ShowComments => ReportBase.m_ps.ShowComments;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      int num1 = ReportUtil.FormatRenderTableHeader(renderTable);
      Style style = ReportUtil.AddAlternateStyle(renderTable);
      DataTable data = (DataTable) this.GetData();
      if (data == null)
      {
        this.hasComments = false;
        this.hasCoordinates = false;
        this.hasUID = false;
        this.DisplayStandardMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgNoNotesOrComments);
      }
      else
      {
        List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
        foreach (ColumnFormat columnFormat in columnFormatList)
          renderTable.Cells[0, columnFormat.ColNum].Text = columnFormat.HeaderText;
        int num2 = count;
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          foreach (ColumnFormat columnFormat in columnFormatList)
            renderTable.Cells[num2, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
          if ((num2 - num1) % 2 == 0)
            renderTable.Rows[num2].Style.Parent = style;
          ++num2;
        }
      }
    }
  }
}
