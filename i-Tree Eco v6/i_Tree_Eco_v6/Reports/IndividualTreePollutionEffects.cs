// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreePollutionEffects
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
  public class IndividualTreePollutionEffects : DatabaseReport
  {
    private string gUnits = ReportBase.GUnits();

    public IndividualTreePollutionEffects()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitlePollutionRemovalByMeasuredTrees;
      this.hasComments = true;
      this.hasCoordinates = this.curYear.RecordTreeGPS;
      this.hasUID = this.curYear.RecordTreeUserId;
    }

    public override object GetData() => (object) this.curInputISession.GetNamedQuery(nameof (IndividualTreePollutionEffects)).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("tonToGrams", 1000000).SetParameter<double>("CoDollars", this.customizedCoDollarsPerTon).SetParameter<double>("O3Dollars", this.customizedO3DollarsPerTon).SetParameter<double>("NO2Dollars", this.customizedNO2DollarsPerTon).SetParameter<double>("SO2Dollars", this.customizedSO2DollarsPerTon).SetParameter<double>("PM10Dollars", this.customizedPM10DollarsPerTon).SetParameter<double>("PM25Dollars", this.customizedPM25DollarsPerTon).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.ClipPage = true;
      C1doc.PageLayout.PageSettings.Landscape = false;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Style.Font = new Font("Calibri", 9f);
      renderTable.Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      ReportUtil.FormatRenderTableHeader(renderTable);
      Style style = ReportUtil.AddAlternateStyle(renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = renderTable.Cols[2].Style.TextAlignHorz = renderTable.Cols[17].Style.TextAlignHorz = renderTable.Cols[18].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[3].Style.Borders.Left = renderTable.Cols[9].Style.Borders.Left = renderTable.Cols[14].Style.Borders.Right = renderTable.Cols[17].Style.Borders.Left = renderTable.Cols[18].Style.Borders.Left = this.tableBorderLineGray;
      renderTable.Cells[0, 3].SpanCols = 5;
      renderTable.Cells[0, 3].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.PollutionRemoved, ReportUtil.GetValuePerYrStr(ReportBase.GUnits()));
      renderTable.Cells[0, 9].SpanCols = 5;
      renderTable.Cells[0, 9].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.RemovalValue, ReportUtil.GetValuePerYrStr(this.CurrencySymbol));
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlotID;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeID;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
      renderTable.Cells[1, 3].Text = i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideFormula;
      renderTable.Cells[1, 4].Text = i_Tree_Eco_v6.Resources.Strings.OzoneFurmula;
      renderTable.Cells[1, 5].Text = i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula;
      renderTable.Cells[1, 6].Text = i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula;
      renderTable.Cells[1, 7].Text = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter10Furmula;
      renderTable.Cells[1, 8].Text = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula;
      renderTable.Cells[1, 9].Text = i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideFormula;
      renderTable.Cells[1, 10].Text = i_Tree_Eco_v6.Resources.Strings.OzoneFurmula;
      renderTable.Cells[1, 11].Text = i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula;
      renderTable.Cells[1, 12].Text = i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula;
      renderTable.Cells[1, 13].Text = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter10Furmula;
      renderTable.Cells[1, 14].Text = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula;
      renderTable.Cells[0, 15].Text = i_Tree_Eco_v6.Resources.Strings.xCoordinate;
      renderTable.Cells[0, 16].Text = i_Tree_Eco_v6.Resources.Strings.yCoordinate;
      renderTable.Cells[0, 17].Text = i_Tree_Eco_v6.Resources.Strings.Comments;
      renderTable.Cells[0, 18].Text = i_Tree_Eco_v6.Resources.Strings.UserID;
      renderTable.Cols[0].Visible = ReportBase.plotInventory;
      renderTable.Cols[15].Visible = renderTable.Cols[16].Visible = this.hasCoordinates && ReportBase.m_ps.ShowGPS;
      renderTable.Cols[17].Visible = ReportBase.m_ps.ShowComments;
      renderTable.Cols[18].Visible = this.hasUID && ReportBase.m_ps.ShowUID;
      DataTable data = (DataTable) this.GetData();
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
      foreach (ColumnFormat columnFormat in columnFormatList)
      {
        ColumnFormat cf = columnFormat;
        if (cf.FormatTotals != null)
          renderTable.Cells[num, cf.ColNum].Text = cf.Format((object) data.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (x => x.Field<double>(cf.ColName))));
      }
      renderTable.Rows[num].Style.FontBold = true;
      renderTable.Rows[num].Style.Borders.Top = LineDef.Default;
      this.Note(C1doc);
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
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.CORemoved, ReportUtil.GetValuePerYrStr(this.gUnits)),
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.O3Removed, ReportUtil.GetValuePerYrStr(this.gUnits)),
        ColName = data.Columns[5].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 4
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.NO2Removed, ReportUtil.GetValuePerYrStr(this.gUnits)),
        ColName = data.Columns[6].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 5
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.SO2Removed, ReportUtil.GetValuePerYrStr(this.gUnits)),
        ColName = data.Columns[7].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 6
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.PM10Removed, ReportUtil.GetValuePerYrStr(this.gUnits)),
        ColName = data.Columns[8].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 7
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.PM25Removed, ReportUtil.GetValuePerYrStr(this.gUnits)),
        ColName = data.Columns[9].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeightGrams),
        ColNum = 8
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.CORemoved, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[10].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 9
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.O3Removed, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[11].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 10
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.NO2Removed, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[12].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 11
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.SO2Removed, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[13].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 12
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.PM10Removed, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[14].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 13
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.PM25Removed, ReportUtil.GetValuePerYrStr(this.CurrencySymbol)),
        ColName = data.Columns[15].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        ColNum = 14
      });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[16].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 15
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[17].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 16
        });
      }
      if (ReportBase.m_ps.ShowComments)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Comments,
          ColName = data.Columns[18].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 17
        });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.UserID,
          ColName = data.Columns[19].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 18
        });
      return columnFormatList;
    }

    protected override string ReportMessage() => this.PollutionRemoval_g_oz_Footer() + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteZeroValuesMeaning + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.WR_PM25Pm10;
  }
}
