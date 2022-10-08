// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ShrubComments
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
  public class ShrubComments : CommentsBase
  {
    public ShrubComments() => this.ReportTitle = Strings.ReportTitleShrubComments;

    public override object GetData()
    {
      Plot p = (Plot) null;
      using (ISession session = ReportBase.m_ps.InputSession.CreateSession())
      {
        using (session.BeginTransaction())
          return (object) session.QueryOver<Shrub>().JoinAlias((System.Linq.Expressions.Expression<Func<Shrub, object>>) (pl => pl.Plot), (System.Linq.Expressions.Expression<Func<object>>) (() => p)).Where((System.Linq.Expressions.Expression<Func<Shrub, bool>>) (_ => (Guid?) p.Year.Guid == ReportBase.m_ps.InputSession.YearKey)).Where((System.Linq.Expressions.Expression<Func<Shrub, bool>>) (s => s.Comments != string.Empty)).Select(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) p.Id)).As("PlotId"), Projections.Property<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => (object) s.Id)).As("ShrubId"), Projections.Property<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => s.Species)).As("Species"), Projections.Property<Shrub>((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => s.Comments)).As("Comments")).OrderBy((System.Linq.Expressions.Expression<Func<Shrub, object>>) (_ => (object) p.Id)).Asc.OrderBy((System.Linq.Expressions.Expression<Func<Shrub, object>>) (s => (object) s.Id)).Asc.TransformUsing((IResultTransformer) new DataTableResultTransformer()).SingleOrDefault<DataTable>();
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
        HeaderText = Strings.ShrubID,
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
        HeaderText = Strings.Comments,
        ColName = data.Columns[3].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 3
      });
      return columnFormatList;
    }
  }
}
