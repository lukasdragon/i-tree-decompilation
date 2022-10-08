// Decompiled with JetBrains decompiler
// Type: Eco.Util.Forecast.PollutantCalculation
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.NHibernate;
using Eco.Domain.v6;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Eco.Util.Forecast
{
  public class PollutantCalculation
  {
    private const double MIN_LAI = 0.5;
    private const double MAX_LAI = 18.0;
    private const int DECIDUOUS = 1;
    private const int EVERGREEN = 2;
    private static CancellationToken _cToken;
    private static InputSession _iSess;
    private static ISession _input;
    private static ISession _ls;
    private static int _steps;
    private static Year _yearObj;
    private static Eco.Domain.v6.Forecast _forecast;
    private static short _currentYear;
    private static bool _isUrban;
    private static int _countyId;
    private static Dictionary<string, LeafType> _lfTypes;
    private static PollutantCalculation.CarbonData _carbonData;
    private static List<PollutantCalculation.PollutantData> _pollutantData;
    private static Dictionary<int, double> _lfTypeCover;
    private static Dictionary<int, double> _lfTypeLfArea;
    private static double _totalTreeCover;
    private static double _totalLfArea;

    public static event ForecastEventHandler ProgressAdvanced;

    public static void begin(InputSession i, ISession input, ISession ls, CancellationToken ct)
    {
      PollutantCalculation.initialize(i, input, ls, ct);
      for (int index = 0; index <= (int) PollutantCalculation._forecast.NumYears; ++index)
      {
        PollutantCalculation._currentYear = (short) index;
        PollutantCalculation.incrementYearData();
      }
      PollutantCalculation.persistResults();
    }

    private static void initialize(
      InputSession i,
      ISession input,
      ISession ls,
      CancellationToken ct)
    {
      PollutantCalculation._cToken = ct;
      PollutantCalculation._ls = ls;
      PollutantCalculation._iSess = i;
      PollutantCalculation._input = input;
      PollutantCalculation._yearObj = PollutantCalculation._input.QueryOver<Year>().Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == PollutantCalculation._iSess.YearKey)).SingleOrDefault();
      PollutantCalculation._forecast = PollutantCalculation._input.QueryOver<Eco.Domain.v6.Forecast>().Where((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => (Guid?) f.Guid == PollutantCalculation._iSess.ForecastKey)).SingleOrDefault();
      PollutantCalculation._currentYear = (short) 0;
      PollutantCalculation._isUrban = PollutantCalculation.isUrban(PollutantCalculation._yearObj.Series.Project);
      PollutantCalculation._countyId = PollutantCalculation.getCountyId(PollutantCalculation._yearObj.Series.Project);
      PollutantCalculation._lfTypes = new Dictionary<string, LeafType>();
      PollutantCalculation._carbonData = new PollutantCalculation.CarbonData(PollutantCalculation.pollutantWithName("CO").Id, PollutantCalculation._forecast.NumYears);
      PollutantCalculation._pollutantData = PollutantCalculation.pollutantsWithoutName("CO");
      PollutantCalculation._lfTypeCover = new Dictionary<int, double>()
      {
        {
          1,
          0.0
        },
        {
          2,
          0.0
        }
      };
      PollutantCalculation._lfTypeLfArea = new Dictionary<int, double>()
      {
        {
          1,
          0.0
        },
        {
          2,
          0.0
        }
      };
      PollutantCalculation._totalTreeCover = PollutantCalculation._totalLfArea = 0.0;
      PollutantCalculation._steps = PollutantCalculation.numCalcSteps();
      PollutantCalculation.getCoversAndLfAreas();
      PollutantCalculation.getDbAmountsAndValues();
      PollutantCalculation.cacheCarbonData();
      PollutantCalculation.advance(Stage.CalculatingPollutants, 1, (Eco.Domain.v6.Entity) null);
      foreach (PollutantCalculation.PollutantData p in PollutantCalculation._pollutantData)
      {
        PollutantCalculation.checkCancellation();
        PollutantCalculation.cachePollutantData(p);
        PollutantCalculation.advance(Stage.CalculatingPollutants, 1, (Eco.Domain.v6.Entity) null);
      }
    }

    private static int numCalcSteps()
    {
      int num1 = PollutantCalculation._input.QueryOver<Cohort>().Inner.JoinQueryOver<Eco.Domain.v6.Forecast>((System.Linq.Expressions.Expression<Func<Cohort, Eco.Domain.v6.Forecast>>) (c => c.Forecast)).Where((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => f.Guid == PollutantCalculation._forecast.Guid)).RowCount();
      int num2 = PollutantCalculation._input.QueryOver<Cohort>().Where((System.Linq.Expressions.Expression<Func<Cohort, bool>>) (c => c.ForecastedYear == 0)).Inner.JoinQueryOver<Eco.Domain.v6.Forecast>((System.Linq.Expressions.Expression<Func<Cohort, Eco.Domain.v6.Forecast>>) (c => c.Forecast)).Where((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => f.Guid == PollutantCalculation._forecast.Guid)).RowCount();
      int num3 = PollutantCalculation.numModelPollutants();
      int num4 = PollutantCalculation._ls.QueryOver<PollutantLeafType>().Cacheable().RowCount();
      return num4 + num3 + num2 + num1 + num4 * (int) PollutantCalculation._forecast.NumYears + num4 * ((int) PollutantCalculation._forecast.NumYears + 1);
    }

    private static int numModelPollutants() => PollutantCalculation._input.GetNamedQuery("GetPollutantsFromTablePollutants").List<string>().Count;

    private static bool isUrban(Project proj) => PollutantCalculation._input.QueryOver<ProjectLocation>().Where((System.Linq.Expressions.Expression<Func<ProjectLocation, bool>>) (pl => pl.Project.Guid == proj.Guid && pl.LocationId == proj.LocationId)).Select((System.Linq.Expressions.Expression<Func<ProjectLocation, object>>) (pl => (object) pl.IsUrban)).SingleOrDefault<bool>();

    private static int getCountyId(Project proj)
    {
      Location location = PollutantCalculation._ls.QueryOver<LocationRelation>().Where((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (lr => lr.Location.Id == proj.LocationId && lr.Code != default (string))).Select((System.Linq.Expressions.Expression<Func<LocationRelation, object>>) (lr => lr.Parent)).Cacheable().SingleOrDefault<Location>();
      return location == null ? proj.LocationId : location.Id;
    }

    private static Pollutant pollutantWithName(string name) => PollutantCalculation._ls.QueryOver<Pollutant>().Where((System.Linq.Expressions.Expression<Func<Pollutant, bool>>) (p => p.Name == name)).Cacheable().SingleOrDefault();

    private static List<PollutantCalculation.PollutantData> pollutantsWithoutName(
      string name)
    {
      return PollutantCalculation._ls.QueryOver<PollutantLeafType>().Inner.JoinQueryOver<Pollutant>((System.Linq.Expressions.Expression<Func<PollutantLeafType, Pollutant>>) (plt => plt.Pollutant)).Where((System.Linq.Expressions.Expression<Func<Pollutant, bool>>) (p => p.Name != name)).Cacheable().List().Select<PollutantLeafType, PollutantCalculation.PollutantData>((Func<PollutantLeafType, PollutantCalculation.PollutantData>) (plt => new PollutantCalculation.PollutantData(plt, PollutantCalculation._forecast.NumYears))).ToList<PollutantCalculation.PollutantData>();
    }

    private static void getCoversAndLfAreas()
    {
      double num1 = PollutantCalculation._yearObj.Unit == YearUnit.English ? 0.30480000000121921 : 1.0;
      double num2 = PollutantCalculation._yearObj.Unit == YearUnit.English ? 0.092903 : 1.0;
      foreach (Cohort cohort in (IEnumerable<Cohort>) PollutantCalculation.cohortsInYear((short) 0))
      {
        PollutantCalculation.checkCancellation();
        PollutantCalculation.advance(Stage.CalculatingPollutants, 1, (Eco.Domain.v6.Entity) cohort);
        if (!PollutantCalculation._lfTypes.ContainsKey(cohort.Species))
          PollutantCalculation._lfTypes.Add(cohort.Species, PollutantCalculation.getParentLeafType(cohort.Species));
        double num3 = num1 * cohort.AvgCrownWidth;
        int key = PollutantCalculation._lfTypes[cohort.Species].Id == 1 ? 1 : 2;
        PollutantCalculation._lfTypeCover[key] += (double) cohort.NumTrees * Math.PI * num3 / 2.0 * num3 / 2.0;
        PollutantCalculation._lfTypeLfArea[key] += (double) cohort.NumTrees * cohort.AvgLeafArea * num2;
      }
      PollutantCalculation._totalTreeCover = PollutantCalculation._lfTypeCover.Sum<KeyValuePair<int, double>>((Func<KeyValuePair<int, double>, double>) (c => c.Value));
      PollutantCalculation._totalLfArea = PollutantCalculation._lfTypeLfArea.Sum<KeyValuePair<int, double>>((Func<KeyValuePair<int, double>, double>) (a => a.Value));
    }

    private static IList<Cohort> cohortsInYear(short year) => PollutantCalculation._input.QueryOver<Cohort>().Where((System.Linq.Expressions.Expression<Func<Cohort, bool>>) (c => c.ForecastedYear == (int) year)).Inner.JoinQueryOver<Eco.Domain.v6.Forecast>((System.Linq.Expressions.Expression<Func<Cohort, Eco.Domain.v6.Forecast>>) (c => c.Forecast)).Where((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => f.Guid == PollutantCalculation._forecast.Guid)).List();

    private static LeafType getParentLeafType(string spCode)
    {
      LeafType parentLeafType = PollutantCalculation._ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Code == spCode)).Cacheable().SingleOrDefault().LeafType;
      while (parentLeafType.Parent != null)
        parentLeafType = parentLeafType.Parent;
      return parentLeafType;
    }

    private static void getDbAmountsAndValues()
    {
      double num = 1.0;
      if (NationFeatures.IsUSlikeNation(PollutantCalculation._yearObj.Series.Project.NationCode))
      {
        ExchangeRate exchangeRate = PollutantCalculation._yearObj.ExchangeRate;
        num = exchangeRate != null ? exchangeRate.Price : 1.0;
      }
      IList<string> source = PollutantCalculation._ls.QueryOver<Pollutant>().Select(Projections.Distinct((IProjection) Projections.Property<Pollutant>((System.Linq.Expressions.Expression<Func<Pollutant, object>>) (p => p.Name)))).Cacheable().List<string>();
      Dictionary<string, double> dictionary = new Dictionary<string, double>();
      if (!NationFeatures.isUsingBenMAPresults(PollutantCalculation._yearObj.Series.Project.NationCode))
      {
        foreach ((string, double) tuple in (IEnumerable<(string, double)>) PollutantCalculation._input.GetNamedQuery("GetElementPrices").SetGuid("YearKey", PollutantCalculation._yearObj.Guid).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double)>()).List<(string, double)>())
        {
          if (source.Contains<string>(tuple.Item1, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase))
            dictionary.Add(tuple.Item1, tuple.Item2 / 1000000.0);
        }
      }
      foreach ((_, _, _) in (IEnumerable<(string, double, double)>) PollutantCalculation._input.GetNamedQuery("GetTreePollutantRemovalAmountAndValuesFromTablePollutants").SetGuid("YearGuid", PollutantCalculation._yearObj.Guid).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(string, double, double)>()).List<(string, double, double)>())
      {
        (string, double, double) item;
        if (item.Item1 == "CO")
        {
          PollutantCalculation._carbonData.DbAmount = item.Item2;
          PollutantCalculation._carbonData.DbValue = NationFeatures.isUsingBenMAPresults(PollutantCalculation._yearObj.Series.Project.NationCode) ? item.Item3 : (!dictionary.ContainsKey(item.Item1) ? item.Item3 : item.Item2 * dictionary[item.Item1]);
          PollutantCalculation.advance(Stage.CalculatingPollutants, 1, (Eco.Domain.v6.Entity) null);
        }
        else
        {
          foreach (PollutantCalculation.PollutantData pollutantData in (IEnumerable<PollutantCalculation.PollutantData>) PollutantCalculation._pollutantData.Where<PollutantCalculation.PollutantData>((Func<PollutantCalculation.PollutantData, bool>) (pl => pl.Root.Pollutant.Name == item.Item1)).ToList<PollutantCalculation.PollutantData>())
          {
            PollutantCalculation.checkCancellation();
            pollutantData.DbAmount = item.Item2;
            pollutantData.DbValue = NationFeatures.isUsingBenMAPresults(PollutantCalculation._yearObj.Series.Project.NationCode) ? num * item.Item3 : (!dictionary.ContainsKey(item.Item1) ? item.Item3 : item.Item2 * dictionary[item.Item1]);
            PollutantCalculation.advance(Stage.CalculatingPollutants, 1, (Eco.Domain.v6.Entity) null);
          }
        }
      }
    }

    private static void cacheCarbonData()
    {
      LocationCarbon locCarbon1 = PollutantCalculation.getLocCarbon(1);
      LocationCarbon locCarbon2 = PollutantCalculation.getLocCarbon(2);
      if (locCarbon1 != null)
      {
        PollutantCalculation._carbonData.AmountMultiplier[1] = locCarbon1.AmountMultiplier;
        PollutantCalculation._carbonData.ValueMultiplier[1] = locCarbon1.ValueMultiplier;
      }
      if (locCarbon2 != null)
      {
        PollutantCalculation._carbonData.AmountMultiplier[2] = locCarbon2.AmountMultiplier;
        PollutantCalculation._carbonData.ValueMultiplier[2] = locCarbon2.ValueMultiplier;
      }
      if (PollutantCalculation._totalTreeCover == 0.0)
      {
        PollutantCalculation._carbonData.AmountAdjust[1] = 0.0;
        PollutantCalculation._carbonData.AmountAdjust[2] = 0.0;
        PollutantCalculation._carbonData.ValueAdjust[1] = 0.0;
        PollutantCalculation._carbonData.ValueAdjust[2] = 0.0;
      }
      else
      {
        PollutantCalculation._carbonData.AmountAdjust[1] = PollutantCalculation._carbonData.DbAmount / PollutantCalculation._carbonData.AmountMultiplier[1] / PollutantCalculation._totalTreeCover;
        PollutantCalculation._carbonData.AmountAdjust[2] = PollutantCalculation._carbonData.DbAmount / PollutantCalculation._carbonData.AmountMultiplier[2] / PollutantCalculation._totalTreeCover;
        PollutantCalculation._carbonData.ValueAdjust[1] = PollutantCalculation._carbonData.DbValue / PollutantCalculation._carbonData.ValueMultiplier[1] / PollutantCalculation._totalTreeCover;
        PollutantCalculation._carbonData.ValueAdjust[2] = PollutantCalculation._carbonData.DbValue / PollutantCalculation._carbonData.ValueMultiplier[2] / PollutantCalculation._totalTreeCover;
      }
    }

    private static LocationCarbon getLocCarbon(int lfTypeId)
    {
      int pltId = lfTypeId == 1 ? 1 : 2;
      Location loc = PollutantCalculation._ls.QueryOver<Location>().Where((System.Linq.Expressions.Expression<Func<Location, bool>>) (lc => lc.Id == PollutantCalculation._countyId)).Cacheable().SingleOrDefault();
      LocationCarbon locCarbon;
      LocationRelation locationRelation;
      for (locCarbon = (LocationCarbon) null; locCarbon == null && loc != null; loc = locationRelation?.Parent)
      {
        locCarbon = PollutantCalculation._ls.QueryOver<LocationCarbon>().Where((System.Linq.Expressions.Expression<Func<LocationCarbon, bool>>) (lc => lc.Location.Id == loc.Id && lc.IsUrban == PollutantCalculation._isUrban)).Inner.JoinQueryOver<PollutantLeafType>((System.Linq.Expressions.Expression<Func<LocationCarbon, PollutantLeafType>>) (lc => lc.PollutantLeafType)).Where((System.Linq.Expressions.Expression<Func<PollutantLeafType, bool>>) (plt => plt.Id == pltId)).Cacheable().SingleOrDefault();
        locationRelation = loc.Relations.Where<LocationRelation>((Func<LocationRelation, bool>) (lr => lr.Code != "0")).Where<LocationRelation>((Func<LocationRelation, bool>) (lr => lr.Code != null)).SingleOrDefault<LocationRelation>();
      }
      return locCarbon;
    }

    private static IList<LocationPollutant> getLocPolls(int pltId)
    {
      Location loc = PollutantCalculation._ls.QueryOver<Location>().Where((System.Linq.Expressions.Expression<Func<Location, bool>>) (lc => lc.Id == PollutantCalculation._countyId)).Cacheable().SingleOrDefault();
      IList<LocationPollutant> locPolls;
      LocationRelation locationRelation;
      for (locPolls = (IList<LocationPollutant>) new List<LocationPollutant>(); locPolls.Count == 0 && loc != null; loc = locationRelation?.Parent)
      {
        locPolls = PollutantCalculation._ls.QueryOver<LocationPollutant>().Where((System.Linq.Expressions.Expression<Func<LocationPollutant, bool>>) (lp => lp.Location.Id == loc.Id && lp.IsUrban == PollutantCalculation._isUrban)).Inner.JoinQueryOver<PollutantLeafType>((System.Linq.Expressions.Expression<Func<LocationPollutant, PollutantLeafType>>) (lp => lp.PollutantLeafType)).Where((System.Linq.Expressions.Expression<Func<PollutantLeafType, bool>>) (plt => plt.Id == pltId)).Cacheable().List();
        locationRelation = loc.Relations.Where<LocationRelation>((Func<LocationRelation, bool>) (lr => lr.Code != "0")).Where<LocationRelation>((Func<LocationRelation, bool>) (lr => lr.Code != null)).SingleOrDefault<LocationRelation>();
      }
      return locPolls;
    }

    private static LocationPollutantRegression getLocPollRegress(
      int pltId)
    {
      Location loc = PollutantCalculation._ls.QueryOver<Location>().Where((System.Linq.Expressions.Expression<Func<Location, bool>>) (lc => lc.Id == PollutantCalculation._countyId)).Cacheable().SingleOrDefault();
      LocationPollutantRegression locPollRegress;
      LocationRelation locationRelation;
      for (locPollRegress = (LocationPollutantRegression) null; locPollRegress == null && loc != null; loc = locationRelation?.Parent)
      {
        locPollRegress = PollutantCalculation._ls.QueryOver<LocationPollutantRegression>().Where((System.Linq.Expressions.Expression<Func<LocationPollutantRegression, bool>>) (lc => lc.Location.Id == loc.Id && lc.IsUrban == PollutantCalculation._isUrban)).Inner.JoinQueryOver<PollutantLeafType>((System.Linq.Expressions.Expression<Func<LocationPollutantRegression, PollutantLeafType>>) (lc => lc.PollutantLeafType)).Where((System.Linq.Expressions.Expression<Func<PollutantLeafType, bool>>) (plt => plt.Id == pltId)).Cacheable().SingleOrDefault();
        locationRelation = loc.Relations.Where<LocationRelation>((Func<LocationRelation, bool>) (lr => lr.Code != "0")).Where<LocationRelation>((Func<LocationRelation, bool>) (lr => lr.Code != null)).SingleOrDefault<LocationRelation>();
      }
      return locPollRegress;
    }

    private static void cachePollutantData(PollutantCalculation.PollutantData p)
    {
      int id = p.Root.Id;
      LocationPollutantRegression locPollRegress = PollutantCalculation.getLocPollRegress(id);
      IList<LocationPollutant> locPolls = PollutantCalculation.getLocPolls(id);
      if (locPollRegress != null && locPollRegress.AmountIntercept != 0.0 && locPollRegress.AmountSlope != 0.0)
        PollutantCalculation.cacheAmount(p, locPollRegress);
      else if (locPolls.Count > 0)
        PollutantCalculation.cacheAmount(p, locPolls);
      if (locPollRegress != null && locPollRegress.ValueIntercept != 0.0 && locPollRegress.ValueSlope != 0.0)
      {
        PollutantCalculation.cacheValue(p, locPollRegress);
      }
      else
      {
        if (locPolls.Count <= 0)
          return;
        PollutantCalculation.cacheValue(p, locPolls);
      }
    }

    private static void cacheAmount(
      PollutantCalculation.PollutantData p,
      LocationPollutantRegression regression)
    {
      p.AmountRegression = regression;
      int id = p.Root.LeafType.Id;
      if (PollutantCalculation._lfTypeCover[id] == 0.0)
      {
        p.AmountAdjust = 1.0;
      }
      else
      {
        double d = PollutantCalculation._lfTypeLfArea[id] / PollutantCalculation._lfTypeCover[id];
        double num = Math.Exp(p.AmountRegression.AmountIntercept + p.AmountRegression.AmountSlope * Math.Log(d));
        if (PollutantCalculation._totalTreeCover == 0.0 || d == 0.0 || num == 0.0 || PollutantCalculation._lfTypeCover[id] == 0.0)
          p.AmountAdjust = 1.0;
        else
          p.AmountAdjust = p.DbAmount * PollutantCalculation._lfTypeCover[id] / PollutantCalculation._totalTreeCover / (num * PollutantCalculation._lfTypeCover[id]);
      }
    }

    private static void cacheAmount(
      PollutantCalculation.PollutantData p,
      IList<LocationPollutant> lookups)
    {
      p.AmountLookups = new Dictionary<double, LocationPollutant>();
      foreach (LocationPollutant lookup in (IEnumerable<LocationPollutant>) lookups)
      {
        PollutantCalculation.checkCancellation();
        p.AmountLookups[lookup.LAI] = lookup;
      }
      int id = p.Root.LeafType.Id;
      if (PollutantCalculation._lfTypeCover[id] == 0.0)
      {
        p.AmountAdjust = 1.0;
      }
      else
      {
        double key = PollutantCalculation.adjustedLAI(PollutantCalculation._lfTypeLfArea[id] / PollutantCalculation._lfTypeCover[id]);
        double amountMultiplier = p.AmountLookups[key].AmountMultiplier;
        if (PollutantCalculation._totalTreeCover == 0.0 || amountMultiplier == 0.0 || PollutantCalculation._lfTypeCover[id] == 0.0)
          p.AmountAdjust = 1.0;
        else
          p.AmountAdjust = p.DbAmount * PollutantCalculation._lfTypeCover[id] / PollutantCalculation._totalTreeCover / (amountMultiplier * PollutantCalculation._lfTypeCover[id]);
      }
    }

    private static void cacheValue(
      PollutantCalculation.PollutantData p,
      LocationPollutantRegression regression)
    {
      p.ValueRegression = regression;
      int id = p.Root.LeafType.Id;
      if (PollutantCalculation._lfTypeCover[id] == 0.0)
      {
        p.ValueAdjust = 1.0;
      }
      else
      {
        double d = PollutantCalculation._lfTypeLfArea[id] / PollutantCalculation._lfTypeCover[id];
        double num = Math.Exp(p.ValueRegression.ValueIntercept + p.ValueRegression.ValueSlope * Math.Log(d));
        if (PollutantCalculation._totalTreeCover == 0.0 || d == 0.0 || PollutantCalculation._lfTypeCover[id] == 0.0 || num == 0.0)
          p.ValueAdjust = 1.0;
        else
          p.ValueAdjust = p.DbValue * PollutantCalculation._lfTypeCover[id] / PollutantCalculation._totalTreeCover / (num * PollutantCalculation._lfTypeCover[id]);
      }
    }

    private static void cacheValue(
      PollutantCalculation.PollutantData p,
      IList<LocationPollutant> lookups)
    {
      p.ValueLookups = new Dictionary<double, LocationPollutant>();
      foreach (LocationPollutant lookup in (IEnumerable<LocationPollutant>) lookups)
      {
        PollutantCalculation.checkCancellation();
        p.ValueLookups[lookup.LAI] = lookup;
      }
      int id = p.Root.LeafType.Id;
      if (PollutantCalculation._lfTypeCover[id] == 0.0)
      {
        p.ValueAdjust = 1.0;
      }
      else
      {
        double key = PollutantCalculation.adjustedLAI(PollutantCalculation._lfTypeLfArea[id] / PollutantCalculation._lfTypeCover[id]);
        double valueMultiplier = p.ValueLookups[key].ValueMultiplier;
        if (PollutantCalculation._totalTreeCover == 0.0 || valueMultiplier == 0.0 || PollutantCalculation._lfTypeCover[id] == 0.0)
          p.ValueAdjust = 1.0;
        else
          p.ValueAdjust = p.DbValue * PollutantCalculation._lfTypeCover[id] / PollutantCalculation._totalTreeCover / (valueMultiplier * PollutantCalculation._lfTypeCover[id]);
      }
    }

    private static double adjustedLAI(double lai)
    {
      if (lai < 0.25)
        return -1.0;
      return lai > 18.0 ? 18.0 : Math.Round(2.0 * lai, MidpointRounding.AwayFromZero) / 2.0;
    }

    private static void incrementYearData()
    {
      IList<Cohort> cohortList = PollutantCalculation.cohortsInYear(PollutantCalculation._currentYear);
      double num1 = PollutantCalculation._yearObj.Unit == YearUnit.English ? 0.30480000000121921 : 1.0;
      double num2 = PollutantCalculation._yearObj.Unit == YearUnit.English ? 0.092903040000743217 : 1.0;
      double treeCover1 = 0.0;
      double num3 = 0.0;
      double treeCover2 = 0.0;
      double num4 = 0.0;
      foreach (Cohort cohort in (IEnumerable<Cohort>) cohortList)
      {
        double num5 = num1 * cohort.AvgCrownWidth / 2.0;
        if ((PollutantCalculation._lfTypes[cohort.Species].Id == 1 ? 1 : 2) == 1)
        {
          treeCover1 += (double) cohort.NumTrees * Math.PI * num5 * num5;
          num3 += (double) cohort.NumTrees * cohort.AvgLeafArea * num2;
        }
        else
        {
          treeCover2 += (double) cohort.NumTrees * Math.PI * num5 * num5;
          num4 += (double) cohort.NumTrees * cohort.AvgLeafArea * num2;
        }
        PollutantCalculation.advance(Stage.CalculatingPollutants, 1, (Eco.Domain.v6.Entity) cohort);
      }
      PollutantCalculation._carbonData.Results[(int) PollutantCalculation._currentYear].Amount = PollutantCalculation._carbonData.AmountMultiplier[1] * PollutantCalculation._carbonData.AmountAdjust[1] * treeCover1 + PollutantCalculation._carbonData.AmountMultiplier[2] * PollutantCalculation._carbonData.AmountAdjust[2] * treeCover2;
      PollutantCalculation._carbonData.Results[(int) PollutantCalculation._currentYear].Value = PollutantCalculation._carbonData.ValueMultiplier[1] * PollutantCalculation._carbonData.ValueAdjust[1] * treeCover1 + PollutantCalculation._carbonData.ValueMultiplier[2] * PollutantCalculation._carbonData.ValueAdjust[2] * treeCover2;
      PollutantCalculation.advance(Stage.CalculatingPollutants, 1, (Eco.Domain.v6.Entity) null);
      double lai1 = treeCover1 == 0.0 ? 0.0 : num3 / treeCover1;
      double lai2 = treeCover2 == 0.0 ? 0.0 : num4 / treeCover2;
      foreach (PollutantCalculation.PollutantData p in PollutantCalculation._pollutantData)
      {
        PollutantCalculation.checkCancellation();
        if (p.Root.LeafType.Id == 2)
          PollutantCalculation.incrementPollutantData(p, lai2, treeCover2);
        else
          PollutantCalculation.incrementPollutantData(p, lai1, treeCover1);
        PollutantCalculation.advance(Stage.CalculatingPollutants, 1, (Eco.Domain.v6.Entity) null);
      }
    }

    private static void incrementCarbonData(string spCode, double treeCover)
    {
      int id = PollutantCalculation._lfTypes[spCode].Id;
      double num1 = PollutantCalculation._carbonData.AmountMultiplier[id] * PollutantCalculation._carbonData.AmountAdjust[id] * treeCover;
      PollutantCalculation._carbonData.Results[(int) PollutantCalculation._currentYear].Amount += num1;
      double num2 = PollutantCalculation._carbonData.ValueMultiplier[id] * PollutantCalculation._carbonData.ValueAdjust[id] * treeCover;
      PollutantCalculation._carbonData.Results[(int) PollutantCalculation._currentYear].Value += num2;
    }

    private static void incrementPollutantData(
      PollutantCalculation.PollutantData p,
      double lai,
      double treeCover)
    {
      double key = PollutantCalculation.adjustedLAI(lai);
      if (p.AmountRegression != null)
      {
        if (lai != 0.0)
        {
          double d = p.AmountRegression.AmountIntercept + p.AmountRegression.AmountSlope * Math.Log(lai);
          p.Results[(int) PollutantCalculation._currentYear].Amount += treeCover * p.AmountAdjust * Math.Exp(d);
        }
      }
      else if (p.AmountLookups != null)
      {
        if (key != -1.0)
        {
          LocationPollutant locationPollutant = (LocationPollutant) null;
          if (p.AmountLookups.TryGetValue(key, out locationPollutant))
            p.Results[(int) PollutantCalculation._currentYear].Amount += locationPollutant.AmountMultiplier * p.AmountAdjust * treeCover;
        }
      }
      else
        p.Results[(int) PollutantCalculation._currentYear].Amount += p.DbAmount / PollutantCalculation._totalTreeCover * treeCover;
      if (p.ValueRegression != null)
      {
        if (lai == 0.0)
          return;
        double d = p.ValueRegression.ValueIntercept + p.ValueRegression.ValueSlope * Math.Log(lai);
        p.Results[(int) PollutantCalculation._currentYear].Value += treeCover * p.ValueAdjust * Math.Exp(d);
      }
      else if (p.ValueLookups != null)
      {
        if (key == -1.0)
          return;
        LocationPollutant locationPollutant = (LocationPollutant) null;
        if (!p.ValueLookups.TryGetValue(key, out locationPollutant))
          return;
        p.Results[(int) PollutantCalculation._currentYear].Value += p.ValueLookups[key].ValueMultiplier * p.ValueAdjust * treeCover;
      }
      else
        p.Results[(int) PollutantCalculation._currentYear].Value += PollutantCalculation._carbonData.DbValue / PollutantCalculation._totalTreeCover * treeCover;
    }

    private static void persistResults()
    {
      double num = PollutantCalculation._yearObj.Unit != YearUnit.English ? 0.001 : 0.0022046244201837776;
      using (ITransaction transaction = PollutantCalculation._input.BeginTransaction())
      {
        try
        {
          foreach (PollutantResult result in PollutantCalculation._carbonData.Results)
          {
            PollutantCalculation.checkCancellation();
            result.Amount *= num;
            PollutantCalculation._input.Save((object) result);
            PollutantCalculation.advance(Stage.CalculatingPollutants, 1, (Eco.Domain.v6.Entity) result);
          }
          transaction.Commit();
        }
        catch
        {
          transaction.Rollback();
        }
      }
      PollutantCalculation._pollutantData = PollutantCalculation._pollutantData.OrderBy<PollutantCalculation.PollutantData, int>((Func<PollutantCalculation.PollutantData, int>) (pd => pd.Root.Pollutant.Id)).ToList<PollutantCalculation.PollutantData>();
      do
      {
        using (ITransaction transaction = PollutantCalculation._input.BeginTransaction())
        {
          try
          {
            for (int index = 0; index <= (int) PollutantCalculation._forecast.NumYears; ++index)
            {
              PollutantResult pollutantResult = new PollutantResult()
              {
                Forecast = PollutantCalculation._forecast,
                ForecastedYear = index,
                PollutantId = PollutantCalculation._pollutantData[0].Root.Pollutant.Id,
                Amount = num * (PollutantCalculation._pollutantData[0].Results[index].Amount + PollutantCalculation._pollutantData[1].Results[index].Amount),
                Value = PollutantCalculation._pollutantData[0].Results[index].Value + PollutantCalculation._pollutantData[1].Results[index].Value
              };
              PollutantCalculation._input.Save((object) pollutantResult);
            }
          }
          catch
          {
            transaction.Rollback();
            throw;
          }
        }
        PollutantCalculation._pollutantData.RemoveAt(0);
        PollutantCalculation._pollutantData.RemoveAt(0);
      }
      while (PollutantCalculation._pollutantData.Count > 0);
    }

    private static void checkCancellation()
    {
      if (!PollutantCalculation._cToken.IsCancellationRequested)
        return;
      PollutantCalculation._cToken.ThrowIfCancellationRequested();
    }

    private static void advance(Stage stage, int steps, Eco.Domain.v6.Entity obj)
    {
      ForecastEventArgs forecastEventArgs = new ForecastEventArgs()
      {
        Forecast = PollutantCalculation._forecast,
        InitialYear = PollutantCalculation._yearObj,
        Progress = steps,
        EndProgress = PollutantCalculation._steps,
        AffectedObject = obj,
        Stage = stage,
        CurrentYear = PollutantCalculation._currentYear
      };
      ForecastEventHandler progressAdvanced = PollutantCalculation.ProgressAdvanced;
      if (progressAdvanced == null)
        return;
      foreach (Delegate invocation in progressAdvanced.GetInvocationList())
      {
        Control target = invocation.Target as Control;
        if (!target.IsDisposed && !target.Disposing)
        {
          if (target.InvokeRequired)
            target.Invoke(invocation, new object[2]
            {
              null,
              (object) forecastEventArgs
            });
          else
            invocation.DynamicInvoke(null, (object) forecastEventArgs);
        }
      }
    }

    private class CarbonData
    {
      public double DbAmount { get; set; }

      public double DbValue { get; set; }

      public Dictionary<int, double> AmountAdjust { get; set; }

      public Dictionary<int, double> ValueAdjust { get; set; }

      public Dictionary<int, double> AmountMultiplier { get; set; }

      public Dictionary<int, double> ValueMultiplier { get; set; }

      public PollutantResult[] Results { get; set; }

      public CarbonData(int carbonId, short years)
      {
        this.DbAmount = 0.0;
        this.DbValue = 0.0;
        this.AmountAdjust = new Dictionary<int, double>()
        {
          {
            1,
            1.0
          },
          {
            2,
            1.0
          }
        };
        this.ValueAdjust = new Dictionary<int, double>()
        {
          {
            1,
            1.0
          },
          {
            2,
            1.0
          }
        };
        this.AmountMultiplier = new Dictionary<int, double>()
        {
          {
            1,
            1.0
          },
          {
            2,
            1.0
          }
        };
        this.ValueMultiplier = new Dictionary<int, double>()
        {
          {
            1,
            1.0
          },
          {
            2,
            1.0
          }
        };
        this.Results = new PollutantResult[(int) years + 1];
        for (short index = 0; (int) index <= (int) PollutantCalculation._forecast.NumYears; ++index)
        {
          PollutantCalculation.checkCancellation();
          this.Results[(int) index] = new PollutantResult()
          {
            Forecast = PollutantCalculation._forecast,
            ForecastedYear = (int) index,
            PollutantId = carbonId,
            Amount = 0.0,
            Value = 0.0
          };
        }
      }
    }

    private class PollutantData
    {
      public PollutantLeafType Root { get; protected set; }

      public double AmountMultiplier { get; set; }

      public double ValueMultiplier { get; set; }

      public double AmountAdjust { get; set; }

      public double ValueAdjust { get; set; }

      public double DbAmount { get; set; }

      public double DbValue { get; set; }

      public LocationPollutantRegression AmountRegression { get; set; }

      public LocationPollutantRegression ValueRegression { get; set; }

      public Dictionary<double, LocationPollutant> AmountLookups { get; set; }

      public Dictionary<double, LocationPollutant> ValueLookups { get; set; }

      public PollutantResult[] Results { get; set; }

      public PollutantData(PollutantLeafType plt, short years)
      {
        this.Root = plt;
        this.DbAmount = 0.0;
        this.DbValue = 0.0;
        this.AmountAdjust = 1.0;
        this.ValueAdjust = 1.0;
        this.AmountRegression = (LocationPollutantRegression) null;
        this.ValueRegression = (LocationPollutantRegression) null;
        this.Results = new PollutantResult[(int) years + 1];
        for (short index = 0; (int) index <= (int) PollutantCalculation._forecast.NumYears; ++index)
        {
          PollutantCalculation.checkCancellation();
          this.Results[(int) index] = new PollutantResult()
          {
            Forecast = PollutantCalculation._forecast,
            ForecastedYear = (int) index,
            PollutantId = this.Root.Pollutant.Id,
            Amount = 0.0,
            Value = 0.0
          };
        }
      }
    }
  }
}
