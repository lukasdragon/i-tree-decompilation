// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.TreeComments
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.NHibernate;
using Eco.Domain.v6;
using i_Tree_Eco_v6.Resources;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;

namespace i_Tree_Eco_v6.Reports
{
  public class TreeComments : CommentsBase
  {
    public TreeComments()
    {
      this.ReportTitle = Strings.ReportTitleTreeComments;
      this.hasCoordinates = this.curYear.RecordTreeGPS;
      this.hasUID = this.curYear.RecordTreeUserId;
    }

    public override object GetData()
    {
      Plot p = (Plot) null;
      using (ISession session = ReportBase.m_ps.InputSession.CreateSession())
      {
        using (session.BeginTransaction())
          return (object) session.QueryOver<Tree>().WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
          {
            (object) TreeStatus.InitialSample,
            (object) TreeStatus.Ingrowth,
            (object) TreeStatus.NoChange,
            (object) TreeStatus.Planted,
            (object) TreeStatus.Unknown
          }).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (pl => pl.Plot), (System.Linq.Expressions.Expression<Func<object>>) (() => p)).Where((System.Linq.Expressions.Expression<Func<Tree, bool>>) (_ => (Guid?) p.Year.Guid == ReportBase.m_ps.InputSession.YearKey)).Where((System.Linq.Expressions.Expression<Func<Tree, bool>>) (t => t.Comments != string.Empty)).Select(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.Id)).As("PlotId"), Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Id)).As("TreeId"), Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => t.Species)).As("Species"), Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.SurveyDate)).As("SurveyDate"), Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).As("Status"), Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (t => (object) t.Longitude)).As("xCoordinate"), Projections.Property<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (t => (object) t.Latitude)).As("yCoordinate"), Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => t.Comments)).As("Comments"), Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => t.UserId)).As("UserId")).OrderBy((System.Linq.Expressions.Expression<Func<Tree, object>>) (_ => (object) p.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Id)).Asc.TransformUsing((IResultTransformer) new DataTableResultTransformer()).SingleOrDefault<DataTable>();
      }
    }

    public override List<ColumnFormat> ColumnsFormat(DataTable data)
    {
      List<ColumnFormat> columnFormatList = new List<ColumnFormat>();
      if (ReportBase.plotInventory)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = Strings.PlotID,
          ColName = data.Columns[0].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatInt),
          ColNum = 0
        });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.TreeID,
        ColName = data.Columns[1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatInt),
        ColNum = 1
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.SpeciesName,
        ColName = data.Columns[2].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatSpecies),
        ColNum = 2
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.Date,
        ColName = data.Columns[3].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDate),
        ColNum = 3
      });
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.Status,
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatTreeStatus),
        ColNum = 4
      });
      if (this.hasCoordinates && ReportBase.m_ps.ShowGPS)
      {
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = Strings.xCoordinate,
          ColName = data.Columns[5].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 5
        });
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = Strings.yCoordinate,
          ColName = data.Columns[6].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 6
        });
      }
      columnFormatList.Add(new ColumnFormat()
      {
        HeaderText = Strings.Comments,
        ColName = data.Columns[7].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 7
      });
      if (this.hasUID && ReportBase.m_ps.ShowUID)
        columnFormatList.Add(new ColumnFormat()
        {
          HeaderText = Strings.UserID,
          ColName = data.Columns[8].ColumnName,
          Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
          ColNum = 8
        });
      return columnFormatList;
    }
  }
}
