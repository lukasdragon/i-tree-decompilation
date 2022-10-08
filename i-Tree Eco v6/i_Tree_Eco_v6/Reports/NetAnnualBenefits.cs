// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.NetAnnualBenefits
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Services;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class NetAnnualBenefits : DatabaseReport
  {
    private Year m_year;
    private Project m_project;
    private YearlyCost m_costs;
    private LocationSpecies.Domain.Currency m_currency;
    private Decimal? m_population;
    private Decimal m_totalTrees;
    private Decimal m_totalEnergy;
    private Decimal m_totalCarbon;
    private Decimal m_totalPollution;
    private Decimal m_totalRunnoff;
    private LocationService _locationService;

    public NetAnnualBenefits()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleNetAnnualBenefitsForAllTrees;
      this._locationService = new LocationService(ReportBase.m_ps.LocSp);
      this.Init();
    }

    private void Init()
    {
      using (this.curInputISession.BeginTransaction())
      {
        this.m_year = this.curInputISession.Get<Year>((object) ReportBase.m_ps.InputSession.YearKey);
        this.m_project = this.m_year.Series.Project;
        foreach (YearLocationData yearLocationData in (IEnumerable<YearLocationData>) this.m_year.YearLocationData)
        {
          if (yearLocationData.ProjectLocation.LocationId == this.m_project.LocationId)
          {
            this.m_population = new Decimal?((Decimal) yearLocationData.Population);
            break;
          }
        }
        IList<YearlyCost> yearlyCostList = this.curInputISession.CreateCriteria<YearlyCost>().Add((ICriterion) Restrictions.Eq("Year", (object) this.m_year)).List<YearlyCost>();
        this.m_costs = new YearlyCost();
        this.m_currency = this.LocationService.GetLocationCurrency(this.project.LocationId);
        foreach (YearlyCost yearlyCost in (IEnumerable<YearlyCost>) yearlyCostList)
        {
          this.m_costs.Administrative += yearlyCost.Administrative;
          this.m_costs.CleanUp += yearlyCost.CleanUp;
          this.m_costs.Inspection += yearlyCost.Inspection;
          this.m_costs.Irrigation += yearlyCost.Irrigation;
          this.m_costs.Legal += yearlyCost.Legal;
          this.m_costs.Other += yearlyCost.Other;
          this.m_costs.PestControl += yearlyCost.PestControl;
          this.m_costs.Planting += yearlyCost.Planting;
          this.m_costs.Pruning += yearlyCost.Pruning;
          this.m_costs.Repair += yearlyCost.Repair;
          this.m_costs.TreeRemoval += yearlyCost.TreeRemoval;
        }
      }
      if (!this.m_population.HasValue)
      {
        int? population = (int?) this._locationService.GetLocation(this.m_project.LocationId)?.Population;
        this.m_population = population.HasValue ? new Decimal?((Decimal) population.GetValueOrDefault()) : new Decimal?();
      }
      this.m_totalTrees = Convert.ToDecimal(ReportUtil.ConvertFromDBVal<double>((object) this.GetTreeTotalsEstimatedValuesData()));
      this.m_totalEnergy = Convert.ToDecimal(this.estUtil.GetEnergyValues(1, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon) + this.estUtil.GetEnergyValues(2, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon) + this.estUtil.GetEnergyValues(2, 1, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon));
      this.m_totalPollution = 0M;
      DataTable pollutionRemovalTons = this.GetPollutionRemovalTons();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Pollutant];
      foreach (DataRow row in (InternalDataCollectionBase) pollutionRemovalTons.Rows)
      {
        int num1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["sumEstimate"]);
        if (num1 == (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "CO"))
          this.m_totalPollution += (Decimal) (num2 * this.customizedCoDollarsPerTon);
        else if (num1 == (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "O3"))
          this.m_totalPollution += (Decimal) (num2 * this.customizedO3DollarsPerTon);
        else if (num1 == (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "NO2"))
          this.m_totalPollution += (Decimal) (num2 * this.customizedNO2DollarsPerTon);
        else if (num1 == (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "SO2"))
          this.m_totalPollution += (Decimal) (num2 * this.customizedSO2DollarsPerTon);
        else if (num1 == (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM10*"))
          this.m_totalPollution += (Decimal) (num2 * this.customizedPM10DollarsPerTon);
        else if (num1 == (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM2.5"))
          this.m_totalPollution += (Decimal) (num2 * this.customizedPM25DollarsPerTon);
      }
      this.m_totalCarbon = Convert.ToDecimal(ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatedAnualCarbonTotal()) * this.customizedCarbonDollarsPerTon);
      this.m_totalRunnoff = Convert.ToDecimal(ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatedAnualAvoidedRunoffTotal()) * this.customizedWaterDollarsPerM3);
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Style.FontSize = 10f;
      renderTable.Cols[0].Width = new Unit(0.25, UnitTypeEnum.Inch);
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      string str = ReportUtil.FormatInlineHeaderUnitsStr(this.m_currency.Symbol, this.m_currency.Abbreviation);
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      int count = 1;
      int num3 = 11;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = (float) num3;
      renderTable.Cells[0, 0].SpanCols = 2;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Benefits;
      renderTable.Cells[0, 2].Text = string.Format("{0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.Total, (object) str);
      renderTable.Cells[0, 3].Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) str, (object) v6Strings.Tree_SingularName.ToLower());
      renderTable.Cells[0, 4].Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) str, (object) i_Tree_Eco_v6.Resources.Strings.Capita);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.Energy, this.m_totalEnergy);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.GrossCarbonSequestration, this.m_totalCarbon);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.PollutionRemoval, this.m_totalPollution);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff, this.m_totalRunnoff);
      Decimal numerator = num2 + this.m_totalEnergy + this.m_totalCarbon + this.m_totalPollution + this.m_totalRunnoff;
      this.AddTotalRow(renderTable, i_Tree_Eco_v6.Resources.Strings.TotalBenefits, numerator);
      renderTable.Rows[6].Style.FontBold = true;
      renderTable.Rows[6].Style.FontSize = (float) num3;
      renderTable.Cells[6, 0].SpanCols = 2;
      renderTable.Cells[6, 0].Text = i_Tree_Eco_v6.Resources.Strings.Costs;
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.PurchasingTreesAndPlanting, this.m_costs.Planting);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.ContractPruning, this.m_costs.Pruning);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.PestManagement, this.m_costs.PestControl);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.Irrigation, this.m_costs.Irrigation);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.Removal, this.m_costs.TreeRemoval);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.Administration, this.m_costs.Administrative);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.InspectionService, this.m_costs.Inspection);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.InfrastructureRepairs, this.m_costs.Repair);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.LitterCleanup, this.m_costs.CleanUp);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.LiabilityClaims, this.m_costs.Legal);
      this.AddDataRow(renderTable, i_Tree_Eco_v6.Resources.Strings.OtherCosts, this.m_costs.Other);
      Decimal denominator = num1 + this.m_costs.Planting + this.m_costs.Pruning + this.m_costs.PestControl + this.m_costs.Irrigation + this.m_costs.TreeRemoval + this.m_costs.Administrative + this.m_costs.Inspection + this.m_costs.Repair + this.m_costs.CleanUp + this.m_costs.Legal + this.m_costs.Other;
      this.AddTotalRow(renderTable, i_Tree_Eco_v6.Resources.Strings.TotalCosts, denominator);
      Decimal num4 = numerator - denominator;
      TableRow tableRow = this.AddTotalRow(renderTable, i_Tree_Eco_v6.Resources.Strings.NetBenefits, num4);
      tableRow.Style.Borders.Bottom = LineDef.Empty;
      tableRow.Style.FontSize = (float) num3;
      TableRow row = renderTable.Rows[renderTable.Rows.Count];
      row.Style.FontBold = true;
      row.Style.FontSize = (float) num3;
      row[0].SpanCols = 2;
      row[0].Text = i_Tree_Eco_v6.Resources.Strings.BenefitCostRatio;
      row[2].Text = EstimateUtil.DivideOrZero(numerator, denominator).ToString("N2");
      ReportUtil.FormatRenderTable(renderTable);
      this.Note(C1doc);
    }

    private TableRow AddDataRow(RenderTable rt, string header, Decimal value)
    {
      int count = rt.Rows.Count;
      TableRow row = rt.Rows[count];
      row[1].Text = header;
      row[2].Text = value.ToString("N2");
      row[3].Text = EstimateUtil.DivideOrZero(value, this.m_totalTrees).ToString("N2");
      row[4].Text = !this.m_population.HasValue ? i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr : EstimateUtil.DivideOrZero(value, this.m_population.Value).ToString("N2");
      return row;
    }

    private TableRow AddTotalRow(RenderTable rt, string header, Decimal value)
    {
      int count = rt.Rows.Count;
      TableRow row = rt.Rows[count];
      row[0].SpanCols = 2;
      row[0].Text = header;
      row[2].Text = value.ToString("N2");
      row[3].Text = EstimateUtil.DivideOrZero(value, this.m_totalTrees).ToString("N2");
      row[4].Text = !this.m_population.HasValue ? i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr : EstimateUtil.DivideOrZero(value, this.m_population.Value).ToString("N2");
      row.Style.Borders.Top = row.Style.Borders.Bottom = LineDef.Default;
      row.Style.FontBold = true;
      return row;
    }

    private double GetTreeTotalsEstimatedValuesData() => this.GetTreeTotalsEstimatedValues().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>();

    private IQuery GetTreeTotalsEstimatedValues() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValueSum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private DataTable GetPollutionRemovalTons() => this.GetPollutantsEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.PollutionRemoval]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetPollutantsEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedPollutantsValuesByStratum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Pollutant
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.Pollutant]);

    private double GetEstimatedAnualCarbonTotal() => this.GetEstimatedTotalValues().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>();

    private IQuery GetEstimatedTotalValues() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValueSum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private double GetEstimatedAnualAvoidedRunoffTotal() => this.GetEstimatedTotalAvoidedRunoffValues().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.AvoidedRunoff]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.Year)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>();

    private IQuery GetEstimatedTotalAvoidedRunoffValues() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValueSum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    protected override string ReportMessage()
    {
      string str = this.Energy_MKW_MBTU_Footer() + Environment.NewLine + this.Carbon_MetricTon_ShortTon_Footer(i_Tree_Eco_v6.Resources.Strings.NoteGrossCarbonSequestrationValue) + Environment.NewLine + this.AvoidedRunoff_m3_ft3_Footer();
      if (this.m_population.HasValue)
        str = str + Environment.NewLine + string.Format(i_Tree_Eco_v6.Resources.Strings.NoteValuesPerCapita, (object) this.m_population.Value);
      return str;
    }

    public override void SetAlignment(C1PrintDocument C1doc) => C1doc.Style.FlowAlignChildren = FlowAlignEnum.Near;
  }
}
