// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.WildLifeSuitability
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class WildLifeSuitability : WildLifeBase
  {
    protected int headerRows = 2;
    protected string formatDecimals = "{0:N3}";
    protected string reportBy = string.Empty;

    protected void Header(RenderTable rTable)
    {
      for (int index = 0; index < this.headerRows; ++index)
        rTable.Rows[index].Style.FontBold = true;
      rTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      rTable.Cells[0, 2].SpanCols = 2;
      rTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SuitabilityIndex;
      rTable.Cells[0, 4].SpanCols = 2;
      rTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.IndexChangeDueToTrees;
      for (int col = 2; col < 5; ++col)
        rTable.Cells[0, col].Style.Borders.Bottom = new LineDef((Unit) "1pt", Color.Black);
      rTable.Cells[1, 0].Text = this.reportBy;
      rTable.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.WildlifeName;
      rTable.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.WithTrees;
      rTable.Cells[1, 3].Text = i_Tree_Eco_v6.Resources.Strings.WithoutTrees;
      rTable.Cells[1, 4].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Relative, "%");
      rTable.Cells[1, 5].Text = i_Tree_Eco_v6.Resources.Strings.Absolute;
    }

    protected void PopulateHydrologyRow(
      RenderTable rTable,
      int tableRow,
      WildLifeSuitabilityBase plot,
      string unifier)
    {
      rTable.Cells[tableRow, 0].Text = unifier;
      rTable.Cells[tableRow, 1].Text = ReportBase.m_ps.SpeciesDisplayName != SpeciesDisplayEnum.CommonName ? plot.Wildlife.ScientificName : plot.Wildlife.CommonName;
      rTable.Cells[tableRow, 2].Text = string.Format(this.formatDecimals, (object) plot.SuitabilityIndexWithTree);
      rTable.Cells[tableRow, 3].Text = string.Format(this.formatDecimals, (object) plot.SuitabilityIndexWithoutTree);
      rTable.Cells[tableRow, 4].Text = string.Format(this.formatDecimals, (object) plot.RelativeChangeOfSuitabilityIndexWithTree);
      rTable.Cells[tableRow, 5].Text = string.Format(this.formatDecimals, (object) plot.AbsoluteChangeOfSuitabilityIndexWithTree);
    }

    protected void BodyStylinig(C1PrintDocument C1doc, RenderTable rTable)
    {
      rTable.Width = (Unit) "100%";
      C1doc.ClipPage = true;
      rTable.Rows[0].SizingMode = TableSizingModeEnum.Auto;
      rTable.Style.TextAlignHorz = AlignHorzEnum.Right;
      rTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      rTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      rTable.Style.Font = new Font("Calibri", 10f);
      rTable.Cols[1].Style.Borders.Right = rTable.Cols[3].Style.Borders.Right = new LineDef((Unit) "1pt", Color.Black);
      rTable.RowGroups[0, this.headerRows].Header = TableHeaderEnum.Page;
      rTable.RowGroups[0, this.headerRows].Style.Borders.Bottom = LineDef.Default;
      ReportUtil.FormatRenderTable(rTable);
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      this.PopulateReportBody(renderTable);
      this.BodyStylinig(C1doc, renderTable);
      this.Header(renderTable);
      this.Note(C1doc);
    }

    protected virtual void PopulateReportBody(RenderTable rTable)
    {
    }

    protected override string ReportMessage() => i_Tree_Eco_v6.Resources.Strings.NoteSuitabilityIndex;
  }
}
