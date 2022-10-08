// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.CarbonSequestrationBySpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  internal class CarbonSequestrationBySpecies : DatabaseReport
  {
    public CarbonSequestrationBySpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleAnnualCarbonSequestrationOfTreesBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.GrossCarbonSequestration;
      renderTable.Cells[1, 1].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonneUnits());
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.CO2Equivalent;
      renderTable.Cells[1, 2].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonneUnits());
      SortedList<int, double> sequestrationSortedList = this.GetCarbonSequestrationSortedList(this.GetCarbonSequestration());
      int index;
      for (index = 0; index < sequestrationSortedList.Count; ++index)
      {
        int row = index + count;
        Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][(short) sequestrationSortedList.Keys[index]];
        renderTable.Cells[row, 0].Text = ReportBase.ScientificName ? tuple.Item2 : tuple.Item1;
        renderTable.Cells[row, 1].Text = EstimateUtil.ConvertToEnglish(sequestrationSortedList.Values[index], Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits).ToString("N2");
        renderTable.Cells[row, 2].Text = (EstimateUtil.ConvertToEnglish(sequestrationSortedList.Values[index], Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits) * 3.667).ToString("N2");
      }
      renderTable.Cells[index + count, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      renderTable.Cells[index + count, 1].Text = EstimateUtil.ConvertToEnglish(Enumerable.Sum(sequestrationSortedList.Values), Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits).ToString("N2");
      renderTable.Cells[index + count, 2].Text = (EstimateUtil.ConvertToEnglish(Enumerable.Sum(sequestrationSortedList.Values), Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits) * 3.667).ToString("N2");
      renderTable.Rows[index + count].Style.FontBold = true;
      renderTable.Rows[index + count].Style.Borders.Top = LineDef.Default;
      ReportUtil.FormatRenderTable(renderTable);
      if (!this.ProjectIsUsingTropicalEquations())
        return;
      this.Note(C1doc);
    }

    protected override string ReportMessage() => i_Tree_Eco_v6.Resources.Strings.MsgTropicalEquations;

    private SortedList<int, double> GetCarbonSequestrationSortedList(
      DataTable estimatedValues)
    {
      SortedList<int, double> sequestrationSortedList = new SortedList<int, double>();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
        sequestrationSortedList.Add(ReportUtil.ConvertFromDBVal<int>(row[classifierName]), ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]));
      return sequestrationSortedList;
    }

    private DataTable GetCarbonSequestration() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery()
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSE(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Species
      })], classifierName);
    }
  }
}
