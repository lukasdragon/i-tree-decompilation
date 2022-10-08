// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.StreetTrees
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class StreetTrees : DatabaseReport
  {
    private IList<Strata> m_strata;
    private IList<Tree> m_trees;
    private IList<Plot> m_plots;
    private PublicPrivateStats m_streetTreeStats;
    private PublicPrivateStats m_nonStreetTreeStats;
    private PublicPrivateStats m_cwTotal;
    private IDictionary<Strata, PublicPrivateStats> m_dCwStats;
    private IDictionary<Strata, PublicPrivateStats> m_dStreetTrreeStats;
    private IDictionary<Strata, PublicPrivateStats> m_dNonStreetTreesStats;
    private bool m_calculated;

    public StreetTrees()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitlePopulationSummaryOfStreetTreesByStratum;
      this.m_calculated = false;
      this.Init();
    }

    private void Init()
    {
      using (this.curInputISession.BeginTransaction())
      {
        this.m_strata = this.curInputISession.CreateCriteria<Strata>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).AddOrder(Order.Asc("Description")).List<Strata>();
        this.m_plots = this.curInputISession.CreateCriteria<Plot>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).Add((ICriterion) Restrictions.Eq("IsComplete", (object) true)).List<Plot>();
        this.m_trees = this.curInputISession.CreateCriteria<Tree>().CreateAlias("Plot", "p").Add((ICriterion) Restrictions.Eq("p.Year", (object) this.curYear)).Add((ICriterion) Restrictions.Eq("p.IsComplete", (object) true)).Add((ICriterion) Restrictions.On<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
        {
          (object) TreeStatus.InitialSample,
          (object) TreeStatus.Ingrowth,
          (object) TreeStatus.NoChange,
          (object) TreeStatus.Planted,
          (object) TreeStatus.Unknown
        })).List<Tree>();
      }
    }

    private void CalculateStats()
    {
      IDictionary<Strata, IDictionary<Plot, int>> dTotals1 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      IDictionary<Strata, IDictionary<Plot, int>> dTotals2 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      IDictionary<Strata, IDictionary<Plot, int>> dTotals3 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      this.m_dStreetTrreeStats = (IDictionary<Strata, PublicPrivateStats>) new Dictionary<Strata, PublicPrivateStats>();
      this.m_dNonStreetTreesStats = (IDictionary<Strata, PublicPrivateStats>) new Dictionary<Strata, PublicPrivateStats>();
      this.m_dCwStats = (IDictionary<Strata, PublicPrivateStats>) new Dictionary<Strata, PublicPrivateStats>();
      this.m_streetTreeStats = new PublicPrivateStats();
      this.m_nonStreetTreeStats = new PublicPrivateStats();
      this.m_cwTotal = new PublicPrivateStats();
      foreach (Plot plot in (IEnumerable<Plot>) this.m_plots)
      {
        IDictionary<Plot, int> dictionary = (IDictionary<Plot, int>) null;
        if (!dTotals3.TryGetValue(plot.Strata, out dictionary))
        {
          dictionary = (IDictionary<Plot, int>) new Dictionary<Plot, int>();
          dTotals3[plot.Strata] = dictionary;
        }
        dictionary[plot] = 0;
      }
      foreach (Strata stratum in (IEnumerable<Strata>) this.m_strata)
      {
        this.m_dStreetTrreeStats[stratum] = new PublicPrivateStats();
        this.m_dNonStreetTreesStats[stratum] = new PublicPrivateStats();
        this.m_dCwStats[stratum] = new PublicPrivateStats();
      }
      foreach (Tree tree in (IEnumerable<Tree>) this.m_trees)
      {
        if (ReportBase.m_ps.Species.ContainsKey(tree.Species))
        {
          this.IncStrataCount(dTotals3, true, tree.Plot.Strata, tree.Plot);
          this.IncStrataCount(dTotals1, tree.StreetTree, tree.Plot.Strata, tree.Plot);
          this.IncStrataCount(dTotals2, !tree.StreetTree, tree.Plot.Strata, tree.Plot);
        }
      }
      IDictionary<Strata, double> dictionary1 = (IDictionary<Strata, double>) new Dictionary<Strata, double>();
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair1 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dTotals3)
      {
        Strata key1 = keyValuePair1.Key;
        PublicPrivateStats dCwStat = this.m_dCwStats[key1];
        int count = keyValuePair1.Value.Count;
        double divisor = 0.0;
        double num1 = 0.0;
        foreach (KeyValuePair<Plot, int> keyValuePair2 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair1.Value)
        {
          Plot key2 = keyValuePair2.Key;
          divisor += (double) key2.Size * (double) key2.PercentMeasured / 100.0;
          dCwStat.Count += (double) keyValuePair2.Value;
          num1 += (double) (keyValuePair2.Value * keyValuePair2.Value);
        }
        if (this.series.IsSample)
        {
          double num2 = EstimateUtil.DivideOrZero(num1 - EstimateUtil.DivideOrZero(dCwStat.Count * dCwStat.Count, (long) count), (long) (count - 1));
          double num3 = EstimateUtil.DivideOrZero((double) key1.Size * (double) count, divisor);
          dCwStat.StdErr = Math.Sqrt(EstimateUtil.DivideOrZero(num3 * (num3 - (double) count) * num2, (long) count));
          dCwStat.Count = EstimateUtil.DivideOrZero(dCwStat.Count * (double) key1.Size, divisor);
          this.m_cwTotal.StdErr += EstimateUtil.DivideOrZero(num3 * (num3 - (double) count) * num2, (long) count);
          this.m_cwTotal.Count += dCwStat.Count;
        }
        dictionary1[key1] = divisor;
      }
      if (this.series.IsSample)
        this.m_cwTotal.StdErr = Math.Sqrt(this.m_cwTotal.StdErr);
      this.m_cwTotal.Pct = 100.0;
      this.m_cwTotal.PctTrees = 100.0;
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair3 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dTotals1)
      {
        Strata key3 = keyValuePair3.Key;
        PublicPrivateStats dStreetTrreeStat = this.m_dStreetTrreeStats[key3];
        int count = dTotals3[key3].Values.Count;
        double divisor = dictionary1[key3];
        double num4 = 0.0;
        foreach (KeyValuePair<Plot, int> keyValuePair4 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair3.Value)
        {
          Plot key4 = keyValuePair4.Key;
          dStreetTrreeStat.Count += (double) keyValuePair4.Value;
          num4 += (double) (keyValuePair4.Value * keyValuePair4.Value);
        }
        if (this.series.IsSample)
        {
          double num5 = EstimateUtil.DivideOrZero(num4 - EstimateUtil.DivideOrZero(dStreetTrreeStat.Count * dStreetTrreeStat.Count, (long) count), (long) (count - 1));
          double num6 = EstimateUtil.DivideOrZero((double) key3.Size * (double) count, divisor);
          dStreetTrreeStat.StdErr = EstimateUtil.DivideOrZero(num6 * (num6 - (double) count) * num5, (long) count);
          dStreetTrreeStat.Count = EstimateUtil.DivideOrZero(dStreetTrreeStat.Count * (double) key3.Size, divisor);
        }
        this.m_streetTreeStats.StdErr += dStreetTrreeStat.StdErr;
        this.m_streetTreeStats.Count += dStreetTrreeStat.Count;
        dStreetTrreeStat.StdErr = Math.Sqrt(dStreetTrreeStat.StdErr);
        dStreetTrreeStat.PctStrata = EstimateUtil.DivideOrZero(dStreetTrreeStat.Count * 100.0, this.m_dCwStats[key3].Count);
        dStreetTrreeStat.PctStrataSE = EstimateUtil.DivideOrZero(dStreetTrreeStat.StdErr * 100.0, this.m_dCwStats[key3].Count);
        dStreetTrreeStat.PctTrees = EstimateUtil.DivideOrZero(dStreetTrreeStat.Count * 100.0, this.m_cwTotal.Count);
        dStreetTrreeStat.PctTreesSE = EstimateUtil.DivideOrZero(dStreetTrreeStat.StdErr * 100.0, this.m_cwTotal.Count);
      }
      if (this.series.IsSample)
        this.m_streetTreeStats.StdErr = Math.Sqrt(this.m_streetTreeStats.StdErr);
      this.m_streetTreeStats.Pct = EstimateUtil.DivideOrZero(this.m_streetTreeStats.Count * 100.0, this.m_streetTreeStats.Count);
      this.m_streetTreeStats.PctSE = EstimateUtil.DivideOrZero(this.m_streetTreeStats.StdErr * 100.0, this.m_streetTreeStats.Count);
      this.m_streetTreeStats.PctTrees = EstimateUtil.DivideOrZero(this.m_streetTreeStats.Count * 100.0, this.m_cwTotal.Count);
      this.m_streetTreeStats.PctTreesSE = EstimateUtil.DivideOrZero(this.m_streetTreeStats.StdErr * 100.0, this.m_cwTotal.Count);
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair5 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dTotals2)
      {
        Strata key5 = keyValuePair5.Key;
        PublicPrivateStats nonStreetTreesStat = this.m_dNonStreetTreesStats[key5];
        int count = dTotals3[key5].Values.Count;
        double divisor = dictionary1[key5];
        double num7 = 0.0;
        foreach (KeyValuePair<Plot, int> keyValuePair6 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair5.Value)
        {
          Plot key6 = keyValuePair6.Key;
          nonStreetTreesStat.Count += (double) keyValuePair6.Value;
          num7 += (double) (keyValuePair6.Value * keyValuePair6.Value);
        }
        if (this.series.IsSample)
        {
          double num8 = EstimateUtil.DivideOrZero(num7 - EstimateUtil.DivideOrZero(nonStreetTreesStat.Count * nonStreetTreesStat.Count, (long) count), (long) (count - 1));
          double num9 = EstimateUtil.DivideOrZero((double) key5.Size * (double) count, divisor);
          nonStreetTreesStat.StdErr = EstimateUtil.DivideOrZero(num9 * (num9 - (double) count) * num8, (long) count);
          nonStreetTreesStat.Count = EstimateUtil.DivideOrZero(nonStreetTreesStat.Count * (double) key5.Size, divisor);
        }
        this.m_nonStreetTreeStats.StdErr += nonStreetTreesStat.StdErr;
        this.m_nonStreetTreeStats.Count += nonStreetTreesStat.Count;
        nonStreetTreesStat.StdErr = Math.Sqrt(nonStreetTreesStat.StdErr);
        nonStreetTreesStat.PctStrata = EstimateUtil.DivideOrZero(nonStreetTreesStat.Count * 100.0, this.m_dCwStats[key5].Count);
        nonStreetTreesStat.PctStrataSE = EstimateUtil.DivideOrZero(nonStreetTreesStat.StdErr * 100.0, this.m_dCwStats[key5].Count);
        nonStreetTreesStat.PctTrees = EstimateUtil.DivideOrZero(nonStreetTreesStat.Count * 100.0, this.m_cwTotal.Count);
        nonStreetTreesStat.PctTreesSE = EstimateUtil.DivideOrZero(nonStreetTreesStat.StdErr * 100.0, this.m_cwTotal.Count);
      }
      if (this.series.IsSample)
        this.m_nonStreetTreeStats.StdErr = Math.Sqrt(this.m_nonStreetTreeStats.StdErr);
      this.m_nonStreetTreeStats.Pct = EstimateUtil.DivideOrZero(this.m_nonStreetTreeStats.Count * 100.0, this.m_nonStreetTreeStats.Count);
      this.m_nonStreetTreeStats.PctSE = EstimateUtil.DivideOrZero(this.m_nonStreetTreeStats.StdErr * 100.0, this.m_nonStreetTreeStats.Count);
      this.m_nonStreetTreeStats.PctTrees = EstimateUtil.DivideOrZero(this.m_nonStreetTreeStats.Count * 100.0, this.m_cwTotal.Count);
      this.m_nonStreetTreeStats.PctTreesSE = EstimateUtil.DivideOrZero(this.m_nonStreetTreeStats.StdErr * 100.0, this.m_cwTotal.Count);
      foreach (Strata stratum in (IEnumerable<Strata>) this.m_strata)
      {
        this.m_dCwStats[stratum].Pct = EstimateUtil.DivideOrZero(this.m_dCwStats[stratum].Count * 100.0, this.m_cwTotal.Count);
        this.m_dCwStats[stratum].PctSE = EstimateUtil.DivideOrZero(this.m_dCwStats[stratum].StdErr * 100.0, this.m_cwTotal.Count);
        this.m_dStreetTrreeStats[stratum].Pct = EstimateUtil.DivideOrZero(this.m_dStreetTrreeStats[stratum].Count * 100.0, this.m_streetTreeStats.Count);
        this.m_dStreetTrreeStats[stratum].PctSE = EstimateUtil.DivideOrZero(this.m_dStreetTrreeStats[stratum].StdErr * 100.0, this.m_streetTreeStats.Count);
        this.m_dNonStreetTreesStats[stratum].Pct = EstimateUtil.DivideOrZero(this.m_dNonStreetTreesStats[stratum].Count * 100.0, this.m_nonStreetTreeStats.Count);
        this.m_dNonStreetTreesStats[stratum].PctSE = EstimateUtil.DivideOrZero(this.m_dNonStreetTreesStats[stratum].StdErr * 100.0, this.m_nonStreetTreeStats.Count);
      }
      this.m_calculated = true;
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

    public override void RenderBody(C1PrintDocument C1doc, Graphics FormGraphics)
    {
      if (!this.m_calculated)
        this.CalculateStats();
      C1doc.ClipPage = true;
      C1doc.Body.Children.Add((RenderObject) this.CreateStrataTable(this.m_dStreetTrreeStats, this.m_streetTreeStats, i_Tree_Eco_v6.Resources.Strings.SummaryOfStreetTrees, i_Tree_Eco_v6.Resources.Strings.PercentOfStreetTrees));
      C1doc.Body.Children.Add((RenderObject) this.CreateStrataTable(this.m_dNonStreetTreesStats, this.m_nonStreetTreeStats, i_Tree_Eco_v6.Resources.Strings.SummaryOfNonStreetTrees, i_Tree_Eco_v6.Resources.Strings.PercentOfNonStreetTrees));
      RenderTable renderTable = new RenderTable();
    }

    public override void SetAlignment(C1PrintDocument C1doc) => C1doc.Style.FlowAlignChildren = FlowAlignEnum.Near;

    private RenderTable CreateStrataTable(
      IDictionary<Strata, PublicPrivateStats> dStats,
      PublicPrivateStats totStats,
      string mainHeader,
      string colHeader)
    {
      RenderTable rt = new RenderTable();
      rt.Style.FontSize = 10f;
      rt.Style.Spacing.Top = (Unit) "1ls";
      rt.Width = (Unit) "100%";
      rt.Cols[0].Width = (Unit) "14%";
      rt.Cols[1].Width = (Unit) "17%";
      rt.Cols[2].Width = (Unit) "10%";
      rt.Cols[3].Width = (Unit) "13%";
      rt.Cols[4].Width = (Unit) "5%";
      rt.Cols[5].Width = (Unit) "19%";
      rt.Cols[6].Width = (Unit) "5%";
      rt.Cols[7].Width = (Unit) "13%";
      rt.Cols[8].Width = (Unit) "5%";
      rt.Cols[2].Visible = rt.Cols[4].Visible = rt.Cols[6].Visible = rt.Cols[8].Visible = this.series.IsSample;
      int count = 2;
      rt.RowGroups[0, count].Header = TableHeaderEnum.Page;
      rt.Rows[0].Style.FontSize = 16f;
      rt.Rows[1].Style.FontSize = 11f;
      rt.Rows[0].Style.Borders.Bottom = LineDef.Default;
      rt.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      rt.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
      rt.Cells[0, 0].SpanCols = 9;
      rt.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Center;
      rt.Cells[0, 0].Style.TextColor = Color.FromArgb(85, 133, 191);
      rt.Cells[0, 0].Text = mainHeader;
      rt.Cells[1, 0].Text = v6Strings.Strata_SingularName;
      rt.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
      rt.Cells[1, 3].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfStrataTrees;
      rt.Cells[1, 5].Text = colHeader;
      rt.Cells[1, 7].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfAllTrees;
      rt.Cells[1, 2].Text = rt.Cells[1, 4].Text = rt.Cells[1, 6].Text = rt.Cells[1, 8].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      int num = count;
      foreach (KeyValuePair<Strata, PublicPrivateStats> dStat in (IEnumerable<KeyValuePair<Strata, PublicPrivateStats>>) dStats)
      {
        Strata key = dStat.Key;
        PublicPrivateStats publicPrivateStats = dStat.Value;
        rt.Cells[num, 0].Text = dStats.Count != 1 || this.curYear.RecordStrata ? key.Description : i_Tree_Eco_v6.Resources.Strings.StudyArea;
        rt.Cells[num, 1].Text = string.Format("{0:N0}", (object) publicPrivateStats.Count);
        rt.Cells[num, 2].Text = string.Format("(±{0:N0})", (object) publicPrivateStats.StdErr);
        rt.Cells[num, 3].Text = string.Format("{0:F1}", (object) publicPrivateStats.PctStrata);
        rt.Cells[num, 4].Text = string.Format("{0:F1}", (object) publicPrivateStats.PctStrataSE);
        rt.Cells[num, 5].Text = string.Format("{0:F1}", (object) publicPrivateStats.Pct);
        rt.Cells[num, 6].Text = string.Format("{0:F1}", (object) publicPrivateStats.PctSE);
        rt.Cells[num, 7].Text = string.Format("{0:F1}", (object) publicPrivateStats.PctTrees);
        rt.Cells[num, 8].Text = string.Format("{0:F1}", (object) publicPrivateStats.PctTreesSE);
        ++num;
      }
      if (dStats.Count > 1)
      {
        rt.Rows[num].Style.Borders.Top = LineDef.Default;
        rt.Rows[num].Style.FontBold = true;
        rt.Cells[num, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        rt.Cells[num, 1].Text = string.Format("{0:N0}", (object) totStats.Count);
        rt.Cells[num, 2].Text = string.Format("(±{0:N0})", (object) totStats.StdErr);
        rt.Cells[num, 5].Text = string.Format("{0:F1}", (object) totStats.Pct);
        rt.Cells[num, 6].Text = string.Format("{0:F1}", (object) totStats.PctSE);
        rt.Cells[num, 7].Text = string.Format("{0:F1}", (object) totStats.PctTrees);
        rt.Cells[num, 8].Text = string.Format("{0:F1}", (object) totStats.PctTreesSE);
      }
      ReportUtil.FormatRenderTable(rt);
      return rt;
    }
  }
}
