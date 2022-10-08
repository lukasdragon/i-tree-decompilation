// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.SpeciesDistributionByDBHClassChart
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class SpeciesDistributionByDBHClassChart : DatabaseReport
  {
    private List<SpeciesDistributionByDBHClassChart.totalTreeObject> speciesData;
    private SortedList<short, string> dbhRanges;
    private string dbhLabel = string.Empty;
    private string percentLabel = string.Empty;

    public SpeciesDistributionByDBHClassChart() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSpeciesDistributionbyDBHClass;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.Init();
      C1doc.ClipPage = true;
      this.dbhLabel = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.DBHClass, ReportBase.CmUnits());
      this.percentLabel = ReportUtil.FormatHeaderUnitsStr("%");
      RenderObject chartRenderObject1 = (RenderObject) ReportUtil.CreateChartRenderObject(this.RenderStackedBarChart(), g, C1doc, 1.0, 0.88);
      C1doc.Body.Children.Add(chartRenderObject1);
      RenderArea ro = this.RenderDataTable(C1doc);
      C1doc.Body.Children.Add((RenderObject) ro);
      RenderObject chartRenderObject2 = (RenderObject) ReportUtil.CreateChartRenderObject(this.RenderAreaChart(), g, C1doc, 1.0, 0.88);
      C1doc.Body.Children.Add(chartRenderObject2);
      RenderObject chartRenderObject3 = (RenderObject) ReportUtil.CreateChartRenderObject(this.RenderXYPlotChart(), g, C1doc, 1.0, 0.88);
      C1doc.Body.Children.Add(chartRenderObject3);
      int num = 0;
      foreach (SpeciesDistributionByDBHClassChart.totalTreeObject speciesTotals in this.speciesData)
      {
        RenderObject chartRenderObject4 = (RenderObject) ReportUtil.CreateChartRenderObject(this.RenderIndividualChart(speciesTotals, num++), g, C1doc, 1.0, 0.88);
        C1doc.Body.Children.Add(chartRenderObject4);
      }
    }

    private void Init()
    {
      this.speciesData = new List<SpeciesDistributionByDBHClassChart.totalTreeObject>();
      this.dbhRanges = this.estUtil.ConvertDBHRangesToEnglish(ReportBase.EnglishUnits);
      string species = this.estUtil.ClassifierNames[Classifiers.Species];
      string classifierName = this.estUtil.ClassifierNames[Classifiers.CDBH];
      DataTable estimatedDbHsForSpecies = this.GetEstimatedDBHsForSpecies();
      DataTable speciesCounts = this.GetSpeciesCounts();
      int num1 = 1;
      foreach (DataRow row1 in (InternalDataCollectionBase) speciesCounts.Rows)
      {
        if (num1 > 10)
          break;
        SpeciesDistributionByDBHClassChart.totalTreeObject curSpecies = new SpeciesDistributionByDBHClassChart.totalTreeObject()
        {
          SpeciesCVO = ReportUtil.ConvertFromDBVal<int>(row1[species]),
          NumTrees = ReportUtil.ConvertFromDBVal<double>(row1["EstimateValue"]),
          DBHValues = new SortedList<int, double>()
        };
        foreach (DataRow row2 in (InternalDataCollectionBase) estimatedDbHsForSpecies.AsEnumerable().Where<DataRow>((Func<DataRow, bool>) (row => row.Field<int>(species) == curSpecies.SpeciesCVO)).CopyToDataTable<DataRow>().Rows)
        {
          int key = ReportUtil.ConvertFromDBVal<int>(row2[classifierName]);
          if (!curSpecies.DBHValues.ContainsKey(key))
          {
            double num2 = ReportUtil.ConvertFromDBVal<double>(row2["EstimateValue"]);
            curSpecies.DBHValues.Add(key, num2);
          }
        }
        this.speciesData.Add(curSpecies);
        ++num1;
      }
    }

    private DataTable GetEstimatedDBHsForSpecies() => this.GetEstimateValuesWithSEAndTotals().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimateValuesWithSEAndTotals() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValues2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species,
      Classifiers.CDBH
    })], this.estUtil.ClassifierNames[Classifiers.Species], this.estUtil.ClassifierNames[Classifiers.CDBH]);

    private IQuery GetMultipleFieldsData() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetSignificantEstimateValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    private DataTable GetSpeciesCounts()
    {
      DataTable dataTable = this.GetMultipleFieldsData().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      dataTable.DefaultView.Sort = "EstimateValue DESC";
      return dataTable.DefaultView.ToTable();
    }

    private C1.Win.C1Chart.C1Chart RenderStackedBarChart()
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      this.SetDefaultChartStyle(chart);
      chart.ChartGroups[0].Stacked = true;
      chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels;
      chart.ChartArea.AxisX.AnnotationRotation = -45;
      chart.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      chart.ChartArea.AxisY.Min = 0.0;
      chart.ChartArea.AxisY.Max = 100.0;
      chart.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Species;
      chart.ChartArea.AxisY.Text = "(%)";
      chart.ChartArea.AxisY.GridMajor.Visible = true;
      chart.Legend.Text = this.dbhLabel;
      int num1 = 0;
      foreach (SpeciesDistributionByDBHClassChart.totalTreeObject speciesTotals in this.speciesData)
        chart.ChartArea.AxisX.ValueLabels.Add((object) num1++, this.GetSpeciesName(speciesTotals));
      int index = 0;
      foreach (KeyValuePair<short, string> dbhRange in this.dbhRanges)
      {
        ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries.FillStyle.OutlineColor = Color.Transparent;
        chartDataSeries.LineStyle.Color = ReportUtil.GetColor(index);
        chartDataSeries.Label = dbhRange.Value;
        int num2 = 0;
        foreach (SpeciesDistributionByDBHClassChart.totalTreeObject totalTreeObject in this.speciesData)
        {
          chartDataSeries.X.Add((object) num2++);
          if (totalTreeObject.DBHValues.ContainsKey((int) dbhRange.Key))
          {
            double dbhValue = totalTreeObject.DBHValues[(int) dbhRange.Key];
            chartDataSeries.Y.Add((object) dbhValue);
          }
          else
            chartDataSeries.Y.Add((object) 0);
        }
        ++index;
      }
      return chart;
    }

    private RenderArea RenderDataTable(C1PrintDocument C1doc)
    {
      RenderArea ra = new RenderArea();
      this.AssignTableTitle(C1doc, ra);
      RenderTable renderTable = new RenderTable();
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.Never;
      renderTable.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
      renderTable.Style.BackColor = Color.Transparent;
      int count = 3;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
      ReportUtil.AddWrittenReportTableHeaderFormat(renderTable);
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
      renderTable.Cells[0, 1].Text = this.dbhLabel;
      renderTable.Cells[0, 1].SpanCols = this.dbhRanges.Count;
      renderTable.Cols[0].Style.Borders.Right = LineDef.Default;
      int num1 = 1;
      int num2 = 0;
      foreach (string str in (IEnumerable<string>) this.dbhRanges.Values)
      {
        renderTable.Cells[1, num1].Text = str;
        renderTable.Cells[2, num1].Text = "(%)";
        renderTable.Cols[num1].Style.Borders.Right = LineDef.Default;
        ++num1;
        ++num2;
      }
      int maxValue;
      int num3 = (maxValue = (int) byte.MaxValue) / (this.speciesData.Count + 4);
      int row = count;
      foreach (SpeciesDistributionByDBHClassChart.totalTreeObject speciesTotals in this.speciesData)
      {
        maxValue -= num3;
        int index = 0;
        int col = 1;
        foreach (KeyValuePair<short, string> dbhRange in this.dbhRanges)
        {
          short key = dbhRange.Key;
          Color color = ReportUtil.GetColor(index);
          renderTable.Cells[row, 0].Text = this.GetSpeciesName(speciesTotals);
          renderTable.Cells[row, col].Text = speciesTotals.DBHValues[(int) key].ToString("N1");
          renderTable.Cells[row, col].Style.BackColor = Color.FromArgb(maxValue, color);
          ++index;
          ++col;
        }
        ++row;
      }
      ReportUtil.FormatRenderTableWrittenReport(renderTable);
      ra.Children.Add((RenderObject) renderTable);
      return ra;
    }

    private void AssignTableTitle(C1PrintDocument C1doc, RenderArea ra)
    {
      RenderText ro = this.AddTableTitle(C1doc, i_Tree_Eco_v6.Resources.Strings.SpeciesDistributionChartDataTableTitle);
      ra.Children.Add((RenderObject) ro);
    }

    private RenderText AddTableTitle(C1PrintDocument C1doc, string s)
    {
      C1.C1Preview.Style tableTitleStyle = this.GetTableTitleStyle(C1doc);
      RenderText renderText = new RenderText(s);
      renderText.Style.Parents = tableTitleStyle;
      return renderText;
    }

    private C1.C1Preview.Style GetTableTitleStyle(C1PrintDocument C1doc)
    {
      C1.C1Preview.Style tableTitleStyle = C1doc.Style.Children.Add();
      tableTitleStyle.Spacing.Top = (Unit) "1ls";
      tableTitleStyle.Spacing.Bottom = (Unit) 0;
      tableTitleStyle.FontBold = true;
      tableTitleStyle.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      return tableTitleStyle;
    }

    private C1.Win.C1Chart.C1Chart RenderAreaChart()
    {
      C1.Win.C1Chart.C1Chart c1Chart = this.InitChart();
      c1Chart.ChartArea.AxisX.AnnotationRotation = -45;
      c1Chart.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Area;
      c1Chart.ChartGroups.Group0.Use3D = true;
      c1Chart.ChartArea.PlotArea.View3D.Depth = 25;
      c1Chart.ChartArea.PlotArea.View3D.Elevation = 70;
      c1Chart.ChartArea.PlotArea.View3D.Rotation = 70;
      int index = 0;
      foreach (SpeciesDistributionByDBHClassChart.totalTreeObject speciesTotals in this.speciesData)
      {
        ChartDataSeries cds = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        cds.LineStyle.Color = ReportUtil.GetColor(index);
        cds.Label = this.GetSpeciesName(speciesTotals);
        this.AssignXYValues(cds, speciesTotals);
        ++index;
      }
      return c1Chart;
    }

    private C1.Win.C1Chart.C1Chart RenderXYPlotChart()
    {
      C1.Win.C1Chart.C1Chart c1Chart = this.InitChart();
      c1Chart.ChartArea.AxisX.AnnotationRotation = -45;
      c1Chart.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.XYPlot;
      c1Chart.Legend.Visible = true;
      int num = 0;
      foreach (SpeciesDistributionByDBHClassChart.totalTreeObject speciesTotals in this.speciesData)
      {
        ChartDataSeries cds = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        cds.LineStyle.Color = ReportUtil.GetColor(num++);
        cds.LineStyle.Thickness = 2;
        cds.SymbolStyle.Shape = SymbolShapeEnum.None;
        cds.Label = this.GetSpeciesName(speciesTotals);
        this.AssignXYValues(cds, speciesTotals);
      }
      return c1Chart;
    }

    private C1.Win.C1Chart.C1Chart RenderIndividualChart(
      SpeciesDistributionByDBHClassChart.totalTreeObject speciesTotals,
      int colorId)
    {
      C1.Win.C1Chart.C1Chart c1Chart = this.InitChart();
      c1Chart.ChartArea.AxisX.AnnotationRotation = -45;
      c1Chart.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.XYPlot;
      ChartDataSeries cds = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      cds.LineStyle.Color = ReportUtil.GetColor(colorId);
      cds.LineStyle.Thickness = 2;
      cds.SymbolStyle.Shape = SymbolShapeEnum.Circle;
      cds.Label = this.GetSpeciesName(speciesTotals);
      this.AssignXYValues(cds, speciesTotals);
      return c1Chart;
    }

    private C1.Win.C1Chart.C1Chart InitChart()
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      this.SetDefaultChartStyle(chart);
      chart.ChartArea.AxisX.Text = this.dbhLabel;
      chart.ChartArea.AxisY.Text = this.percentLabel;
      this.AssignX(chart);
      return chart;
    }

    private void SetDefaultChartStyle(C1.Win.C1Chart.C1Chart chart)
    {
      chart.BackColor = Color.White;
      chart.Style.Font = new Font("Calibri", 14f);
      chart.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      chart.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels;
      chart.Legend.Visible = true;
    }

    private void AssignX(C1.Win.C1Chart.C1Chart chart)
    {
      foreach (KeyValuePair<short, string> dbhRange in this.dbhRanges)
        chart.ChartArea.AxisX.ValueLabels.Add((object) dbhRange.Key, dbhRange.Value);
    }

    private string GetSpeciesName(
      SpeciesDistributionByDBHClassChart.totalTreeObject speciesTotals)
    {
      Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][(short) speciesTotals.SpeciesCVO];
      return !ReportBase.ScientificName ? tuple.Item1 : tuple.Item2;
    }

    private void AssignXYValues(
      ChartDataSeries cds,
      SpeciesDistributionByDBHClassChart.totalTreeObject speciesTotals)
    {
      foreach (short key in (IEnumerable<short>) this.dbhRanges.Keys)
      {
        cds.X.Add((object) key);
        if (speciesTotals.DBHValues.ContainsKey((int) key))
          cds.Y.Add((object) speciesTotals.DBHValues[(int) key]);
        else
          cds.Y.Add((object) 0);
      }
    }

    private class totalTreeObject
    {
      public int SpeciesCVO { get; set; }

      public double NumTrees { get; set; }

      public SortedList<int, double> DBHValues { get; set; }
    }
  }
}
