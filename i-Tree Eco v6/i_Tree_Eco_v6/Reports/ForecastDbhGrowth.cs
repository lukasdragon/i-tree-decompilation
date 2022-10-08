// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastDbhGrowth
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.v6;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class ForecastDbhGrowth : ForecastSimpleReport
  {
    public ForecastDbhGrowth()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleAverageDBHGrowth;
      this.dataValueTableFormatString = "N2";
      this.ForecastedYear_DataValue_FromCohortResults(CohortResultDataType.DBHGrowth);
    }

    public override void InitDocument(C1PrintDocument C1doc)
    {
      base.InitDocument(C1doc);
      this.dataValueHeading = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AverageDBHGrowth, ReportBase.CmUnits());
      this.SetConvertRatio(Conversions.InToCm);
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1.Win.C1Chart.C1Chart simpleChart = this.CreateSimpleChart();
      simpleChart.ChartArea.AxisY.Min = 0.0;
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(simpleChart, g, C1doc, 1.0, 0.79);
      C1doc.Body.Children.Add(chartRenderObject);
      C1doc.Body.Children.Add((RenderObject) this.CreateSimpleTable());
    }
  }
}
