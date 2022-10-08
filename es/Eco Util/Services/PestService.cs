// Decompiled with JetBrains decompiler
// Type: Eco.Util.Services.PestService
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using LocationSpecies.Domain;
using NHibernate;
using System;
using System.Collections.Generic;

namespace Eco.Util.Services
{
  public class PestService
  {
    private ISessionFactory _sf;

    public PestService(ISessionFactory sessionFactory) => this._sf = sessionFactory;

    public ISet<Species> GetSusceptableSpecies(int pestId) => RetryExecutionHandler.Execute<ISet<Species>>((Func<ISet<Species>>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          Pest pest = session.Load<Pest>((object) pestId);
          NHibernateUtil.Initialize((object) pest.SusceptableSpecies);
          ISet<Species> susceptableSpecies = pest.SusceptableSpecies;
          transaction.Commit();
          return susceptableSpecies;
        }
      }
    }));
  }
}
