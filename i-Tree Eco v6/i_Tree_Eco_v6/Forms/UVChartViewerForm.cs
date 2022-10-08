// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.UVChartViewerForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.Win.C1Chart;
using i_Tree_Eco_v6.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Forms
{
  public class UVChartViewerForm : SimpleChartViewerForm
  {
    public int GetMax(DataTable dt, string columnName)
    {
      double num = double.MinValue;
      foreach (DataRow row in (InternalDataCollectionBase) this._c.dt.Rows)
      {
        double val2 = row.Field<double>(columnName);
        num = Math.Max(num, val2);
      }
      return (int) Math.Ceiling(num);
    }

    public override void ModifyChartData(DataTable copyDt)
    {
      DataRow row1 = copyDt.NewRow();
      row1[0] = (object) ((DateTime) this._c.dt.Rows[0][0]).AddSeconds(-1.0);
      row1[1] = (object) 0;
      copyDt.Rows.InsertAt(row1, 0);
      DataRow row2 = copyDt.NewRow();
      row2[0] = (object) ((DateTime) this._c.dt.Rows[this._c.dt.Rows.Count - 1][0]).AddSeconds(1.0);
      row2[1] = (object) 0;
      copyDt.Rows.Add(row2);
    }

    public override void AddColoring(ChartDataSeriesCollection dsc0, ChartDataSeries cds)
    {
      this.chart.Legend.Text = Strings.UVLegendTitle;
      this.chart.Legend.Visible = true;
      AlarmZonesCollection alarmZones = this.chart.ChartArea.PlotArea.AlarmZones;
      List<SimpleChartViewerForm.UVIndexSettings> uvIndexSettingsList = this.SetColors();
      Dictionary<AlarmZone, Tuple<ChartDataSeries, SimpleChartViewerForm.UVIndexSettings>> source = new Dictionary<AlarmZone, Tuple<ChartDataSeries, SimpleChartViewerForm.UVIndexSettings>>();
      foreach (SimpleChartViewerForm.UVIndexSettings uvIndexSettings in uvIndexSettingsList)
        source.Add(alarmZones.AddNewZone(), new Tuple<ChartDataSeries, SimpleChartViewerForm.UVIndexSettings>(dsc0.AddNewSeries(), uvIndexSettings));
      int max = this.GetMax(this._c.dt, this._c.dt.Columns[1].ColumnName);
      foreach (KeyValuePair<AlarmZone, Tuple<ChartDataSeries, SimpleChartViewerForm.UVIndexSettings>> keyValuePair in source)
      {
        keyValuePair.Key.Visible = true;
        keyValuePair.Key.Name = keyValuePair.Value.Item2.Name;
        keyValuePair.Key.BackColor = keyValuePair.Value.Item2.BackColor;
        keyValuePair.Key.ForeColor = Color.Transparent;
        keyValuePair.Key.UpperExtent = source.Last<KeyValuePair<AlarmZone, Tuple<ChartDataSeries, SimpleChartViewerForm.UVIndexSettings>>>().Equals((object) keyValuePair) ? (double) max : (double) keyValuePair.Value.Item2.UpperExtent;
        keyValuePair.Key.LowerExtent = (double) keyValuePair.Value.Item2.LowerExtent;
        keyValuePair.Key.Shape = AlarmZoneShapeEnum.Polygon;
        keyValuePair.Key.PolygonData.ChartGroup = this.chart.ChartGroups[0];
        keyValuePair.Key.PolygonData.PolygonSource = PolygonSourceEnum.DataSeries;
        keyValuePair.Key.PolygonData.SeriesIndex = dsc0.IndexOf(cds);
        keyValuePair.Value.Item1.SymbolStyle.Shape = SymbolShapeEnum.None;
        keyValuePair.Value.Item1.LineStyle.Color = keyValuePair.Value.Item2.BackColor;
        keyValuePair.Value.Item1.LineStyle.Thickness = 16;
        if (max <= keyValuePair.Value.Item2.LowerExtent)
          keyValuePair.Value.Item1.SeriesData.LegendEntry = false;
        keyValuePair.Value.Item1.Label = keyValuePair.Value.Item2.Name;
      }
    }

    public List<SimpleChartViewerForm.UVIndexSettings> SetColors() => new List<SimpleChartViewerForm.UVIndexSettings>()
    {
      new SimpleChartViewerForm.UVIndexSettings()
      {
        Name = string.Format("0-3 {0}", (object) Strings.RatingLow),
        BackColor = Color.Green,
        LowerExtent = 0,
        UpperExtent = 3
      },
      new SimpleChartViewerForm.UVIndexSettings()
      {
        Name = string.Format("3-6 {0}", (object) Strings.RatingModerate),
        BackColor = Color.Yellow,
        LowerExtent = 3,
        UpperExtent = 6
      },
      new SimpleChartViewerForm.UVIndexSettings()
      {
        Name = string.Format("6-8 {0}", (object) Strings.RatingHigh),
        BackColor = Color.Orange,
        LowerExtent = 6,
        UpperExtent = 8
      },
      new SimpleChartViewerForm.UVIndexSettings()
      {
        Name = string.Format("8-11 {0}", (object) Strings.RatingVeryHigh),
        BackColor = Color.Red,
        LowerExtent = 8,
        UpperExtent = 11
      },
      new SimpleChartViewerForm.UVIndexSettings()
      {
        Name = string.Format("11+ {0}", (object) Strings.RatingExtreme),
        BackColor = Color.BlueViolet,
        LowerExtent = 11,
        UpperExtent = 20
      }
    };

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (UVChartViewerForm));
      this.chart.BeginInit();
      this.SuspendLayout();
      this.chart.PropBag = componentResourceManager.GetString("chart.PropBag");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.ClientSize = new Size(686, 453);
      this.Name = nameof (UVChartViewerForm);
      this.chart.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
