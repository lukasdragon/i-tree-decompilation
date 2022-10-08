// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.HydrologyEffects
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
  public class HydrologyEffects : DatabaseReport
  {
    protected Classifiers classifier;
    protected string unifier;
    protected int headerRows = 2;
    protected Dictionary<int, HydrologyEffects.TotalRunoffObject> avoidedRunoff;
    protected List<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>> outList;

    protected void BodyStylinig(C1PrintDocument C1doc, RenderTable rTable)
    {
      ReportUtil.SetDefaultReportFormatting(C1doc, rTable, this.curYear, this.headerRows);
      rTable.Rows[0].SizingMode = TableSizingModeEnum.Auto;
      rTable.Cols[0].Style.TextAlignHorz = AlignHorzEnum.Left;
      rTable.Cols[3].Style.Borders.Right = rTable.Cols[6].Style.Borders.Right = new LineDef((Unit) "1pt", Color.Gray);
      ReportUtil.FormatRenderTable(rTable);
    }

    private double GetLeafArea(double val) => val * (ReportBase.EnglishUnits ? 247.105 : 100.0);

    protected void PopulateHydrologyRow(
      RenderTable rTable,
      int tableRow,
      HydrologyEffects.TotalRunoffObject values)
    {
      rTable.Cells[tableRow, 1].Text = string.Format("{0:N0}", (object) values.NumTrees);
      rTable.Cells[tableRow, 2].Text = ReportBase.FormatDoubleValue2((object) this.GetLeafArea(values.LeafArea));
      rTable.Cells[tableRow, 3].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) values.PotentialEvapotranspiration);
      rTable.Cells[tableRow, 4].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) values.Evaporation);
      rTable.Cells[tableRow, 5].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) values.Transpiration);
      rTable.Cells[tableRow, 6].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) values.WaterInterception);
      rTable.Cells[tableRow, 7].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) values.AvoidedRunoff);
      rTable.Cells[tableRow, 8].Text = ReportBase.FormatDoubleValueZero2((object) values.AvoidedRunoffValue);
    }

    protected void Header(RenderTable rTable)
    {
      rTable.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
      rTable.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.LeafArea;
      rTable.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.PotentialEvapotranspiration;
      rTable.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.Evaporation;
      rTable.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.Transpiration;
      rTable.Cells[0, 6].Text = i_Tree_Eco_v6.Resources.Strings.WaterIntercepted;
      rTable.Cells[0, 7].Text = i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff;
      rTable.Cells[0, 8].Text = i_Tree_Eco_v6.Resources.Strings.AvoidedRunoffValue;
      rTable.Cells[1, 2].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.HaUnits());
      rTable.Cells[1, 3].Text = rTable.Cells[1, 4].Text = rTable.Cells[1, 5].Text = rTable.Cells[1, 6].Text = rTable.Cells[1, 7].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.CubicMeterUnits());
      rTable.Cells[1, 8].Text = ReportUtil.GetFormattedValuePerYrStr(this.CurrencySymbol);
    }

    protected void Totals(RenderTable rTable)
    {
      int num = this.outList.Count + this.headerRows;
      rTable.Rows[num].Style.Borders.Top = LineDef.Default;
      rTable.Cells[num, 1].Text = this.outList.Sum<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>((Func<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>, double>) (p => p.Value.NumTrees)).ToString("N0");
      rTable.Cells[num, 2].Text = string.Format("{0:N2}", (object) ReportBase.FormatDoubleValue2((object) this.GetLeafArea(this.outList.Sum<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>((Func<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>, double>) (p => p.Value.LeafArea)))));
      rTable.Cells[num, 3].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) this.outList.Sum<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>((Func<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>, double>) (p => p.Value.PotentialEvapotranspiration)));
      rTable.Cells[num, 4].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) this.outList.Sum<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>((Func<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>, double>) (p => p.Value.Evaporation)));
      rTable.Cells[num, 5].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) this.outList.Sum<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>((Func<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>, double>) (p => p.Value.Transpiration)));
      rTable.Cells[num, 6].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) this.outList.Sum<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>((Func<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>, double>) (p => p.Value.WaterInterception)));
      rTable.Cells[num, 7].Text = ReportBase.FormatDoubleVolumeCubicMeterZero2((object) this.outList.Sum<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>((Func<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>, double>) (p => p.Value.AvoidedRunoff)));
      rTable.Cells[num, 8].Text = ReportBase.FormatDoubleValueZero2((object) this.outList.Sum<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>>((Func<KeyValuePair<int, HydrologyEffects.TotalRunoffObject>, double>) (p => p.Value.AvoidedRunoffValue)));
    }

    protected override string ReportMessage() => this.AvoidedRunoff_m3_ft3_Footer();

    protected void GetData()
    {
      if (this.avoidedRunoff != null)
        return;
      this.avoidedRunoff = new Dictionary<int, HydrologyEffects.TotalRunoffObject>();
      DataTable hydrologyEffects = this.GetHydrologyEffects(this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees], this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]);
      string classifierName = this.estUtil.ClassifierNames[this.classifier];
      string columnName = "EstimateValue";
      foreach (DataRow row in (InternalDataCollectionBase) hydrologyEffects.Rows)
        this.avoidedRunoff.Add(ReportUtil.ConvertFromDBVal<int>(row[classifierName]), new HydrologyEffects.TotalRunoffObject()
        {
          CVO = ReportUtil.ConvertFromDBVal<int>(row[classifierName]),
          NumTrees = ReportUtil.ConvertFromDBVal<double>(row[columnName])
        });
      foreach (DataRow row in (InternalDataCollectionBase) this.GetHydrologyEffects(this.estUtil.EstTypes[EstimateTypeEnum.LeafArea], this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)]).Rows)
        this.avoidedRunoff[ReportUtil.ConvertFromDBVal<int>(row[classifierName])].LeafArea = ReportUtil.ConvertFromDBVal<double>(row[columnName]);
      foreach (DataRow row in (InternalDataCollectionBase) this.GetHydrologyEffects(this.estUtil.EstTypes[EstimateTypeEnum.AvoidedRunoff], this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.Year)]).Rows)
      {
        this.avoidedRunoff[ReportUtil.ConvertFromDBVal<int>(row[classifierName])].AvoidedRunoff = ReportUtil.ConvertFromDBVal<double>(row[columnName]);
        double num = ReportUtil.ConvertFromDBVal<double>(row[columnName]);
        this.avoidedRunoff[ReportUtil.ConvertFromDBVal<int>(row[classifierName])].AvoidedRunoffValue = num * this.customizedWaterDollarsPerM3;
      }
      foreach (EstimateTypeEnum estType in new List<EstimateTypeEnum>()
      {
        EstimateTypeEnum.PotentialEvapotranspiration,
        EstimateTypeEnum.Evaporation,
        EstimateTypeEnum.Transpiration,
        EstimateTypeEnum.WaterInterception
      })
      {
        int estUnit = this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.Year)];
        foreach (DataRow row in (InternalDataCollectionBase) this.GetHydrologyEffects((int) estType, estUnit).Rows)
        {
          int key = ReportUtil.ConvertFromDBVal<int>(row[classifierName]);
          double num = ReportUtil.ConvertFromDBVal<double>(row[columnName]);
          this.avoidedRunoff[key][estType.ToString()] = (object) num;
        }
      }
    }

    public DataTable GetHydrologyEffects(int estType, int estUnits) => this.GetEstimatesQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>(nameof (estType), estType).SetParameter<int>(nameof (estUnits), estUnits).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(this.classifier, this.unifier)).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesWithSE(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      this.classifier
    })], this.estUtil.ClassifierNames[this.classifier]);

    protected class TotalRunoffObject
    {
      public object this[string propertyName]
      {
        get => this.GetType().GetProperty(propertyName).GetValue((object) this, (object[]) null);
        set => this.GetType().GetProperty(propertyName).SetValue((object) this, value, (object[]) null);
      }

      public int CVO { get; set; }

      public double NumTrees { get; set; }

      public double LeafArea { get; set; }

      public double PotentialEvapotranspiration { get; set; }

      public double Evaporation { get; set; }

      public double Transpiration { get; set; }

      public double WaterInterception { get; set; }

      public double AvoidedRunoff { get; set; }

      public double AvoidedRunoffValue { get; set; }
    }
  }
}
