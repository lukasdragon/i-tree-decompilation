// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.OxygenProductionOfTreesBase
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
  internal class OxygenProductionOfTreesBase : DatabaseReport
  {
    protected Tuple<Units, Units, Units> units;
    protected string header = string.Empty;
    protected string headerUnits = string.Empty;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable data = (DataTable) this.GetData();
      int count1 = data.Rows.Count;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      int count2 = 2;
      renderTable.RowGroups[0, count2].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count2].Style.FontSize = 14f;
      renderTable.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = this.header;
      renderTable.Cells[1, 1].Text = ReportUtil.FormatHeaderUnitsStr(this.headerUnits);
      int num = count2;
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        string str = this.getStratumName(row);
        if (str == "Study Area")
        {
          str = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          renderTable.Rows[num].Style.Borders.Top = LineDef.Default;
          renderTable.Rows[num].Style.FontBold = true;
        }
        if (count1 == 1 && !this.curYear.RecordStrata)
          str = i_Tree_Eco_v6.Resources.Strings.StudyArea;
        renderTable.Cells[num, 0].Text = str;
        renderTable.Cells[num, 1].Text = EstimateUtil.ConvertToEnglish(Convert.ToDouble(row["carbonStorage"]), this.units, ReportBase.EnglishUnits).ToString("N1");
        ++num;
      }
      ReportUtil.FormatRenderTable(renderTable);
      C1.Win.C1Chart.C1Chart c1Chart = new C1.Win.C1Chart.C1Chart();
      c1Chart.Style.Font = new Font("Calibri", 14f);
      c1Chart.ChartLabels.DefaultLabelStyle.Font = new Font("Calibri", 12f);
      c1Chart.ChartGroups.Group0.ChartType = Chart2DTypeEnum.Bar;
      c1Chart.BackColor = Color.White;
      c1Chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(this.header, this.headerUnits);
      c1Chart.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      ReportUtil.SetChartOptions(c1Chart, true);
      ChartDataSeries chartDataSeries = c1Chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(1);
      int val = 0;
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        string text = this.getStratumName(row);
        if (count1 > 1 && text != "Study Area" || count1 == 1)
        {
          if (count1 == 1 && !this.curYear.RecordStrata)
            text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          chartDataSeries.X.Add((object) val);
          chartDataSeries.Y.Add((object) EstimateUtil.ConvertToEnglish(Convert.ToDouble(row["carbonStorage"]), this.units, ReportBase.EnglishUnits));
          c1Chart.ChartArea.AxisX.ValueLabels.Add((object) val, text);
          ++val;
        }
      }
      RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart, g, C1doc, 1.0, 0.8);
      C1doc.Body.Children.Add(chartRenderObject);
    }

    public override object GetData()
    {
      EstimateTypeEnum key = ReportBase.plotInventory ? EstimateTypeEnum.NetCarbonSequestration : EstimateTypeEnum.GrossCarbonSequestration;
      DataTable data = this.GetOxygenProductionOfTrees().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[key]).SetParameter<int>("estUnits", this.estUtil.EstUnits[this.units]).SetParameter<short>("speciesTotal", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      if (data.Rows.Count == 2)
        data.Rows.RemoveAt(1);
      return (object) data;
    }

    private IQuery GetOxygenProductionOfTrees() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetOxygenProductionOfTrees(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.Species]);

    private string getStratumName(DataRow row) => this.estUtil.ClassValues[Classifiers.Strata][Convert.ToInt16(row[this.estUtil.ClassifierNames[Classifiers.Strata]])].Item1;
  }
}
