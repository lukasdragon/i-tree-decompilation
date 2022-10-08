// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeCarbonBase
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

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeCarbonBase : DatabaseReport
  {
    private double _totCarbon;
    protected string _column = string.Empty;

    protected virtual void Init()
    {
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.Init();
      DataTable data = (DataTable) this.GetData();
      if (data == null)
      {
        this.DisplayStandardMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgNoData);
      }
      else
      {
        RenderTable renderTable = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable);
        renderTable.Style.Font = new Font("Calibri", 12f);
        renderTable.Style.TextAlignHorz = AlignHorzEnum.Right;
        renderTable.BreakAfter = BreakEnum.None;
        renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
        renderTable.Width = Unit.Auto;
        renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
        int count = 1;
        renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
        ReportUtil.FormatRenderTableHeader(renderTable);
        Style style = ReportUtil.AddAlternateStyle(renderTable);
        renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = renderTable.Cols[2].Style.TextAlignHorz = renderTable.Cols[7].Style.TextAlignHorz = renderTable.Cols[8].Style.TextAlignHorz = AlignHorzEnum.Left;
        renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlotID;
        renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeID;
        renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
        renderTable.Cells[0, 3].Text = this.GetColumnTitle();
        renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfTotal;
        renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.xCoordinate;
        renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.yCoordinate;
        renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.Comments;
        renderTable.Cells[0, 8].Text = i_Tree_Eco_v6.Resources.Strings.UserID;
        renderTable.Cols[0].Visible = ReportBase.plotInventory;
        renderTable.Cols[5].Visible = renderTable.Cols[6].Visible = this.curYear.RecordTreeGPS && ReportBase.m_ps.ShowGPS;
        renderTable.Cols[7].Visible = ReportBase.m_ps.ShowComments;
        renderTable.Cols[8].Visible = this.hasUID && ReportBase.m_ps.ShowUID;
        List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
        int num = count;
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          foreach (ColumnFormat columnFormat in columnFormatList)
            renderTable.Cells[num, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
          if ((num - count) % 2 == 0)
            renderTable.Rows[num].Style.Parent = style;
          ++num;
        }
        renderTable.Cells[num, 2].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        renderTable.Cells[num, 3].Text = ReportBase.FormatDoubleWeight((object) this._totCarbon);
        renderTable.Cells[num, 4].Text = "100%";
        renderTable.Rows[num].Style.FontBold = true;
        renderTable.Rows[num].Style.Borders.Top = LineDef.Default;
        this.Note(C1doc);
      }
    }

    protected virtual string GetColumnTitle() => string.Empty;

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
        HeaderText = this.GetColumnTitle(),
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.PercentOfTotal,
        ColName = data.Columns[5].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue1),
        ColNum = 4
      });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[6].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 5
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[7].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 6
        });
      }
      if (ReportBase.m_ps.ShowComments)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Comments,
          ColName = data.Columns[8].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 7
        });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.UserID,
          ColName = data.Columns[9].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 8
        });
      return columnFormatList;
    }

    public override object GetData()
    {
      object obj = (object) this.estUtil.queryProvider.GetEstimateUtilProvider().GetCarbonSum(this._column).SetParameter<Guid>("y", this.YearGuid).UniqueResult<double>();
      if (obj.GetType() != typeof (double) || (double) obj == 0.0)
        return (object) null;
      this._totCarbon = (double) obj;
      return (object) this.estUtil.queryProvider.GetEstimateUtilProvider().GetCarbonData(this._column).SetParameter<Guid>("y", this.YearGuid).SetParameter<double>("tc", this._totCarbon).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
    }
  }
}
