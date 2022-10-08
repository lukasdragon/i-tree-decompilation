// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PublicPrivate
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate.Extensions;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class PublicPrivate : DatabaseReport
  {
    private IList<Strata> m_strata;
    private IList<Tree> m_trees;
    private IList<Plot> m_plots;
    private PublicPrivateStats m_publicStats;
    private PublicPrivateStats m_privateStats;
    private PublicPrivateStats m_unknownStats;
    private PublicPrivateStats m_cwTotal;
    private IDictionary<Strata, PublicPrivateStats> m_dCwStats;
    private IDictionary<Strata, PublicPrivateStats> m_dPublicStats;
    private IDictionary<Strata, PublicPrivateStats> m_dPrivateStats;
    private IDictionary<Strata, PublicPrivateStats> m_dUnknownStats;
    private bool m_calculated;

    public PublicPrivate()
    {
      this.m_calculated = false;
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitlePopulationSummaryOfPublicAndPrivateTreesByStratum;
      this.Init();
    }

    private void Init()
    {
      using (this.curInputISession.BeginTransaction())
      {
        this.curYear = this.curInputISession.Get<Year>((object) ReportBase.m_ps.InputSession.YearKey);
        this.series = this.curYear.Series;
        this.m_strata = this.curInputISession.CreateCriteria<Strata>().Add((ICriterion) Restrictions.Eq("Year", (object) this.curYear)).AddOrder(Order.Asc("Description")).List<Strata>();
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
    }

    private void CalculateStats()
    {
      IDictionary<Strata, IDictionary<Plot, int>> dTotals1 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      IDictionary<Strata, IDictionary<Plot, int>> dTotals2 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      IDictionary<Strata, IDictionary<Plot, int>> dictionary1 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      IDictionary<Strata, IDictionary<Plot, int>> dTotals3 = (IDictionary<Strata, IDictionary<Plot, int>>) new Dictionary<Strata, IDictionary<Plot, int>>();
      this.m_dPublicStats = (IDictionary<Strata, PublicPrivateStats>) new Dictionary<Strata, PublicPrivateStats>();
      this.m_dPrivateStats = (IDictionary<Strata, PublicPrivateStats>) new Dictionary<Strata, PublicPrivateStats>();
      this.m_dUnknownStats = (IDictionary<Strata, PublicPrivateStats>) new Dictionary<Strata, PublicPrivateStats>();
      this.m_dCwStats = (IDictionary<Strata, PublicPrivateStats>) new Dictionary<Strata, PublicPrivateStats>();
      this.m_publicStats = new PublicPrivateStats();
      this.m_privateStats = new PublicPrivateStats();
      this.m_unknownStats = new PublicPrivateStats();
      this.m_cwTotal = new PublicPrivateStats();
      foreach (Plot plot in (IEnumerable<Plot>) this.m_plots)
      {
        IDictionary<Plot, int> dictionary2 = (IDictionary<Plot, int>) null;
        if (!dTotals3.TryGetValue(plot.Strata, out dictionary2))
        {
          dictionary2 = (IDictionary<Plot, int>) new Dictionary<Plot, int>();
          dTotals3[plot.Strata] = dictionary2;
        }
        dictionary2[plot] = 0;
      }
      foreach (Strata stratum in (IEnumerable<Strata>) this.m_strata)
      {
        this.m_dPublicStats[stratum] = new PublicPrivateStats();
        this.m_dPrivateStats[stratum] = new PublicPrivateStats();
        this.m_dUnknownStats[stratum] = new PublicPrivateStats();
        this.m_dCwStats[stratum] = new PublicPrivateStats();
      }
      foreach (Tree tree in (IEnumerable<Tree>) this.m_trees)
      {
        if (tree.Species != null && ReportBase.m_ps.Species.ContainsKey(tree.Species))
        {
          short? nullable1 = tree.CityManaged;
          short? nullable2;
          if (!nullable1.HasValue)
          {
            nullable2 = tree.CityManaged;
          }
          else
          {
            nullable1 = tree.CityManaged;
            int? nullable3 = nullable1.HasValue ? new int?((int) nullable1.GetValueOrDefault()) : new int?();
            int num = 1;
            if (!nullable3.HasValue)
            {
              nullable1 = new short?();
              nullable2 = nullable1;
            }
            else
              nullable2 = new short?((short) (nullable3.GetValueOrDefault() & num));
          }
          short? nullable4 = nullable2;
          this.IncStrataCount(dTotals3, true, tree.Plot.Strata, tree.Plot);
          this.IncStrataCount(dTotals1, nullable4.HasValue && nullable4.Value == (short) 0, tree.Plot.Strata, tree.Plot);
          this.IncStrataCount(dTotals2, nullable4.HasValue && nullable4.Value == (short) 1, tree.Plot.Strata, tree.Plot);
          IDictionary<Strata, IDictionary<Plot, int>> dTotals4 = dictionary1;
          nullable1 = tree.CityManaged;
          int num1 = !nullable1.HasValue ? 1 : 0;
          Strata strata = tree.Plot.Strata;
          Plot plot = tree.Plot;
          this.IncStrataCount(dTotals4, num1 != 0, strata, plot);
        }
      }
      IDictionary<Strata, double> dictionary3 = (IDictionary<Strata, double>) new Dictionary<Strata, double>();
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair1 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dTotals3)
      {
        Strata key1 = keyValuePair1.Key;
        PublicPrivateStats dCwStat = this.m_dCwStats[key1];
        int count = keyValuePair1.Value.Count;
        double divisor = 0.0;
        double num2 = 0.0;
        foreach (KeyValuePair<Plot, int> keyValuePair2 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair1.Value)
        {
          Plot key2 = keyValuePair2.Key;
          divisor += (double) key2.Size * (double) key2.PercentMeasured / 100.0;
          dCwStat.Count += (double) keyValuePair2.Value;
          num2 += (double) (keyValuePair2.Value * keyValuePair2.Value);
        }
        if (this.series.IsSample)
        {
          double num3 = EstimateUtil.DivideOrZero(num2 - EstimateUtil.DivideOrZero(dCwStat.Count * dCwStat.Count, (long) count), (long) (count - 1));
          double num4 = EstimateUtil.DivideOrZero((double) key1.Size * (double) count, divisor);
          dCwStat.StdErr = Math.Sqrt(EstimateUtil.DivideOrZero(num4 * (num4 - (double) count) * num3, (long) count));
          dCwStat.Count = EstimateUtil.DivideOrZero(dCwStat.Count * (double) key1.Size, divisor);
          this.m_cwTotal.StdErr += EstimateUtil.DivideOrZero(num4 * (num4 - (double) count) * num3, (long) count);
          this.m_cwTotal.Count += dCwStat.Count;
        }
        dictionary3[key1] = divisor;
      }
      if (this.series.IsSample)
        this.m_cwTotal.StdErr = Math.Sqrt(this.m_cwTotal.StdErr);
      this.m_cwTotal.Pct = 100.0;
      this.m_cwTotal.PctTrees = 100.0;
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair3 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dTotals1)
      {
        Strata key3 = keyValuePair3.Key;
        PublicPrivateStats dPublicStat = this.m_dPublicStats[key3];
        int count = dTotals3[key3].Values.Count;
        double divisor = dictionary3[key3];
        double num5 = 0.0;
        foreach (KeyValuePair<Plot, int> keyValuePair4 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair3.Value)
        {
          Plot key4 = keyValuePair4.Key;
          dPublicStat.Count += (double) keyValuePair4.Value;
          num5 += (double) (keyValuePair4.Value * keyValuePair4.Value);
        }
        if (this.series.IsSample)
        {
          double num6 = EstimateUtil.DivideOrZero(num5 - EstimateUtil.DivideOrZero(dPublicStat.Count * dPublicStat.Count, (long) count), (long) (count - 1));
          double num7 = EstimateUtil.DivideOrZero((double) key3.Size * (double) count, divisor);
          dPublicStat.StdErr = EstimateUtil.DivideOrZero(num7 * (num7 - (double) count) * num6, (long) count);
          dPublicStat.Count = EstimateUtil.DivideOrZero(dPublicStat.Count * (double) key3.Size, divisor);
        }
        this.m_publicStats.StdErr += dPublicStat.StdErr;
        this.m_publicStats.Count += dPublicStat.Count;
        dPublicStat.StdErr = Math.Sqrt(dPublicStat.StdErr);
        dPublicStat.PctStrata = EstimateUtil.DivideOrZero(dPublicStat.Count * 100.0, this.m_dCwStats[key3].Count);
        dPublicStat.PctStrataSE = EstimateUtil.DivideOrZero(dPublicStat.StdErr * 100.0, this.m_dCwStats[key3].Count);
        dPublicStat.PctTrees = EstimateUtil.DivideOrZero(dPublicStat.Count * 100.0, this.m_cwTotal.Count);
        dPublicStat.PctTreesSE = EstimateUtil.DivideOrZero(dPublicStat.StdErr * 100.0, this.m_cwTotal.Count);
      }
      if (this.series.IsSample)
        this.m_publicStats.StdErr = Math.Sqrt(this.m_publicStats.StdErr);
      this.m_publicStats.Pct = EstimateUtil.DivideOrZero(this.m_publicStats.Count * 100.0, this.m_publicStats.Count);
      this.m_publicStats.PctSE = EstimateUtil.DivideOrZero(this.m_publicStats.StdErr * 100.0, this.m_publicStats.Count);
      this.m_publicStats.PctTrees = EstimateUtil.DivideOrZero(this.m_publicStats.Count * 100.0, this.m_cwTotal.Count);
      this.m_publicStats.PctTreesSE = EstimateUtil.DivideOrZero(this.m_publicStats.StdErr * 100.0, this.m_cwTotal.Count);
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair5 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dTotals2)
      {
        Strata key5 = keyValuePair5.Key;
        PublicPrivateStats dPrivateStat = this.m_dPrivateStats[key5];
        int count = dTotals3[key5].Values.Count;
        double divisor = dictionary3[key5];
        double num8 = 0.0;
        foreach (KeyValuePair<Plot, int> keyValuePair6 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair5.Value)
        {
          Plot key6 = keyValuePair6.Key;
          dPrivateStat.Count += (double) keyValuePair6.Value;
          num8 += (double) (keyValuePair6.Value * keyValuePair6.Value);
        }
        if (this.series.IsSample)
        {
          double num9 = EstimateUtil.DivideOrZero(num8 - EstimateUtil.DivideOrZero(dPrivateStat.Count * dPrivateStat.Count, (long) count), (long) (count - 1));
          double num10 = EstimateUtil.DivideOrZero((double) key5.Size * (double) count, divisor);
          dPrivateStat.StdErr = EstimateUtil.DivideOrZero(num10 * (num10 - (double) count) * num9, (long) count);
          dPrivateStat.Count = EstimateUtil.DivideOrZero(dPrivateStat.Count * (double) key5.Size, divisor);
        }
        this.m_privateStats.StdErr += dPrivateStat.StdErr;
        this.m_privateStats.Count += dPrivateStat.Count;
        dPrivateStat.StdErr = Math.Sqrt(dPrivateStat.StdErr);
        dPrivateStat.PctStrata = EstimateUtil.DivideOrZero(dPrivateStat.Count * 100.0, this.m_dCwStats[key5].Count);
        dPrivateStat.PctStrataSE = EstimateUtil.DivideOrZero(dPrivateStat.StdErr * 100.0, this.m_dCwStats[key5].Count);
        dPrivateStat.PctTrees = EstimateUtil.DivideOrZero(dPrivateStat.Count * 100.0, this.m_cwTotal.Count);
        dPrivateStat.PctTreesSE = EstimateUtil.DivideOrZero(dPrivateStat.StdErr * 100.0, this.m_cwTotal.Count);
      }
      if (this.series.IsSample)
        this.m_privateStats.StdErr = Math.Sqrt(this.m_privateStats.StdErr);
      this.m_privateStats.Pct = EstimateUtil.DivideOrZero(this.m_privateStats.Count * 100.0, this.m_privateStats.Count);
      this.m_privateStats.PctSE = EstimateUtil.DivideOrZero(this.m_privateStats.StdErr * 100.0, this.m_privateStats.Count);
      this.m_privateStats.PctTrees = EstimateUtil.DivideOrZero(this.m_privateStats.Count * 100.0, this.m_cwTotal.Count);
      this.m_privateStats.PctTreesSE = EstimateUtil.DivideOrZero(this.m_privateStats.StdErr * 100.0, this.m_cwTotal.Count);
      foreach (KeyValuePair<Strata, IDictionary<Plot, int>> keyValuePair7 in (IEnumerable<KeyValuePair<Strata, IDictionary<Plot, int>>>) dictionary1)
      {
        Strata key7 = keyValuePair7.Key;
        PublicPrivateStats dUnknownStat = this.m_dUnknownStats[key7];
        int count = dTotals3[key7].Values.Count;
        double divisor = dictionary3[key7];
        double num11 = 0.0;
        foreach (KeyValuePair<Plot, int> keyValuePair8 in (IEnumerable<KeyValuePair<Plot, int>>) keyValuePair7.Value)
        {
          Plot key8 = keyValuePair8.Key;
          dUnknownStat.Count += (double) keyValuePair8.Value;
          num11 += (double) (keyValuePair8.Value * keyValuePair8.Value);
        }
        if (this.series.IsSample)
        {
          double num12 = EstimateUtil.DivideOrZero(num11 - EstimateUtil.DivideOrZero(dUnknownStat.Count * dUnknownStat.Count, (long) count), (long) (count - 1));
          double num13 = EstimateUtil.DivideOrZero((double) key7.Size * (double) count, divisor);
          dUnknownStat.StdErr = EstimateUtil.DivideOrZero(num13 * (num13 - (double) count) * num12, (long) count);
          dUnknownStat.Count = EstimateUtil.DivideOrZero(dUnknownStat.Count * (double) key7.Size, divisor);
        }
        this.m_unknownStats.StdErr += dUnknownStat.StdErr;
        this.m_unknownStats.Count += dUnknownStat.Count;
        dUnknownStat.StdErr = Math.Sqrt(dUnknownStat.StdErr);
        dUnknownStat.PctStrata = EstimateUtil.DivideOrZero(dUnknownStat.Count * 100.0, this.m_dCwStats[key7].Count);
        dUnknownStat.PctStrataSE = EstimateUtil.DivideOrZero(dUnknownStat.StdErr * 100.0, this.m_dCwStats[key7].Count);
        dUnknownStat.PctTrees = EstimateUtil.DivideOrZero(dUnknownStat.Count * 100.0, this.m_cwTotal.Count);
        dUnknownStat.PctTreesSE = EstimateUtil.DivideOrZero(dUnknownStat.StdErr * 100.0, this.m_cwTotal.Count);
      }
      if (this.series.IsSample)
        this.m_unknownStats.StdErr = Math.Sqrt(this.m_unknownStats.StdErr);
      this.m_unknownStats.Pct = EstimateUtil.DivideOrZero(this.m_unknownStats.Count * 100.0, this.m_unknownStats.Count);
      this.m_unknownStats.PctSE = EstimateUtil.DivideOrZero(this.m_unknownStats.StdErr * 100.0, this.m_unknownStats.Count);
      this.m_unknownStats.PctTrees = EstimateUtil.DivideOrZero(this.m_unknownStats.Count * 100.0, this.m_cwTotal.Count);
      this.m_unknownStats.PctTreesSE = EstimateUtil.DivideOrZero(this.m_unknownStats.StdErr * 100.0, this.m_cwTotal.Count);
      foreach (Strata stratum in (IEnumerable<Strata>) this.m_strata)
      {
        this.m_dCwStats[stratum].Pct = EstimateUtil.DivideOrZero(this.m_dCwStats[stratum].Count * 100.0, this.m_cwTotal.Count);
        this.m_dCwStats[stratum].PctSE = EstimateUtil.DivideOrZero(this.m_dCwStats[stratum].StdErr * 100.0, this.m_cwTotal.Count);
        this.m_dPublicStats[stratum].Pct = EstimateUtil.DivideOrZero(this.m_dPublicStats[stratum].Count * 100.0, this.m_publicStats.Count);
        this.m_dPublicStats[stratum].PctSE = EstimateUtil.DivideOrZero(this.m_dPublicStats[stratum].StdErr * 100.0, this.m_publicStats.Count);
        this.m_dPrivateStats[stratum].Pct = EstimateUtil.DivideOrZero(this.m_dPrivateStats[stratum].Count * 100.0, this.m_privateStats.Count);
        this.m_dPrivateStats[stratum].PctSE = EstimateUtil.DivideOrZero(this.m_dPrivateStats[stratum].StdErr * 100.0, this.m_privateStats.Count);
        this.m_dUnknownStats[stratum].Pct = EstimateUtil.DivideOrZero(this.m_dUnknownStats[stratum].Count * 100.0, this.m_unknownStats.Count);
        this.m_dUnknownStats[stratum].PctSE = EstimateUtil.DivideOrZero(this.m_dUnknownStats[stratum].StdErr * 100.0, this.m_unknownStats.Count);
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

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      if (!this.m_calculated)
        this.CalculateStats();
      C1doc.Body.Children.Add((RenderObject) this.CreateStrataTable(this.m_dPublicStats, this.m_publicStats, i_Tree_Eco_v6.Resources.Strings.SummaryOfPublicTrees, i_Tree_Eco_v6.Resources.Strings.PercentOfPublicTrees));
      C1doc.Body.Children.Add((RenderObject) this.CreateStrataTable(this.m_dPrivateStats, this.m_privateStats, i_Tree_Eco_v6.Resources.Strings.SummaryOfPrivateTrees, i_Tree_Eco_v6.Resources.Strings.PercentOfPrivateTrees));
      if (this.m_unknownStats.Count <= 0.0)
        return;
      C1doc.Body.Children.Add((RenderObject) this.CreateStrataTable(this.m_dUnknownStats, this.m_unknownStats, i_Tree_Eco_v6.Resources.Strings.SummaryOfUndefinedTrees, i_Tree_Eco_v6.Resources.Strings.PercentOfUndefinedTrees));
    }

    private RenderTable CreateStrataTable(
      IDictionary<Strata, PublicPrivateStats> dStats,
      PublicPrivateStats totStats,
      string mainHeader,
      string colHeader)
    {
      RenderTable rt = new RenderTable();
      rt.Style.Spacing.Top = (Unit) "1ls";
      rt.Style.FontSize = 10f;
      rt.Width = (Unit) "100%";
      rt.Cols[0].Width = (Unit) "28%";
      rt.Cols[1].Width = (Unit) "10%";
      rt.Cols[2].Width = (Unit) "10%";
      rt.Cols[3].Width = (Unit) "13%";
      rt.Cols[4].Width = (Unit) "5%";
      rt.Cols[5].Width = (Unit) "14%";
      rt.Cols[6].Width = (Unit) "5%";
      rt.Cols[7].Width = (Unit) "13%";
      rt.Cols[8].Width = (Unit) "5%";
      int count = 2;
      rt.RowGroups[0, count].Header = TableHeaderEnum.Page;
      rt.Rows[0].Style.FontSize = 16f;
      rt.Rows[1].Style.FontSize = 11f;
      rt.Rows[0].Style.Borders.Bottom = LineDef.Default;
      rt.Cells[0, 0].SpanCols = 9;
      rt.Cells[0, 0].Style.TextAlignHorz = AlignHorzEnum.Center;
      rt.Cells[0, 0].Style.TextColor = Color.FromArgb(85, 133, 191);
      rt.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      rt.Cols[2].Style.TextAlignHorz = AlignHorzEnum.Left;
      rt.Cols[2].Visible = this.series.IsSample;
      rt.Cols[4].Visible = this.series.IsSample;
      rt.Cols[6].Visible = this.series.IsSample;
      rt.Cols[8].Visible = this.series.IsSample;
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
