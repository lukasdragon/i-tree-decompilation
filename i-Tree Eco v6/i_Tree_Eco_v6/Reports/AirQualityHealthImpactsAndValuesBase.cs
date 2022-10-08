// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.AirQualityHealthImpactsAndValuesBase
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
  public class AirQualityHealthImpactsAndValuesBase : DatabaseReport
  {
    protected string TreeShrub;

    public AirQualityHealthImpactsAndValuesBase() => this.HelpTopic = "AirQualityHealthImpactsAndValues";

    protected virtual DataTable GetDBData() => this.curInputISession.GetNamedQuery("AirQualityHealthImpactsAndValues").SetParameter<Guid>("y", this.YearGuid).SetParameter<string>("TreeShrub", this.TreeShrub).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    public override object GetData() => (object) this.GetDBData();

    private string GetIcidenceStr() => i_Tree_Eco_v6.Resources.Strings.Incidence;

    private string GetReductionPerYrStr() => ReportUtil.FormatHeaderUnitsStr(string.Format(i_Tree_Eco_v6.Resources.Strings.ReductionPerYr, (object) i_Tree_Eco_v6.Resources.Strings.YearAbbr));

    private string GetvalueStr() => i_Tree_Eco_v6.Resources.Strings.Value;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      int headerRows = 3;
      ReportUtil.SetDefaultReportFormatting(C1doc, renderTable, this.curYear, headerRows);
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cells[0, 1].SpanCols = 2;
      renderTable.Cells[0, 3].SpanCols = 2;
      renderTable.Cells[0, 5].SpanCols = 2;
      renderTable.Cells[0, 7].SpanCols = 2;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.OzoneFurmula;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula;
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula;
      renderTable.Cells[1, 1].Text = renderTable.Cells[1, 3].Text = renderTable.Cells[1, 5].Text = renderTable.Cells[1, 7].Text = this.GetIcidenceStr();
      renderTable.Cells[2, 1].Text = renderTable.Cells[2, 3].Text = renderTable.Cells[2, 5].Text = renderTable.Cells[2, 7].Text = this.GetReductionPerYrStr();
      renderTable.Cells[1, 2].Text = renderTable.Cells[1, 4].Text = renderTable.Cells[1, 6].Text = renderTable.Cells[1, 8].Text = this.GetvalueStr();
      renderTable.Cells[2, 2].Text = renderTable.Cells[2, 4].Text = renderTable.Cells[2, 6].Text = renderTable.Cells[2, 8].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      renderTable.Cols[0].Width = (Unit) "20%";
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[0].Style.Borders.Right = renderTable.Cols[2].Style.Borders.Right = renderTable.Cols[4].Style.Borders.Right = renderTable.Cols[6].Style.Borders.Right = renderTable.Cols[8].Style.Borders.Right = ReportUtil.GetTableLine();
      DataTable data = (DataTable) this.GetData();
      if (data.Rows.Count <= 0)
      {
        DatabaseReport.NewReportMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgGrassReport);
      }
      else
      {
        List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
        int num = headerRows;
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          foreach (ColumnFormat columnFormat in columnFormatList)
            renderTable.Cells[num, columnFormatList.IndexOf(columnFormat)].Text = columnFormat.Format(row[columnFormat.ColName]);
          ++num;
        }
        renderTable.Cells[num, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        renderTable.Rows[num].Style.Borders.Top = LineDef.Default;
        renderTable.Cells[num, 1].Text = this.SumCol(data, "NO2Inc", "N3");
        renderTable.Cells[num, 2].Text = this.SumCol(data, "NO2Val", "N2");
        renderTable.Cells[num, 3].Text = this.SumCol(data, "O3Inc", "N3");
        renderTable.Cells[num, 4].Text = this.SumCol(data, "O3Val", "N2");
        renderTable.Cells[num, 5].Text = this.SumCol(data, "PM25Inc", "N3");
        renderTable.Cells[num, 6].Text = this.SumCol(data, "PM25Val", "N2");
        renderTable.Cells[num, 7].Text = this.SumCol(data, "SO2Inc", "N3");
        renderTable.Cells[num, 8].Text = this.SumCol(data, "SO2Val", "N2");
        renderTable.Rows[num].Style.FontBold = true;
        ReportUtil.FormatRenderTable(renderTable);
        this.Note(C1doc);
      }
    }

    private string SumCol(DataTable data, string colName, string round)
    {
      double? nullable = data.AsEnumerable().Sum<DataRow>((Func<DataRow, double?>) (r => r.Field<double?>(colName)));
      return nullable.Equals((object) DBNull.Value) ? "" : Convert.ToDouble((object) nullable).ToString(round);
    }

    protected override string ReportMessage() => i_Tree_Eco_v6.Resources.Strings.NoteAirQualityHealthImpactsAndValuesBase;

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      string str1 = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
      {
        this.GetIcidenceStr(),
        this.GetReductionPerYrStr()
      });
      string str2 = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
      {
        this.GetvalueStr(),
        ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol)
      });
      return new List<ColumnFormat>()
      {
        new ColumnFormat()
        {
          HeaderText = string.Format(string.Empty),
          ColName = data.Columns[0].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr)
        },
        new ColumnFormat()
        {
          HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
          {
            i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula,
            str1
          }),
          ColName = data.Columns[1].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue3)
        },
        new ColumnFormat()
        {
          HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
          {
            i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula,
            str2
          }),
          ColName = data.Columns[2].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2)
        },
        new ColumnFormat()
        {
          HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
          {
            i_Tree_Eco_v6.Resources.Strings.OzoneFurmula,
            str1
          }),
          ColName = data.Columns[3].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue3)
        },
        new ColumnFormat()
        {
          HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
          {
            i_Tree_Eco_v6.Resources.Strings.OzoneFurmula,
            str2
          }),
          ColName = data.Columns[4].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2)
        },
        new ColumnFormat()
        {
          HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
          {
            i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula,
            str1
          }),
          ColName = data.Columns[5].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue3)
        },
        new ColumnFormat()
        {
          HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
          {
            i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula,
            str2
          }),
          ColName = data.Columns[6].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2)
        },
        new ColumnFormat()
        {
          HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
          {
            i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula,
            str1
          }),
          ColName = data.Columns[7].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue3)
        },
        new ColumnFormat()
        {
          HeaderText = ReportUtil.FormatInlineCSVHeaderStr(new string[2]
          {
            i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula,
            str2
          }),
          ColName = data.Columns[8].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2)
        }
      };
    }
  }
}
