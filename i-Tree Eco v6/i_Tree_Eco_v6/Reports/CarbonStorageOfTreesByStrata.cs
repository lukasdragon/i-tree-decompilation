// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.CarbonStorageOfTreesByStrata
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
using System.Text;

namespace i_Tree_Eco_v6.Reports
{
  public class CarbonStorageOfTreesByStrata : DatabaseReport
  {
    public CarbonStorageOfTreesByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleCarbonStorageOfTreesByStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.PageLayout.PageSettings.Landscape = false;
      List<(short, double)> data = (List<(short, double)>) this.GetData();
      double num1 = data.Sum<(short, double)>((Func<(short, double), double>) (x => x.carbon));
      List<(short, double)> list = data.OrderByDescending<(short, double), double>((Func<(short, double), double>) (x => x.carbon)).ToList<(short, double)>();
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Width = Unit.Auto;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontBold = true;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable.Cells[1, 1].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonneUnits());
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable.Cells[1, 2].Text = ReportUtil.FormatHeaderUnitsStr("%");
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.CO2Equivalent;
      renderTable.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonneUnits());
      int num2 = count;
      foreach ((int key, double d) in list)
      {
        renderTable.Cells[num2, 0].Text = this.estUtil.ClassValues[Classifiers.Strata][(short) key].Item1;
        renderTable.Cells[num2, 1].Text = ReportBase.FormatDoubleValue1((object) EstimateUtil.ConvertToEnglish(d, Units.MetricTons, ReportBase.EnglishUnits));
        renderTable.Cells[num2, 2].Text = (d / num1).ToString("P1");
        double english = EstimateUtil.ConvertToEnglish(d, Units.MetricTons, ReportBase.EnglishUnits);
        renderTable.Cells[num2, 3].Text = ReportBase.FormatDoubleValue1((object) UnitsHelper.CarbonToCarbonDioxide(english));
        ++num2;
      }
      if (list.Count == 1)
      {
        --num2;
        if (!this.curYear.RecordStrata)
          renderTable.Cells[num2, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
      }
      else
      {
        renderTable.Cells[num2, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
        renderTable.Cells[num2, 1].Text = ReportBase.FormatDoubleValue1((object) EstimateUtil.ConvertToEnglish(num1, Units.MetricTons, ReportBase.EnglishUnits));
        renderTable.Cells[num2, 2].Text = "100%";
        double english = EstimateUtil.ConvertToEnglish(num1, Units.MetricTons, ReportBase.EnglishUnits);
        renderTable.Cells[num2, 3].Text = ReportBase.FormatDoubleValue1((object) UnitsHelper.CarbonToCarbonDioxide(english));
      }
      renderTable.Rows[num2].Style.Borders.Top = LineDef.Default;
      renderTable.Rows[num2].Style.FontBold = true;
      ReportUtil.FormatRenderTable(renderTable);
      this.Note(C1doc);
      if (list.Count <= 1)
        return;
      this.GenerateCharts(C1doc, g, (IList<(short, double)>) list, num1);
    }

    public void GenerateCharts(
      C1PrintDocument C1doc,
      Graphics g,
      IList<(short stratum, double carbon)> carbonStorage,
      double totCarbon)
    {
      short num1 = 11;
      short count = 50;
      bool flag = carbonStorage.Count > (int) count;
      int num2 = flag ? (int) count : carbonStorage.Count;
      IList<(short, double)> list = (IList<(short, double)>) carbonStorage.Select<(short, double), (short, double)>((Func<(short, double), (short, double)>) (x => (x.stratum, x.carbon))).Take<(short, double)>((int) count).ToList<(short, double)>();
      C1.Win.C1Chart.C1Chart reportBarChart = this.GenerateReportBarChart(list);
      reportBarChart.Header.Visible = true;
      reportBarChart.Header.Text = string.Format("Carbon Storage by Stratum {0}", flag ? (object) string.Format("(top {0})", (object) num2) : (object) string.Empty);
      RenderObject chartRenderObject1 = (RenderObject) ReportUtil.CreateChartRenderObject(reportBarChart, g, C1doc, 0.9, 0.9);
      chartRenderObject1.BreakBefore = BreakEnum.Page;
      C1doc.Body.Children.Add(chartRenderObject1);
      if (carbonStorage.Count >= (int) num1)
        return;
      RenderObject chartRenderObject2 = (RenderObject) ReportUtil.CreateChartRenderObject(this.GenerateReportPieChart(list, totCarbon), g, C1doc, 1.0, 0.8);
      chartRenderObject2.BreakBefore = BreakEnum.Page;
      C1doc.Body.Children.Add(chartRenderObject2);
    }

    private C1.Win.C1Chart.C1Chart GenerateReportBarChart(IList<(short, double)> data)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      ReportUtil.SetChartOptions(chart, true);
      chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CO2Equivalent, ReportBase.TonneUnits());
      chart.ChartArea.AxisY2.Visible = true;
      chart.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CarbonStorage, ReportBase.TonneUnits());
      chart.ChartArea.Inverted = true;
      chart.ChartArea.AxisX.AnnotationRotation = 0;
      chart.ChartArea.Style.HorizontalAlignment = C1.Win.C1Chart.AlignHorzEnum.Near;
      ChartDataSeries chartDataSeries1 = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries1.LineStyle.Color = ReportUtil.GetColor(1);
      int val = 0;
      foreach ((short key, double d) in (IEnumerable<(short, double)>) data.OrderBy<(short, double), double>((Func<(short, double), double>) (x => x.Item2)))
      {
        chartDataSeries1.X.Add((object) val);
        chart.ChartArea.AxisX.ValueLabels.Add((object) val, data.Count != 1 || this.curYear.RecordStrata ? (this.estUtil.ClassValues[Classifiers.Strata].ContainsKey(key) ? this.estUtil.ClassValues[Classifiers.Strata][key].Item1 : i_Tree_Eco_v6.Resources.Strings.Other) : i_Tree_Eco_v6.Resources.Strings.StudyArea);
        chartDataSeries1.Y.Add((object) UnitsHelper.CarbonToCarbonDioxide(EstimateUtil.ConvertToEnglish(d, Units.MetricTons, ReportBase.EnglishUnits)));
        ++val;
      }
      chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels;
      ValueLabel valueLabel = chart.ChartArea.AxisX.ValueLabels.AddNewLabel();
      valueLabel.Appearance = ValueLabelAppearanceEnum.TriangleMarker;
      valueLabel.MarkerSize = 50;
      valueLabel.Color = Color.Transparent;
      valueLabel.NumericValue = 2.0;
      ChartDataSeries chartDataSeries2 = chart.ChartGroups.Group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries2.X.Add((object) 1);
      chartDataSeries2.Y.Add((object) chartDataSeries1.MaxY);
      chartDataSeries2.Display = SeriesDisplayEnum.Hide;
      double num1 = chartDataSeries1.MinY > 0.0 ? 0.0 : chartDataSeries1.MinY * 1.15;
      chart.ChartArea.AxisY.Min = num1;
      chart.ChartArea.AxisY.Max = chartDataSeries1.MaxY * 1.15;
      double num2 = chartDataSeries1.MinY > 0.0 ? 0.0 : UnitsHelper.CarbonDioxideToCarbon(chartDataSeries1.MinY * 1.15);
      chart.ChartArea.AxisY2.Min = num2;
      chart.ChartArea.AxisY2.Max = UnitsHelper.CarbonDioxideToCarbon(chartDataSeries1.MaxY * 1.15);
      return chart;
    }

    private C1.Win.C1Chart.C1Chart GenerateReportPieChart(
      IList<(short, double)> data,
      double totCarbon)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetPieChartStyle(chart);
      int num1 = 0;
      int num2 = 80;
      int num3 = 0;
      foreach ((short key, double num4) in (IEnumerable<(short, double)>) data)
      {
        ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries.LineStyle.Color = ReportUtil.GetColor(num1);
        double num5 = totCarbon;
        double num6 = num4 / num5;
        chartDataSeries.Label = num6.ToString("P1");
        chartDataSeries.X.Add((object) 0);
        chartDataSeries.Y.Add((object) num6);
        Label label1 = chart.ChartLabels.LabelsCollection.AddNewLabel();
        label1.AttachMethod = AttachMethodEnum.DataIndex;
        label1.Text = data.Count == 1 ? i_Tree_Eco_v6.Resources.Strings.StudyArea : (this.estUtil.ClassValues[Classifiers.Strata].ContainsKey(key) ? this.estUtil.ClassValues[Classifiers.Strata][key].Item1 : i_Tree_Eco_v6.Resources.Strings.Other);
        if (num3 < label1.Text.Length)
          num3 = label1.Text.Length;
        this.SetDefaultLabelStyle(num1, label1);
        label1.Offset = num2;
        label1.Connected = true;
        label1.Compass = LabelCompassEnum.Radial;
        Label label2 = chart.ChartLabels.LabelsCollection.AddNewLabel();
        this.SetDefaultLabelStyle(num1, label2);
        label2.Text = chartDataSeries.Label;
        label2.Offset = -4;
        label2.Compass = LabelCompassEnum.RadialText;
        ++num1;
      }
      int num7 = num3 * 2 + num2;
      chart.ChartArea.Margins.SetMargins(num7, num7, num7, num7);
      chart.PerformAutoScale();
      return chart;
    }

    private void SetDefaultLabelStyle(int i, Label label)
    {
      label.AttachMethod = AttachMethodEnum.DataIndex;
      label.Style.Font = new Font("Calibri", 10f);
      label.Visible = true;
      AttachMethodData attachMethodData = label.AttachMethodData;
      attachMethodData.GroupIndex = 0;
      attachMethodData.SeriesIndex = i;
      attachMethodData.PointIndex = 0;
    }

    protected override string ReportMessage()
    {
      StringBuilder stringBuilder = new StringBuilder(i_Tree_Eco_v6.Resources.Strings.NoteCarbonStorageLimit);
      if (this.ProjectIsUsingTropicalEquations())
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.MsgTropicalEquations);
      }
      return stringBuilder.ToString();
    }

    public override object GetData()
    {
      DataTable sequestrationWithTotals = this.GetCarbonSequestrationWithTotals();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      List<(short, double)> data = new List<(short, double)>();
      foreach (DataRow row in (InternalDataCollectionBase) sequestrationWithTotals.Rows)
        data.Add((ReportUtil.ConvertFromDBVal<short>(row[classifierName]), ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"])));
      return (object) data;
    }

    private DataTable GetCarbonSequestrationWithTotals() => this.GetEstimateValuesWithSEAndTotals().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CarbonStorage]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimateValuesWithSEAndTotals()
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSE(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata
      })], classifierName);
    }
  }
}
