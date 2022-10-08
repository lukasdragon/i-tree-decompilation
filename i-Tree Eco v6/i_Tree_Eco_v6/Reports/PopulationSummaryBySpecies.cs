// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PopulationSummaryBySpecies
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  internal class PopulationSummaryBySpecies : DatabaseReport
  {
    public PopulationSummaryBySpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTtilePopulationSummaryBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Width = (Unit) "70%";
      renderTable.Cols[0].Width = (Unit) "20%";
      renderTable.BreakAfter = BreakEnum.Page;
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPopulation;
      DataTable treeCounts = this.GetTreeCounts();
      treeCounts.DefaultView.Sort = "EstimateValue desc";
      DataTable table = treeCounts.DefaultView.ToTable();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      double num1 = table.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (r => r.Field<double>("EstimateValue")));
      int num2 = count;
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][(short) ReportUtil.ConvertFromDBVal<int>(row[classifierName])];
        renderTable.Cells[num2, 0].Text = ReportBase.ScientificName ? tuple.Item2 : tuple.Item1;
        double num3 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        renderTable.Cells[num2, 1].Text = num3.ToString("N0");
        double num4 = Math.Round(num3 / num1, 3);
        renderTable.Cells[num2, 2].Text = num4 >= 0.01 ? num4.ToString("P1") : "<0.1%";
        ++num2;
      }
      renderTable.Cells[num2, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      renderTable.Cells[num2, 1].Text = num1.ToString("N0");
      renderTable.Cells[num2, 2].Text = "100%";
      renderTable.Rows[num2].Style.Borders.Top = LineDef.Default;
      renderTable.Rows[num2].Style.FontBold = true;
      ReportUtil.FormatRenderTable(renderTable);
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      C1.Win.C1Chart.C1Chart ChartIn = new C1.Win.C1Chart.C1Chart();
      ChartIn.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Pie;
      ChartIn.Style.BackColor = Color.White;
      ChartIn.Style.Font = new Font("Calibri", 14f);
      ChartIn.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      double num5 = 0.0;
      int index1 = 0;
      for (int index2 = 0; index2 < table.Rows.Count && index2 < 10; ++index2)
      {
        ChartDataSeries chartDataSeries = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries.LineStyle.Color = ReportUtil.GetColor(index2);
        chartDataSeries.X.Add((object) 0);
        double num6 = ReportUtil.ConvertFromDBVal<double>(table.Rows[index2]["EstimateValue"]) / num1;
        num5 += num6;
        chartDataSeries.Y.Add((object) num6);
        chartDataSeries.Label = num6.ToString("P1");
        Label label = ChartIn.ChartLabels.LabelsCollection.AddNewLabel();
        label.AttachMethod = AttachMethodEnum.DataIndex;
        AttachMethodData attachMethodData = label.AttachMethodData;
        attachMethodData.GroupIndex = 0;
        attachMethodData.SeriesIndex = index2;
        attachMethodData.PointIndex = 0;
        Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][(short) ReportUtil.ConvertFromDBVal<int>(table.Rows[index2][classifierName])];
        label.Text = string.Format("{0} ({1:P1})", ReportBase.ScientificName ? (object) tuple.Item2 : (object) tuple.Item1, (object) num6);
        label.Compass = LabelCompassEnum.Radial;
        label.Connected = true;
        label.Offset = 50;
        label.Visible = true;
        chartDataSeries.LegendEntry = true;
        ++index1;
      }
      double num7 = 1.0 - num5;
      if (table.Rows.Count > 10 && num7 > 0.1)
      {
        ChartDataSeries chartDataSeries = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries.LineStyle.Color = ReportUtil.GetColor(index1);
        chartDataSeries.X.Add((object) 0);
        chartDataSeries.Y.Add((object) num7);
        chartDataSeries.Label = num7.ToString("P1");
        Label label = ChartIn.ChartLabels.LabelsCollection.AddNewLabel();
        label.AttachMethod = AttachMethodEnum.DataIndex;
        AttachMethodData attachMethodData = label.AttachMethodData;
        attachMethodData.GroupIndex = 0;
        attachMethodData.SeriesIndex = index1;
        attachMethodData.PointIndex = 0;
        label.Text = string.Format("{1} ({0:P1})", (object) num7, (object) i_Tree_Eco_v6.Resources.Strings.Other);
        label.Compass = LabelCompassEnum.Radial;
        label.Connected = true;
        label.Offset = 50;
        label.Visible = true;
        chartDataSeries.LegendEntry = true;
      }
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(ChartIn, g, C1doc, 1.0, 0.75);
      C1doc.Body.Children.Add(chartRenderObject);
    }

    private DataTable GetTreeCounts() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetSignificantEstimateValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);
  }
}
