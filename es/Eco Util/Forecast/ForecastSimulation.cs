// Decompiled with JetBrains decompiler
// Type: Eco.Util.Forecast.ForecastSimulation
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.NHibernate;
using Eco.Domain.v6;
using Eco.Util.Queries.Interfaces;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Eco.Util.Forecast
{
  public class ForecastSimulation
  {
    private static CancellationToken _cToken;
    private static ISession _input;
    private static ISession _ls;
    private static Year _initYear;
    private static Eco.Domain.v6.Forecast _forecast;
    private static Stage _currentStage;
    private static int _steps;
    private const int INIT_STEPS = 7;
    private const double URBAN_REDUCTION_FACTOR = 0.8;
    private static readonly Random getRandom = new Random();
    private static short _currentYear;
    private static long _cohortTag;
    private static double _dbhToEng;
    private static double _dimToEng;
    private static double _dbhToMetr;
    private static double _dimToMetr;
    private static double _dimOutFact;
    private static double _wtOutFact;
    private static double _areaOutFact;
    private static double _avgCleAdj;
    private static double _avgTreeMortFactor;
    private static TropicalClimate? _tropicalClimate;
    private static Condition _healthy;
    private static Dictionary<int, double> _dbhValues;
    private static IList<ForecastPestEvent> _pestEvents;
    private static IList<ForecastWeatherEvent> _weatherEvents;
    private static Dictionary<Guid, Pest> _pests;
    private static Dictionary<Guid, ISet<Cohort>> _incoming;
    private static Dictionary<string, TaxonModel> _taxa;
    private static Dictionary<string, Mortality> _genusMorts;
    private static Dictionary<string, Mortality> _condMorts;
    private static Dictionary<string, Mortality> _strataMorts;
    private static Dictionary<ForecastSimulation.Distrib, double> _distribution;
    private static Dictionary<long, Dictionary<ForecastSimulation.Ratio, double>> _ratio;
    private static Dictionary<long, double> _initialHeight;
    private static Dictionary<long, ForecastSimulation.CohortStatus> _statusOfInitialCohorts;
    private const short DBH_RANGES = 7;
    private static Dictionary<ForecastSimulation.Scale, int> LONGEVITY;
    private static Dictionary<ForecastSimulation.Scale, double> MATURE_HT;
    private static Dictionary<ForecastSimulation.Cle, double> CLE_ADJUST;
    private static Dictionary<string, Mortality> BASE_MORTALITIES;
    private static Dictionary<ForecastSimulation.Scale, double[]> MORT_DBHS;
    private static double[] MORT_RED_FACT;
    private static double defaultDieback = Condition.Default.PctDieback;
    private static double _carbonCap;
    private static double _bigGrowDBH;

    public static event ForecastEventHandler ProgressAdvanced;

    public static void begin(
      EstimateUtil estUtil,
      InputSession i,
      ISession input,
      ISession ls,
      CancellationToken ct)
    {
      Dictionary<string, double> dictionary1 = new Dictionary<string, double>();
      Dictionary<string, double> dictionary2 = new Dictionary<string, double>();
      Dictionary<string, double> dictionary3 = new Dictionary<string, double>();
      Dictionary<string, double> dictionary4 = new Dictionary<string, double>();
      Dictionary<string, double> InitialCohortTreeCoverByStrata = new Dictionary<string, double>();
      Dictionary<string, double> InitialCohortLeafAreaByStrata = new Dictionary<string, double>();
      Dictionary<string, double> InitialCohortLeafBiomassByStrata = new Dictionary<string, double>();
      Dictionary<string, double> InitialCohortCarbonStorageByStrata = new Dictionary<string, double>();
      ForecastSimulation._currentStage = Stage.Initializing;
      ForecastSimulation.initialize(i, input, ls, ct);
      ForecastSimulation._currentStage = Stage.CalculatingCohorts;
      ForecastSimulation._steps = ForecastSimulation.numCalcSteps();
      IList<Cohort> cohortList = ForecastSimulation.createInitialCohorts();
      ForecastSimulation.calculateAdjustmentToSASresults(estUtil, i, input, cohortList, InitialCohortTreeCoverByStrata, dictionary1, InitialCohortLeafAreaByStrata, dictionary2, InitialCohortLeafBiomassByStrata, dictionary3, InitialCohortCarbonStorageByStrata, dictionary4);
      ForecastSimulation.persist(cohortList);
      foreach (Cohort cohort in (IEnumerable<Cohort>) cohortList)
      {
        ForecastSimulation.CohortStatus cohortStatus1 = new ForecastSimulation.CohortStatus();
        cohortStatus1.originalNumberOfTrees = cohort.NumTrees;
        cohortStatus1.currentNumberOfTrees = cohort.NumTrees;
        ForecastSimulation.CohortStatus cohortStatus2 = cohortStatus1;
        Guid guid = cohort.Stratum.Guid;
        string str = guid.ToString();
        cohortStatus2.strataGuidStr = str;
        guid = cohort.Stratum.Guid;
        string key = guid.ToString();
        cohortStatus1.percentOfTotalTreeCover = 0.7853981625 * (double) cohort.NumTrees * cohort.AvgCrownWidth * cohort.AvgCrownWidth / InitialCohortTreeCoverByStrata[key];
        cohortStatus1.percentOfTotalLeafArea = (double) cohort.NumTrees * cohort.AvgLeafArea / InitialCohortLeafAreaByStrata[key];
        cohortStatus1.percentOfTotalLeafBiomass = (double) cohort.NumTrees * cohort.AvgLeafBiomass / InitialCohortLeafBiomassByStrata[key];
        cohortStatus1.percentOfTotalCarbonStorage = (double) cohort.NumTrees * cohort.CarbonStorage / InitialCohortCarbonStorageByStrata[key];
        ForecastSimulation._statusOfInitialCohorts.Add(cohort.CohortTag, cohortStatus1);
      }
      ForecastSimulation.SaveAdjustmentToTempTable(0, dictionary1, dictionary2, dictionary3, dictionary4);
      ForecastSimulation._currentStage = Stage.CalculatingReplantCohorts;
      ForecastSimulation._avgCleAdj = ForecastSimulation.avgCleAdjustment(cohortList);
      ForecastSimulation._avgTreeMortFactor = ForecastSimulation.avgTreeMortFactor(cohortList);
      ForecastSimulation._input.Refresh((object) ForecastSimulation._forecast);
      ForecastSimulation._steps = ForecastSimulation._forecast.Replanting.Count;
      foreach (Replanting inc in (IEnumerable<Replanting>) ForecastSimulation._forecast.Replanting)
      {
        ForecastSimulation.checkCancellation();
        ForecastSimulation._incoming.Add(inc.Guid, ForecastSimulation.createIncomingCohorts(inc));
        ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) inc);
      }
      ForecastSimulation._currentStage = Stage.Forecasting;
      ForecastSimulation._currentYear = (short) 1;
      ForecastSimulation._steps = ForecastSimulation.numForecastSteps();
      for (ForecastSimulation._currentYear = (short) 1; (int) ForecastSimulation._currentYear <= (int) ForecastSimulation._forecast.NumYears; ++ForecastSimulation._currentYear)
      {
        ForecastSimulation.checkCancellation();
        cohortList = ForecastSimulation.forecast(cohortList);
        ForecastSimulation.persist(cohortList);
        ForecastSimulation.SaveAdjustmentToTempTable((int) ForecastSimulation._currentYear, dictionary1, dictionary2, dictionary3, dictionary4);
      }
    }

    private static void SaveAdjustmentToTempTable(
      int forecastYear,
      Dictionary<string, double> initialTreeCoverAdjustAmountByStrata,
      Dictionary<string, double> initialLeafAreaAdjustAmountByStrata,
      Dictionary<string, double> initialLeafBiomassAdjustAmountByStrata,
      Dictionary<string, double> initialCarbonStorageAdjustAmountByStrata)
    {
      if (forecastYear == 0)
        ForecastSimulation.RecreateEcoForecastAdjustmentTable();
      Dictionary<string, double> data1 = new Dictionary<string, double>();
      Dictionary<string, double> data2 = new Dictionary<string, double>();
      Dictionary<string, double> data3 = new Dictionary<string, double>();
      Dictionary<string, double> data4 = new Dictionary<string, double>();
      foreach (KeyValuePair<long, ForecastSimulation.CohortStatus> statusOfInitialCohort in ForecastSimulation._statusOfInitialCohorts)
      {
        string strataGuidStr = statusOfInitialCohort.Value.strataGuidStr;
        double num = (double) statusOfInitialCohort.Value.currentNumberOfTrees / (double) statusOfInitialCohort.Value.originalNumberOfTrees;
        ForecastSimulation.AddAmounts(data1, strataGuidStr, num * statusOfInitialCohort.Value.percentOfTotalTreeCover * initialTreeCoverAdjustAmountByStrata[strataGuidStr]);
        ForecastSimulation.AddAmounts(data2, strataGuidStr, num * statusOfInitialCohort.Value.percentOfTotalLeafArea * initialLeafAreaAdjustAmountByStrata[strataGuidStr]);
        ForecastSimulation.AddAmounts(data3, strataGuidStr, num * statusOfInitialCohort.Value.percentOfTotalLeafBiomass * initialLeafBiomassAdjustAmountByStrata[strataGuidStr]);
        ForecastSimulation.AddAmounts(data4, strataGuidStr, num * statusOfInitialCohort.Value.percentOfTotalCarbonStorage * initialCarbonStorageAdjustAmountByStrata[strataGuidStr]);
      }
      ITransaction transaction = ForecastSimulation._input.BeginTransaction();
      IQuery namedQuery1 = ForecastSimulation._input.GetNamedQuery("InsertIntoEcoForecastAdjustment");
      IQuery namedQuery2 = ForecastSimulation._input.GetNamedQuery("InsertTotalIntoEcoForecastAdjustment");
      namedQuery1.SetParameter<int>("year", forecastYear);
      namedQuery2.SetParameter<int>("year", forecastYear);
      ForecastSimulation.InsertEcoForecastAdjustmentData(CohortResultDataType.TreeCover, data1, namedQuery1, namedQuery2);
      ForecastSimulation.InsertEcoForecastAdjustmentData(CohortResultDataType.LeafArea, data2, namedQuery1, namedQuery2);
      ForecastSimulation.InsertEcoForecastAdjustmentData(CohortResultDataType.LeafBiomass, data3, namedQuery1, namedQuery2);
      ForecastSimulation.InsertEcoForecastAdjustmentData(CohortResultDataType.CarbonStorage, data4, namedQuery1, namedQuery2);
      transaction.Commit();
    }

    private static void AddAmounts(Dictionary<string, double> obj, string guid, double val)
    {
      if (obj.ContainsKey(guid))
        obj[guid] += val;
      else
        obj.Add(guid, val);
    }

    private static void InsertEcoForecastAdjustmentData(
      CohortResultDataType cohortResultDataType,
      Dictionary<string, double> data,
      IQuery q,
      IQuery qTotal)
    {
      double val = 0.0;
      q.SetParameter<CohortResultDataType>("datatype", cohortResultDataType);
      foreach (KeyValuePair<string, double> keyValuePair in data)
      {
        q.SetParameter<Guid>("stratumkey", new Guid(keyValuePair.Key)).SetParameter<double>("difference", keyValuePair.Value).ExecuteUpdate();
        val += keyValuePair.Value;
      }
      qTotal.SetParameter<CohortResultDataType>("datatype", cohortResultDataType).SetParameter<double>("difference", val).ExecuteUpdate();
    }

    private static void RecreateEcoForecastAdjustmentTable()
    {
      try
      {
        ForecastSimulation._input.GetNamedQuery("DropEcoForecastAdjustment").ExecuteUpdate();
      }
      catch (Exception ex)
      {
      }
      ForecastSimulation._input.GetNamedQuery("CreateEcoForecastAdjustment").ExecuteUpdate();
    }

    private static void checkCancellation()
    {
      if (!ForecastSimulation._cToken.IsCancellationRequested)
        return;
      ForecastSimulation._cToken.ThrowIfCancellationRequested();
    }

    private static void initialize(
      InputSession i,
      ISession input,
      ISession ls,
      CancellationToken ct)
    {
      ForecastSimulation._steps = 7;
      ForecastSimulation._cToken = ct;
      ForecastSimulation._ls = ls;
      ForecastSimulation._input = input;
      ForecastSimulation._initYear = ForecastSimulation._input.QueryOver<Year>().Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == i.YearKey)).SingleOrDefault();
      ForecastSimulation._forecast = ForecastSimulation._input.QueryOver<Eco.Domain.v6.Forecast>().Where((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => (Guid?) f.Guid == i.ForecastKey)).SingleOrDefault();
      if (ForecastSimulation._forecast.Year.Series.Project.Locations.Where<ProjectLocation>((Func<ProjectLocation, bool>) (p => p.LocationId == ForecastSimulation._forecast.Year.Series.Project.LocationId)).First<ProjectLocation>().UseTropical)
        ForecastSimulation._tropicalClimate = ls.QueryOver<Location>().Where((System.Linq.Expressions.Expression<Func<Location, bool>>) (s => s.Id == ForecastSimulation._forecast.Year.Series.Project.LocationId)).Cacheable().SingleOrDefault().TropicalClimate;
      else
        ForecastSimulation._tropicalClimate = new TropicalClimate?();
      ForecastSimulation.LONGEVITY = new Dictionary<ForecastSimulation.Scale, int>()
      {
        {
          ForecastSimulation.Scale.Small,
          0
        },
        {
          ForecastSimulation.Scale.Medium,
          35
        },
        {
          ForecastSimulation.Scale.Large,
          55
        }
      };
      ForecastSimulation.MATURE_HT = new Dictionary<ForecastSimulation.Scale, double>()
      {
        {
          ForecastSimulation.Scale.Small,
          0.0
        },
        {
          ForecastSimulation.Scale.Medium,
          40.0
        },
        {
          ForecastSimulation.Scale.Large,
          60.0
        }
      };
      ForecastSimulation.CLE_ADJUST = new Dictionary<ForecastSimulation.Cle, double>()
      {
        {
          ForecastSimulation.Cle.Open,
          1.0
        },
        {
          ForecastSimulation.Cle.Park,
          1.78
        },
        {
          ForecastSimulation.Cle.Closed,
          2.26
        }
      };
      ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) null);
      ForecastSimulation._genusMorts = new Dictionary<string, Mortality>();
      ForecastSimulation._condMorts = new Dictionary<string, Mortality>();
      ForecastSimulation._strataMorts = new Dictionary<string, Mortality>();
      ForecastSimulation.BASE_MORTALITIES = ForecastSimulation.getBaseMortalities();
      ForecastSimulation.getOverrideMortalities();
      ForecastSimulation.MORT_DBHS = new Dictionary<ForecastSimulation.Scale, double[]>()
      {
        {
          ForecastSimulation.Scale.Small,
          new double[7]{ 0.0, 2.0, 4.0, 8.0, 12.0, 16.0, 20.0 }
        },
        {
          ForecastSimulation.Scale.Medium,
          new double[7]{ 0.0, 3.0, 6.0, 12.0, 18.0, 24.0, 30.0 }
        },
        {
          ForecastSimulation.Scale.Large,
          new double[7]{ 0.0, 4.0, 8.0, 16.0, 24.0, 32.0, 40.0 }
        }
      };
      ForecastSimulation.MORT_RED_FACT = new double[7]
      {
        0.029,
        0.022,
        0.021,
        0.021,
        0.029,
        0.03,
        0.054
      };
      ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) null);
      if (ForecastSimulation._initYear.Unit == YearUnit.Metric)
      {
        ForecastSimulation._dbhToEng = 0.393700787;
        ForecastSimulation._dimToEng = 1250.0 / 381.0;
        ForecastSimulation._dbhToMetr = 1.0;
        ForecastSimulation._dimToMetr = 1.0;
        ForecastSimulation._dimOutFact = 0.30480000000121921;
        ForecastSimulation._wtOutFact = 1.0;
        ForecastSimulation._areaOutFact = 1.0;
        ForecastSimulation._carbonCap = 7500.0;
        ForecastSimulation._bigGrowDBH = 0.203200000207264;
      }
      else
      {
        ForecastSimulation._dbhToEng = 1.0;
        ForecastSimulation._dimToEng = 1.0;
        ForecastSimulation._dbhToMetr = 2.5400000025908;
        ForecastSimulation._dimToMetr = 0.30480000000121921;
        ForecastSimulation._dimOutFact = 1.0;
        ForecastSimulation._wtOutFact = 2.204622621845;
        ForecastSimulation._areaOutFact = 10.763910416623613;
        ForecastSimulation._carbonCap = 16534.6696638375;
        ForecastSimulation._bigGrowDBH = 0.08;
      }
      ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) null);
      ForecastSimulation._weatherEvents = ForecastSimulation.weatherEvents();
      ForecastSimulation._pestEvents = ForecastSimulation.pestEvents();
      ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) null);
      ForecastSimulation._pests = ForecastSimulation.getEventPests();
      ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) null);
      ForecastSimulation._healthy = ForecastSimulation.getHealthyCondition();
      ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) null);
      ForecastSimulation._dbhValues = ForecastSimulation.getDBHValues();
      ForecastSimulation._taxa = new Dictionary<string, TaxonModel>();
      ForecastSimulation._incoming = new Dictionary<Guid, ISet<Cohort>>();
      ForecastSimulation._distribution = new Dictionary<ForecastSimulation.Distrib, double>();
      ForecastSimulation._cohortTag = 0L;
      ForecastSimulation._ratio = new Dictionary<long, Dictionary<ForecastSimulation.Ratio, double>>();
      ForecastSimulation._initialHeight = new Dictionary<long, double>();
      ForecastSimulation._statusOfInitialCohorts = new Dictionary<long, ForecastSimulation.CohortStatus>();
      ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) null);
    }

    private static IList<Cohort> createInitialCohorts()
    {
      Dictionary<Guid, float> sampledSize = new Dictionary<Guid, float>();
      foreach (Strata stratum1 in (IEnumerable<Strata>) ForecastSimulation._initYear.Strata)
      {
        Strata stratum = stratum1;
        ForecastSimulation.checkCancellation();
        object obj = ForecastSimulation._input.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.IsComplete)).Select((IProjection) Projections.Sum<Plot>((System.Linq.Expressions.Expression<Func<Plot, object>>) (p => (object) (p.Size * (float) p.PercentMeasured / 100f)))).Inner.JoinQueryOver<Strata>((System.Linq.Expressions.Expression<Func<Plot, Strata>>) (p => p.Strata)).Where((System.Linq.Expressions.Expression<Func<Strata, bool>>) (s => s.Guid == stratum.Guid)).SingleOrDefault<object>();
        if (obj != null)
          sampledSize.Add(stratum.Guid, (float) obj);
        else
          sampledSize.Add(stratum.Guid, 0.0f);
      }
      Dictionary<Cohort, ISet<Tree>> cohortTrees = new Dictionary<Cohort, ISet<Tree>>();
      TreeStatus[] source = new TreeStatus[5]
      {
        TreeStatus.InitialSample,
        TreeStatus.Ingrowth,
        TreeStatus.NoChange,
        TreeStatus.Planted,
        TreeStatus.Unknown
      };
      foreach (Plot plot in (IEnumerable<Plot>) ForecastSimulation._initYear.Plots)
      {
        if (plot.IsComplete)
        {
          foreach (Tree tree in (IEnumerable<Tree>) plot.Trees)
          {
            if (((IEnumerable<TreeStatus>) source).Contains<TreeStatus>((TreeStatus) tree.Status) && (!ForecastSimulation._initYear.RecordCrownCondition || tree.Crown.Condition.PctDieback != 100.0))
            {
              if (!ForecastSimulation._taxa.ContainsKey(tree.Species))
                ForecastSimulation._taxa.Add(tree.Species, new TaxonModel(tree.Species, ForecastSimulation._ls));
              ForecastSimulation.addTreeToCohort(tree, cohortTrees);
              ForecastSimulation.checkCancellation();
              ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) tree);
            }
          }
        }
      }
      if (cohortTrees.Count == 0)
        throw new Exception("Forecast is not able to run due to the lack of initial tree cohorts");
      foreach (Cohort key in cohortTrees.Keys)
      {
        ForecastSimulation.checkCancellation();
        ForecastSimulation.updateInitialCohort(key, cohortTrees[key], sampledSize);
      }
      Dictionary<string, double> dictionary = new Dictionary<string, double>();
      foreach (ForecastSimulation.Distrib key in ForecastSimulation._distribution.Keys.ToList<ForecastSimulation.Distrib>())
      {
        ForecastSimulation.checkCancellation();
        if (dictionary.ContainsKey(key.Stratum))
          dictionary[key.Stratum] += ForecastSimulation._distribution[key];
        else
          dictionary.Add(key.Stratum, ForecastSimulation._distribution[key]);
      }
      foreach (ForecastSimulation.Distrib key in ForecastSimulation._distribution.Keys.ToList<ForecastSimulation.Distrib>())
      {
        ForecastSimulation.checkCancellation();
        ForecastSimulation._distribution[key] /= dictionary[key.Stratum];
      }
      return (IList<Cohort>) cohortTrees.Keys.ToList<Cohort>();
    }

    private static ISet<Cohort> createIncomingCohorts(Replanting inc)
    {
      ISet<Cohort> incomingCohorts = (ISet<Cohort>) new HashSet<Cohort>();
      foreach (ForecastSimulation.Distrib key in ForecastSimulation._distribution.Keys)
      {
        if (key.Stratum == inc.StratumDesc)
          incomingCohorts.Add(ForecastSimulation.addIncomingCohort(inc, key));
      }
      return incomingCohorts;
    }

    private static IList<Cohort> forecast(IList<Cohort> oldCohorts)
    {
      IList<Cohort> cohortList = ForecastSimulation.replantedTrees();
      HashSet<Cohort> cohortSet = new HashSet<Cohort>();
      foreach (Cohort oldCohort in (IEnumerable<Cohort>) oldCohorts)
      {
        ForecastSimulation.checkCancellation();
        Cohort c = oldCohort.Clone() as Cohort;
        c.ForecastedYear = (int) ForecastSimulation._currentYear;
        c.NumTrees -= ForecastSimulation.treesThatDied(c);
        if (ForecastSimulation._statusOfInitialCohorts.ContainsKey(c.CohortTag))
          ForecastSimulation._statusOfInitialCohorts[oldCohort.CohortTag].currentNumberOfTrees = c.NumTrees;
        c.Parent = oldCohort;
        if (c.NumTrees > 0)
        {
          TaxonModel taxon = ForecastSimulation._taxa[c.Species];
          if (c.Parent.CarbonStorage >= ForecastSimulation._carbonCap)
            c.AvgDBH += ForecastSimulation._bigGrowDBH;
          else
            c.AvgDBH += ForecastSimulation.annualDBHGrowthInInches(c) / ForecastSimulation._dbhToEng;
          double num = c.AvgDBH * ForecastSimulation._dbhToEng;
          c.AvgTreeHeight = ForecastSimulation._dimOutFact * taxon.Height(num) * ForecastSimulation._ratio[c.CohortTag][ForecastSimulation.Ratio.Height];
          c.AvgCrownHeight = ForecastSimulation._dimOutFact * taxon.CrownHeight(num) * ForecastSimulation._ratio[c.CohortTag][ForecastSimulation.Ratio.CrownHeight];
          c.AvgCrownWidth = ForecastSimulation._dimOutFact * taxon.CrownWidthBySpeciesGroup(num, ForecastSimulation._initialHeight[c.CohortTag] * ForecastSimulation._dimToEng) * ForecastSimulation._ratio[c.CohortTag][ForecastSimulation.Ratio.CrownWidth];
          if (c.AvgCrownHeight > c.AvgTreeHeight)
            c.AvgCrownHeight = c.AvgTreeHeight;
          ForecastSimulation.getLeafData(c);
          c.CarbonStorage = ForecastSimulation._tropicalClimate.HasValue ? ForecastSimulation._wtOutFact * 0.5 * taxon.TropicalBiomass(ForecastSimulation._tropicalClimate.Value, ForecastSimulation._dbhToMetr * c.AvgDBH, ForecastSimulation._dimToMetr * c.AvgTreeHeight) : ForecastSimulation._wtOutFact * (c.CrownLightExposure < 4 || taxon.Root.IsPalm() ? 1.0 : 0.8) * (taxon.Root.IsPalm() ? 0.4 : 0.5) * taxon.Biomass(ForecastSimulation._dbhToMetr * c.AvgDBH, ForecastSimulation._dimToMetr * c.AvgTreeHeight, ForecastSimulation._dimToMetr * c.AvgCrownHeight, c.AvgLeafBiomass / ForecastSimulation._wtOutFact);
          if (c.Parent.CarbonStorage >= c.CarbonStorage || c.CarbonStorage > ForecastSimulation._carbonCap)
            c.CarbonStorage = c.Parent.CarbonStorage + ForecastSimulation._dbhToMetr * (c.AvgDBH - c.Parent.AvgDBH) * 40.0 * ForecastSimulation._wtOutFact;
          cohortList.Add(c);
          ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) c);
        }
      }
      return cohortList;
    }

    private static double avgDbhOf(Tree t)
    {
      double num;
      return t.Plot.Year.DBHActual ? Math.Sqrt(t.Stems.Sum<Stem>((Func<Stem, double>) (s => s.Diameter * s.Diameter))) : Math.Sqrt(t.Stems.Sum<Stem>((Func<Stem, double>) (s => !ForecastSimulation._dbhValues.TryGetValue((int) s.Diameter, out num) ? 0.0 : num * num)));
    }

    private static void addTreeToCohort(Tree t, Dictionary<Cohort, ISet<Tree>> cohortTrees)
    {
      double dbh = Math.Round(ForecastSimulation.avgDbhOf(t) * ForecastSimulation._dbhToEng, 1) / ForecastSimulation._dbhToEng;
      Cohort key1 = cohortTrees.Keys.Where<Cohort>((Func<Cohort, bool>) (c => c.Forecast.Guid == ForecastSimulation._forecast.Guid && c.Species == t.Species && c.Stratum.Guid == t.Plot.Strata.Guid && c.AvgDBH == dbh && c.Condition == t.Crown.Condition && c.PercentCrownMissing == t.Crown.PercentMissing && (CrownLightExposure) c.CrownLightExposure == t.Crown.LightExposure)).SingleOrDefault<Cohort>();
      if (key1 != null)
      {
        cohortTrees[key1].Add(t);
      }
      else
      {
        Cohort key2 = new Cohort()
        {
          Forecast = ForecastSimulation._forecast,
          ForecastedYear = 0,
          Species = t.Species,
          Stratum = t.Plot.Strata,
          AvgDBH = dbh,
          Condition = t.Crown.Condition,
          PercentCrownMissing = t.Crown.PercentMissing,
          CrownLightExposure = (int) t.Crown.LightExposure
        };
        cohortTrees.Add(key2, (ISet<Tree>) new HashSet<Tree>()
        {
          t
        });
      }
    }

    private static void updateInitialCohort(
      Cohort c,
      ISet<Tree> trees,
      Dictionary<Guid, float> sampledSize)
    {
      if (c.Forecast.Year.Series.IsSample)
      {
        IList<Tree> list = (IList<Tree>) trees.Where<Tree>((Func<Tree, bool>) (maxT => maxT.Plot.Id == trees.Max<Tree>((Func<Tree, int>) (t => t.Plot.Id)))).ToList<Tree>();
        Strata strata = list.Single<Tree>((Func<Tree, bool>) (maxT => maxT.Id == list.Max<Tree>((Func<Tree, int>) (t => t.Id)))).Plot.Strata;
        c.NumTrees = Convert.ToInt32((float) trees.Count * strata.Size / sampledSize[strata.Guid]);
      }
      else
        c.NumTrees = trees.Count;
      c.AvgTreeHeight = trees.Average<Tree>((Func<Tree, double>) (t => !ForecastSimulation._initYear.RecordHeight || (double) t.TreeHeight == -1.0 ? ForecastSimulation._dimOutFact * ForecastSimulation._taxa[t.Species].Height(ForecastSimulation.avgDbhOf(t) * ForecastSimulation._dbhToEng) : (double) t.TreeHeight));
      c.AvgCrownHeight = trees.Average<Tree>((Func<Tree, double>) (t => !ForecastSimulation._initYear.RecordCrownSize || (double) t.TreeHeight == -1.0 || (double) t.Crown.BaseHeight == -1.0 ? ForecastSimulation._dimOutFact * ForecastSimulation._taxa[t.Species].CrownHeight(ForecastSimulation.avgDbhOf(t) * ForecastSimulation._dbhToEng) : (double) t.TreeHeight - (double) t.Crown.BaseHeight));
      c.AvgCrownWidth = trees.Average<Tree>((Func<Tree, double>) (t => !ForecastSimulation._initYear.RecordCrownSize || (double) t.Crown.WidthEW == -1.0 || (double) t.Crown.WidthNS == -1.0 ? ForecastSimulation._dimOutFact * ForecastSimulation._taxa[t.Species].CrownWidthBySpeciesGroup(ForecastSimulation.avgDbhOf(t) * ForecastSimulation._dbhToEng, c.AvgTreeHeight * ForecastSimulation._dimToEng) : ((double) t.Crown.WidthEW + (double) t.Crown.WidthNS) / 2.0));
      if (c.AvgCrownHeight > c.AvgTreeHeight)
        c.AvgCrownHeight = c.AvgTreeHeight;
      ForecastSimulation.getLeafData(c);
      if (!ForecastSimulation._tropicalClimate.HasValue)
      {
        double num = ForecastSimulation._taxa[c.Species].Biomass(ForecastSimulation._dbhToMetr * c.AvgDBH, ForecastSimulation._dimToMetr * c.AvgTreeHeight, ForecastSimulation._dimToMetr * c.AvgCrownHeight, c.AvgLeafBiomass / ForecastSimulation._wtOutFact);
        c.CarbonStorage = ForecastSimulation._wtOutFact * (c.CrownLightExposure < 4 || ForecastSimulation._taxa[c.Species].Root.IsPalm() ? 1.0 : 0.8) * (ForecastSimulation._taxa[c.Species].Root.IsPalm() ? 0.4 : 0.5) * num;
      }
      else
      {
        double num = ForecastSimulation._taxa[c.Species].TropicalBiomass(ForecastSimulation._tropicalClimate.Value, ForecastSimulation._dbhToMetr * c.AvgDBH, ForecastSimulation._dimToMetr * c.AvgTreeHeight);
        c.CarbonStorage = ForecastSimulation._wtOutFact * 0.5 * num;
      }
      c.Mortality = ForecastSimulation.getMortalityOf(c);
      c.CohortTag = ++ForecastSimulation._cohortTag;
      if (c.CarbonStorage > ForecastSimulation._carbonCap)
        c.CarbonStorage = ForecastSimulation._carbonCap;
      TaxonModel taxon = ForecastSimulation._taxa[c.Species];
      double num1 = ForecastSimulation._dimOutFact * taxon.Height(c.AvgDBH * ForecastSimulation._dbhToEng);
      double num2 = ForecastSimulation._dimOutFact * taxon.CrownHeight(c.AvgDBH * ForecastSimulation._dbhToEng);
      double num3 = ForecastSimulation._dimOutFact * taxon.CrownWidthBySpeciesGroup(c.AvgDBH * ForecastSimulation._dbhToEng, c.AvgTreeHeight * ForecastSimulation._dimToEng);
      ForecastSimulation._ratio.Add(c.CohortTag, new Dictionary<ForecastSimulation.Ratio, double>()
      {
        {
          ForecastSimulation.Ratio.Height,
          c.AvgTreeHeight / num1
        },
        {
          ForecastSimulation.Ratio.CrownHeight,
          c.AvgCrownHeight / num2
        },
        {
          ForecastSimulation.Ratio.CrownWidth,
          c.AvgCrownWidth / num3
        }
      });
      ForecastSimulation._initialHeight.Add(c.CohortTag, c.AvgTreeHeight);
      ForecastSimulation.Distrib key = new ForecastSimulation.Distrib()
      {
        SpCode = c.Species,
        Stratum = c.Stratum.Description
      };
      if (ForecastSimulation._distribution.ContainsKey(key))
        ForecastSimulation._distribution[key] += (double) c.NumTrees;
      else
        ForecastSimulation._distribution.Add(key, (double) c.NumTrees);
    }

    private static Cohort addIncomingCohort(
      Replanting incoming,
      ForecastSimulation.Distrib dist)
    {
      Strata strata = ForecastSimulation._input.QueryOver<Strata>().Where((System.Linq.Expressions.Expression<Func<Strata, bool>>) (u => u.Description == dist.Stratum)).JoinQueryOver<Year>((System.Linq.Expressions.Expression<Func<Strata, Year>>) (f => f.Year)).Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => y.Guid == ForecastSimulation._initYear.Guid)).SingleOrDefault();
      Cohort c = new Cohort()
      {
        Forecast = ForecastSimulation._forecast,
        Stratum = strata,
        Species = dist.SpCode,
        AvgDBH = incoming.DBH,
        Condition = ForecastSimulation._healthy,
        CrownLightExposure = 6,
        PercentCrownMissing = PctMidRange.PR0,
        NumTrees = (int) ((double) incoming.Number * ForecastSimulation._distribution[dist] + 0.5)
      };
      TaxonModel taxon = ForecastSimulation._taxa[c.Species];
      double num = c.AvgDBH * ForecastSimulation._dbhToEng;
      c.AvgTreeHeight = ForecastSimulation._dimOutFact * taxon.Height(num);
      c.AvgCrownHeight = ForecastSimulation._dimOutFact * taxon.CrownHeight(num);
      c.AvgCrownWidth = ForecastSimulation._dimOutFact * taxon.CrownWidthBySpeciesGroup(num, c.AvgTreeHeight * ForecastSimulation._dimToEng);
      if (c.AvgCrownHeight > c.AvgTreeHeight)
        c.AvgCrownHeight = c.AvgTreeHeight;
      ForecastSimulation.getLeafData(c);
      c.CarbonStorage = ForecastSimulation._tropicalClimate.HasValue ? ForecastSimulation._wtOutFact * 0.5 * taxon.TropicalBiomass(ForecastSimulation._tropicalClimate.Value, ForecastSimulation._dbhToMetr * c.AvgDBH, ForecastSimulation._dimToMetr * c.AvgTreeHeight) : ForecastSimulation._wtOutFact * (c.CrownLightExposure < 4 || taxon.Root.IsPalm() ? 1.0 : 0.8) * (taxon.Root.IsPalm() ? 0.4 : 0.5) * taxon.Biomass(c.AvgDBH * ForecastSimulation._dbhToMetr, c.AvgTreeHeight * ForecastSimulation._dimToMetr, c.AvgCrownHeight * ForecastSimulation._dimToMetr, c.AvgLeafBiomass / ForecastSimulation._wtOutFact);
      c.Mortality = ForecastSimulation.getMortalityOf(c);
      c.CohortTag = ++ForecastSimulation._cohortTag;
      if (c.CarbonStorage > ForecastSimulation._carbonCap)
        c.CarbonStorage = ForecastSimulation._carbonCap;
      ForecastSimulation._ratio.Add(c.CohortTag, new Dictionary<ForecastSimulation.Ratio, double>()
      {
        {
          ForecastSimulation.Ratio.Height,
          1.0
        },
        {
          ForecastSimulation.Ratio.CrownHeight,
          1.0
        },
        {
          ForecastSimulation.Ratio.CrownWidth,
          1.0
        }
      });
      ForecastSimulation._initialHeight.Add(c.CohortTag, c.AvgTreeHeight);
      return c;
    }

    private static double avgCleAdjustment(IList<Cohort> cohorts)
    {
      long num1 = cohorts.LongCount<Cohort>((Func<Cohort, bool>) (c => c.CrownLightExposure == 5 || c.CrownLightExposure == 4));
      long num2 = cohorts.LongCount<Cohort>((Func<Cohort, bool>) (c => c.CrownLightExposure == 3 || c.CrownLightExposure == 2 || c.CrownLightExposure < 0));
      long num3 = cohorts.LongCount<Cohort>((Func<Cohort, bool>) (c => c.CrownLightExposure == 1 || c.CrownLightExposure == 0));
      double d = ((double) num1 * ForecastSimulation.CLE_ADJUST[ForecastSimulation.Cle.Open] + (double) num2 * ForecastSimulation.CLE_ADJUST[ForecastSimulation.Cle.Park] + (double) num3 * ForecastSimulation.CLE_ADJUST[ForecastSimulation.Cle.Closed]) / (double) (num1 + num2 + num3);
      return !double.IsNaN(d) ? d : ForecastSimulation.CLE_ADJUST[ForecastSimulation.Cle.Open];
    }

    private static double avgTreeMortFactor(IList<Cohort> cohorts) => cohorts.Where<Cohort>((Func<Cohort, bool>) (c => c.Mortality.Type == "Base" && c.Mortality.Value == "00-49% Dieback")).Sum<Cohort>((Func<Cohort, double>) (c => (double) c.NumTrees * ForecastSimulation.getReductionFactor(c))) / (double) cohorts.Where<Cohort>((Func<Cohort, bool>) (c => c.Mortality.Type == "Base" && c.Mortality.Value == "00-49% Dieback")).Sum<Cohort>((Func<Cohort, int>) (c => c.NumTrees));

    private static Dictionary<string, Mortality> getBaseMortalities()
    {
      Dictionary<string, Mortality> baseMortalities = new Dictionary<string, Mortality>();
      baseMortalities.Add("00-49% Dieback", ForecastSimulation._input.QueryOver<Mortality>().Where((System.Linq.Expressions.Expression<Func<Mortality, bool>>) (m => m.Type == "Base" && m.Value == "00-49% Dieback")).Inner.JoinQueryOver<Eco.Domain.v6.Forecast>((System.Linq.Expressions.Expression<Func<Mortality, Eco.Domain.v6.Forecast>>) (m => m.Forecast)).Where((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => f.Guid == ForecastSimulation._forecast.Guid)).SingleOrDefault());
      baseMortalities.Add("50-74% Dieback", ForecastSimulation._input.QueryOver<Mortality>().Where((System.Linq.Expressions.Expression<Func<Mortality, bool>>) (m => m.Type == "Base" && m.Value == "50-74% Dieback")).Inner.JoinQueryOver<Eco.Domain.v6.Forecast>((System.Linq.Expressions.Expression<Func<Mortality, Eco.Domain.v6.Forecast>>) (m => m.Forecast)).Where((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => f.Guid == ForecastSimulation._forecast.Guid)).SingleOrDefault());
      baseMortalities.Add("75-99% Dieback", ForecastSimulation._input.QueryOver<Mortality>().Where((System.Linq.Expressions.Expression<Func<Mortality, bool>>) (m => m.Type == "Base" && m.Value == "75-99% Dieback")).Inner.JoinQueryOver<Eco.Domain.v6.Forecast>((System.Linq.Expressions.Expression<Func<Mortality, Eco.Domain.v6.Forecast>>) (m => m.Forecast)).Where((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => f.Guid == ForecastSimulation._forecast.Guid)).SingleOrDefault());
      return baseMortalities;
    }

    private static void getOverrideMortalities()
    {
      IQueryOver<Mortality, Eco.Domain.v6.Forecast> queryOver = ForecastSimulation._input.QueryOver<Mortality>().Where((System.Linq.Expressions.Expression<Func<Mortality, bool>>) (m => m.Type != "Base")).Inner.JoinQueryOver<Eco.Domain.v6.Forecast>((System.Linq.Expressions.Expression<Func<Mortality, Eco.Domain.v6.Forecast>>) (m => m.Forecast));
      System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>> expression = (System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => f.Guid == ForecastSimulation._forecast.Guid);
      foreach (Mortality mortality in (IEnumerable<Mortality>) queryOver.Where(expression).List())
      {
        if (mortality.Type == "Genus")
          ForecastSimulation._genusMorts.Add(mortality.Value, mortality);
        if (mortality.Type == "Condition")
          ForecastSimulation._condMorts.Add(mortality.Value, mortality);
        if (mortality.Type == "Stratum")
          ForecastSimulation._strataMorts.Add(mortality.Value, mortality);
      }
    }

    private static Condition getHealthyCondition()
    {
      double minDieback = ForecastSimulation._input.QueryOver<Condition>().Where((System.Linq.Expressions.Expression<Func<Condition, bool>>) (c => c.PctDieback > -1.0)).Inner.JoinQueryOver<Year>((System.Linq.Expressions.Expression<Func<Condition, Year>>) (c => c.Year)).Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => y.Guid == ForecastSimulation._initYear.Guid)).Select((IProjection) Projections.Min<Condition>((System.Linq.Expressions.Expression<Func<Condition, object>>) (c => (object) c.PctDieback))).SingleOrDefault<double>();
      return ForecastSimulation._input.QueryOver<Condition>().Where((System.Linq.Expressions.Expression<Func<Condition, bool>>) (c => c.PctDieback == minDieback)).Inner.JoinQueryOver<Year>((System.Linq.Expressions.Expression<Func<Condition, Year>>) (c => c.Year)).Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => y.Guid == ForecastSimulation._initYear.Guid)).SingleOrDefault();
    }

    private static Dictionary<int, double> getDBHValues()
    {
      IList<object[]> source = ForecastSimulation._input.QueryOver<DBH>().Where((System.Linq.Expressions.Expression<Func<DBH, bool>>) (dc => dc.Year == ForecastSimulation._initYear)).Select((IProjection) Projections.Property<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (dc => (object) dc.Id)), (IProjection) Projections.Property<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (dc => (object) dc.Value))).List<object[]>();
      return source.Count == 0 ? new Dictionary<int, double>() : source.ToDictionary<object[], int, double>((Func<object[], int>) (dc => (int) dc[0]), (Func<object[], double>) (dc => (double) dc[1]));
    }

    private static Dictionary<Guid, Pest> getEventPests()
    {
      Dictionary<Guid, Pest> eventPests = new Dictionary<Guid, Pest>();
      foreach (ForecastPestEvent pestEvent in (IEnumerable<ForecastPestEvent>) ForecastSimulation._pestEvents)
      {
        ForecastPestEvent pe = pestEvent;
        Pest pest = ForecastSimulation._ls.QueryOver<Pest>().Where((System.Linq.Expressions.Expression<Func<Pest, bool>>) (p => p.Id == pe.PestId)).Cacheable().SingleOrDefault();
        eventPests.Add(pe.Guid, pest);
      }
      return eventPests;
    }

    private static void persist(IList<Cohort> cohorts)
    {
      if (cohorts.Count == 0)
        return;
      ITransaction transaction = ForecastSimulation._input.BeginTransaction();
      try
      {
        for (int index = 0; index < cohorts.Count; ++index)
        {
          ForecastSimulation.checkCancellation();
          ForecastSimulation._input.SaveOrUpdate((object) cohorts[index]);
          if (index % 1000 == 0)
          {
            transaction.Commit();
            transaction = ForecastSimulation._input.BeginTransaction();
          }
          ForecastSimulation.advance(1, (Eco.Domain.v6.Entity) cohorts[index]);
        }
        transaction.Commit();
      }
      catch
      {
        throw;
      }
    }

    private static int numCalcSteps()
    {
      Tree tAlias = (Tree) null;
      Condition cAlias = (Condition) null;
      return 7 + 2 * ForecastSimulation._input.QueryOver<Tree>((System.Linq.Expressions.Expression<Func<Tree>>) (() => tAlias)).WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
      {
        (object) TreeStatus.InitialSample,
        (object) TreeStatus.Ingrowth,
        (object) TreeStatus.NoChange,
        (object) TreeStatus.Planted,
        (object) TreeStatus.Unknown
      }).JoinAlias((System.Linq.Expressions.Expression<Func<object>>) (() => tAlias.Crown.Condition), (System.Linq.Expressions.Expression<Func<object>>) (() => cAlias)).Where((System.Linq.Expressions.Expression<Func<bool>>) (() => cAlias == default (object) || cAlias.PctDieback < 100.0)).JoinQueryOver<Plot>((System.Linq.Expressions.Expression<Func<Tree, Plot>>) (t => t.Plot)).JoinQueryOver<Year>((System.Linq.Expressions.Expression<Func<Plot, Year>>) (p => p.Year)).Where((System.Linq.Expressions.Expression<Func<Year, bool>>) (y => y.Guid == ForecastSimulation._initYear.Guid)).RowCount() + ForecastSimulation._forecast.Replanting.Count<Replanting>();
    }

    private static int numForecastSteps()
    {
      int num1 = ForecastSimulation._input.QueryOver<Cohort>().Where((System.Linq.Expressions.Expression<Func<Cohort, bool>>) (c => c.ForecastedYear == 0)).Inner.JoinQueryOver<Eco.Domain.v6.Forecast>((System.Linq.Expressions.Expression<Func<Cohort, Eco.Domain.v6.Forecast>>) (c => c.Forecast)).Where((System.Linq.Expressions.Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => f.Guid == ForecastSimulation._forecast.Guid)).RowCount();
      Dictionary<Guid, int> dictionary1 = new Dictionary<Guid, int>();
      Dictionary<Guid, int> dictionary2 = new Dictionary<Guid, int>();
      IQueryOver<Replanting, Replanting> queryOver = ForecastSimulation._input.QueryOver<Replanting>();
      System.Linq.Expressions.Expression<Func<Replanting, bool>> expression = (System.Linq.Expressions.Expression<Func<Replanting, bool>>) (i => i.Forecast.Guid == ForecastSimulation._forecast.Guid);
      foreach (Replanting replanting in (IEnumerable<Replanting>) queryOver.Where(expression).List())
      {
        dictionary1.Add(replanting.Guid, (int) replanting.Duration);
        dictionary2.Add(replanting.Guid, (int) replanting.StartYear);
      }
      int num2 = 0;
      foreach (Guid key in ForecastSimulation._incoming.Keys)
      {
        ISet<Cohort> cohortSet = ForecastSimulation._incoming[key];
        num2 += cohortSet.Count * dictionary1[key] * dictionary1[key] / 2 + cohortSet.Count * dictionary1[key] * ((int) ForecastSimulation._forecast.NumYears - dictionary2[key] - dictionary1[key] + 1);
      }
      return 2 * ((int) ForecastSimulation._forecast.NumYears * num1 + num2);
    }

    private static void getLeafData(Cohort c)
    {
      if (c.AvgCrownHeight == 0.0 || c.AvgCrownWidth == 0.0)
      {
        c.LeafAreaIndex = 1.0;
        c.AvgLeafArea = 0.0;
        c.AvgLeafBiomass = 0.0;
      }
      else
      {
        double num1 = 0.0617 * Math.Log(c.AvgDBH * ForecastSimulation._dbhToMetr) + 0.615 + ForecastSimulation._taxa[c.Species].ShadeFactor;
        if (num1 > 0.95)
          num1 = 0.95;
        double num2 = ForecastSimulation._dimToMetr * c.AvgCrownHeight;
        double num3 = ForecastSimulation._dimToMetr * c.AvgCrownWidth;
        double num4 = num2 / num3;
        if (num4 > 2.0)
          num4 = 2.0;
        if (num4 < 0.5)
          num4 = 0.5;
        if (num2 < 1.0)
        {
          num2 = 1.0;
          num3 = num2 / num4;
        }
        else if (num2 > 12.0)
        {
          num2 = 12.0;
          num3 = num2 / num4;
        }
        else if (num3 < 1.0)
        {
          num3 = 1.0;
          num2 = num3 * num4;
        }
        else if (num3 > 14.0)
        {
          num3 = 14.0;
          num2 = num3 * num4;
        }
        double num5 = Math.Exp(0.2942 * num2 - 4.3309 + 457.0 / 625.0 * num3 + 5.7217 * num1 - 0.046495571273128943 * num3 * (num2 + num3) / 2.0 + 0.11585);
        double num6 = Math.PI * num3 * num3 / 4.0;
        double num7 = Math.PI * (ForecastSimulation._dimToMetr * c.AvgCrownWidth) * (ForecastSimulation._dimToMetr * c.AvgCrownWidth) / 4.0;
        double num8 = num6;
        double num9 = num5 / num8;
        double num10 = num9 * num7;
        double num11 = num10 / ForecastSimulation._taxa[c.Species].LeafBiomassToAreaConversionFactor * 0.001;
        double num12 = ForecastSimulation._initYear.RecordCrownCondition ? 1.0 - Math.Max(c.Condition.PctDieback, (double) c.PercentCrownMissing) / 100.0 : 1.0 - Math.Max(ForecastSimulation.defaultDieback, ForecastSimulation._initYear.RecordCrownSize ? (double) c.PercentCrownMissing : 0.0) / 100.0;
        c.LeafAreaIndex = num9 * num12;
        if (c.LeafAreaIndex < 1.0)
          c.LeafAreaIndex = 1.0;
        else if (c.LeafAreaIndex > 18.0)
          c.LeafAreaIndex = 18.0;
        c.AvgLeafArea = ForecastSimulation._areaOutFact * num10 * num12;
        c.AvgLeafBiomass = ForecastSimulation._wtOutFact * num11 * num12;
      }
    }

    private static Mortality getMortalityOf(Cohort c)
    {
      double num = ForecastSimulation.defaultDieback;
      if (ForecastSimulation._initYear.RecordCrownCondition)
        num = c.Condition.PctDieback;
      if (75.0 <= num && num <= 100.0)
        return ForecastSimulation.BASE_MORTALITIES["75-99% Dieback"];
      if (50.0 <= num && num < 75.0)
        return ForecastSimulation.BASE_MORTALITIES["50-74% Dieback"];
      Mortality mortality = (Mortality) null;
      Species genus = ForecastSimulation._taxa[c.Species].Genus;
      if (genus != null && ForecastSimulation._genusMorts.ContainsKey(genus.Code))
        mortality = ForecastSimulation._genusMorts[genus.Code];
      if (mortality == null && c.Condition != null && ForecastSimulation._condMorts.ContainsKey(c.Condition.Description))
        mortality = ForecastSimulation._condMorts[c.Condition.Description];
      if (mortality == null && ForecastSimulation._strataMorts.ContainsKey(c.Stratum.Description))
        mortality = ForecastSimulation._strataMorts[c.Stratum.Description];
      return mortality ?? ForecastSimulation.BASE_MORTALITIES["00-49% Dieback"];
    }

    private static IList<ForecastPestEvent> pestEvents() => ForecastSimulation._input.QueryOver<ForecastPestEvent>().Where((System.Linq.Expressions.Expression<Func<ForecastPestEvent, bool>>) (fe => fe.Forecast == ForecastSimulation._forecast)).List();

    private static IList<ForecastWeatherEvent> weatherEvents() => ForecastSimulation._input.QueryOver<ForecastWeatherEvent>().Where((System.Linq.Expressions.Expression<Func<ForecastWeatherEvent, bool>>) (fe => fe.Forecast == ForecastSimulation._forecast)).List();

    private static IList<Cohort> replantedTrees()
    {
      IList<Cohort> cohortList = (IList<Cohort>) new List<Cohort>();
      foreach (Guid key in ForecastSimulation._incoming.Keys)
      {
        Guid ig = key;
        Replanting replanting = ForecastSimulation._input.QueryOver<Replanting>().Where((System.Linq.Expressions.Expression<Func<Replanting, bool>>) (i => i.Guid == ig)).SingleOrDefault();
        if ((int) ForecastSimulation._currentYear >= (int) replanting.StartYear && (int) replanting.StartYear + (int) replanting.Duration > (int) ForecastSimulation._currentYear)
        {
          foreach (Cohort cohort1 in (IEnumerable<Cohort>) ForecastSimulation._incoming[replanting.Guid])
          {
            ForecastSimulation.checkCancellation();
            if (ForecastSimulation._taxa[cohort1.Species].Root.Pests.Select<Pest, int>((Func<Pest, int>) (p => p.Id)).Intersect<int>(ForecastSimulation._pestEvents.Where<ForecastPestEvent>((Func<ForecastPestEvent, bool>) (pe => !pe.PlantPestHosts)).Select<ForecastPestEvent, int>((Func<ForecastPestEvent, int>) (pe => pe.PestId))).Count<int>() <= 0)
            {
              Cohort cohort2 = cohort1.Clone() as Cohort;
              cohort2.ForecastedYear = (int) ForecastSimulation._currentYear;
              cohort2.Parent = (Cohort) null;
              cohortList.Add(cohort2);
            }
          }
        }
      }
      return cohortList;
    }

    private static int treesThatDied(Cohort c)
    {
      Species taxon = ForecastSimulation._taxa[c.Species].Root;
      double num1 = ForecastSimulation._pestEvents.Where<ForecastPestEvent>((Func<ForecastPestEvent, bool>) (pe => (int) pe.StartYear <= (int) ForecastSimulation._currentYear && (int) ForecastSimulation._currentYear < (int) pe.StartYear + (int) pe.Duration && taxon.Pests.Contains(ForecastSimulation._pests[pe.Guid]))).Sum<ForecastPestEvent>((Func<ForecastPestEvent, double>) (s => (double) c.NumTrees / ((double) s.Duration / (s.MortalityPercent / 100.0) - (double) ((int) ForecastSimulation._currentYear - (int) s.StartYear))));
      double num2 = (double) c.NumTrees * ForecastSimulation.mortalityPercent(c);
      double num3 = ForecastSimulation._weatherEvents.Where<ForecastWeatherEvent>((Func<ForecastWeatherEvent, bool>) (s => (int) s.StartYear == (int) ForecastSimulation._currentYear)).Sum<ForecastWeatherEvent>((Func<ForecastWeatherEvent, double>) (s => (double) c.NumTrees * (s.MortalityPercent / 100.0)));
      int num4 = 0;
      if (num1 + num2 + num3 < 1.0)
      {
        if (ForecastSimulation.getRandom.NextDouble() < num1 + num2 + num3)
          num4 = 1;
      }
      else
        num4 = (int) (num1 + num2 + num3 + 0.5);
      if (num4 > c.NumTrees)
        num4 = c.NumTrees;
      return num4;
    }

    private static double mortalityPercent(Cohort c)
    {
      double num = ForecastSimulation._initYear.RecordCrownCondition ? c.Condition.PctDieback : ForecastSimulation.defaultDieback;
      if (c.Mortality.Type == "Base")
      {
        if (num == 100.0)
          return 1.0;
        if (75.0 <= num && num < 100.0)
          return ForecastSimulation.BASE_MORTALITIES["75-99% Dieback"].Percent / 100.0;
        return 50.0 <= num && num < 75.0 ? ForecastSimulation.BASE_MORTALITIES["50-74% Dieback"].Percent / 100.0 : ForecastSimulation.getReductionFactor(c) / ForecastSimulation._avgTreeMortFactor * ForecastSimulation.BASE_MORTALITIES["00-49% Dieback"].Percent / 100.0;
      }
      if (!c.Mortality.IsPercentStarting)
        return c.Mortality.Percent / 100.0;
      return 100.0 - (double) ((int) ForecastSimulation._currentYear - 1) * c.Mortality.Percent > 0.0 ? c.Mortality.Percent / (100.0 - (double) ((int) ForecastSimulation._currentYear - 1) * c.Mortality.Percent) : 1.0;
    }

    private static double annualDBHGrowthInInches(Cohort c)
    {
      double num1 = c.CrownLightExposure >= 0 ? (c.CrownLightExposure >= 2 ? (c.CrownLightExposure >= 4 ? (c.CrownLightExposure >= 6 ? ForecastSimulation._avgCleAdj : ForecastSimulation.CLE_ADJUST[ForecastSimulation.Cle.Open]) : ForecastSimulation.CLE_ADJUST[ForecastSimulation.Cle.Park]) : ForecastSimulation.CLE_ADJUST[ForecastSimulation.Cle.Closed]) : ForecastSimulation.CLE_ADJUST[ForecastSimulation.Cle.Park];
      double num2 = ForecastSimulation._dbhToEng * c.AvgDBH / ForecastSimulation._taxa[c.Species].MaximumDBH;
      double num3;
      if (num2 <= 0.8)
        num3 = 1.0;
      else if (0.8 < num2 && num2 < 1.25)
      {
        num3 = 0.5555555 + 2.222222 * (1.0 - num2);
        if (num3 < 0.0222)
          num3 = 0.0222;
      }
      else
        num3 = 0.0222;
      double growthRate = ForecastSimulation._taxa[c.Species].GrowthRate;
      double num4 = ForecastSimulation._initYear.RecordCrownCondition ? c.Condition.PctDieback : ForecastSimulation.defaultDieback;
      double num5 = (double) ForecastSimulation._forecast.FrostFreeDays / 153.0;
      return growthRate * num5 / num1 * (1.0 - num4 / 100.0) * num3;
    }

    private static double getReductionFactor(Cohort c)
    {
      double matureHeight = ForecastSimulation._taxa[c.Species].MatureHeight;
      double[] numArray = matureHeight >= ForecastSimulation.MATURE_HT[ForecastSimulation.Scale.Medium] ? (matureHeight < ForecastSimulation.MATURE_HT[ForecastSimulation.Scale.Large] ? ForecastSimulation.MORT_DBHS[ForecastSimulation.Scale.Medium] : ForecastSimulation.MORT_DBHS[ForecastSimulation.Scale.Large]) : ForecastSimulation.MORT_DBHS[ForecastSimulation.Scale.Small];
      double num = c.AvgDBH * ForecastSimulation._dbhToEng;
      for (short index = 0; index < (short) 6; ++index)
      {
        if (numArray[(int) index] <= num && num < numArray[(int) index + 1])
          return ForecastSimulation.MORT_RED_FACT[(int) index];
      }
      return ForecastSimulation.MORT_RED_FACT[6];
    }

    private static void advance(int steps, Eco.Domain.v6.Entity obj)
    {
      ForecastEventArgs forecastEventArgs = new ForecastEventArgs()
      {
        Forecast = ForecastSimulation._forecast,
        InitialYear = ForecastSimulation._initYear,
        Progress = steps,
        EndProgress = ForecastSimulation._steps,
        AffectedObject = obj,
        Stage = ForecastSimulation._currentStage,
        CurrentYear = ForecastSimulation._currentYear
      };
      ForecastEventHandler progressAdvanced = ForecastSimulation.ProgressAdvanced;
      if (progressAdvanced == null)
        return;
      foreach (Delegate invocation in progressAdvanced.GetInvocationList())
      {
        if (invocation.Target is Control target && !target.IsDisposed && !target.Disposing)
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

    private static void calculateAdjustmentToSASresults(
      EstimateUtil estUtil,
      InputSession _iSess,
      ISession sess,
      IList<Cohort> cohorts,
      Dictionary<string, double> InitialCohortTreeCoverByStrata,
      Dictionary<string, double> TreeCoverAdjustAmountByStrata,
      Dictionary<string, double> InitialCohortLeafAreaByStrata,
      Dictionary<string, double> LeafAreaAdjustAmountByStrata,
      Dictionary<string, double> InitialCohortLeafBiomassByStrata,
      Dictionary<string, double> LeafBiomassAdjustAmountByStrata,
      Dictionary<string, double> InitialCohortCarbonStorageByStrata,
      Dictionary<string, double> CarbonStorageAdjustAmountByStrata)
    {
      Dictionary<string, double> dictionary1 = new Dictionary<string, double>();
      Dictionary<string, double> dictionary2 = new Dictionary<string, double>();
      Dictionary<string, double> dictionary3 = new Dictionary<string, double>();
      Dictionary<string, double> dictionary4 = new Dictionary<string, double>();
      foreach (Cohort cohort in (IEnumerable<Cohort>) cohorts)
      {
        if (cohort.ForecastedYear == 0)
        {
          Dictionary<string, double> dictionary5 = InitialCohortTreeCoverByStrata;
          Guid guid = cohort.Stratum.Guid;
          string key1 = guid.ToString();
          if (dictionary5.ContainsKey(key1))
          {
            Dictionary<string, double> dictionary6 = InitialCohortLeafAreaByStrata;
            guid = cohort.Stratum.Guid;
            string key2 = guid.ToString();
            dictionary6[key2] += (double) cohort.NumTrees * cohort.AvgLeafArea;
            Dictionary<string, double> dictionary7 = InitialCohortLeafBiomassByStrata;
            guid = cohort.Stratum.Guid;
            string key3 = guid.ToString();
            dictionary7[key3] += (double) cohort.NumTrees * cohort.AvgLeafBiomass;
            Dictionary<string, double> dictionary8 = InitialCohortCarbonStorageByStrata;
            guid = cohort.Stratum.Guid;
            string key4 = guid.ToString();
            dictionary8[key4] += (double) cohort.NumTrees * cohort.CarbonStorage;
            Dictionary<string, double> dictionary9 = InitialCohortTreeCoverByStrata;
            guid = cohort.Stratum.Guid;
            string key5 = guid.ToString();
            dictionary9[key5] += 0.7853981625 * (double) cohort.NumTrees * cohort.AvgCrownWidth * cohort.AvgCrownWidth;
          }
          else
          {
            Dictionary<string, double> dictionary10 = InitialCohortLeafAreaByStrata;
            guid = cohort.Stratum.Guid;
            string key6 = guid.ToString();
            double num1 = (double) cohort.NumTrees * cohort.AvgLeafArea;
            dictionary10.Add(key6, num1);
            Dictionary<string, double> dictionary11 = InitialCohortLeafBiomassByStrata;
            guid = cohort.Stratum.Guid;
            string key7 = guid.ToString();
            double num2 = (double) cohort.NumTrees * cohort.AvgLeafBiomass;
            dictionary11.Add(key7, num2);
            Dictionary<string, double> dictionary12 = InitialCohortCarbonStorageByStrata;
            guid = cohort.Stratum.Guid;
            string key8 = guid.ToString();
            double num3 = (double) cohort.NumTrees * cohort.CarbonStorage;
            dictionary12.Add(key8, num3);
            Dictionary<string, double> dictionary13 = InitialCohortTreeCoverByStrata;
            guid = cohort.Stratum.Guid;
            string key9 = guid.ToString();
            double num4 = 0.7853981625 * (double) cohort.NumTrees * cohort.AvgCrownWidth * cohort.AvgCrownWidth;
            dictionary13.Add(key9, num4);
          }
        }
      }
      string estTable1 = estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata
      })];
      int valueOrderFromName1 = (int) estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area");
      double num5 = 1.0;
      double num6 = 1.0;
      if (ForecastSimulation._initYear.Unit == YearUnit.English)
      {
        num5 = 10.7639;
        num6 = 2.20462;
      }
      IQuerySupplier querySupplier = _iSess.GetQuerySupplier(sess);
      foreach (DataRow row in (InternalDataCollectionBase) ForecastSimulation.GetSASOutput(querySupplier, estTable1, valueOrderFromName1, 5, estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)], _iSess.YearKey).Rows)
        dictionary2.Add(row.Field<Guid>("StrataKey").ToString(), row.Field<double>("EstimateValue") * 1000000.0);
      foreach (string key in InitialCohortLeafAreaByStrata.Keys)
      {
        double num7 = dictionary2.ContainsKey(key) ? dictionary2[key] * num5 - InitialCohortLeafAreaByStrata[key] : -InitialCohortLeafAreaByStrata[key];
        LeafAreaAdjustAmountByStrata.Add(key, num7);
      }
      foreach (DataRow row in (InternalDataCollectionBase) ForecastSimulation.GetSASOutput(querySupplier, estTable1, valueOrderFromName1, 6, estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)], _iSess.YearKey).Rows)
        dictionary3.Add(row.Field<Guid>("StrataKey").ToString(), row.Field<double>("EstimateValue") * 1000.0);
      foreach (string key in InitialCohortLeafBiomassByStrata.Keys)
      {
        double num8 = dictionary3.ContainsKey(key) ? dictionary3[key] * num6 - InitialCohortLeafBiomassByStrata[key] : -InitialCohortLeafBiomassByStrata[key];
        LeafBiomassAdjustAmountByStrata.Add(key, num8);
      }
      foreach (DataRow row in (InternalDataCollectionBase) ForecastSimulation.GetSASOutput(querySupplier, estTable1, valueOrderFromName1, 2, estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)], _iSess.YearKey).Rows)
        dictionary4.Add(row.Field<Guid>("StrataKey").ToString(), row.Field<double>("EstimateValue") * 1000.0);
      foreach (string key in InitialCohortCarbonStorageByStrata.Keys)
      {
        double num9 = dictionary4.ContainsKey(key) ? dictionary4[key] * num6 - InitialCohortCarbonStorageByStrata[key] : -CarbonStorageAdjustAmountByStrata[key];
        CarbonStorageAdjustAmountByStrata.Add(key, num9);
      }
      Year year = _iSess.CreateSession().Get<Year>((object) _iSess.YearKey);
      if (year.RecordStrata)
      {
        List<Classifiers> classifiersList = new List<Classifiers>()
        {
          Classifiers.Strata,
          Classifiers.GroundCover
        };
        string estTable2 = estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Cover, classifiersList)];
        Classifiers.GroundCover.ToString();
        int valueOrderFromName2 = (int) estUtil.GetClassValueOrderFromName(Classifiers.GroundCover, "TREE");
        foreach (DataRow row in (InternalDataCollectionBase) ForecastSimulation.GetGroundCoverByStratumSASOutput(querySupplier, estTable2, valueOrderFromName1, valueOrderFromName2, 12, estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)], _iSess.YearKey).Rows)
        {
          double num10 = 0.01 * row.Field<double>("EstimateValue") * row.Field<double>("Area") * (year.Unit == YearUnit.Metric ? 10000.0 : 4046.86);
          dictionary1.Add(row.Field<Guid>("StrataKey").ToString(), num10);
        }
      }
      else
      {
        foreach (DataRow row in (InternalDataCollectionBase) sess.GetNamedQuery("GetSASEstimateValuesFromIndividualTreeEffects").SetParameter<Guid?>("year", _iSess.YearKey).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>().Rows)
          dictionary1.Add(row.Field<Guid>("StrataKey").ToString(), row.Field<double>("SumOfGroundArea"));
      }
      foreach (string key in InitialCohortTreeCoverByStrata.Keys)
      {
        double num11 = dictionary1.ContainsKey(key) ? dictionary1[key] * num5 - InitialCohortTreeCoverByStrata[key] : -InitialCohortTreeCoverByStrata[key];
        TreeCoverAdjustAmountByStrata.Add(key, num11);
      }
    }

    private static DataTable GetGroundCoverByStratumSASOutput(
      IQuerySupplier queryProvider,
      string aTable,
      int StudyAreaClassValue,
      int treeClassValue,
      int estimatetype,
      int unitId,
      Guid? yearKey)
    {
      IEstimateUtilProvider estimateUtilProvider = queryProvider.GetEstimateUtilProvider();
      string tableName = aTable;
      Classifiers classifiers = Classifiers.Strata;
      string c1 = classifiers.ToString();
      classifiers = Classifiers.GroundCover;
      string c2 = classifiers.ToString();
      return estimateUtilProvider.GetSASEstimateValues(tableName, c1, c2).SetParameter<Classifiers>("strataclassifier", Classifiers.Strata).SetParameter<int>("studyareaclassvalue", StudyAreaClassValue).SetParameter<int>(nameof (estimatetype), estimatetype).SetParameter<EquationTypes>("equationtype", EquationTypes.None).SetParameter<int>("unitid", unitId).SetParameter<Guid?>("y", yearKey).SetParameter<int>("treeclassvalue", treeClassValue).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
    }

    private static DataTable GetSASOutput(
      IQuerySupplier queryProvider,
      string aTable,
      int StudyAreaClassValue,
      int estimatetype,
      int unitId,
      Guid? yearKey)
    {
      return queryProvider.GetEstimateUtilProvider().GetSASEstimateValues(aTable, Classifiers.Strata.ToString()).SetParameter<Classifiers>("strataclassifier", Classifiers.Strata).SetParameter<int>("studyareaclassvalue", StudyAreaClassValue).SetParameter<int>(nameof (estimatetype), estimatetype).SetParameter<EquationTypes>("equationtype", EquationTypes.None).SetParameter<int>("unitid", unitId).SetParameter<Guid?>("y", yearKey).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
    }

    private enum Scale
    {
      Small,
      Medium,
      Large,
    }

    private enum Cle
    {
      Open,
      Park,
      Closed,
    }

    private struct Distrib
    {
      public string SpCode;
      public string Stratum;
    }

    private enum Ratio
    {
      Height,
      CrownHeight,
      CrownWidth,
    }

    private class CohortStatus
    {
      public int originalNumberOfTrees;
      public int currentNumberOfTrees;
      public string strataGuidStr = "";
      public double percentOfTotalTreeCover;
      public double percentOfTotalLeafArea;
      public double percentOfTotalLeafBiomass;
      public double percentOfTotalCarbonStorage;
    }
  }
}
