// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.DatabaseReport
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate.Extensions;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Services;
using Eco.Util.Views;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using TreeEnergy;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  public class DatabaseReport : BasicReport
  {
    protected EstimateUtil estUtil;
    private ExchangeRate er;
    private double _currencyExchangeRate;
    private double _customizedWaterDollarsPerM3;
    private double _customizedCarbonDollarsPerTon;
    private double _customizedElectricityDollarsPerKwh;
    private double _customizedHeatingDollarsPerTherm;
    private double _customizedCoDollarsPerTon;
    private double _customizedO3DollarsPerTon;
    private double _customizedNO2DollarsPerTon;
    private double _customizedSO2DollarsPerTon;
    private double _customizedPM25DollarsPerTon;
    private double _customizedPM10DollarsPerTon;
    private double _yearlyPrecipitationMeters;

    public double currencyExchangeRate => this._currencyExchangeRate;

    public LocationEnvironmentalValue lEv => this.nation.EnvironmentalValue;

    public double customizedWaterDollarsPerM3 => this._customizedWaterDollarsPerM3;

    public double customizedCarbonDollarsPerTon => this._customizedCarbonDollarsPerTon;

    public double customizedElectricityDollarsPerKwh => this._customizedElectricityDollarsPerKwh;

    public double customizedHeatingDollarsPerTherm => this._customizedHeatingDollarsPerTherm;

    public double customizedCoDollarsPerTon => this._customizedCoDollarsPerTon;

    public double customizedO3DollarsPerTon => this._customizedO3DollarsPerTon;

    public double customizedNO2DollarsPerTon => this._customizedNO2DollarsPerTon;

    public double customizedSO2DollarsPerTon => this._customizedSO2DollarsPerTon;

    public double customizedPM25DollarsPerTon => this._customizedPM25DollarsPerTon;

    public double customizedPM10DollarsPerTon => this._customizedPM10DollarsPerTon;

    public double yearlyPrecipitationMeters => this._yearlyPrecipitationMeters;

    protected LocationService LocationService { get; }

    public static void NewReportMessage(C1PrintDocument C1doc, string msg)
    {
      RenderTable ro = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) ro);
      ro.Style.FontSize = 12f;
      ro.Style.TextColor = Color.Red;
      ro.Style.FontBold = true;
      ro.Width = (Unit) "100%";
      ro.Cols[0].Width = (Unit) "100%";
      ro.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      ro.Cells[0, 0].Text = msg;
    }

    protected string GetSpeciesName(ImportanceValueSpecies imp, bool scientificName) => !scientificName ? this.estUtil.ClassValues[Classifiers.Species][imp.SpeciesCVO].Item1 : this.estUtil.ClassValues[Classifiers.Species][imp.SpeciesCVO].Item2;

    public DatabaseReport()
    {
      this.LocationService = new LocationService(ReportBase.m_ps.LocSp);
      this.estUtil = new EstimateUtil(ReportBase.m_ps.InputSession, ReportBase.m_ps.LocSp);
      this.er = this.curYear.ExchangeRate;
      this._currencyExchangeRate = this.er != null ? this.er.Price : 1.0;
      this.SetDollarAmounts();
      this._yearlyPrecipitationMeters = this.estUtil.GetYearlyPrecipitationMetersFromEstimateDb();
      this.LocationService = new LocationService(ReportBase.m_ps.LocSp);
    }

    public double AdjustCurrency(double value) => value * this._currencyExchangeRate;

    public override void InitDocument(C1PrintDocument C1doc)
    {
      base.InitDocument(C1doc);
      this.SetCustomTags(C1doc);
    }

    public string AvoidedRunoff_m3_ft3_Footer() => string.Format(i_Tree_Eco_v6.Resources.Strings.NoteAvoidedRunoff, (object) this.CurrencySymbol, (object) this.GetWaterDollarsStr(), (object) ReportBase.CubicMeterUnits(), (object) this.GetPrecipitationStr(), (object) ReportBase.CentimeterUnits());

    private string GetPrecipitationStr() => (ReportBase.EnglishUnits ? UnitsHelper.MetersToInches(this.yearlyPrecipitationMeters) : UnitsHelper.MetersToCentimeters(this.yearlyPrecipitationMeters)).ToString("N1");

    private string GetWaterDollarsStr() => (ReportBase.EnglishUnits ? UnitsHelper.CubicMetersToCubicFeet(this.customizedWaterDollarsPerM3) : this.customizedWaterDollarsPerM3).ToString("N3");

    private string GetCustomizedDollarsPerTon(double val) => (ReportBase.EnglishUnits ? val / 1.10231 : val).ToString("N2");

    protected string GetCustomizedDollarsPerKg(double val)
    {
      double tonne = UnitsHelper.KgToTonne(val);
      return (ReportBase.EnglishUnits ? tonne / 2.20462 : tonne).ToString("N2");
    }

    private string GetCustomizedDollarsPerG(double val) => (ReportBase.EnglishUnits ? val / 1.10231 / 32000.0 : val / 1000000.0).ToString("N5");

    public string PollutionRemoval_g_oz_Footer() => string.Format(i_Tree_Eco_v6.Resources.Strings.NotePollutionRemovalFooter, (object) ReportBase.GUnits(), (object) this.GetCustomizedDollarsPerG(this.customizedCoDollarsPerTon), (object) this.GetCustomizedDollarsPerG(this.customizedO3DollarsPerTon), (object) this.GetCustomizedDollarsPerG(this.customizedNO2DollarsPerTon), (object) this.GetCustomizedDollarsPerG(this.customizedSO2DollarsPerTon), (object) this.GetCustomizedDollarsPerG(this.customizedPM25DollarsPerTon), (object) this.GetCustomizedDollarsPerG(this.customizedPM10DollarsPerTon), (object) this.CurrencySymbol);

    public string PollutionRemoval_kg_lb_Footer() => string.Format(i_Tree_Eco_v6.Resources.Strings.NotePollutionRemovalFooter, (object) ReportBase.KilogramUnits(), (object) this.GetCustomizedDollarsPerKg(this.customizedCoDollarsPerTon), (object) this.GetCustomizedDollarsPerKg(this.customizedO3DollarsPerTon), (object) this.GetCustomizedDollarsPerKg(this.customizedNO2DollarsPerTon), (object) this.GetCustomizedDollarsPerKg(this.customizedSO2DollarsPerTon), (object) this.GetCustomizedDollarsPerKg(this.customizedPM25DollarsPerTon), (object) this.GetCustomizedDollarsPerKg(this.customizedPM10DollarsPerTon), (object) this.CurrencySymbol);

    public string PollutionRemoval_MetricTon_ShortTon_Footer() => string.Format(i_Tree_Eco_v6.Resources.Strings.NotePollutionRemovalFooter, (object) ReportBase.TonneUnits(), (object) this.GetCustomizedDollarsPerTon(this.customizedCoDollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedO3DollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedNO2DollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedSO2DollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedPM25DollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedPM10DollarsPerTon), (object) this.CurrencySymbol);

    public string PollutionRemoval_kg_lb_Footer(string pollutantName)
    {
      double val = 0.0;
      if (!(pollutantName == "CO"))
      {
        if (!(pollutantName == "NO2"))
        {
          if (!(pollutantName == "O3"))
          {
            if (!(pollutantName == "SO2"))
            {
              if (!(pollutantName == "PM2.5"))
              {
                if (pollutantName == "PM10*")
                  val = this.customizedPM10DollarsPerTon;
              }
              else
                val = this.customizedPM25DollarsPerTon;
            }
            else
              val = this.customizedSO2DollarsPerTon;
          }
          else
            val = this.customizedO3DollarsPerTon;
        }
        else
          val = this.customizedNO2DollarsPerTon;
      }
      else
        val = this.customizedCoDollarsPerTon;
      return string.Format(i_Tree_Eco_v6.Resources.Strings.NotePollutionRemovalForPolutant, (object) this.CurrencySymbol, (object) this.GetCustomizedDollarsPerKg(val), (object) ReportBase.KilogramUnits(), (object) pollutantName);
    }

    internal string Carbon_MetricTon_ShortTon_Footer(string footerStr) => string.Format(footerStr, (object) ReportBase.TonneUnits(), (object) (ReportBase.EnglishUnits ? this.customizedCarbonDollarsPerTon / 1.10231 : this.customizedCarbonDollarsPerTon).ToString("N2"), (object) this.CurrencySymbol);

    public string ReplacementValue_Footer() => this.CheckNations() ? i_Tree_Eco_v6.Resources.Strings.NoteReplacementValueFooterNationsIncluded : i_Tree_Eco_v6.Resources.Strings.NoteReplacementValueFooterNationsNotIncluded;

    public string Carbon_kg_lb_Footer() => string.Format(i_Tree_Eco_v6.Resources.Strings.NoteCarbonCarbonKgLbFooter, (object) this.CurrencySymbol, (object) (ReportBase.EnglishUnits ? this.customizedCarbonDollarsPerTon / 1000.0 / 2.20462 : this.customizedCarbonDollarsPerTon / 1000.0).ToString("N5"), (object) ReportBase.KilogramUnits());

    public string Energy_MKW_MBTU_Footer()
    {
      string energyMkwmbtuFooter = i_Tree_Eco_v6.Resources.Strings.NoteEnergyMKWMBTUFooter;
      double num = this.customizedElectricityDollarsPerKwh * 1000.0;
      string str1 = num.ToString("N2");
      num = this.customizedHeatingDollarsPerTherm * 10.0023877;
      string str2 = num.ToString("N2");
      string currencySymbol = this.CurrencySymbol;
      return string.Format("{0} {1}", (object) string.Format(energyMkwmbtuFooter, (object) str1, (object) str2, (object) currencySymbol), (object) i_Tree_Eco_v6.Resources.Strings.NoteTreeBuildingEnergyBenefit1);
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
    }

    private void SetCustomTags(C1PrintDocument C1doc) => C1doc.Tags.Add(new Tag("CURRENCY", (object) this.CurrencySymbol));

    private void SetDollarAmounts()
    {
      if (this.CheckNations())
        this._customizedWaterDollarsPerM3 = this.curYear.H2O == null ? this.GetDefaultWaterDollarsPerM3() : this.curYear.H2O.Price;
      else if (this.curYear.H2O != null)
      {
        this._customizedWaterDollarsPerM3 = this.curYear.H2O.Price;
      }
      else
      {
        this._customizedWaterDollarsPerM3 = this.GetDefaultWaterDollarsPerM3();
        if (this._customizedWaterDollarsPerM3 == 0.0)
          this._customizedWaterDollarsPerM3 = this.GetUSADefaultWaterDollarsPerM3() * this.currencyExchangeRate;
      }
      if (this.CheckNations())
        this._customizedCarbonDollarsPerTon = this.curYear.Carbon == null ? (double) this.GetDefaultCarbonDollarsPerTon() : this.curYear.Carbon.Price;
      else if (this.curYear.Carbon != null)
      {
        this._customizedCarbonDollarsPerTon = this.curYear.Carbon.Price;
      }
      else
      {
        this._customizedCarbonDollarsPerTon = (double) this.GetDefaultCarbonDollarsPerTon();
        if (this._customizedCarbonDollarsPerTon == 0.0)
          this._customizedCarbonDollarsPerTon = (double) this.GetUSADefaultCarbonDollarsPerTon() * this.currencyExchangeRate;
      }
      double electricityDollarsPerKwh = this.GetDefaultElectricityDollarsPerKwh();
      if (this.CheckNations())
        this._customizedElectricityDollarsPerKwh = this.curYear.Electricity == null ? electricityDollarsPerKwh : this.curYear.Electricity.Price;
      else if (this.curYear.Electricity != null)
      {
        this._customizedElectricityDollarsPerKwh = this.curYear.Electricity.Price;
      }
      else
      {
        this._customizedElectricityDollarsPerKwh = electricityDollarsPerKwh;
        if (this._customizedElectricityDollarsPerKwh == 0.0)
        {
          LocationCost locationCost = this.LocationService.GetLocationCost(219);
          if (locationCost != null)
            this._customizedElectricityDollarsPerKwh = locationCost.Electricity * this.currencyExchangeRate;
        }
      }
      double num = this.state.LocationCost != null ? this.state.LocationCost.Fuels / 10.002387672 : (this.nation.LocationCost != null ? this.nation.LocationCost.Fuels / 10.002387672 : 0.0);
      if (this.CheckNations())
        this._customizedHeatingDollarsPerTherm = this.curYear.Gas == null ? num : this.curYear.Gas.Price;
      else if (this.curYear.Gas != null)
      {
        this._customizedHeatingDollarsPerTherm = this.curYear.Gas.Price;
      }
      else
      {
        this._customizedHeatingDollarsPerTherm = num;
        if (this._customizedHeatingDollarsPerTherm == 0.0)
        {
          LocationCost locationCost = this.LocationService.GetLocationCost(219);
          if (locationCost != null)
            this._customizedHeatingDollarsPerTherm = locationCost.Fuels / 10.002387672;
        }
      }
      double tonFromEstimateDb1 = this.estUtil.GetCalculatedPollutantDollarsPerTonFromEstimateDb("CO");
      this._customizedCoDollarsPerTon = !this.CheckNations() ? (this.curYear.CO == null ? tonFromEstimateDb1 * this.currencyExchangeRate : this.curYear.CO.Price) : tonFromEstimateDb1;
      double tonFromEstimateDb2 = this.estUtil.GetCalculatedPollutantDollarsPerTonFromEstimateDb("O3");
      this._customizedO3DollarsPerTon = this.nation.Id != 219 ? (!NationFeatures.IsUSlikeNation(this.nation.Id) ? (this.curYear.O3 == null ? tonFromEstimateDb2 * this.currencyExchangeRate : this.curYear.O3.Price) : tonFromEstimateDb2 * this.currencyExchangeRate) : tonFromEstimateDb2;
      double tonFromEstimateDb3 = this.estUtil.GetCalculatedPollutantDollarsPerTonFromEstimateDb("NO2");
      this._customizedNO2DollarsPerTon = this.nation.Id != 219 ? (!NationFeatures.IsUSlikeNation(this.nation.Id) ? (this.curYear.NO2 == null ? tonFromEstimateDb3 * this.currencyExchangeRate : this.curYear.NO2.Price) : tonFromEstimateDb3 * this.currencyExchangeRate) : tonFromEstimateDb3;
      double tonFromEstimateDb4 = this.estUtil.GetCalculatedPollutantDollarsPerTonFromEstimateDb("SO2");
      this._customizedSO2DollarsPerTon = this.nation.Id != 219 ? (!NationFeatures.IsUSlikeNation(this.nation.Id) ? (this.curYear.SO2 == null ? tonFromEstimateDb4 * this.currencyExchangeRate : this.curYear.SO2.Price) : tonFromEstimateDb4 * this.currencyExchangeRate) : tonFromEstimateDb4;
      double tonFromEstimateDb5 = this.estUtil.GetCalculatedPollutantDollarsPerTonFromEstimateDb("PM2.5");
      this._customizedPM25DollarsPerTon = this.nation.Id != 219 ? (!NationFeatures.IsUSlikeNation(this.nation.Id) ? (this.curYear.PM25 == null ? tonFromEstimateDb5 * this.currencyExchangeRate : this.curYear.PM25.Price) : tonFromEstimateDb5 * this.currencyExchangeRate) : tonFromEstimateDb5;
      double tonFromEstimateDb6 = this.estUtil.GetCalculatedPollutantDollarsPerTonFromEstimateDb("PM10*");
      if (this.CheckNations())
        this._customizedPM10DollarsPerTon = tonFromEstimateDb6;
      else if (this.curYear.PM10 != null)
        this._customizedPM10DollarsPerTon = this.curYear.PM10.Price;
      else
        this._customizedPM10DollarsPerTon = tonFromEstimateDb6 * this.currencyExchangeRate;
    }

    private double GetDefaultElectricityDollarsPerKwh()
    {
      Location[] locationArray = new Location[4]
      {
        this.city,
        this.county,
        this.state,
        this.nation
      };
      foreach (Location location in locationArray)
      {
        if (location.LocationCost != null)
          return location.LocationCost.Electricity;
      }
      return 0.0;
    }

    private float GetDefaultCarbonDollarsPerTon() => this.nation.EnvironmentalValue != null ? this.nation.EnvironmentalValue.Carbon : 0.0f;

    private float GetUSADefaultCarbonDollarsPerTon() => RetryExecutionHandler.Execute<float>((Func<float>) (() =>
    {
      using (ISession session = ReportBase.m_ps.LocSp.OpenSession())
        return session.QueryOver<LocationEnvironmentalValue>().JoinQueryOver<Location>((System.Linq.Expressions.Expression<Func<LocationEnvironmentalValue, Location>>) (ev => ev.Location)).Where((System.Linq.Expressions.Expression<Func<Location, bool>>) (l => l.Id == 219)).Select((System.Linq.Expressions.Expression<Func<LocationEnvironmentalValue, object>>) (ev => (object) ev.Carbon)).SingleOrDefault<float>();
    }));

    private double GetDefaultWaterDollarsPerM3() => this.nation.EnvironmentalValue != null ? this.nation.EnvironmentalValue.RainfallInterception * 264.17 : 0.0;

    private double GetUSADefaultWaterDollarsPerM3() => RetryExecutionHandler.Execute<double>((Func<double>) (() =>
    {
      using (ISession session = ReportBase.m_ps.LocSp.OpenSession())
        return session.QueryOver<LocationEnvironmentalValue>().JoinQueryOver<Location>((System.Linq.Expressions.Expression<Func<LocationEnvironmentalValue, Location>>) (ev => ev.Location)).Where((System.Linq.Expressions.Expression<Func<Location, bool>>) (l => l.Id == 219)).Select((System.Linq.Expressions.Expression<Func<LocationEnvironmentalValue, object>>) (ev => (object) ev.RainfallInterception)).SingleOrDefault<double>();
    })) * 264.17;

    protected void CalculateEnergy<T>(SortedDictionary<Tuple<int, int>, T> energyEffects) where T : IndividualTreeEnergyFieldsBase, new()
    {
      Dictionary<Plot, int> levelBuildingCover = this.GetPlotLevelBuildingCover();
      IndividualTree indivTree = new IndividualTree(this.locSpSession, this.city, this._currencyExchangeRate, this.curYear.Unit == YearUnit.Metric, this.curYear.RecordHeight);
      IList<Tree> treeList = this.GetTreeList();
      Dictionary<string, SpeciesView> species1 = ReportBase.m_ps.Species;
      IDictionary<int, Species> treeSpecies = this.GetTreeSpecies();
      foreach (Tree tree in (IEnumerable<Tree>) treeList)
      {
        if (tree.Buildings.Count > 0)
        {
          SpeciesView spView = (SpeciesView) null;
          if (species1.TryGetValue(tree.Species, out spView))
          {
            double? percentGroundCover = this.GetPercentGroundCover(tree, levelBuildingCover);
            Tuple<int, int> tuple = Tuple.Create<int, int>(tree.Plot.Id, tree.Id);
            this.GetEnergyPlotTreeSpNameEntry<T>(ref energyEffects, tuple, spView);
            Species species2 = (Species) null;
            if (treeSpecies.TryGetValue(spView.Id, out species2))
              energyEffects[tuple].GetEnergyImpactValues(this, tree, species2, indivTree, percentGroundCover);
          }
        }
      }
    }

    public IList<Tree> GetTreeList() => (IList<Tree>) this.curInputISession.CreateCriteria<Tree>().CreateAlias("Plot", "p").Add((ICriterion) Restrictions.On<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
    {
      (object) TreeStatus.InitialSample,
      (object) TreeStatus.Ingrowth,
      (object) TreeStatus.NoChange,
      (object) TreeStatus.Planted,
      (object) TreeStatus.Unknown
    })).Add((ICriterion) Restrictions.Eq("p.Year", (object) this.curYear)).Add((ICriterion) Restrictions.Eq("p.IsComplete", (object) true)).Fetch(SelectMode.Fetch, "Plot").AddOrder(Order.Asc("p.Id")).AddOrder(Order.Asc("Id")).PagedList<Tree>(1000);

    public IDictionary<int, Species> GetTreeSpecies()
    {
      IList<string> source = this.curInputISession.CreateCriteria<Tree>().CreateAlias("Plot", "p").Add((ICriterion) Restrictions.On<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
      {
        (object) TreeStatus.InitialSample,
        (object) TreeStatus.Ingrowth,
        (object) TreeStatus.NoChange,
        (object) TreeStatus.Planted,
        (object) TreeStatus.Unknown
      })).Add((ICriterion) Restrictions.Eq("p.Year", (object) this.curYear)).Add((ICriterion) Restrictions.Eq("p.IsComplete", (object) true)).SetProjection((IProjection) Projections.ProjectionList().Add(Projections.Distinct((IProjection) Projections.Property<Tree>((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => t.Species))))).List<string>();
      IList<Species> speciesList = this.locSpSession.CreateCriteria<Species>().Add((ICriterion) Restrictions.In((IProjection) Projections.Property<Species>((System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.Code)), (object[]) source.ToArray<string>())).Fetch(SelectMode.Fetch, "Parent").List<Species>();
      Dictionary<int, Species> treeSpecies = new Dictionary<int, Species>();
      foreach (Species species in (IEnumerable<Species>) speciesList)
        treeSpecies[species.Id] = species;
      return (IDictionary<int, Species>) treeSpecies;
    }

    private void GetEnergyPlotTreeSpNameEntry<T>(
      ref SortedDictionary<Tuple<int, int>, T> energyEffects,
      Tuple<int, int> curId,
      SpeciesView spView)
      where T : IndividualTreeEnergyFieldsBase, new()
    {
      if (energyEffects.ContainsKey(curId))
        return;
      SortedDictionary<Tuple<int, int>, T> sortedDictionary = energyEffects;
      Tuple<int, int> key = curId;
      T obj = new T();
      obj.PlotID = curId.Item1;
      obj.TreeID = curId.Item2;
      obj.Name = ReportBase.ScientificName ? spView.ScientificName : spView.CommonName;
      sortedDictionary.Add(key, obj);
    }

    private double? GetPercentGroundCover(Tree tree, Dictionary<Plot, int> PercentPlotBuildingCover)
    {
      double? percentGroundCover = new double?();
      if (this.series.IsSample)
      {
        int percentTreeCover = (int) tree.Plot.PercentTreeCover;
        int num1 = 0;
        if (PercentPlotBuildingCover.ContainsKey(tree.Plot))
          num1 = PercentPlotBuildingCover[tree.Plot];
        percentGroundCover = new double?((double) (percentTreeCover + num1));
        double? nullable = percentGroundCover;
        double num2 = 100.0;
        percentGroundCover = nullable.GetValueOrDefault() > num2 & nullable.HasValue ? new double?(100.0) : percentGroundCover;
      }
      return percentGroundCover;
    }

    private Dictionary<Plot, int> GetPlotLevelBuildingCover()
    {
      Dictionary<Plot, int> levelBuildingCover = new Dictionary<Plot, int>();
      if (this.curYear.RecordGroundCover)
      {
        List<GroundCover> list = this.curYear.GroundCovers.Where<GroundCover>((Func<GroundCover, bool>) (g => g.CoverTypeId == 1)).ToList<GroundCover>();
        foreach (PlotGroundCover plotGroundCover in (IEnumerable<PlotGroundCover>) this.curInputISession.CreateCriteria<PlotGroundCover>().Add((ICriterion) Restrictions.In("GroundCover", (ICollection) list)).List<PlotGroundCover>())
        {
          if (levelBuildingCover.ContainsKey(plotGroundCover.Plot))
            levelBuildingCover[plotGroundCover.Plot] += plotGroundCover.PercentCovered;
          else
            levelBuildingCover.Add(plotGroundCover.Plot, plotGroundCover.PercentCovered);
        }
      }
      return levelBuildingCover;
    }

    protected int CountTreesWithBuildings()
    {
      Plot p = (Plot) null;
      Building b = (Building) null;
      using (ISession session = ReportBase.m_ps.InputSession.CreateSession())
      {
        using (session.BeginTransaction())
          return session.QueryOver<Tree>().WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => (object) t.Status)).IsIn(new object[5]
          {
            (object) TreeStatus.InitialSample,
            (object) TreeStatus.Ingrowth,
            (object) TreeStatus.NoChange,
            (object) TreeStatus.Planted,
            (object) TreeStatus.Unknown
          }).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => t.Plot), (System.Linq.Expressions.Expression<Func<object>>) (() => p)).JoinAlias((System.Linq.Expressions.Expression<Func<Tree, object>>) (t => t.Buildings), (System.Linq.Expressions.Expression<Func<object>>) (() => b)).Where((System.Linq.Expressions.Expression<Func<Tree, bool>>) (_ => (Guid?) p.Year.Guid == ReportBase.m_ps.InputSession.YearKey)).RowCount();
      }
    }
  }
}
