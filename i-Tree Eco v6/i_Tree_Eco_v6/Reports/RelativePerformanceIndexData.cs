// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.RelativePerformanceIndexData
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.NHibernate.Extensions;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Data;

namespace i_Tree_Eco_v6.Reports
{
  public class RelativePerformanceIndexData : DatabaseReport
  {
    private IList<Condition> m_conditions;
    private IList<Strata> m_strata;
    private IList<Tree> m_trees;
    private IList<Plot> m_plots;
    private IDictionary<SpeciesView, IDictionary<HealthRptClass, TreeStats>> m_dSpCmnStats;
    private IDictionary<SpeciesView, IDictionary<HealthRptClass, TreeStats>> m_dSpSciStats;
    private IDictionary<HealthRptClass, TreeStats> m_dCwStats;
    private TreeStats m_cwTotal;
    private IDictionary<SpeciesView, TreeStats> m_dSpStats;
    private IDictionary<SpeciesView, double> m_dSpRPI;
    protected List<HealthRptClass> healthClasses;
    protected string speciesColumn = "species";
    protected string relativePerformanceIndexColumn = "relativePerformanceIndex";
    protected string treeCountColumn = "treeCount";
    protected string standardErrorColumn = "standardError";
    protected string percentOfTreesColumn = "percentOfTrees";

