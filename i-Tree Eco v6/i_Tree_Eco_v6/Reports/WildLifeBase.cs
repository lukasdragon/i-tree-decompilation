// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.WildLifeBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.v6;
using Eco.Util.Convert;
using LocationSpecies.Domain;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class WildLifeBase : DatabaseReport
  {
    public List<PlotWildLifeSuitability> SuitabilityIndexByPlots = new List<PlotWildLifeSuitability>();
    public List<StrataWildLifeSuitability> SuitabilityIndexByStrata = new List<StrataWildLifeSuitability>();
    public List<StrataWildLifeSuitability> SuitabilityIndexByStrataForTotal = new List<StrataWildLifeSuitability>();

    public WildLifeBase()
    {
      IList<Wildlife> wildlife = this.GetWildlife();
      if (wildlife.Count == 0)
        return;
      Dictionary<string, double> dictionary1 = new Dictionary<string, double>();
      Dictionary<string, double> dictionary2 = new Dictionary<string, double>();
      Dictionary<int, PlotInfo> dictionary3 = new Dictionary<int, PlotInfo>();
      List<GroundCover> gcBuildings = this.curYear.GroundCovers.Where<GroundCover>((Func<GroundCover, bool>) (g => g.CoverTypeId == 1)).ToList<GroundCover>();
      List<GroundCover> gcGrasses = this.curYear.GroundCovers.Where<GroundCover>((Func<GroundCover, bool>) (g => g.CoverTypeId == 8)).ToList<GroundCover>();
      IList<Plot> plotList = this.curInputISession.QueryOver<Plot>().Where((System.Linq.Expressions.Expression<Func<Plot, bool>>) (p => p.Year.Guid == this.curYear.Guid && p.IsComplete)).List<Plot>();
      double num1 = 0.0;
      foreach (Plot plot in (IEnumerable<Plot>) plotList)
      {
        PlotInfo plotInfo = new PlotInfo();
        dictionary3.Add(plot.Id, plotInfo);
        plotInfo.PlotID = plot.Id;
        plotInfo.StrataDesc = plot.Strata.Description;
        plotInfo.PlotSize = Math.Round((double) plot.Size, 6) * (double) plot.PercentMeasured / 100.0;
        if (dictionary2.ContainsKey(plot.Strata.Description))
          dictionary2[plot.Strata.Description] += plotInfo.PlotSize;
        else
          dictionary2.Add(plot.Strata.Description, plotInfo.PlotSize);
        if (!dictionary1.ContainsKey(plot.Strata.Description))
        {
          dictionary1.Add(plot.Strata.Description, (double) plot.Strata.Size);
          num1 += (double) plot.Strata.Size;
        }
        if (this.curYear.RecordGroundCover)
        {
          List<PlotGroundCover> list1 = plot.PlotGroundCovers.Where<PlotGroundCover>((Func<PlotGroundCover, bool>) (pg => gcBuildings.Contains(pg.GroundCover))).ToList<PlotGroundCover>();
          List<PlotGroundCover> list2 = plot.PlotGroundCovers.Where<PlotGroundCover>((Func<PlotGroundCover, bool>) (pg => gcGrasses.Contains(pg.GroundCover))).ToList<PlotGroundCover>();
          plotInfo.percentBuilding = (double) list1.Sum<PlotGroundCover>((Func<PlotGroundCover, int>) (pg => pg.PercentCovered));
          plotInfo.percentGrass = (double) list2.Sum<PlotGroundCover>((Func<PlotGroundCover, int>) (pg => pg.PercentCovered));
        }
        plotInfo.percentTree = (double) plot.PercentTreeCover;
        if (this.curYear.RecordPercentShrub)
          plotInfo.percentShrub = (double) plot.PercentShrubCover;
      }
      IList<Tree> treeList = this.curInputISession.CreateCriteria<Tree>().CreateAlias("Plot", "p").CreateAlias("p.Year", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.curYear.Guid)).Add((ICriterion) Restrictions.Eq("p.IsComplete", (object) true)).Add((ICriterion) Restrictions.On<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
      {
        (object) TreeStatus.InitialSample,
        (object) TreeStatus.Ingrowth,
        (object) TreeStatus.NoChange,
        (object) TreeStatus.Planted,
        (object) TreeStatus.Unknown
      })).AddOrder(Order.Asc("p.Id")).List<Tree>();
      DefaultTreeData defaultTreeData = new DefaultTreeData();
      defaultTreeData.initialize(this.curInputISession, this.curYear.Guid, this.locSpSession);
      double num2 = 1.0;
      double num3 = 1.0;
      if (this.curYear.Unit == YearUnit.English)
      {
        num2 = 2.54;
        num3 = 0.3048;
      }
      foreach (Tree aTree in (IEnumerable<Tree>) treeList)
      {
        PlotInfo plotInfo = dictionary3[aTree.Plot.Id];
        ++plotInfo.numTrees;
        DBHCrownSize dbhCrownSize = defaultTreeData.calculateDbhCrownSize(aTree);
        if (dbhCrownSize.dbh * num2 < 10.0)
          ++plotInfo.numSapling;
        if (dbhCrownSize.dbh * num2 > 23.0)
          ++plotInfo.numBigTrees;
        if (dbhCrownSize.dbh * num2 > 6.0)
          plotInfo.sumtreeBasalAreaM2 += 3.1425926535 * dbhCrownSize.dbh * num2 * dbhCrownSize.dbh * num2 / 4.0 / 10000.0;
        if (dbhCrownSize.dieback > 13)
          ++plotInfo.numDead;
        plotInfo.sumLiveHeightMeters += dbhCrownSize.treeHeight * num3;
      }
      foreach (PlotInfo theInfo in dictionary3.Values)
        this.CalculatePlotSuitabilityIndices(wildlife, theInfo);
      Dictionary<Tuple<string, Wildlife>, StrataWildLifeSuitability> dictionary4 = new Dictionary<Tuple<string, Wildlife>, StrataWildLifeSuitability>();
      foreach (PlotWildLifeSuitability suitabilityIndexByPlot in this.SuitabilityIndexByPlots)
      {
        Tuple<string, Wildlife> key = Tuple.Create<string, Wildlife>(suitabilityIndexByPlot.StrataDesc, suitabilityIndexByPlot.Wildlife);
        if (dictionary4.ContainsKey(key))
        {
          StrataWildLifeSuitability wildLifeSuitability = dictionary4[key];
          wildLifeSuitability.SuitabilityIndexWithTree += suitabilityIndexByPlot.SuitabilityIndexWithTree * suitabilityIndexByPlot.PlotSize;
          wildLifeSuitability.SuitabilityIndexWithoutTree += suitabilityIndexByPlot.SuitabilityIndexWithoutTree * suitabilityIndexByPlot.PlotSize;
        }
        else
        {
          StrataWildLifeSuitability wildLifeSuitability1 = new StrataWildLifeSuitability();
          wildLifeSuitability1.StrataDesc = suitabilityIndexByPlot.StrataDesc;
          wildLifeSuitability1.Wildlife = suitabilityIndexByPlot.Wildlife;
          wildLifeSuitability1.SuitabilityIndexWithTree = suitabilityIndexByPlot.SuitabilityIndexWithTree * suitabilityIndexByPlot.PlotSize;
          wildLifeSuitability1.SuitabilityIndexWithoutTree = suitabilityIndexByPlot.SuitabilityIndexWithoutTree * suitabilityIndexByPlot.PlotSize;
          StrataWildLifeSuitability wildLifeSuitability2 = wildLifeSuitability1;
          dictionary4.Add(key, wildLifeSuitability2);
        }
      }
      foreach (KeyValuePair<Tuple<string, Wildlife>, StrataWildLifeSuitability> keyValuePair in dictionary4)
      {
        keyValuePair.Value.SuitabilityIndexWithTree /= dictionary2[keyValuePair.Key.Item1];
        keyValuePair.Value.SuitabilityIndexWithoutTree /= dictionary2[keyValuePair.Key.Item1];
        keyValuePair.Value.RelativeChangeOfSuitabilityIndexWithTree = keyValuePair.Value.SuitabilityIndexWithTree == 0.0 ? 0.0 : (keyValuePair.Value.SuitabilityIndexWithTree - keyValuePair.Value.SuitabilityIndexWithoutTree) / keyValuePair.Value.SuitabilityIndexWithTree * 100.0;
        keyValuePair.Value.AbsoluteChangeOfSuitabilityIndexWithTree = keyValuePair.Value.SuitabilityIndexWithTree - keyValuePair.Value.SuitabilityIndexWithoutTree;
      }
      Dictionary<Wildlife, StrataWildLifeSuitability> dictionary5 = new Dictionary<Wildlife, StrataWildLifeSuitability>();
      foreach (StrataWildLifeSuitability wildLifeSuitability3 in dictionary4.Values)
      {
        if (dictionary5.ContainsKey(wildLifeSuitability3.Wildlife))
        {
          StrataWildLifeSuitability wildLifeSuitability4 = dictionary5[wildLifeSuitability3.Wildlife];
          wildLifeSuitability4.SuitabilityIndexWithTree += wildLifeSuitability3.SuitabilityIndexWithTree * dictionary1[wildLifeSuitability3.StrataDesc];
          wildLifeSuitability4.SuitabilityIndexWithoutTree += wildLifeSuitability3.SuitabilityIndexWithoutTree * dictionary1[wildLifeSuitability3.StrataDesc];
        }
        else
        {
          StrataWildLifeSuitability wildLifeSuitability5 = new StrataWildLifeSuitability();
          wildLifeSuitability5.StrataDesc = string.Empty;
          wildLifeSuitability5.Wildlife = wildLifeSuitability3.Wildlife;
          wildLifeSuitability5.SuitabilityIndexWithTree = wildLifeSuitability3.SuitabilityIndexWithTree * dictionary1[wildLifeSuitability3.StrataDesc];
          wildLifeSuitability5.SuitabilityIndexWithoutTree = wildLifeSuitability3.SuitabilityIndexWithoutTree * dictionary1[wildLifeSuitability3.StrataDesc];
          dictionary5.Add(wildLifeSuitability3.Wildlife, wildLifeSuitability5);
        }
      }
      foreach (StrataWildLifeSuitability wildLifeSuitability in dictionary5.Values)
      {
        wildLifeSuitability.SuitabilityIndexWithTree /= num1;
        wildLifeSuitability.SuitabilityIndexWithoutTree /= num1;
        wildLifeSuitability.RelativeChangeOfSuitabilityIndexWithTree = wildLifeSuitability.SuitabilityIndexWithTree == 0.0 ? 0.0 : (wildLifeSuitability.SuitabilityIndexWithTree - wildLifeSuitability.SuitabilityIndexWithoutTree) / wildLifeSuitability.SuitabilityIndexWithTree * 100.0;
        wildLifeSuitability.AbsoluteChangeOfSuitabilityIndexWithTree = wildLifeSuitability.SuitabilityIndexWithTree - wildLifeSuitability.SuitabilityIndexWithoutTree;
      }
      this.SuitabilityIndexByStrata = dictionary4.Values.ToList<StrataWildLifeSuitability>();
      this.SuitabilityIndexByStrataForTotal = dictionary5.Values.ToList<StrataWildLifeSuitability>();
    }

    public override void InitDocument(C1PrintDocument C1doc)
    {
      base.InitDocument(C1doc);
      if (ReportBase.m_ps.SpeciesDisplayName == SpeciesDisplayEnum.CommonName)
      {
        this.SuitabilityIndexByPlots = this.SuitabilityIndexByPlots.OrderBy<PlotWildLifeSuitability, int>((Func<PlotWildLifeSuitability, int>) (s => s.PlotID)).ThenBy<PlotWildLifeSuitability, string>((Func<PlotWildLifeSuitability, string>) (a => a.Wildlife.CommonName)).ToList<PlotWildLifeSuitability>();
        this.SuitabilityIndexByStrata = this.SuitabilityIndexByStrata.OrderBy<StrataWildLifeSuitability, string>((Func<StrataWildLifeSuitability, string>) (w => w.StrataDesc)).ThenBy<StrataWildLifeSuitability, string>((Func<StrataWildLifeSuitability, string>) (w => w.Wildlife.CommonName)).ToList<StrataWildLifeSuitability>();
        this.SuitabilityIndexByStrataForTotal = this.SuitabilityIndexByStrataForTotal.OrderBy<StrataWildLifeSuitability, string>((Func<StrataWildLifeSuitability, string>) (w => w.Wildlife.CommonName)).ToList<StrataWildLifeSuitability>();
      }
      else
      {
        this.SuitabilityIndexByPlots = this.SuitabilityIndexByPlots.OrderBy<PlotWildLifeSuitability, int>((Func<PlotWildLifeSuitability, int>) (s => s.PlotID)).ThenBy<PlotWildLifeSuitability, string>((Func<PlotWildLifeSuitability, string>) (a => a.Wildlife.ScientificName)).ToList<PlotWildLifeSuitability>();
        this.SuitabilityIndexByStrata = this.SuitabilityIndexByStrata.OrderBy<StrataWildLifeSuitability, string>((Func<StrataWildLifeSuitability, string>) (w => w.StrataDesc)).ThenBy<StrataWildLifeSuitability, string>((Func<StrataWildLifeSuitability, string>) (w => w.Wildlife.ScientificName)).ToList<StrataWildLifeSuitability>();
        this.SuitabilityIndexByStrataForTotal = this.SuitabilityIndexByStrataForTotal.OrderBy<StrataWildLifeSuitability, string>((Func<StrataWildLifeSuitability, string>) (w => w.Wildlife.ScientificName)).ToList<StrataWildLifeSuitability>();
      }
    }

    private void CalculatePlotSuitabilityIndices(IList<Wildlife> wildLifes, PlotInfo theInfo)
    {
      double num1 = 0.04046856;
      double num2 = 1.0;
      if (this.curYear.Unit == YearUnit.English)
      {
        num1 = 0.1;
        num2 = 0.4046856;
      }
      double num3 = (double) theInfo.numDead / theInfo.PlotSize * num1;
      double num4 = (double) theInfo.numBigTrees / theInfo.PlotSize * num1;
      double num5 = (double) theInfo.numSapling / theInfo.PlotSize * num1;
      double num6 = (double) theInfo.numTrees / theInfo.PlotSize * num1;
      double num7 = theInfo.numTrees == 0 ? 0.0 : theInfo.sumLiveHeightMeters / (double) theInfo.numTrees;
      double num8 = theInfo.sumtreeBasalAreaM2 / (theInfo.PlotSize * num2);
      foreach (Wildlife wildLife in (IEnumerable<Wildlife>) wildLifes)
      {
        PlotWildLifeSuitability wildLifeSuitability = new PlotWildLifeSuitability();
        wildLifeSuitability.PlotID = theInfo.PlotID;
        wildLifeSuitability.PlotSize = theInfo.PlotSize;
        wildLifeSuitability.StrataDesc = theInfo.StrataDesc;
        wildLifeSuitability.Wildlife = wildLife;
        wildLifeSuitability.SuitabilityIndexWithTree = 1.0;
        wildLifeSuitability.SuitabilityIndexWithoutTree = 1.0;
        wildLifeSuitability.RelativeChangeOfSuitabilityIndexWithTree = 0.0;
        wildLifeSuitability.AbsoluteChangeOfSuitabilityIndexWithTree = 0.0;
        List<double> doubleList1 = new List<double>();
        List<double> doubleList2 = new List<double>();
        List<double> doubleList3 = new List<double>();
        List<double> doubleList4 = new List<double>();
        foreach (Equation equation in (IEnumerable<Equation>) wildLife.Equations)
        {
          double X1;
          double X2;
          switch (equation.XVariable)
          {
            case XVariable.Landuse:
            case XVariable.ForestArea:
            case XVariable.ForestCover1KM:
              throw new Exception("XVariable " + ((int) equation.XVariable).ToString((IFormatProvider) CultureInfo.InvariantCulture) + " is not used.");
            case XVariable.PercentBuilding:
              X1 = theInfo.percentBuilding;
              X2 = theInfo.percentBuilding;
              break;
            case XVariable.PercentGrass:
              X1 = theInfo.percentGrass;
              X2 = theInfo.percentGrass;
              break;
            case XVariable.PercentShrub:
              X1 = theInfo.percentShrub;
              X2 = theInfo.percentShrub;
              break;
            case XVariable.PercentTree:
              X1 = theInfo.percentTree;
              X2 = 0.0;
              break;
            case XVariable.TreeNumberInTenthAcre:
              X1 = num6;
              X2 = 0.0;
              break;
            case XVariable.SmallTreeNumberInTenthAcre:
              X1 = num5;
              X2 = 0.0;
              break;
            case XVariable.LargeTreeNumberInTenthAcre:
              X1 = num4;
              X2 = 0.0;
              break;
            case XVariable.DeadTreeNumberInTenthAcre:
              X1 = num3;
              X2 = 0.0;
              break;
            case XVariable.BasalAreaDensityM2overHectare6CM:
              X1 = num8;
              X2 = 0.0;
              break;
            case XVariable.MeanHeight:
              X1 = num7;
              X2 = 0.0;
              break;
            default:
              throw new Exception("XVariable " + ((int) equation.XVariable).ToString((IFormatProvider) CultureInfo.InvariantCulture) + " is invalid.");
          }
          if (wildLife.Id != 5)
          {
            doubleList1.Add(this.fitTo0_1Range(equation.Evaluate(X1)));
            doubleList2.Add(this.fitTo0_1Range(equation.Evaluate(X2)));
          }
          else if (wildLife.Id == 5 && (equation.XVariable == XVariable.TreeNumberInTenthAcre || equation.XVariable == XVariable.DeadTreeNumberInTenthAcre))
          {
            doubleList1.Add(this.fitTo0_1Range(equation.Evaluate(X1)));
            doubleList2.Add(this.fitTo0_1Range(equation.Evaluate(X2)));
          }
          else
          {
            doubleList3.Add(this.fitTo0_1Range(equation.Evaluate(X1)));
            doubleList4.Add(this.fitTo0_1Range(equation.Evaluate(X2)));
          }
        }
        double x1 = 1.0;
        double x2 = 1.0;
        for (int index = 0; index < doubleList1.Count; ++index)
        {
          x1 *= doubleList1[index];
          x2 *= doubleList2[index];
        }
        if (doubleList1.Count > 1)
        {
          x1 = Math.Pow(x1, 1.0 / (double) doubleList1.Count);
          x2 = Math.Pow(x2, 1.0 / (double) doubleList2.Count);
        }
        if (wildLife.Id == 5)
        {
          double num9;
          double num10;
          if (doubleList3.Count > 0)
          {
            num9 = doubleList3[0];
            foreach (double num11 in doubleList3)
              num9 = num9 < num11 ? num11 : num9;
            num10 = doubleList4[0];
            foreach (double num12 in doubleList4)
              num10 = num10 < num12 ? num12 : num10;
          }
          else
          {
            num9 = 1.0;
            num10 = 1.0;
          }
          wildLifeSuitability.SuitabilityIndexWithTree = x1 * num9;
          wildLifeSuitability.SuitabilityIndexWithoutTree = x2 * num10;
        }
        else
        {
          wildLifeSuitability.SuitabilityIndexWithTree = x1;
          wildLifeSuitability.SuitabilityIndexWithoutTree = x2;
        }
        wildLifeSuitability.RelativeChangeOfSuitabilityIndexWithTree = wildLifeSuitability.SuitabilityIndexWithTree == 0.0 ? 0.0 : (wildLifeSuitability.SuitabilityIndexWithTree - wildLifeSuitability.SuitabilityIndexWithoutTree) / wildLifeSuitability.SuitabilityIndexWithTree * 100.0;
        wildLifeSuitability.AbsoluteChangeOfSuitabilityIndexWithTree = wildLifeSuitability.SuitabilityIndexWithTree - wildLifeSuitability.SuitabilityIndexWithoutTree;
        this.SuitabilityIndexByPlots.Add(wildLifeSuitability);
      }
    }

    private IList<Wildlife> GetWildlife()
    {
      IList<Wildlife> list = (IList<Wildlife>) this.city.Wildlife.ToList<Wildlife>();
      if (list.Count == 0)
      {
        list = (IList<Wildlife>) this.county.Wildlife.ToList<Wildlife>();
        if (list.Count == 0)
        {
          list = (IList<Wildlife>) this.state.Wildlife.ToList<Wildlife>();
          if (list.Count == 0)
            list = (IList<Wildlife>) this.nation.Wildlife.ToList<Wildlife>();
        }
      }
      return list;
    }

    private double fitTo0_1Range(double aValue)
    {
      aValue = Math.Abs(aValue);
      return aValue > 1.0 ? 1.0 : aValue;
    }
  }
}
