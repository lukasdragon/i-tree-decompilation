// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeBioemissions
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeBioemissions : DatabaseReport
  {
    public IndividualTreeBioemissions()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleVOCEmissionsByMeasuredTrees;
      this.hasComments = true;
      this.hasCoordinates = this.curYear.RecordTreeGPS;
      this.hasUID = this.curYear.RecordTreeUserId;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Style.Font = new Font("Calibri", 10f);
      renderTable.Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      ReportUtil.FormatRenderTableHeader(renderTable);
      Style style = ReportUtil.AddAlternateStyle(renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = renderTable.Cols[2].Style.TextAlignHorz = renderTable.Cols[8].Style.TextAlignHorz = renderTable.Cols[9].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlotID;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeID;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.Isoprene;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.Monoterpene;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.VOCS;
      renderTable.Cells[1, 3].Text = renderTable.Cells[1, 4].Text = renderTable.Cells[1, 5].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.GUnits());
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.xCoordinate;
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.yCoordinate;
      renderTable.Cells[0, 8].Text = i_Tree_Eco_v6.Resources.Strings.Comments;
      renderTable.Cells[0, 9].Text = i_Tree_Eco_v6.Resources.Strings.UserID;
      renderTable.Cols[0].Visible = ReportBase.plotInventory;
      renderTable.Cols[6].Visible = renderTable.Cols[7].Visible = this.hasCoordinates && ReportBase.m_ps.ShowGPS;
      renderTable.Cols[8].Visible = ReportBase.m_ps.ShowComments;
      renderTable.Cols[9].Visible = this.hasUID && ReportBase.m_ps.ShowUID;
      DataTable dataTable = this.curInputISession.GetNamedQuery(nameof (IndividualTreeBioemissions)).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      List<ColumnFormat> columnFormatList = this.ColumnsFormat(dataTable);
      int num = count;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        foreach (ColumnFormat columnFormat in columnFormatList)
          renderTable.Cells[num, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
        if ((num - count) % 2 == 0)
          renderTable.Rows[num].Style.Parent = style;
        ++num;
      }
      renderTable.Cells[num, 2].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      foreach (ColumnFormat columnFormat in columnFormatList)
      {
        ColumnFormat cf = columnFormat;
        if (cf.FormatTotals != null)
          renderTable.Cells[num, cf.ColNum].Text = cf.Format((object) dataTable.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (x => x.Field<double>(cf.ColName))));
      }
      renderTable.Rows[num].Style.FontBold = true;
      renderTable.Rows[num].Style.Borders.Top = LineDef.Default;
    }

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList = new List<ColumnFormat>();
      if (ReportBase.plotInventory)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.PlotID,
          ColName = data.Columns[0].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatInt),
          ColNum = 0
        });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.TreeID,
        ColName = data.Columns[1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatInt),
        ColNum = 1
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.SpeciesName,
        ColName = ReportBase.ScientificName ? data.Columns[2].ColumnName : data.Columns[3].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 2
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Isoprene, ReportUtil.GetValuePerYrStr(ReportBase.GUnits())),
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Monoterpene, ReportUtil.GetValuePerYrStr(ReportBase.GUnits())),
        ColName = data.Columns[5].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 4
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.VOCS, ReportUtil.GetValuePerYrStr(ReportBase.GUnits())),
        ColName = data.Columns[6].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 5
      });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[7].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 6
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[8].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 7
        });
      }
      if (ReportBase.m_ps.ShowComments)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Comments,
          ColName = data.Columns[9].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 8
        });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.UserID,
          ColName = data.Columns[10].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 9
        });
      return columnFormatList;
    }
  }
}
