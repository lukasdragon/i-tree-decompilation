// Decompiled with JetBrains decompiler
// Type: Eco.Util.Services.WeatherService
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eco.Util.Services
{
  public class WeatherService
  {
    private ISessionFactory _sf;

    public WeatherService(ISessionFactory sessionFactory) => this._sf = sessionFactory;

    public IList<int> GetWeatherYears() => (IList<int>) RetryExecutionHandler.Execute<List<int>>((Func<List<int>>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          List<int> list = session.Query<WeatherDetail>().WithOptions<WeatherDetail>((System.Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Where<WeatherDetail>((System.Linq.Expressions.Expression<Func<WeatherDetail, bool>>) (wd => wd.Processible)).OrderByDescending<WeatherDetail, int>((System.Linq.Expressions.Expression<Func<WeatherDetail, int>>) (wd => wd.Year)).Select<WeatherDetail, int>((System.Linq.Expressions.Expression<Func<WeatherDetail, int>>) (wd => wd.Year)).Distinct<int>().ToList<int>();
          transaction.Commit();
          return list;
        }
      }
    }));

    public WeatherDetail GetWeatherYearData(string station, int year) => RetryExecutionHandler.Execute<WeatherDetail>((Func<WeatherDetail>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          WeatherDetail weatherYearData = session.QueryOver<WeatherDetail>().Where((System.Linq.Expressions.Expression<Func<WeatherDetail, bool>>) (wd => wd.Year == this.year)).JoinQueryOver<WeatherStation>((System.Linq.Expressions.Expression<Func<WeatherDetail, WeatherStation>>) (wd => wd.WeatherStation)).Where((System.Linq.Expressions.Expression<Func<WeatherStation, bool>>) (ws => Projections.Concat(new string[]
          {
            ws.USAF,
            "-",
            ws.WBAN
          }) == this.station)).Cacheable().SingleOrDefault();
          transaction.Commit();
          return weatherYearData;
        }
      }
    }));
  }
}
