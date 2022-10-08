// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.SpeciesRichnessShannonWienerDiversityIndexByStrata
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Numerics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace i_Tree_Eco_v6.Reports
{
  internal class SpeciesRichnessShannonWienerDiversityIndexByStrata : DatabaseReport
  {
    public SpeciesRichnessShannonWienerDiversityIndexByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.DiversityIndicesByStratum;

    private void AddDefinition(RenderParagraph rPara, string term, string def)
    {
      ParagraphText po = new ParagraphText(term);
      po.Style.FontBold = true;
      rPara.Content.Add((ParagraphObject) po);
      rPara.Content.Add((ParagraphObject) new ParagraphText()
      {
        Text = string.Format(": {0}{1}", (object) def, (object) Environment.NewLine)
      });
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      Dictionary<Strata, SortedList<string, int>> dictionary1 = new Dictionary<Strata, SortedList<string, int>>();
      Dictionary<Strata, double> dictionary2 = new Dictionary<Strata, double>();
      SortedList<string, int> sortedList1 = new SortedList<string, int>();
      object[] source = new object[5]
      {
        (object) TreeStatus.InitialSample,
        (object) TreeStatus.Ingrowth,
        (object) TreeStatus.NoChange,
        (object) TreeStatus.Planted,
        (object) TreeStatus.Unknown
      };
      List<Strata> strataList = new List<Strata>();
      foreach (Strata stratum in (IEnumerable<Strata>) this.curYear.Strata)
      {
        double num = stratum.Plots.Where<Plot>((Func<Plot, bool>) (p => p.IsComplete)).Sum<Plot>((Func<Plot, double>) (p => (double) p.Size * (double) p.PercentMeasured / 100.0));
        if (num > 0.0)
        {
          dictionary1.Add(stratum, new SortedList<string, int>());
          strataList.Add(stratum);
          if (this.curYear.Unit == YearUnit.Metric)
            dictionary2.Add(stratum, num);
          else
            dictionary2.Add(stratum, num * 0.404686);
          foreach (Plot plot in (IEnumerable<Plot>) stratum.Plots)
          {
            foreach (Tree tree in !this.curYear.RecordCrownCondition ? (IEnumerable<Tree>) plot.Trees : plot.Trees.Where<Tree>((Func<Tree, bool>) (t => t.Crown.Condition.PctDieback != 100.0)))
            {
              if (((IEnumerable<object>) source).Contains<object>((object) (TreeStatus) tree.Status))
              {
                string species = tree.Species;
                if (!dictionary1[stratum].ContainsKey(species))
                  dictionary1[stratum].Add(species, 0);
                ++dictionary1[stratum][species];
                if (!sortedList1.ContainsKey(species))
                  sortedList1.Add(species, 0);
                ++sortedList1[species];
              }
            }
          }
        }
      }
      strataList.Sort((Comparison<Strata>) ((x, y) => x.Id.CompareTo(y.Id)));
      Enumerable.Sum(dictionary2.Values);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Width = (Unit) "100%";
      renderTable.Cols[0].Width = (Unit) "14%";
      int count1 = 1;
      renderTable.RowGroups[0, count1].Header = TableHeaderEnum.Page;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.Richness;
      renderTable.Cells[0, 2].Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) i_Tree_Eco_v6.Resources.Strings.SPP, (object) ReportBase.HaUnits());
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.Shannon;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.Menhinick;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.Simpson;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.Evenness;
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.Rarefaction;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      int k = -1;
      foreach (Strata key in dictionary1.Keys)
      {
        int num = Enumerable.Sum(dictionary1[key].Values);
        for (int index = 0; index < dictionary1[key].Count; ++index)
        {
          if (k == -1 && num - dictionary1[key].Values[index] > 1)
            k = num - dictionary1[key].Values[index];
          if (num - dictionary1[key].Values[index] > 1 && k > num - dictionary1[key].Values[index])
            k = num - dictionary1[key].Values[index];
        }
      }
      int num1 = count1;
      for (int index1 = 0; index1 < dictionary2.Count; ++index1)
      {
        Strata key1 = strataList[index1];
        SortedList<string, int> sortedList2 = dictionary1[key1];
        renderTable.Cells[num1, 0].Text = this.curYear.RecordStrata ? key1.Description : i_Tree_Eco_v6.Resources.Strings.StudyArea;
        int count2 = sortedList2.Keys.Count;
        renderTable.Cells[num1, 1].Text = count2.ToString("N0");
        double english = EstimateUtil.ConvertToEnglish(dictionary2[key1], Units.Hectare, ReportBase.EnglishUnits);
        double num2 = (double) count2 / english;
        renderTable.Cells[num1, 2].Text = num2.ToString("N1");
        int num3 = Enumerable.Sum(sortedList2.Values);
        double num4 = 0.0;
        for (int index2 = 0; index2 < sortedList2.Keys.Count; ++index2)
        {
          string key2 = sortedList2.Keys[index2];
          double d = (double) sortedList2[key2] / (double) num3;
          num4 += d * Math.Log(d);
        }
        double num5 = num4 * -1.0;
        renderTable.Cells[num1, 3].Text = num5.ToString("N1");
        double num6 = Math.Log((double) count2);
        double num7 = num5 / num6;
        if (num3 == 0)
        {
          renderTable.Cells[num1, 4].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        }
        else
        {
          double num8 = (double) count2 / Math.Sqrt((double) num3);
          renderTable.Cells[num1, 4].Text = num8.ToString("N1");
        }
        double num9 = 0.0;
        if (count2 > 1)
        {
          for (int index3 = 0; index3 < sortedList2.Keys.Count; ++index3)
          {
            string key3 = sortedList2.Keys[index3];
            double num10 = (double) sortedList2[key3];
            num9 += num10 * (num10 - 1.0) / (double) (num3 * (num3 - 1));
          }
        }
        if (num9 > 0.0)
        {
          double num11 = 1.0 / num9;
          renderTable.Cells[num1, 5].Text = num11.ToString("N1");
        }
        else
          renderTable.Cells[num1, 5].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        renderTable.Cells[num1, 6].Text = num7.ToString("N1");
        if (this.curYear.RecordStrata && this.curYear.Strata.Count > 1)
        {
          BigRational bigRational1 = (BigRational) 0;
          BigRational bigRational2 = (BigRational) this.BinC((BigInteger) num3, (BigInteger) k);
          for (int index4 = 0; index4 < sortedList2.Keys.Count; ++index4)
          {
            string key4 = sortedList2.Keys[index4];
            int num12 = sortedList2[key4];
            BigRational bigRational3 = (BigRational) this.BinC((BigInteger) (num3 - num12), (BigInteger) k);
            if (bigRational3 > (BigRational) 0)
              bigRational1 += (BigRational) 1 - bigRational3 / bigRational2;
          }
          renderTable.Cells[num1, 7].Text = ((double) bigRational1).ToString("N1");
        }
        else
          renderTable.Cells[num1, 7].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        ++num1;
      }
      if (this.curYear.RecordStrata && this.curYear.Strata.Count > 1)
      {
        renderTable.Rows[num1].Style.Borders.Top = LineDef.Default;
        renderTable.Rows[num1].Style.FontBold = true;
        renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
        double count3 = (double) sortedList1.Count;
        renderTable.Cells[num1, 1].Text = count3.ToString("N0");
        double num13 = Enumerable.Sum(dictionary2.Values);
        renderTable.Cells[num1, 2].Text = (count3 / num13).ToString("N1");
        double num14 = 0.0;
        int d1 = Enumerable.Sum(sortedList1.Values);
        for (int index = 0; index < sortedList1.Count; ++index)
        {
          string key = sortedList1.Keys[index];
          double d2 = (double) sortedList1[key] / (double) d1;
          num14 += d2 * Math.Log(d2);
        }
        double num15 = num14 * -1.0;
        renderTable.Cells[num1, 3].Text = num15.ToString("N1");
        if (d1 == 0)
        {
          renderTable.Cells[num1, 4].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        }
        else
        {
          double num16 = count3 / Math.Sqrt((double) d1);
          renderTable.Cells[num1, 4].Text = num16.ToString("N1");
        }
        double num17 = 0.0;
        if (count3 > 0.0)
        {
          for (int index = 0; index < sortedList1.Count; ++index)
          {
            string key = sortedList1.Keys[index];
            double num18 = (double) sortedList1[key];
            num17 += num18 * (num18 - 1.0) / (double) (d1 * (d1 - 1));
          }
        }
        if (num17 > 0.0)
        {
          double num19 = 1.0 / num17;
          renderTable.Cells[num1, 5].Text = num19.ToString("N1");
        }
        else
          renderTable.Cells[num1, 5].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
        double num20 = Math.Log(count3);
        double num21 = num15 / num20;
        renderTable.Cells[num1, 6].Text = num21.ToString("N1");
        renderTable.Cells[num1, 7].Text = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr;
      }
      ReportUtil.FormatRenderTable(renderTable);
      RenderParagraph renderParagraph = new RenderParagraph();
      renderParagraph.Style.Spacing.Top = (Unit) "1ls";
      this.AddDefinition(renderParagraph, i_Tree_Eco_v6.Resources.Strings.Richness, i_Tree_Eco_v6.Resources.Strings.RichnessDefinition);
      this.AddDefinition(renderParagraph, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) i_Tree_Eco_v6.Resources.Strings.SPP, (object) ReportBase.HaUnits()), string.Format(i_Tree_Eco_v6.Resources.Strings.SPPDefinition, (object) ReportBase.HectarUnits()));
      this.AddDefinition(renderParagraph, i_Tree_Eco_v6.Resources.Strings.Shannon, i_Tree_Eco_v6.Resources.Strings.ShannonDefinition);
      this.AddDefinition(renderParagraph, i_Tree_Eco_v6.Resources.Strings.Menhinick, i_Tree_Eco_v6.Resources.Strings.MenhinickDefinition);
      this.AddDefinition(renderParagraph, i_Tree_Eco_v6.Resources.Strings.Simpson, i_Tree_Eco_v6.Resources.Strings.SimpsonDefinition);
      this.AddDefinition(renderParagraph, i_Tree_Eco_v6.Resources.Strings.Evenness, i_Tree_Eco_v6.Resources.Strings.EvennessDefinition);
      this.AddDefinition(renderParagraph, i_Tree_Eco_v6.Resources.Strings.Rarefaction, i_Tree_Eco_v6.Resources.Strings.RarefactionDefinition);
      renderParagraph.Style.TextAlignHorz = AlignHorzEnum.Left;
      C1doc.Body.Children.Add((RenderObject) renderParagraph);
    }

    private BigInteger BinC(BigInteger n, BigInteger k)
    {
      if (k < 0L || k > n)
        return (BigInteger) 0;
      if (k == 0L || k == n)
        return (BigInteger) 1;
      if (n - k < k)
        k = n - k;
      BigInteger bigInteger1 = (BigInteger) 1;
      BigInteger bigInteger2 = (BigInteger) 0;
      while (bigInteger2 < k)
      {
        bigInteger1 = bigInteger1 * (n - bigInteger2) / (bigInteger2 + (BigInteger) 1);
        ++bigInteger2;
      }
      return bigInteger1;
    }
  }
}
