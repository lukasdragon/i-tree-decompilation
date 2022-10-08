// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.LeafAreaByStrata
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
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class LeafAreaByStrata : LeafAreaByStrataBase
  {
    public LeafAreaByStrata()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleLeafAreaByStratum;
      this._estUnits = this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)];
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      SortedList<int, double> data = (SortedList<int, double>) this.GetData();
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      c1Chart.Style.Font = new Font("Calibri", 14f);
      c1Chart.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      c1Chart.BackColor = Color.White;
      c1Chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, ReportBase.HaUnits());
      c1Chart.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      ReportUtil.SetChartOptions(c1Chart, true);
      ChartDataSeries chartDataSeries1 = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries1.LineStyle.Color = ReportUtil.GetColor(1);
      for (int index = 0; index < data.Count; ++index)
      {
        string text = this.curYear.RecordStrata || data.Count != 1 ? this.estUtil.ClassValues[Classifiers.Strata][(short) data.Keys[index]].Item1 : i_Tree_Eco_v6.Resources.Strings.StudyArea;
        if (data.Keys.Count > 1 && text != i_Tree_Eco_v6.Resources.Strings.StudyArea || data.Count == 1)
        {
          chartDataSeries1.X.Add((object) index);
          chartDataSeries1.Y.Add((object) (data.Values[index] * (ReportBase.EnglishUnits ? 247.105 : 100.0)));
          c1Chart.ChartArea.AxisX.ValueLabels.Add((object) index, text);
        }
      }
      RenderObject chartRenderObject1 = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 1.0, 0.79);
      C1doc.Body.Children.Add(chartRenderObject1);
      short valueOrderFromName = this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area");
      double num1 = data.Sum<KeyValuePair<int, double>>((Func<KeyValuePair<int, double>, double>) (p => p.Value));
      if (data.Count > 1)
        num1 -= data[(int) valueOrderFromName];
      C1.Win.C1Chart.C1Chart ChartIn = new C1.Win.C1Chart.C1Chart();
      ChartIn.Style.BackColor = Color.White;
      ChartIn.Style.Font = new Font("Calibri", 14f);
      ChartIn.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      ChartIn.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Pie;
      ChartIn.Legend.Visible = true;
      for (int index = 0; index < data.Count; ++index)
      {
        if (data.Keys[index] != (int) valueOrderFromName)
        {
          ChartDataSeries chartDataSeries2 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
          chartDataSeries2.LineStyle.Color = ReportUtil.GetColor(index);
          chartDataSeries2.X.Add((object) 0);
          double num2 = data.Values[index] / num1;
          chartDataSeries2.Y.Add((object) num2);
          chartDataSeries2.Label = num2.ToString("P1");
          Label label = ChartIn.ChartLabels.LabelsCollection.AddNewLabel();
          label.AttachMethod = AttachMethodEnum.DataIndex;
          AttachMethodData attachMethodData = label.AttachMethodData;
          attachMethodData.GroupIndex = 0;
          attachMethodData.SeriesIndex = index;
          attachMethodData.PointIndex = 0;
          label.Text = this.curYear.RecordStrata || data.Count != 1 ? this.estUtil.ClassValues[Classifiers.Strata][(short) data.Keys[index]].Item1 : i_Tree_Eco_v6.Resources.Strings.StudyArea;
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
      renderTable.Width = (Unit) "80%";
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, ReportBase.HaUnits());
      renderTable.Cells[0, 2].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, "%");
      int row = count;
      for (int index = 0; index < data.Count; ++index)
      {
        renderTable.Cells[row, 0].Text = this.curYear.RecordStrata || data.Count != 1 ? this.estUtil.ClassValues[Classifiers.Strata][(short) data.Keys[index]].Item1 : i_Tree_Eco_v6.Resources.Strings.StudyArea;
        TableCell cell1 = renderTable.Cells[row, 1];
        double num3 = data.Values[index] * (ReportBase.EnglishUnits ? 247.105 : 100.0);
        string str1 = num3.ToString("N2");
        cell1.Text = str1;
        TableCell cell2 = renderTable.Cells[row, 2];
        num3 = data.Values[index] / num1;
        string str2 = num3.ToString("P1");
        cell2.Text = str2;
        ++row;
      }
      int index1 = row - 1;
      renderTable.Rows[index1].Style.Borders.Top = LineDef.Default;
      renderTable.Rows[index1].Style.FontBold = true;
      ReportUtil.FormatRenderTable(renderTable);
    }
  }
}
