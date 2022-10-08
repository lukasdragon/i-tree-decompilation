// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeCharacteristics
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util.Convert;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeCharacteristics : DatabaseReport
  {
    public IndividualTreeCharacteristics()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleIndividualTreeData;
      this.hasComments = true;
      this.hasCoordinates = this.curYear.RecordTreeGPS;
      this.hasUID = this.curYear.RecordTreeUserId;
    }

    private string FormatNativeToState(object val)
    {
      if (ReportBase.IsNull(val))
        return string.Empty;
      return this.CheckNations() ? val.ToString() : i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.ClipPage = true;
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
      renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = renderTable.Cols[2].Style.TextAlignHorz = renderTable.Cols[8].Style.TextAlignHorz = renderTable.Cols[13].Style.TextAlignHorz = renderTable.Cols[14].Style.TextAlignHorz = renderTable.Cols[15].Style.TextAlignHorz = renderTable.Cols[17].Style.TextAlignHorz = renderTable.Cols[18].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlotID;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.TreeID;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.DBH;
      renderTable.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.CmUnits());
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.Height;
      renderTable.Cells[1, 4].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.MUnits());
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.CrownHeight;
      renderTable.Cells[1, 5].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.MUnits());
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.CrownWidth;
      renderTable.Cells[1, 6].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.MUnits());
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.CanopyCover;
      renderTable.Cells[1, 7].Text = ReportUtil.FormatHeaderUnitsStr(this.SquareMeterUnits());
      renderTable.Cells[0, 8].Text = i_Tree_Eco_v6.Resources.Strings.TreeCondition;
      renderTable.Cells[0, 9].Text = i_Tree_Eco_v6.Resources.Strings.LeafArea;
      renderTable.Cells[1, 9].Text = ReportUtil.FormatHeaderUnitsStr(this.SquareMeterUnits());
      renderTable.Cells[0, 10].Text = i_Tree_Eco_v6.Resources.Strings.LeafBiomass;
      renderTable.Cells[1, 10].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.KgUnits());
      renderTable.Cells[0, 11].Text = i_Tree_Eco_v6.Resources.Strings.LeafAreaIndex;
      renderTable.Cells[0, 12].Text = i_Tree_Eco_v6.Resources.Strings.BasalArea;
      renderTable.Cells[1, 12].Text = ReportUtil.FormatHeaderUnitsStr(this.SquareMeterUnits());
      renderTable.Cells[0, 13].Text = i_Tree_Eco_v6.Resources.Strings.StreetTree;
      renderTable.Cells[0, 14].Text = i_Tree_Eco_v6.Resources.Strings.NativeToState;
      renderTable.Cells[0, 15].Text = i_Tree_Eco_v6.Resources.Strings.PrivateTree;
      renderTable.Cells[0, 16].Text = i_Tree_Eco_v6.Resources.Strings.xCoordinate;
      renderTable.Cells[0, 17].Text = i_Tree_Eco_v6.Resources.Strings.yCoordinate;
      renderTable.Cells[0, 18].Text = i_Tree_Eco_v6.Resources.Strings.Comments;
      renderTable.Cells[0, 19].Text = i_Tree_Eco_v6.Resources.Strings.UserID;
      renderTable.Cells[0, 20].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 21].Text = i_Tree_Eco_v6.Resources.Strings.Date;
      renderTable.Cells[0, 22].Text = i_Tree_Eco_v6.Resources.Strings.Crew;
      renderTable.Cells[0, 23].Text = i_Tree_Eco_v6.Resources.Strings.LandUse;
      renderTable.Cells[0, 24].Text = v6Strings.GroundCover_SingularName;
      renderTable.Cols[0].Visible = ReportBase.plotInventory;
      renderTable.Cols[13].Visible = this.curYear.RecordStreetTree;
      renderTable.Cols[15].Visible = this.curYear.RecordMgmtStyle;
      renderTable.Cols[16].Visible = renderTable.Cols[17].Visible = this.hasCoordinates && ReportBase.m_ps.ShowGPS;
      renderTable.Cols[18].Visible = ReportBase.m_ps.ShowComments;
      renderTable.Cols[19].Visible = this.hasUID && ReportBase.m_ps.ShowUID;
      renderTable.Cols[20].Visible = !ReportBase.plotInventory && this.curYear.RecordStrata;
      renderTable.Cols[21].Visible = renderTable.Cols[22].Visible = !ReportBase.plotInventory;
      renderTable.Cols[23].Visible = !ReportBase.plotInventory && this.curYear.RecordLanduse;
      renderTable.Cols[24].Visible = !ReportBase.plotInventory && this.curYear.RecordGroundCover;
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
    }

    public override object GetData()
    {
      DataTable data = this.curInputISession.GetNamedQuery(nameof (IndividualTreeCharacteristics)).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      if (!ReportBase.plotInventory)
        this.AddPlotData(this.curInputISession, data);
      if (!this.curYear.RecordCrownSize)
        this.SetDefaultCrownValues(data);
      return (object) data;
    }

    public void AddPlotData(ISession session, DataTable dt)
    {
      Tree tree = (Tree) null;
      Plot plot = (Plot) null;
      PlotLandUse plotLandUse = (PlotLandUse) null;
      PlotGroundCover plotGroundCover = (PlotGroundCover) null;
      GroundCover groundCover = (GroundCover) null;
      LandUse landUse = (LandUse) null;
      Strata strata = (Strata) null;
      IQueryOver<Tree, Tree> queryOver = session.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => tree)).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.Plot), (System.Linq.Expressions.Expression<Func<object>>) (() => plot)).Where((System.Linq.Expressions.Expression<Func<bool>>) (() => (Guid?) plot.Year.Guid == ReportBase.m_ps.InputSession.YearKey));
      ProjectionList projectionList = Projections.ProjectionList().Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plot.Id)).As("PlotId")).Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) tree.Id)).As("TreeId")).Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) plot.Date)).As("Date")).Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => plot.Crew)).As("Crew"));
      List<string> stringList = new List<string>()
      {
        "Date",
        "Crew"
      };
      if (this.curYear.RecordStrata)
      {
        queryOver.JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => plot.Strata), (System.Linq.Expressions.Expression<Func<object>>) (() => strata));
        projectionList.Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => strata.Description)).As("Stratum"));
        stringList.Add("Stratum");
      }
      if (this.curYear.RecordLanduse)
      {
        queryOver.JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => tr.PlotLandUse), (System.Linq.Expressions.Expression<Func<object>>) (() => plotLandUse)).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => plotLandUse.LandUse), (System.Linq.Expressions.Expression<Func<object>>) (() => landUse));
        projectionList.Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => landUse.Description)).As("LandUse"));
        stringList.Add("LandUse");
      }
      if (this.curYear.RecordGroundCover)
      {
        queryOver.JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => plot.PlotGroundCovers.First<PlotGroundCover>()), (System.Linq.Expressions.Expression<Func<object>>) (() => plotGroundCover)).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (tr => plotGroundCover.GroundCover), (System.Linq.Expressions.Expression<Func<object>>) (() => groundCover));
        projectionList.Add(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => groundCover.Description)).As("GoundCover"));
        stringList.Add("GoundCover");
      }
      DataTable source = queryOver.Select((IProjection) projectionList).TransformUsing((IResultTransformer) new DataTableResultTransformer()).SingleOrDefault<DataTable>();
      dt.Columns.Add("Stratum", typeof (string));
      dt.Columns.Add("Date", typeof (DateTime));
      dt.Columns.Add("Crew", typeof (string));
      dt.Columns.Add("LandUse", typeof (string));
      dt.Columns.Add("GoundCover", typeof (string));
      foreach (DataRow row1 in (InternalDataCollectionBase) dt.Rows)
      {
        DataRow row = row1;
        DataRow dataRow = source.AsEnumerable().Where<DataRow>((Func<DataRow, bool>) (r => r.Field<int>("PlotId") == row.Field<int>("PlotId") && r.Field<int>("TreeId") == row.Field<int>("TreeId"))).First<DataRow>();
        foreach (string columnName in stringList)
          row[columnName] = dataRow[columnName];
      }
    }

    private void SetDefaultCrownValues(DataTable data)
    {
      IList<Tree> treeList = this.curInputISession.CreateCriteria<Tree>().CreateAlias("Plot", "p").CreateAlias("p.Year", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.curYear.Guid)).Add((ICriterion) Restrictions.Eq("p.IsComplete", (object) true)).Add((ICriterion) Restrictions.On<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
      {
        (object) TreeStatus.InitialSample,
        (object) TreeStatus.Ingrowth,
        (object) TreeStatus.NoChange,
        (object) TreeStatus.Planted,
        (object) TreeStatus.Unknown
      })).AddOrder(Order.Asc("p.Id")).List<Tree>();
      DefaultTreeData defaultTreeData = new DefaultTreeData();
      defaultTreeData.initialize(this.curInputISession, this.curYear.Guid, this.locSpSession);
      foreach (Tree tree1 in (IEnumerable<Tree>) treeList)
      {
        Tree tree = tree1;
        DBHCrownSize dbhCrownSize = defaultTreeData.calculateDbhCrownSize(tree);
        DataRow dataRow = data.AsEnumerable().Where<DataRow>((Func<DataRow, bool>) (r => r.Field<int>("PlotId") == tree.Plot.Id && r.Field<int>("TreeId") == tree.Id)).First<DataRow>();
        dataRow["CrownHeight"] = (object) dbhCrownSize.crownHeight;
        dataRow["CrownWidth"] = (object) dbhCrownSize.crownWidth;
      }
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
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.DBH, ReportBase.CmUnits()),
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleCentimeters),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Height, ReportBase.MUnits()),
        ColName = data.Columns[5].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleMeters),
        ColNum = 4
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.CrownHeight, ReportBase.MUnits()),
        ColName = data.Columns[6].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(((BasicReport) this).ProjectConvertFeetMeter),
        ColNum = 5
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.CrownWidth, ReportBase.MUnits()),
        ColName = data.Columns[7].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(((BasicReport) this).ProjectConvertFeetMeter),
        ColNum = 6
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.CanopyCover, this.SquareMeterUnits()),
        ColName = data.Columns[8].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleSquaremeter),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleSquaremeter),
        ColNum = 7
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.TreeCondition,
        ColName = data.Columns[9].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 8
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, this.SquareMeterUnits()),
        ColName = data.Columns[10].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleSquaremeter),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleSquaremeter),
        ColNum = 9
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafBiomass, ReportBase.KgUnits()),
        ColName = data.Columns[11].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0),
        ColNum = 10
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.LeafAreaIndex,
        ColName = data.Columns[12].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue1),
        ColNum = 11
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.BasalArea, this.SquareMeterUnits()),
        ColName = data.Columns[13].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleSquaremeter3),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleSquaremeter),
        ColNum = 12
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.StreetTree,
        ColName = data.Columns[14].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 13
      });
      if (this.curYear.RecordStreetTree)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.NativeToState,
          ColName = data.Columns[15].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(this.FormatNativeToState),
          ColNum = 14
        });
      if (this.curYear.RecordMgmtStyle)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.PrivateTree,
          ColName = data.Columns[16].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDBBool),
          ColNum = 15
        });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.xCoordinate,
          ColName = data.Columns[17].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 16
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.yCoordinate,
          ColName = data.Columns[18].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 17
        });
      }
      if (ReportBase.m_ps.ShowComments)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Comments,
          ColName = data.Columns[19].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 18
        });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.UserID,
          ColName = data.Columns[20].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 19
        });
      if (!ReportBase.plotInventory)
      {
        if (this.curYear.RecordStrata)
          columnFormatList.Add(new ColumnFormat()
          {
            HeaderText = v6Strings.Strata_SingularName,
            ColName = data.Columns[21].ColumnName,
            Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
            ColNum = 20
          });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Date,
          ColName = data.Columns[22].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDate),
          ColNum = 21
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = i_Tree_Eco_v6.Resources.Strings.Crew,
          ColName = data.Columns[23].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 22
        });
        if (this.curYear.RecordLanduse)
          columnFormatList.Add(new ColumnFormat()
          {
            HeaderText = i_Tree_Eco_v6.Resources.Strings.LandUse,
            ColName = data.Columns[24].ColumnName,
            Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
            ColNum = 23
          });
        if (this.curYear.RecordGroundCover)
          columnFormatList.Add(new ColumnFormat()
          {
            HeaderText = v6Strings.GroundCover_SingularName,
            ColName = data.Columns[25].ColumnName,
            Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
            ColNum = 24
          });
      }
      return columnFormatList;
    }
  }
}
