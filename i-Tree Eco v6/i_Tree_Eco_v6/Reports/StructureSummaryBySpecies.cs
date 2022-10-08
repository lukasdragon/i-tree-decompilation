// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.StructureSummaryBySpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Util;
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
  internal class StructureSummaryBySpecies : StructureSummaryBase
  {
    public StructureSummaryBySpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleStructureSummaryBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = this.StudyAreaAverageConditionToStructureSummaryObj(this.StudyAreaValuesToStructureSummaryObj(this.AverageConditionToStructureSummaryObj(this.CarbonStorageToStructureSummaryObj(this.BiomassToStructureSummaryObj(this.LeafAreaToStructureSummaryObj(this.TreeCountToStructureSummaryObj(new List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>())))))));
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Style.FontSize = 11f;
      renderTable.Cols[0].Width = (Unit) "17%";
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 12f;
      renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 1].SpanCols = renderTable.Cells[0, 3].SpanCols = renderTable.Cells[0, 5].SpanCols = renderTable.Cells[0, 7].SpanCols = 2;
      renderTable.Cells[0, 1].Text = v6Strings.Tree_PluralName;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.LeafArea;
      renderTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.LeafBiomass;
      renderTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.TreeDryWeightBiomass;
      renderTable.Cells[0, 9].Text = i_Tree_Eco_v6.Resources.Strings.AverageCondition;
      renderTable.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Number;
      renderTable.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.HaUnits());
      renderTable.Cells[1, 5].Text = renderTable.Cells[1, 7].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonneUnits());
      renderTable.Cells[1, 2].Text = renderTable.Cells[1, 4].Text = renderTable.Cells[1, 6].Text = renderTable.Cells[1, 8].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      renderTable.Cells[1, 9].Text = ReportUtil.FormatHeaderUnitsStr("%");
      LineDef lineDef = new LineDef((Unit) "1pt", Color.Gray);
      renderTable.Cols[1].Style.Borders.Left = lineDef;
      renderTable.Cols[3].Style.Borders.Left = lineDef;
      renderTable.Cols[5].Style.Borders.Left = lineDef;
      renderTable.Cols[7].Style.Borders.Left = lineDef;
      renderTable.Cols[9].Style.Borders.Left = lineDef;
      renderTable.Cols[9].Style.Borders.Right = lineDef;
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> list = structureSummaryObj.OrderByDescending<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>, double>((Func<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>, double>) (p => p.Value.NumTrees)).ToList<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>();
      KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies> dataRow1 = EstimateUtil.PopAt<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>(list, 0);
      int num = count;
      foreach (KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies> dataRow2 in list)
        this.AssignValues(renderTable, num++, dataRow2);
      this.AssignValues(renderTable, num, dataRow1);
      renderTable.Cells[num, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
      renderTable.Rows[num].Style.FontBold = true;
      renderTable.Rows[num].Style.Borders.Top = LineDef.Default;
      ReportUtil.FormatRenderTable(renderTable);
    }

    private void AssignValues(
      RenderTable rTable,
      int row,
      KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies> dataRow)
    {
      StructureSummaryBase.StructureSummarySpecies structureSummarySpecies = dataRow.Value;
      rTable.Cells[row, 0].Text = ReportBase.ScientificName ? this.estUtil.ClassValues[Classifiers.Species][(short) dataRow.Key].Item2 : this.estUtil.ClassValues[Classifiers.Species][(short) dataRow.Key].Item1;
      TableCell cell1 = rTable.Cells[row, 1];
      double num = structureSummarySpecies.NumTrees;
      string str1 = num.ToString("N0");
      cell1.Text = str1;
      TableCell cell2 = rTable.Cells[row, 3];
      num = EstimateUtil.ConvertToEnglish(structureSummarySpecies.LeafArea * 100.0, Units.Hectare, ReportBase.EnglishUnits);
      string str2 = num.ToString("N3");
      cell2.Text = str2;
      TableCell cell3 = rTable.Cells[row, 5];
      num = EstimateUtil.ConvertToEnglish(structureSummarySpecies.LeafBiomass, Units.MetricTons, ReportBase.EnglishUnits);
      string str3 = num.ToString("N3");
      cell3.Text = str3;
      TableCell cell4 = rTable.Cells[row, 7];
      num = EstimateUtil.ConvertToEnglish(structureSummarySpecies.CarbonStorage * 2.0, Units.MetricTons, ReportBase.EnglishUnits);
      string str4 = num.ToString("N3");
      cell4.Text = str4;
      TableCell cell5 = rTable.Cells[row, 2];
      num = structureSummarySpecies.NumTreesSE;
      string str5 = "±" + num.ToString("N0");
      cell5.Text = str5;
      TableCell cell6 = rTable.Cells[row, 4];
      num = EstimateUtil.ConvertToEnglish(structureSummarySpecies.LeafAreaSE * 100.0, Units.Hectare, ReportBase.EnglishUnits);
      string str6 = "±" + num.ToString("N3");
      cell6.Text = str6;
      TableCell cell7 = rTable.Cells[row, 6];
      num = EstimateUtil.ConvertToEnglish(structureSummarySpecies.LeafBiomassSE, Units.MetricTons, ReportBase.EnglishUnits);
      string str7 = "±" + num.ToString("N3");
      cell7.Text = str7;
      TableCell cell8 = rTable.Cells[row, 8];
      num = EstimateUtil.ConvertToEnglish(structureSummarySpecies.CarbonStorageSE * 2.0, Units.MetricTons, ReportBase.EnglishUnits);
      string str8 = "±" + num.ToString("N3");
      cell8.Text = str8;
      TableCell cell9 = rTable.Cells[row, 9];
      num = structureSummarySpecies.AverageCondition;
      string str9 = num.ToString("N2");
      cell9.Text = str9;
    }

    private List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> TreeCountToStructureSummaryObj(
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>((IEnumerable<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Species
      }, EstimateTypeEnum.NumberofTrees, Units.Count, Units.None, Units.None).Rows)
      {
        int num = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        structureSummaryObj.Add(new KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>(ReportUtil.ConvertFromDBVal<int>((object) num), new StructureSummaryBase.StructureSummarySpecies()
        {
          NumTrees = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]),
          NumTreesSE = ReportUtil.ConvertFromDBVal<double>(row[this._estimateStandardError])
        }));
      }
      return structureSummaryObj;
    }

    private List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> LeafAreaToStructureSummaryObj(
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>((IEnumerable<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Species
      }, EstimateTypeEnum.LeafArea, Units.Squarekilometer, Units.None, Units.None).Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateStandardError]);
        KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies> keyValuePair = structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item => item.Key == curSpecies));
        keyValuePair.Value.LeafArea = num1;
        keyValuePair = structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item => item.Key == curSpecies));
        keyValuePair.Value.LeafAreaSE = num2;
      }
      return structureSummaryObj;
    }

    private List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> BiomassToStructureSummaryObj(
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>((IEnumerable<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Species
      }, EstimateTypeEnum.LeafBiomass, Units.MetricTons, Units.None, Units.None).Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateStandardError]);
        KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies> keyValuePair = structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item => item.Key == curSpecies));
        keyValuePair.Value.LeafBiomass = num1;
        keyValuePair = structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item => item.Key == curSpecies));
        keyValuePair.Value.LeafBiomassSE = num2;
      }
      return structureSummaryObj;
    }

    private List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> CarbonStorageToStructureSummaryObj(
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>((IEnumerable<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Species
      }, EstimateTypeEnum.CarbonStorage, Units.MetricTons, Units.None, Units.None).Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateStandardError]);
        KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies> keyValuePair = structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item => item.Key == curSpecies));
        keyValuePair.Value.CarbonStorage = num1;
        keyValuePair = structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item => item.Key == curSpecies));
        keyValuePair.Value.CarbonStorageSE = num2;
      }
      return structureSummaryObj;
    }

    private List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> AverageConditionToStructureSummaryObj(
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>((IEnumerable<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetAverageCondition(new List<Classifiers>()
      {
        Classifiers.Species
      }).Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        double num = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]);
        structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item => item.Key == curSpecies)).Value.AverageCondition = num;
      }
      return structureSummaryObj;
    }

    private List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> StudyAreaValuesToStructureSummaryObj(
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>((IEnumerable<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      DataTable studyAreaValues = this.GetStudyAreaValues(this._totalSpecies);
      structureSummaryObj.Add(new KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>(this._totalSpecies, new StructureSummaryBase.StructureSummarySpecies()));
      string columnName = "EstimateType";
      foreach (DataRow row in (InternalDataCollectionBase) studyAreaValues.Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        int num1 = ReportUtil.ConvertFromDBVal<int>(row[columnName]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]);
        double num3 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateStandardError]);
        if (num1 == this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees])
        {
          structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item1 => item1.Key == curSpecies)).Value.NumTrees = num2;
          structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item1 => item1.Key == curSpecies)).Value.NumTreesSE = num3;
        }
        else if (num1 == this.estUtil.EstTypes[EstimateTypeEnum.LeafArea])
        {
          structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item1 => item1.Key == curSpecies)).Value.LeafArea = num2;
          structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item1 => item1.Key == curSpecies)).Value.LeafAreaSE = num3;
        }
        else if (num1 == this.estUtil.EstTypes[EstimateTypeEnum.LeafBiomass])
        {
          structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item1 => item1.Key == curSpecies)).Value.LeafBiomass = num2;
          structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item1 => item1.Key == curSpecies)).Value.LeafBiomassSE = num3;
        }
        else if (num1 == this.estUtil.EstTypes[EstimateTypeEnum.CarbonStorage])
        {
          structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item1 => item1.Key == curSpecies)).Value.CarbonStorage = num2;
          structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item1 => item1.Key == curSpecies)).Value.CarbonStorageSE = num3;
        }
      }
      return structureSummaryObj;
    }

    private List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> StudyAreaAverageConditionToStructureSummaryObj(
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new List<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>((IEnumerable<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetStudyAreaAverageCondition(this._totalSpecies).Rows)
      {
        int curSpecies = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        double num = ReportUtil.ConvertFromDBVal<double>(row["AverageCondition"]);
        structureSummaryObj.Find((Predicate<KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies>>) (item => item.Key == curSpecies)).Value.AverageCondition = num;
      }
      return structureSummaryObj;
    }

    public DataTable GetStudyAreaValues(int totalSpecies) => this.estUtil.queryProvider.GetEstimateUtilProvider().GetStudyAreaValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.Species]).SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<int>("eqType", 1).SetParameter<int>("estType2", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits2", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)]).SetParameter<int>("eqType2", 1).SetParameter<int>("estType3", this.estUtil.EstTypes[EstimateTypeEnum.LeafBiomass]).SetParameter<int>("estUnits3", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<int>("eqType3", 1).SetParameter<int>("estType4", this.estUtil.EstTypes[EstimateTypeEnum.CarbonStorage]).SetParameter<int>("estUnits4", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<int>("eqType4", 1).SetParameter<short>("strataStudyArea", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetParameter<int>("speciesTotal", totalSpecies).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    public DataTable GetStudyAreaAverageCondition(int totalSpecies) => this.estUtil.queryProvider.GetEstimateUtilProvider().GetStudyAreaAverageCondition(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Species,
      Classifiers.Dieback
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.Species], this.estUtil.ClassifierNames[Classifiers.Dieback]).SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<int>("eqType", 1).SetParameter<short>("strataStudyArea", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetParameter<int>("speciesTotal", totalSpecies).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
  }
}
