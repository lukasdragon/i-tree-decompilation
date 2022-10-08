// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.BenefitsSummaryByStrataSpecies
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
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  internal class BenefitsSummaryByStrataSpecies : BenefitsSummaryBase
  {
    public BenefitsSummaryByStrataSpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleBenefitsSummaryByStratumAndSpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> dictBenefitsSummary = new Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>>();
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary1 = this.AddTreeCountToDictOfBenefitsSummary(this.GetTreeCount(), dictBenefitsSummary);
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary2 = this.AddCarbonStorageToDictOfBenefitsSummary(this.GetCarbonStorage(), ofBenefitsSummary1);
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary3 = this.AddCarbonSequestrationToDictOfBenefitsSummary(this.GetCarbonSequestration(), ofBenefitsSummary2);
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary4 = this.AddAvoidedRunoffToDictOfBenefitsSummary(this.GetAvoidedRunoff(), ofBenefitsSummary3);
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary5 = this.AddPollutionRemovalToDictOfBenefitsSummary(this.GetPollutionRemovalTons(), ofBenefitsSummary4);
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary6 = this.AddStructuralValueToDictOfBenefitsSummary(this.GetStructuralValue(), ofBenefitsSummary5);
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      renderTable.Style.FontSize = 11f;
      renderTable.Rows[0].Style.FontSize = 12f;
      renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[0].Style.FontBold = true;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Cells[0, 2].Text = v6Strings.Tree_PluralName;
      renderTable.Cells[0, 2].SpanCols = 2;
      renderTable.Cells[0, 4].SpanCols = 3;
      renderTable.Cells[0, 7].SpanCols = 3;
      renderTable.Cells[0, 10].SpanCols = 2;
      renderTable.Cells[0, 12].SpanCols = 2;
      renderTable.Cells[0, 14].SpanCols = 2;
      renderTable.ColGroups[2, 2].SplitBehavior = renderTable.ColGroups[4, 3].SplitBehavior = renderTable.ColGroups[7, 3].SplitBehavior = renderTable.ColGroups[10, 2].SplitBehavior = renderTable.ColGroups[12, 2].SplitBehavior = renderTable.ColGroups[14, 2].SplitBehavior = SplitBehaviorEnum.Never;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.GrossCarbonSequestration;
      renderTable.Cells[0, 10].Text = i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff;
      renderTable.Cells[0, 12].Text = i_Tree_Eco_v6.Resources.Strings.PollutionRemoval;
      renderTable.Cells[0, 14].Text = i_Tree_Eco_v6.Resources.Strings.ReplacementValue;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.Number;
      if (this.series.IsSample)
      {
        renderTable.Cells[1, 3].Text = renderTable.Cells[1, 5].Text = renderTable.Cells[1, 8].Text = renderTable.Cells[1, 15].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      }
      else
      {
        renderTable.Cols[3].Visible = false;
        renderTable.Cols[3].Visible = false;
        renderTable.Cols[8].Visible = false;
        renderTable.Cols[15].Visible = false;
      }
      renderTable.Cols[12].Visible = renderTable.Cols[13].Visible = this.pollutionIsAvailable;
      renderTable.Cells[1, 4].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonneUnits());
      renderTable.Cells[1, 7].Text = renderTable.Cells[1, 12].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonneUnits());
      renderTable.Cells[1, 10].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.CubicMeterUnits());
      renderTable.Cells[1, 9].Text = renderTable.Cells[1, 11].Text = renderTable.Cells[1, 13].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
      renderTable.Cells[1, 6].Text = renderTable.Cells[1, 14].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
      renderTable.Cols[2].Style.Borders.Left = renderTable.Cols[3].Style.Borders.Right = renderTable.Cols[4].Style.Borders.Left = renderTable.Cols[6].Style.Borders.Right = renderTable.Cols[7].Style.Borders.Left = renderTable.Cols[9].Style.Borders.Right = renderTable.Cols[10].Style.Borders.Left = renderTable.Cols[11].Style.Borders.Right = renderTable.Cols[12].Style.Borders.Left = renderTable.Cols[13].Style.Borders.Right = renderTable.Cols[14].Style.Borders.Left = renderTable.Style.Borders.Right = new LineDef((Unit) "1pt", Color.Gray);
      bool flag = ofBenefitsSummary6.Count == 2 && this.estUtil.ClassValues[Classifiers.Strata][(short) 2].Item1 == "Study Area";
      int num1 = count;
      foreach (KeyValuePair<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> keyValuePair1 in ofBenefitsSummary6.ToList<KeyValuePair<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>>>())
      {
        string str1 = this.estUtil.ClassValues[Classifiers.Strata][(short) keyValuePair1.Key].Item1;
        if (!flag || !(str1 == "Study Area"))
        {
          if (flag && !this.curYear.RecordStrata)
            renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
          else if (str1 == "Study Area")
          {
            renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
            renderTable.Rows[num1].Style.FontBold = true;
          }
          else
            renderTable.Cells[num1, 0].Text = str1;
          foreach (KeyValuePair<int, BenefitsSummaryBase.BenefitsSummary> keyValuePair2 in keyValuePair1.Value)
          {
            string str2 = this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair2.Key].Item1;
            if (str1 != "Study Area")
              renderTable.Cells[num1, 1].Text = ReportBase.ScientificName ? this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair2.Key].Item2 : this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair2.Key].Item1;
            if (str2 == "Total" && str1 != "Study Area")
            {
              renderTable.Cells[num1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
              renderTable.Rows[num1].Style.FontBold = true;
              renderTable.Rows[num1].Style.Borders.Top = renderTable.Rows[num1].Style.Borders.Bottom = LineDef.Default;
            }
            renderTable.Cells[num1, 2].Text = keyValuePair2.Value.NumTrees.ToString("N0");
            if (this.series.IsSample)
              renderTable.Cells[num1, 3].Text = "±" + keyValuePair2.Value.NumTreesSE.ToString("N0");
            renderTable.Cells[num1, 4].Text = EstimateUtil.ConvertToEnglish(keyValuePair2.Value.CarbonStorage, Units.MetricTons, ReportBase.EnglishUnits).ToString("N2");
            double num2;
            if (this.series.IsSample)
            {
              TableCell cell = renderTable.Cells[num1, 5];
              num2 = EstimateUtil.ConvertToEnglish(keyValuePair2.Value.CarbonStorageSE, Units.MetricTons, ReportBase.EnglishUnits);
              string str3 = "±" + num2.ToString("N2");
              cell.Text = str3;
            }
            TableCell cell1 = renderTable.Cells[num1, 6];
            num2 = keyValuePair2.Value.CarbonStorage * this.customizedCarbonDollarsPerTon;
            string str4 = num2.ToString("N2");
            cell1.Text = str4;
            TableCell cell2 = renderTable.Cells[num1, 7];
            num2 = EstimateUtil.ConvertToEnglish(keyValuePair2.Value.GrossCarbonSeq, Units.MetricTons, ReportBase.EnglishUnits);
            string str5 = num2.ToString("N2");
            cell2.Text = str5;
            if (this.series.IsSample)
            {
              TableCell cell3 = renderTable.Cells[num1, 8];
              num2 = EstimateUtil.ConvertToEnglish(keyValuePair2.Value.GrossCarbonSeqSE, Units.MetricTons, ReportBase.EnglishUnits);
              string str6 = "±" + num2.ToString("N2");
              cell3.Text = str6;
            }
            TableCell cell4 = renderTable.Cells[num1, 9];
            num2 = keyValuePair2.Value.GrossCarbonSeq * this.customizedCarbonDollarsPerTon;
            string str7 = num2.ToString("N2");
            cell4.Text = str7;
            TableCell cell5 = renderTable.Cells[num1, 10];
            num2 = EstimateUtil.ConvertToEnglish(keyValuePair2.Value.AvoidedRunoff, Units.CubicMeter, ReportBase.EnglishUnits);
            string str8 = num2.ToString("N2");
            cell5.Text = str8;
            TableCell cell6 = renderTable.Cells[num1, 11];
            num2 = keyValuePair2.Value.AvoidedRunoff * this.customizedWaterDollarsPerM3;
            string str9 = num2.ToString("N2");
            cell6.Text = str9;
            BenefitsSummaryBase.PollutionRemoval pollutionRemoval = keyValuePair2.Value.PollutionRemoval;
            TableCell cell7 = renderTable.Cells[num1, 12];
            num2 = EstimateUtil.ConvertToEnglish(pollutionRemoval.PollutionTotal, Units.MetricTons, ReportBase.EnglishUnits);
            string str10 = num2.ToString("N2");
            cell7.Text = str10;
            double num3 = pollutionRemoval.PollutionAmountCo * this.customizedCoDollarsPerTon + pollutionRemoval.PollutionAmountNO2 * this.customizedNO2DollarsPerTon + pollutionRemoval.PollutionAmountO3 * this.customizedO3DollarsPerTon + pollutionRemoval.PollutionAmountPM10 * this.customizedPM10DollarsPerTon + pollutionRemoval.PollutionAmountPM25 * this.customizedPM25DollarsPerTon + pollutionRemoval.PollutionAmountSO2 * this.customizedSO2DollarsPerTon;
            renderTable.Cells[num1, 13].Text = num3.ToString("N2");
            TableCell cell8 = renderTable.Cells[num1, 14];
            num2 = keyValuePair2.Value.StructuralValue;
            string str11 = num2.ToString("N2");
            cell8.Text = str11;
            if (this.series.IsSample)
            {
              TableCell cell9 = renderTable.Cells[num1, 15];
              num2 = keyValuePair2.Value.StructuralValueSE;
              string str12 = "±" + num2.ToString("N2");
              cell9.Text = str12;
            }
            ++num1;
          }
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
      this.Note(C1doc);
    }

    private IQuery GetEstimatesQuery()
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedSpeciesByStratumValuesWithSE(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species
      })], classifierName1, classifierName2);
    }

    private IQuery GetPollutantsEstimatesQuery()
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedPollutantsByStratumValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species,
        Classifiers.Pollutant
      })], classifierName1, classifierName2);
    }

    private DataTable GetTreeCount() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetCarbonStorage() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CarbonStorage]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetCarbonSequestration() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetAvoidedRunoff() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.AvoidedRunoff]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.Year)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetStructuralValue() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CompensatoryValue]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Monetaryunit, Units.None, Units.None)]).SetParameter<int>("eqType", 1).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetPollutionRemovalTons() => this.GetPollutantsEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.PollutionRemoval]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> AddTreeCountToDictOfBenefitsSummary(
      DataTable estimatedValues,
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> dictBenefitsSummary)
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary = dictBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        if (!ofBenefitsSummary.ContainsKey(key1))
          ofBenefitsSummary.Add(key1, new Dictionary<int, BenefitsSummaryBase.BenefitsSummary>());
        if (!ofBenefitsSummary[key1].ContainsKey(key2))
          ofBenefitsSummary[key1].Add(key2, new BenefitsSummaryBase.BenefitsSummary()
          {
            NumTrees = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]),
            NumTreesSE = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"])
          });
      }
      return ofBenefitsSummary;
    }

    private Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> AddCarbonStorageToDictOfBenefitsSummary(
      DataTable estimatedValues,
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> dictBenefitsSummary)
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary = dictBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        dictBenefitsSummary[key1][key2].CarbonStorage = num1;
        dictBenefitsSummary[key1][key2].CarbonStorageSE = num2;
      }
      return ofBenefitsSummary;
    }

    private Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> AddCarbonSequestrationToDictOfBenefitsSummary(
      DataTable estimatedValues,
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> dictBenefitsSummary)
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary = dictBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        dictBenefitsSummary[key1][key2].GrossCarbonSeq = num1;
        dictBenefitsSummary[key1][key2].GrossCarbonSeqSE = num2;
      }
      return ofBenefitsSummary;
    }

    private Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> AddAvoidedRunoffToDictOfBenefitsSummary(
      DataTable estimatedValues,
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> dictBenefitsSummary)
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary = dictBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        double num = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        dictBenefitsSummary[key1][key2].AvoidedRunoff = num;
      }
      return ofBenefitsSummary;
    }

    private Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> AddStructuralValueToDictOfBenefitsSummary(
      DataTable estimatedValues,
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> dictBenefitsSummary)
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary = dictBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
        dictBenefitsSummary[key1][key2].StructuralValue = num1;
        dictBenefitsSummary[key1][key2].StructuralValueSE = num2;
      }
      return ofBenefitsSummary;
    }

    private Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> AddPollutionRemovalToDictOfBenefitsSummary(
      DataTable estimatedValues,
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> dictBenefitsSummary)
    {
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      Dictionary<int, Dictionary<int, BenefitsSummaryBase.BenefitsSummary>> ofBenefitsSummary = dictBenefitsSummary;
      foreach (DataRow row in (InternalDataCollectionBase) estimatedValues.Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        int num1 = ReportUtil.ConvertFromDBVal<int>(row["Pollutant"]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        int valueOrderFromName1 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "CO");
        int valueOrderFromName2 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "NO2");
        int valueOrderFromName3 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "O3");
        int valueOrderFromName4 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM10*");
        int valueOrderFromName5 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM2.5");
        int valueOrderFromName6 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "SO2");
        if (dictBenefitsSummary[key1][key2].PollutionRemoval == null)
          dictBenefitsSummary[key1][key2].PollutionRemoval = new BenefitsSummaryBase.PollutionRemoval();
        BenefitsSummaryBase.PollutionRemoval pollutionRemoval = dictBenefitsSummary[key1][key2].PollutionRemoval;
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
      return ofBenefitsSummary;
    }
  }
}
