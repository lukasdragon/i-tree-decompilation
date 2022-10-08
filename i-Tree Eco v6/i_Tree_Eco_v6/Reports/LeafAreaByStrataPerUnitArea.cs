// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.LeafAreaByStrataPerUnitArea
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using Eco.Domain.Properties;
using Eco.Util;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class LeafAreaByStrataPerUnitArea : LeafAreaByStrataBase
  {
    public LeafAreaByStrataPerUnitArea()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleLeafAreaByStratumPerUnitArea;
      this._estUnits = this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.Hectare, Units.None)];
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      SortedList<int, double> data = (SortedList<int, double>) this.GetData();
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      c1Chart.Style.Font = new Font("Calibri", 14f);
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      c1Chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafAreaDensity, ReportUtil.GetValuePerValueStr(this.SquareMeterUnits(), ReportBase.HaUnits()));
      c1Chart.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      ReportUtil.SetChartOptions(c1Chart, true);
      ChartDataSeries chartDataSeries = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(1);
      for (int index = 0; index < data.Count; ++index)
      {
        string text = this.curYear.RecordStrata || data.Count != 1 ? this.estUtil.ClassValues[Classifiers.Strata][(short) data.Keys[index]].Item1 : i_Tree_Eco_v6.Resources.Strings.StudyArea;
        if (data.Keys.Count > 1 && text != i_Tree_Eco_v6.Resources.Strings.StudyArea || data.Keys.Count == 1)
        {
          chartDataSeries.X.Add((object) index);
          chartDataSeries.Y.Add((object) EstimateUtil.ConvertToEnglish(data.Values[index] * 1000000.0, Tuple.Create<Units, Units, Units>(Units.Squaremeter, Units.Hectare, Units.None), ReportBase.EnglishUnits));
          c1Chart.ChartArea.AxisX.ValueLabels.Add((object) index, text);
        }
      }
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 1.0, 0.79);
      C1doc.Body.Children.Add(chartRenderObject);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Insert(0, (RenderObject) renderTable);
      renderTable.Width = (Unit) "50%";
      renderTable.RowGroups[0, 1].Header = TableHeaderEnum.Page;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafAreaDensity, ReportUtil.GetValuePerValueStr(this.SquareMeterUnits(), ReportBase.HaUnits()));
      for (int index = 0; index < data.Count; ++index)
      {
        renderTable.Cells[index + 1, 0].Text = this.curYear.RecordStrata || data.Count != 1 ? this.estUtil.ClassValues[Classifiers.Strata][(short) data.Keys[index]].Item1 : "Study Area";
        if (renderTable.Cells[index + 1, 0].Text == "Study Area")
        {
          renderTable.Cells[index + 1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          renderTable.Rows[index + 1].Style.Borders.Top = LineDef.Default;
          renderTable.Rows[index + 1].Style.FontBold = true;
        }
        renderTable.Cells[index + 1, 1].Text = EstimateUtil.ConvertToEnglish(data.Values[index] * 1000000.0, Tuple.Create<Units, Units, Units>(Units.Squaremeter, Units.Hectare, Units.None), ReportBase.EnglishUnits).ToString("N2");
      }
      ReportUtil.FormatRenderTable(renderTable);
    }

    public override void SetAlignment(C1PrintDocument C1doc) => C1doc.Style.FlowAlignChildren = FlowAlignEnum.Center;
  }
}
