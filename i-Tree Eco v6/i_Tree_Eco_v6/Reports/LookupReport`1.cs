// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.LookupReport`1
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;

namespace i_Tree_Eco_v6.Reports
{
  public abstract class LookupReport<T> : DatabaseReport, IDisposable where T : Lookup<T>
  {
    private IList<T> m_lookups;
    private IList<DBHRptClass> m_dbhClasses;
    private IList<DBH> m_dbhs;
    private IList<Strata> m_strata;
    private IList<Tree> m_trees;
    private IList<Plot> m_plots;
    private Dictionary<int, double> m_dMidPts;
    private Dictionary<string, SpeciesView> m_dSpCache;
    private IDictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>> m_dSpCmnStats;
    private IDictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>> m_dSpSciStats;
    private IDictionary<T, IDictionary<SpeciesView, TreeStats>> m_dLuStats;
    private List<Tuple<T, TreeStats>> m_dMtStats;
    private Func<Tree, object> m_treeProp;
    private string m_header;
    public bool totalsOnly;
    public bool byMaintenance;

    public LookupReport(string property, string title, string header)
    {
      this.hasZeros = true;
      this.m_dMidPts = new Dictionary<int, double>();
      this.m_dSpCache = ProgramSession.GetInstance().Species;
      this.ReportTitle = title;
      this.m_header = header;
      ParameterExpression parameterExpression;
      this.m_treeProp = System.Linq.Expressions.Expression.Lambda<Func<Tree, object>>((System.Linq.Expressions.Expression) System.Linq.Expressions.Expression.Property((System.Linq.Expressions.Expression) parameterExpression, property), parameterExpression).Compile();
      this.Init();
    }

