// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.BenefitsSummaryBySpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
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

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  internal class BenefitsSummaryBySpecies : BenefitsSummaryBase
  {
    public BenefitsSummaryBySpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTItleBenefitsSummaryBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary1 = new List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>();
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary2 = this.AddTreeCountToLstBenefitsSummary(this.GetTreeCount(), lstBenefitsSummary1);
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary3 = this.AddCarbonStorageToLstBenefitsSummary(this.GetCarbonStorage(), lstBenefitsSummary2);
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary4 = this.AddCarbonSequestrationToLstBenefitsSummary(this.GetCarbonSequestration(), lstBenefitsSummary3);
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary5 = this.AddAvoidedRunoffToLstBenefitsSummary(this.GetAvoidedRunoff(), lstBenefitsSummary4);
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary6 = this.AddPollutionRemovalTonsToLstBenefitsSummary(this.GetPollutionRemovalTons(), lstBenefitsSummary5);
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary7 = this.AddStructualValueToLstBenefitsSummary(this.GetStructuralValue(), lstBenefitsSummary6);
      lstBenefitsSummary7.Add(new KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>((int) this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total"), this.getTotalLine()));
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Style.FontSize = 11f;
      renderTable.Rows[0].Style.FontSize = 12f;
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.ColGroups[1, 2].SplitBehavior = renderTable.ColGroups[3, 3].SplitBehavior = renderTable.ColGroups[6, 3].SplitBehavior = renderTable.ColGroups[9, 2].SplitBehavior = renderTable.ColGroups[11, 2].SplitBehavior = renderTable.ColGroups[13, 2].SplitBehavior = SplitBehaviorEnum.Never;
      renderTable.Cells[0, 1].SpanCols = 2;
      renderTable.Cells[0, 3].SpanCols = 3;
      renderTable.Cells[0, 6].SpanCols = 3;
      renderTable.Cells[0, 9].SpanCols = 2;
      renderTable.Cells[0, 11].SpanCols = 2;
      renderTable.Cells[0, 13].SpanCols = 2;
      renderTable.Cells[0, 1].Text = v6Strings.Tree_PluralName;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.GrossCarbonSequestration;
      renderTable.Cells[0, 9].Text = i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff;
      renderTable.Cells[0, 11].Text = i_Tree_Eco_v6.Resources.Strings.PollutionRemoval;
      renderTable.Cells[0, 13].Text = i_Tree_Eco_v6.Resources.Strings.ReplacementValue;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Number;
      if (this.series.IsSample)
      {
        renderTable.Cells[1, 2].Text = renderTable.Cells[1, 4].Text = renderTable.Cells[1, 7].Text = renderTable.Cells[1, 14].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      }
      else
      {
        renderTable.Cols[2].Visible = false;
        renderTable.Cols[4].Visible = false;
        renderTable.Cols[7].Visible = false;
        renderTable.Cols[14].Visible = false;
      }
      renderTable.Cols[11].Visible = renderTable.Cols[12].Visible = this.pollutionIsAvailable;
      renderTable.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonneUnits());
      renderTable.Cells[1, 5].Text = renderTable.Cells[1, 13].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
      renderTable.Cells[1, 6].Text = renderTable.Cells[1, 11].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonneUnits());
      renderTable.Cells[1, 8].Text = renderTable.Cells[1, 10].Text = renderTable.Cells[1, 12].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      renderTable.Cells[1, 9].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.CubicMeterUnits());
      renderTable.Cols[1].Style.Borders.Left = renderTable.Cols[2].Style.Borders.Right = renderTable.Cols[3].Style.Borders.Left = renderTable.Cols[5].Style.Borders.Right = renderTable.Cols[6].Style.Borders.Left = renderTable.Cols[8].Style.Borders.Right = renderTable.Cols[9].Style.Borders.Left = renderTable.Cols[10].Style.Borders.Right = renderTable.Cols[11].Style.Borders.Left = renderTable.Cols[12].Style.Borders.Right = renderTable.Cols[13].Style.Borders.Left = renderTable.Style.Borders.Right = new LineDef((Unit) "1pt", Color.Gray);
      int num1 = count;
      for (int index = 0; index < lstBenefitsSummary7.Count; ++index)
      {
        renderTable.Cells[num1, 0].Text = ReportBase.ScientificName ? this.estUtil.ClassValues[Classifiers.Species][(short) lstBenefitsSummary7[index].Key].Item2 : this.estUtil.ClassValues[Classifiers.Species][(short) lstBenefitsSummary7[index].Key].Item1;
        TableCell cell1 = renderTable.Cells[num1, 1];
        double num2 = lstBenefitsSummary7[index].Value.NumTrees;
        string str1 = num2.ToString("N0");
        cell1.Text = str1;
        if (this.series.IsSample)
        {
          TableCell cell2 = renderTable.Cells[num1, 2];
          num2 = lstBenefitsSummary7[index].Value.NumTreesSE;
          string str2 = "±" + num2.ToString("N0");
          cell2.Text = str2;
        }
        TableCell cell3 = renderTable.Cells[num1, 3];
        num2 = EstimateUtil.ConvertToEnglish(lstBenefitsSummary7[index].Value.CarbonStorage, Units.MetricTons, ReportBase.EnglishUnits);
        string str3 = num2.ToString("N2");
        cell3.Text = str3;
        if (this.series.IsSample)
        {
          TableCell cell4 = renderTable.Cells[num1, 4];
          num2 = EstimateUtil.ConvertToEnglish(lstBenefitsSummary7[index].Value.CarbonStorageSE, Units.MetricTons, ReportBase.EnglishUnits);
          string str4 = "±" + num2.ToString("N2");
          cell4.Text = str4;
        }
        TableCell cell5 = renderTable.Cells[num1, 5];
        num2 = lstBenefitsSummary7[index].Value.CarbonStorage * this.customizedCarbonDollarsPerTon;
        string str5 = num2.ToString("N2");
        cell5.Text = str5;
        TableCell cell6 = renderTable.Cells[num1, 6];
        num2 = EstimateUtil.ConvertToEnglish(lstBenefitsSummary7[index].Value.GrossCarbonSeq, Units.MetricTons, ReportBase.EnglishUnits);
        string str6 = num2.ToString("N2");
        cell6.Text = str6;
        if (this.series.IsSample)
        {
          TableCell cell7 = renderTable.Cells[num1, 7];
          num2 = EstimateUtil.ConvertToEnglish(lstBenefitsSummary7[index].Value.GrossCarbonSeqSE, Units.MetricTons, ReportBase.EnglishUnits);
          string str7 = "±" + num2.ToString("N2");
          cell7.Text = str7;
        }
        TableCell cell8 = renderTable.Cells[num1, 8];
        num2 = lstBenefitsSummary7[index].Value.GrossCarbonSeq * this.customizedCarbonDollarsPerTon;
        string str8 = num2.ToString("N2");
        cell8.Text = str8;
        TableCell cell9 = renderTable.Cells[num1, 9];
        num2 = EstimateUtil.ConvertToEnglish(lstBenefitsSummary7[index].Value.AvoidedRunoff, Units.CubicMeter, ReportBase.EnglishUnits);
        string str9 = num2.ToString("N2");
        cell9.Text = str9;
        TableCell cell10 = renderTable.Cells[num1, 10];
        num2 = lstBenefitsSummary7[index].Value.AvoidedRunoff * this.customizedWaterDollarsPerM3;
        string str10 = num2.ToString("N2");
        cell10.Text = str10;
        BenefitsSummaryBase.PollutionRemoval pollutionRemoval = lstBenefitsSummary7[index].Value.PollutionRemoval;
        TableCell cell11 = renderTable.Cells[num1, 11];
        num2 = EstimateUtil.ConvertToEnglish(pollutionRemoval.PollutionTotal, Units.MetricTons, ReportBase.EnglishUnits);
        string str11 = num2.ToString("N2");
        cell11.Text = str11;
        double num3 = pollutionRemoval.PollutionAmountCo * this.customizedCoDollarsPerTon + pollutionRemoval.PollutionAmountNO2 * this.customizedNO2DollarsPerTon + pollutionRemoval.PollutionAmountO3 * this.customizedO3DollarsPerTon + pollutionRemoval.PollutionAmountPM10 * this.customizedPM10DollarsPerTon + pollutionRemoval.PollutionAmountPM25 * this.customizedPM25DollarsPerTon + pollutionRemoval.PollutionAmountSO2 * this.customizedSO2DollarsPerTon;
        renderTable.Cells[num1, 12].Text = num3.ToString("N2");
        TableCell cell12 = renderTable.Cells[num1, 13];
        num2 = lstBenefitsSummary7[index].Value.StructuralValue;
        string str12 = num2.ToString("N2");
        cell12.Text = str12;
        if (this.series.IsSample)
        {
          TableCell cell13 = renderTable.Cells[num1, 14];
          num2 = lstBenefitsSummary7[index].Value.StructuralValueSE;
          string str13 = "±" + num2.ToString("N2");
          cell13.Text = str13;
        }
        if (this.estUtil.ClassValues[Classifiers.Species][(short) lstBenefitsSummary7[index].Key].Item1 == "Total")
        {
          renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
          renderTable.Rows[num1].Style.Borders.Top = LineDef.Default;
          renderTable.Rows[num1].Style.FontBold = true;
        }
        ++num1;
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.Note(C1doc);
    }

    private List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> AddTreeCountToLstBenefitsSummary(
      DataTable estimatedValues,
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary)
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary1 = lstBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
        lstBenefitsSummary1.Add(new KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>(ReportUtil.ConvertFromDBVal<int>((object) ReportUtil.ConvertFromDBVal<int>(row[classifierName])), new BenefitsSummaryBase.BenefitsSummary()
        {
          NumTrees = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]),
          NumTreesSE = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"])
        }));
      return lstBenefitsSummary1;
    }

    private List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> AddCarbonStorageToLstBenefitsSummary(
      DataTable estimatedValues,
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary)
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary1 = lstBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[classifierName]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        lstBenefitsSummary1.Find((Predicate<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>) (item => item.Key == curSpecies)).Value.CarbonStorage = num1;
        lstBenefitsSummary1.Find((Predicate<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>) (item => item.Key == curSpecies)).Value.CarbonStorageSE = num2;
      }
      return lstBenefitsSummary1;
    }

    private List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> AddCarbonSequestrationToLstBenefitsSummary(
      DataTable estimatedValues,
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary)
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary1 = lstBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[classifierName]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        lstBenefitsSummary1.Find((Predicate<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>) (item => item.Key == curSpecies)).Value.GrossCarbonSeq = num1;
        lstBenefitsSummary1.Find((Predicate<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>) (item => item.Key == curSpecies)).Value.GrossCarbonSeqSE = num2;
      }
      return lstBenefitsSummary1;
    }

    private List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> AddAvoidedRunoffToLstBenefitsSummary(
      DataTable estimatedValues,
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary)
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary1 = lstBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[classifierName]);
        double num = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        lstBenefitsSummary1.Find((Predicate<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>) (item => item.Key == curSpecies)).Value.AvoidedRunoff = num;
      }
      return lstBenefitsSummary1;
    }

    private List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> AddStructualValueToLstBenefitsSummary(
      DataTable estimatedValues,
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary)
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary1 = lstBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[classifierName]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        lstBenefitsSummary1.Find((Predicate<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>) (item => item.Key == curSpecies)).Value.StructuralValue = num1;
        lstBenefitsSummary1.Find((Predicate<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>) (item => item.Key == curSpecies)).Value.StructuralValueSE = num2;
      }
      return lstBenefitsSummary1;
    }

    private List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> AddPollutionRemovalTonsToLstBenefitsSummary(
      DataTable estimatedValues,
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary)
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      List<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>> lstBenefitsSummary1 = lstBenefitsSummary;
      int valueOrderFromName1 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "CO");
      int valueOrderFromName2 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "NO2");
      int valueOrderFromName3 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "O3");
      int valueOrderFromName4 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM10*");
      int valueOrderFromName5 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM2.5");
      int valueOrderFromName6 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "SO2");
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[classifierName]);
        int num1 = ReportUtil.ConvertFromDBVal<int>(row["Pollutant"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["PolutionRemoval"]);
        KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary> keyValuePair = lstBenefitsSummary1.Find((Predicate<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>) (item => item.Key == curSpecies));
        BenefitsSummaryBase.PollutionRemoval pollutionRemoval = keyValuePair.Value.PollutionRemoval;
        if (pollutionRemoval == null)
        {
          pollutionRemoval = new BenefitsSummaryBase.PollutionRemoval();
          keyValuePair = lstBenefitsSummary1.Find((Predicate<KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary>>) (item => item.Key == curSpecies));
          keyValuePair.Value.PollutionRemoval = pollutionRemoval;
        }
        if (num1 == valueOrderFromName1)
          pollutionRemoval.PollutionAmountCo = num2;
        else if (num1 == valueOrderFromName2)
          pollutionRemoval.PollutionAmountNO2 = num2;
        else if (num1 == valueOrderFromName3)
          pollutionRemoval.PollutionAmountO3 = num2;
        else if (num1 == valueOrderFromName4)
          pollutionRemoval.PollutionAmountPM10 = num2;
        else if (num1 == valueOrderFromName5)
          pollutionRemoval.PollutionAmountPM25 = num2;
        else if (num1 == valueOrderFromName6)
          pollutionRemoval.PollutionAmountSO2 = num2;
      }
      return lstBenefitsSummary1;
    }

    private BenefitsSummaryBase.BenefitsSummary getTotalLine()
    {
      BenefitsSummaryBase.BenefitsSummary totalLine = new BenefitsSummaryBase.BenefitsSummary();
      foreach (DataRow row in (InternalDataCollectionBase) this.GetTotals().Rows)
      {
        int num1 = ReportUtil.ConvertFromDBVal<int>(row["EstimateType"]);
        int num2 = ReportUtil.ConvertFromDBVal<int>(row["EstimateUnitsId"]);
        int num3 = ReportUtil.ConvertFromDBVal<int>(row["EquationType"]);
        double num4 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num5 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        if (num3 == 1)
        {
          if (num1 == 1 && num2 == this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)])
          {
            totalLine.NumTrees = num4;
            totalLine.NumTreesSE = num5;
          }
          else if (num1 == 2 && num2 == this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)])
          {
            totalLine.CarbonStorage = num4;
            totalLine.CarbonStorageSE = num5;
          }
          else if (num1 == 3 && num2 == this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)])
          {
            totalLine.GrossCarbonSeq = num4;
            totalLine.GrossCarbonSeqSE = num5;
          }
          else if (num1 == 34 && num2 == this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.Year)])
            totalLine.AvoidedRunoff = num4;
          else if (num1 == 8 && num2 == this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Monetaryunit, Units.None, Units.None)])
          {
            totalLine.StructuralValue = num4;
            totalLine.StructuralValueSE = num5;
          }
        }
      }
      totalLine.PollutionRemoval = new BenefitsSummaryBase.PollutionRemoval();
      foreach (DataRow row in (InternalDataCollectionBase) this.GetPollutionRemovalTotals().Rows)
      {
        int num6 = ReportUtil.ConvertFromDBVal<int>(row["Pollutant"]);
        double num7 = ReportUtil.ConvertFromDBVal<double>(row["PolutionRemoval"]);
        int valueOrderFromName1 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "CO");
        int valueOrderFromName2 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "NO2");
        int valueOrderFromName3 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "O3");
        int valueOrderFromName4 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM10*");
        int valueOrderFromName5 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM2.5");
        int valueOrderFromName6 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "SO2");
        if (num6 == valueOrderFromName1)
          totalLine.PollutionRemoval.PollutionAmountCo = num7;
        else if (num6 == valueOrderFromName2)
          totalLine.PollutionRemoval.PollutionAmountNO2 = num7;
        else if (num6 == valueOrderFromName3)
          totalLine.PollutionRemoval.PollutionAmountO3 = num7;
        else if (num6 == valueOrderFromName4)
          totalLine.PollutionRemoval.PollutionAmountPM10 = num7;
        else if (num6 == valueOrderFromName5)
          totalLine.PollutionRemoval.PollutionAmountPM25 = num7;
        else if (num6 == valueOrderFromName6)
          totalLine.PollutionRemoval.PollutionAmountSO2 = num7;
      }
      return totalLine;
    }

    private IQuery GetEstimatesQuery()
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSE(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Species
      })], classifierName);
    }

    private IQuery GetEstimatedSpeciesTotalsValuesWithSE() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedSpeciesTotalsValuesWithSE(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private IQuery GetPollutantsEstimatesQuery()
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedPollutantsValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species,
        Classifiers.Pollutant
      })], classifierName);
    }

    private IQuery GetPollutantsTotalEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedPollutantsTotalValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Species,
      Classifiers.Pollutant
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private DataTable GetTreeCount() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetCarbonStorage() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CarbonStorage]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetCarbonSequestration() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetAvoidedRunoff() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.AvoidedRunoff]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.Year)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetStructuralValue() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CompensatoryValue]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Monetaryunit, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetPollutionRemovalTons() => this.GetPollutantsEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.PollutionRemoval]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("speciesTotalCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetTotals() => this.GetEstimatedSpeciesTotalsValuesWithSE().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<short>("StudyAreaCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetPollutionRemovalTotals() => this.GetPollutantsTotalEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.PollutionRemoval]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("StudyAreaCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
  }
}
