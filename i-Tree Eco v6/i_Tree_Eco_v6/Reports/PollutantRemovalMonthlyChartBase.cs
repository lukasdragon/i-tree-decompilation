// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.PollutantRemovalMonthlyChartBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  internal class PollutantRemovalMonthlyChartBase : PollutantRemovalMonthlyBase
  {
    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      if (this.isGrassReport && !this.DataExists())
      {
        DatabaseReport.NewReportMessage(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgGrassReport);
      }
      else
      {
        SortedList<int, PollutantRemovalMonthlyChartBase.PollRemoval> sortedList = new SortedList<int, PollutantRemovalMonthlyChartBase.PollRemoval>();
        foreach (DataRow row in (InternalDataCollectionBase) this.GetData().Rows)
        {
          int key = ReportUtil.ConvertFromDBVal<int>(row["Month"]);
          double num = ReportUtil.ConvertFromDBVal<double>(row["myAmount"]);
          string str = ReportUtil.ConvertFromDBVal<string>(row["Pollutant"]);
          if (!sortedList.ContainsKey(key))
            sortedList.Add(key, new PollutantRemovalMonthlyChartBase.PollRemoval()
            {
              CORemoval = 0.0,
              NO2Removal = 0.0,
              O3Removal = 0.0,
              PM10Removal = 0.0,
              PM25Removal = 0.0,
              SO2Removal = 0.0
            });
          switch (str)
          {
            case "CO":
              sortedList[key].CORemoval = num;
              continue;
            case "NO2":
              sortedList[key].NO2Removal = num;
              continue;
            case "O3":
              sortedList[key].O3Removal = num;
              continue;
            case "PM10*":
              sortedList[key].PM10Removal = num;
              continue;
            case "PM2.5":
            case "PM25":
              sortedList[key].PM25Removal = num;
              continue;
            case "SO2":
              sortedList[key].SO2Removal = num;
              continue;
            default:
              continue;
          }
        }
        C1.Win.C1Chart.C1Chart c1Chart1 = new C1.Win.C1Chart.C1Chart();
        c1Chart1.BackColor = Color.White;
        c1Chart1.Font = new Font("Calibri", 12f);
        c1Chart1.Legend.Visible = true;
        c1Chart1.ChartGroups.Group0.ChartType = Chart2DTypeEnum.XYPlot;
        c1Chart1.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Removal, ReportBase.KilogramsUnits());
        c1Chart1.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Month;
        c1Chart1.Footer.Visible = true;
        c1Chart1.Footer.Style.Font = new Font("Calibri", 10f);
        c1Chart1.Footer.Text = i_Tree_Eco_v6.Resources.Strings.WR_PM25Pm10;
        ReportUtil.SetChartOptions(c1Chart1, false);
        ChartDataSeries chartDataSeries1 = c1Chart1.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries1.Label = i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideFormula;
        chartDataSeries1.SymbolStyle.Shape = SymbolShapeEnum.Tri;
        chartDataSeries1.LineStyle.Color = ReportUtil.GetColor(0);
        ChartDataSeries chartDataSeries2 = c1Chart1.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries2.Label = i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula;
        chartDataSeries2.SymbolStyle.Shape = SymbolShapeEnum.Square;
        chartDataSeries2.LineStyle.Color = ReportUtil.GetColor(1);
        ChartDataSeries chartDataSeries3 = c1Chart1.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries3.Label = i_Tree_Eco_v6.Resources.Strings.OzoneFurmula;
        chartDataSeries3.SymbolStyle.Shape = SymbolShapeEnum.Circle;
        chartDataSeries3.LineStyle.Color = ReportUtil.GetColor(2);
        ChartDataSeries chartDataSeries4 = c1Chart1.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries4.Label = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter10Furmula;
        chartDataSeries4.SymbolStyle.Shape = SymbolShapeEnum.Dot;
        chartDataSeries4.LineStyle.Color = ReportUtil.GetColor(3);
        ChartDataSeries chartDataSeries5 = c1Chart1.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries5.Label = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula;
        chartDataSeries5.SymbolStyle.Shape = SymbolShapeEnum.Diamond;
        chartDataSeries5.LineStyle.Color = ReportUtil.GetColor(4);
        ChartDataSeries chartDataSeries6 = c1Chart1.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries6.Label = i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula;
        chartDataSeries6.SymbolStyle.Shape = SymbolShapeEnum.Star;
        chartDataSeries6.LineStyle.Color = ReportUtil.GetColor(5);
        for (int index = 0; index < sortedList.Count; ++index)
        {
          chartDataSeries1.X.Add((object) sortedList.Keys[index]);
          chartDataSeries1.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].CORemoval, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries2.X.Add((object) sortedList.Keys[index]);
          chartDataSeries2.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].NO2Removal, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries3.X.Add((object) sortedList.Keys[index]);
          chartDataSeries3.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].O3Removal, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries4.X.Add((object) sortedList.Keys[index]);
          chartDataSeries4.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].PM10Removal, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries5.X.Add((object) sortedList.Keys[index]);
          chartDataSeries5.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].PM25Removal, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries6.X.Add((object) sortedList.Keys[index]);
          chartDataSeries6.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].SO2Removal, Units.Kilograms, ReportBase.EnglishUnits));
        }
        RenderObject chartRenderObject1 = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart1, g, C1doc, 1.0, 0.79);
        chartRenderObject1.BreakAfter = BreakEnum.Page;
        C1doc.Body.Children.Add(chartRenderObject1);
        RenderTable renderTable1 = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable1);
        renderTable1.Width = (Unit) "60%";
        renderTable1.RowGroups[0, 2].Header = TableHeaderEnum.Page;
        renderTable1.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        renderTable1.Cols[1].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        renderTable1.Cols[2].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
        renderTable1.Rows[0].Style.FontSize = 14f;
        renderTable1.Rows[1].Style.FontSize = 12f;
        renderTable1.Rows[0].Style.FontBold = true;
        renderTable1.Rows[1].Style.FontBold = true;
        renderTable1.Cells[0, 0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
        renderTable1.Cells[0, 0].SpanCols = 3;
        renderTable1.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.PollutantRemoval;
        renderTable1.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.Month;
        renderTable1.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Pollutant;
        renderTable1.Cells[1, 2].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Amount, ReportBase.KilogramsUnits());
        double num1;
        for (int index = 0; index < sortedList.Count; ++index)
        {
          renderTable1.Cells[index * 6 + 2, 0].Text = sortedList.Keys[index].ToString();
          renderTable1.Cells[index * 6 + 2, 1].Text = i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideFormula;
          TableCell cell1 = renderTable1.Cells[index * 6 + 2, 2];
          num1 = EstimateUtil.ConvertToEnglish(sortedList.Values[index].CORemoval, Units.Kilograms, ReportBase.EnglishUnits);
          string str1 = num1.ToString("N3");
          cell1.Text = str1;
          renderTable1.Cells[index * 6 + 3, 1].Text = i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula;
          TableCell cell2 = renderTable1.Cells[index * 6 + 3, 2];
          num1 = EstimateUtil.ConvertToEnglish(sortedList.Values[index].NO2Removal, Units.Kilograms, ReportBase.EnglishUnits);
          string str2 = num1.ToString("N3");
          cell2.Text = str2;
          renderTable1.Cells[index * 6 + 4, 1].Text = i_Tree_Eco_v6.Resources.Strings.OzoneFurmula;
          TableCell cell3 = renderTable1.Cells[index * 6 + 4, 2];
          num1 = EstimateUtil.ConvertToEnglish(sortedList.Values[index].O3Removal, Units.Kilograms, ReportBase.EnglishUnits);
          string str3 = num1.ToString("N3");
          cell3.Text = str3;
          renderTable1.Cells[index * 6 + 5, 1].Text = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter10Furmula;
          TableCell cell4 = renderTable1.Cells[index * 6 + 5, 2];
          num1 = EstimateUtil.ConvertToEnglish(sortedList.Values[index].PM10Removal, Units.Kilograms, ReportBase.EnglishUnits);
          string str4 = num1.ToString("N3");
          cell4.Text = str4;
          renderTable1.Cells[index * 6 + 6, 1].Text = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula;
          TableCell cell5 = renderTable1.Cells[index * 6 + 6, 2];
          num1 = EstimateUtil.ConvertToEnglish(sortedList.Values[index].PM25Removal, Units.Kilograms, ReportBase.EnglishUnits);
          string str5 = num1.ToString("N3");
          cell5.Text = str5;
          renderTable1.Cells[index * 6 + 7, 1].Text = i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula;
          TableCell cell6 = renderTable1.Cells[index * 6 + 7, 2];
          num1 = EstimateUtil.ConvertToEnglish(sortedList.Values[index].SO2Removal, Units.Kilograms, ReportBase.EnglishUnits);
          string str6 = num1.ToString("N3");
          cell6.Text = str6;
          renderTable1.Rows[index * 6 + 7].Style.Borders.Bottom = new LineDef((Unit) "1pt", Color.Black);
        }
        ReportUtil.FormatRenderTable(renderTable1);
        RenderText ro1 = new RenderText();
        ro1.Text = i_Tree_Eco_v6.Resources.Strings.WR_PM25Pm10;
        ro1.BreakAfter = BreakEnum.Page;
        C1doc.Body.Children.Add((RenderObject) ro1);
        C1.Win.C1Chart.C1Chart ChartIn = new C1.Win.C1Chart.C1Chart();
        ChartIn.BackColor = Color.White;
        ChartIn.Font = new Font("Calibri", 12f);
        ChartIn.Legend.Visible = true;
        ChartIn.ChartGroups.Group0.ChartType = Chart2DTypeEnum.XYPlot;
        ChartIn.Header.Visible = true;
        ChartIn.Header.Style.Font = ChartIn.Font = new Font("Calibri", 14f);
        ChartIn.Header.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.PollutantRemovalValueInLocation, (object) this.locationName);
        ChartIn.ChartArea.AxisY.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.RemovalValueMonetary, (object) this.CurrencySymbol);
        ChartIn.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Month;
        ChartIn.Footer.Visible = true;
        ChartIn.Footer.Style.Font = new Font("Calibri", 10f);
        ChartIn.Footer.Text = this.PollutionRemoval_kg_lb_Footer() + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteZeroValuesMeaning + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.WR_PM25Pm10;
        ChartDataSeries chartDataSeries7 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries7.Label = i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideFormula;
        chartDataSeries7.SymbolStyle.Shape = SymbolShapeEnum.Tri;
        chartDataSeries7.LineStyle.Color = ReportUtil.GetColor(0);
        ChartDataSeries chartDataSeries8 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries8.Label = i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula;
        chartDataSeries8.SymbolStyle.Shape = SymbolShapeEnum.Square;
        chartDataSeries8.LineStyle.Color = ReportUtil.GetColor(1);
        ChartDataSeries chartDataSeries9 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries9.Label = i_Tree_Eco_v6.Resources.Strings.OzoneFurmula;
        chartDataSeries9.SymbolStyle.Shape = SymbolShapeEnum.Circle;
        chartDataSeries9.LineStyle.Color = ReportUtil.GetColor(2);
        ChartDataSeries chartDataSeries10 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries10.Label = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter10Furmula;
        chartDataSeries10.SymbolStyle.Shape = SymbolShapeEnum.Dot;
        chartDataSeries10.LineStyle.Color = ReportUtil.GetColor(3);
        ChartDataSeries chartDataSeries11 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries11.Label = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula;
        chartDataSeries11.SymbolStyle.Shape = SymbolShapeEnum.Diamond;
        chartDataSeries11.LineStyle.Color = ReportUtil.GetColor(4);
        ChartDataSeries chartDataSeries12 = ChartIn.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        chartDataSeries12.Label = i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula;
        chartDataSeries12.SymbolStyle.Shape = SymbolShapeEnum.Star;
        chartDataSeries12.LineStyle.Color = ReportUtil.GetColor(5);
        for (int index = 0; index < sortedList.Count; ++index)
        {
          chartDataSeries7.X.Add((object) sortedList.Keys[index]);
          chartDataSeries7.Y.Add((object) (sortedList.Values[index].CORemoval * (this.customizedCoDollarsPerTon / 1000.0)));
          chartDataSeries8.X.Add((object) sortedList.Keys[index]);
          chartDataSeries8.Y.Add((object) (sortedList.Values[index].NO2Removal * (this.customizedNO2DollarsPerTon / 1000.0)));
          chartDataSeries9.X.Add((object) sortedList.Keys[index]);
          chartDataSeries9.Y.Add((object) (sortedList.Values[index].O3Removal * (this.customizedO3DollarsPerTon / 1000.0)));
          chartDataSeries10.X.Add((object) sortedList.Keys[index]);
          chartDataSeries10.Y.Add((object) (sortedList.Values[index].PM10Removal * (this.customizedPM10DollarsPerTon / 1000.0)));
          chartDataSeries11.X.Add((object) sortedList.Keys[index]);
          chartDataSeries11.Y.Add((object) (sortedList.Values[index].PM25Removal * (this.customizedPM25DollarsPerTon / 1000.0)));
          chartDataSeries12.X.Add((object) sortedList.Keys[index]);
          chartDataSeries12.Y.Add((object) (sortedList.Values[index].SO2Removal * (this.customizedSO2DollarsPerTon / 1000.0)));
        }
        RenderObject chartRenderObject2 = (RenderObject) ReportUtil.CreateChartRenderObject(ChartIn, g, C1doc, 1.0, 0.79);
        chartRenderObject2.BreakAfter = BreakEnum.Page;
        C1doc.Body.Children.Add(chartRenderObject2);
        RenderTable renderTable2 = new RenderTable();
        C1doc.Body.Children.Add((RenderObject) renderTable2);
        renderTable2.Width = (Unit) "60%";
        renderTable2.RowGroups[0, 2].Header = TableHeaderEnum.Page;
        renderTable2.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        renderTable2.Cols[1].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        renderTable2.Cols[2].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
        renderTable2.Rows[0].Style.FontSize = 14f;
        renderTable2.Rows[1].Style.FontSize = 12f;
        renderTable2.Rows[0].Style.FontBold = true;
        renderTable2.Rows[1].Style.FontBold = true;
        renderTable2.Cells[0, 0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
        renderTable2.Cells[0, 0].SpanCols = 3;
        renderTable2.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.PollutantRemovalValue;
        renderTable2.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.Month;
        renderTable2.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Pollutant;
        renderTable2.Cells[1, 2].Text = string.Format(ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Value, this.CurrencySymbol));
        for (int index = 0; index < sortedList.Count; ++index)
        {
          renderTable2.Cells[index * 6 + 2, 0].Text = sortedList.Keys[index].ToString();
          renderTable2.Cells[index * 6 + 2, 1].Text = i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideFormula;
          TableCell cell7 = renderTable2.Cells[index * 6 + 2, 2];
          num1 = sortedList.Values[index].CORemoval * (this.customizedCoDollarsPerTon / 1000.0);
          string str7 = num1.ToString("N3");
          cell7.Text = str7;
          renderTable2.Cells[index * 6 + 3, 1].Text = i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula;
          TableCell cell8 = renderTable2.Cells[index * 6 + 3, 2];
          num1 = sortedList.Values[index].NO2Removal * (this.customizedNO2DollarsPerTon / 1000.0);
          string str8 = num1.ToString("N3");
          cell8.Text = str8;
          renderTable2.Cells[index * 6 + 4, 1].Text = i_Tree_Eco_v6.Resources.Strings.OzoneFurmula;
          TableCell cell9 = renderTable2.Cells[index * 6 + 4, 2];
          num1 = sortedList.Values[index].O3Removal * (this.customizedO3DollarsPerTon / 1000.0);
          string str9 = num1.ToString("N3");
          cell9.Text = str9;
          renderTable2.Cells[index * 6 + 5, 1].Text = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter10Furmula;
          TableCell cell10 = renderTable2.Cells[index * 6 + 5, 2];
          num1 = sortedList.Values[index].PM10Removal * (this.customizedPM10DollarsPerTon / 1000.0);
          string str10 = num1.ToString("N3");
          cell10.Text = str10;
          renderTable2.Cells[index * 6 + 6, 1].Text = i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula;
          TableCell cell11 = renderTable2.Cells[index * 6 + 6, 2];
          num1 = sortedList.Values[index].PM25Removal * (this.customizedPM25DollarsPerTon / 1000.0);
          string str11 = num1.ToString("N3");
          cell11.Text = str11;
          renderTable2.Cells[index * 6 + 7, 1].Text = i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula;
          TableCell cell12 = renderTable2.Cells[index * 6 + 7, 2];
          num1 = sortedList.Values[index].SO2Removal * (this.customizedSO2DollarsPerTon / 1000.0);
          string str12 = num1.ToString("N3");
          cell12.Text = str12;
          renderTable2.Rows[index * 6 + 7].Style.Borders.Bottom = new LineDef((Unit) "1pt", Color.Black);
        }
        ReportUtil.FormatRenderTable(renderTable2);
        RenderText ro2 = new RenderText();
        ro2.Text = this.PollutionRemoval_kg_lb_Footer() + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.NoteZeroValuesMeaning + Environment.NewLine + i_Tree_Eco_v6.Resources.Strings.WR_PM25Pm10;
        ro2.BreakAfter = BreakEnum.Page;
        C1doc.Body.Children.Add((RenderObject) ro2);
        C1.Win.C1Chart.C1Chart c1Chart2 = new C1.Win.C1Chart.C1Chart();
        ReportUtil.SetChartOptions(c1Chart2, false);
        c1Chart2.Legend.Visible = true;
        c1Chart2.ChartGroups.Group0.ChartType = Chart2DTypeEnum.XYPlot;
        c1Chart2.ChartGroups.Group1.ChartType = Chart2DTypeEnum.Bar;
        ChartGroup group0_1 = c1Chart2.ChartGroups.Group0;
        ChartGroup group1_1 = c1Chart2.ChartGroups.Group1;
        c1Chart2.Header.Visible = true;
        c1Chart2.Header.Style.Font = c1Chart2.Font = new Font("Calibri", 14f);
        c1Chart2.Header.Text = i_Tree_Eco_v6.Resources.Strings.CORemovalByMonth;
        c1Chart2.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Removal, ReportBase.KilogramsUnits());
        c1Chart2.ChartArea.AxisY.Visible = true;
        c1Chart2.ChartArea.AxisY2.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.RemovalValueMonetary, (object) this.CurrencySymbol);
        c1Chart2.ChartArea.AxisY2.Visible = true;
        c1Chart2.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Month;
        c1Chart2.ChartArea.AxisY.Rotation = RotationEnum.Rotate270;
        c1Chart2.ChartArea.AxisY2.Rotation = RotationEnum.Rotate270;
        c1Chart2.Footer.Visible = true;
        c1Chart2.Footer.Style.Font = new Font("Calibri", 10f);
        c1Chart2.Footer.Text = this.PollutionRemoval_kg_lb_Footer("CO");
        ChartDataSeries chartDataSeries13 = group0_1.ChartData.SeriesList.AddNewSeries();
        chartDataSeries13.Label = i_Tree_Eco_v6.Resources.Strings.Removal;
        chartDataSeries13.LineStyle.Pattern = LinePatternEnum.None;
        chartDataSeries13.SymbolStyle.Shape = SymbolShapeEnum.Tri;
        chartDataSeries13.SymbolStyle.Size = 10;
        chartDataSeries13.SymbolStyle.Color = Color.Black;
        ChartDataSeries chartDataSeries14 = group1_1.ChartData.SeriesList.AddNewSeries();
        chartDataSeries14.Label = i_Tree_Eco_v6.Resources.Strings.Value;
        chartDataSeries14.LineStyle.Color = ReportUtil.GetColor(0);
        for (int index = 0; index < sortedList.Count; ++index)
        {
          chartDataSeries13.X.Add((object) sortedList.Keys[index]);
          chartDataSeries13.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].CORemoval, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries14.X.Add((object) sortedList.Keys[index]);
          chartDataSeries14.Y.Add((object) (sortedList.Values[index].CORemoval * (this.customizedCoDollarsPerTon / 1000.0)));
        }
        c1Chart2.GetImage();
        c1Chart2.Legend.Compass = CompassEnum.East;
        c1Chart2.Legend.Location = new Point(-1, 0);
        RenderObject chartRenderObject3 = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart2, g, C1doc, 1.0, 0.79);
        chartRenderObject3.BreakAfter = BreakEnum.Page;
        C1doc.Body.Children.Add(chartRenderObject3);
        C1.Win.C1Chart.C1Chart c1Chart3 = new C1.Win.C1Chart.C1Chart();
        ReportUtil.SetChartOptions(c1Chart3, false);
        c1Chart3.Legend.Visible = true;
        c1Chart3.Legend.Compass = CompassEnum.East;
        ChartGroup group0_2 = c1Chart3.ChartGroups.Group0;
        ChartGroup group1_2 = c1Chart3.ChartGroups.Group1;
        group1_2.ChartType = Chart2DTypeEnum.Bar;
        group0_2.ChartType = Chart2DTypeEnum.XYPlot;
        c1Chart3.Header.Visible = true;
        c1Chart3.Header.Style.Font = c1Chart3.Font = new Font("Calibri", 14f);
        c1Chart3.Header.Text = i_Tree_Eco_v6.Resources.Strings.NO2RemovalByMonth;
        c1Chart3.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Removal, ReportBase.KilogramsUnits());
        c1Chart3.ChartArea.AxisY2.Visible = true;
        c1Chart3.ChartArea.AxisY2.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.RemovalValueMonetary, (object) this.CurrencySymbol);
        c1Chart3.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Month;
        c1Chart3.ChartArea.AxisY.Rotation = RotationEnum.Rotate270;
        c1Chart3.ChartArea.AxisY2.Rotation = RotationEnum.Rotate270;
        c1Chart3.Footer.Visible = true;
        c1Chart3.Footer.Style.Font = new Font("Calibri", 10f);
        c1Chart3.Footer.Text = this.PollutionRemoval_kg_lb_Footer("NO2");
        ChartDataSeries chartDataSeries15 = group0_2.ChartData.SeriesList.AddNewSeries();
        chartDataSeries15.Label = i_Tree_Eco_v6.Resources.Strings.Removal;
        chartDataSeries15.LineStyle.Pattern = LinePatternEnum.None;
        chartDataSeries15.SymbolStyle.Shape = SymbolShapeEnum.Tri;
        chartDataSeries15.SymbolStyle.Size = 10;
        chartDataSeries15.SymbolStyle.Color = Color.Black;
        ChartDataSeries chartDataSeries16 = group1_2.ChartData.SeriesList.AddNewSeries();
        chartDataSeries16.Label = i_Tree_Eco_v6.Resources.Strings.Value;
        chartDataSeries16.LineStyle.Color = ReportUtil.GetColor(0);
        for (int index = 0; index < sortedList.Count; ++index)
        {
          chartDataSeries15.X.Add((object) sortedList.Keys[index]);
          chartDataSeries15.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].NO2Removal, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries16.X.Add((object) sortedList.Keys[index]);
          chartDataSeries16.Y.Add((object) (sortedList.Values[index].NO2Removal * (this.customizedNO2DollarsPerTon / 1000.0)));
        }
        c1Chart3.GetImage();
        c1Chart3.Legend.Compass = CompassEnum.East;
        c1Chart3.Legend.Location = new Point(-1, 0);
        RenderObject chartRenderObject4 = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart3, g, C1doc, 1.0, 0.79);
        chartRenderObject4.BreakAfter = BreakEnum.Page;
        C1doc.Body.Children.Add(chartRenderObject4);
        C1.Win.C1Chart.C1Chart c1Chart4 = new C1.Win.C1Chart.C1Chart();
        ReportUtil.SetChartOptions(c1Chart4, false);
        c1Chart4.Legend.Visible = true;
        c1Chart4.Legend.Compass = CompassEnum.East;
        ChartGroup group0_3 = c1Chart4.ChartGroups.Group0;
        ChartGroup group1_3 = c1Chart4.ChartGroups.Group1;
        group1_3.ChartType = Chart2DTypeEnum.Bar;
        group0_3.ChartType = Chart2DTypeEnum.XYPlot;
        c1Chart4.Header.Visible = true;
        c1Chart4.Header.Style.Font = c1Chart4.Font = new Font("Calibri", 14f);
        c1Chart4.Header.Text = i_Tree_Eco_v6.Resources.Strings.O3RemovalByMonth;
        c1Chart4.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Removal, ReportBase.KilogramsUnits());
        c1Chart4.ChartArea.AxisY2.Visible = true;
        c1Chart4.ChartArea.AxisY2.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.RemovalValueMonetary, (object) this.CurrencySymbol);
        c1Chart4.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Month;
        c1Chart4.ChartArea.AxisY.Rotation = RotationEnum.Rotate270;
        c1Chart4.ChartArea.AxisY2.Rotation = RotationEnum.Rotate270;
        c1Chart4.Footer.Visible = true;
        c1Chart4.Footer.Style.Font = new Font("Calibri", 10f);
        c1Chart4.Footer.Text = this.PollutionRemoval_kg_lb_Footer("O3");
        ChartDataSeries chartDataSeries17 = group0_3.ChartData.SeriesList.AddNewSeries();
        chartDataSeries17.Label = i_Tree_Eco_v6.Resources.Strings.Removal;
        chartDataSeries17.LineStyle.Pattern = LinePatternEnum.None;
        chartDataSeries17.SymbolStyle.Shape = SymbolShapeEnum.Tri;
        chartDataSeries17.SymbolStyle.Size = 10;
        chartDataSeries17.SymbolStyle.Color = Color.Black;
        ChartDataSeries chartDataSeries18 = group1_3.ChartData.SeriesList.AddNewSeries();
        chartDataSeries18.Label = i_Tree_Eco_v6.Resources.Strings.Value;
        chartDataSeries18.LineStyle.Color = ReportUtil.GetColor(0);
        for (int index = 0; index < sortedList.Count; ++index)
        {
          chartDataSeries17.X.Add((object) sortedList.Keys[index]);
          chartDataSeries17.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].O3Removal, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries18.X.Add((object) sortedList.Keys[index]);
          chartDataSeries18.Y.Add((object) (sortedList.Values[index].O3Removal * (this.customizedO3DollarsPerTon / 1000.0)));
        }
        c1Chart4.GetImage();
        c1Chart4.Legend.Compass = CompassEnum.East;
        c1Chart4.Legend.Location = new Point(-1, 0);
        RenderObject chartRenderObject5 = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart4, g, C1doc, 1.0, 0.79);
        chartRenderObject5.BreakAfter = BreakEnum.Page;
        C1doc.Body.Children.Add(chartRenderObject5);
        C1.Win.C1Chart.C1Chart c1Chart5 = new C1.Win.C1Chart.C1Chart();
        ReportUtil.SetChartOptions(c1Chart5, false);
        c1Chart5.Legend.Visible = true;
        c1Chart5.Legend.Compass = CompassEnum.East;
        ChartGroup group0_4 = c1Chart5.ChartGroups.Group0;
        ChartGroup group1_4 = c1Chart5.ChartGroups.Group1;
        group1_4.ChartType = Chart2DTypeEnum.Bar;
        group0_4.ChartType = Chart2DTypeEnum.XYPlot;
        c1Chart5.Header.Visible = true;
        c1Chart5.Header.Style.Font = c1Chart5.Font = new Font("Calibri", 14f);
        c1Chart5.Header.Text = i_Tree_Eco_v6.Resources.Strings.PM10RemovalByMonth;
        c1Chart5.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Removal, ReportBase.KilogramsUnits());
        c1Chart5.ChartArea.AxisY2.Visible = true;
        c1Chart5.ChartArea.AxisY2.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.RemovalValueMonetary, (object) this.CurrencySymbol);
        c1Chart5.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Month;
        c1Chart5.ChartArea.AxisY.Rotation = RotationEnum.Rotate270;
        c1Chart5.ChartArea.AxisY2.Rotation = RotationEnum.Rotate270;
        c1Chart5.Footer.Visible = true;
        c1Chart5.Footer.Style.Font = new Font("Calibri", 10f);
        c1Chart5.Footer.Text = this.PollutionRemoval_kg_lb_Footer("PM10*");
        ChartDataSeries chartDataSeries19 = group0_4.ChartData.SeriesList.AddNewSeries();
        chartDataSeries19.Label = i_Tree_Eco_v6.Resources.Strings.Removal;
        chartDataSeries19.LineStyle.Pattern = LinePatternEnum.None;
        chartDataSeries19.SymbolStyle.Shape = SymbolShapeEnum.Tri;
        chartDataSeries19.SymbolStyle.Size = 10;
        chartDataSeries19.SymbolStyle.Color = Color.Black;
        ChartDataSeries chartDataSeries20 = group1_4.ChartData.SeriesList.AddNewSeries();
        chartDataSeries20.Label = i_Tree_Eco_v6.Resources.Strings.Value;
        chartDataSeries20.LineStyle.Color = ReportUtil.GetColor(0);
        for (int index = 0; index < sortedList.Count; ++index)
        {
          chartDataSeries19.X.Add((object) sortedList.Keys[index]);
          chartDataSeries19.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].PM10Removal, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries20.X.Add((object) sortedList.Keys[index]);
          chartDataSeries20.Y.Add((object) (sortedList.Values[index].PM10Removal * (this.customizedPM10DollarsPerTon / 1000.0)));
        }
        c1Chart5.GetImage();
        c1Chart5.Legend.Compass = CompassEnum.East;
        c1Chart5.Legend.Location = new Point(-1, 0);
        RenderObject chartRenderObject6 = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart5, g, C1doc, 1.0, 0.79);
        chartRenderObject6.BreakAfter = BreakEnum.Page;
        C1doc.Body.Children.Add(chartRenderObject6);
        C1.Win.C1Chart.C1Chart c1Chart6 = new C1.Win.C1Chart.C1Chart();
        ReportUtil.SetChartOptions(c1Chart6, false);
        c1Chart6.Legend.Visible = true;
        c1Chart6.Legend.Compass = CompassEnum.East;
        ChartGroup group0_5 = c1Chart6.ChartGroups.Group0;
        ChartGroup group1_5 = c1Chart6.ChartGroups.Group1;
        group1_5.ChartType = Chart2DTypeEnum.Bar;
        group0_5.ChartType = Chart2DTypeEnum.XYPlot;
        c1Chart6.Header.Visible = true;
        c1Chart6.Header.Style.Font = c1Chart6.Font = new Font("Calibri", 14f);
        c1Chart6.Header.Text = i_Tree_Eco_v6.Resources.Strings.PM25RemovalByMonth;
        c1Chart6.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Removal, ReportBase.KilogramsUnits());
        c1Chart6.ChartArea.AxisY2.Visible = true;
        c1Chart6.ChartArea.AxisY2.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.RemovalValueMonetary, (object) this.CurrencySymbol);
        c1Chart6.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Month;
        c1Chart6.ChartArea.AxisY.Rotation = RotationEnum.Rotate270;
        c1Chart6.ChartArea.AxisY2.Rotation = RotationEnum.Rotate270;
        c1Chart6.Footer.Visible = true;
        c1Chart6.Footer.Style.Font = new Font("Calibri", 10f);
        c1Chart6.Footer.Text = this.PollutionRemoval_kg_lb_Footer("PM2.5");
        ChartDataSeries chartDataSeries21 = group0_5.ChartData.SeriesList.AddNewSeries();
        chartDataSeries21.Label = i_Tree_Eco_v6.Resources.Strings.Removal;
        chartDataSeries21.LineStyle.Pattern = LinePatternEnum.None;
        chartDataSeries21.SymbolStyle.Shape = SymbolShapeEnum.Tri;
        chartDataSeries21.SymbolStyle.Size = 10;
        chartDataSeries21.SymbolStyle.Color = Color.Black;
        ChartDataSeries chartDataSeries22 = group1_5.ChartData.SeriesList.AddNewSeries();
        chartDataSeries22.Label = i_Tree_Eco_v6.Resources.Strings.Value;
        chartDataSeries22.LineStyle.Color = ReportUtil.GetColor(0);
        for (int index = 0; index < sortedList.Count; ++index)
        {
          chartDataSeries21.X.Add((object) sortedList.Keys[index]);
          chartDataSeries21.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].PM25Removal, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries22.X.Add((object) sortedList.Keys[index]);
          chartDataSeries22.Y.Add((object) (sortedList.Values[index].PM25Removal * (this.customizedPM25DollarsPerTon / 1000.0)));
        }
        c1Chart6.GetImage();
        c1Chart6.Legend.Compass = CompassEnum.East;
        c1Chart6.Legend.Location = new Point(-1, 0);
        RenderObject chartRenderObject7 = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart6, g, C1doc, 1.0, 0.79);
        chartRenderObject7.BreakAfter = BreakEnum.Page;
        C1doc.Body.Children.Add(chartRenderObject7);
        C1.Win.C1Chart.C1Chart c1Chart7 = new C1.Win.C1Chart.C1Chart();
        ReportUtil.SetChartOptions(c1Chart7, false);
        c1Chart7.Legend.Visible = true;
        ChartGroup group0_6 = c1Chart7.ChartGroups.Group0;
        ChartGroup group1_6 = c1Chart7.ChartGroups.Group1;
        group1_6.ChartType = Chart2DTypeEnum.Bar;
        group0_6.ChartType = Chart2DTypeEnum.XYPlot;
        c1Chart7.Header.Visible = true;
        c1Chart7.Header.Style.Font = c1Chart7.Font = new Font("Calibri", 14f);
        c1Chart7.Header.Text = i_Tree_Eco_v6.Resources.Strings.SO2RemovalByMonth;
        c1Chart7.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Removal, ReportBase.KilogramsUnits());
        c1Chart7.ChartArea.AxisY2.Visible = true;
        c1Chart7.ChartArea.AxisY2.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.RemovalValueMonetary, (object) this.CurrencySymbol);
        c1Chart7.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Month;
        c1Chart7.ChartArea.AxisY.Rotation = RotationEnum.Rotate270;
        c1Chart7.ChartArea.AxisY2.Rotation = RotationEnum.Rotate270;
        c1Chart7.Footer.Visible = true;
        c1Chart7.Footer.Style.Font = new Font("Calibri", 10f);
        c1Chart7.Footer.Text = this.PollutionRemoval_kg_lb_Footer("SO2");
        ChartDataSeries chartDataSeries23 = group0_6.ChartData.SeriesList.AddNewSeries();
        chartDataSeries23.Label = i_Tree_Eco_v6.Resources.Strings.Removal;
        chartDataSeries23.LineStyle.Pattern = LinePatternEnum.None;
        chartDataSeries23.SymbolStyle.Shape = SymbolShapeEnum.Tri;
        chartDataSeries23.SymbolStyle.Size = 10;
        chartDataSeries23.SymbolStyle.Color = Color.Black;
        ChartDataSeries chartDataSeries24 = group1_6.ChartData.SeriesList.AddNewSeries();
        chartDataSeries24.Label = i_Tree_Eco_v6.Resources.Strings.Value;
        chartDataSeries24.LineStyle.Color = ReportUtil.GetColor(0);
        for (int index = 0; index < sortedList.Count; ++index)
        {
          chartDataSeries23.X.Add((object) sortedList.Keys[index]);
          chartDataSeries23.Y.Add((object) EstimateUtil.ConvertToEnglish(sortedList.Values[index].SO2Removal, Units.Kilograms, ReportBase.EnglishUnits));
          chartDataSeries24.X.Add((object) sortedList.Keys[index]);
          chartDataSeries24.Y.Add((object) (sortedList.Values[index].SO2Removal * (this.customizedSO2DollarsPerTon / 1000.0)));
        }
        c1Chart7.GetImage();
        c1Chart7.Legend.Compass = CompassEnum.East;
        c1Chart7.Legend.Location = new Point(-1, 0);
        RenderObject chartRenderObject8 = (RenderObject) ReportUtil.CreateChartRenderObject(c1Chart7, g, C1doc, 1.0, 0.79);
        C1doc.Body.Children.Add(chartRenderObject8);
      }
    }

    public DataTable GetData() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetPollutionRemovalByMonthAndPollutant(this.TreeShrub).SetParameter<Guid>("y", this.YearGuid).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private class PollRemoval
    {
      public double CORemoval { get; set; }

      public double NO2Removal { get; set; }

      public double O3Removal { get; set; }

      public double PM10Removal { get; set; }

      public double PM25Removal { get; set; }

      public double SO2Removal { get; set; }
    }
  }
}