    private void Init()
    {
      using (this.curInputISession.BeginTransaction())
      {
        this.m_lookups = this.curInputISession.CreateCriteria<T>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).List<T>();
        this.m_dbhClasses = this.curInputISession.CreateCriteria<DBHRptClass>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).List<DBHRptClass>();
        this.m_strata = this.curInputISession.CreateCriteria<Strata>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).List<Strata>();
        this.m_trees = this.curInputISession.CreateCriteria<Tree>().CreateAlias("Plot", "p").Add((ICriterion) Restrictions.Eq("p.Year", (object) this.curYear)).Add((ICriterion) Restrictions.Eq("p.IsComplete", (object) true)).Add((ICriterion) Restrictions.On<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
        {
          (object) TreeStatus.InitialSample,
          (object) TreeStatus.Ingrowth,
          (object) TreeStatus.NoChange,
          (object) TreeStatus.Planted,
          (object) TreeStatus.Unknown
        })).List<Tree>();
        this.m_plots = this.curInputISession.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).Add((ICriterion) Restrictions.Eq("IsComplete", (object) true)).List<Plot>();
        this.m_dbhs = this.curInputISession.CreateCriteria<DBH>().Add((ICriterion) Restrictions.Eq((IProjection) Projections.Property<DBH>((System.Linq.Expressions.Expression<Func<DBH, object>>) (d => d.Year)), (object) this.curYear)).List<DBH>();
        foreach (DBH dbh in (IEnumerable<DBH>) this.m_dbhs)
          this.m_dMidPts[dbh.Id] = dbh.Value;
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.curInputISession == null)
        return;
      this.curInputISession.Dispose();
      this.curInputISession = (ISession) null;
    }

    private TreeStats GetStats(
      IDictionary<Strata, IDictionary<Plot, int>> group,
      IDictionary<Strata, double> dStrataArea,
      IDictionary<Strata, IDictionary<Plot, int>> dCwTotals,
      double spTotal,
      double cwTotal)
    {
      TreeStats stats = new TreeStats();
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair1 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) group)
      {
        Strata key1 = keyValuePair1.Key;
        double divisor = dStrataArea[key1];
        double num1 = 0.0;
        double num2 = 0.0;
        int count = dCwTotals[key1].Keys.Count;
        foreach (KeyValuePair<Plot, int> keyValuePair2 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair1.Value)
        {
          Plot key2 = keyValuePair2.Key;
          num1 += (double) keyValuePair2.Value;
          num2 += (double) (keyValuePair2.Value * keyValuePair2.Value);
        }
        if (this.series.IsSample)
        {
          double num3 = EstimateUtil.DivideOrZero(num2 - EstimateUtil.DivideOrZero(num1 * num1, (long) count), (long) (count - 1));
          double num4 = EstimateUtil.DivideOrZero((double) key1.Size * (double) count, divisor);
          stats.StdErr += EstimateUtil.DivideOrZero(num4 * (num4 - (double) count) * num3, (long) count);
          stats.Count += EstimateUtil.DivideOrZero(num1 * (double) key1.Size, divisor);
        }
        else
          stats.Count += num1;
      }
      stats.Pct = EstimateUtil.DivideOrZero(stats.Count * 100.0, spTotal);
      stats.PctTrees = EstimateUtil.DivideOrZero(stats.Count * 100.0, cwTotal);
      if (this.series.IsSample)
        stats.StdErr = Math.Sqrt(stats.StdErr);
      return stats;
    }

    private void CalculateStats()
    {
      this.m_dSpCmnStats = (IDictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>>) new SortedDictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>>((IComparer<T>) new PropertyComparer<T>((Func<T, object>) (lu => (object) lu.Description)));
      this.m_dSpSciStats = (IDictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>>) new SortedDictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>>((IComparer<T>) new PropertyComparer<T>((Func<T, object>) (lu => (object) lu.Description)));
      this.m_dLuStats = (IDictionary<T, IDictionary<SpeciesView, TreeStats>>) new Dictionary<T, IDictionary<SpeciesView, TreeStats>>();
      this.m_dMtStats = new List<Tuple<T, TreeStats>>();
      IDictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>> dictionary1 = (IDictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>>) new Dictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>>();
      IDictionary<T, IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>> dictionary2 = (IDictionary<T, IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>>) new Dictionary<T, IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>>();
      IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>> dictionary3 = (IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>) new Dictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>();
      IDictionary<Strata, IDictionary<Plot, int>> dictionary4 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      IDictionary<T, IDictionary<Strata, IDictionary<Plot, int>>> dictionary5 = (IDictionary<T, IDictionary<Strata, IDictionary<Plot, int>>>) new Dictionary<T, IDictionary<Strata, IDictionary<Plot, int>>>();
      foreach (Plot plot in (IEnumerable<Plot>) this.m_plots)
      {
        IDictionary<Plot, int> dictionary6 = (IDictionary<Plot, int>) null;
        if (!dictionary4.TryGetValue(plot.Strata, out dictionary6))
        {
          dictionary6 = (IDictionary<Plot, int>) new Dictionary<Plot, int>();
          dictionary4[plot.Strata] = dictionary6;
        }
        dictionary6[plot] = 0;
      }
      foreach (Tree tree in (IEnumerable<Tree>) this.m_trees)
      {
        SpeciesView key1;
        if (this.m_dSpCache.TryGetValue(tree.Species, out key1))
        {
          double d = 0.0;
          foreach (Stem stem in (IEnumerable<Stem>) tree.Stems)
          {
            double num = 0.0;
            if (this.curYear.DBHActual)
              num = stem.Diameter;
            else
              this.m_dMidPts.TryGetValue((int) stem.Diameter, out num);
            d += num * num;
          }
          double num1 = Math.Sqrt(d);
          DBHRptClass dbhRptClass = (DBHRptClass) null;
          foreach (DBHRptClass dbhClass in (IEnumerable<DBHRptClass>) this.m_dbhClasses)
          {
            if (num1 > dbhClass.RangeStart && (dbhRptClass == null || dbhClass.RangeStart > dbhRptClass.RangeStart))
              dbhRptClass = dbhClass;
          }
          if (key1 != null && dbhRptClass != null)
          {
            IDictionary<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>> dictionary7 = (IDictionary<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>) null;
            IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>> dictionary8 = (IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>) null;
            IDictionary<Strata, IDictionary<Plot, int>> dTotals1 = (IDictionary<Strata, IDictionary<Plot, int>>) null;
            IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>> dictionary9 = (IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>) null;
            IDictionary<Strata, IDictionary<Plot, int>> dTotals2 = (IDictionary<Strata, IDictionary<Plot, int>>) null;
            T key2 = (T) this.m_treeProp(tree);
            foreach (T lookup in (IEnumerable<T>) this.m_lookups)
            {
              if (!dictionary1.TryGetValue(lookup, out dictionary7))
              {
                dictionary7 = (IDictionary<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>) new Dictionary<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>();
                dictionary1[lookup] = dictionary7;
              }
              if (!dictionary7.TryGetValue(key1, out dictionary8))
              {
                dictionary8 = (IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>) new SortedDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>((IComparer<DBHRptClass>) new PropertyComparer<DBHRptClass>((Func<DBHRptClass, object>) (dbh => (object) dbh.RangeStart)));
                dictionary7[key1] = dictionary8;
              }
              foreach (DBHRptClass dbhClass in (IEnumerable<DBHRptClass>) this.m_dbhClasses)
              {
                if (!dictionary8.TryGetValue(dbhClass, out dTotals1))
                {
                  dTotals1 = (IDictionary<Strata, IDictionary<Plot, int>>) new SortedDictionary<Strata, IDictionary<Plot, int>>((IComparer<Strata>) new PropertyComparer<Strata>((Func<Strata, object>) (dbh => (object) dbh.Description)));
                  dictionary8[dbhClass] = dTotals1;
                }
                this.IncStrataCount(dTotals1, lookup.Equals((object) key2) && dbhClass == dbhRptClass, tree.Plot.Strata, tree.Plot);
              }
              if (!dictionary2.TryGetValue(lookup, out dictionary9))
              {
                dictionary9 = (IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>) new Dictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>();
                dictionary2[lookup] = dictionary9;
              }
              if (!dictionary9.TryGetValue(key1, out dTotals2))
              {
                dTotals2 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
                dictionary9[key1] = dTotals2;
              }
              this.IncStrataCount(dTotals2, lookup.Equals((object) key2), tree.Plot.Strata, tree.Plot);
            }
            IDictionary<Strata, IDictionary<Plot, int>> dTotals3 = (IDictionary<Strata, IDictionary<Plot, int>>) null;
            if (!dictionary3.TryGetValue(key1, out dTotals3))
            {
              dTotals3 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
              dictionary3[key1] = dTotals3;
            }
            this.IncStrataCount(dTotals3, true, tree.Plot.Strata, tree.Plot);
            IDictionary<Strata, IDictionary<Plot, int>> dTotals4 = (IDictionary<Strata, IDictionary<Plot, int>>) null;
            if ((object) key2 != null)
            {
              if (!dictionary5.TryGetValue(key2, out dTotals4))
              {
                dTotals4 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
                dictionary5[key2] = dTotals4;
              }
              this.IncStrataCount(dTotals4, true, tree.Plot.Strata, tree.Plot);
            }
            this.IncStrataCount(dictionary4, true, tree.Plot.Strata, tree.Plot);
          }
        }
      }
      IDictionary<Strata, double> dStrataArea = (IDictionary<Strata, double>) new Dictionary<Strata, double>();
      IDictionary<SpeciesView, double> dictionary10 = (IDictionary<SpeciesView, double>) new Dictionary<SpeciesView, double>();
      double cwTotal = 0.0;
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair1 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dictionary4)
      {
        double num = 0.0;
        Strata key3 = keyValuePair1.Key;
        foreach (KeyValuePair<Plot, int> keyValuePair2 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair1.Value)
        {
          Plot key4 = keyValuePair2.Key;
          num += (double) key4.Size * (double) key4.PercentMeasured / 100.0;
        }
        dStrataArea[key3] = num;
      }
      foreach (KeyValuePair<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>> keyValuePair3 in (IEnumerable<KeyValuePair<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>>) dictionary3)
      {
        SpeciesView key5 = keyValuePair3.Key;
        double num2 = 0.0;
        foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair4 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) keyValuePair3.Value)
        {
          Strata key6 = keyValuePair4.Key;
          long num3 = 0;
          foreach (KeyValuePair<Plot, int> keyValuePair5 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair4.Value)
          {
            Plot key7 = keyValuePair5.Key;
            num3 += (long) keyValuePair5.Value;
          }
          if (this.series.IsSample)
          {
            double divisor = dStrataArea[key6];
            num2 += EstimateUtil.DivideOrZero((double) num3 * (double) key6.Size, divisor);
          }
          else
            num2 += (double) num3;
        }
        dictionary10[key5] = num2;
        cwTotal += num2;
      }
      foreach (KeyValuePair<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>> keyValuePair6 in (IEnumerable<KeyValuePair<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>>>) dictionary1)
      {
        IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>> dictionary11 = (IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>) new SortedDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>((IComparer<SpeciesView>) new PropertyComparer<SpeciesView>((Func<SpeciesView, object>) (sv => (object) sv.CommonName)));
        IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>> dictionary12 = (IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>) new SortedDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>((IComparer<SpeciesView>) new PropertyComparer<SpeciesView>((Func<SpeciesView, object>) (sv => (object) sv.ScientificName)));
        T key8 = keyValuePair6.Key;
        foreach (KeyValuePair<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>> keyValuePair7 in (IEnumerable<KeyValuePair<SpeciesView, IDictionary<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>>) keyValuePair6.Value)
        {
          IDictionary<DBHRptClass, TreeStats> dictionary13 = (IDictionary<DBHRptClass, TreeStats>) new SortedDictionary<DBHRptClass, TreeStats>((IComparer<DBHRptClass>) new PropertyComparer<DBHRptClass>((Func<DBHRptClass, object>) (dbh => (object) dbh.RangeStart)));
          SpeciesView key9 = keyValuePair7.Key;
          double spTotal = dictionary10[key9];
          foreach (KeyValuePair<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>> keyValuePair8 in (IEnumerable<KeyValuePair<DBHRptClass, IDictionary<Strata, IDictionary<Plot, int>>>>) keyValuePair7.Value)
          {
            DBHRptClass key10 = keyValuePair8.Key;
            dictionary13[key10] = this.GetStats(keyValuePair8.Value, dStrataArea, dictionary4, spTotal, cwTotal);
          }
          dictionary11[key9] = dictionary13;
          dictionary12[key9] = dictionary13;
        }
        this.m_dSpCmnStats[key8] = dictionary11;
        this.m_dSpSciStats[key8] = dictionary12;
      }
      foreach (KeyValuePair<T, IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>> keyValuePair9 in (IEnumerable<KeyValuePair<T, IDictionary<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>>>) dictionary2)
      {
        IDictionary<SpeciesView, TreeStats> dictionary14 = (IDictionary<SpeciesView, TreeStats>) new Dictionary<SpeciesView, TreeStats>();
        T key11 = keyValuePair9.Key;
        foreach (KeyValuePair<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>> keyValuePair10 in (IEnumerable<KeyValuePair<SpeciesView, IDictionary<Strata, IDictionary<Plot, int>>>>) keyValuePair9.Value)
        {
          SpeciesView key12 = keyValuePair10.Key;
          double spTotal = dictionary10[key12];
          dictionary14[key12] = this.GetStats(keyValuePair10.Value, dStrataArea, dictionary4, spTotal, cwTotal);
        }
        this.m_dLuStats[key11] = dictionary14;
      }
      foreach (KeyValuePair<T, IDictionary<Strata, IDictionary<Plot, int>>> keyValuePair in (IEnumerable<KeyValuePair<T, IDictionary<Strata, IDictionary<Plot, int>>>>) dictionary5)
      {
        Dictionary<T, TreeStats> dictionary15 = new Dictionary<T, TreeStats>();
        this.m_dMtStats.Add(new Tuple<T, TreeStats>(keyValuePair.Key, this.GetStats(keyValuePair.Value, dStrataArea, dictionary4, 0.0, cwTotal)));
      }
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics FormGraphics)
    {
      if (this.m_dMtStats == null || this.m_dSpCmnStats == null || this.m_dSpSciStats == null || this.m_dLuStats == null)
        this.CalculateStats();
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Style.Font = new Font("Calibri", 10f);
      renderTable.Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      ReportUtil.FormatRenderTableHeader(renderTable);
      Style style = ReportUtil.AddAlternateStyle(renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = renderTable.Cols[4].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = this.m_header;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 2].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.DBHClass, ReportBase.CmUnits());
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.TreeCount;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.StandardError;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfSpecies;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfTrees;
      renderTable.Cols[4].Visible = this.series.IsSample;
      renderTable.Cols[2].Visible = !this.totalsOnly;
      if (this.byMaintenance)
        renderTable.Cols[1].Visible = renderTable.Cols[2].Visible = renderTable.Cols[5].Visible = false;
      IDictionary<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>> dictionary = ReportBase.ScientificName ? this.m_dSpSciStats : this.m_dSpCmnStats;
      YearUnit yearUnit = ReportBase.EnglishUnits ? YearUnit.English : YearUnit.Metric;
      int num = count;
      if (this.byMaintenance)
      {
        this.m_dMtStats.Sort((Comparison<Tuple<T, TreeStats>>) ((x, y) => x.Item1.Description.CompareTo(y.Item1.Description)));
        foreach (Tuple<T, TreeStats> dMtStat in this.m_dMtStats)
        {
          TreeStats treeStats = dMtStat.Item2;
          renderTable.Cells[num, 0].Text = dMtStat.Item1.Description;
          renderTable.Cells[num, 3].Text = string.Format("{0:N0}", (object) treeStats.Count);
          renderTable.Cells[num, 4].Text = string.Format("(±{0:N0})", (object) treeStats.StdErr);
          renderTable.Cells[num, 6].Text = string.Format("{0:F1}", (object) treeStats.PctTrees);
          if ((num - count) % 2 == 0)
            renderTable.Rows[num].Style.Parent = style;
          ++num;
        }
      }
      else
      {
        foreach (KeyValuePair<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>> keyValuePair1 in (IEnumerable<KeyValuePair<T, IDictionary<SpeciesView, IDictionary<DBHRptClass, TreeStats>>>>) dictionary)
        {
          T key1 = keyValuePair1.Key;
          foreach (KeyValuePair<SpeciesView, IDictionary<DBHRptClass, TreeStats>> keyValuePair2 in (IEnumerable<KeyValuePair<SpeciesView, IDictionary<DBHRptClass, TreeStats>>>) keyValuePair1.Value)
          {
            SpeciesView key2 = keyValuePair2.Key;
            TreeStats treeStats1 = this.m_dLuStats[key1][key2];
            if (!ReportBase.m_ps.HideZeros || treeStats1.Count != 0.0)
            {
              renderTable.Cells[num, 0].Text = key1.Description;
              renderTable.Cells[num, 1].Text = ReportBase.ScientificName ? key2.ScientificName : key2.CommonName;
              if (!this.totalsOnly)
              {
                foreach (KeyValuePair<DBHRptClass, TreeStats> keyValuePair3 in (IEnumerable<KeyValuePair<DBHRptClass, TreeStats>>) keyValuePair2.Value)
                {
                  DBHRptClass key3 = keyValuePair3.Key;
                  TreeStats treeStats2 = keyValuePair3.Value;
                  double rangeStart = key3.RangeStart;
                  double rangeEnd = key3.RangeEnd;
                  if (this.curYear.Unit != yearUnit)
                  {
                    if (yearUnit == YearUnit.Metric)
                    {
                      rangeStart *= 2.54;
                      rangeEnd *= 2.54;
                    }
                    else
                    {
                      rangeStart /= 2.54;
                      rangeEnd /= 2.54;
                    }
                  }
                  renderTable.Cells[num, 2].Text = Math.Abs(key3.RangeEnd - 1000.0) <= double.Epsilon ? string.Format("{0:0.0}+", (object) rangeStart) : string.Format("{0:0.0} - {1:0.0}", (object) rangeStart, (object) rangeEnd);
                  renderTable.Cells[num, 3].Text = string.Format("{0:N0}", (object) treeStats2.Count);
                  renderTable.Cells[num, 4].Text = string.Format("(±{0:N0})", (object) treeStats2.StdErr);
                  renderTable.Cells[num, 5].Text = string.Format("{0:F1}", (object) treeStats2.Pct);
                  renderTable.Cells[num, 6].Text = string.Format("{0:F1}", (object) treeStats2.PctTrees);
                  if ((num - count) % 2 == 0)
                    renderTable.Rows[num].Style.Parent = style;
                  ++num;
                }
                renderTable.Rows[num].Style.FontBold = true;
                renderTable.Rows[num].Style.Borders.Top = renderTable.Rows[num].Style.Borders.Bottom = LineDef.Default;
              }
              renderTable.Cells[num, 2].Text = i_Tree_Eco_v6.Resources.Strings.Total;
              renderTable.Cells[num, 3].Text = string.Format("{0:N0}", (object) treeStats1.Count);
              renderTable.Cells[num, 4].Text = string.Format("(±{0:N0})", (object) treeStats1.StdErr);
              renderTable.Cells[num, 5].Text = string.Format("{0:F1}", (object) treeStats1.Pct);
              renderTable.Cells[num, 6].Text = string.Format("{0:F1}", (object) treeStats1.PctTrees);
              if ((num - count) % 2 == 0)
                renderTable.Rows[num].Style.Parent = style;
              ++num;
            }
          }
        }
      }
    }

    private void IncStrataCount(
      IDictionary<Strata, IDictionary<Plot, int>> dTotals,
      bool condition,
      Strata strata,
      Plot plot)
    {
      IDictionary<Plot, int> dictionary = (IDictionary<Plot, int>) null;
      int num = 0;
      if (!dTotals.TryGetValue(strata, out dictionary))
      {
        dictionary = (IDictionary<Plot, int>) new Dictionary<Plot, int>();
        dTotals[strata] = dictionary;
      }
      dictionary.TryGetValue(plot, out num);
      if (condition)
        ++num;
      dictionary[plot] = num;
    }

    protected string FormatReportTwithSubtitle(string t, string subt) => string.Format("{0} {1}", (object) t, (object) subt);
  }
}
