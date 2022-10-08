// Decompiled with JetBrains decompiler
// Type: Eco.Util.Services.LocationService
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Eco.Util.Services
{
  public class LocationService
  {
    private ISessionFactory _sf;

    public LocationService(ISessionFactory sessionFactory) => this._sf = sessionFactory;

    public IList<Species> GetInvasiveSpecies(int locationId) => RetryExecutionHandler.Execute<IList<Species>>((Func<IList<Species>>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          // ISSUE: reference to a compiler-generated field
          IList<Species> invasiveSpecies = session.QueryOver<Species>().Inner.JoinQueryOver<Location>((Expression<Func<Species, IEnumerable<Location>>>) (sp => sp.InvasiveLocations)).Where((Expression<Func<Location, bool>>) (l => l.Id == this.locationId)).Cacheable().List();
          transaction.Commit();
          return invasiveSpecies;
        }
      }
    }));

    public IList<LocationPestRisk> GetPestRisks(int locationId) => RetryExecutionHandler.Execute<IList<LocationPestRisk>>((Func<IList<LocationPestRisk>>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          // ISSUE: reference to a compiler-generated field
          IList<LocationPestRisk> pestRisks = session.QueryOver<LocationPestRisk>().Fetch<LocationPestRisk, LocationPestRisk>(SelectMode.Fetch, (Expression<Func<LocationPestRisk, object>>) (pr => pr.Pest)).Fetch<LocationPestRisk, LocationPestRisk>(SelectMode.Fetch, (Expression<Func<LocationPestRisk, object>>) (pr => pr.Risk)).Fetch<LocationPestRisk, LocationPestRisk>(SelectMode.Fetch, (Expression<Func<LocationPestRisk, object>>) (pr => pr.Pest.SusceptableSpecies)).JoinQueryOver<Location>((Expression<Func<LocationPestRisk, Location>>) (pr => pr.Location)).Where((Expression<Func<Location, bool>>) (l => l.Id == this.locationId)).TransformUsing(Transformers.DistinctRootEntity).Cacheable().List();
          transaction.Commit();
          return pestRisks;
        }
      }
    }));

    public LocationCost GetLocationCost(int locationId) => RetryExecutionHandler.Execute<LocationCost>((Func<LocationCost>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          // ISSUE: reference to a compiler-generated field
          Location location = session.QueryOver<Location>().Where((Expression<Func<Location, bool>>) (l => l.Id == this.locationId)).Fetch<Location, Location>(SelectMode.Fetch, (Expression<Func<Location, object>>) (l => l.LocationCost)).Cacheable().SingleOrDefault();
          LocationRelation locationRelation;
          Location loc;
          for (loc = location; loc != null && loc.LocationCost == null; loc = locationRelation?.Parent)
            locationRelation = session.QueryOver<LocationRelation>().Where((Expression<Func<LocationRelation, bool>>) (r => r.Location == loc)).And((Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).JoinQueryOver<Location>((Expression<Func<LocationRelation, Location>>) (r => r.Parent)).Fetch<LocationRelation, Location>((Expression<Func<Location, object>>) (l => l.LocationCost)).Cacheable().SingleOrDefault();
          transaction.Commit();
          return loc?.LocationCost;
        }
      }
    }));

    public LocationEnvironmentalValue GetEnvironmentalValue(int locationId) => RetryExecutionHandler.Execute<LocationEnvironmentalValue>((Func<LocationEnvironmentalValue>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          // ISSUE: reference to a compiler-generated field
          Location location = session.QueryOver<Location>().Where((Expression<Func<Location, bool>>) (l => l.Id == this.locationId)).Fetch<Location, Location>(SelectMode.Fetch, (Expression<Func<Location, object>>) (l => l.EnvironmentalValue)).Cacheable().SingleOrDefault();
          LocationRelation locationRelation;
          Location loc;
          for (loc = location; loc != null && loc.EnvironmentalValue == null; loc = locationRelation?.Parent)
            locationRelation = session.QueryOver<LocationRelation>().Where((Expression<Func<LocationRelation, bool>>) (r => r.Location == loc)).And((Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).JoinQueryOver<Location>((Expression<Func<LocationRelation, Location>>) (r => r.Parent)).Fetch<LocationRelation, Location>((Expression<Func<Location, object>>) (l => l.EnvironmentalValue)).Cacheable().SingleOrDefault();
          transaction.Commit();
          return loc?.EnvironmentalValue;
        }
      }
    }));

    public LocationSpecies.Domain.Currency GetLocationCurrency(int locationId) => RetryExecutionHandler.Execute<LocationSpecies.Domain.Currency>((Func<LocationSpecies.Domain.Currency>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          // ISSUE: reference to a compiler-generated field
          Location location = session.QueryOver<Location>().Where((Expression<Func<Location, bool>>) (l => l.Id == this.locationId)).Fetch<Location, Location>(SelectMode.Fetch, (Expression<Func<Location, object>>) (l => l.Currency)).Cacheable().SingleOrDefault();
          LocationRelation locationRelation;
          Location loc;
          for (loc = location; loc != null && loc.Currency == null; loc = locationRelation?.Parent)
            locationRelation = session.QueryOver<LocationRelation>().Where((Expression<Func<LocationRelation, bool>>) (r => r.Location == loc)).And((Expression<Func<LocationRelation, bool>>) (r => r.Code != default (string))).JoinQueryOver<Location>((Expression<Func<LocationRelation, Location>>) (r => r.Parent)).Fetch<LocationRelation, Location>((Expression<Func<Location, object>>) (l => l.Currency)).Cacheable().SingleOrDefault();
          transaction.Commit();
          return loc?.Currency;
        }
      }
    }));

    public IList<LocationLeafNutrientValue> GetLeafNutrientValues(
      int locationId)
    {
      return RetryExecutionHandler.Execute<IList<LocationLeafNutrientValue>>((Func<IList<LocationLeafNutrientValue>>) (() =>
      {
        using (ISession session = this._sf.OpenSession())
        {
          using (ITransaction transaction = session.BeginTransaction())
          {
            // ISSUE: reference to a compiler-generated field
            IList<LocationLeafNutrientValue> leafNutrientValues = session.QueryOver<LocationLeafNutrientValue>().Fetch<LocationLeafNutrientValue, LocationLeafNutrientValue>(SelectMode.Fetch, (Expression<Func<LocationLeafNutrientValue, object>>) (lfv => lfv.LeafNutrient)).Where((Expression<Func<LocationLeafNutrientValue, bool>>) (lnv => lnv.Location.Id == this.locationId)).Cacheable().List();
            transaction.Commit();
            return leafNutrientValues;
          }
        }
      }));
    }

    public Location GetLocation(int locationId) => RetryExecutionHandler.Execute<Location>((Func<Location>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          // ISSUE: reference to a compiler-generated field
          Location location = session.Query<Location>().WithOptions<Location>((System.Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Where<Location>((Expression<Func<Location, bool>>) (l => l.Id == this.locationId)).SingleOrDefault<Location>();
          transaction.Commit();
          return location;
        }
      }
    }));

    public GrowthPeriod GetGrowthPeriod(int locationId) => RetryExecutionHandler.Execute<GrowthPeriod>((Func<GrowthPeriod>) (() =>
    {
      using (ISession session = this._sf.OpenSession())
      {
        using (session.BeginTransaction())
        {
          // ISSUE: reference to a compiler-generated field
          return session.QueryOver<GrowthPeriod>().JoinQueryOver<Location>((Expression<Func<GrowthPeriod, IEnumerable<Location>>>) (g => g.Locations)).Where((Expression<Func<Location, bool>>) (l => l.Id == this.locationId)).Cacheable().SingleOrDefault();
        }
      }
    }));
  }
}
