// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.NetCarbonSequestrationByStrata
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

namespace i_Tree_Eco_v6.Reports
{
  internal class NetCarbonSequestrationByStrata : DatabaseReport
  {
    public NetCarbonSequestrationByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleAnnualNetCarbonSequestrationOfTreesByStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      DataTable carbonSequestration = this.GetCarbonSequestration();
      if (carbonSequestration.Rows.Count == 2)
        carbonSequestration.Rows.RemoveAt(1);
      C1.Win.C1Chart.C1Chart ChartIn = new C1.Win.C1Chart.C1Chart();
      ChartIn.Style.Font = new Font("Calibri", 14f);
      ChartIn.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      ChartIn.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      ChartIn.BackColor = Color.White;
      ChartIn.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.NetCarbonSequestration, ReportUtil.GetValuePerYrStr(ReportBase.TonneUnits()));
      ChartIn.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      ChartIn.ChartArea.AxisX.AnnoMethod = AnnotationMethodEnum.ValueLabels;
      ChartIn.ChartArea.AxisX.AnnotationRotation = -45;
      ChartIn.ChartArea.AxisX.TickMinor = TickMarksEnum.None;
      ChartIn.ChartArea.Margins.Top = 0;
      ChartDataSeries chartDataSeries1 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries1.LineStyle.Color = ReportUtil.GetColor(1);
      int val = 0;
      foreach (DataRow row in (InternalDataCollectionBase) carbonSequestration.Rows)
      {
        string str = this.estUtil.ClassValues[Classifiers.Strata][(short) ReportUtil.ConvertFromDBVal<int>(row[classifierName])].Item1;
        if (carbonSequestration.Rows.Count > 1 && str != "Study Area" || carbonSequestration.Rows.Count == 1)
        {
          chartDataSeries1.X.Add((object) val);
          chartDataSeries1.Y.Add((object) EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]), Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits));
          ChartIn.ChartArea.AxisX.ValueLabels.Add((object) val, carbonSequestration.Rows.Count != 1 || this.curYear.RecordStrata ? str : i_Tree_Eco_v6.Resources.Strings.StudyArea);
        }
        ++val;
      }
      ChartIn.GetImage();
      ChartIn.ChartArea.AxisY2.Visible = true;
      ChartIn.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CO2Equivalent, ReportUtil.GetValuePerYrStr(ReportBase.TonneUnits()));
      ChartDataSeries chartDataSeries2 = ChartIn.ChartGroups.Group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries2.X.Add((object) 1);
      chartDataSeries2.Y.Add((object) (chartDataSeries1.MaxY * 3.667));
      chartDataSeries2.Display = SeriesDisplayEnum.Hide;
      double num1 = chartDataSeries1.MinY > 0.0 ? 0.0 : chartDataSeries1.MinY * 1.15;
      ChartIn.ChartArea.AxisY.Min = num1;
      ChartIn.ChartArea.AxisY.Max = chartDataSeries1.MaxY * 1.15;
      double num2 = chartDataSeries1.MinY > 0.0 ? 0.0 : chartDataSeries1.MinY * 1.15 * 3.667;
      ChartIn.ChartArea.AxisY2.Min = num2;
      ChartIn.ChartArea.AxisY2.Max = chartDataSeries1.MaxY * 1.15 * 3.667;
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(ChartIn, g, C1doc, 1.0, 0.79);
      C1doc.Body.Children.Add(chartRenderObject);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Insert(0, (RenderObject) renderTable);
      renderTable.Width = (Unit) "70%";
      renderTable.Rows[0].Style.FontSize = 14f;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.NetCarbonSequestration;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.CO2Equivalent;
      renderTable.Cells[1, 1].Text = renderTable.Cells[1, 2].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonneUnits());
      int num3 = count;
      foreach (DataRow row in (InternalDataCollectionBase) carbonSequestration.Rows)
      {
        string str = this.estUtil.ClassValues[Classifiers.Strata][(short) ReportUtil.ConvertFromDBVal<int>(row[classifierName])].Item1;
        renderTable.Cells[num3, 0].Text = carbonSequestration.Rows.Count != 1 || this.curYear.RecordStrata ? str : "Study Area";
        if (renderTable.Cells[num3, 0].Text == "Study Area")
        {
          renderTable.Cells[num3, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          renderTable.Rows[num3].Style.Borders.Top = LineDef.Default;
          renderTable.Rows[num3].Style.FontBold = true;
        }
        double english = EstimateUtil.ConvertToEnglish(ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]), Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None), ReportBase.EnglishUnits);
        renderTable.Cells[num3, 1].Text = english.ToString("N2");
        renderTable.Cells[num3, 2].Text = (english * 3.667).ToString("N2");
        ++num3;
      }
      ReportUtil.FormatRenderTable(renderTable);
      if (!this.ProjectIsUsingTropicalEquations())
        return;
      this.Note(C1doc);
    }

    protected override string ReportMessage() => i_Tree_Eco_v6.Resources.Strings.MsgTropicalEquations;

    private DataTable GetCarbonSequestration() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NetCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSEAndTotals(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);
  }
}
