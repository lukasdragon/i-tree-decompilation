// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PlotComments
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
using System.Data;

namespace i_Tree_Eco_v6.Reports
{
  public class PlotComments : PlotCommon
  {
    public PlotComments() => this.ReportTitle = Strings.ReportTitlePlotComments;

    public override bool ShowComments => true;

    public override object GetData()
    {
      Plot pl = (Plot) null;
      Strata s = (Strata) null;
      using (ISession session = ReportBase.m_ps.InputSession.CreateSession())
      {
        using (session.BeginTransaction())
          return (object) session.QueryOver<Plot>((System.Linq.Expressions.Expression<Func<Plot>>) (() => pl)).JoinAlias((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => p.Strata), (System.Linq.Expressions.Expression<Func<object>>) (() => s)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => (Guid?) p.Year.Guid == ReportBase.m_ps.InputSession.YearKey)).Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Comments != string.Empty)).Select(Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pl.Id)).As("PlotId"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => s.Description)).As("Stratum"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => pl.Address)).As("Address"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pl.Date)).As("Date"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => pl.Crew)).As("Crew"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pl.IsComplete)).As("Complete"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pl.Longitude)).As("xCoordinate"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => (object) pl.Latitude)).As("yCoordinate"), Projections.Property((System.Linq.Expressions.Expression<Func<object>>) (() => pl.Comments)).As("Comments")).OrderBy((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) p.Id)).Asc.TransformUsing((IResultTransformer) new DataTableResultTransformer()).SingleOrDefault<DataTable>();
      }
    }
  }
}
