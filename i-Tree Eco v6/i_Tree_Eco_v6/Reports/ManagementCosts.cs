// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ManagementCosts
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Services;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class ManagementCosts : DatabaseReport
  {
    private Year m_year;
    private Project m_project;
    private YearlyCost m_costs;
    private Decimal m_totalTrees;
    private Decimal? m_population;
    private LocationService _locationService;

    public ManagementCosts()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleManagementCostsByExpenditure;
      this._locationService = new LocationService(ReportBase.m_ps.LocSp);
      this.Init();
    }

    private void Init()
    {
      this.m_totalTrees = Convert.ToDecimal(this.GetEstimatedValuesData());
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
      if (this.m_population.HasValue)
        return;
      int? population = (int?) this._locationService.GetLocation(this.m_project.LocationId)?.Population;
      this.m_population = population.HasValue ? new Decimal?((Decimal) population.GetValueOrDefault()) : new Decimal?();
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      Decimal num = 0M;
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Style.FontSize = 10f;
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 11f;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Expenditures;
      renderTable.Cells[0, 1].Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCost, (object) i_Tree_Eco_v6.Resources.Strings.Total, (object) this.CurrencySymbol);
      renderTable.Cells[0, 2].Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) this.CurrencySymbol, (object) v6Strings.Tree_SingularName.ToLower());
      renderTable.Cells[0, 3].Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) this.CurrencySymbol, (object) i_Tree_Eco_v6.Resources.Strings.Capita);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.PurchasingTreesAndPlanting, this.m_costs.Planting);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.ContractPruning, this.m_costs.Pruning);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.PestManagement, this.m_costs.PestControl);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.Irrigation, this.m_costs.Irrigation);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.Removal, this.m_costs.TreeRemoval);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.Administration, this.m_costs.Administrative);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.InspectionService, this.m_costs.Inspection);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.InfrastructureRepairs, this.m_costs.Repair);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.LitterCleanup, this.m_costs.CleanUp);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.LiabilityClaims, this.m_costs.Legal);
      this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.OtherCosts, this.m_costs.Other);
      Decimal cost = num + this.m_costs.Planting + this.m_costs.Pruning + this.m_costs.PestControl + this.m_costs.Irrigation + this.m_costs.TreeRemoval + this.m_costs.Administrative + this.m_costs.Inspection + this.m_costs.Repair + this.m_costs.CleanUp + this.m_costs.Legal + this.m_costs.Other;
      TableRow tableRow = this.AddExpenseRow(renderTable, i_Tree_Eco_v6.Resources.Strings.TotalExpenditures, cost);
      tableRow.Style.Borders.Top = LineDef.Default;
      tableRow.Style.FontBold = true;
      ReportUtil.FormatRenderTable(renderTable);
      this.Note(C1doc);
    }

    private TableRow AddExpenseRow(RenderTable table, string expense, Decimal cost)
    {
      int count = table.Rows.Count;
      TableRow row = table.Rows[count];
      row[0].Text = expense;
      row[1].Text = cost.ToString("N2");
      row[2].Text = EstimateUtil.DivideOrZero(cost, this.m_totalTrees).ToString("N2");
      row[3].Text = !this.m_population.HasValue ? i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr : EstimateUtil.DivideOrZero(cost, this.m_population.Value).ToString("N2");
      return row;
    }

    protected override string ReportMessage()
    {
      string str = i_Tree_Eco_v6.Resources.Strings.NoteManagementCostsSettings;
      if (this.m_population.HasValue)
        str = str + Environment.NewLine + string.Format(i_Tree_Eco_v6.Resources.Strings.NoteValuesPerCapita, (object) this.m_population.Value);
      return str;
    }

    public override void SetAlignment(C1PrintDocument C1doc) => C1doc.Style.FlowAlignChildren = FlowAlignEnum.Near;

    private double GetEstimatedValuesData() => this.GetEstimatedValues().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>();

    private IQuery GetEstimatedValues() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValueSum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);
  }
}
