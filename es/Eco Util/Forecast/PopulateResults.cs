// Decompiled with JetBrains decompiler
// Type: Eco.Util.Forecast.PopulateResults
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using Eco.Util.Queries.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Forms;

namespace Eco.Util.Forecast
{
  [DesignerCategory("Code")]
  public class PopulateResults
  {
    private static CancellationToken _cToken;
    private static InputSession _iSess;
    private static ISession _input;
    private static List<string> listQueryNamesOverall = new List<string>();
    private static List<CohortResultDataType> listResultDataTypesOverall = new List<CohortResultDataType>();
    private static List<string> listQueryNamesByStrata = new List<string>();
    private static List<CohortResultDataType> listResultDataTypesByStrata = new List<CohortResultDataType>();
    private static Dictionary<string, Strata> strataNameToStrata = new Dictionary<string, Strata>();
    private static int _steps;
    private static Year _yearObj;
    private static Eco.Domain.v6.Forecast _forecast;

    public static event ForecastEventHandler ProgressAdvanced;

    public static void begin(
      InputSession actualPs,
      InputSession iS,
      ISession input,
      CancellationToken ct)
    {
      PopulateResults.initialize(iS, input, ct);
      for (int index = 0; index < PopulateResults.listQueryNamesOverall.Count; ++index)
      {
        PopulateResults.checkCancellation();
        string queryName = PopulateResults.listQueryNamesOverall.ElementAt<string>(index);
        CohortResultDataType theDataType = PopulateResults.listResultDataTypesOverall.ElementAt<CohortResultDataType>(index);
        using (ITransaction transaction = input.BeginTransaction())
        {
          try
          {
            foreach (PopulateResults.Result2 result2 in (IEnumerable<PopulateResults.Result2>) input.GetNamedQuery(queryName).SetGuid("guid", iS.ForecastKey.Value).List<object[]>().Select<object[], PopulateResults.Result2>((Func<object[], PopulateResults.Result2>) (pair => new PopulateResults.Result2()
            {
              Stratum = (Strata) null,
              Year = Convert.ToInt32(pair[0]),
              DataType = theDataType,
              DBHRangeStart = 0.0,
              DBHRangeEnd = 0.0,
              DataValue = Convert.ToDouble(pair[1])
            })).ToList<PopulateResults.Result2>())
            {
              input.Save((object) new CohortResult()
              {
                Forecast = PopulateResults._forecast,
                Stratum = result2.Stratum,
                ForecastedYear = result2.Year,
                DataType = result2.DataType,
                DBHRangeStart = result2.DBHRangeStart,
                DBHRangeEnd = result2.DBHRangeEnd,
                DataValue = result2.DataValue
              });
              PopulateResults.advance(Stage.PolulatingResults, 1, (Entity) null);
            }
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }
      for (int index = 0; index < PopulateResults.listQueryNamesByStrata.Count; ++index)
      {
        PopulateResults.checkCancellation();
        string queryName = PopulateResults.listQueryNamesByStrata.ElementAt<string>(index);
        CohortResultDataType theDataType = PopulateResults.listResultDataTypesByStrata.ElementAt<CohortResultDataType>(index);
        using (ITransaction transaction = input.BeginTransaction())
        {
          try
          {
            foreach (PopulateResults.Result2 result2 in (IEnumerable<PopulateResults.Result2>) input.GetNamedQuery(queryName).SetGuid("guid", iS.ForecastKey.Value).List<object[]>().Select<object[], PopulateResults.Result2>((Func<object[], PopulateResults.Result2>) (pair => new PopulateResults.Result2()
            {
              Stratum = PopulateResults.strataNameToStrata[pair[0].ToString()],
              Year = Convert.ToInt32(pair[1]),
              DataType = theDataType,
              DBHRangeStart = 0.0,
              DBHRangeEnd = 0.0,
              DataValue = Convert.ToDouble(pair[2])
            })).ToList<PopulateResults.Result2>())
            {
              input.Save((object) new CohortResult()
              {
                Forecast = PopulateResults._forecast,
                Stratum = result2.Stratum,
                ForecastedYear = result2.Year,
                DataType = result2.DataType,
                DBHRangeStart = result2.DBHRangeStart,
                DBHRangeEnd = result2.DBHRangeEnd,
                DataValue = result2.DataValue
              });
              PopulateResults.advance(Stage.PolulatingResults, 1, (Entity) null);
            }
            transaction.Commit();
          }
          catch (Exception ex)
          {
            transaction.Rollback();
            throw;
          }
        }
      }
      input.GetNamedQuery("ForecastAdjustLeafAreaTreeCoverLeafBiomassCarbonStorage").SetGuid("ForecastKey", PopulateResults._forecast.Guid).SetInt32("TypeOfLeafArea", 6).SetInt32("TypeOfTreeCover", 2).SetInt32("TypeOfLeafBiomass", 8).SetInt32("TypeOfCarbonStorage", 10).ExecuteUpdate();
      input.GetNamedQuery("ForecastAdjustLeafAreaTreeCoverLeafBiomassCarbonStorageByStrata").SetGuid("ForecastKey", PopulateResults._forecast.Guid).SetInt32("TypeOfLeafArea", 6).SetInt32("TypeOfTreeCover", 2).SetInt32("TypeOfLeafBiomass", 8).SetInt32("TypeOfCarbonStorage", 10).ExecuteUpdate();
      input.GetNamedQuery("ForecastRecalculateLAI").SetGuid("ForecastKey", PopulateResults._forecast.Guid).SetInt32("TypeOfLAI", 7).SetInt32("TypeOfLeafArea", 6).SetInt32("TypeOfTreeCover", 2).ExecuteUpdate();
      input.GetNamedQuery("ForecastRecalculateLAIByStrata").SetGuid("ForecastKey", PopulateResults._forecast.Guid).SetInt32("TypeOfLAI", 7).SetInt32("TypeOfLeafArea", 6).SetInt32("TypeOfTreeCover", 2).ExecuteUpdate();
      PopulateResults.checkCancellation();
      input.GetNamedQuery("ForecastCreateEcoDBHClasses").ExecuteUpdate();
      IQuery query1 = input.GetNamedQuery("ForecastInsertEcoDBHClasses").SetGuid("YearKey", iS.YearKey.Value);
      using (ITransaction transaction = input.BeginTransaction())
      {
        try
        {
          for (int index = 0; index <= 32; ++index)
          {
            query1.SetInt32("DBHClassId", index + 1);
            if (PopulateResults._yearObj.Unit == YearUnit.English)
            {
              query1.SetDouble("RangeStart", (double) (index * 3));
              if (index == 32)
                query1.SetDouble("RangeEnd", 1000.0);
              else
                query1.SetDouble("RangeEnd", (double) ((index + 1) * 3));
            }
            else
            {
              query1.SetDouble("RangeStart", Math.Round((double) (index * 3) * 2.54, 1));
              if (index == 32)
                query1.SetDouble("RangeEnd", Math.Round(2540.0, 1));
              else
                query1.SetDouble("RangeEnd", Math.Round((double) ((index + 1) * 3) * 2.54, 1));
            }
            query1.ExecuteUpdate();
          }
          transaction.Commit();
        }
        catch (Exception ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      PopulateResults.checkCancellation();
      using (ITransaction transaction = input.BeginTransaction())
      {
        try
        {
          string queryName = "TreeDBHDistribution";
          CohortResultDataType theDataType = CohortResultDataType.DBHDistribution;
          foreach (PopulateResults.Result2 result2 in (IEnumerable<PopulateResults.Result2>) input.GetNamedQuery(queryName).SetGuid("fguid", iS.ForecastKey.Value).SetGuid("yguid", iS.YearKey.Value).List<object[]>().Select<object[], PopulateResults.Result2>((Func<object[], PopulateResults.Result2>) (pair => new PopulateResults.Result2()
          {
            Stratum = (Strata) null,
            Year = Convert.ToInt32(pair[0]),
            DataType = theDataType,
            DBHRangeStart = Convert.ToDouble(pair[1]),
            DBHRangeEnd = Convert.ToDouble(pair[2]),
            DataValue = Convert.ToDouble(pair[3])
          })).ToList<PopulateResults.Result2>())
          {
            input.Save((object) new CohortResult()
            {
              Forecast = PopulateResults._forecast,
              Stratum = result2.Stratum,
              ForecastedYear = result2.Year,
              DataType = result2.DataType,
              DBHRangeStart = result2.DBHRangeStart,
              DBHRangeEnd = result2.DBHRangeEnd,
              DataValue = result2.DataValue
            });
            PopulateResults.advance(Stage.PolulatingResults, 1, (Entity) null);
          }
          transaction.Commit();
        }
        catch (Exception ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      PopulateResults.checkCancellation();
      using (ITransaction transaction = input.BeginTransaction())
      {
        try
        {
          string queryName = "TreeCDBHDistribution";
          CohortResultDataType theDataType = CohortResultDataType.CDBHDistribution;
          foreach (PopulateResults.Result2 result2 in (IEnumerable<PopulateResults.Result2>) input.GetNamedQuery(queryName).SetGuid("fguid", iS.ForecastKey.Value).SetGuid("yguid", iS.YearKey.Value).List<object[]>().Select<object[], PopulateResults.Result2>((Func<object[], PopulateResults.Result2>) (pair => new PopulateResults.Result2()
          {
            Stratum = (Strata) null,
            Year = Convert.ToInt32(pair[0]),
            DataType = theDataType,
            DBHRangeStart = Convert.ToDouble(pair[1]),
            DBHRangeEnd = Convert.ToDouble(pair[2]),
            DataValue = Convert.ToDouble(pair[3])
          })).ToList<PopulateResults.Result2>())
          {
            input.Save((object) new CohortResult()
            {
              Forecast = PopulateResults._forecast,
              Stratum = result2.Stratum,
              ForecastedYear = result2.Year,
              DataType = result2.DataType,
              DBHRangeStart = result2.DBHRangeStart,
              DBHRangeEnd = result2.DBHRangeEnd,
              DataValue = result2.DataValue
            });
            PopulateResults.advance(Stage.PolulatingResults, 1, (Entity) null);
          }
          transaction.Commit();
        }
        catch (Exception ex)
        {
          transaction.Rollback();
          throw;
        }
      }
      using (ISession session = actualPs.CreateSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          try
          {
            session.GetNamedQuery("DeleteCohorts").SetGuid("guid", PopulateResults._iSess.ForecastKey.Value).ExecuteUpdate();
            session.GetNamedQuery("DeletePollutantResults").SetGuid("guid", PopulateResults._iSess.ForecastKey.Value).ExecuteUpdate();
            session.GetNamedQuery("DeleteCohortResults").SetGuid("guid", PopulateResults._iSess.ForecastKey.Value).ExecuteUpdate();
            transaction.Commit();
          }
          catch
          {
            transaction.Rollback();
            throw;
          }
        }
        PopulateResults.advance(Stage.PolulatingResults, 1, (Entity) null);
        IEstimateUtilProvider estimateUtilProvider = actualPs.GetQuerySupplier(session).GetEstimateUtilProvider();
        IQuery query2 = estimateUtilProvider.MovingEcoForecastCohortResults(PopulateResults._iSess.InputDb);
        query2.SetGuid("ForecastKey", PopulateResults._iSess.ForecastKey.Value);
        query2.ExecuteUpdate();
        PopulateResults.advance(Stage.PolulatingResults, 1, (Entity) null);
        IList<int> intList = input.GetNamedQuery("GetForecastYearsWithNonZeroDataDesc").SetGuid("ForecastKey", PopulateResults._iSess.ForecastKey.Value).List<int>();
        int val = -1;
        if (intList.Count > 0)
          val = intList[0];
        estimateUtilProvider.MovingEcoForecastPollutantResults(PopulateResults._iSess.InputDb).SetGuid("ForecastKey", PopulateResults._iSess.ForecastKey.Value).SetInt32("ForecastedYear", val).ExecuteUpdate();
      }
      PopulateResults.advance(Stage.Succeeded, 0, (Entity) null);
    }

    private static void initialize(InputSession i, ISession input, CancellationToken ct)
    {
      PopulateResults._cToken = ct;
      PopulateResults._iSess = i;
      PopulateResults._input = input;
      PopulateResults._yearObj = PopulateResults._input.QueryOver<Year>().Where((Expression<Func<Year, bool>>) (y => (Guid?) y.Guid == PopulateResults._iSess.YearKey)).SingleOrDefault();
      PopulateResults._forecast = PopulateResults._input.QueryOver<Eco.Domain.v6.Forecast>().Where((Expression<Func<Eco.Domain.v6.Forecast, bool>>) (f => (Guid?) f.Guid == PopulateResults._iSess.ForecastKey)).SingleOrDefault();
      PopulateResults.listQueryNamesOverall.Clear();
      PopulateResults.listResultDataTypesOverall.Clear();
      PopulateResults.listQueryNamesOverall.Add("NumTrees");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.TreeNumber);
      PopulateResults.listQueryNamesOverall.Add("TreeCoverArea");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.TreeCover);
      PopulateResults.listQueryNamesOverall.Add("DBHGrowth");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.DBHGrowth);
      PopulateResults.listQueryNamesOverall.Add("LeafArea");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.LeafArea);
      PopulateResults.listQueryNamesOverall.Add("LeafAreaIndex");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.LeafAreaIndex);
      PopulateResults.listQueryNamesOverall.Add("LeafBiomass");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.LeafBiomass);
      PopulateResults.listQueryNamesOverall.Add("BasalArea");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.BasalArea);
      PopulateResults.listQueryNamesOverall.Add("CarbonStorage");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.CarbonStorage);
      PopulateResults.listQueryNamesOverall.Add("CarbonSequestration");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.CarbonSequestration);
      PopulateResults.listQueryNamesOverall.Add("TreeBiomass");
      PopulateResults.listResultDataTypesOverall.Add(CohortResultDataType.TreeBiomass);
      PopulateResults.listQueryNamesByStrata.Clear();
      PopulateResults.listResultDataTypesByStrata.Clear();
      PopulateResults.listQueryNamesByStrata.Add("NumTreesByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.TreeNumber);
      PopulateResults.listQueryNamesByStrata.Add("TreeCoverAreaByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.TreeCover);
      PopulateResults.listQueryNamesByStrata.Add("DBHGrowthByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.DBHGrowth);
      PopulateResults.listQueryNamesByStrata.Add("LeafAreaByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.LeafArea);
      PopulateResults.listQueryNamesByStrata.Add("LeafAreaIndexByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.LeafAreaIndex);
      PopulateResults.listQueryNamesByStrata.Add("LeafBiomassByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.LeafBiomass);
      PopulateResults.listQueryNamesByStrata.Add("BasalAreaByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.BasalArea);
      PopulateResults.listQueryNamesByStrata.Add("CarbonStorageByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.CarbonStorage);
      PopulateResults.listQueryNamesByStrata.Add("CarbonSequestrationByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.CarbonSequestration);
      PopulateResults.listQueryNamesByStrata.Add("TreeBiomassByStrata");
      PopulateResults.listResultDataTypesByStrata.Add(CohortResultDataType.TreeBiomass);
      IList<Strata> strataList = PopulateResults._input.QueryOver<Strata>().Where((Expression<Func<Strata, bool>>) (s => s.Year.Guid == PopulateResults._iSess.YearKey.Value)).List();
      PopulateResults.strataNameToStrata.Clear();
      foreach (Strata strata in (IEnumerable<Strata>) strataList)
        PopulateResults.strataNameToStrata.Add(strata.Description, strata);
      PopulateResults._steps = PopulateResults.numCalcSteps();
      PopulateResults.advance(Stage.PolulatingResults, 1, (Entity) null);
    }

    private static int numCalcSteps() => 0 + 1 + ((int) PopulateResults._forecast.NumYears + 1) * PopulateResults.listQueryNamesOverall.Count + ((int) PopulateResults._forecast.NumYears + 1) * PopulateResults.listQueryNamesOverall.Count * PopulateResults.strataNameToStrata.Count + ((int) PopulateResults._forecast.NumYears + 1) * 32 + ((int) PopulateResults._forecast.NumYears + 1) * PopulateResults._input.QueryOver<DBHRptClass>().Where((Expression<Func<DBHRptClass, bool>>) (y => y.Year.Guid == PopulateResults._yearObj.Guid)).RowCount() + 2 + 2;

    private static void checkCancellation()
    {
      if (!PopulateResults._cToken.IsCancellationRequested)
        return;
      PopulateResults._cToken.ThrowIfCancellationRequested();
    }

    private static void advance(Stage stage, int steps, Entity obj)
    {
      ForecastEventArgs forecastEventArgs = new ForecastEventArgs()
      {
        Forecast = PopulateResults._forecast,
        InitialYear = PopulateResults._yearObj,
        Progress = steps,
        EndProgress = PopulateResults._steps,
        AffectedObject = obj,
        Stage = stage,
        CurrentYear = 0
      };
      ForecastEventHandler progressAdvanced = PopulateResults.ProgressAdvanced;
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

    private struct Result2
    {
      public int Year;
      public Strata Stratum;
      public CohortResultDataType DataType;
      public double DBHRangeStart;
      public double DBHRangeEnd;
      public double DataValue;
    }
  }
}