    protected void Init()
    {
      using (this.curInputISession.BeginTransaction())
      {
        this.m_conditions = this.curInputISession.CreateCriteria<Condition>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).List<Condition>();
        this.m_strata = this.curInputISession.CreateCriteria<Strata>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).List<Strata>();
        this.m_plots = this.curInputISession.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).Add((ICriterion) Restrictions.Eq("IsComplete", (object) true)).List<Plot>();
        this.m_trees = (IList<Tree>) this.curInputISession.CreateCriteria<Tree>().CreateAlias("Plot", "p").Add((ICriterion) Restrictions.Eq("p.Year", (object) this.curYear)).Add((ICriterion) Restrictions.Eq("p.IsComplete", (object) true)).Add((ICriterion) Restrictions.On<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
        {
          (object) TreeStatus.InitialSample,
          (object) TreeStatus.Ingrowth,
          (object) TreeStatus.NoChange,
          (object) TreeStatus.Planted,
          (object) TreeStatus.Unknown
        })).PagedList<Tree>(1000);
      }
      this.healthClasses = new List<HealthRptClass>((IEnumerable<HealthRptClass>) this.curYear.HealthRptClasses);
    }

    protected DataTable GetStats()
    {
      this.Init();
      if (this.m_dSpCmnStats == null || this.m_dSpSciStats == null)
        this.CalculateStats();
      DataTable dataTable = this.CreateDataTable();
      this.AssignDataRows(dataTable);
      this.AssignTotals(dataTable);
      return dataTable;
    }

    private DataTable CreateDataTable()
    {
      DataTable dataTable = new DataTable();
      dataTable.Columns.Add(this.speciesColumn, typeof (string));
      foreach (HealthRptClass healthClass in this.healthClasses)
        dataTable.Columns.Add(healthClass.Id.ToString(), typeof (double));
      dataTable.Columns.Add(this.relativePerformanceIndexColumn, typeof (double));
      dataTable.Columns.Add(this.treeCountColumn, typeof (int));
      dataTable.Columns.Add(this.standardErrorColumn, typeof (double));
      dataTable.Columns.Add(this.percentOfTreesColumn, typeof (double));
      return dataTable;
    }

    private void CalculateStats()
    {
      this.m_dSpCmnStats = (IDictionary<SpeciesView, IDictionary<HealthRptClass, TreeStats>>) new SortedDictionary<SpeciesView, IDictionary<HealthRptClass, TreeStats>>((IComparer<SpeciesView>) new PropertyComparer<SpeciesView>((Func<SpeciesView, object>) (sv => (object) sv.CommonName)));
      this.m_dSpSciStats = (IDictionary<SpeciesView, IDictionary<HealthRptClass, TreeStats>>) new SortedDictionary<SpeciesView, IDictionary<HealthRptClass, TreeStats>>((IComparer<SpeciesView>) new PropertyComparer<SpeciesView>((Func<SpeciesView, object>) (sv => (object) sv.ScientificName)));
      this.m_dSpStats = (IDictionary<SpeciesView, TreeStats>) new Dictionary<SpeciesView, TreeStats>();
      this.m_dCwStats = (IDictionary<HealthRptClass, TreeStats>) new Dictionary<HealthRptClass, TreeStats>();
      this.m_dSpRPI = (IDictionary<SpeciesView, double>) new Dictionary<SpeciesView, double>();
      this.m_cwTotal = new TreeStats();
      IDictionary<SpeciesView, IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>> dSpCounts = (IDictionary<SpeciesView, IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>) new Dictionary<SpeciesView, IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>();
      IDictionary<SpeciesView, IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>> dSpCond = (IDictionary<SpeciesView, IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>>) new Dictionary<SpeciesView, IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>>();
      IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>> dSpTotals = (IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>) new Dictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>();
      IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>> dictionary = (IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>) new Dictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>();
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      this.getCwTotals(dCwTotals);
      foreach (HealthRptClass healthRptClass in (IEnumerable<HealthRptClass>) this.curYear.HealthRptClasses)
        dictionary[healthRptClass] = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      foreach (Tree tree in (IEnumerable<Tree>) this.m_trees)
      {
        Condition treeCondition = this.getTreeCondition(tree);
        SpeciesView species = this.getSpecies(tree);
        if (species != null)
          this.InitializeStrataCounts(species, treeCondition, tree, dSpCond, dSpCounts, dSpTotals, dictionary, dCwTotals);
      }
      IDictionary<Strata, double> dStrataArea = this.getdStrataArea(dCwTotals);
      if (this.series.IsSample)
        this.m_cwTotal.StdErr = Math.Sqrt(this.m_cwTotal.StdErr);
      this.m_cwTotal.Pct = 100.0;
      this.m_cwTotal.PctTrees = 100.0;
      IDictionary<SpeciesView, double> cwSpTotal = this.getCwSpTotal(dSpTotals, dStrataArea, dCwTotals);
      this.InitializeSpeciesStats(dSpCounts, cwSpTotal, dStrataArea, dCwTotals);
      this.CalculateRelativePerformanceIndex(dSpCond, dStrataArea, dCwTotals);
      this.CalculateSEandPercentOfTrees(dictionary, dStrataArea, dCwTotals);
    }

    private void getCwTotals(
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals)
    {
      foreach (Plot plot in (IEnumerable<Plot>) this.m_plots)
      {
        IDictionary<Plot, int> dictionary = (IDictionary<Plot, int>) null;
        if (!dCwTotals.TryGetValue(plot.Strata, out dictionary))
        {
          dictionary = (IDictionary<Plot, int>) new Dictionary<Plot, int>();
          dCwTotals[plot.Strata] = dictionary;
        }
        dictionary[plot] = 0;
      }
    }

    private Condition getTreeCondition(Tree tree)
    {
      Condition condition = Condition.Default;
      if (this.curYear.RecordCrownCondition && tree.Crown != null && tree.Crown.Condition != null)
        condition = tree.Crown.Condition;
      return condition;
    }

    private SpeciesView getSpecies(Tree tree)
    {
      SpeciesView species;
      ReportBase.m_ps.Species.TryGetValue(tree.Species, out species);
      return species;
    }

    private void InitializeStrataCounts(
      SpeciesView species,
      Condition cond,
      Tree tree,
      IDictionary<SpeciesView, IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>> dSpCond,
      IDictionary<SpeciesView, IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>> dSpCounts,
      IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>> dSpTotals,
      IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>> dCwCond,
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals)
    {
      IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>> dictionary1 = (IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>) null;
      IDictionary<Strata, IDictionary<Plot, int>> totals1 = (IDictionary<Strata, IDictionary<Plot, int>>) null;
      if (!dSpCond.TryGetValue(species, out dictionary1))
      {
        dictionary1 = (IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>) new Dictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>();
        dSpCond[species] = dictionary1;
      }
      if (!dictionary1.TryGetValue(cond, out totals1))
      {
        totals1 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
        dictionary1[cond] = totals1;
      }
      this.IncrementStratumCount(totals1, true, tree.Plot.Strata, tree.Plot);
      IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>> dictionary2 = (IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>) null;
      IDictionary<Strata, IDictionary<Plot, int>> totals2 = (IDictionary<Strata, IDictionary<Plot, int>>) null;
      if (!dSpCounts.TryGetValue(species, out dictionary2))
      {
        dictionary2 = (IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>) new Dictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>();
        dSpCounts[species] = dictionary2;
      }
      foreach (HealthRptClass healthRptClass in (IEnumerable<HealthRptClass>) this.curYear.HealthRptClasses)
      {
        if (!dictionary2.TryGetValue(healthRptClass, out totals2))
        {
          totals2 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
          dictionary2[healthRptClass] = totals2;
        }
        bool treeConditionExists = healthRptClass.Equals((object) YearHelper.ReturnHealthClass(this.curYear.HealthRptClasses, cond.PctDieback));
        this.IncrementStratumCount(totals2, treeConditionExists, tree.Plot.Strata, tree.Plot);
      }
      IDictionary<Strata, IDictionary<Plot, int>> totals3 = (IDictionary<Strata, IDictionary<Plot, int>>) null;
      if (!dSpTotals.TryGetValue(species, out totals3))
      {
        totals3 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
        dSpTotals[species] = totals3;
      }
      this.IncrementStratumCount(totals3, true, tree.Plot.Strata, tree.Plot);
      HealthRptClass key = YearHelper.ReturnHealthClass(this.curYear.HealthRptClasses, cond.PctDieback);
      this.IncrementStratumCount(dCwCond[key], true, tree.Plot.Strata, tree.Plot);
      this.IncrementStratumCount(dCwTotals, true, tree.Plot.Strata, tree.Plot);
    }

    private IDictionary<Strata, double> getdStrataArea(
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals)
    {
      IDictionary<Strata, double> dictionary = (IDictionary<Strata, double>) new Dictionary<Strata, double>();
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> dCwTotal in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dCwTotals)
      {
        int count = dCwTotal.Value.Count;
        double divisor = 0.0;
        double num1 = 0.0;
        long num2 = 0;
        Strata key1 = dCwTotal.Key;
        foreach (KeyValuePair<Plot, int> keyValuePair in (IEnumerable<KeyValuePair<Plot, int>>) dCwTotal.Value)
        {
          Plot key2 = keyValuePair.Key;
          divisor += (double) key2.Size * (double) key2.PercentMeasured / 100.0;
          num2 += (long) keyValuePair.Value;
          num1 += (double) (keyValuePair.Value * keyValuePair.Value);
        }
        if (this.series.IsSample)
        {
          double num3 = EstimateUtil.DivideOrZero(num1 - EstimateUtil.DivideOrZero((double) (num2 * num2), (long) count), (long) (count - 1));
          double num4 = EstimateUtil.DivideOrZero((double) key1.Size * (double) count, divisor);
          this.m_cwTotal.StdErr += EstimateUtil.DivideOrZero(num4 * (num4 - (double) count) * num3, (long) count);
          this.m_cwTotal.Count += EstimateUtil.DivideOrZero((double) num2 * (double) key1.Size, divisor);
        }
        else
          this.m_cwTotal.Count += (double) num2;
        dictionary[key1] = divisor;
      }
      return dictionary;
    }

    private void InitializeSpeciesStats(
      IDictionary<SpeciesView, IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>> dSpCounts,
      IDictionary<SpeciesView, double> dCwSpTotal,
      IDictionary<Strata, double> dStrataArea,
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals)
    {
      foreach (KeyValuePair<SpeciesView, IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>> dSpCount in (IEnumerable<KeyValuePair<SpeciesView, IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>>) dSpCounts)
      {
        IDictionary<HealthRptClass, TreeStats> dictionary = (IDictionary<HealthRptClass, TreeStats>) new SortedDictionary<HealthRptClass, TreeStats>((IComparer<HealthRptClass>) new HealthRptClass.HealthRptClassComparer());
        SpeciesView key1 = dSpCount.Key;
        double divisor1 = dCwSpTotal[key1];
        foreach (KeyValuePair<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>> keyValuePair1 in (IEnumerable<KeyValuePair<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>) dSpCount.Value)
        {
          HealthRptClass key2 = keyValuePair1.Key;
          TreeStats treeStats = new TreeStats();
          foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair2 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) keyValuePair1.Value)
          {
            Strata key3 = keyValuePair2.Key;
            double num1 = 0.0;
            double num2 = 0.0;
            foreach (KeyValuePair<Plot, int> keyValuePair3 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair2.Value)
            {
              Plot key4 = keyValuePair3.Key;
              num1 += (double) keyValuePair3.Value;
              num2 += (double) (keyValuePair3.Value * keyValuePair3.Value);
            }
            if (this.series.IsSample)
            {
              double divisor2 = dStrataArea[key3];
              int count = dCwTotals[key3].Keys.Count;
              double num3 = EstimateUtil.DivideOrZero(num2 - EstimateUtil.DivideOrZero(num1 * num1, (long) count), (long) (count - 1));
              double num4 = EstimateUtil.DivideOrZero((double) key3.Size * (double) count, divisor2);
              treeStats.StdErr += EstimateUtil.DivideOrZero(num4 * (num4 - (double) count) * num3, (long) count);
              treeStats.Count += EstimateUtil.DivideOrZero(num1 * (double) key3.Size, divisor2);
            }
            else
              treeStats.Count += num1;
          }
          treeStats.Pct = EstimateUtil.DivideOrZero(treeStats.Count * 100.0, divisor1);
          treeStats.PctTrees = EstimateUtil.DivideOrZero(treeStats.Count * 100.0, this.m_cwTotal.Count);
          if (this.series.IsSample)
            treeStats.StdErr = Math.Sqrt(treeStats.StdErr);
          dictionary[key2] = treeStats;
        }
        this.m_dSpCmnStats[key1] = dictionary;
        this.m_dSpSciStats[key1] = dictionary;
      }
    }

    private void CalculateRelativePerformanceIndex(
      IDictionary<SpeciesView, IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>> dSpCond,
      IDictionary<Strata, double> dStrataArea,
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals)
    {
      double dividend1 = 0.0;
      foreach (KeyValuePair<SpeciesView, IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>> keyValuePair1 in (IEnumerable<KeyValuePair<SpeciesView, IDictionary<Condition, IDictionary<Strata, IDictionary<Plot, int>>>>>) dSpCond)
      {
        SpeciesView key1 = keyValuePair1.Key;
        double dividend2 = 0.0;
        foreach (KeyValuePair<Condition, IDictionary<Strata, IDictionary<Plot, int>>> keyValuePair2 in (IEnumerable<KeyValuePair<Condition, IDictionary<Strata, IDictionary<Plot, int>>>>) keyValuePair1.Value)
        {
          Condition key2 = keyValuePair2.Key;
          foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair3 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) keyValuePair2.Value)
          {
            Strata key3 = keyValuePair3.Key;
            double num = 0.0;
            foreach (KeyValuePair<Plot, int> keyValuePair4 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair3.Value)
              num += (double) keyValuePair4.Value;
            if (this.series.IsSample)
            {
              double divisor = dStrataArea[key3];
              int count = dCwTotals[key3].Keys.Count;
              num = EstimateUtil.DivideOrZero(num * (double) key3.Size, divisor);
            }
            dividend2 += num * (1.0 - key2.PctDieback / 100.0);
          }
        }
        dividend1 += dividend2;
        double num1 = EstimateUtil.DivideOrZero(dividend2, this.m_dSpStats[key1].Count);
        this.m_dSpRPI[key1] = num1;
      }
      double divisor1 = EstimateUtil.DivideOrZero(dividend1, this.m_cwTotal.Count);
      foreach (KeyValuePair<SpeciesView, TreeStats> dSpStat in (IEnumerable<KeyValuePair<SpeciesView, TreeStats>>) this.m_dSpStats)
      {
        SpeciesView key = dSpStat.Key;
        TreeStats treeStats = dSpStat.Value;
        treeStats.PctTrees = EstimateUtil.DivideOrZero(treeStats.Count * 100.0, this.m_cwTotal.Count);
        this.m_dSpRPI[key] = EstimateUtil.DivideOrZero(this.m_dSpRPI[key], divisor1);
      }
    }

    private IDictionary<SpeciesView, double> getCwSpTotal(
      IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>> dSpTotals,
      IDictionary<Strata, double> dStrataArea,
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals)
    {
      IDictionary<SpeciesView, double> cwSpTotal = (IDictionary<SpeciesView, double>) new Dictionary<SpeciesView, double>();
      foreach (KeyValuePair<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>> dSpTotal in (IEnumerable<KeyValuePair<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>>) dSpTotals)
      {
        SpeciesView key1 = dSpTotal.Key;
        TreeStats treeStats = new TreeStats();
        double divisor1 = 0.0;
        foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair1 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dSpTotal.Value)
        {
          Strata key2 = keyValuePair1.Key;
          double divisor2 = dStrataArea[key2];
          double num1 = 0.0;
          int count = dCwTotals[key2].Keys.Count;
          long num2 = 0;
          foreach (KeyValuePair<Plot, int> keyValuePair2 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair1.Value)
          {
            Plot key3 = keyValuePair2.Key;
            num2 += (long) keyValuePair2.Value;
            num1 += (double) (keyValuePair2.Value * keyValuePair2.Value);
          }
          if (this.series.IsSample)
          {
            double num3 = EstimateUtil.DivideOrZero(num1 - EstimateUtil.DivideOrZero((double) (num2 * num2), (long) count), (long) (count - 1));
            double num4 = EstimateUtil.DivideOrZero((double) key2.Size * (double) count, divisor2);
            treeStats.StdErr += EstimateUtil.DivideOrZero(num4 * (num4 - (double) count) * num3, (long) count);
            treeStats.Count += EstimateUtil.DivideOrZero((double) num2 * (double) key2.Size, divisor2);
            divisor1 += EstimateUtil.DivideOrZero((double) num2 * (double) key2.Size, divisor2);
          }
          else
          {
            treeStats.Count += (double) num2;
            divisor1 += (double) num2;
          }
        }
        treeStats.Pct = EstimateUtil.DivideOrZero(treeStats.Count * 100.0, divisor1);
        if (this.series.IsSample)
          treeStats.StdErr = Math.Sqrt(treeStats.StdErr);
        this.m_dSpStats[key1] = treeStats;
        cwSpTotal[key1] = divisor1;
      }
      return cwSpTotal;
    }

    private void CalculateSEandPercentOfTrees(
      IDictionary<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>> conditionCategoryGroups,
      IDictionary<Strata, double> dStrataArea,
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals)
    {
      foreach (KeyValuePair<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>> conditionCategoryGroup in (IEnumerable<KeyValuePair<HealthRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>) conditionCategoryGroups)
        this.m_dCwStats[conditionCategoryGroup.Key] = this.GetConditionStrataStats(conditionCategoryGroup.Value, dStrataArea, dCwTotals);
    }

    private TreeStats GetConditionStrataStats(
      IDictionary<Strata, IDictionary<Plot, int>> conditionStrata,
      IDictionary<Strata, double> dStrataArea,
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals)
    {
      TreeStats conditionStrataStats = new TreeStats();
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> conditionStratum in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) conditionStrata)
      {
        Strata key = conditionStratum.Key;
        double num1 = 0.0;
        double num2 = 0.0;
        foreach (KeyValuePair<Plot, int> keyValuePair in (IEnumerable<KeyValuePair<Plot, int>>) conditionStratum.Value)
        {
          num1 += (double) keyValuePair.Value;
          num2 += (double) (keyValuePair.Value * keyValuePair.Value);
        }
        if (this.series.IsSample)
        {
          double divisor = dStrataArea[key];
          int count = dCwTotals[key].Keys.Count;
          double num3 = EstimateUtil.DivideOrZero(num2 - EstimateUtil.DivideOrZero(num1 * num1, (long) count), (long) (count - 1));
          double num4 = EstimateUtil.DivideOrZero((double) key.Size * (double) count, divisor);
          conditionStrataStats.StdErr += EstimateUtil.DivideOrZero(num4 * (num4 - (double) count) * num3, (long) count);
          conditionStrataStats.Count += EstimateUtil.DivideOrZero(num1 * (double) key.Size, divisor);
        }
        else
          conditionStrataStats.Count += num1;
      }
      conditionStrataStats.Pct = EstimateUtil.DivideOrZero(conditionStrataStats.Count * 100.0, this.m_cwTotal.Count);
      conditionStrataStats.PctTrees = EstimateUtil.DivideOrZero(conditionStrataStats.Count * 100.0, this.m_cwTotal.Count);
      if (this.series.IsSample)
        conditionStrataStats.StdErr = Math.Sqrt(conditionStrataStats.StdErr);
      return conditionStrataStats;
    }

    private void IncrementStratumCount(
      IDictionary<Strata, IDictionary<Plot, int>> totals,
      bool treeConditionExists,
      Strata stratum,
      Plot plot)
    {
      IDictionary<Plot, int> dictionary = (IDictionary<Plot, int>) null;
      int num = 0;
      if (!totals.TryGetValue(stratum, out dictionary))
      {
        dictionary = (IDictionary<Plot, int>) new Dictionary<Plot, int>();
        totals[stratum] = dictionary;
      }
      dictionary.TryGetValue(plot, out num);
      if (treeConditionExists)
        ++num;
      dictionary[plot] = num;
    }

    private void AssignDataRows(DataTable data)
    {
      foreach (KeyValuePair<SpeciesView, IDictionary<HealthRptClass, TreeStats>> keyValuePair in ReportBase.ScientificName ? (IEnumerable<KeyValuePair<SpeciesView, IDictionary<HealthRptClass, TreeStats>>>) this.m_dSpSciStats : (IEnumerable<KeyValuePair<SpeciesView, IDictionary<HealthRptClass, TreeStats>>>) this.m_dSpCmnStats)
      {
        SpeciesView key = keyValuePair.Key;
        DataRow row = data.Rows.Add();
        row[this.speciesColumn] = ReportBase.ScientificName ? (object) key.ScientificName : (object) key.CommonName;
        this.AssignHealthClassesDataRow(row, this.healthClasses, keyValuePair.Value);
        row[this.relativePerformanceIndexColumn] = (object) this.m_dSpRPI[key];
        TreeStats dSpStat = this.m_dSpStats[key];
        row[this.treeCountColumn] = (object) dSpStat.Count;
        row[this.standardErrorColumn] = (object) dSpStat.StdErr;
        row[this.percentOfTreesColumn] = (object) dSpStat.PctTrees;
      }
    }

    private void AssignHealthClassesDataRow(
      DataRow row,
      List<HealthRptClass> healthClasses,
      IDictionary<HealthRptClass, TreeStats> stats)
    {
      foreach (HealthRptClass healthClass in healthClasses)
        row[healthClass.Id.ToString()] = (object) stats[healthClass].Pct;
    }

    private void AssignTotals(DataTable data)
    {
      DataRow row = data.Rows.Add();
      row[this.speciesColumn] = (object) i_Tree_Eco_v6.Resources.Strings.Total;
      this.AssignHealthClassesDataRow(row, this.healthClasses, this.m_dCwStats);
      row[this.relativePerformanceIndexColumn] = (object) 1.0;
      row[this.treeCountColumn] = (object) this.m_cwTotal.Count;
      row[this.standardErrorColumn] = (object) this.m_cwTotal.StdErr;
      row[this.percentOfTreesColumn] = (object) this.m_cwTotal.PctTrees;
    }
  }
}
