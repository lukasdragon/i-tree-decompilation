// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.UVEffectsOfTreesByStrata
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using NHibernate.Transform;
using System;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class UVEffectsOfTreesByStrata : DatabaseReport
  {
    protected int headerRows = 2;
    protected string formatDecimals = "{0:N3}";
    protected string formatPercents = "{0:N2}";
    protected bool NAdisplayed;

    public UVEffectsOfTreesByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleUVEffectsofTreesbyStratum;

    public override object GetData() => (object) this.curInputISession.GetNamedQuery(nameof (UVEffectsOfTreesByStrata)).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    protected void Header(RenderTable rTable)
    {
      for (int index = 0; index < this.headerRows; ++index)
        rTable.Rows[index].Style.FontBold = true;
      rTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      rTable.Cells[0, 1].SpanCols = 3;
      rTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.UVEffectsinTreeShade;
      rTable.Cells[0, 4].SpanCols = 3;
      rTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.UVEffectsOverall;
      for (int col = 1; col < 6; ++col)
        rTable.Cells[0, col].Style.Borders.Bottom = LineDef.Default;
      rTable.Cells[1, 0].Text = v6Strings.Strata_SingularName;
      rTable.Cells[1, 1].Text = rTable.Cells[1, 4].Text = i_Tree_Eco_v6.Resources.Strings.ProtectionFactor;
      rTable.Cells[1, 2].Text = rTable.Cells[1, 5].Text = i_Tree_Eco_v6.Resources.Strings.ReductionInUVIndex;
      rTable.Cells[1, 3].Text = rTable.Cells[1, 6].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.PercentReduction, "%");
    }

    protected void PopulateUVEffectsData(RenderTable rTable)
    {
      DataTable data = (DataTable) this.GetData();
      int headerRows = this.headerRows;
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        int num;
        if (row["Description"].ToString().Equals(string.Empty))
        {
          num = this.headerRows + data.Rows.Count;
          rTable.Cells[num, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          rTable.Rows[num].Style.Borders.Top = LineDef.Default;
          rTable.Rows[num].Style.FontBold = true;
        }
        else
        {
          num = headerRows;
          rTable.Cells[num, 0].Text = row["Description"].ToString();
          ++headerRows;
        }
        object obj1 = row["MeanShadedUVRedutionPercent"];
        if (double.Parse(obj1.ToString()) == 100.0)
        {
          rTable.Cells[num, 1].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
          this.NAdisplayed = true;
        }
        else
          rTable.Cells[num, 1].Text = string.Format(this.formatDecimals, row["MeanShadedUVProtectionFactor"]);
        rTable.Cells[num, 2].Text = string.Format(this.formatDecimals, row["MeanShadedUVReduction"]);
        rTable.Cells[num, 3].Text = string.Format(this.formatPercents, obj1);
        object obj2 = row["MeanOverallUVReductionPercent"];
        if (double.Parse(obj2.ToString()) == 100.0)
        {
          rTable.Cells[num, 4].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
          this.NAdisplayed = true;
        }
        else
          rTable.Cells[num, 4].Text = string.Format(this.formatDecimals, row["MeanOverallUVProtectionFactor"]);
        rTable.Cells[num, 5].Text = string.Format(this.formatDecimals, row["MeanOverallUVReduction"]);
        rTable.Cells[num, 6].Text = string.Format(this.formatPercents, obj2);
      }
    }

    protected void BodyStylinig(C1PrintDocument C1doc, RenderTable rTable)
    {
      C1doc.ClipPage = true;
      rTable.Width = (Unit) "100%";
      rTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      rTable.SplitHorzBehavior = SplitBehaviorEnum.Never;
      rTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      rTable.Style.Font = new Font("Calibri", 10f);
      rTable.Cols[0].Style.Borders.Right = rTable.Cols[3].Style.Borders.Right = LineDef.Default;
      rTable.RowGroups[0, this.headerRows].Header = TableHeaderEnum.Page;
      ReportUtil.FormatRenderTable(rTable);
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      this.PopulateUVEffectsData(renderTable);
      this.BodyStylinig(C1doc, renderTable);
      this.Header(renderTable);
      C1doc.Body.Children.Add((RenderObject) this.Footer());
      if (!this.NAdisplayed)
        return;
      C1doc.Body.Children.Add((RenderObject) this.FooterForNA());
    }

    protected RenderText Footer()
    {
      RenderText renderText = new RenderText();
      renderText.Text = i_Tree_Eco_v6.Resources.Strings.NoteUVEffectsOfTreesByStrata;
      renderText.Style.TextAlignHorz = AlignHorzEnum.Left;
      renderText.Style.Spacing.Top = (Unit) "1ls";
      return renderText;
    }

    protected RenderText FooterForNA()
    {
      RenderText renderText = new RenderText();
      renderText.Text = i_Tree_Eco_v6.Resources.Strings.NoteUVEffectsOfTreesByStrataNA;
      renderText.Style.TextAlignHorz = AlignHorzEnum.Left;
      renderText.Style.Spacing.Top = (Unit) "1ls";
      return renderText;
    }
  }
}
