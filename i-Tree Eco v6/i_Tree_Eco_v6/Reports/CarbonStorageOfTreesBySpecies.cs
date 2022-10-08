// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.CarbonStorageOfTreesBySpecies
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
using System.Text;

namespace i_Tree_Eco_v6.Reports
{
  public class CarbonStorageOfTreesBySpecies : DatabaseReport
  {
    public CarbonStorageOfTreesBySpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleCarbonStorageOfTreesBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      SortedList<int, double> data = (SortedList<int, double>) this.GetData();
      double d = Enumerable.Sum(data.Values);
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Insert(0, (RenderObject) renderTable);
      renderTable.Width = Unit.Auto;
      renderTable.ColumnSizingMode = TableSizingModeEnum.Auto;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.RowGroups[0, count].Style.FontBold = true;
      renderTable.RowGroups[0, count].Style.FontSize = 14f;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable.Cells[1, 1].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonneUnits());
      renderTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable.Cells[1, 2].Text = ReportUtil.FormatHeaderUnitsStr("%");
      renderTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.CO2Equivalent;
      renderTable.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonneUnits());
      int num1 = count;
      double num2;
      for (int index = 0; index < data.Count; ++index)
      {
        Tuple<string, string> tuple = this.estUtil.ClassValues[Classifiers.Species][(short) data.Keys[index]];
        renderTable.Cells[num1, 0].Text = ReportBase.ScientificName ? tuple.Item2 : tuple.Item1;
        TableCell cell1 = renderTable.Cells[num1, 1];
        num2 = EstimateUtil.ConvertToEnglish(data.Values[index], Units.MetricTons, ReportBase.EnglishUnits);
        string str1 = num2.ToString("N1");
        cell1.Text = str1;
        TableCell cell2 = renderTable.Cells[num1, 2];
        num2 = data.Values[index] / d;
        string str2 = num2.ToString("P1");
        cell2.Text = str2;
        TableCell cell3 = renderTable.Cells[num1, 3];
        num2 = EstimateUtil.ConvertToEnglish(data.Values[index], Units.MetricTons, ReportBase.EnglishUnits) * 3.667;
        string str3 = num2.ToString("N1");
        cell3.Text = str3;
        ++num1;
      }
      renderTable.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
      TableCell cell4 = renderTable.Cells[num1, 1];
      num2 = EstimateUtil.ConvertToEnglish(d, Units.MetricTons, ReportBase.EnglishUnits);
      string str4 = num2.ToString("N1");
      cell4.Text = str4;
      renderTable.Cells[num1, 2].Text = "100%";
      TableCell cell5 = renderTable.Cells[num1, 3];
      num2 = EstimateUtil.ConvertToEnglish(d, Units.MetricTons, ReportBase.EnglishUnits) * 3.667;
      string str5 = num2.ToString("N1");
      cell5.Text = str5;
      renderTable.Rows[num1].Style.FontBold = true;
      renderTable.Rows[num1].Style.Borders.Top = LineDef.Default;
      ReportUtil.FormatRenderTable(renderTable);
      this.Note(C1doc);
    }

    protected override string ReportMessage()
    {
      StringBuilder stringBuilder = new StringBuilder(i_Tree_Eco_v6.Resources.Strings.NoteCarbonStorageLimit);
      if (this.ProjectIsUsingTropicalEquations())
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(i_Tree_Eco_v6.Resources.Strings.MsgTropicalEquations);
      }
      return stringBuilder.ToString();
    }

    public override object GetData()
    {
      DataTable sequestrationWithTotals = this.GetCarbonSequestrationWithTotals();
      SortedList<int, double> data = new SortedList<int, double>();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) sequestrationWithTotals.Rows)
        data.Add(ReportUtil.ConvertFromDBVal<int>(row[classifierName]), ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]));
      return (object) data;
    }

    private DataTable GetCarbonSequestrationWithTotals() => this.GetEstimateValuesWithSEAndTotals().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CarbonStorage]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimateValuesWithSEAndTotals()
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      return this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSEAndTotals(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Species
      })], classifierName);
    }
  }
}
