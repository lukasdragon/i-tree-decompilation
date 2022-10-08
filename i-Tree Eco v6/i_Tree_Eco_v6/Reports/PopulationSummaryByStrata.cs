// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PopulationSummaryByStrata
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  internal class PopulationSummaryByStrata : DatabaseReport
  {
    public PopulationSummaryByStrata() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitlePopulationSummaryByStratum;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      SortedList<int, PopulationSummaryByStrata.totalTreeObject> data = (SortedList<int, PopulationSummaryByStrata.totalTreeObject>) this.GetData();
      int studyAreaKey = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area");
      double numTrees;
      if (data.ContainsKey(studyAreaKey) && data.Count == 2)
      {
        int key = data.First<KeyValuePair<int, PopulationSummaryByStrata.totalTreeObject>>((Func<KeyValuePair<int, PopulationSummaryByStrata.totalTreeObject>, bool>) (item => item.Key != studyAreaKey)).Key;
        if (this.curYear.RecordStrata)
        {
          data.Remove(studyAreaKey);
          numTrees = data[key].NumTrees;
        }
        else
        {
          data.Remove(key);
          numTrees = data[studyAreaKey].NumTrees;
        }
      }
      else
        numTrees = data[studyAreaKey].NumTrees;
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      c1Chart.Style.Font = new Font("Calibri", 14f);
      c1Chart.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      c1Chart.BackColor = Color.White;
      c1Chart.ChartArea.AxisY.Text = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
      c1Chart.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      ReportUtil.SetChartOptions(c1Chart, true);
      ChartDataSeries chartDataSeries1 = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries1.LineStyle.Color = ReportUtil.GetColor(1);
      for (int index = 0; index < data.Count; ++index)
      {
        string str = this.estUtil.ClassValues[Classifiers.Strata][(short) data.Keys[index]].Item1;
        if (data.Keys.Count > 1 && str != "Study Area" || data.Count == 1)
        {
          chartDataSeries1.X.Add((object) index);
          chartDataSeries1.Y.Add((object) data.Values[index].NumTrees);
          c1Chart.ChartArea.AxisX.ValueLabels.Add((object) index, this.estUtil.ClassValues[Classifiers.Strata][(short) data.Values[index].StrataCVO].Item1);
        }
      }
      RenderObject chartRenderObject1 = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 1.0, 0.79);
      chartRenderObject1.BreakAfter = BreakEnum.Page;
      C1doc.Body.Children.Add(chartRenderObject1);
      C1.Win.C1Chart.C1Chart ChartIn = new C1.Win.C1Chart.C1Chart();
      ChartIn.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Pie;
      ChartIn.Style.BackColor = Color.White;
      ChartIn.Style.Font = new Font("Calibri", 14f);
      ChartIn.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      ChartIn.Legend.Visible = true;
      for (int index = 0; index < data.Count; ++index)
      {
        if (data.Count == 1 || this.estUtil.ClassValues[Classifiers.Strata][(short) data.Values[index].StrataCVO].Item1 != "Study Area")
        {
          ChartDataSeries chartDataSeries2 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
          chartDataSeries2.LineStyle.Color = ReportUtil.GetColor(index);
          chartDataSeries2.X.Add((object) 0);
          double num = data.Values[index].NumTrees / numTrees;
          chartDataSeries2.Y.Add((object) num);
          chartDataSeries2.Label = num.ToString("P1");
          Label label = ChartIn.ChartLabels.LabelsCollection.AddNewLabel();
          label.AttachMethod = AttachMethodEnum.DataIndex;
          AttachMethodData attachMethodData = label.AttachMethodData;
          attachMethodData.GroupIndex = 0;
          attachMethodData.SeriesIndex = index;
          attachMethodData.PointIndex = 0;
          label.Text = this.estUtil.ClassValues[Classifiers.Strata][(short) data.Values[index].StrataCVO].Item1;
          label.Compass = LabelCompassEnum.Radial;
          label.Connected = true;
          label.Offset = 40;
          label.Visible = true;
          chartDataSeries2.LegendEntry = true;
        }
      }
      RenderObject chartRenderObject2 = (RenderObject) ReportUtil.CreateChartRenderObject(ChartIn, g, C1doc, 1.0, 0.79);
      C1doc.Body.Children.Add(chartRenderObject2);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Insert(0, (RenderObject) renderTable);
      renderTable.Width = (Unit) "70%";
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfPopulation;
      int num1 = count;
      for (int index = 0; index < data.Count; ++index)
      {
        renderTable.Cells[num1, 0].Text = this.estUtil.ClassValues[Classifiers.Strata][(short) data.Values[index].StrataCVO].Item1;
        if (renderTable.Cells[num1, 0].Text == "Study Area")
        {
          renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          renderTable.Rows[num1].Style.Borders.Top = LineDef.Default;
          renderTable.Rows[num1].Style.FontBold = true;
        }
        renderTable.Cells[num1, 1].Text = data.Values[index].NumTrees.ToString("N0");
        renderTable.Cells[num1, 2].Text = (data.Values[index].NumTrees / numTrees).ToString("P1");
        ++num1;
      }
      ReportUtil.FormatRenderTable(renderTable);
    }

    public override object GetData()
    {
      DataTable countsWithTotals = this.GetCountsWithTotals();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      SortedList<int, PopulationSummaryByStrata.totalTreeObject> data = new SortedList<int, PopulationSummaryByStrata.totalTreeObject>();
      foreach (DataRow row in (InternalDataCollectionBase) countsWithTotals.Rows)
      {
        int key = ReportUtil.ConvertFromDBVal<int>(row[classifierName]);
        data.Add(key, new PopulationSummaryByStrata.totalTreeObject()
        {
          StrataCVO = key,
          NumTrees = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"])
        });
      }
      return (object) data;
    }

    private DataTable GetCountsWithTotals() => this.GetEstimateValuesWithSEAndTotals().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimateValuesWithSEAndTotals()
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSEAndTotals(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata
      })], classifierName);
    }

    private class totalTreeObject
    {
      public int StrataCVO { get; set; }

      public double NumTrees { get; set; }
    }
  }
}
