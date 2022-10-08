// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.StructureSummaryByStrataSpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using Eco.Domain.Properties;
using Eco.Util;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  internal class StructureSummaryByStrataSpecies : StructureSummaryBase
  {
    private readonly string _strata;

    public StructureSummaryByStrataSpecies()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleStructureSummaryByStratumAndSpecies;
      this._strata = this.estUtil.ClassifierNames[Classifiers.Strata];
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      C1doc.ClipPage = true;
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Style.FontSize = 11f;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 12f;
      renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cells[0, 2].SpanCols = renderTable.Cells[0, 4].SpanCols = renderTable.Cells[0, 6].SpanCols = renderTable.Cells[0, 8].SpanCols = 2;
      renderTable.Cells[0, 2].Text = v6Strings.Tree_PluralName;
      renderTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.LeafArea;
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.LeafBiomass;
      renderTable.Cells[0, 8].Text = i_Tree_Eco_v6.Resources.Strings.TreeDryWeightBiomass;
      renderTable.Cells[0, 10].Text = i_Tree_Eco_v6.Resources.Strings.AverageCondition;
      renderTable.Cells[0, 0].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.Number;
      renderTable.Cells[1, 4].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.HaUnits());
      renderTable.Cells[1, 6].Text = renderTable.Cells[1, 8].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonneUnits());
      renderTable.Cells[1, 3].Text = renderTable.Cells[1, 5].Text = renderTable.Cells[1, 7].Text = renderTable.Cells[1, 9].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      renderTable.Cells[1, 10].Text = ReportUtil.FormatHeaderUnitsStr("%");
      renderTable.Cols[0].Width = (Unit) "10%";
      renderTable.Cols[1].Width = (Unit) "14%";
      renderTable.Cols[2].Width = (Unit) "7%";
      renderTable.Cols[3].Width = (Unit) "7%";
      renderTable.Cols[8].Width = (Unit) "10%";
      renderTable.Cols[9].Width = (Unit) "10%";
      renderTable.Cols[2].Style.Borders.Left = renderTable.Cols[4].Style.Borders.Left = renderTable.Cols[6].Style.Borders.Left = renderTable.Cols[8].Style.Borders.Left = renderTable.Cols[10].Style.Borders.Left = renderTable.Cols[10].Style.Borders.Right = new LineDef((Unit) "1pt", Color.Gray);
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = this.AverageConditionToStructureSummaryObj(this.CarbonStorageToStructureSummaryObj(this.BiomassToStructureSummaryObj(this.LeafAreaToStructureSummaryObj(this.TreeCountToStructureSummaryObj(new SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>())))));
      bool flag = structureSummaryObj.Count == 2 && this.estUtil.ClassValues[Classifiers.Strata][(short) 2].Item1 == "Study Area";
      int num1 = count;
      foreach (KeyValuePair<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> keyValuePair1 in structureSummaryObj)
      {
        string str1 = this.estUtil.ClassValues[Classifiers.Strata][(short) keyValuePair1.Key].Item1;
        if (!flag || !(str1 == "Study Area"))
        {
          if (flag && !this.curYear.RecordStrata)
          {
            renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
            renderTable.Cells[num1, 0].Style.FontBold = true;
          }
          else
          {
            renderTable.Cells[num1, 0].Text = str1;
            renderTable.Cells[num1, 0].Style.FontBold = true;
          }
          foreach (KeyValuePair<int, StructureSummaryBase.StructureSummarySpecies> keyValuePair2 in keyValuePair1.Value)
          {
            string str2 = this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair2.Key].Item1;
            if (str1 != "Study Area")
              renderTable.Cells[num1, 1].Text = ReportBase.ScientificName ? this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair2.Key].Item2 : this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair2.Key].Item1;
            if (str2 == "Total")
            {
              renderTable.Cells[num1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
              renderTable.Rows[num1].Style.FontBold = true;
              renderTable.Rows[num1].Style.Borders.Top = LineDef.Default;
              renderTable.Rows[num1].Style.Borders.Bottom = LineDef.Default;
              if (str1 == "Study Area")
              {
                renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
                renderTable.Cells[num1, 1].Text = string.Empty;
              }
            }
            renderTable.Cells[num1, 2].Text = keyValuePair2.Value.NumTrees.ToString("N0");
            renderTable.Cells[num1, 4].Text = EstimateUtil.ConvertToEnglish(keyValuePair2.Value.LeafArea * 100.0, Units.Hectare, ReportBase.EnglishUnits).ToString("N3");
            renderTable.Cells[num1, 6].Text = EstimateUtil.ConvertToEnglish(keyValuePair2.Value.LeafBiomass, Units.MetricTons, ReportBase.EnglishUnits).ToString("N3");
            TableCell cell1 = renderTable.Cells[num1, 8];
            double num2 = EstimateUtil.ConvertToEnglish(keyValuePair2.Value.CarbonStorage * 2.0, Units.MetricTons, ReportBase.EnglishUnits);
            string str3 = num2.ToString("N3");
            cell1.Text = str3;
            TableCell cell2 = renderTable.Cells[num1, 3];
            num2 = keyValuePair2.Value.NumTreesSE;
            string str4 = "±" + num2.ToString("N0");
            cell2.Text = str4;
            renderTable.Cells[num1, 5].Text = "±" + EstimateUtil.ConvertToEnglish(keyValuePair2.Value.LeafAreaSE * 100.0, Units.Hectare, ReportBase.EnglishUnits).ToString("N3");
            renderTable.Cells[num1, 7].Text = "±" + EstimateUtil.ConvertToEnglish(keyValuePair2.Value.LeafBiomassSE, Units.MetricTons, ReportBase.EnglishUnits).ToString("N3");
            renderTable.Cells[num1, 9].Text = "±" + EstimateUtil.ConvertToEnglish(keyValuePair2.Value.CarbonStorageSE * 2.0, Units.MetricTons, ReportBase.EnglishUnits).ToString("N3");
            renderTable.Cells[num1, 10].Text = keyValuePair2.Value.AverageCondition.ToString("N2");
            ++num1;
          }
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
    }

    private SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> TreeCountToStructureSummaryObj(
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>((IDictionary<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species
      }, EstimateTypeEnum.NumberofTrees, Units.Count, Units.None, Units.None).Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[this._strata]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        if (!structureSummaryObj.ContainsKey(key1))
          structureSummaryObj.Add(key1, new SortedList<int, StructureSummaryBase.StructureSummarySpecies>());
        if (!structureSummaryObj[key1].ContainsKey(key2))
          structureSummaryObj[key1].Add(key2, new StructureSummaryBase.StructureSummarySpecies()
          {
            NumTrees = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]),
            NumTreesSE = ReportUtil.ConvertFromDBVal<double>(row[this._estimateStandardError])
          });
      }
      return structureSummaryObj;
    }

    private SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> LeafAreaToStructureSummaryObj(
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>((IDictionary<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species
      }, EstimateTypeEnum.LeafArea, Units.Squarekilometer, Units.None, Units.None).Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[this._strata]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateStandardError]);
        structureSummaryObj[key1][key2].LeafArea = num1;
        structureSummaryObj[key1][key2].LeafAreaSE = num2;
      }
      return structureSummaryObj;
    }

    private SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> BiomassToStructureSummaryObj(
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>((IDictionary<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species
      }, EstimateTypeEnum.LeafBiomass, Units.MetricTons, Units.None, Units.None).Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[this._strata]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateStandardError]);
        structureSummaryObj[key1][key2].LeafBiomass = num1;
        structureSummaryObj[key1][key2].LeafBiomassSE = num2;
      }
      return structureSummaryObj;
    }

    private SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> CarbonStorageToStructureSummaryObj(
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>((IDictionary<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetMultipleFieldsData(new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species
      }, EstimateTypeEnum.CarbonStorage, Units.MetricTons, Units.None, Units.None).Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[this._strata]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        double num1 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row[this._estimateStandardError]);
        structureSummaryObj[key1][key2].CarbonStorage = num1;
        structureSummaryObj[key1][key2].CarbonStorageSE = num2;
      }
      return structureSummaryObj;
    }

    private SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> AverageConditionToStructureSummaryObj(
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> lstStructureSummary)
    {
      SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>> structureSummaryObj = new SortedList<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>((IDictionary<int, SortedList<int, StructureSummaryBase.StructureSummarySpecies>>) lstStructureSummary);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetAverageCondition(new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.Species
      }).Rows)
      {
        int key1 = ReportUtil.ConvertFromDBVal<int>(row[this._strata]);
        int key2 = ReportUtil.ConvertFromDBVal<int>(row[this._species]);
        double num = ReportUtil.ConvertFromDBVal<double>(row[this._estimateValue]);
        structureSummaryObj[key1][key2].AverageCondition = num;
      }
      return structureSummaryObj;
    }
  }
}
