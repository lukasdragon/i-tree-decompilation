// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeEnergyBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.v6;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeEnergyBase : DatabaseReport
  {
    protected string plotId_col = "PlotID";
    protected string treeId_col = "TreeID";

    public IndividualTreeEnergyBase()
    {
      this.ReportTitle = string.Empty;
      this.hasComments = true;
      this.hasCoordinates = this.curYear.RecordTreeGPS;
      this.hasUID = this.curYear.RecordTreeUserId;
    }

    public override object GetData()
    {
      Tree tr = (Tree) null;
      Plot p = (Plot) null;
      Building b = (Building) null;
      DataTable results;
      using (ISession session = ReportBase.m_ps.InputSession.CreateSession())
      {
        using (session.BeginTransaction())
          results = session.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => tr)).WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
          {
            (object) TreeStatus.InitialSample,
            (object) TreeStatus.Ingrowth,
            (object) TreeStatus.NoChange,
            (object) TreeStatus.Planted,
            (object) TreeStatus.Unknown
          }).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<object>>) (() => p)).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => t.Buildings), (System.Linq.Expressions.Expression<Func<object>>) (() => b)).Where((System.Linq.Expressions.Expression<Func<Tree, bool>>) (_ => (Guid?) p.Year.Guid == ReportBase.m_ps.InputSession.YearKey && p.IsComplete)).Select(Projections.Distinct((IProjection) Projections.ProjectionList().Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.Id)).As(this.plotId_col)).Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tr.Id)).As(this.treeId_col)).Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => tr.Species)).As("Name")).Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tr.Longitude)).As("xCoordinate")).Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tr.Latitude)).As("yCoordinate")).Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => tr.Comments)).As("Comments")).Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => tr.UserId)).As("UserId")))).OrderBy((System.Linq.Expressions.Expression<Func<Tree, object>>) (_ => (object) p.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Id)).Asc.TransformUsing((IResultTransformer) new DataTableResultTransformer()).SingleOrDefault<DataTable>();
      }
      if (results != null)
        return (object) this.ReportSpecificColumnsData(results);
      return (object) results;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      if (this.CountTreesWithBuildings() == 0)
      {
        this.DisplayStandardMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgNoData);
      }
      else
      {
        C1doc.ClipPage = true;
        RenderTable rTable = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) rTable);
        rTable.Style.Font = new Font("Calibri", 10f);
        rTable.Style.TextAlignHorz = AlignHorzEnum.Right;
        int count = 2;
        rTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
        ReportUtil.FormatRenderTableHeader(rTable);
        Style style = ReportUtil.AddAlternateStyle(rTable);
        rTable.BreakAfter = BreakEnum.None;
        rTable.Width = Unit.Auto;
        rTable.ColumnSizingMode = TableSizingModeEnum.Auto;
        rTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
        rTable.Cols[0].Style.TextAlignHorz = rTable.Cols[1].Style.TextAlignHorz = rTable.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
        rTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlotID;
        rTable.Cols[0].Visible = ReportBase.plotInventory;
        rTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeID;
        rTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
        int num1 = this.ReportSpecificColumns(ref rTable);
        int index1;
        rTable.Cells[0, index1 = num1 + 1].Text = i_Tree_Eco_v6.Resources.Strings.xCoordinate;
        rTable.Cols[index1].Visible = this.hasCoordinates && ReportBase.m_ps.ShowGPS;
        rTable.Cols[index1].Style.Borders.Left = this.tableBorderLineGray;
        int index2;
        rTable.Cells[0, index2 = index1 + 1].Text = i_Tree_Eco_v6.Resources.Strings.yCoordinate;
        rTable.Cols[index2].Visible = this.hasCoordinates && ReportBase.m_ps.ShowGPS;
        rTable.Cols[index2].Style.Borders.Left = this.tableBorderLineGray;
        int index3;
        rTable.Cells[0, index3 = index2 + 1].Text = i_Tree_Eco_v6.Resources.Strings.Comments;
        rTable.Cols[index3].Visible = ReportBase.m_ps.ShowComments;
        rTable.Cols[index3].Style.TextAlignHorz = AlignHorzEnum.Left;
        rTable.Cols[index3].Style.Borders.Left = this.tableBorderLineGray;
        int index4;
        rTable.Cells[0, index4 = index3 + 1].Text = i_Tree_Eco_v6.Resources.Strings.UserID;
        rTable.Cols[index4].Visible = this.hasUID && ReportBase.m_ps.ShowUID;
        rTable.Cols[index4].Style.Borders.Left = this.tableBorderLineGray;
        DataTable data = (DataTable) this.GetData();
        List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
        int num2 = count;
        foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        {
          foreach (ColumnFormat columnFormat in columnFormatList)
            rTable.Cells[num2, columnFormat.ColNum].Text = columnFormat.Format(row[columnFormat.ColName]);
          if ((num2 - count) % 2 == 0)
            rTable.Rows[num2].Style.Parent = style;
          ++num2;
        }
        rTable.Cells[num2, 2].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        rTable.Rows[num2].Style.Borders.Top = LineDef.Default;
        rTable.Rows[num2].Style.FontBold = true;
        foreach (ColumnFormat columnFormat in columnFormatList)
        {
          if (columnFormat.FormatTotals != null)
            rTable.Cells[num2, columnFormat.ColNum].Text = columnFormat.FormatTotals(data.Compute(string.Format("Sum({0})", (object) columnFormat.ColName), string.Empty));
        }
        this.Note(C1doc);
      }
    }

    public virtual int ReportSpecificColumns(ref RenderTable rTable) => 0;

    public virtual DataTable ReportSpecificColumnsData(DataTable results) => new DataTable();
  }
}
