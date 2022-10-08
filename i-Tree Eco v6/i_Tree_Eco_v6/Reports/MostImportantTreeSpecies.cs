// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.MostImportantTreeSpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Util;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class MostImportantTreeSpecies : DatabaseReport
  {
    public MostImportantTreeSpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleImportanceValuesBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject> envServicesBySpecies = new Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject>();
      Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject> totalEstimateObject1 = this.AddTreesToTotalEstimateObject(this.GetNumberofTrees(), envServicesBySpecies);
      Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject> totalEstimateObject2 = this.AddLeafAreaToTotalEstimateObject(this.GetLeafArea(), totalEstimateObject1);
      double totalPopulation = totalEstimateObject2.Sum<KeyValuePair<int, MostImportantTreeSpecies.TotalEstimateObject>>((Func<KeyValuePair<int, MostImportantTreeSpecies.TotalEstimateObject>, double>) (p => p.Value.NumTrees));
      double totalLeafArea = totalEstimateObject2.Sum<KeyValuePair<int, MostImportantTreeSpecies.TotalEstimateObject>>((Func<KeyValuePair<int, MostImportantTreeSpecies.TotalEstimateObject>, double>) (p => p.Value.LeafArea));
      List<KeyValuePair<int, MostImportantTreeSpecies.TotalEstimateObject>> list = totalEstimateObject2.ToList<KeyValuePair<int, MostImportantTreeSpecies.TotalEstimateObject>>();
      list.Sort((Comparison<KeyValuePair<int, MostImportantTreeSpecies.TotalEstimateObject>>) ((x, y) => (y.Value.NumTrees / totalPopulation + y.Value.LeafArea / totalLeafArea).CompareTo(x.Value.NumTrees / totalPopulation + x.Value.LeafArea / totalLeafArea)));
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Width = Unit.Auto;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      int count = 1;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.PercentPopulation;
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.PercentLeafArea;
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.ImportanceValue;
      for (int index = 0; index < list.Count; ++index)
      {
        TableCell cell1 = renderTable.Cells[index + 1, 0];
        KeyValuePair<int, MostImportantTreeSpecies.TotalEstimateObject> keyValuePair;
        string str1;
        if (!ReportBase.ScientificName)
        {
          SortedList<short, Tuple<string, string>> classValue = this.estUtil.ClassValues[Classifiers.Species];
          keyValuePair = list[index];
          int key = (int) (short) keyValuePair.Key;
          str1 = classValue[(short) key].Item1;
        }
        else
        {
          SortedList<short, Tuple<string, string>> classValue = this.estUtil.ClassValues[Classifiers.Species];
          keyValuePair = list[index];
          int key = (int) (short) keyValuePair.Key;
          str1 = classValue[(short) key].Item2;
        }
        cell1.Text = str1;
        TableCell cell2 = renderTable.Cells[index + 1, 1];
        keyValuePair = list[index];
        string str2 = (keyValuePair.Value.NumTrees / totalPopulation * 100.0).ToString("N1");
        cell2.Text = str2;
        TableCell cell3 = renderTable.Cells[index + 1, 2];
        keyValuePair = list[index];
        string str3 = (keyValuePair.Value.LeafArea / totalLeafArea * 100.0).ToString("N1");
        cell3.Text = str3;
        TableCell cell4 = renderTable.Cells[index + 1, 3];
        keyValuePair = list[index];
        double num1 = keyValuePair.Value.NumTrees / totalPopulation * 100.0;
        keyValuePair = list[index];
        double num2 = keyValuePair.Value.LeafArea / totalLeafArea * 100.0;
        string str4 = (num1 + num2).ToString("N1");
        cell4.Text = str4;
      }
      ReportUtil.FormatRenderTable(renderTable);
    }

    private Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject> AddTreesToTotalEstimateObject(
      DataTable data,
      Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject> envServicesBySpecies)
    {
      Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject> totalEstimateObject = envServicesBySpecies;
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
        totalEstimateObject.Add(ReportUtil.ConvertFromDBVal<int>(row[classifierName]), new MostImportantTreeSpecies.TotalEstimateObject()
        {
          SpeciesCVO = ReportUtil.ConvertFromDBVal<int>(row[classifierName]),
          NumTrees = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]),
          NumTreesSE = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"])
        });
      return totalEstimateObject;
    }

    private Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject> AddLeafAreaToTotalEstimateObject(
      DataTable data,
      Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject> envServicesBySpecies)
    {
      Dictionary<int, MostImportantTreeSpecies.TotalEstimateObject> totalEstimateObject = envServicesBySpecies;
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        envServicesBySpecies[ReportUtil.ConvertFromDBVal<int>(row[classifierName])].LeafArea = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        envServicesBySpecies[ReportUtil.ConvertFromDBVal<int>(row[classifierName])].LeafAreaSE = ReportUtil.ConvertFromDBVal<double>(row["EstimateStandardError"]);
      }
      return totalEstimateObject;
    }

    private DataTable GetNumberofTrees() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private DataTable GetLeafArea() => this.GetEstimatesQuery().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSE(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    private class TotalEstimateObject
    {
      public int SpeciesCVO { get; set; }

      public double NumTrees { get; set; }

      public double NumTreesSE { get; set; }

      public double CarbonStorage { get; set; }

      public double CarbonStorageSE { get; set; }

      public double GrossCarbonSeq { get; set; }

      public double GrossCarbonSeqSE { get; set; }

      public double NetCarbonSeq { get; set; }

      public double NetCarbonSeqSE { get; set; }

      public double LeafArea { get; set; }

      public double LeafAreaSE { get; set; }

      public double LeafBiomass { get; set; }

      public double LeafBiomassSE { get; set; }

      public double CompensatoryValue { get; set; }

      public double CompensatoryValueSE { get; set; }
    }
  }
}
