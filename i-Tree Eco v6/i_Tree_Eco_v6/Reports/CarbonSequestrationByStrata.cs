// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.CarbonSequestrationByStrata
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
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
  internal class CarbonSequestrationByStrata : DatabaseReport
  {
    public CarbonSequestrationByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleAnnualCarbonSequestrationOfTreesByStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      SortedList<int, double> sequestrationSortedList = this.GetCarbonSequestrationSortedList(this.GetCarbonSequestration());
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontBold = true;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[1, 0].Text = string.Empty;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.GrossCarbonSequestration;
      renderTable.Cells[1, 1].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonneUnits());
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.CO2Equivalent;
      renderTable.Cells[1, 2].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonneUnits());
      int num1 = count;
      for (int index = 0; index < sequestrationSortedList.Count; ++index)
      {
        string str = this.estUtil.ClassValues[Classifiers.Strata][(short) sequestrationSortedList.Keys[index]].Item1;
        renderTable.Cells[num1, 0].Text = sequestrationSortedList.Keys.Count != 1 || this.curYear.RecordStrata ? str : i_Tree_Eco_v6.Resources.Strings.StudyArea;
        double english = EstimateUtil.ConvertToEnglish(sequestrationSortedList.Values[index], Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits);
        renderTable.Cells[num1, 1].Text = english.ToString("N2");
        renderTable.Cells[num1, 2].Text = (english * 3.667).ToString("N2");
        ++num1;
      }
      if (sequestrationSortedList.Count > 1)
      {
        renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
        double english = EstimateUtil.ConvertToEnglish(Enumerable.Sum(sequestrationSortedList.Values), Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits);
        renderTable.Cells[num1, 1].Text = english.ToString("N2");
        renderTable.Cells[num1, 2].Text = (english * 3.667).ToString("N2");
        renderTable.Rows[num1].Style.FontBold = true;
        renderTable.Rows[num1].Style.Borders.Top = LineDef.Default;
      }
      ReportUtil.FormatRenderTable(renderTable);
      C1.Win.C1Chart.C1Chart ChartIn = new C1.Win.C1Chart.C1Chart();
      ChartIn.Style.Font = new Font("Calibri", 14f);
      ChartIn.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      ChartIn.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      ChartIn.BackColor = Color.White;
      ChartIn.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.GrossCarbonSequestration, ReportUtil.GetValuePerYrStr(ReportBase.TonneUnits()));
      ChartIn.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      ChartIn.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels;
      ChartIn.ChartArea.AxisX.AnnotationRotation = -45;
      ChartIn.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      ChartIn.ChartArea.Margins.Top = 0;
      ChartDataSeries chartDataSeries1 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries1.LineStyle.Color = ReportUtil.GetColor(1);
      for (int index = 0; index < sequestrationSortedList.Keys.Count; ++index)
      {
        string str = this.estUtil.ClassValues[Classifiers.Strata][(short) sequestrationSortedList.Keys[index]].Item1;
        if (sequestrationSortedList.Keys.Count > 1 && str != "Study Area" || sequestrationSortedList.Keys.Count == 1)
        {
          chartDataSeries1.X.Add((object) index);
          chartDataSeries1.Y.Add((object) EstimateUtil.ConvertToEnglish(sequestrationSortedList.Values[index], Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits));
          ChartIn.ChartArea.AxisX.ValueLabels.Add((object) index, sequestrationSortedList.Keys.Count != 1 || this.curYear.RecordStrata ? str : i_Tree_Eco_v6.Resources.Strings.StudyArea);
        }
      }
      ChartIn.GetImage();
      ChartIn.ChartArea.AxisY2.Visible = true;
      ChartIn.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CO2Equivalent, ReportUtil.GetValuePerYrStr(ReportBase.TonneUnits()));
      ChartDataSeries chartDataSeries2 = ChartIn.ChartGroups.Group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries2.X.Add((object) 1);
      chartDataSeries2.Y.Add((object) (chartDataSeries1.MaxY * 3.667));
      chartDataSeries2.Display = SeriesDisplayEnum.Hide;
      double num2 = chartDataSeries1.MinY > 0.0 ? 0.0 : chartDataSeries1.MinY * 1.15;
      ChartIn.ChartArea.AxisY.Min = num2;
      ChartIn.ChartArea.AxisY.Max = chartDataSeries1.MaxY * 1.15;
      double num3 = chartDataSeries1.MinY > 0.0 ? 0.0 : chartDataSeries1.MinY * 1.15 * 3.667;
      ChartIn.ChartArea.AxisY2.Min = num3;
      ChartIn.ChartArea.AxisY2.Max = chartDataSeries1.MaxY * 1.15 * 3.667;
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(ChartIn, g, C1doc, 1.0, 0.79);
      C1doc.Body.Children.Add(chartRenderObject);
      if (!this.ProjectIsUsingTropicalEquations())
        return;
      this.Note(C1doc);
    }

    private SortedList<int, double> GetCarbonSequestrationSortedList(
      DataTable estimatedValues)
    {
      SortedList<int, double> sequestrationSortedList = new SortedList<int, double>();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
        sequestrationSortedList.Add(ReportUtil.ConvertFromDBVal<int>(row[classifierName]), ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]));
      return sequestrationSortedList;
    }

    private DataTable GetCarbonSequestration() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery()
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSE(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata
      })], classifierName);
    }

    protected override string ReportMessage() => i_Tree_Eco_v6.Resources.Strings.MsgTropicalEquations;
  }
}
