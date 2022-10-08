// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeAvoidedRunoff
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
  public class IndividualTreeAvoidedRunoff : DatabaseReport
  {
    private DataTable _dt;
    protected int headerRows = 2;

    public IndividualTreeAvoidedRunoff()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleHydrologyEffectsByMeasuredTrees;
      this.hasComments = true;
      this.hasCoordinates = this.curYear.RecordTreeGPS;
      this.hasUID = this.curYear.RecordTreeUserId;
    }

    public override object GetData()
    {
      if (this._dt == null)
      {
        this._dt = this.curInputISession.GetNamedQuery(nameof (IndividualTreeAvoidedRunoff)).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
        int columnIndex = this._dt.Columns.Count - 5;
        int num = columnIndex + 1;
        this._dt.Columns.Add("AvoidedRunoffValue", typeof (double)).SetOrdinal(num);
        foreach (DataRow row in (InternalDataCollectionBase) this._dt.Rows)
          row[num] = (object) ((double) row[columnIndex] * this.customizedWaterDollarsPerM3);
      }
      return (object) this._dt;
    }

    protected void Header(RenderTable rTable)
    {
      rTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlotID;
      rTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeID;
      rTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
      rTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.LeafArea;
      rTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PotentialEvapotranspiration;
      rTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.Evaporation;
      rTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.Transpiration;
      rTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.WaterIntercepted;
      rTable.Cells[0, 8].Text = i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff;
      rTable.Cells[0, 9].Text = i_Tree_Eco_v6.Resources.Strings.AvoidedRunoffValue;
      rTable.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(this.SquareMeterUnits());
      rTable.Cells[1, 4].Text = rTable.Cells[1, 5].Text = rTable.Cells[1, 6].Text = rTable.Cells[1, 7].Text = rTable.Cells[1, 8].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.CubicMeterUnits());
      rTable.Cells[1, 9].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      rTable.Cells[0, 10].Text = i_Tree_Eco_v6.Resources.Strings.xCoordinate;
      rTable.Cells[0, 11].Text = i_Tree_Eco_v6.Resources.Strings.yCoordinate;
      rTable.Cells[0, 12].Text = i_Tree_Eco_v6.Resources.Strings.Comments;
      rTable.Cells[0, 13].Text = i_Tree_Eco_v6.Resources.Strings.UserID;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      C1doc.ClipPage = true;
      this.Header(renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = renderTable.Cols[2].Style.TextAlignHorz = renderTable.Cols[12].Style.TextAlignHorz = renderTable.Cols[13].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[4].Style.Borders.Right = renderTable.Cols[7].Style.Borders.Right = renderTable.Cols[10].Style.Borders.Left = renderTable.Cols[12].Style.Borders.Left = renderTable.Cols[13].Style.Borders.Left = this.tableBorderLineGray;
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      Style style = ReportUtil.SetDefaultReportFormatting(C1doc, renderTable, this.curYear, this.headerRows);
      renderTable.Cols[0].Visible = ReportBase.plotInventory;
      renderTable.Cols[10].Visible = renderTable.Cols[11].Visible = this.hasCoordinates && ReportBase.m_ps.ShowGPS;
      renderTable.Cols[12].Visible = ReportBase.m_ps.ShowComments;
      renderTable.Cols[13].Visible = this.hasUID && ReportBase.m_ps.ShowUID;
      DataTable data = (DataTable) this.GetData();
      List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
      int headerRows = this.headerRows;
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        foreach (ColumnFormat columnFormat in columnFormatList)
          renderTable.Cells[headerRows, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
        if ((headerRows - this.headerRows) % 2 == 0)
          renderTable.Rows[headerRows].Style.Parent = style;
        ++headerRows;
      }
      renderTable.Cells[headerRows, 2].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      foreach (ColumnFormat columnFormat in columnFormatList)
      {
        ColumnFormat cf = columnFormat;
        if (cf.FormatTotals != null)
          renderTable.Cells[headerRows, cf.ColNum].Text = cf.Format((object) data.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (x => x.Field<double>(cf.ColName))));
      }
      renderTable.Rows[headerRows].Style.FontBold = true;
      renderTable.Rows[headerRows].Style.Borders.Top = LineDef.Default;
      this.Note(C1doc);
    }

    protected override string ReportMessage() => this.AvoidedRunoff_m3_ft3_Footer();

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
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, this.SquareMeterUnits()),
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleSquaremeter),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleSquaremeter),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.PotentialEvapotranspiration, ReportUtil.GetValuePerYrStr(ReportBase.CubicMeterUnits())),
        ColName = data.Columns[5].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        ColNum = 4
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Evaporation, ReportUtil.GetValuePerYrStr(ReportBase.CubicMeterUnits())),
        ColName = data.Columns[6].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        ColNum = 5
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Transpiration, ReportUtil.GetValuePerYrStr(ReportBase.CubicMeterUnits())),
        ColName = data.Columns[7].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        ColNum = 6
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.WaterIntercepted, ReportUtil.GetValuePerYrStr(ReportBase.CubicMeterUnits())),
        ColName = data.Columns[8].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        ColNum = 7
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff, ReportUtil.GetValuePerYrStr(ReportBase.CubicMeterUnits())),
        ColName = data.Columns[9].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleVolumeCubicMeter1),
        ColNum = 8
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.AvoidedRunoffValue, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[10].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 9
      });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[11].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 10
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[12].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 11
        });
      }
      if (ReportBase.m_ps.ShowComments)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Comments,
          ColName = data.Columns[13].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 12
        });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.UserID,
          ColName = data.Columns[14].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 13
        });
      return columnFormatList;
    }
  }
}
