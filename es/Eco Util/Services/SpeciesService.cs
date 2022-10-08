// Decompiled with JetBrains decompiler
// Type: Eco.Util.Services.SpeciesService
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Eco.Util.Services
{
  public class SpeciesService
  {
    private readonly ISessionFactory _sf;

    public SpeciesService(ISessionFactory sessionFactory) => this._sf = sessionFactory;

    public IList<Species> GetValidSpecies() => RetryExecutionHandler.Execute<IList<Species>>((Func<IList<Species>>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (session.BeginTransaction())
          return session.QueryOver<Species>().Fetch<Species, Species>(SelectMode.Fetch, (Expression<Func<Species, object>>) (sp => sp.Names), (Expression<Func<Species, object>>) (sp => sp.LeafType), (Expression<Func<Species, object>>) (sp => sp.Parent), (Expression<Func<Species, object>>) (sp => (object) sp.PercentLeafType)).Where((Expression<Func<Species, bool>>) (sp => sp.Code != default (string) && sp.ReplacedBy == default (object) && (int) sp.Rank != 8)).JoinQueryOver<LeafType>((Expression<Func<Species, LeafType>>) (sp => sp.LeafType)).Where((Expression<Func<LeafType, bool>>) (lt => lt.Id != 3)).TransformUsing(Transformers.DistinctRootEntity).Cacheable().List();
      }
    }));

    public IList<Species> GetInvalidSpecies() => RetryExecutionHandler.Execute<IList<Species>>((Func<IList<Species>>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (session.BeginTransaction())
          return session.QueryOver<Species>().Fetch<Species, Species>(SelectMode.Fetch, (Expression<Func<Species, object>>) (sp => sp.Names), (Expression<Func<Species, object>>) (sp => sp.LeafType), (Expression<Func<Species, object>>) (sp => sp.Parent), (Expression<Func<Species, object>>) (sp => (object) sp.PercentLeafType)).Where((Expression<Func<Species, bool>>) (sp => sp.Code != default (string) && (sp.ReplacedBy != default (object) || (int) sp.Rank == 8))).JoinQueryOver<LeafType>((Expression<Func<Species, LeafType>>) (sp => sp.LeafType)).Where((Expression<Func<LeafType, bool>>) (lt => lt.Id != 3)).TransformUsing(Transformers.DistinctRootEntity).Cacheable().List();
      }
    }));
  }
}
