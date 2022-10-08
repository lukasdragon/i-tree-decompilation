// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.CarbonStorageOfTreesByStrataPerUnitArea
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
using System.Text;

namespace i_Tree_Eco_v6.Reports
{
  internal class CarbonStorageOfTreesByStrataPerUnitArea : DatabaseReport
  {
    public CarbonStorageOfTreesByStrataPerUnitArea() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleCarbonStorageOfTreesByStratumPerUnitArea;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      SortedList<int, double> data = (SortedList<int, double>) this.GetData();
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      c1Chart.Style.Font = new Font("Calibri", 14f);
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      c1Chart.BackColor = Color.White;
      c1Chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CarbonStorage, ReportUtil.GetValuePerValueStr(ReportBase.KgUnits(), ReportBase.HaUnits()));
      c1Chart.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      ReportUtil.SetChartOptions(c1Chart, true);
      ChartDataSeries chartDataSeries1 = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries1.LineStyle.Color = ReportUtil.GetColor(1);
      for (int index = 0; index < data.Keys.Count; ++index)
      {
        string str = this.estUtil.ClassValues[Classifiers.Strata][(short) data.Keys[index]].Item1;
        if (data.Keys.Count > 1 && str != "Study Area" || data.Count == 1)
        {
          chartDataSeries1.X.Add((object) index);
          chartDataSeries1.Y.Add((object) EstimateUtil.ConvertToEnglish(data.Values[index], Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.Hectare, Units.None), ReportBase.EnglishUnits));
          c1Chart.ChartArea.AxisX.ValueLabels.Add((object) index, data.Keys.Count != 1 || this.curYear.RecordStrata ? str : i_Tree_Eco_v6.Resources.Strings.StudyArea);
        }
      }
      c1Chart.GetImage();
      c1Chart.ChartArea.AxisY2.Visible = true;
      c1Chart.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CO2Equivalent, ReportUtil.GetValuePerValueStr(ReportBase.KgUnits(), ReportBase.HaUnits()));
      ChartDataSeries chartDataSeries2 = c1Chart.ChartGroups.Group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries2.X.Add((object) 1);
      chartDataSeries2.Y.Add((object) (chartDataSeries1.MaxY * 3.667));
      chartDataSeries2.Display = SeriesDisplayEnum.Hide;
      double num1 = chartDataSeries1.MinY > 0.0 ? 0.0 : chartDataSeries1.MinY * 1.15;
      c1Chart.ChartArea.AxisY.Min = num1;
      c1Chart.ChartArea.AxisY.Max = chartDataSeries1.MaxY * 1.15;
      double num2 = chartDataSeries1.MinY > 0.0 ? 0.0 : chartDataSeries1.MinY * 1.15 * 3.667;
      c1Chart.ChartArea.AxisY2.Min = num2;
      c1Chart.ChartArea.AxisY2.Max = chartDataSeries1.MaxY * 1.15 * 3.667;
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 1.0, 0.79);
      C1doc.Body.Children.Add(chartRenderObject);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Insert(0, (RenderObject) renderTable);
      renderTable.Width = Unit.Auto;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontBold = true;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable.Cells[1, 1].Text = ReportUtil.FormatHeaderUnitsStr(ReportUtil.GetValuePerValueStr(ReportBase.KgUnits(), ReportBase.HaUnits()));
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.CO2Equivalent;
      renderTable.Cells[1, 2].Text = ReportUtil.FormatHeaderUnitsStr(ReportUtil.GetValuePerValueStr(ReportBase.KgUnits(), ReportBase.HaUnits()));
      int num3 = count;
      for (int index = 0; index < data.Count; ++index)
      {
        string studyArea = this.estUtil.ClassValues[Classifiers.Strata][(short) data.Keys[index]].Item1;
        if (studyArea == "Study Area" || data.Count == 1)
        {
          studyArea = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          renderTable.Rows[num3].Style.Borders.Top = LineDef.Default;
          renderTable.Rows[num3].Style.FontBold = true;
        }
        if (data.Count == 1 && !this.curYear.RecordStrata)
        {
          studyArea = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          renderTable.Rows[num3].Style.FontBold = true;
        }
        renderTable.Cells[num3, 0].Text = studyArea;
        renderTable.Cells[num3, 1].Text = EstimateUtil.ConvertToEnglish(data.Values[index], Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.Hectare, Units.None), ReportBase.EnglishUnits).ToString("N1");
        renderTable.Cells[num3, 2].Text = (EstimateUtil.ConvertToEnglish(data.Values[index], Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.Hectare, Units.None), ReportBase.EnglishUnits) * 3.667).ToString("N1");
        ++num3;
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.Note(C1doc);
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
      SortedList<int, double> data = new SortedList<int, double>();
      foreach (DataRow row in (InternalDataCollectionBase) sequestrationWithTotals.Rows)
        data.Add(ReportUtil.ConvertFromDBVal<int>(row[classifierName]), ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]) * 1000.0);
      if (data.Count == 2)
        data.RemoveAt(1);
      return (object) data;
    }

    private DataTable GetCarbonSequestrationWithTotals() => this.GetEstimateValuesWithSEAndTotals().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CarbonStorage]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Hectare, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimateValuesWithSEAndTotals()
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSEAndTotals(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata
      })], classifierName);
    }
  }
}
