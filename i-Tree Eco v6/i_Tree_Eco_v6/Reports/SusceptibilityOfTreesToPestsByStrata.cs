// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.SusceptibilityOfTreesToPestsByStrata
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Util;
using Eco.Util.Services;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  internal class SusceptibilityOfTreesToPestsByStrata : DatabaseReport
  {
    private const int _sqKilometerToHa = 100;
    private Dictionary<int, Dictionary<int, double>> _treeCounts = new Dictionary<int, Dictionary<int, double>>();
    private Dictionary<int, Dictionary<int, double>> _structuralValues = new Dictionary<int, Dictionary<int, double>>();
    private Dictionary<int, Dictionary<int, double>> _leafAreas = new Dictionary<int, Dictionary<int, double>>();

    public SusceptibilityOfTreesToPestsByStrata()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleSusceptibilityToPestsByStratum;
      this.PestService = new PestService(ReportBase.m_ps.LocSp);
    }

    private PestService PestService { get; }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      string abbreviation = this.nation.Currency.Abbreviation;
      List<Pest> validPests = this.GetValidPests();
      Dictionary<Pest, List<int>> speciesForAllPests = this.GetSusceptibleSpeciesForAllPests(validPests);
      Pest key1 = new Pest()
      {
        ABB = "AP",
        CommonName = i_Tree_Eco_v6.Resources.Strings.AllPests,
        ScientificName = i_Tree_Eco_v6.Resources.Strings.AllPests
      };
      List<int> intList1 = new List<int>();
      foreach (KeyValuePair<Pest, List<int>> keyValuePair in speciesForAllPests)
      {
        for (int index = 0; index < keyValuePair.Value.Count; ++index)
        {
          if (!intList1.Contains(keyValuePair.Value[index]))
            intList1.Add(keyValuePair.Value[index]);
        }
      }
      validPests.Add(key1);
      speciesForAllPests.Add(key1, intList1);
      SortedList<int, List<int>> strataSpeciesList = this.GetStrataSpeciesList();
      Dictionary<Pest, SortedList<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>> dictionary = new Dictionary<Pest, SortedList<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>();
      for (int index1 = 0; index1 < strataSpeciesList.Count; ++index1)
      {
        int key2 = strataSpeciesList.Keys[index1];
        List<int> intList2 = strataSpeciesList[key2];
        for (int index2 = 0; index2 < validPests.Count; ++index2)
        {
          Pest key3 = validPests[index2];
          List<int> intList3 = speciesForAllPests[key3];
          if (!dictionary.ContainsKey(validPests[index2]))
            dictionary.Add(key3, new SortedList<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>());
          if (!dictionary[key3].ContainsKey(key2))
            dictionary[key3].Add(key2, new SusceptibilityOfTreesToPestsByStrata.PestStrataDetail());
          for (int index3 = 0; index3 < intList2.Count; ++index3)
          {
            int SpeciesCVO = intList2[index3];
            if (intList3.Contains(SpeciesCVO))
            {
              dictionary[key3][key2].SusceptibleLeafArea += this.GetLeafArea(SpeciesCVO, key2);
              dictionary[key3][key2].SusceptibleStructuralValue += this.GetStructuralValue(SpeciesCVO, key2);
              dictionary[key3][key2].SusceptibleTreeCount += this.GetTreeCount(SpeciesCVO, key2);
            }
            else
            {
              dictionary[key3][key2].NotSusceptibleLeafArea += this.GetLeafArea(SpeciesCVO, key2);
              dictionary[key3][key2].NotSusceptibleStructuralValue += this.GetStructuralValue(SpeciesCVO, key2);
              dictionary[key3][key2].NotSusceptibleTreeCount += this.GetTreeCount(SpeciesCVO, key2);
            }
          }
        }
      }
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      int count = 2;
      int num1 = count;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.BreakAfter = BreakEnum.None;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Width = Unit.Auto;
      renderTable.SplitHorzBehavior = SplitBehaviorEnum.SplitIfNeeded;
      renderTable.Cells[0, 2].SpanCols = renderTable.Cells[0, 4].SpanCols = renderTable.Cells[0, 6].SpanCols = renderTable.Cells[0, 8].SpanCols = 2;
      renderTable.Style.FontSize = 8f;
      renderTable.Rows[0].Style.FontSize = renderTable.Rows[1].Style.FontSize = 10f;
      renderTable.Rows[0].Style.FontBold = renderTable.Rows[1].Style.FontBold = true;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
      renderTable.Cells[0, 4].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.ReplacementValue, this.CurrencySymbol);
      renderTable.Cells[0, 6].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, "%");
      renderTable.Cells[0, 8].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, ReportBase.HaUnits());
      renderTable.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.PestName;
      renderTable.Cells[1, 1].Text = v6Strings.Strata_SingularName;
      renderTable.Cells[1, 2].Text = renderTable.Cells[1, 4].Text = renderTable.Cells[1, 6].Text = renderTable.Cells[1, 8].Text = i_Tree_Eco_v6.Resources.Strings.Susceptible;
      renderTable.Cells[1, 3].Text = renderTable.Cells[1, 5].Text = renderTable.Cells[1, 7].Text = renderTable.Cells[1, 9].Text = i_Tree_Eco_v6.Resources.Strings.NotSusceptible;
      renderTable.UserCellGroups.Add(new UserCellGroup(new Rectangle(2, 0, 8, 2))
      {
        Style = {
          TextAlignHorz = AlignHorzEnum.Center
        }
      });
      for (int index = 2; index < renderTable.Cols.Count; ++index)
        renderTable.Cols[index].Style.TextAlignHorz = AlignHorzEnum.Right;
      renderTable.Cols[0].Width = (Unit) "14%";
      renderTable.Cols[1].Width = (Unit) "9%";
      renderTable.Cols[0].Style.TextAlignHorz = renderTable.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Left;
      foreach (Pest key4 in validPests)
      {
        SortedList<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail> source = dictionary[key4];
        double num2 = dictionary[key4].Sum<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>((Func<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>, double>) (p => p.Value.SusceptibleLeafArea + p.Value.NotSusceptibleLeafArea));
        renderTable.Cells[num1, 0].Text = ReportBase.m_ps.SpeciesDisplayName == SpeciesDisplayEnum.CommonName ? key4.CommonName : key4.ScientificName;
        foreach (KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail> keyValuePair in source)
        {
          string str1 = source.Keys.Count != 1 || this.curYear.RecordStrata ? this.estUtil.ClassValues[Classifiers.Strata][(short) keyValuePair.Key].Item1 : i_Tree_Eco_v6.Resources.Strings.StudyArea;
          SusceptibilityOfTreesToPestsByStrata.PestStrataDetail pestStrataDetail = keyValuePair.Value;
          renderTable.Cells[num1, 1].Text = str1;
          renderTable.Cells[num1, 2].Text = pestStrataDetail.SusceptibleTreeCount.ToString("N0");
          TableCell cell1 = renderTable.Cells[num1, 3];
          double num3 = pestStrataDetail.NotSusceptibleTreeCount;
          string str2 = num3.ToString("N0");
          cell1.Text = str2;
          TableCell cell2 = renderTable.Cells[num1, 4];
          num3 = pestStrataDetail.SusceptibleStructuralValue;
          string str3 = num3.ToString("N0");
          cell2.Text = str3;
          TableCell cell3 = renderTable.Cells[num1, 5];
          num3 = pestStrataDetail.NotSusceptibleStructuralValue;
          string str4 = num3.ToString("N0");
          cell3.Text = str4;
          TableCell cell4 = renderTable.Cells[num1, 6];
          num3 = pestStrataDetail.SusceptibleLeafArea / num2 * 100.0;
          string str5 = num3.ToString("N1");
          cell4.Text = str5;
          TableCell cell5 = renderTable.Cells[num1, 7];
          num3 = pestStrataDetail.NotSusceptibleLeafArea / num2 * 100.0;
          string str6 = num3.ToString("N1");
          cell5.Text = str6;
          TableCell cell6 = renderTable.Cells[num1, 8];
          num3 = EstimateUtil.ConvertToEnglish(pestStrataDetail.SusceptibleLeafArea, Units.Hectare, ReportBase.EnglishUnits);
          string str7 = num3.ToString("N1");
          cell6.Text = str7;
          TableCell cell7 = renderTable.Cells[num1, 9];
          num3 = EstimateUtil.ConvertToEnglish(pestStrataDetail.NotSusceptibleLeafArea, Units.Hectare, ReportBase.EnglishUnits);
          string str8 = num3.ToString("N1");
          cell7.Text = str8;
          ++num1;
        }
        if (source.Keys.Count > 1)
        {
          renderTable.Rows[num1].Style.FontBold = true;
          renderTable.Cells[num1, 1].Text = i_Tree_Eco_v6.Resources.Strings.Total;
          renderTable.Cells[num1, 2].Text = source.Sum<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>((Func<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>, double>) (p => p.Value.SusceptibleTreeCount)).ToString("N0");
          renderTable.Cells[num1, 3].Text = source.Sum<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>((Func<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>, double>) (p => p.Value.NotSusceptibleTreeCount)).ToString("N0");
          renderTable.Cells[num1, 4].Text = source.Sum<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>((Func<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>, double>) (p => p.Value.SusceptibleStructuralValue)).ToString("N0");
          renderTable.Cells[num1, 5].Text = source.Sum<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>((Func<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>, double>) (p => p.Value.NotSusceptibleStructuralValue)).ToString("N0");
          renderTable.Cells[num1, 6].Text = (source.Sum<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>((Func<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>, double>) (p => p.Value.SusceptibleLeafArea)) / num2 * 100.0).ToString("N0");
          renderTable.Cells[num1, 7].Text = (source.Sum<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>((Func<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>, double>) (p => p.Value.NotSusceptibleLeafArea)) / num2 * 100.0).ToString("N0");
          renderTable.Cells[num1, 8].Text = EstimateUtil.ConvertToEnglish(source.Sum<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>((Func<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>, double>) (p => p.Value.SusceptibleLeafArea)), Units.Hectare, ReportBase.EnglishUnits).ToString("N1");
          renderTable.Cells[num1, 9].Text = EstimateUtil.ConvertToEnglish(source.Sum<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>>((Func<KeyValuePair<int, SusceptibilityOfTreesToPestsByStrata.PestStrataDetail>, double>) (p => p.Value.NotSusceptibleLeafArea)), Units.Hectare, ReportBase.EnglishUnits).ToString("N1");
          num1 = ReportUtil.AddEmptyRow(renderTable, num1);
          ++num1;
        }
      }
      ReportUtil.FormatRenderTable(renderTable);
      int row = ReportUtil.AddEmptyRow(renderTable, num1);
      ReportUtil.AddTableNote(renderTable, row, C1doc.Style.Font.Size);
    }

    private List<Pest> GetValidPests()
    {
      List<string> validPestAbbvs = new List<string>()
      {
        "AL",
        "ALB",
        "BWA",
        "BBD",
        "BC",
        "CB",
        "DA",
        "DED",
        "DFB",
        "DBSR",
        "EAB",
        "FE",
        "FR",
        "GM",
        "GSOB",
        "HWA",
        "JPB",
        "LAT",
        "LWD",
        "MPB",
        "NSE",
        "OW",
        "POCRD",
        "PBSR",
        "PSB",
        "PSHB",
        "SB",
        "SBW",
        "SOD",
        "SPB",
        "SW",
        "TCD",
        "WPB",
        "WPBR",
        "WSB",
        "WM"
      };
      return RetryExecutionHandler.Execute<List<Pest>>((Func<List<Pest>>) (() =>
      {
        using (ISession session = ReportBase.m_ps.LocSp.OpenSession())
        {
          using (ITransaction transaction = session.BeginTransaction())
          {
            List<Pest> list = session.CreateCriteria<Pest>().Add((ICriterion) Restrictions.In("ABB", (ICollection) validPestAbbvs)).AddOrder(new Order("ABB", true)).SetCacheable(true).List<Pest>().ToList<Pest>();
            transaction.Commit();
            return list;
          }
        }
      }));
    }

    public override void SetAlignment(C1PrintDocument C1doc)
    {
      C1doc.Style.FlowAlign = FlowAlignEnum.Center;
      C1doc.Style.TextAlignHorz = AlignHorzEnum.Center;
    }

    private Dictionary<Pest, List<int>> GetSusceptibleSpeciesForAllPests(
      List<Pest> ValidPests)
    {
      Dictionary<Pest, List<int>> speciesForAllPests = new Dictionary<Pest, List<int>>();
      foreach (Pest validPest in ValidPests)
      {
        if (!speciesForAllPests.ContainsKey(validPest))
          speciesForAllPests.Add(validPest, new List<int>());
        foreach (Species susceptableSpecy in (IEnumerable<Species>) this.PestService.GetSusceptableSpecies(validPest.Id))
        {
          int valueOrderFromName = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Species, susceptableSpecy.CommonName);
          if (valueOrderFromName != -1 && !speciesForAllPests[validPest].Contains(valueOrderFromName))
            speciesForAllPests[validPest].Add(valueOrderFromName);
        }
      }
      return speciesForAllPests;
    }

    private SortedList<int, List<int>> GetStrataSpeciesList()
    {
      DataTable speciesByStratum = this.GetSpeciesByStratum();
      SortedList<int, List<int>> strataSpeciesList = new SortedList<int, List<int>>();
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.Strata];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) speciesByStratum.Rows)
      {
        int key = ReportUtil.ConvertFromDBVal<int>(row[classifierName1]);
        int num = ReportUtil.ConvertFromDBVal<int>(row[classifierName2]);
        if (!strataSpeciesList.ContainsKey(key))
          strataSpeciesList.Add(key, new List<int>());
        if (!strataSpeciesList[key].Contains(num))
          strataSpeciesList[key].Add(num);
      }
      return strataSpeciesList;
    }

    private DataTable GetSpeciesByStratum() => this.GetSpeciesByStratumQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<short>("strataTotalCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetParameter<short>("speciesTotalCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetSpeciesByStratumQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetSpeciesByStratum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.Species]);

    private double GetTreeCount(int SpeciesCVO, int StrataCVO)
    {
      if (this._treeCounts.ContainsKey(StrataCVO) && this._treeCounts[StrataCVO].ContainsKey(SpeciesCVO))
        return this._treeCounts[StrataCVO][SpeciesCVO];
      if (!this._treeCounts.ContainsKey(StrataCVO))
        this._treeCounts.Add(StrataCVO, new Dictionary<int, double>());
      double treeCount = this.GetEstimatedValueByStratumQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<int>("c1", StrataCVO).SetParameter<int>("c2", SpeciesCVO).UniqueResult<double>();
      this._treeCounts[StrataCVO].Add(SpeciesCVO, treeCount);
      return treeCount;
    }

    private double GetStructuralValue(int SpeciesCVO, int StrataCVO)
    {
      if (this._structuralValues.ContainsKey(StrataCVO) && this._structuralValues[StrataCVO].ContainsKey(SpeciesCVO))
        return this._structuralValues[StrataCVO][SpeciesCVO];
      double structuralValue = ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatedValueByStratumQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CompensatoryValue]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Monetaryunit, Units.None, Units.None)]).SetParameter<int>("c1", StrataCVO).SetParameter<int>("c2", SpeciesCVO).UniqueResult<double>());
      if (!this._structuralValues.ContainsKey(StrataCVO))
        this._structuralValues.Add(StrataCVO, new Dictionary<int, double>());
      this._structuralValues[StrataCVO].Add(SpeciesCVO, structuralValue);
      return structuralValue;
    }

    private double GetLeafArea(int SpeciesCVO, int StrataCVO)
    {
      if (this._leafAreas.ContainsKey(StrataCVO) && this._leafAreas[StrataCVO].ContainsKey(SpeciesCVO))
        return this._leafAreas[StrataCVO][SpeciesCVO];
      if (!this._leafAreas.ContainsKey(StrataCVO))
        this._leafAreas.Add(StrataCVO, new Dictionary<int, double>());
      double leafArea = ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatedValueByStratumQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)]).SetParameter<int>("c1", StrataCVO).SetParameter<int>("c2", SpeciesCVO).UniqueResult<double>()) * 100.0;
      this._leafAreas[StrataCVO].Add(SpeciesCVO, leafArea);
      return leafArea;
    }

    private IQuery GetEstimatedValueByStratumQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesForClassifier2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.Species]);

    private class PestStrataDetail
    {
      public PestStrataDetail()
      {
        this.SusceptibleTreeCount = 0.0;
        this.NotSusceptibleTreeCount = 0.0;
        this.SusceptibleStructuralValue = 0.0;
        this.NotSusceptibleStructuralValue = 0.0;
        this.SusceptibleLeafArea = 0.0;
        this.NotSusceptibleLeafArea = 0.0;
      }

      public double SusceptibleTreeCount { get; set; }

      public double NotSusceptibleTreeCount { get; set; }

      public double SusceptibleStructuralValue { get; set; }

      public double NotSusceptibleStructuralValue { get; set; }

      public double SusceptibleLeafArea { get; set; }

      public double NotSusceptibleLeafArea { get; set; }
    }
  }
}
