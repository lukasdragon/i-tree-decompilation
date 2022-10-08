// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.LeafNutrientsOfTreesBySpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using DaveyTree.NHibernate;
using Eco.Util;
using Eco.Util.Views;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class LeafNutrientsOfTreesBySpecies : DatabaseReport
  {
    public LeafNutrientsOfTreesBySpecies() => this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleLeafNutrientsOfTreesBySpecies;

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      DataTable data = (DataTable) this.GetData();
      RenderTable renderTable = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable);
      renderTable.Width = (Unit) "100%";
      renderTable.Cols[0].Width = (Unit) "20%";
      renderTable.Rows[0].Style.FontSize = 14f;
      int count = 2;
      renderTable.RowGroups[0, count].Header = TableHeaderEnum.Page;
      renderTable.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
      renderTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      renderTable.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
      renderTable.Cols[1].Style.Borders.Left = renderTable.Cols[6].Style.Borders.Left = renderTable.Cols[2].Style.Borders.Right = this.tableBorderLineGray;
      renderTable.Cells[0, 1].SpanCols = 2;
      renderTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.LeafBiomass;
      renderTable.Cells[1, 1].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonneUnits());
      renderTable.Cells[1, 2].Text = i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr;
      renderTable.Cells[0, 3].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Nitrogen, i_Tree_Eco_v6.Resources.Strings.NitrogenFormula);
      renderTable.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.KgUnits());
      renderTable.Cells[0, 4].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Phosphorus, i_Tree_Eco_v6.Resources.Strings.PhosphorusFormula);
      renderTable.Cells[1, 4].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.KgUnits());
      renderTable.Cells[0, 5].Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Potassium, i_Tree_Eco_v6.Resources.Strings.PotassiumFormula);
      renderTable.Cells[1, 5].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.KgUnits());
      renderTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.Value;
      renderTable.Cells[1, 6].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
      List<ColumnFormat> columnFormatList = this.ColumnsFormat(data);
      int row1 = count;
      ReportUtil.AddAlternateStyle(renderTable);
      foreach (DataRow row2 in (InternalDataCollectionBase) data.Rows)
      {
        foreach (ColumnFormat columnFormat in columnFormatList)
          renderTable.Cells[row1, columnFormat.ColNum].Text = columnFormat.Format(row2[columnFormat.ColName]);
        ++row1;
      }
      ReportUtil.FormatRenderTable(renderTable);
    }

    public override object GetData()
    {
      SortedList<int, Tuple<double, double>> leafBiomassData = this.GetLeafBiomassData();
      Dictionary<string, SpeciesView> species = ReportBase.m_ps.Species;
      IDictionary<Nutrient, double> nutrientValues = this.GetNutrientValues(this.project.LocationId);
      IDictionary<Nutrient, double> leafNutrients = this.GetLeafNutrients();
      DataTable dataTable = new DataTable();
      string columnName1 = "Species";
      dataTable.Columns.Add(columnName1, typeof (string));
      string columnName2 = "LeafBiomass";
      dataTable.Columns.Add(columnName2, typeof (double));
      string columnName3 = "LeafBiomassSE";
      dataTable.Columns.Add(columnName3, typeof (double));
      string columnName4 = "Nitrogen";
      dataTable.Columns.Add(columnName4, typeof (double));
      string columnName5 = "Phosphorus";
      dataTable.Columns.Add(columnName5, typeof (double));
      string columnName6 = "Potassium";
      dataTable.Columns.Add(columnName6, typeof (double));
      string columnName7 = "Value";
      dataTable.Columns.Add(columnName7, typeof (double));
      foreach (KeyValuePair<int, Tuple<double, double>> keyValuePair in leafBiomassData)
      {
        short key = (short) keyValuePair.Key;
        SpeciesView speciesView = (SpeciesView) null;
        if (species.TryGetValue(this.estUtil.SpeciesValues[key].Item3, out speciesView) && speciesView.Species.LeafType.TopClassification.Id == 1)
        {
          DataRow dataRow = dataTable.Rows.Add();
          dataRow[columnName1] = ReportBase.ScientificName ? (object) this.estUtil.SpeciesValues[key].Item2 : (object) this.estUtil.SpeciesValues[key].Item1;
          Tuple<double, double> tuple = keyValuePair.Value;
          double num1 = tuple.Item1;
          dataRow[columnName2] = (object) num1;
          double num2 = tuple.Item2;
          dataRow[columnName3] = (object) num2;
          double val1 = this.AdjustPercent(num1, leafNutrients[Nutrient.Nitrogen]);
          double val2 = this.AdjustPercent(num1, leafNutrients[Nutrient.Phosphorus]);
          double val3 = this.AdjustPercent(num1, leafNutrients[Nutrient.Potassium]);
          dataRow[columnName4] = (object) UnitsHelper.TonneToKg(val1);
          dataRow[columnName5] = (object) UnitsHelper.TonneToKg(val2);
          dataRow[columnName6] = (object) UnitsHelper.TonneToKg(val3);
          dataRow[columnName7] = (object) ReportBase.FormatDoubleValue1((object) this.AdjustCurrency(val1 * nutrientValues[Nutrient.Nitrogen] + val2 * nutrientValues[Nutrient.Phosphorus] + val3 * nutrientValues[Nutrient.Potassium]));
        }
      }
      DataView defaultView = dataTable.DefaultView;
      defaultView.Sort = string.Format("{0} DESC", (object) columnName2);
      return (object) defaultView.ToTable();
    }

    protected SortedList<int, Tuple<double, double>> GetLeafBiomassData()
    {
      List<Classifiers> source = new List<Classifiers>()
      {
        Classifiers.Species
      };
      IList<(int, double, double)> tupleList = this.estUtil.queryProvider.GetEstimateUtilProvider().GetMultipleFieldsData2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, source)], source.Select<Classifiers, string>((Func<Classifiers, string>) (c => this.estUtil.ClassifierNames[c])).ToArray<string>()).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafBiomass]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double, double)>()).List<(int, double, double)>();
      SortedList<int, Tuple<double, double>> leafBiomassData = new SortedList<int, Tuple<double, double>>();
      foreach ((int key, double num1, double num2) in (IEnumerable<(int, double, double)>) tupleList)
        leafBiomassData.Add(key, Tuple.Create<double, double>(num1, num2));
      return leafBiomassData;
    }

    private IDictionary<Nutrient, double> GetLeafNutrients() => RetryExecutionHandler.Execute<IDictionary<Nutrient, double>>((Func<IDictionary<Nutrient, double>>) (() =>
    {
      using (ISession session = ReportBase.m_ps.LocSp.OpenSession())
      {
        using (ITransaction transaction = session.BeginTransaction())
        {
          Dictionary<Nutrient, double> dictionary = this.locSpSession.QueryOver<LeafNutrient>().List<LeafNutrient>().ToDictionary<LeafNutrient, Nutrient, double>((Func<LeafNutrient, Nutrient>) (ln => ln.Nutrient), (Func<LeafNutrient, double>) (ln => ln.Percent));
          transaction.Commit();
          return (IDictionary<Nutrient, double>) dictionary;
        }
      }
    }));

    protected override string ReportMessage() => i_Tree_Eco_v6.Resources.Strings.NoteLeafNutrients;

    private IDictionary<Nutrient, double> GetNutrientValues(int locationId)
    {
      IList<LocationLeafNutrientValue> leafNutrientValues = this.LocationService.GetLeafNutrientValues(locationId);
      if (leafNutrientValues.Count == 0)
        leafNutrientValues = this.LocationService.GetLeafNutrientValues(this.GetDefaultLocationID());
      return (IDictionary<Nutrient, double>) leafNutrientValues.ToDictionary<LocationLeafNutrientValue, Nutrient, double>((Func<LocationLeafNutrientValue, Nutrient>) (nv => nv.LeafNutrient.Nutrient), (Func<LocationLeafNutrientValue, double>) (nv => nv.NutrientValue));
    }

    public double AdjustPercent(double value, double percent) => value * percent / 100.0;

    public int GetDefaultLocationID() => NationFeatures.defaultLocation.LocId;

    public override List<ColumnFormat> ColumnsFormat(DataTable data) => new List<ColumnFormat>()
    {
      new ColumnFormat()
      {
        HeaderText = i_Tree_Eco_v6.Resources.Strings.Species,
        ColName = data.Columns[0].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatStr),
        ColNum = 0
      },
      new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafBiomass, ReportBase.TonnesUnits()),
        ColName = data.Columns[1].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatTonne1),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatTonne1),
        ColNum = 1
      },
      new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafBiomass, i_Tree_Eco_v6.Resources.Strings.StandardErrorAbbr),
        ColName = data.Columns[2].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatTonneSE1),
        ColNum = 2
      },
      new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Nitrogen, ReportBase.KgUnits()),
        ColName = data.Columns[3].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        ColNum = 3
      },
      new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Phosphorus, ReportBase.KgUnits()),
        ColName = data.Columns[4].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        ColNum = 4
      },
      new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Potassium, ReportBase.KgUnits()),
        ColName = data.Columns[5].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleWeight),
        ColNum = 5
      },
      new ColumnFormat()
      {
        HeaderText = ReportUtil.FormatInlineCSVHeaderWUnitsStr(i_Tree_Eco_v6.Resources.Strings.Value, this.CurrencySymbol),
        ColName = data.Columns[6].ColumnName,
        Format = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue2),
        FormatTotals = new ColumnFormat.ObjectFormatter(ReportBase.FormatDoubleValue0),
        ColNum = 6
      }
    };
  }
}
