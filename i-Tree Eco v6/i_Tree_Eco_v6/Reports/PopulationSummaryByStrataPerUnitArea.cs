// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PopulationSummaryByStrataPerUnitArea
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
  internal class PopulationSummaryByStrataPerUnitArea : DatabaseReport
  {
    public PopulationSummaryByStrataPerUnitArea() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitlePopulationSummaryByStratumPerUnitArea;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable stratumValuesData = this.GetEstimatedStratumValuesData();
      stratumValuesData.DefaultView.Sort = string.Format("{0} desc", (object) this.estUtil.ClassifierNames[Classifiers.Strata]);
      DataTable table = stratumValuesData.DefaultView.ToTable();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      SortedList<int, PopulationSummaryByStrataPerUnitArea.totalTreeObject> source = new SortedList<int, PopulationSummaryByStrataPerUnitArea.totalTreeObject>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
      {
        int key = ReportUtil.ConvertFromDBVal<int>(row[classifierName]);
        double num = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        source.Add(key, new PopulationSummaryByStrataPerUnitArea.totalTreeObject()
        {
          StrataCVO = key,
          NumTrees = num
        });
      }
      int studyArea = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area");
      if (source.ContainsKey(studyArea) && source.Count == 2)
      {
        if (this.curYear.RecordStrata)
        {
          source.Remove(studyArea);
        }
        else
        {
          int key = source.First<KeyValuePair<int, PopulationSummaryByStrataPerUnitArea.totalTreeObject>>((Func<KeyValuePair<int, PopulationSummaryByStrataPerUnitArea.totalTreeObject>, bool>) (item => item.Key != studyArea)).Key;
          source.Remove(key);
        }
      }
      source.Sum<KeyValuePair<int, PopulationSummaryByStrataPerUnitArea.totalTreeObject>>((Func<KeyValuePair<int, PopulationSummaryByStrataPerUnitArea.totalTreeObject>, double>) (p => p.Value.NumTrees));
      string str1 = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.TreeDensity, ReportUtil.GetValuePerValueStr(i_Tree_Eco_v6.Resources.Strings.Number, ReportBase.HaUnits()));
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      c1Chart.Style.Font = new Font("Calibri", 14f);
      c1Chart.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      c1Chart.BackColor = Color.White;
      c1Chart.ChartArea.AxisY.Text = str1;
      c1Chart.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      ReportUtil.SetChartOptions(c1Chart, true);
      ChartDataSeries chartDataSeries = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(1);
      for (int index = 0; index < source.Count; ++index)
      {
        string str2 = this.estUtil.ClassValues[Classifiers.Strata][(short) source.Keys[index]].Item1;
        if (source.Keys.Count > 1 && str2 != "Study Area" || source.Count == 1)
        {
          chartDataSeries.X.Add((object) index);
          chartDataSeries.Y.Add((object) EstimateUtil.ConvertToEnglish(source.Values[index].NumTrees, Tuple.Create<Units, Units, Units>(Units.Count, Units.Hectare, Units.None), ReportBase.EnglishUnits));
          c1Chart.ChartArea.AxisX.ValueLabels.Add((object) index, this.estUtil.ClassValues[Classifiers.Strata][(short) source.Values[index].StrataCVO].Item1);
        }
      }
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 1.0, 0.79);
      chartRenderObject.BreakBefore = BreakEnum.Page;
      C1doc.Body.Children.Add(chartRenderObject);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Insert(0, (RenderObject) renderTable);
      renderTable.Width = (Unit) "70%";
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = str1;
      int num1 = count;
      for (int index = 0; index < source.Count; ++index)
      {
        renderTable.Cells[num1, 0].Text = this.estUtil.ClassValues[Classifiers.Strata][(short) source.Values[index].StrataCVO].Item1;
        if (renderTable.Cells[num1, 0].Text == "Study Area" && source.Count > 1)
        {
          renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          renderTable.Rows[num1].Style.Borders.Top = LineDef.Default;
          renderTable.Rows[num1].Style.FontBold = true;
        }
        renderTable.Cells[num1, 1].Text = EstimateUtil.ConvertToEnglish(source.Values[index].NumTrees, Tuple.Create<Units, Units, Units>(Units.Count, Units.Hectare, Units.None), ReportBase.EnglishUnits).ToString("N1");
        ++num1;
      }
      ReportUtil.FormatRenderTable(renderTable);
    }

    private DataTable GetEstimatedStratumValuesData() => this.GetEstimatedStratumValues().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.Hectare, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatedStratumValues() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSEAndTotals(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private class totalTreeObject
    {
      public int StrataCVO { get; set; }

      public double NumTrees { get; set; }
    }
  }
}
