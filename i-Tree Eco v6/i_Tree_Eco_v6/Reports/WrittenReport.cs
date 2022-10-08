// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.WrittenReport
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using C1.Win.C1Chart;
using DaveyTree.NHibernate;
using Eco.Domain.Properties;
using Eco.Domain.v6;
using Eco.Util;
using Eco.Util.Views;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  public class WrittenReport : DatabaseReport
  {
    private ResourceManager resmanPestReferences;
    private ResourceManager resmanStateInvasives;
    private string[] _pests = WR_Pest.Pests;
    private C1.C1Preview.Style styleDefaultParagraph;
    private C1.C1Preview.Style styleList;
    private C1.C1Preview.Style styleBullet;
    private C1.C1Preview.Style styleSectionTitle;
    private C1.C1Preview.Style styleTableTitle;
    private C1.C1Preview.Style styleTinyText;
    private C1.C1Preview.Style styleIndentedTinyText;
    private C1.C1Preview.Style styleTocItem;
    private RenderText rText;
    private RenderParagraph rParagraph;
    private ParagraphText pText;
    private RenderGraphics rg;
    private RenderArea ra;
    private Graphics formGraphics;
    private Rectangle pageSizePix;
    private RenderToc toc;
    private string fourHundredKm;
    private string fourHundredKmTwelveHundredKm;
    private string twelveHundredKm;

    public WrittenReport()
    {
      this.fourHundredKm = ReportBase.EnglishUnits ? string.Format("250 {0}", (object) i_Tree_Eco_v6.Resources.Strings.UnitMiles) : string.Format("400 {0}", (object) i_Tree_Eco_v6.Resources.Strings.UnitKilometers);
      this.fourHundredKmTwelveHundredKm = ReportBase.EnglishUnits ? string.Format("250 {0} 750 {1}", (object) i_Tree_Eco_v6.Resources.Strings.And, (object) i_Tree_Eco_v6.Resources.Strings.UnitMiles) : string.Format("400 {0} 1210 {1}", (object) i_Tree_Eco_v6.Resources.Strings.And, (object) i_Tree_Eco_v6.Resources.Strings.UnitKilometers);
      this.twelveHundredKm = ReportBase.EnglishUnits ? string.Format("750 {0}", (object) i_Tree_Eco_v6.Resources.Strings.UnitMiles) : string.Format("1210 {0}", (object) i_Tree_Eco_v6.Resources.Strings.UnitKilometers);
    }

    public override void InitDocument(C1PrintDocument C1doc)
    {
      base.InitDocument(C1doc);
      this.ra = new RenderArea();
      this.toc = new RenderToc();
      C1doc.Style.Font = new Font("Calibri", 11f);
      C1doc.Style.Spacing.Bottom = new Unit(1.0, UnitTypeEnum.LineSpacing);
      C1doc.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Justify;
      this.styleDefaultParagraph = C1doc.Style.Children.Add();
      this.styleDefaultParagraph.Spacing.Top = (Unit) "1ls";
      this.styleDefaultParagraph.Spacing.Bottom = (Unit) 0;
      this.styleDefaultParagraph.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Justify;
      this.styleBullet = C1doc.Style.Children.Add();
      this.styleBullet.Spacing.Left = (Unit) ".6cm";
      this.styleBullet.Spacing.Top = (Unit) 0;
      this.styleBullet.Spacing.Bottom = (Unit) 0;
      this.styleList = C1doc.Style.Children.Add();
      this.styleList.Spacing.Left = (Unit) "1.2cm";
      this.styleList.Spacing.Bottom = (Unit) 0;
      this.styleSectionTitle = C1doc.Style.Children.Add();
      this.styleSectionTitle.FontSize = 15f;
      this.styleSectionTitle.FontBold = true;
      this.styleSectionTitle.Spacing.Bottom = (Unit) 0;
      this.styleTableTitle = C1doc.Style.Children.Add();
      this.styleTableTitle.Spacing.Top = (Unit) "1ls";
      this.styleTableTitle.Spacing.Bottom = (Unit) 0;
      this.styleTableTitle.FontBold = true;
      this.styleTinyText = C1doc.Style.Children.Add();
      this.styleTinyText.FontSize = 8f;
      this.styleIndentedTinyText = C1doc.Style.Children.Add();
      this.styleIndentedTinyText.AmbientParent = this.styleTinyText;
      this.styleIndentedTinyText.Parent = this.styleBullet;
      this.styleIndentedTinyText.Spacing.Bottom = (Unit) 0;
      this.toc.Style.Spacing.Top = (Unit) "1ls";
      this.styleTocItem = C1doc.Style.Children.Add();
      this.styleTocItem.Spacing.Bottom = (Unit) 0;
      this.styleTocItem.Spacing.Left = (Unit) ".6cm";
    }

    public override void RenderBody(C1PrintDocument C1doc, Graphics g)
    {
      this.formGraphics = g;
      this.pageSizePix = this.GetPageRectanglePixels(C1doc, g);
      this.CreateWrittenReport(C1doc);
    }

    public override void RenderHeader(C1PrintDocument C1doc)
    {
    }

    public List<PestAffected> GetPestAffectedAllPests(
      ISession LocSpSession,
      Dictionary<int, double> SpeciesTreeCount,
      Dictionary<int, double> speciesReplacementValues)
    {
      SortedDictionary<string, PestAffected> sortedDictionary = new SortedDictionary<string, PestAffected>();
      foreach (Pest pest in (IEnumerable<Pest>) LocSpSession.CreateCriteria<Pest>().List<Pest>())
      {
        if (((IEnumerable<string>) this._pests).Contains<string>(pest.ABB))
        {
          if (!sortedDictionary.ContainsKey(pest.ABB))
            sortedDictionary.Add(pest.ABB, new PestAffected()
            {
              Abb = pest.ABB.ToUpper(),
              CommonName = pest.CommonName,
              ScientificName = pest.ScientificName
            });
          foreach (Species susceptableSpecy in (IEnumerable<Species>) pest.SusceptableSpecies)
          {
            int valueOrderFromName = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Species, susceptableSpecy.CommonName);
            if (valueOrderFromName != -1 && SpeciesTreeCount.ContainsKey(valueOrderFromName))
            {
              sortedDictionary[pest.ABB].PopulationAffected += SpeciesTreeCount[valueOrderFromName];
              sortedDictionary[pest.ABB].ReplacementValue += speciesReplacementValues[valueOrderFromName];
            }
          }
        }
      }
      return sortedDictionary.Values.ToList<PestAffected>();
    }

    public List<PestAffected> GetPestAffectedUSA(
      Location County,
      Dictionary<int, double> SpeciesTreeCount,
      Dictionary<int, double> speciesStructuralValues)
    {
      List<PestAffected> pestAffectedList = new List<PestAffected>();
      SortedDictionary<string, PestAffected> sortedDictionary = new SortedDictionary<string, PestAffected>();
      foreach (LocationPestRisk locationPestRisk in this.LocationService.GetPestRisks(County.Id).Where<LocationPestRisk>((Func<LocationPestRisk, bool>) (p => p.Risk.Level == "Red")).Where<LocationPestRisk>((Func<LocationPestRisk, bool>) (p => ((IEnumerable<string>) this._pests).Contains<string>(p.Pest.ABB))).OrderBy<LocationPestRisk, string>((Func<LocationPestRisk, string>) (p => p.Pest.CommonName)).ToList<LocationPestRisk>())
      {
        if (!sortedDictionary.ContainsKey(locationPestRisk.Pest.ABB))
          sortedDictionary.Add(locationPestRisk.Pest.ABB, new PestAffected()
          {
            Abb = locationPestRisk.Pest.ABB.ToUpper()
          });
        foreach (Species susceptableSpecy in (IEnumerable<Species>) locationPestRisk.Pest.SusceptableSpecies)
        {
          int valueOrderFromName = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Species, susceptableSpecy.CommonName);
          if (valueOrderFromName != -1 && SpeciesTreeCount.ContainsKey(valueOrderFromName))
          {
            sortedDictionary[locationPestRisk.Pest.ABB].PopulationAffected += SpeciesTreeCount[valueOrderFromName];
            sortedDictionary[locationPestRisk.Pest.ABB].ReplacementValue += speciesStructuralValues[valueOrderFromName];
          }
        }
      }
      return sortedDictionary.Values.ToList<PestAffected>();
    }

    public Dictionary<int, double> GetStructuralValues()
    {
      Dictionary<int, double> structuralValues = new Dictionary<int, double>();
      foreach ((int key, double num) in (IEnumerable<(int, double)>) this.GetSpeciesEstimatesQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CompensatoryValue]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Monetaryunit, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double)>()).List<(int, double)>())
        structuralValues[key] = num;
      return structuralValues;
    }

    private IQuery GetEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesForClassifier(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    private IQuery GetSpeciesEstimatesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    public List<SpeciesOxygenProduction> GetTop20OxygenProductionSpecies()
    {
      List<SpeciesOxygenProduction> productionSpecies = new List<SpeciesOxygenProduction>();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) this.GetOxygenProduction().Rows)
      {
        short num1 = ReportUtil.ConvertFromDBVal<short>(row[classifierName]);
        double num2 = ReportUtil.ConvertFromDBVal<double>(row["EstimateValue"]);
        productionSpecies.Add(new SpeciesOxygenProduction()
        {
          SppName = this.GetSpeciesName(num1),
          O2 = num2 * 32.0 / 12.0,
          NetCarbonSequestration = num2,
          NumberOfTrees = this.GetTreeCount(num1),
          LeafArea = this.GetLeafArea(num1)
        });
      }
      return productionSpecies;
    }

    public DataTable GetOxygenProduction() => this.GetSpeciesEstimatesQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.series.SampleType == SampleType.Inventory ? this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration] : this.estUtil.EstTypes[EstimateTypeEnum.NetCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>().AsEnumerable().Take<DataRow>(20).CopyToDataTable<DataRow>();

    public double GetTreeCount(short species) => ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatesQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<short>("classifier", species).UniqueResult<double>());

    public double GetLeafArea(short species) => ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatesQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)]).SetParameter<short>("classifier", species).UniqueResult<double>());

    public double GetTotalOxygenProduction() => ReportUtil.ConvertFromDBVal<double>((object) this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValueSum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.series.SampleType == SampleType.Inventory ? this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration] : this.estUtil.EstTypes[EstimateTypeEnum.NetCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).UniqueResult<double>()) * 32.0 / 12.0;

    private C1.Win.C1Chart.C1Chart CreateChartTreeSpeciesComposition(
      List<Tuple<int, double, string>> SpeciesByCountDesc,
      double TreeCount,
      string LocationName)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetPieChartStyle(chart);
      string format = "{0} ({1:N1}%)";
      double num1 = 1.0;
      for (int index = 0; index < 10 && index < SpeciesByCountDesc.Count; ++index)
      {
        ChartDataSeries ser = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        ser.LineStyle.Color = ReportUtil.GetColor(index);
        ser.X.Add((object) 0);
        double num2 = SpeciesByCountDesc[index].Item2 / TreeCount;
        num1 -= num2;
        ser.Y.Add((object) num2);
        Label label = chart.ChartLabels.LabelsCollection.AddNewLabel();
        label.Text = string.Format(format, (object) SpeciesByCountDesc[index].Item3, (object) (num2 * 100.0));
        label.AttachMethod = AttachMethodEnum.DataIndex;
        label.Visible = true;
        label.Offset = 50;
        label.Connected = true;
        label.Compass = LabelCompassEnum.Radial;
        AttachMethodData attachMethodData = label.AttachMethodData;
        attachMethodData.GroupIndex = 0;
        attachMethodData.SeriesIndex = chart.ChartGroups.Group0.ChartData.SeriesList.IndexOf(ser);
        attachMethodData.PointIndex = 0;
      }
      if (num1 > 0.0)
      {
        ChartDataSeries ser = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
        ser.X.Add((object) 0);
        ser.Y.Add((object) num1);
        ser.Label = i_Tree_Eco_v6.Resources.Strings.Other;
        Label label = chart.ChartLabels.LabelsCollection.AddNewLabel();
        label.Text = string.Format(format, (object) i_Tree_Eco_v6.Resources.Strings.Other, (object) (num1 * 100.0));
        label.AttachMethod = AttachMethodEnum.DataIndex;
        label.Connected = true;
        label.AttachMethodData.GroupIndex = 0;
        label.AttachMethodData.SeriesIndex = chart.ChartGroups.Group0.ChartData.SeriesList.IndexOf(ser);
        label.AttachMethodData.PointIndex = 0;
        label.Offset = 50;
        label.Compass = LabelCompassEnum.Radial;
        label.Visible = true;
      }
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure1, (object) LocationName);
      return chart;
    }

    private C1.Win.C1Chart.C1Chart CreateChartTreesPerAreaByLandUse(
      bool EnglishUnits,
      string LocationName)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetBarChartStyle(chart);
      chart.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      chart.ChartArea.AxisY.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.FmtValuePerValue, (object) i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV, (object) ReportBase.HaUnits());
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_PerAreaFigure2, (object) ReportBase.HaUnits(), (object) LocationName);
      ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      DataTable stratumValuesData = this.GetEstimatedStratumValuesData();
      int val = 0;
      foreach (DataRow row in (InternalDataCollectionBase) stratumValuesData.Rows)
      {
        string text = this.estUtil.ClassValues[Classifiers.Strata][(short) (int) row[classifierName]].Item1;
        if (val != 1 || !(text == "Study Area"))
        {
          chartDataSeries.X.Add((object) val);
          chartDataSeries.Y.Add((object) EstimateUtil.ConvertToEnglish((double) row["EstimateValue"], Tuple.Create<Units, Units, Units>(Units.Count, Units.Hectare, Units.None), EnglishUnits));
          chart.ChartArea.AxisX.ValueLabels.Add((object) val, text);
          ++val;
        }
      }
      if (chart.ChartArea.AxisX.ValueLabels.Count == 1 && !this.curYear.RecordStrata)
        chart.ChartArea.AxisX.ValueLabels[0].Text = i_Tree_Eco_v6.Resources.Strings.StudyArea;
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(1);
      return chart;
    }

    private DataTable GetEstimatedStratumValuesData() => this.GetEstimatedStratumValues().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.Hectare, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetEstimatedStratumValues() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedStratumValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private C1.Win.C1Chart.C1Chart CreateChartTreesByLandUse(
      bool EnglishUnits,
      string LocationName)
    {
      List<double> dataList = new List<double>();
      List<string> stringList = new List<string>();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      DataTable countsPerStratum = this.GetTreeCountsPerStratum();
      int num = 0;
      foreach (DataRow row in (InternalDataCollectionBase) countsPerStratum.Rows)
      {
        string str = this.estUtil.ClassValues[Classifiers.Strata][(short) (int) row[classifierName]].Item1;
        if (num != 1 || !(str == "Study Area"))
        {
          dataList.Add((double) row["EstimateValue"]);
          stringList.Add(str);
          ++num;
        }
      }
      if (stringList.Count == 1 && !this.curYear.RecordStrata)
        stringList[0] = i_Tree_Eco_v6.Resources.Strings.StudyArea;
      DataScaler dataScaler = new DataScaler(dataList, Units.Count, Units.None);
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetBarChartStyle(chart);
      chart.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      chart.ChartArea.AxisY.Text = !string.IsNullOrEmpty(dataScaler.GetScaledPrefix(EnglishUnits)) ? ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV, dataScaler.GetScaledPrefix(EnglishUnits)) : i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure2, (object) LocationName);
      ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      for (int index = 0; index < stringList.Count; ++index)
      {
        chartDataSeries.X.Add((object) index);
        chartDataSeries.Y.Add((object) dataScaler.GetScaledValue(dataList[index], EnglishUnits));
        chart.ChartArea.AxisX.ValueLabels.Add((object) index, stringList[index]);
        ++num;
      }
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(1);
      return chart;
    }

    private DataTable GetTreeCountsPerStratum() => this.GetEstimatedStratumValues().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private C1.Win.C1Chart.C1Chart CreateChartPercentTreePopulationByDBHClass(
      SortedList<int, double> DbhTreeList,
      double TreeCount,
      bool EnglishUnits)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetBarChartStyle(chart);
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure3, EnglishUnits ? (object) i_Tree_Eco_v6.Resources.Strings.DBHDefinitionEnglish : (object) i_Tree_Eco_v6.Resources.Strings.DBHDefinitionMetric);
      chart.ChartArea.AxisX.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.DBHClass, ReportBase.CmUnits());
      chart.ChartArea.AxisY.Text = i_Tree_Eco_v6.Resources.Strings.Percent;
      SortedList<short, string> english = this.estUtil.ConvertDBHRangesToEnglish(EnglishUnits);
      SortedList<int, double> sortedList = new SortedList<int, double>();
      int num1 = 0;
      int key = 0;
      foreach (KeyValuePair<int, double> dbhTree in DbhTreeList)
      {
        if (num1 < 10)
        {
          sortedList.Add(dbhTree.Key, dbhTree.Value);
          key = dbhTree.Key;
          ++num1;
        }
        else
          sortedList[key] += dbhTree.Value;
      }
      ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(3);
      short num2 = 0;
      foreach (KeyValuePair<int, double> keyValuePair in sortedList)
      {
        chartDataSeries.X.Add((object) keyValuePair.Key);
        chartDataSeries.Y.Add((object) (100.0 * keyValuePair.Value / TreeCount));
        chart.ChartArea.AxisX.ValueLabels.Add((object) keyValuePair.Key, english[(short) keyValuePair.Key]);
        ++num2;
      }
      return chart;
    }

    private Tuple<C1.Win.C1Chart.C1Chart, bool> CreateChartPercentOfLiveTreePopulationByAreaOfNativeOrigin(
      string LocationName,
      Location State)
    {
      bool flag = false;
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetBarChartStyle(chart);
      chart.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Origin;
      chart.ChartArea.AxisY.Text = i_Tree_Eco_v6.Resources.Strings.Percent;
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure4, (object) LocationName);
      ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(4);
      DataTable areaOfNativeOrigin = this.GetPercentOfLiveTreePopulationByAreaOfNativeOrigin();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Continent];
      int val = 0;
      foreach (DataRow row in (InternalDataCollectionBase) areaOfNativeOrigin.Rows)
      {
        chartDataSeries.X.Add((object) val);
        chartDataSeries.Y.Add((object) (double) row["SumOfEstimate"]);
        string name = this.estUtil.ClassValues[Classifiers.Continent][(short) (int) row[classifierName]].Item1;
        if (name.EndsWith("+"))
          flag = true;
        if (name == "STATE")
          name = State.Name;
        chart.ChartArea.AxisX.ValueLabels.Add((object) val, name);
        ++val;
      }
      return Tuple.Create<C1.Win.C1Chart.C1Chart, bool>(chart, flag);
    }

    public DataTable GetPercentOfLiveTreePopulationByAreaOfNativeOrigin() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetSignificantEstimateValuesGroupedBy(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Continent
    })], this.estUtil.ClassifierNames[Classifiers.Continent], this.estUtil.ClassifierNames[Classifiers.Strata]).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private C1.Win.C1Chart.C1Chart CreateChartLeafAreaByStrata(
      List<LanduseLeafArea> landuseLeafAreas,
      string LocationName,
      Guid YearGuid,
      bool EnglishUnits)
    {
      bool flag = landuseLeafAreas.Count == 1;
      List<double> dataList = new List<double>();
      foreach (LanduseLeafArea landuseLeafArea in landuseLeafAreas)
        dataList.Add(landuseLeafArea.LeafAreaKm);
      DataScaler dataScaler = new DataScaler(dataList, Units.Squarekilometer, Units.None);
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetBarChartStyle(chart);
      chart.ChartArea.AxisY.Min = 0.0;
      chart.ChartArea.AxisX.Text = v6Strings.Strata_SingularName;
      chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.LeafArea, dataScaler.GetPrefixPlusUnitAbbreviation(EnglishUnits));
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure5, (object) LocationName);
      ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(4);
      int val = 0;
      landuseLeafAreas = landuseLeafAreas.OrderByDescending<LanduseLeafArea, double>((Func<LanduseLeafArea, double>) (x => x.LeafAreaKm)).ToList<LanduseLeafArea>();
      foreach (LanduseLeafArea landuseLeafArea in landuseLeafAreas)
      {
        chartDataSeries.X.Add((object) val);
        chartDataSeries.Y.Add((object) dataScaler.GetScaledValue(landuseLeafArea.LeafAreaKm, EnglishUnits));
        string text = !flag || this.curYear.RecordStrata ? landuseLeafArea.Landuse : i_Tree_Eco_v6.Resources.Strings.StudyArea;
        chart.ChartArea.AxisX.ValueLabels.Add((object) val, text);
        ++val;
      }
      return chart;
    }

    private C1.Win.C1Chart.C1Chart CreateChartPercentOfLandByGroundCoverClasses(
      List<GroundCoverPercent> GroundCovers,
      string LocationName)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetPieChartStyle(chart);
      for (int index = 0; index < 10 && index < GroundCovers.Count; ++index)
      {
        if (GroundCovers[index].PercentCover > 0.0)
        {
          ChartDataSeries ser = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
          ser.LineStyle.Color = ReportUtil.GetColor(index);
          ser.X.Add((object) 0);
          ser.Y.Add((object) GroundCovers[index].PercentCover);
          Label label = chart.ChartLabels.LabelsCollection.AddNewLabel();
          label.Text = GroundCovers[index].Name;
          label.AttachMethod = AttachMethodEnum.DataIndex;
          label.Visible = true;
          label.Offset = 50;
          label.Connected = true;
          label.Compass = LabelCompassEnum.Radial;
          AttachMethodData attachMethodData = label.AttachMethodData;
          attachMethodData.GroupIndex = 0;
          attachMethodData.SeriesIndex = chart.ChartGroups.Group0.ChartData.SeriesList.IndexOf(ser);
          attachMethodData.PointIndex = 0;
        }
      }
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure6, (object) LocationName);
      return chart;
    }

    private C1.Win.C1Chart.C1Chart CreateChartPollutionRemovalAndCost(
      double CORemoved,
      double CORemovalPrice,
      double NO2Removed,
      double NO2RemovalPrice,
      double OzoneRemoved,
      double OzoneRemovalPrice,
      double PM25Removed,
      double PM25RemovalPrice,
      double PM10Removed,
      double PM10RemovalPrice,
      double SO2Removed,
      double SO2RemovalPrice,
      string LocationName,
      bool EnglishUnits,
      string CurrencySymbol)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetXYPlotChartStyle(chart);
      chart.ChartArea.AxisX.AnnotationRotation = 0;
      DataScaler dataScaler1 = new DataScaler(new List<double>()
      {
        CORemoved,
        NO2Removed,
        OzoneRemoved,
        PM25Removed,
        PM10Removed,
        SO2Removed
      }, Units.MetricTons, Units.None);
      DataScaler dataScaler2 = new DataScaler(new List<double>()
      {
        CORemoved * CORemovalPrice,
        NO2Removed * NO2RemovalPrice,
        OzoneRemoved * OzoneRemovalPrice,
        PM25Removed * PM25RemovalPrice,
        PM10Removed * PM10RemovalPrice,
        SO2Removed * SO2RemovalPrice
      }, Units.Monetaryunit, Units.None, this.CurrencyName, CurrencySymbol);
      chart.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Pollutants;
      chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.PollutionRemoved, dataScaler1.GetPrefixPlusUnitAbbreviation(EnglishUnits));
      chart.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Value, string.Format("{0} {1}", (object) dataScaler2.GetScaledPrefix(EnglishUnits), (object) CurrencySymbol));
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure7, this.curYear.RecordPercentShrub ? (object) string.Format(" {0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.And, (object) i_Tree_Eco_v6.Resources.Strings.Shrubs) : (object) string.Empty, (object) LocationName);
      ChartGroup group0 = chart.ChartGroups.Group0;
      ChartDataSeries chartDataSeries = chart.ChartGroups.Group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(6);
      ChartDataSeries cdsAmount = group0.ChartData.SeriesList.AddNewSeries();
      ReportUtil.AddChartTriangleMarker(cdsAmount);
      cdsAmount.X.Add((object) 1);
      cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(CORemoved, EnglishUnits));
      chartDataSeries.X.Add((object) 1);
      chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(CORemoved * CORemovalPrice, EnglishUnits));
      chart.ChartArea.AxisX.ValueLabels.Add((object) 1, i_Tree_Eco_v6.Resources.Strings.CarbonMonoxideFormula);
      cdsAmount.X.Add((object) 2);
      cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(NO2Removed, EnglishUnits));
      chartDataSeries.X.Add((object) 2);
      chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(NO2Removed * NO2RemovalPrice, EnglishUnits));
      chart.ChartArea.AxisX.ValueLabels.Add((object) 2, i_Tree_Eco_v6.Resources.Strings.NitrogenDioxideFurmula);
      cdsAmount.X.Add((object) 3);
      cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(OzoneRemoved, EnglishUnits));
      chartDataSeries.X.Add((object) 3);
      chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(OzoneRemoved * OzoneRemovalPrice, EnglishUnits));
      chart.ChartArea.AxisX.ValueLabels.Add((object) 3, i_Tree_Eco_v6.Resources.Strings.OzoneFurmula);
      cdsAmount.X.Add((object) 4);
      cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(PM25Removed, EnglishUnits));
      chartDataSeries.X.Add((object) 4);
      chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(PM25Removed * PM25RemovalPrice, EnglishUnits));
      chart.ChartArea.AxisX.ValueLabels.Add((object) 4, i_Tree_Eco_v6.Resources.Strings.ParticulateMatter25Furmula);
      cdsAmount.X.Add((object) 5);
      cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(PM10Removed, EnglishUnits));
      chartDataSeries.X.Add((object) 5);
      chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(PM10Removed * PM10RemovalPrice, EnglishUnits));
      chart.ChartArea.AxisX.ValueLabels.Add((object) 5, i_Tree_Eco_v6.Resources.Strings.ParticulateMatter10Furmula);
      cdsAmount.X.Add((object) 6);
      cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(SO2Removed, EnglishUnits));
      chartDataSeries.X.Add((object) 6);
      chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(SO2Removed * SO2RemovalPrice, EnglishUnits));
      chart.ChartArea.AxisX.ValueLabels.Add((object) 6, i_Tree_Eco_v6.Resources.Strings.SulfurDioxideFurmula);
      return chart;
    }

    private C1.Win.C1Chart.C1Chart CreateChartAnnualCarbonSequestration(
      List<KeyValuePair<int, double>> SpeciesCarbonSequestration,
      double CustomizedCarbonDollarsPerTon,
      string LocationName,
      bool EnglishUnits,
      bool UseScientificNames,
      string CurrencySymbol)
    {
      List<double> dataList1 = new List<double>();
      List<double> dataList2 = new List<double>();
      foreach (KeyValuePair<int, double> keyValuePair in SpeciesCarbonSequestration)
      {
        dataList1.Add(keyValuePair.Value);
        dataList2.Add(keyValuePair.Value * CustomizedCarbonDollarsPerTon);
      }
      DataScaler dataScaler1 = new DataScaler(dataList1, Units.MetricTons, Units.None);
      DataScaler dataScaler2 = new DataScaler(dataList2, Units.Monetaryunit, Units.None, this.CurrencyName, CurrencySymbol);
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetXYPlotChartStyle(chart);
      chart.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Species;
      chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Sequestration, dataScaler1.GetPrefixPlusUnitAbbreviation(EnglishUnits));
      chart.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Value, string.Format("{0} {1}", (object) dataScaler2.GetScaledPrefix(EnglishUnits), (object) ReportUtil.GetValuePerYrStr(CurrencySymbol)));
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure8, (object) LocationName);
      ChartGroup group0 = chart.ChartGroups.Group0;
      ChartGroup group1 = chart.ChartGroups.Group1;
      ChartDataSeries cdsAmount = group0.ChartData.SeriesList.AddNewSeries();
      ReportUtil.AddChartTriangleMarker(cdsAmount);
      ChartDataSeries chartDataSeries = group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(6);
      int val = 0;
      foreach (KeyValuePair<int, double> keyValuePair in SpeciesCarbonSequestration)
      {
        string text = UseScientificNames ? this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair.Key].Item2 : this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair.Key].Item1;
        cdsAmount.X.Add((object) val);
        cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(keyValuePair.Value, EnglishUnits));
        chartDataSeries.X.Add((object) val);
        chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(keyValuePair.Value * CustomizedCarbonDollarsPerTon, EnglishUnits));
        chart.ChartArea.AxisX.ValueLabels.Add((object) val, text);
        ++val;
      }
      return chart;
    }

    private C1.Win.C1Chart.C1Chart CreateChartCarbonStorageAndValue(
      List<KeyValuePair<int, double>> SpeciesCarbonStorage,
      double CustomizedCarbonDollarsPerTon,
      string LocationName,
      bool EnglishUnits,
      bool UseScientificNames,
      string CurrencySymbol)
    {
      List<double> dataList1 = new List<double>();
      List<double> dataList2 = new List<double>();
      foreach (KeyValuePair<int, double> keyValuePair in SpeciesCarbonStorage)
      {
        dataList1.Add(keyValuePair.Value);
        dataList2.Add(keyValuePair.Value * CustomizedCarbonDollarsPerTon);
      }
      DataScaler dataScaler1 = new DataScaler(dataList1, Units.MetricTons, Units.None);
      DataScaler dataScaler2 = new DataScaler(dataList2, Units.Monetaryunit, Units.None, this.CurrencyName, CurrencySymbol);
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetXYPlotChartStyle(chart);
      chart.ChartArea.AxisX.Text = v6Strings.Tree_Species;
      chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Storage, dataScaler1.GetPrefixPlusUnitAbbreviation(EnglishUnits));
      chart.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Value, string.Format("{0} {1}", (object) dataScaler2.GetScaledPrefix(EnglishUnits), (object) CurrencySymbol));
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure9, (object) LocationName);
      ChartGroup group0 = chart.ChartGroups.Group0;
      ChartGroup group1 = chart.ChartGroups.Group1;
      ChartDataSeries cdsAmount = group0.ChartData.SeriesList.AddNewSeries();
      ReportUtil.AddChartTriangleMarker(cdsAmount);
      ChartDataSeries chartDataSeries = group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(6);
      int val = 0;
      foreach (KeyValuePair<int, double> keyValuePair in SpeciesCarbonStorage)
      {
        string text = UseScientificNames ? this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair.Key].Item2 : this.estUtil.ClassValues[Classifiers.Species][(short) keyValuePair.Key].Item1;
        cdsAmount.X.Add((object) val);
        cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(keyValuePair.Value, EnglishUnits));
        chartDataSeries.X.Add((object) val);
        chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(keyValuePair.Value * CustomizedCarbonDollarsPerTon, EnglishUnits));
        chart.ChartArea.AxisX.ValueLabels.Add((object) val, text);
        ++val;
      }
      return chart;
    }

    private C1.Win.C1Chart.C1Chart CreateChartAvoidedRunoffBySpecies(
      List<KeyValuePair<int, double>> topSpecies,
      double CustomizedRainfallWaterPrice,
      string LocationName,
      bool EnglishUnits,
      bool ScientificName,
      string CurrencySymbol)
    {
      List<double> dataList1 = new List<double>();
      List<double> dataList2 = new List<double>();
      foreach (KeyValuePair<int, double> topSpecy in topSpecies)
      {
        dataList1.Add(topSpecy.Value);
        dataList2.Add(topSpecy.Value * CustomizedRainfallWaterPrice);
      }
      DataScaler dataScaler1 = new DataScaler(dataList1, Units.CubicMeter, Units.None);
      DataScaler dataScaler2 = new DataScaler(dataList2, Units.Monetaryunit, Units.None, this.CurrencyName, CurrencySymbol);
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetXYPlotChartStyle(chart);
      chart.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Species;
      chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff, dataScaler1.GetPrefixPlusUnitDescription(EnglishUnits));
      chart.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.Value, string.Format("{0} {1}", (object) CurrencySymbol, (object) dataScaler2.GetScaledPrefix(EnglishUnits)));
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure10, (object) LocationName);
      ChartGroup group0 = chart.ChartGroups.Group0;
      ChartGroup group1 = chart.ChartGroups.Group1;
      group1.ChartType = Chart2DTypeEnum.Bar;
      group0.ChartType = Chart2DTypeEnum.XYPlot;
      ChartDataSeries cdsAmount = group0.ChartData.SeriesList.AddNewSeries();
      ReportUtil.AddChartTriangleMarker(cdsAmount);
      ChartDataSeries chartDataSeries = group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(6);
      int val = 0;
      foreach (KeyValuePair<int, double> topSpecy in topSpecies)
      {
        string text = ScientificName ? this.estUtil.ClassValues[Classifiers.Species][(short) topSpecy.Key].Item2 : this.estUtil.ClassValues[Classifiers.Species][(short) topSpecy.Key].Item1;
        cdsAmount.X.Add((object) val);
        cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(topSpecy.Value, EnglishUnits));
        chartDataSeries.X.Add((object) val);
        chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(topSpecy.Value * CustomizedRainfallWaterPrice, EnglishUnits));
        chart.ChartArea.AxisX.ValueLabels.Add((object) val, text);
        ++val;
      }
      return chart;
    }

    private C1.Win.C1Chart.C1Chart CreateChartGreatestReplacementValues(
      string LocationName,
      string CurrencySymbol)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetBarChartStyle(chart);
      chart.Font = new Font("Calibri", 10f);
      chart.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Species;
      chart.Footer.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure11, (object) LocationName);
      DataTable structuralValues = this.GetGreatestStructuralValues();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      List<int> intList = new List<int>();
      List<double> dataList = new List<double>();
      int num1 = 0;
      foreach (DataRow row in (InternalDataCollectionBase) structuralValues.Rows)
      {
        double num2 = (double) row["EstimateValue"];
        dataList.Add(num2);
        intList.Add((int) row[classifierName]);
        ++num1;
      }
      DataScaler dataScaler = new DataScaler(dataList, Units.Monetaryunit, Units.None, this.CurrencyName, CurrencySymbol);
      chart.ChartArea.AxisY.Text = !(dataScaler.GetScaledPrefix(ReportBase.EnglishUnits) == string.Empty) ? ReportUtil.FormatInlineHeaderUnitsStr(string.Format("{0}{1}", (object) i_Tree_Eco_v6.Resources.Strings.ReplacementValue, (object) Environment.NewLine), string.Format("{0} {1}", (object) CurrencySymbol, (object) dataScaler.GetScaledPrefix(ReportBase.EnglishUnits))) : ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.ReplacementValue, CurrencySymbol);
      ChartDataSeries chartDataSeries = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      for (int index = 0; index < dataList.Count; ++index)
      {
        chartDataSeries.X.Add((object) index);
        chartDataSeries.Y.Add((object) dataScaler.GetScaledValue(dataList[index], ReportBase.EnglishUnits));
        string speciesName = this.GetSpeciesName((short) intList[index]);
        chart.ChartArea.AxisX.ValueLabels.Add((object) index, speciesName);
      }
      return chart;
    }

    public DataTable GetGreatestStructuralValues() => this.GetEstimatedValues().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CompensatoryValue]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Monetaryunit, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>().AsEnumerable().Take<DataRow>(10).CopyToDataTable<DataRow>();

    private C1.Win.C1Chart.C1Chart CreateChartSpeciesPestRiskCounty(
      List<PestAffected> PestsInLocation,
      string LocationName,
      string CurrencySymbol,
      bool IsInternational)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetXYPlotChartStyle(chart);
      chart.ChartArea.AxisX.AnnotationRotation = -90;
      List<double> dataList1 = new List<double>();
      List<double> dataList2 = new List<double>();
      foreach (PestAffected pestAffected in PestsInLocation)
      {
        dataList1.Add(pestAffected.PopulationAffected);
        dataList2.Add(pestAffected.ReplacementValue);
      }
      DataScaler dataScaler1 = new DataScaler(dataList1, Units.Count, Units.None);
      DataScaler dataScaler2 = new DataScaler(dataList2, Units.Monetaryunit, Units.None, this.CurrencyName, CurrencySymbol);
      chart.ChartArea.AxisX.Text = i_Tree_Eco_v6.Resources.Strings.Pest;
      chart.ChartArea.AxisY.Text = dataScaler1.GetScaledPrefix(ReportBase.EnglishUnits) == string.Empty ? i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV : ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV, dataScaler1.GetScaledPrefix(ReportBase.EnglishUnits));
      chart.ChartArea.AxisY2.Text = dataScaler2.GetScaledPrefix(ReportBase.EnglishUnits) == string.Empty ? ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.ReplacementValue, CurrencySymbol) : ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.ReplacementValue, string.Format("{0} {1}", (object) CurrencySymbol, (object) dataScaler2.GetScaledPrefix(ReportBase.EnglishUnits)));
      chart.Footer.Text = IsInternational ? string.Format(i_Tree_Eco_v6.Resources.Strings.WR_InternationalFigure12, (object) LocationName) : string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Figure12, (object) LocationName);
      ChartGroup group1 = chart.ChartGroups.Group1;
      ChartDataSeries cdsAmount = chart.ChartGroups.Group0.ChartData.SeriesList.AddNewSeries();
      ReportUtil.AddChartTriangleMarker(cdsAmount);
      ChartDataSeries chartDataSeries = group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = ReportUtil.GetColor(6);
      int val = 0;
      foreach (PestAffected pestAffected in PestsInLocation)
      {
        cdsAmount.X.Add((object) val);
        cdsAmount.Y.Add((object) dataScaler1.GetScaledValue(pestAffected.PopulationAffected, ReportBase.EnglishUnits));
        chartDataSeries.X.Add((object) val);
        chartDataSeries.Y.Add((object) dataScaler2.GetScaledValue(pestAffected.ReplacementValue, ReportBase.EnglishUnits));
        chart.ChartArea.AxisX.ValueLabels.Add((object) val, pestAffected.Abb);
        ++val;
      }
      return chart;
    }

    private C1.Win.C1Chart.C1Chart CreateChartPestRiskAndDistance(
      ISession LocSpSession,
      Location County,
      string Level,
      Dictionary<int, double> SpeciesTreeCount,
      DataScaler pestTreeCountScaler,
      DataScaler pestReplacementValueScaler,
      Dictionary<int, double> speciesReplacementValues)
    {
      C1.Win.C1Chart.C1Chart chart = new C1.Win.C1Chart.C1Chart();
      ReportUtil.SetXYPlotChartStyle(chart);
      chart.Footer.Visible = false;
      chart.ChartArea.AxisX.Font = new Font("Calibri", 8f);
      chart.Font = new Font("Calibri", 8f);
      chart.ChartArea.AxisX.AnnotationRotation = -90;
      ChartGroup group0 = chart.ChartGroups.Group0;
      ChartGroup group1 = chart.ChartGroups.Group1;
      ChartDataSeries cdsAmount = group0.ChartData.SeriesList.AddNewSeries();
      ReportUtil.AddChartTriangleMarker(cdsAmount);
      cdsAmount.SymbolStyle.Size = 5;
      ChartDataSeries chartDataSeries = group1.ChartData.SeriesList.AddNewSeries();
      chartDataSeries.LineStyle.Color = Color.FromName(Level);
      List<PestAffected> pestAffectedList = new List<PestAffected>();
      List<Pest> list = LocSpSession.CreateCriteria<Pest>().List<Pest>().ToList<Pest>();
      SortedDictionary<string, PestAffected> sortedDictionary = new SortedDictionary<string, PestAffected>();
      IList<LocationPestRisk> pestRisks = this.LocationService.GetPestRisks(County.Id);
      foreach (Pest pest in Level != "Green" ? list.Where<Pest>((Func<Pest, bool>) (x => pestRisks.Where<LocationPestRisk>((Func<LocationPestRisk, bool>) (y => y.Risk.Level == Level && y.Pest.Equals((object) x))).Count<LocationPestRisk>() > 0)) : list.Where<Pest>((Func<Pest, bool>) (x => pestRisks.Count<LocationPestRisk>((Func<LocationPestRisk, bool>) (p => p.Pest.Equals((object) x))) == 0)))
      {
        if (((IEnumerable<string>) this._pests).Contains<string>(pest.ABB))
        {
          if (!sortedDictionary.ContainsKey(pest.ABB))
            sortedDictionary.Add(pest.ABB, new PestAffected()
            {
              Abb = pest.ABB.ToUpper()
            });
          foreach (Species susceptableSpecy in (IEnumerable<Species>) pest.SusceptableSpecies)
          {
            int valueOrderFromName = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Species, susceptableSpecy.CommonName);
            if (valueOrderFromName != -1 && SpeciesTreeCount.ContainsKey(valueOrderFromName))
            {
              sortedDictionary[pest.ABB].PopulationAffected += SpeciesTreeCount[valueOrderFromName];
              sortedDictionary[pest.ABB].ReplacementValue += speciesReplacementValues[valueOrderFromName];
            }
          }
        }
      }
      int val = 0;
      if (pestTreeCountScaler == null || pestReplacementValueScaler == null)
      {
        List<double> dataList1 = new List<double>();
        List<double> dataList2 = new List<double>();
        foreach (PestAffected pestAffected in sortedDictionary.Values)
        {
          dataList1.Add(pestAffected.PopulationAffected);
          dataList2.Add(pestAffected.ReplacementValue);
        }
        pestTreeCountScaler = new DataScaler(dataList1, Units.Count, Units.None);
        pestReplacementValueScaler = new DataScaler(dataList2, Units.Monetaryunit, Units.None, this.CurrencyName, this.CurrencySymbol);
      }
      chart.ChartArea.AxisY.Text = pestTreeCountScaler.GetScaledPrefix(ReportBase.EnglishUnits) != string.Empty ? (chart.ChartArea.AxisY.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV, pestTreeCountScaler.GetScaledPrefix(ReportBase.EnglishUnits))) : ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.ReplacementValue, this.CurrencySymbol);
      if (pestReplacementValueScaler.GetScaledPrefix(ReportBase.EnglishUnits) != string.Empty)
        chart.ChartArea.AxisY2.Text = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.ReplacementValue, string.Format("{0} {1}", (object) this.CurrencySymbol, (object) pestReplacementValueScaler.GetScaledPrefix(ReportBase.EnglishUnits)));
      foreach (PestAffected pestAffected in sortedDictionary.Values)
      {
        cdsAmount.X.Add((object) val);
        cdsAmount.Y.Add((object) pestTreeCountScaler.GetScaledValue(pestAffected.PopulationAffected, ReportBase.EnglishUnits));
        chartDataSeries.X.Add((object) val);
        chartDataSeries.Y.Add((object) pestReplacementValueScaler.GetScaledValue(pestAffected.ReplacementValue, ReportBase.EnglishUnits));
        chart.ChartArea.AxisX.ValueLabels.Add((object) val, pestAffected.Abb);
        ++val;
      }
      return chart;
    }

    private void CreatePestRiskTable(
      C1PrintDocument C1doc,
      Location County,
      Dictionary<int, double> SpeciesCounts,
      bool ScientificName)
    {
      ISession s = ReportBase.m_ps.LocSp.OpenSession();
      List<Pest> pestList = RetryExecutionHandler.Execute<List<Pest>>((Func<List<Pest>>) (() =>
      {
        try
        {
          using (ITransaction transaction = s.BeginTransaction())
          {
            List<Pest> list = s.QueryOver<Pest>().Fetch<Pest, Pest>(SelectMode.Fetch, (System.Linq.Expressions.Expression<Func<Pest, object>>) (p => p.SusceptableSpecies)).TransformUsing(Transformers.DistinctRootEntity).Cacheable().List().Where<Pest>((Func<Pest, bool>) (p => ((IEnumerable<string>) this._pests).Contains<string>(p.ABB))).OrderBy<Pest, string>((Func<Pest, string>) (p => p.ABB)).ToList<Pest>();
            transaction.Commit();
            return list;
          }
        }
        catch (Exception ex)
        {
          s.Dispose();
          s = ReportBase.m_ps.LocSp.OpenSession();
          throw;
        }
      }));
      Dictionary<Species, Dictionary<Pest, int>> speciesPestDictionary = new Dictionary<Species, Dictionary<Pest, int>>();
      List<SpeciesView> list1 = ReportBase.m_ps.Species.Values.ToList<SpeciesView>();
      IList<LocationPestRisk> pestRisks = this.LocationService.GetPestRisks(County.Id);
      foreach (int key in SpeciesCounts.Keys)
      {
        string cn = this.estUtil.ClassValues[Classifiers.Species][(short) key].Item1;
        Species species = list1.Where<SpeciesView>((Func<SpeciesView, bool>) (spec => spec.CommonName.Equals(cn))).FirstOrDefault<SpeciesView>()?.Species;
        if (species != null)
        {
          speciesPestDictionary.Add(species, new Dictionary<Pest, int>());
          foreach (Pest pest in pestList)
          {
            Pest p = pest;
            if (p.SusceptableSpecies.Contains(species))
            {
              LocationPestRisk locationPestRisk = pestRisks.Where<LocationPestRisk>((Func<LocationPestRisk, bool>) (pr => pr.Pest.Equals((object) p))).SingleOrDefault<LocationPestRisk>();
              if (locationPestRisk != null)
              {
                if (locationPestRisk.Risk.Level == "Red")
                  speciesPestDictionary[species].Add(p, 4);
                else if (locationPestRisk.Risk.Level == "Orange")
                  speciesPestDictionary[species].Add(p, 3);
                else if (locationPestRisk.Risk.Level == "Yellow")
                  speciesPestDictionary[species].Add(p, 2);
              }
              else
                speciesPestDictionary[species].Add(p, 1);
            }
            else
              speciesPestDictionary[species].Add(p, 0);
          }
        }
      }
      s.Dispose();
      List<Species> list2 = speciesPestDictionary.Keys.OrderByDescending<Species, int>((Func<Species, int>) (p => Enumerable.Sum(speciesPestDictionary[p].Values))).ToList<Species>();
      RenderTable ro = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) ro);
      ro.Rows[0].Style.TextAngle = 90f;
      ro.Rows[0].Style.FontBold = true;
      ro.Rows[0].Style.Font = new Font("Calibri", 8f, FontStyle.Bold);
      ro.Rows[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
      ro.Style.Padding.All = (Unit) ".1cm";
      ro.Style.GridLines.All = LineDef.Default;
      ro.Cols[0].Width = (Unit) "2.2%";
      ro.Cols[1].Width = (Unit) "4%";
      ro.Cols[2].Width = (Unit) "14%";
      ro.Cols[1].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
      ro.Cols[2].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      ro.Cols[2].Style.WordWrap = true;
      ro.Cells[0, 2].CellStyle.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
      ro.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.WR_Apendix6P3TableSppRisk;
      ro.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.WR_Apendix6P3TableRiskNewLineWeight;
      ro.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.WR_Apendix6P3TableSpeciesNewLineName;
      for (int index = 0; index < pestList.Count; ++index)
        ro.Cells[0, index + 3].Text = pestList[index].ABB;
      Dictionary<int, Color> dictionary = new Dictionary<int, Color>();
      dictionary.Add(0, Color.White);
      dictionary.Add(1, Color.Green);
      dictionary.Add(2, Color.Yellow);
      dictionary.Add(3, Color.Orange);
      dictionary.Add(4, Color.Red);
      for (int index1 = 0; index1 < list2.Count; ++index1)
      {
        int num = Enumerable.Sum(speciesPestDictionary[list2[index1]].Values);
        if (num == 0)
          break;
        ro.Cells[index1 + 1, 0].Style.BackColor = dictionary[Enumerable.Max(speciesPestDictionary[list2[index1]].Values)];
        ro.Cells[index1 + 1, 1].Text = num.ToString();
        if (ScientificName)
        {
          string str = list2[index1].ScientificName;
          if (list2[index1].Rank == SpeciesRank.Species)
            str = string.Format("{0} {1}", (object) list2[index1].Parent.ScientificName, (object) str);
          ro.Cells[index1 + 1, 2].Text = str;
        }
        else
          ro.Cells[index1 + 1, 2].Text = list2[index1].CommonName;
        for (int index2 = 0; index2 < pestList.Count; ++index2)
          ro.Cells[index1 + 1, index2 + 3].Style.BackColor = dictionary[speciesPestDictionary[list2[index1]][pestList[index2]]];
      }
    }

    private string GetInvaisiveSpeciesName(
      SortedList<double, InvasiveSpecies> invasiveSpeciesDesc,
      int i)
    {
      return !ReportBase.ScientificName ? this.estUtil.ClassValues[Classifiers.Species][invasiveSpeciesDesc.Values[i].SpeciesCVO].Item1 : this.estUtil.ClassValues[Classifiers.Species][invasiveSpeciesDesc.Values[i].SpeciesCVO].Item2;
    }

    private string GetSpeciesName(short i) => !ReportBase.ScientificName ? this.estUtil.ClassValues[Classifiers.Species][i].Item1 : this.estUtil.ClassValues[Classifiers.Species][i].Item2;

    private string GetGroundCoversStr(List<string> grCovers)
    {
      string groundCoversStr = string.Empty;
      for (int index = 0; index < grCovers.Count; ++index)
        groundCoversStr = index != 0 ? (index >= grCovers.Count - 1 ? groundCoversStr + string.Format(", {0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.And, (object) grCovers[index].ToLower()) : groundCoversStr + ", " + grCovers[index].ToLower()) : grCovers[index].ToLower();
      return groundCoversStr;
    }

    private void RenderBullet(RenderArea ra, string s)
    {
      RenderText ro1 = new RenderText("•", this.styleBullet);
      ro1.Name = "bullet";
      ra.Children.Add((RenderObject) ro1);
      RenderText ro2 = new RenderText(s, this.styleList);
      ro2.Y = (Unit) "bullet.Top";
      ra.Children.Add((RenderObject) ro2);
    }

    private void RenderListItem(C1PrintDocument C1doc, string text)
    {
      RenderArea renderArea = new RenderArea();
      renderArea.Style.Spacing.Top = (Unit) 0;
      renderArea.Style.Spacing.Bottom = (Unit) 0;
      this.RenderBullet(renderArea, text);
      C1doc.Body.Children.Add((RenderObject) renderArea);
    }

    private void RenderSummaryListItem(C1PrintDocument C1doc, string text)
    {
      RenderArea renderArea = new RenderArea();
      renderArea.Style.Spacing.Top = (Unit) "1ls";
      renderArea.Style.Spacing.Bottom = (Unit) 0;
      this.RenderBullet(renderArea, text);
      C1doc.Body.Children.Add((RenderObject) renderArea);
    }

    private void AddUnitDefinition(
      RenderParagraph rParagraph,
      string superscriptSymbol,
      string definition)
    {
      ParagraphText po1 = new ParagraphText();
      po1.Style.Parents = this.styleTinyText;
      po1.Style.TextPosition = TextPositionEnum.Superscript;
      po1.Text = superscriptSymbol;
      rParagraph.Content.Add((ParagraphObject) po1);
      ParagraphText po2 = new ParagraphText();
      po2.Style.Parents = this.styleTinyText;
      po2.Text = string.Format("{0}{1}", (object) definition, (object) Environment.NewLine);
      rParagraph.Content.Add((ParagraphObject) po2);
    }

    private string GetGroundCoverPercentStr(GroundCoverPercent grCover) => string.Format(i_Tree_Eco_v6.Resources.Strings.WR_FmtGroundCoverAndValueUnits, (object) ReportUtil.UpperCaseFirstLetterEachWord(grCover.Name), (object) grCover.PercentCover.ToString("0.0"), (object) i_Tree_Eco_v6.Resources.Strings.Percent.ToLower());

    private void AddSubscript(RenderParagraph rParagraph, string i)
    {
      ParagraphText po = new ParagraphText();
      po.Text = i;
      po.Style.TextPosition = TextPositionEnum.Superscript;
      rParagraph.Content.Add((ParagraphObject) po);
    }

    private void AddAboveFooterLine(C1PrintDocument C1doc, string h = "-1mm")
    {
      RenderLine ro = new RenderLine((Unit) 0, (Unit) 0, (Unit) "Page.Width / 3", (Unit) 0, LineDef.Default);
      ro.X = (Unit) "prev.left";
      ro.Y = (Unit) string.Format("next.y{0}", (object) h);
      C1doc.Body.Children.Add((RenderObject) ro);
    }

    private void AddAboveAsteriskFooterNote(C1PrintDocument C1doc, string s)
    {
      RenderText ro = new RenderText();
      this.AddAboveFooterLine(C1doc);
      ro.Style.Parents = this.styleTinyText;
      ro.Style.Spacing.Bottom = (Unit) 0;
      ro.Text = s;
      ro.Y = (Unit) "page.bottom - (self.height + 5mm)";
      C1doc.Body.Children.Add((RenderObject) ro);
    }

    private void FormatValuesBulletList(C1PrintDocument C1doc, double val, string title) => this.RenderListItem(C1doc, string.Format("{0} {1}{2}", (object) title, (object) this.CurrencySymbol, (object) ReportUtil.GetNumericWordWithMagnitude(val, 3)));

    private void FormatInternationalNote(
      C1PrintDocument C1doc,
      RenderParagraph rParagraph,
      string s)
    {
      ParagraphText po = new ParagraphText(string.Format(" {0}", (object) s));
      rParagraph.Content.Add((ParagraphObject) po);
      C1doc.Body.Children.Add((RenderObject) rParagraph);
    }

    private void AddAppendix1Definition(C1PrintDocument C1doc, string s)
    {
      RenderText ro = new RenderText();
      ro.Text = s;
      ro.Style.FontUnderline = true;
      ro.Style.Spacing.Top = (Unit) "1ls";
      C1doc.Body.Children.Add((RenderObject) ro);
    }

    private void AddDefaultParagraph(C1PrintDocument C1doc, string s, bool noSpace = false)
    {
      RenderText ro = new RenderText();
      ro.Text = s;
      ro.Style.Parents = this.styleDefaultParagraph;
      if (noSpace)
        ro.Style.Spacing.Top = (Unit) 0;
      C1doc.Body.Children.Add((RenderObject) ro);
    }

    private void AddSectionTitleAndIndex(C1PrintDocument C1doc, string s)
    {
      RenderText renderText = new RenderText(s);
      renderText.Style.Parents = this.styleSectionTitle;
      RenderTocItem renderTocItem = this.toc.AddItem(s, (RenderObject) renderText);
      renderTocItem.Style.Parent = this.styleTocItem;
      renderTocItem.TabPositions.Add(new TabPosition((Unit) ".9cm"));
      renderText.BreakBefore = BreakEnum.Page;
      C1doc.Body.Children.Add((RenderObject) renderText);
    }

    private void StartOnNewPage(C1PrintDocument C1doc)
    {
      this.rText = new RenderText();
      this.rText.BreakBefore = BreakEnum.Page;
      C1doc.Body.Children.Add((RenderObject) this.rText);
    }

    private void InserNewLine(C1PrintDocument C1doc)
    {
      this.rText = new RenderText();
      this.rText.Style.Spacing.Top = (Unit) "1ls";
      C1doc.Body.Children.Add((RenderObject) this.rText);
    }

    private RenderText AddTableTitle(string s)
    {
      RenderText renderText = new RenderText(s);
      renderText.Style.Parents = this.styleTableTitle;
      return renderText;
    }

    private string GetCustomizedDollarsPerTon(double val) => (ReportBase.EnglishUnits ? val / 1.10231 : val).ToString("N0");

    public void CreateWrittenReport(C1PrintDocument C1doc)
    {
      Assembly assembly = this.GetType().Assembly;
      this.resmanStateInvasives = new ResourceManager("i_Tree_Eco_v6.Reports.StateInvasivesReferences", assembly);
      this.resmanPestReferences = new ResourceManager("i_Tree_Eco_v6.Reports.PestReferences", assembly);
      int num1 = this.curInputISession.CreateCriteria<Plot>().CreateAlias("Year", "y").Add((ICriterion) Restrictions.Eq("y.Guid", (object) this.YearGuid)).Add((ICriterion) Restrictions.Eq("IsComplete", (object) true)).SetProjection(Projections.RowCount()).UniqueResult<int>();
      YearLocationData yearLocationData = this.curYear.YearLocationData.FirstOrDefault<YearLocationData>();
      int num2 = yearLocationData != null ? yearLocationData.Population : 0;
      double pollutionRemovalAmount1 = this.GetYearlyPollutionRemovalAmount(EstimateDataTypes.TreeShrubCombined, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "O3"));
      double pollutionRemovalAmount2 = this.GetYearlyPollutionRemovalAmount(EstimateDataTypes.TreeShrubCombined, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "CO"));
      double pollutionRemovalAmount3 = this.GetYearlyPollutionRemovalAmount(EstimateDataTypes.TreeShrubCombined, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "NO2"));
      double pollutionRemovalAmount4 = this.GetYearlyPollutionRemovalAmount(EstimateDataTypes.TreeShrubCombined, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM2.5"));
      double pollutionRemovalAmount5 = this.GetYearlyPollutionRemovalAmount(EstimateDataTypes.TreeShrubCombined, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM10*"));
      double pollutionRemovalAmount6 = this.GetYearlyPollutionRemovalAmount(EstimateDataTypes.TreeShrubCombined, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "SO2"));
      double pollutionRemovalAmount7 = this.GetYearlyTotalPollutionRemovalAmount(EstimateDataTypes.TreeShrubCombined);
      double pollutionRemovalDollars = this.GetYearlyTotalPollutionRemovalDollars(EstimateDataTypes.TreeShrubCombined, this.customizedCoDollarsPerTon, this.customizedO3DollarsPerTon, this.customizedNO2DollarsPerTon, this.customizedSO2DollarsPerTon, this.customizedPM25DollarsPerTon, this.customizedPM10DollarsPerTon);
      double energyValues1 = this.estUtil.GetEnergyValues(1, 2, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon);
      double energyValues2 = this.estUtil.GetEnergyValues(2, 2, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon);
      double energyValues3 = this.estUtil.GetEnergyValues(2, 1, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon);
      double english1 = EstimateUtil.ConvertToEnglish(this.estUtil.GetEnergyValues(3, 2, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon), Units.MetricTons, ReportBase.EnglishUnits);
      double english2 = EstimateUtil.ConvertToEnglish(this.estUtil.GetEnergyValues(3, 1, 1, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon), Units.MetricTons, ReportBase.EnglishUnits);
      double energyValues4 = this.estUtil.GetEnergyValues(1, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon);
      double energyValues5 = this.estUtil.GetEnergyValues(2, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon);
      double energyValues6 = this.estUtil.GetEnergyValues(2, 1, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon);
      double energyValues7 = this.estUtil.GetEnergyValues(3, 2, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon);
      double energyValues8 = this.estUtil.GetEnergyValues(3, 1, 2, this.customizedHeatingDollarsPerTherm * 10.0023877, this.customizedElectricityDollarsPerKwh * 1000.0, this.customizedCarbonDollarsPerTon);
      double num3 = Math.Ceiling(this.GetTreeCounts());
      double avgCoverPercent = this.GetAvgCoverPercent();
      double treeCoverHectare = this.GetTotalTreeCoverHectare(avgCoverPercent);
      DataTable speciesCounts = this.GetSpeciesCounts();
      Dictionary<int, double> dictionary1 = new Dictionary<int, double>();
      List<Tuple<int, double, string>> SpeciesByCountDesc = new List<Tuple<int, double, string>>();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) speciesCounts.Rows)
      {
        int num4 = (int) row[classifierName];
        double num5 = (double) row["estimateValue"];
        dictionary1.Add(num4, num5);
        SpeciesByCountDesc.Add(Tuple.Create<int, double, string>(num4, num5, this.GetSpeciesName((short) num4)));
      }
      int count1 = speciesCounts.Rows.Count;
      SortedList<int, double> dbhTreeCount = this.GetDbhTreeCount(dictionary1);
      double carbonStorage = this.GetCarbonStorage();
      double num6 = carbonStorage * this.customizedCarbonDollarsPerTon;
      double carbonSequestration = this.GetGrossCarbonSequestration();
      double num7 = energyValues4 + energyValues5 + energyValues6;
      double num8 = english1 + english2;
      Dictionary<int, double> structuralValues = this.GetStructuralValues();
      double structuralValue = this.GetStructuralValue();
      this.RenderFooter(C1doc);
      DataScaler dataScaler1 = new DataScaler();
      this.rText = new RenderText(string.Format("i-Tree{0}{1}", (object) Environment.NewLine, (object) i_Tree_Eco_v6.Resources.Strings.ReportTitleITreeEcosystemAnalysis), new Font("Calibri", 50f), Color.Black, C1.C1Preview.AlignHorzEnum.Center);
      this.rg = new RenderGraphics();
      this.rg.Name = "gradientRectangle";
      this.rg.Height = (Unit) "parent.Height";
      this.rg.Width = (Unit) "parent.width";
      C1doc.Body.Children.Add((RenderObject) this.rg);
      this.DrawGradientOnGraphics(this.formGraphics, this.rg);
      RenderImage ro1 = new RenderImage((Image) i_Tree_Eco_v6.Properties.Resources.forest);
      ro1.Name = "forestImage";
      C1doc.Body.Children.Add((RenderObject) ro1);
      ro1.Width = (Unit) "90%";
      ro1.Style.ImageAlign.BestFit = true;
      ro1.Style.ImageAlign.KeepAspectRatio = true;
      ro1.X = (Unit) "gradientRectangle.Left + (gradientRectangle.width/2 - self.width/2)";
      ro1.Y = (Unit) "gradientRectangle.Top + (gradientRectangle.Height/2 - self.height/2) - 10mm";
      C1doc.Body.Children.Add((RenderObject) this.rText);
      this.rText.Y = (Unit) "gradientRectangle.Top";
      this.rText = new RenderText(this.locationName, new Font("Calibri", 40f), Color.White, C1.C1Preview.AlignHorzEnum.Center);
      this.rText.Y = (Unit) "forestImage.Top - .7mm";
      C1doc.Body.Children.Add((RenderObject) this.rText);
      this.rText = new RenderText(string.Format("{0}{1}{2}", (object) i_Tree_Eco_v6.Resources.Strings.UrbanForestEffectsAndValues, (object) Environment.NewLine, (object) DateTime.Now.ToString("MMMM yyyy")), new Font("Calibri", 28f), C1.C1Preview.AlignHorzEnum.Center);
      this.rText.Name = "EffectsAndValues";
      this.rText.Y = (Unit) "forestImage.Top + forestImage.Height + 5mm";
      C1doc.Body.Children.Add((RenderObject) this.rText);
      RenderLine ro2 = new RenderLine();
      ro2.Line.X1 = (Unit) 0;
      ro2.Line.X2 = (Unit) "EffectsAndValues.Width * .8";
      ro2.Line.Y1 = (Unit) "0";
      ro2.Line.Y2 = (Unit) "0";
      ro2.Y = (Unit) "EffectsAndValues.Bottom + 5mm";
      C1doc.Body.Children.Add((RenderObject) ro2);
      RenderObjectCollection children = C1doc.Body.Children;
      RenderArea ro3 = new RenderArea();
      ro3.BreakAfter = BreakEnum.Page;
      children.Add((RenderObject) ro3);
      this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.Summary);
      this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.SummaryText, (object) this.locationName, (object) this.curYear.Id, (object) (this.series.SampleType == SampleType.Inventory ? num3 : (double) num1), this.series.SampleType == SampleType.Inventory ? (object) v6Strings.Tree_PluralName.ToLower() : (object) i_Tree_Eco_v6.Resources.Strings.FieldPlots));
      C1PrintDocument C1doc1 = C1doc;
      string fmtFieldValue1 = i_Tree_Eco_v6.Resources.Strings.FmtFieldValue;
      string numberOfTreesL = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesL;
      double num9 = this.series.SampleType == SampleType.Inventory ? num3 : ReportUtil.RoundToSignificantFigures(num3, 4);
      string str1 = num9.ToString("N0");
      string text1 = string.Format(fmtFieldValue1, (object) numberOfTreesL, (object) str1);
      this.RenderSummaryListItem(C1doc1, text1);
      if (this.curYear.RecordStrata)
      {
        this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.TreeCover, (object) string.Format("{0:N1} %", (object) avgCoverPercent)));
      }
      else
      {
        dataScaler1.SetScaler(treeCoverHectare, Units.Hectare, Units.None);
        this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.TreeCover, (object) dataScaler1.GetPhraseOfScaledValueWithUnit(treeCoverHectare, ReportBase.EnglishUnits)));
      }
      string str2 = string.Empty;
      for (int index = 0; index < 3 && SpeciesByCountDesc.Count > index; ++index)
        str2 = str2 + (index > 0 ? ", " : string.Empty) + SpeciesByCountDesc[index].Item3;
      this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.MostCommonSpeciesOfTrees, (object) str2));
      if (num3 == 0.0)
      {
        this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.PercentageOfTreesLessThan6Inch, (object) "0"));
      }
      else
      {
        C1PrintDocument C1doc2 = C1doc;
        string fmtFieldValue2 = i_Tree_Eco_v6.Resources.Strings.FmtFieldValue;
        string treesLessThan6Inch = i_Tree_Eco_v6.Resources.Strings.PercentageOfTreesLessThan6Inch;
        num9 = dbhTreeCount[1] / num3 + dbhTreeCount[2] / num3;
        string str3 = num9.ToString("P1");
        string text2 = string.Format(fmtFieldValue2, (object) treesLessThan6Inch, (object) str3);
        this.RenderSummaryListItem(C1doc2, text2);
      }
      dataScaler1.SetScaler(pollutionRemovalAmount7, Units.MetricTons, Units.None);
      this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.PollutionRemoval, (object) string.Format("{0}/{2} ({1}/{2})", (object) dataScaler1.GetPhraseOfScaledValueWithUnit(pollutionRemovalAmount7, ReportBase.EnglishUnits), (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCurrencySymbolValue, (object) this.CurrencySymbol, (object) ReportUtil.GetNumericWordWithMagnitude(pollutionRemovalDollars, 3)), (object) v6Strings.Year_SingularName.ToLower())));
      dataScaler1.SetScaler(carbonStorage, Units.MetricTons, Units.None);
      this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.CarbonStorage, (object) string.Format("{0} ({1})", (object) dataScaler1.GetPhraseOfScaledValueWithUnit(carbonStorage, ReportBase.EnglishUnits), (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCurrencySymbolValue, (object) this.CurrencySymbol, (object) ReportUtil.GetNumericWordWithMagnitude(num6, 3)))));
      dataScaler1.SetScaler(carbonSequestration, Units.MetricTons, Units.None);
      this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.CarbonSequestration, (object) string.Format("{0} ({1}/{2})", (object) dataScaler1.GetPhraseOfScaledValueWithUnit(carbonSequestration, ReportBase.EnglishUnits), (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCurrencySymbolValue, (object) this.CurrencySymbol, (object) ReportUtil.GetNumericWordWithMagnitude(carbonSequestration * this.customizedCarbonDollarsPerTon, 3)), (object) v6Strings.Year_SingularName.ToLower())));
      double oxygenProduction = this.GetTotalOxygenProduction();
      dataScaler1.SetScaler(oxygenProduction, Units.MetricTons, Units.None);
      this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.OxygenProduction, (object) string.Format("{0}/{1}", (object) dataScaler1.GetPhraseOfScaledValueWithUnit(oxygenProduction, ReportBase.EnglishUnits), (object) v6Strings.Year_SingularName.ToLower())));
      Tuple<double, double> avoidedRunoff = this.GetAvoidedRunoff(this.customizedWaterDollarsPerM3);
      dataScaler1.SetScaler(avoidedRunoff.Item1, Units.CubicMeter, Units.None);
      this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.AvoidedRunoff, (object) string.Format("{0}/{2} ({1}/{2})", (object) dataScaler1.GetPhraseOfScaledValueWithUnit(avoidedRunoff.Item1, ReportBase.EnglishUnits), (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCurrencySymbolValue, (object) this.CurrencySymbol, (object) ReportUtil.GetNumericWordWithMagnitude(avoidedRunoff.Item2, 3)), (object) v6Strings.Year_SingularName.ToLower())));
      if (this.curYear.RecordEnergy)
      {
        C1PrintDocument C1doc3 = C1doc;
        string fmtFieldValue3 = i_Tree_Eco_v6.Resources.Strings.FmtFieldValue;
        string buildingEnergySavings = i_Tree_Eco_v6.Resources.Strings.BuildingEnergySavings;
        string currencySymbolValue = i_Tree_Eco_v6.Resources.Strings.FmtCurrencySymbolValue;
        string currencySymbol = this.CurrencySymbol;
        num9 = ReportUtil.RoundToSignificantFigures(num7, 3);
        string str4 = num9.ToString("N0");
        string str5 = string.Format("{0}/{1}", (object) string.Format(currencySymbolValue, (object) currencySymbol, (object) str4), (object) v6Strings.Year_SingularName.ToLower());
        string text3 = string.Format(fmtFieldValue3, (object) buildingEnergySavings, (object) str5);
        this.RenderSummaryListItem(C1doc3, text3);
        dataScaler1.SetScaler(ReportBase.EnglishUnits ? num8 / 1.10231 : num8, Units.MetricTons, Units.None);
        this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.CarbonAvoided, (object) string.Format("{0}/{2} ({1}/{2})", (object) dataScaler1.GetPhraseOfScaledValueWithUnit(ReportBase.EnglishUnits ? num8 / 1.10231 : num8, ReportBase.EnglishUnits), (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCurrencySymbolValue, (object) this.CurrencySymbol, (object) ReportUtil.RoundToSignificantFigures(energyValues7 + energyValues8, 3)), (object) v6Strings.Year_SingularName.ToLower())));
      }
      else
      {
        this.RenderSummaryListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.BuildingEnergySavingsNA);
        this.RenderSummaryListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.AvoidedCarbonEmissionNA);
      }
      this.RenderSummaryListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.FmtFieldValue, (object) i_Tree_Eco_v6.Resources.Strings.ReplacementValues, (object) string.Format(i_Tree_Eco_v6.Resources.Strings.FmtCurrencySymbolValue, (object) this.CurrencySymbol, (object) ReportUtil.GetNumericWordWithMagnitude(structuralValue, 3))));
      this.rText = new RenderText(ReportBase.EnglishUnits ? i_Tree_Eco_v6.Resources.Strings.TonDefinition : i_Tree_Eco_v6.Resources.Strings.TonneDefinition);
      this.rText.Style.Spacing.Top = (Unit) "1ls";
      this.rText.Style.Parents = this.styleIndentedTinyText;
      C1doc.Body.Children.Add((RenderObject) this.rText);
      this.rText = new RenderText(string.Format(i_Tree_Eco_v6.Resources.Strings.WR_NoteMonetaryValues, (object) this.CurrencySymbol, (object) this.CurrencyName));
      this.rText.Style.Parents = this.styleIndentedTinyText;
      C1doc.Body.Children.Add((RenderObject) this.rText);
      this.rText = this.series.SampleType != SampleType.RegularPlot || !this.curYear.RecordShrub ? new RenderText(i_Tree_Eco_v6.Resources.Strings.WR_NoteEcosystemServiceEst) : new RenderText(i_Tree_Eco_v6.Resources.Strings.WR_NotePollutionRemoval);
      this.rText.Style.Parents = this.styleIndentedTinyText;
      C1doc.Body.Children.Add((RenderObject) this.rText);
      bool flag1 = this.locAndParents[0].TropicalClimate.HasValue && this.ProjectIsUsingTropicalEquations();
      if (flag1)
      {
        this.rText = new RenderText(i_Tree_Eco_v6.Resources.Strings.MsgTropicalEquations);
        this.rText.Style.Parents = this.styleIndentedTinyText;
        C1doc.Body.Children.Add((RenderObject) this.rText);
      }
      if (num3 == 0.0 || this.GetTotalLeafAreaSquareKilometers() == 0.0)
      {
        this.AddDefaultParagraph(C1doc, "There are no live trees in this project. Nothing more to report.");
      }
      else
      {
        this.AddDefaultParagraph(C1doc, string.Format("{0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.WR_NoteSeeAppendix1, this.series.SampleType == SampleType.RegularPlot ? (object) i_Tree_Eco_v6.Resources.Strings.WR_NoteSeeAppendix1Addition : (object) string.Empty));
        this.rText = new RenderText(i_Tree_Eco_v6.Resources.Strings.TableOfContents);
        this.rText.Style.Parents = this.styleSectionTitle;
        this.rText.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
        this.rText.BreakBefore = BreakEnum.Page;
        C1doc.Body.Children.Add((RenderObject) this.rText);
        C1doc.Body.Children.Add((RenderObject) this.toc);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_TreeCharacteristicsTitle);
        if (this.curYear.RecordStrata)
        {
          switch (dictionary1.Keys.Count)
          {
            case 1:
              C1PrintDocument C1doc4 = C1doc;
              string format1 = this.series.SampleType == SampleType.Inventory ? i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestFull1 : i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestSample1;
              object[] objArray1 = new object[5]
              {
                (object) this.locationName,
                null,
                null,
                null,
                null
              };
              num9 = this.series.SampleType == SampleType.Inventory ? num3 : ReportUtil.RoundToSignificantFigures(num3, 4);
              objArray1[1] = (object) num9.ToString("N0");
              objArray1[2] = (object) avgCoverPercent.ToString("N1");
              objArray1[3] = (object) SpeciesByCountDesc[0].Item3;
              num9 = SpeciesByCountDesc[0].Item2 / num3 * 100.0;
              objArray1[4] = (object) num9.ToString("N1");
              string s1 = string.Format(format1, objArray1);
              this.AddDefaultParagraph(C1doc4, s1);
              break;
            case 2:
              C1PrintDocument C1doc5 = C1doc;
              string format2 = this.series.SampleType == SampleType.Inventory ? i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestFull2 : i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestSample2;
              object[] objArray2 = new object[7];
              objArray2[0] = (object) this.locationName;
              num9 = this.series.SampleType == SampleType.Inventory ? num3 : ReportUtil.RoundToSignificantFigures(num3, 4);
              objArray2[1] = (object) num9.ToString("N0");
              objArray2[2] = (object) avgCoverPercent.ToString("N1");
              objArray2[3] = (object) SpeciesByCountDesc[0].Item3;
              num9 = SpeciesByCountDesc[0].Item2 / num3 * 100.0;
              objArray2[4] = (object) num9.ToString("N1");
              objArray2[5] = (object) SpeciesByCountDesc[1].Item3;
              num9 = SpeciesByCountDesc[1].Item2 / num3 * 100.0;
              objArray2[6] = (object) num9.ToString("N1");
              string s2 = string.Format(format2, objArray2);
              this.AddDefaultParagraph(C1doc5, s2);
              break;
            default:
              C1PrintDocument C1doc6 = C1doc;
              string format3 = this.series.SampleType == SampleType.Inventory ? i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestFullD : i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestSampleD;
              object[] objArray3 = new object[9];
              objArray3[0] = (object) this.locationName;
              num9 = this.series.SampleType == SampleType.Inventory ? num3 : ReportUtil.RoundToSignificantFigures(num3, 4);
              objArray3[1] = (object) num9.ToString("N0");
              objArray3[2] = (object) avgCoverPercent.ToString("N1");
              objArray3[3] = (object) SpeciesByCountDesc[0].Item3;
              num9 = SpeciesByCountDesc[0].Item2 / num3 * 100.0;
              objArray3[4] = (object) num9.ToString("N1");
              objArray3[5] = (object) SpeciesByCountDesc[1].Item3;
              num9 = SpeciesByCountDesc[1].Item2 / num3 * 100.0;
              objArray3[6] = (object) num9.ToString("N1");
              objArray3[7] = (object) SpeciesByCountDesc[2].Item3;
              num9 = SpeciesByCountDesc[2].Item2 / num3 * 100.0;
              objArray3[8] = (object) num9.ToString("N1");
              string s3 = string.Format(format3, objArray3);
              this.AddDefaultParagraph(C1doc6, s3);
              break;
          }
        }
        else
        {
          dataScaler1.SetScaler(treeCoverHectare, Units.Hectare, Units.None);
          switch (dictionary1.Keys.Count)
          {
            case 1:
              C1PrintDocument C1doc7 = C1doc;
              string format4 = this.series.SampleType == SampleType.Inventory ? i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestOfFull1 : i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestOfSample1;
              object[] objArray4 = new object[5]
              {
                (object) this.locationName,
                null,
                null,
                null,
                null
              };
              num9 = this.series.SampleType == SampleType.Inventory ? num3 : ReportUtil.RoundToSignificantFigures(num3, 4);
              objArray4[1] = (object) num9.ToString("N0");
              objArray4[2] = (object) dataScaler1.GetPhraseOfScaledValueWithUnit(treeCoverHectare, ReportBase.EnglishUnits);
              objArray4[3] = (object) SpeciesByCountDesc[0].Item3;
              num9 = SpeciesByCountDesc[0].Item2 / num3 * 100.0;
              objArray4[4] = (object) num9.ToString("N1");
              string s4 = string.Format(format4, objArray4);
              this.AddDefaultParagraph(C1doc7, s4);
              break;
            case 2:
              C1PrintDocument C1doc8 = C1doc;
              string format5 = this.series.SampleType == SampleType.Inventory ? i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestOfFull2 : i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestOfSample2;
              object[] objArray5 = new object[7];
              objArray5[0] = (object) this.locationName;
              num9 = this.series.SampleType == SampleType.Inventory ? num3 : ReportUtil.RoundToSignificantFigures(num3, 4);
              objArray5[1] = (object) num9.ToString("N0");
              objArray5[2] = (object) dataScaler1.GetPhraseOfScaledValueWithUnit(treeCoverHectare, ReportBase.EnglishUnits);
              objArray5[3] = (object) SpeciesByCountDesc[0].Item3;
              num9 = SpeciesByCountDesc[0].Item2 / num3 * 100.0;
              objArray5[4] = (object) num9.ToString("N1");
              objArray5[5] = (object) SpeciesByCountDesc[1].Item3;
              num9 = SpeciesByCountDesc[1].Item2 / num3 * 100.0;
              objArray5[6] = (object) num9.ToString("N1");
              string s5 = string.Format(format5, objArray5);
              this.AddDefaultParagraph(C1doc8, s5);
              break;
            default:
              C1PrintDocument C1doc9 = C1doc;
              string format6 = this.series.SampleType == SampleType.Inventory ? i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestOfFullD : i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestOfSampleD;
              object[] objArray6 = new object[9];
              objArray6[0] = (object) this.locationName;
              num9 = this.series.SampleType == SampleType.Inventory ? num3 : ReportUtil.RoundToSignificantFigures(num3, 4);
              objArray6[1] = (object) num9.ToString("N0");
              objArray6[2] = (object) dataScaler1.GetPhraseOfScaledValueWithUnit(treeCoverHectare, ReportBase.EnglishUnits);
              objArray6[3] = (object) SpeciesByCountDesc[0].Item3;
              num9 = SpeciesByCountDesc[0].Item2 / num3 * 100.0;
              objArray6[4] = (object) num9.ToString("N1");
              objArray6[5] = (object) SpeciesByCountDesc[1].Item3;
              num9 = SpeciesByCountDesc[1].Item2 / num3 * 100.0;
              objArray6[6] = (object) num9.ToString("N1");
              objArray6[7] = (object) SpeciesByCountDesc[2].Item3;
              num9 = SpeciesByCountDesc[2].Item2 / num3 * 100.0;
              objArray6[8] = (object) num9.ToString("N1");
              string s6 = string.Format(format6, objArray6);
              this.AddDefaultParagraph(C1doc9, s6);
              break;
          }
        }
        RenderObject chartRenderObject = (RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartTreeSpeciesComposition(SpeciesByCountDesc, num3, this.locationName), this.formGraphics, C1doc, 1.0, 0.5);
        C1doc.Body.Children.Add(chartRenderObject);
        if (this.curYear.RecordStrata)
        {
          string overallTreeDensity = i_Tree_Eco_v6.Resources.Strings.WR_OverallTreeDensity;
          string locationName = this.locationName;
          num9 = EstimateUtil.ConvertToEnglish(this.GetOverallTreeDensity(), Tuple.Create<Units, Units, Units>(Units.Count, Units.Hectare, Units.None), ReportBase.EnglishUnits);
          string str6 = num9.ToString("N0");
          string str7 = ReportBase.HectareUnits();
          string s7 = string.Format(overallTreeDensity, (object) locationName, (object) str6, (object) str7);
          DataTable threeTreeDensities = this.GetLanduseTopThreeTreeDensities();
          if (threeTreeDensities.Rows.Count > 1)
            s7 += string.Format(" {0}", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.WR_ForStratifiedProjects, (object) this.locationName, (object) this.GetLanduseTopThreeTreeDensitiesString(threeTreeDensities)));
          this.AddDefaultParagraph(C1doc, s7);
        }
        if (this.curYear.RecordStrata)
          C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartTreesPerAreaByLandUse(ReportBase.EnglishUnits, this.locationName), this.formGraphics, C1doc, 1.0, 0.38));
        else
          C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartTreesByLandUse(ReportBase.EnglishUnits, this.locationName), this.formGraphics, C1doc, 1.0, 0.38));
        C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartPercentTreePopulationByDBHClass(dbhTreeCount, num3, ReportBase.EnglishUnits), this.formGraphics, C1doc, 1.0, 0.4));
        string estTable = this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
        {
          Classifiers.Strata,
          Classifiers.Continent
        })];
        int valueOrderFromName1 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Continent, this.continent.Name);
        int valueOrderFromName2 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area");
        int valueOrderFromName3 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Continent, "STATE");
        int estUnit = this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)];
        double percentNativeContinent = this.GetPercentNativeContinent();
        double percentNativeState = this.GetPercentNativeState();
        DataTable exoticToContinent1 = this.GetSpeciesPercentExoticToContinent();
        double exoticToContinent2 = this.GetPercentExoticToContinent(exoticToContinent1);
        double highestSourceContinent = this.GetPercentExoticHighestSourceContinent(exoticToContinent1);
        string empty1 = string.Empty;
        if (highestSourceContinent > 0.0)
          empty1 = this.estUtil.ClassValues[Classifiers.Continent][(short) ReportUtil.ConvertFromDBVal<int>((object) exoticToContinent1.Rows[0].Field<int>(this.estUtil.ClassifierNames[Classifiers.Continent]))].Item1;
        if (this.nation.Id == 219)
        {
          if (highestSourceContinent > 0.0)
            this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestsAreComposedUSNative, (object) this.locationName, (object) percentNativeContinent.ToString("N0"), (object) this.continent.Name, (object) percentNativeState.ToString("N0"), (object) this.state.Name, (object) exoticToContinent2.ToString("N0"), (object) empty1, (object) highestSourceContinent.ToString("N0")));
          else
            this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestsAreComposedUSExotic, (object) this.locationName, (object) percentNativeContinent.ToString("N0"), (object) this.continent.Name, (object) percentNativeState.ToString("N0"), (object) this.state.Name, (object) exoticToContinent2.ToString("N0")));
        }
        else if (highestSourceContinent > 0.0)
          this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestsAreComposedInternationalNative, (object) this.locationName, (object) percentNativeContinent.ToString("N0"), (object) this.continent.Name, (object) empty1, (object) highestSourceContinent.ToString("N0")));
        else
          this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestsAreComposedInternationalExotic, new object[3]
          {
            (object) this.locationName,
            (object) percentNativeContinent.ToString("N0"),
            (object) this.continent.Name
          }));
        Tuple<C1.Win.C1Chart.C1Chart, bool> areaOfNativeOrigin = this.CreateChartPercentOfLiveTreePopulationByAreaOfNativeOrigin(this.locationName, this.state);
        int num10 = areaOfNativeOrigin.Item2 ? 1 : 0;
        C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(areaOfNativeOrigin.Item1, this.formGraphics, C1doc, 1.0, 0.4));
        if (num10 != 0)
        {
          this.rText = new RenderText();
          this.rText.Style.Parents = this.styleTinyText;
          this.rText.Text = i_Tree_Eco_v6.Resources.Strings.WR_Figure4PlusSignExplanation;
          C1doc.Body.Children.Add((RenderObject) this.rText);
        }
        IList<(int, double, double)> countsWithLeafArea = this.GetSpeciesCountsWithLeafArea();
        double totalTreeCount = countsWithLeafArea.Sum<(int, double, double)>((Func<(int, double, double), double>) (r => r.TreeNo));
        double totalLeafArea = countsWithLeafArea.Sum<(int, double, double)>((Func<(int, double, double), double>) (r => r.LeafArea));
        SortedList<double, InvasiveSpecies> invasiveSpeciesList = this.GetInvasiveSpeciesList(countsWithLeafArea, totalTreeCount, totalLeafArea);
        double num11 = invasiveSpeciesList.Sum<KeyValuePair<double, InvasiveSpecies>>((Func<KeyValuePair<double, InvasiveSpecies>, double>) (r => r.Value.PercentTreeNumber));
        string s8 = i_Tree_Eco_v6.Resources.Strings.WR_InvasivePlantSpeciesBase;
        if (this.nation.Id == 219)
        {
          string str8 = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_InvasivePlantSpecies0, (object) ReportUtil.UpperCaseFirstLetter(ReportUtil.NumberToWords(invasiveSpeciesList.Count)), (object) dictionary1.Count, (object) this.locationName, (object) this.resmanStateInvasives.GetString(this.state.Name + "_InText"));
          switch (invasiveSpeciesList.Count)
          {
            case 0:
              s8 += string.Format(" {0}", (object) str8);
              break;
            case 1:
              string str9 = s8;
              string str10 = str8;
              string invasivePlantSpecies1 = i_Tree_Eco_v6.Resources.Strings.WR_InvasivePlantSpecies1;
              string invaisiveSpeciesName = this.GetInvaisiveSpeciesName(invasiveSpeciesList, 0);
              num9 = invasiveSpeciesList.Values[0].PercentTreeNumber;
              string str11 = num9.ToString("N1");
              string str12 = string.Format(invasivePlantSpecies1, (object) invaisiveSpeciesName, (object) str11);
              string str13 = string.Format(" {0} {1}", (object) str10, (object) str12);
              s8 = str9 + str13;
              break;
            case 2:
              string str14 = s8;
              string str15 = str8;
              string invasivePlantSpecies2 = i_Tree_Eco_v6.Resources.Strings.WR_InvasivePlantSpecies2;
              object[] objArray7 = new object[5]
              {
                (object) num11.ToString("N1"),
                (object) this.GetInvaisiveSpeciesName(invasiveSpeciesList, 0),
                null,
                null,
                null
              };
              num9 = invasiveSpeciesList.Values[0].PercentTreeNumber;
              objArray7[2] = (object) num9.ToString("N1");
              objArray7[3] = (object) this.GetInvaisiveSpeciesName(invasiveSpeciesList, 1);
              num9 = invasiveSpeciesList.Values[1].PercentTreeNumber;
              objArray7[4] = (object) num9.ToString("N1");
              string str16 = string.Format(invasivePlantSpecies2, objArray7);
              string str17 = string.Format(" {0} {1}", (object) str15, (object) str16);
              s8 = str14 + str17;
              break;
            case 3:
              string str18 = s8;
              string str19 = str8;
              string invasivePlantSpecies3 = i_Tree_Eco_v6.Resources.Strings.WR_InvasivePlantSpecies3;
              object[] objArray8 = new object[7];
              objArray8[0] = (object) num11.ToString("N1");
              objArray8[1] = (object) this.GetInvaisiveSpeciesName(invasiveSpeciesList, 0);
              num9 = invasiveSpeciesList.Values[0].PercentTreeNumber;
              objArray8[2] = (object) num9.ToString("N1");
              objArray8[3] = (object) this.GetInvaisiveSpeciesName(invasiveSpeciesList, 1);
              num9 = invasiveSpeciesList.Values[1].PercentTreeNumber;
              objArray8[4] = (object) num9.ToString("N1");
              objArray8[5] = (object) this.GetInvaisiveSpeciesName(invasiveSpeciesList, 2);
              num9 = invasiveSpeciesList.Values[2].PercentTreeNumber;
              objArray8[6] = (object) num9.ToString("N1");
              string str20 = string.Format(invasivePlantSpecies3, objArray8);
              string str21 = string.Format(" {0} {1}", (object) str19, (object) str20);
              s8 = str18 + str21;
              break;
            default:
              string str22 = s8;
              string str23 = str8;
              string invasivePlantSpeciesD = i_Tree_Eco_v6.Resources.Strings.WR_InvasivePlantSpeciesD;
              object[] objArray9 = new object[7];
              objArray9[0] = (object) num11.ToString("N1");
              objArray9[1] = (object) this.GetInvaisiveSpeciesName(invasiveSpeciesList, 0);
              num9 = invasiveSpeciesList.Values[0].PercentTreeNumber;
              objArray9[2] = (object) num9.ToString("N1");
              objArray9[3] = (object) this.GetInvaisiveSpeciesName(invasiveSpeciesList, 1);
              num9 = invasiveSpeciesList.Values[1].PercentTreeNumber;
              objArray9[4] = (object) num9.ToString("N1");
              objArray9[5] = (object) this.GetInvaisiveSpeciesName(invasiveSpeciesList, 2);
              num9 = invasiveSpeciesList.Values[2].PercentTreeNumber;
              objArray9[6] = (object) num9.ToString("N1");
              string str24 = string.Format(invasivePlantSpeciesD, objArray9);
              string str25 = string.Format(" {0} {1}", (object) str23, (object) str24);
              s8 = str22 + str25;
              break;
          }
        }
        this.AddDefaultParagraph(C1doc, s8);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestCoverAndLeafArea);
        List<LanduseLeafArea> landuseLeafAreas = this.GetLanduseLeafAreas();
        string str26 = landuseLeafAreas.Count <= 2 ? (landuseLeafAreas.Count != 2 ? landuseLeafAreas[0].Landuse : string.Format(i_Tree_Eco_v6.Resources.Strings.WR_landuseTrailingStringTwo, (object) landuseLeafAreas[0].Landuse, (object) landuseLeafAreas[1].Landuse)) : string.Format(i_Tree_Eco_v6.Resources.Strings.WR_landuseTrailingStringMulti, (object) landuseLeafAreas[0].Landuse, (object) landuseLeafAreas[1].Landuse, (object) landuseLeafAreas[2].Landuse);
        DataScaler dataScaler2 = new DataScaler(treeCoverHectare, Units.Hectare, Units.None);
        double squareKilometers = this.GetTotalLeafAreaSquareKilometers();
        DataScaler dataScaler3 = new DataScaler(squareKilometers, Units.Squarekilometer, Units.None);
        this.rText = new RenderText();
        this.rText.Style.Parents = this.styleDefaultParagraph;
        this.rText.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_UrbanForestCoverP1, (object) (this.curYear.RecordStrata ? ReportUtil.RoundToSignificantFigures(avgCoverPercent, 2) : ReportUtil.RoundToSignificantFigures(dataScaler2.GetScaledValue(treeCoverHectare, ReportBase.EnglishUnits), 4)), this.curYear.RecordStrata ? (object) i_Tree_Eco_v6.Resources.Strings.Percent.ToLower() : (object) ReportUtil.PluralizeLast(dataScaler2.GetPrefixPlusUnitDescription(ReportBase.EnglishUnits)), (object) this.locationName, (object) ReportUtil.RoundToSignificantFigures(dataScaler3.GetScaledValue(squareKilometers, ReportBase.EnglishUnits), 4), (object) ReportUtil.PluralizeLast(dataScaler3.GetPrefixPlusUnitDescription(ReportBase.EnglishUnits)), this.curYear.RecordStrata ? (object) string.Format(i_Tree_Eco_v6.Resources.Strings.WR_TotalLeafAreaIsGreatestIn, (object) str26) : (object) string.Empty);
        C1doc.Body.Children.Add((RenderObject) this.rText);
        C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartLeafAreaByStrata(landuseLeafAreas, this.locationName, this.YearGuid, ReportBase.EnglishUnits), this.formGraphics, C1doc, 1.0, 0.4));
        List<ImportanceValueSpecies> importanceValues = this.GetImportanceValues(countsWithLeafArea, totalTreeCount, totalLeafArea);
        importanceValues.Sort((IComparer<ImportanceValueSpecies>) new LeafAreaSpeciesSorterDescending());
        string empty2 = string.Empty;
        string str27 = importanceValues.Count <= 2 ? (importanceValues.Count <= 1 ? string.Format(i_Tree_Eco_v6.Resources.Strings.WR_MostImportantSpeciesSentence1, (object) this.GetSpeciesName(importanceValues[0], ReportBase.ScientificName)) : string.Format(i_Tree_Eco_v6.Resources.Strings.WR_MostImportantSpeciesSentence2, (object) this.GetSpeciesName(importanceValues[0], ReportBase.ScientificName), (object) this.GetSpeciesName(importanceValues[1], ReportBase.ScientificName))) : string.Format(i_Tree_Eco_v6.Resources.Strings.WR_MostImportantSpeciesSentenceMulti, (object) this.GetSpeciesName(importanceValues[0], ReportBase.ScientificName), (object) this.GetSpeciesName(importanceValues[1], ReportBase.ScientificName), (object) this.GetSpeciesName(importanceValues[2], ReportBase.ScientificName));
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_MostDominantSpecies, (object) this.locationName, (object) str27, (object) (count1 > 10 ? 10 : count1), (object) ReportUtil.GetIsAreString(count1 > 1)));
        this.ra = new RenderArea();
        C1doc.Style.FlowAlignChildren = FlowAlignEnum.Center;
        this.ra.Width = (Unit) "80%";
        this.ra.Children.Add((RenderObject) this.AddTableTitle(string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Table1Title, (object) this.locationName)));
        RenderTable renderTable1 = new RenderTable();
        renderTable1.Name = "table1";
        int count2 = 1;
        renderTable1.RowGroups[0, count2].Header = TableHeaderEnum.Page;
        ReportUtil.AddWrittenReportTableHeaderFormat(renderTable1);
        renderTable1.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.SpeciesName;
        renderTable1.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.PercentNewLinePopulation;
        renderTable1.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.PercentNewLineLeafArea;
        renderTable1.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.ImportanceValues;
        renderTable1.Cols[0].Width = (Unit) "50%";
        renderTable1.Cols[1].Width = (Unit) "16%";
        renderTable1.Cols[2].Width = (Unit) "16%";
        renderTable1.Cols[3].Width = (Unit) "16%";
        renderTable1.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
        renderTable1.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        importanceValues.Sort((IComparer<ImportanceValueSpecies>) new ImportanceValueSpeciesSorter());
        int row1 = count2;
        for (int index = 0; index < importanceValues.Count && index < 10; ++index)
        {
          renderTable1.Cells[row1, 0].Text = this.GetSpeciesName(importanceValues[index], ReportBase.ScientificName);
          TableCell cell1 = renderTable1.Cells[row1, 1];
          num9 = importanceValues[index].PercentTreeNumber;
          string str28 = num9.ToString("N1");
          cell1.Text = str28;
          TableCell cell2 = renderTable1.Cells[row1, 2];
          num9 = importanceValues[index].PercentLeafArea;
          string str29 = num9.ToString("N1");
          cell2.Text = str29;
          TableCell cell3 = renderTable1.Cells[row1, 3];
          num9 = importanceValues[index].ImportanceValue;
          string str30 = num9.ToString("N1");
          cell3.Text = str30;
          ++row1;
        }
        ReportUtil.FormatRenderTableWrittenReport(renderTable1);
        this.ra.Children.Add((RenderObject) renderTable1);
        C1doc.Body.Children.Add((RenderObject) this.ra);
        List<GroundCoverPercent> excludingTreesAndShrubs = this.GetGroundCoversExcludingTreesAndShrubs();
        List<string> grCovers1 = new List<string>();
        List<string> grCovers2 = new List<string>();
        List<string> grCovers3 = new List<string>();
        foreach (GroundCoverPercent groundCoverPercent in excludingTreesAndShrubs)
        {
          string name = groundCoverPercent.Name;
          if (name.Equals("cement", StringComparison.InvariantCultureIgnoreCase))
            grCovers2.Add(name);
          else if (name.Equals("tar", StringComparison.InvariantCultureIgnoreCase))
            grCovers2.Add(name);
          else if (name.Equals("other", StringComparison.InvariantCultureIgnoreCase))
            grCovers2.Add(name);
          else if (name.Equals("herbs", StringComparison.InvariantCultureIgnoreCase))
            grCovers3.Add(name);
          else if (name.Equals("grass", StringComparison.InvariantCultureIgnoreCase))
            grCovers3.Add(name);
          else if (name.Equals("wild grass", StringComparison.InvariantCultureIgnoreCase))
            grCovers3.Add(name);
          else if (name.Equals("building", StringComparison.InvariantCultureIgnoreCase))
            grCovers1.Add("buildings");
          else
            grCovers1.Add(name);
        }
        this.StartOnNewPage(C1doc);
        bool flag2 = grCovers1.Count > 0;
        bool flag3 = grCovers2.Count > 0;
        bool flag4 = grCovers3.Count > 0;
        string str31 = string.Format("{0}{1}{2}{3}{4}", (object) this.GetGroundCoversStr(grCovers1), !flag2 || !(flag3 | flag4) ? (object) string.Empty : (object) ", ", flag3 ? (object) string.Format(i_Tree_Eco_v6.Resources.Strings.WR_ImperviousCoversSuchAs, (object) this.GetGroundCoversStr(grCovers2)) : (object) string.Empty, flag2 | flag3 ? (object) string.Format(", {0} ", (object) i_Tree_Eco_v6.Resources.Strings.And) : (object) string.Empty, flag4 ? (object) string.Format(i_Tree_Eco_v6.Resources.Strings.WR_HerbaceousCoversSuchAs, (object) this.GetGroundCoversStr(grCovers3)) : (object) string.Empty);
        bool multi = excludingTreesAndShrubs.Count > 1;
        string groundCoverPercentStr = this.GetGroundCoverPercentStr(excludingTreesAndShrubs[0]);
        if (this.curYear.RecordGroundCover)
        {
          this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_CommonGroundCoverClasses, (object) this.locationName, (object) str31, multi ? (object) i_Tree_Eco_v6.Resources.Strings.WR_Types : (object) i_Tree_Eco_v6.Resources.Strings.WR_Type, (object) ReportUtil.GetIsAreString(multi), multi ? (object) string.Format("{0} {1} {2}", (object) groundCoverPercentStr, (object) i_Tree_Eco_v6.Resources.Strings.And, (object) this.GetGroundCoverPercentStr(excludingTreesAndShrubs[1])) : (object) groundCoverPercentStr));
          C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartPercentOfLandByGroundCoverClasses(excludingTreesAndShrubs, this.locationName), this.formGraphics, C1doc, 1.0, 0.5));
        }
        else
        {
          this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_CommonGroundCoverClassesDefault, (object) this.locationName));
          C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartPercentOfLandByGroundCoverClasses(new List<GroundCoverPercent>()
          {
            new GroundCoverPercent()
            {
              Name = v6Strings.TreeStatus_Unknown,
              PercentCover = 100.0
            }
          }, this.locationName), this.formGraphics, C1doc, 1.0, 0.4));
        }
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_AirPollutionRemovalByUrbanTrees);
        List<KeyValuePair<string, double>> keyValuePairList = new List<KeyValuePair<string, double>>();
        keyValuePairList.Add(new KeyValuePair<string, double>("carbon monoxide", pollutionRemovalAmount2));
        keyValuePairList.Add(new KeyValuePair<string, double>("nitrogen dioxide", pollutionRemovalAmount3));
        keyValuePairList.Add(new KeyValuePair<string, double>("ozone", pollutionRemovalAmount1));
        keyValuePairList.Add(new KeyValuePair<string, double>("PM2.5", pollutionRemovalAmount4));
        keyValuePairList.Add(new KeyValuePair<string, double>("PM10*", pollutionRemovalAmount5));
        keyValuePairList.Add(new KeyValuePair<string, double>("sulfur dioxide", pollutionRemovalAmount6));
        keyValuePairList.Sort((Comparison<KeyValuePair<string, double>>) ((item1, item2) => item1.Value > item2.Value ? -1 : 1));
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_AirPollutionRemovalByUrbanTreesP1);
        this.rParagraph = new RenderParagraph();
        this.rParagraph.Style.Parents = this.styleDefaultParagraph;
        this.pText = new ParagraphText();
        this.pText.Text = i_Tree_Eco_v6.Resources.Strings.Pollution_removal;
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        this.AddSubscript(this.rParagraph, "1");
        bool flag5 = this.GetShrubCoverPercent() > 0.0;
        this.pText = new ParagraphText();
        this.pText.Text = string.Format(" {0}", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.WR_PollutionRemovalByTreesP2Part1, flag5 ? (object) string.Format("{0} {1} ", (object) i_Tree_Eco_v6.Resources.Strings.And, (object) i_Tree_Eco_v6.Resources.Strings.Shrubs) : (object) string.Empty, (object) this.locationName));
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        this.pText = new ParagraphText();
        this.pText.Text = string.Format(" {0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.And, (object) i_Tree_Eco_v6.Resources.Strings.Weather);
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        dataScaler1.SetScaler(pollutionRemovalAmount7, Units.MetricTons, Units.None);
        this.pText = new ParagraphText();
        this.pText.Text = string.Format(" {0}", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.WR_PollutionRemovalByTreesP2Part2, (object) keyValuePairList[0].Key, flag5 ? (object) string.Format("{0} {1} ", (object) i_Tree_Eco_v6.Resources.Strings.And, (object) i_Tree_Eco_v6.Resources.Strings.Shrubs) : (object) string.Empty, (object) dataScaler1.GetPhraseOfScaledValueWithUnit(pollutionRemovalAmount7, ReportBase.EnglishUnits)));
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        this.AddSubscript(this.rParagraph, "2");
        this.pText = new ParagraphText();
        this.pText.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_PollutionRemovalByTreesP2Part3, (object) this.CurrencySymbol, (object) ReportUtil.GetNumericWordWithMagnitude(pollutionRemovalDollars, 3));
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        C1doc.Body.Children.Add((RenderObject) this.rParagraph);
        C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartPollutionRemovalAndCost(pollutionRemovalAmount2, this.customizedCoDollarsPerTon, pollutionRemovalAmount3, this.customizedNO2DollarsPerTon, pollutionRemovalAmount1, this.customizedO3DollarsPerTon, pollutionRemovalAmount4, this.customizedPM25DollarsPerTon, pollutionRemovalAmount5, this.customizedPM10DollarsPerTon, pollutionRemovalAmount6, this.customizedSO2DollarsPerTon, this.locationName, ReportBase.EnglishUnits, this.CurrencySymbol), this.formGraphics, C1doc, 1.0, 0.4));
        this.AddAboveFooterLine(C1doc);
        this.rParagraph = new RenderParagraph();
        this.rParagraph.Style.Parents = this.styleTinyText;
        this.AddSubscript(this.rParagraph, "1");
        this.pText = new ParagraphText();
        this.pText.Text = string.Format(" {0}", (object) (i_Tree_Eco_v6.Resources.Strings.WR_PollutionRemovalByTreesP2FootNote1 + Environment.NewLine + Environment.NewLine));
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        this.AddSubscript(this.rParagraph, "2");
        this.pText = new ParagraphText();
        this.pText.Text = string.Format(" {0}", (object) i_Tree_Eco_v6.Resources.Strings.WR_PollutionRemovalByTreesP2FootNote2);
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        this.rParagraph.Y = (Unit) "page.bottom - self.height";
        C1doc.Body.Children.Add((RenderObject) this.rParagraph);
        DataTable pollutionByStratum = this.GetPollutionByStratum();
        double num12 = 0.0;
        double num13 = 0.0;
        int valueOrderFromName4 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.VOCs, "Monoterpene");
        int valueOrderFromName5 = (int) this.estUtil.GetClassValueOrderFromName(Classifiers.VOCs, "Isoprene");
        foreach (DataRow row2 in (InternalDataCollectionBase) pollutionByStratum.Rows)
        {
          int num14 = (int) row2[this.estUtil.ClassifierNames[Classifiers.VOCs]];
          double num15 = (double) row2["sumEstimate"];
          if (num14 == valueOrderFromName4)
            num13 = num15;
          else if (num14 == valueOrderFromName5)
            num12 = num15;
        }
        DataTable pollutantSpecies = this.GetTopTwoPollutantSpecies();
        string sp = this.estUtil.ClassifierNames[Classifiers.Species];
        string speciesName = this.GetSpeciesName((short) (int) pollutantSpecies.Rows[0][sp]);
        double num16 = (double) pollutantSpecies.Rows[0]["sumEstimate"];
        if (pollutantSpecies.Rows.Count > 1)
        {
          speciesName += string.Format(" {0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.And, (object) this.GetSpeciesName((short) (int) pollutantSpecies.Rows[1][sp]));
          num16 += (double) pollutantSpecies.Rows[1]["sumEstimate"];
        }
        double valueWithOriginalUnit1 = (num12 + num13) / 1000.0;
        double valueWithOriginalUnit2 = num12 / 1000.0;
        double valueWithOriginalUnit3 = num13 / 1000.0;
        double num17 = num16 / 1000.0;
        dataScaler1.SetScaler(valueWithOriginalUnit1, Units.MetricTons, Units.None);
        this.rText = new RenderText();
        this.rText.Style.Parents = this.styleDefaultParagraph;
        this.rText.Text = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_PollutionRemovalByTreesP3, (object) this.curYear.Id, (object) this.locationName, (object) dataScaler1.GetPhraseOfScaledValueWithUnit(valueWithOriginalUnit1, ReportBase.EnglishUnits), (object) dataScaler1.GetPhraseOfScaledValueWithUnit(valueWithOriginalUnit2, ReportBase.EnglishUnits), (object) dataScaler1.GetPhraseOfScaledValueWithUnit(valueWithOriginalUnit3, ReportBase.EnglishUnits), (object) ReportUtil.UpperCaseFirstLetter(ReportUtil.NumberToWords(valueWithOriginalUnit1 == 0.0 ? 0 : (int) Math.Round(num17 / valueWithOriginalUnit1 * 100.0))), (object) speciesName);
        this.rText.BreakBefore = BreakEnum.Page;
        C1doc.Body.Children.Add((RenderObject) this.rText);
        this.rText = new RenderText();
        this.rText.Style.Parents = this.styleDefaultParagraph;
        this.rText.Text = i_Tree_Eco_v6.Resources.Strings.WR_PollutionRemovalByTreesP4;
        C1doc.Body.Children.Add((RenderObject) this.rText);
        this.AddAboveAsteriskFooterNote(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_PollutionRemovalByTreesP4FooterNote);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_CarbonStorageAndSequestration);
        this.rText = new RenderText();
        this.rText.Style.Parents = this.styleDefaultParagraph;
        this.rText.Text = i_Tree_Eco_v6.Resources.Strings.WR_CarbonStorageAndSequestrationP1;
        C1doc.Body.Children.Add((RenderObject) this.rText);
        DataTable carbonData = this.GetCarbonData();
        double valueWithOriginalUnit4 = carbonData.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (r => r.Field<double>("sumEstimate")));
        dataScaler1.SetScaler(carbonSequestration, Units.MetricTons, Units.None);
        DataScaler dataScaler4 = new DataScaler(valueWithOriginalUnit4, Units.MetricTons, Units.None);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_CarbonStorageAndSequestrationP2, (object) this.locationName, (object) dataScaler1.GetPhraseOfScaledValueWithUnit(carbonSequestration, ReportBase.EnglishUnits), (object) this.CurrencySymbol, (object) ReportUtil.GetNumericWordWithMagnitude(carbonSequestration * this.customizedCarbonDollarsPerTon, 3), this.series.SampleType == SampleType.RegularPlot ? (object) string.Format(" {0}", (object) string.Format(i_Tree_Eco_v6.Resources.Strings.WR_NetCarbonSequestrationP2Part2, (object) dataScaler4.GetPhraseOfScaledValueWithUnit(valueWithOriginalUnit4, ReportBase.EnglishUnits))) : (object) string.Empty));
        DataTable sequestrationData = this.GetGrossCarbonSequestrationData();
        List<KeyValuePair<int, double>> topN1 = this.GetTopN(sequestrationData, 10);
        Dictionary<int, double> dictionary2 = sequestrationData.AsEnumerable().ToDictionary<DataRow, int, double>((Func<DataRow, int>) (r => r.Field<int>(sp)), (Func<DataRow, double>) (r => r.Field<double>("sumEstimate")));
        C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartAnnualCarbonSequestration(topN1, this.customizedCarbonDollarsPerTon, this.locationName, ReportBase.EnglishUnits, ReportBase.ScientificName, this.CurrencySymbol), this.formGraphics, C1doc, 1.0, 0.6));
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_CarbonStorageAndSequestrationP3);
        DataTable dataTable = this.CarbonStorage();
        List<KeyValuePair<int, double>> topN2 = this.GetTopN(dataTable, 10);
        dataTable.AsEnumerable().ToDictionary<DataRow, int, double>((Func<DataRow, int>) (r => r.Field<int>(sp)), (Func<DataRow, double>) (r => r.Field<double>("sumEstimate")));
        string str32 = string.Format(i_Tree_Eco_v6.Resources.Strings.WR_NetCarbonSequestrationP4Part1, (object) this.locationName, (object) ReportUtil.RoundToSignificantFigures(EstimateUtil.ConvertToEnglish(carbonStorage, Units.MetricTons, ReportBase.EnglishUnits), 3), (object) ReportBase.TonnesUnits(), (object) this.CurrencySymbol, (object) ReportUtil.GetNumericWordWithMagnitude(carbonStorage * this.customizedCarbonDollarsPerTon, 3));
        string str33 = ReportBase.ScientificName ? this.estUtil.ClassValues[Classifiers.Species][(short) topN2[0].Key].Item2 : this.estUtil.ClassValues[Classifiers.Species][(short) topN2[0].Key].Item1;
        KeyValuePair<int, double> keyValuePair1 = topN2[0];
        double significantFigures1 = ReportUtil.RoundToSignificantFigures(keyValuePair1.Value / carbonStorage * 100.0, 3);
        keyValuePair1 = topN2[0];
        int key1 = keyValuePair1.Key;
        keyValuePair1 = topN1[0];
        int key2 = keyValuePair1.Key;
        if (key1 == key2)
        {
          C1PrintDocument C1doc10 = C1doc;
          string str34 = str32;
          string sequestrationP4Part2Opt1 = i_Tree_Eco_v6.Resources.Strings.WR_NetCarbonSequestrationP4Part2Opt1;
          string str35 = str33;
          // ISSUE: variable of a boxed type
          __Boxed<double> local = (System.ValueType) significantFigures1;
          Dictionary<int, double> dictionary3 = dictionary2;
          keyValuePair1 = topN2[0];
          int key3 = keyValuePair1.Key;
          // ISSUE: variable of a boxed type
          __Boxed<double> significantFigures2 = (System.ValueType) ReportUtil.RoundToSignificantFigures(dictionary3[key3] / Enumerable.Sum(dictionary2.Values) * 100.0, 3);
          string str36 = string.Format(sequestrationP4Part2Opt1, (object) str35, (object) local, (object) significantFigures2);
          string s9 = string.Format("{0} {1}", (object) str34, (object) str36);
          this.AddDefaultParagraph(C1doc10, s9);
        }
        else
        {
          C1PrintDocument C1doc11 = C1doc;
          string str37 = str32;
          string sequestrationP4Part2Opt2 = i_Tree_Eco_v6.Resources.Strings.WR_NetCarbonSequestrationP4Part2Opt2;
          object[] objArray10 = new object[4]
          {
            (object) str33,
            (object) significantFigures1,
            null,
            null
          };
          keyValuePair1 = topN1[0];
          objArray10[2] = (object) this.GetSpeciesName((short) keyValuePair1.Key);
          keyValuePair1 = topN1[0];
          objArray10[3] = (object) ReportUtil.RoundToSignificantFigures(keyValuePair1.Value / Enumerable.Sum(dictionary2.Values) * 100.0, 3);
          string str38 = string.Format(sequestrationP4Part2Opt2, objArray10);
          string s10 = string.Format("{0} {1}", (object) str37, (object) str38);
          this.AddDefaultParagraph(C1doc11, s10);
        }
        C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartCarbonStorageAndValue(topN2, this.customizedCarbonDollarsPerTon, this.locationName, ReportBase.EnglishUnits, ReportBase.ScientificName, this.CurrencySymbol), this.formGraphics, C1doc, 1.0, 0.6));
        if (flag1)
          this.AddAboveAsteriskFooterNote(C1doc, i_Tree_Eco_v6.Resources.Strings.MsgTropicalEquations);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_OxygenProduction);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_OxygenProductionP1, ReportBase.plotInventory ? (object) i_Tree_Eco_v6.Resources.Strings.WR_NetAnnualOxygen : (object) i_Tree_Eco_v6.Resources.Strings.WR_AnnualOxygen));
        dataScaler1.SetScaler(carbonData.AsEnumerable().Take<DataRow>(10).Sum<DataRow>((Func<DataRow, double>) (r => r.Field<double>("sumEstimate"))) * 32.0 / 12.0, Units.MetricTons, Units.None);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_OxygenProductionP2, (object) this.locationName, (object) dataScaler1.GetPhraseOfScaledValueWithUnit(valueWithOriginalUnit4 * 32.0 / 12.0, ReportBase.EnglishUnits)));
        this.rText = this.AddTableTitle(string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Table2Title, (object) (count1 > 20 ? 20 : count1)));
        C1doc.Body.Children.Add((RenderObject) this.rText);
        RenderTable renderTable2 = new RenderTable();
        int count3 = 2;
        renderTable2.RowGroups[0, count3].Header = TableHeaderEnum.Page;
        ReportUtil.AddWrittenReportTableHeaderFormat(renderTable2);
        renderTable2.Cols[0].Width = (Unit) "28%";
        renderTable2.Cols[1].Width = (Unit) "18%";
        renderTable2.Cols[2].Width = (Unit) "18%";
        renderTable2.Cols[3].Width = (Unit) "18%";
        renderTable2.Cols[4].Width = (Unit) "18%";
        renderTable2.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
        renderTable2.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        List<SpeciesOxygenProduction> productionSpecies = this.GetTop20OxygenProductionSpecies();
        List<double> dataList1 = new List<double>();
        List<double> dataList2 = new List<double>();
        List<double> dataList3 = new List<double>();
        for (int index = 0; index < productionSpecies.Count && index < 20; ++index)
        {
          dataList1.Add(productionSpecies[index].O2);
          dataList2.Add(productionSpecies[index].NetCarbonSequestration);
          dataList3.Add(productionSpecies[index].LeafArea);
        }
        DataScaler dataScaler5 = new DataScaler(dataList1, Units.MetricTons, Units.None);
        DataScaler dataScaler6 = new DataScaler(dataList2, Units.MetricTons, Units.None);
        DataScaler dataScaler7 = new DataScaler(dataList3, Units.Squarekilometer, Units.None);
        renderTable2.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Species;
        renderTable2.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.Oxygen;
        renderTable2.Cells[1, 1].Text = ReportUtil.FormatHeaderUnitsStr(dataScaler5.GetPrefixPlusUnitDescription(ReportBase.EnglishUnits));
        if (this.series.SampleType == SampleType.Inventory)
        {
          renderTable2.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.GrossCarbonNewLineSequestration;
          renderTable2.Cells[1, 2].Text = ReportUtil.GetFormattedValuePerYrStr(dataScaler6.GetPrefixPlusUnitDescription(ReportBase.EnglishUnits));
        }
        else
        {
          renderTable2.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.NetCarbonNewLineSequestration;
          renderTable2.Cells[1, 2].Text = ReportUtil.GetFormattedValuePerYrStr(dataScaler6.GetPrefixPlusUnitDescription(ReportBase.EnglishUnits));
        }
        renderTable2.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
        renderTable2.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.LeafArea;
        renderTable2.Cells[1, 4].Text = ReportUtil.FormatHeaderUnitsStr(dataScaler7.GetPrefixPlusUnitDescription(ReportBase.EnglishUnits));
        int row3 = count3;
        for (int index = 0; index < productionSpecies.Count && index < 20; ++index)
        {
          renderTable2.Cells[row3, 0].Text = productionSpecies[index].SppName;
          renderTable2.Cells[row3, 1].Text = dataScaler5.GetScaledValue(productionSpecies[index].O2, ReportBase.EnglishUnits).ToString("N2");
          renderTable2.Cells[row3, 2].Text = dataScaler6.GetScaledValue(productionSpecies[index].NetCarbonSequestration, ReportBase.EnglishUnits).ToString("N2");
          renderTable2.Cells[row3, 3].Text = productionSpecies[index].NumberOfTrees.ToString("N0");
          renderTable2.Cells[row3, 4].Text = dataScaler7.GetScaledValue(productionSpecies[index].LeafArea, ReportBase.EnglishUnits).ToString("N2");
          ++row3;
        }
        ReportUtil.FormatRenderTableWrittenReport(renderTable2);
        C1doc.Body.Children.Add((RenderObject) renderTable2);
        if (this.series.SampleType != SampleType.Inventory)
          this.AddAboveAsteriskFooterNote(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_CarbonStorageAndSequestrationFootNote);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_AvoidedRunoff);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_AvoidedRunoffP1);
        C1PrintDocument C1doc12 = C1doc;
        string wrAvoidedRunoffP2 = i_Tree_Eco_v6.Resources.Strings.WR_AvoidedRunoffP2;
        object[] objArray11 = new object[8]
        {
          (object) this.locationName,
          (object) ReportUtil.GetNumericWordWithMagnitude(EstimateUtil.ConvertToEnglish(avoidedRunoff.Item1, Units.CubicMeter, ReportBase.EnglishUnits), 3),
          (object) ReportBase.CubicMeterUnitsV(),
          (object) ReportUtil.GetNumericWordWithMagnitude(avoidedRunoff.Item2, 2),
          this.curYear.YearLocationData.Count <= 0 || string.IsNullOrEmpty(this.curYear.YearLocationData.First<YearLocationData>().WeatherStationId) ? (object) i_Tree_Eco_v6.Resources.Strings.WR_YearUnknown : (object) this.curYear.YearLocationData.First<YearLocationData>().WeatherYear.ToString(),
          null,
          null,
          null
        };
        double num18 = ReportBase.EnglishUnits ? this.yearlyPrecipitationMeters * 39.3700787559 : this.yearlyPrecipitationMeters * 100.0;
        objArray11[5] = (object) num18.ToString("N1");
        objArray11[6] = (object) ReportBase.CentimeterUnits();
        objArray11[7] = (object) this.CurrencySymbol;
        string s11 = string.Format(wrAvoidedRunoffP2, objArray11);
        this.AddDefaultParagraph(C1doc12, s11);
        C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartAvoidedRunoffBySpecies(this.GetTopAvoidedRunoffBySpecies(), this.customizedWaterDollarsPerM3, this.locationName, ReportBase.EnglishUnits, ReportBase.ScientificName, this.CurrencySymbol), this.formGraphics, C1doc, 1.0, 0.6));
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_TreesAndBuildingEnergyUse);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_TreesAndBuildingEnergyUseP1);
        double num19 = english1;
        double num20 = english2;
        double num21 = english1 + english2;
        string unit = ReportBase.TonnesUnits();
        if (Math.Abs(num21) < 1.0)
        {
          num19 = ReportBase.EnglishUnits ? 2000.0 * num19 : 1000.0 * num19;
          num20 = ReportBase.EnglishUnits ? 2000.0 * num20 : 1000.0 * num20;
          num21 = ReportBase.EnglishUnits ? 2000.0 * num21 : 1000.0 * num21;
          unit = ReportBase.KilogramsUnits();
        }
        if (this.curYear.RecordEnergy)
        {
          C1PrintDocument C1doc13 = C1doc;
          string buildingEnergyUseP2 = i_Tree_Eco_v6.Resources.Strings.WR_TreesAndBuildingEnergyUseP2;
          object[] objArray12 = new object[6];
          objArray12[0] = (object) this.locationName;
          num18 = ReportUtil.RoundToSignificantFigures(energyValues4 + energyValues5 + energyValues6, 3);
          objArray12[1] = (object) num18.ToString("N0");
          num18 = ReportUtil.RoundToSignificantFigures(energyValues7 + energyValues8, 3);
          objArray12[2] = (object) num18.ToString("N0");
          objArray12[3] = (object) ReportUtil.RoundToSignificantFigures(num21, 3);
          objArray12[4] = (object) unit;
          objArray12[5] = (object) this.CurrencySymbol;
          string s12 = string.Format(buildingEnergyUseP2, objArray12);
          this.AddDefaultParagraph(C1doc13, s12);
          this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_TreesAndBuildingEnergyUseNote);
        }
        else
          this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_TreesAndBuildingEnergyUseP2NoEnergy);
        this.ra = new RenderArea();
        this.rText = this.AddTableTitle(string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Table3Title, (object) this.locationName));
        this.ra.Children.Add((RenderObject) this.rText);
        RenderTable renderTable3 = new RenderTable();
        renderTable3.Cols[0].Width = (Unit) "40%";
        renderTable3.Cols[1].Width = (Unit) "20%";
        renderTable3.Cols[2].Width = (Unit) "20%";
        renderTable3.Cols[3].Width = (Unit) "20%";
        renderTable3.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
        renderTable3.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        int count4 = 1;
        renderTable3.RowGroups[0, count4].Header = TableHeaderEnum.Page;
        ReportUtil.AddWrittenReportTableHeaderFormat(renderTable3);
        renderTable3.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.Heating;
        renderTable3.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.Cooling;
        renderTable3.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        List<EnergyUsage> energyUsageList1 = new List<EnergyUsage>();
        energyUsageList1.Add(new EnergyUsage()
        {
          Title = i_Tree_Eco_v6.Resources.Strings.UnitMBTU,
          Heating = energyValues1.ToString("N0"),
          Cooling = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr,
          Total = energyValues1.ToString("N0")
        });
        List<EnergyUsage> energyUsageList2 = energyUsageList1;
        EnergyUsage energyUsage1 = new EnergyUsage();
        energyUsage1.Title = i_Tree_Eco_v6.Resources.Strings.UnitMWH;
        energyUsage1.Heating = energyValues2.ToString("N0");
        energyUsage1.Cooling = energyValues3.ToString("N0");
        num18 = energyValues2 + energyValues3;
        energyUsage1.Total = num18.ToString("N0");
        energyUsageList2.Add(energyUsage1);
        energyUsageList1.Add(new EnergyUsage()
        {
          Title = ReportUtil.FormatInlineHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.CarbonAvoided, unit),
          Heating = num19.ToString("N0"),
          Cooling = num20.ToString("N0"),
          Total = num21.ToString("N0")
        });
        int row4 = count4;
        for (int index = 0; index < energyUsageList1.Count; ++index)
        {
          this.rParagraph = new RenderParagraph();
          this.pText = new ParagraphText(energyUsageList1[index].Title);
          this.pText.Style.Parents = this.styleDefaultParagraph;
          this.rParagraph.Content.Add((ParagraphObject) this.pText);
          this.pText = new ParagraphText();
          this.pText.Style.Parents = this.styleDefaultParagraph;
          this.pText.Style.TextPosition = TextPositionEnum.Superscript;
          switch (index)
          {
            case 0:
              this.pText.Text = "a";
              break;
            case 1:
              this.pText.Text = "b";
              break;
          }
          this.rParagraph.Content.Add((ParagraphObject) this.pText);
          renderTable3.Cells[row4, 0].RenderObject = (RenderObject) this.rParagraph;
          renderTable3.Cells[row4, 1].Text = energyUsageList1[index].Heating;
          renderTable3.Cells[row4, 2].Text = energyUsageList1[index].Cooling;
          renderTable3.Cells[row4, 3].Text = energyUsageList1[index].Total;
          ++row4;
        }
        ReportUtil.FormatRenderTableWrittenReport(renderTable3);
        this.ra.Children.Add((RenderObject) renderTable3);
        this.rParagraph = new RenderParagraph();
        this.AddUnitDefinition(this.rParagraph, "a", i_Tree_Eco_v6.Resources.Strings.MBTUDefinition);
        this.AddUnitDefinition(this.rParagraph, "b", i_Tree_Eco_v6.Resources.Strings.MWHDefinition);
        this.ra.Children.Add((RenderObject) this.rParagraph);
        this.rParagraph = new RenderParagraph();
        this.rParagraph.Style.Spacing.Top = (Unit) "1ls";
        this.pText = new ParagraphText();
        this.pText.Style.Parents = this.styleTableTitle;
        this.pText.Text = i_Tree_Eco_v6.Resources.Strings.WR_Table4TitleP1;
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        this.pText = new ParagraphText("a");
        this.pText.Style.Parents = this.styleTableTitle;
        this.pText.Style.TextPosition = TextPositionEnum.Superscript;
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        this.pText = new ParagraphText(string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Table4TitleP2, (object) this.CurrencySymbol, (object) this.locationName));
        this.pText.Style.Parents = this.styleTableTitle;
        this.rParagraph.Content.Add((ParagraphObject) this.pText);
        this.ra.Children.Add((RenderObject) this.rParagraph);
        RenderTable renderTable4 = new RenderTable();
        renderTable4.Cols[0].Width = (Unit) "40%";
        renderTable4.Cols[1].Width = (Unit) "20%";
        renderTable4.Cols[2].Width = (Unit) "20%";
        renderTable4.Cols[3].Width = (Unit) "20%";
        renderTable4.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
        renderTable4.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        int count5 = 1;
        renderTable4.RowGroups[0, count5].Header = TableHeaderEnum.Page;
        ReportUtil.AddWrittenReportTableHeaderFormat(renderTable4);
        renderTable4.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.Heating;
        renderTable4.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.Cooling;
        renderTable4.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.Total;
        List<EnergyUsage> energyUsageList3 = new List<EnergyUsage>();
        energyUsageList3.Add(new EnergyUsage()
        {
          Title = i_Tree_Eco_v6.Resources.Strings.UnitMBTU,
          Heating = energyValues4.ToString("N0"),
          Cooling = i_Tree_Eco_v6.Resources.Strings.NotApplicableAbbr,
          Total = energyValues4.ToString("N0")
        });
        List<EnergyUsage> energyUsageList4 = energyUsageList3;
        EnergyUsage energyUsage2 = new EnergyUsage();
        energyUsage2.Title = i_Tree_Eco_v6.Resources.Strings.UnitMWH;
        energyUsage2.Heating = energyValues5.ToString("N0");
        energyUsage2.Cooling = energyValues6.ToString("N0");
        num18 = energyValues5 + energyValues6;
        energyUsage2.Total = num18.ToString("N0");
        energyUsageList4.Add(energyUsage2);
        List<EnergyUsage> energyUsageList5 = energyUsageList3;
        EnergyUsage energyUsage3 = new EnergyUsage();
        energyUsage3.Title = i_Tree_Eco_v6.Resources.Strings.CarbonAvoided;
        energyUsage3.Heating = energyValues7.ToString("N0");
        energyUsage3.Cooling = energyValues8.ToString("N0");
        num18 = energyValues7 + energyValues8;
        energyUsage3.Total = num18.ToString("N0");
        energyUsageList5.Add(energyUsage3);
        int row5 = count5;
        for (int index = 0; index < energyUsageList1.Count; ++index)
        {
          this.rParagraph = new RenderParagraph();
          this.pText = new ParagraphText(energyUsageList3[index].Title);
          this.pText.Style.Parents = this.styleDefaultParagraph;
          this.rParagraph.Content.Add((ParagraphObject) this.pText);
          this.pText = new ParagraphText();
          this.pText.Style.Parents = this.styleDefaultParagraph;
          this.pText.Style.TextPosition = TextPositionEnum.Superscript;
          switch (index)
          {
            case 0:
              this.pText.Text = "b";
              break;
            case 1:
              this.pText.Text = "c";
              break;
          }
          this.rParagraph.Content.Add((ParagraphObject) this.pText);
          renderTable4.Cells[row5, 0].RenderObject = (RenderObject) this.rParagraph;
          renderTable4.Cells[row5, 1].Text = energyUsageList3[index].Heating;
          renderTable4.Cells[row5, 2].Text = energyUsageList3[index].Cooling;
          renderTable4.Cells[row5, 3].Text = energyUsageList3[index].Total;
          ++row5;
        }
        ReportUtil.FormatRenderTableWrittenReport(renderTable4);
        this.ra.Children.Add((RenderObject) renderTable4);
        this.rParagraph = new RenderParagraph();
        this.AddUnitDefinition(this.rParagraph, "b", string.Format(i_Tree_Eco_v6.Resources.Strings.WR_NoteTable4, (object) (this.customizedElectricityDollarsPerKwh * 1000.0), (object) (this.customizedHeatingDollarsPerTherm * 10.0023877), (object) this.CurrencySymbol));
        this.AddUnitDefinition(this.rParagraph, "c", i_Tree_Eco_v6.Resources.Strings.MBTUDefinition);
        this.AddUnitDefinition(this.rParagraph, "c", i_Tree_Eco_v6.Resources.Strings.MWHDefinition);
        this.ra.Children.Add((RenderObject) this.rParagraph);
        this.AddAboveAsteriskFooterNote(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_TreesAndBuildingEnergyFootNote);
        C1doc.Body.Children.Add((RenderObject) this.ra);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValues);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesP1);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesP2);
        this.AddAppendix1Definition(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesReplacementValues, (object) this.locationName));
        this.FormatValuesBulletList(C1doc, structuralValue, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesReplacementValue);
        this.FormatValuesBulletList(C1doc, num6, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesCarbonStorage);
        this.AddAppendix1Definition(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesFunctionalValues, (object) this.locationName));
        this.FormatValuesBulletList(C1doc, carbonSequestration * this.customizedCarbonDollarsPerTon, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesCarbonSequestration);
        this.FormatValuesBulletList(C1doc, avoidedRunoff.Item2, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesAvoidedRunoff);
        this.FormatValuesBulletList(C1doc, pollutionRemovalDollars, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesPollutionRemoval);
        this.FormatValuesBulletList(C1doc, num7 + energyValues7 + energyValues8, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesEnergyAndCarbonValues);
        this.rText = new RenderText(i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesFunctionalValuesNote);
        this.rText.Style.Parents = this.styleTinyText;
        C1doc.Body.Children.Add((RenderObject) this.rText);
        C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartGreatestReplacementValues(this.locationName, this.CurrencySymbol), this.formGraphics, C1doc, 1.0, 0.4));
        if (this.nation.Id != 219)
        {
          this.rParagraph = new RenderParagraph();
          this.rParagraph.Style.Parents = this.styleTinyText;
          this.pText = new ParagraphText("1");
          this.pText.Style.TextPosition = TextPositionEnum.Superscript;
          this.rParagraph.Content.Add((ParagraphObject) this.pText);
          if (this.nation.Id == 21)
            this.FormatInternationalNote(C1doc, this.rParagraph, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesAU);
          else if (this.nation.Id == 45)
            this.FormatInternationalNote(C1doc, this.rParagraph, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesAustralCA);
          else if (this.nation.Id == 218)
            this.FormatInternationalNote(C1doc, this.rParagraph, i_Tree_Eco_v6.Resources.Strings.WR_ReplacementAndFunctionalValuesAustralUK);
        }
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_PotentialPestImpacts);
        List<PestAffected> affectedAllPests = this.GetPestAffectedAllPests(this.locSpSession, dictionary1, structuralValues);
        List<PestAffected> PestsInLocation = this.nation.Id != 219 ? affectedAllPests : this.GetPestAffectedUSA(this.county, dictionary1, structuralValues);
        string pestImpactsP1Part1 = i_Tree_Eco_v6.Resources.Strings.WR_PotentialPestImpactsP1Part1;
        string s13 = this.nation.Id != 219 ? pestImpactsP1Part1 + i_Tree_Eco_v6.Resources.Strings.WR_PotentialPestImpactsP1Part2Opt2 : pestImpactsP1Part1 + string.Format(i_Tree_Eco_v6.Resources.Strings.WR_PotentialPestImpactsP1Part2Opt1, (object) this.county.Name, (object) ReportUtil.UpperCaseFirstLetter(ReportUtil.NumberToWords(PestsInLocation.Count)));
        this.AddDefaultParagraph(C1doc, s13);
        C1doc.Body.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartSpeciesPestRiskCounty(PestsInLocation, this.locationName, this.CurrencySymbol, this.nation.Id != 219), this.formGraphics, C1doc, 1.0, 0.5));
        for (int index = 0; index < PestsInLocation.Count; ++index)
        {
          C1PrintDocument C1doc14 = C1doc;
          string pestNote = WR_Pest.GetPestNote(PestsInLocation[index]);
          object[] objArray13 = new object[4];
          num18 = 100.0 * PestsInLocation[index].PopulationAffected / num3;
          objArray13[0] = (object) num18.ToString("N1");
          objArray13[1] = (object) this.CurrencySymbol;
          objArray13[2] = (object) ReportUtil.GetNumericWordWithMagnitude(PestsInLocation[index].ReplacementValue, 3);
          objArray13[3] = (object) this.locationName;
          string s14 = string.Format(pestNote, objArray13);
          this.AddDefaultParagraph(C1doc14, s14);
        }
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BulletList1, this.series.SampleType == SampleType.RegularPlot ? (object) string.Format("{0} ", (object) i_Tree_Eco_v6.Resources.Strings.WR_FromRandomlyLocatedPlots) : (object) string.Empty));
        this.InserNewLine(C1doc);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BulletList1Item1);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BulletList1Item2);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BulletList1Item3);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BulletList1Item4);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BulletList1Item5);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BulletList1Item6);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1P1);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1P2);
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1TreeCharacteristics);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1TreeCharacteristicsP1);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Appendix1TreeCharacteristicsP2, this.nation.Id == 219 ? (object) string.Format("({0})", (object) this.resmanStateInvasives.GetString(this.state.Name + "_InText")) : (object) string.Empty));
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PollutionRemoval);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PollutionRemovalP1);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PollutionRemovalP2);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PollutionRemovalP3);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PollutionRemovalP4);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PollutionRemovalP5);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PollutionRemovalP6, (object) this.GetCustomizedDollarsPerTon(this.customizedCoDollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedO3DollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedNO2DollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedSO2DollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedPM25DollarsPerTon), (object) this.GetCustomizedDollarsPerTon(this.customizedPM10DollarsPerTon), (object) ReportBase.TonneUnits(), (object) this.CurrencySymbol));
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1CarbonStorageAndSequestration);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1CarbonStorageAndSequestrationP1);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1CarbonStorageAndSequestrationP2);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1CarbonStorageAndSequestrationP3);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Appendix1CarbonStorageAndSequestrationP4, (object) this.CurrencySymbol, (object) this.GetCustomizedDollarsPerTon(this.customizedCarbonDollarsPerTon), (object) ReportBase.TonneUnits()));
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1OxygenProduction);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1OxygenProductionP1);
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1AvoidedRunoff);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1AvoidedRunoffP1);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1AvoidedRunoffP2);
        C1PrintDocument C1doc15 = C1doc;
        string appendix1AvoidedRunoffP3 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix1AvoidedRunoffP3;
        string currencySymbol1 = this.CurrencySymbol;
        string str39;
        if (!ReportBase.EnglishUnits)
        {
          num18 = this.customizedWaterDollarsPerM3;
          str39 = num18.ToString("N2");
        }
        else
        {
          num18 = this.customizedWaterDollarsPerM3 / 35.3147;
          str39 = num18.ToString("N2");
        }
        string str40 = ReportBase.CubicMeterUnits();
        string s15 = string.Format(appendix1AvoidedRunoffP3, (object) currencySymbol1, (object) str39, (object) str40);
        this.AddDefaultParagraph(C1doc15, s15);
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BuildingEnergyUse);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BuildingEnergyUseP1, this.nation.Id == 21 ? (object) i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BuildingEnergyUseP1AU : (object) string.Empty));
        C1PrintDocument C1doc16 = C1doc;
        string buildingEnergyUseP2_1 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix1BuildingEnergyUseP2;
        string currencySymbol2 = this.CurrencySymbol;
        num18 = this.customizedElectricityDollarsPerKwh * 1000.0;
        string str41 = num18.ToString("N2");
        num18 = this.customizedHeatingDollarsPerTherm * 10.002387672;
        string str42 = num18.ToString("N2");
        string s16 = string.Format(buildingEnergyUseP2_1, (object) currencySymbol2, (object) str41, (object) str42);
        this.AddDefaultParagraph(C1doc16, s16);
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1ReplacementValues);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1ReplacementValuesP1);
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PotentialPeastImpacts);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PotentialPeastImpactsP1);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Appendix1PotentialPeastImpactsP2, (object) this.fourHundredKm, (object) this.fourHundredKmTwelveHundredKm, (object) this.twelveHundredKm));
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1RelativeTreeEffects);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1RelativeTreeEffectsP1);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1RelativeTreeEffectsP2);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1RelativeTreeEffectsP3);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1RelativeTreeEffectsP4);
        this.rText = new RenderText();
        this.rText.Style.Spacing.Top = (Unit) "1ls";
        C1doc.Body.Children.Add((RenderObject) this.rText);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1RelativeTreeEffectsBulletLI1);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1RelativeTreeEffectsBulletLI2);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1RelativeTreeEffectsBulletLI3);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix1RelativeTreeEffectsBulletLI4);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix2);
        this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Appendix2P1, (object) this.locationName));
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix2CarbonStorageIsEquivalentTo);
        C1PrintDocument C1doc17 = C1doc;
        string isEquivalentToB1 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2CarbonStorageIsEquivalentToB1;
        string locationName1 = this.locationName;
        num18 = Math.Round(carbonStorage / (4.8 * (double) num2) * 365.0, 0);
        string str43 = num18.ToString("N0");
        string text4 = string.Format(isEquivalentToB1, (object) locationName1, (object) str43);
        this.RenderListItem(C1doc17, text4);
        C1PrintDocument C1doc18 = C1doc;
        string isEquivalentToB2 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2CarbonStorageIsEquivalentToB2;
        num18 = ReportUtil.RoundToSignificantFigures(carbonStorage * 1000.0 * 2.204 / 2826.167618, 3);
        string str44 = num18.ToString("N0");
        string text5 = string.Format(isEquivalentToB2, (object) str44);
        this.RenderListItem(C1doc18, text5);
        C1PrintDocument C1doc19 = C1doc;
        string isEquivalentToB3 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2CarbonStorageIsEquivalentToB3;
        num18 = ReportUtil.RoundToSignificantFigures(carbonStorage / 3.128598, 3);
        string str45 = num18.ToString("N0");
        string text6 = string.Format(isEquivalentToB3, (object) str45);
        this.RenderListItem(C1doc19, text6);
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix2CarbonMonoxideRemoval);
        C1PrintDocument C1doc20 = C1doc;
        string monoxideRemovalB1 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2CarbonMonoxideRemovalB1;
        num18 = ReportUtil.RoundToSignificantFigures(pollutionRemovalAmount2 * 1000.0 * 2.204 / 217.7687326, 3);
        string str46 = num18.ToString("N0");
        string text7 = string.Format(monoxideRemovalB1, (object) str46);
        this.RenderListItem(C1doc20, text7);
        C1PrintDocument C1doc21 = C1doc;
        string monoxideRemovalB2 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2CarbonMonoxideRemovalB2;
        num18 = ReportUtil.RoundToSignificantFigures(pollutionRemovalAmount2 / 0.035856, 3);
        string str47 = num18.ToString("N0");
        string text8 = string.Format(monoxideRemovalB2, (object) str47);
        this.RenderListItem(C1doc21, text8);
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix2NitrogenDioxideRemoval);
        C1PrintDocument C1doc22 = C1doc;
        string dioxideRemovalB1_1 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2NitrogenDioxideRemovalB1;
        num18 = ReportUtil.RoundToSignificantFigures(pollutionRemovalAmount3 * 1000.0 * 2.204 / 13.96912832, 3);
        string str48 = num18.ToString("N0");
        string text9 = string.Format(dioxideRemovalB1_1, (object) str48);
        this.RenderListItem(C1doc22, text9);
        C1PrintDocument C1doc23 = C1doc;
        string dioxideRemovalB2_1 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2NitrogenDioxideRemovalB2;
        num18 = ReportUtil.RoundToSignificantFigures(pollutionRemovalAmount3 / 0.014067, 3);
        string str49 = num18.ToString("N0");
        string text10 = string.Format(dioxideRemovalB2_1, (object) str49);
        this.RenderListItem(C1doc23, text10);
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix2SulfurDioxideRemoval);
        C1PrintDocument C1doc24 = C1doc;
        string dioxideRemovalB1_2 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2SulfurDioxideRemovalB1;
        num18 = ReportUtil.RoundToSignificantFigures(pollutionRemovalAmount6 * 1000.0 * 2.204 / 0.18592053, 3);
        string str50 = num18.ToString("N0");
        string text11 = string.Format(dioxideRemovalB1_2, (object) str50);
        this.RenderListItem(C1doc24, text11);
        C1PrintDocument C1doc25 = C1doc;
        string dioxideRemovalB2_2 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2SulfurDioxideRemovalB2;
        num18 = ReportUtil.RoundToSignificantFigures(pollutionRemovalAmount6 / 0.031926, 3);
        string str51 = num18.ToString("N0");
        string text12 = string.Format(dioxideRemovalB2_2, (object) str51);
        this.RenderListItem(C1doc25, text12);
        this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix2AnnualCarbonSequestration);
        C1PrintDocument C1doc26 = C1doc;
        string carbonSequestrationB1 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2AnnualCarbonSequestrationB1;
        string locationName2 = this.locationName;
        num18 = ReportUtil.RoundToSignificantFigures(carbonSequestration / (4.8000087272885956 * (double) num2) * 365.0, 2);
        string str52 = num18.ToString("N1");
        string text13 = string.Format(carbonSequestrationB1, (object) locationName2, (object) str52);
        this.RenderListItem(C1doc26, text13);
        C1PrintDocument C1doc27 = C1doc;
        string carbonSequestrationB2 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2AnnualCarbonSequestrationB2;
        num18 = Math.Round(carbonSequestration * 1000.0 * 2.204 / 2826.167618 / 100.0, 0, MidpointRounding.AwayFromZero) * 100.0;
        string str53 = num18.ToString("N0");
        string text14 = string.Format(carbonSequestrationB2, (object) str53);
        this.RenderListItem(C1doc27, text14);
        C1PrintDocument C1doc28 = C1doc;
        string carbonSequestrationB3 = i_Tree_Eco_v6.Resources.Strings.WR_Appendix2AnnualCarbonSequestrationB3;
        num18 = Math.Round(carbonSequestration / 3.128598 / 100.0, 0, MidpointRounding.AwayFromZero) * 100.0;
        string str54 = num18.ToString("N0");
        string text15 = string.Format(carbonSequestrationB3, (object) str54);
        this.RenderListItem(C1doc28, text15);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix3);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix3P1);
        this.GenerateComparisonOfUrbanForestsTables(C1doc);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix4);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix4P1);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix4BulletLItem1);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix4BulletLItem2);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix4BulletLItem3);
        this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix4BulletLItem4);
        this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix4P2);
        this.ra = new RenderArea();
        this.ra.Width = (Unit) "90%";
        this.ra.X = (Unit) "parent.left + (parent.width / 2 - self.Width/2)";
        this.rText = new RenderText(i_Tree_Eco_v6.Resources.Strings.UrbanForestManagementStrategiesTableHeader);
        this.rText.Style.Spacing.Top = (Unit) "1ls";
        this.ra.Children.Add((RenderObject) this.rText);
        RenderTable renderTable5 = new RenderTable();
        renderTable5.Style.GridLines.All = LineDef.Default;
        renderTable5.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
        renderTable5.RowGroups[0, 1].Header = TableHeaderEnum.Page;
        ReportUtil.AddWrittenReportTableHeaderFormat(renderTable5);
        renderTable5.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.Strategy;
        renderTable5.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.Result;
        renderTable5.Cells[1, 0].Text = i_Tree_Eco_v6.Resources.Strings.IncreaseTheNumberOfHealthyTrees;
        renderTable5.Cells[1, 1].Text = i_Tree_Eco_v6.Resources.Strings.IncreasePollutionRemoval;
        renderTable5.Cells[2, 0].Text = i_Tree_Eco_v6.Resources.Strings.SustainExistingTreeCover;
        renderTable5.Cells[2, 1].Text = i_Tree_Eco_v6.Resources.Strings.MaintainPollutionRemovalLevels;
        renderTable5.Cells[3, 0].Text = i_Tree_Eco_v6.Resources.Strings.MaximizeUseOfLowVOCEmittingTrees;
        renderTable5.Cells[3, 1].Text = i_Tree_Eco_v6.Resources.Strings.ReducesOzoneAndCarbonMonoxideFormation;
        renderTable5.Cells[4, 0].Text = i_Tree_Eco_v6.Resources.Strings.SustainLargeHealthyTrees;
        renderTable5.Cells[4, 1].Text = i_Tree_Eco_v6.Resources.Strings.LargeTreesHaveGreatestPerTreeEffects;
        renderTable5.Cells[5, 0].Text = i_Tree_Eco_v6.Resources.Strings.UseLongLivedTrees;
        renderTable5.Cells[5, 1].Text = i_Tree_Eco_v6.Resources.Strings.ReduceLongTermPollutantEmissions;
        renderTable5.Cells[6, 0].Text = i_Tree_Eco_v6.Resources.Strings.UseLowMaintenanceTrees;
        renderTable5.Cells[6, 1].Text = i_Tree_Eco_v6.Resources.Strings.ReducePollutantsEmissions;
        renderTable5.Cells[7, 0].Text = i_Tree_Eco_v6.Resources.Strings.ReduceFossilFuelUse;
        renderTable5.Cells[7, 1].Text = i_Tree_Eco_v6.Resources.Strings.ReducePollutantEmissions;
        renderTable5.Cells[8, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlantTreesInEnergyConservingLocations;
        renderTable5.Cells[8, 1].Text = i_Tree_Eco_v6.Resources.Strings.ReducePollutantEmissionsFromPowerPlants;
        renderTable5.Cells[9, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlantTreesToShadeParkedCars;
        renderTable5.Cells[9, 1].Text = i_Tree_Eco_v6.Resources.Strings.ReduceVehicularVOCEmissions;
        renderTable5.Cells[10, 0].Text = i_Tree_Eco_v6.Resources.Strings.SupplyAmpleWaterToVegetation;
        renderTable5.Cells[10, 1].Text = i_Tree_Eco_v6.Resources.Strings.EnhancePollutionRemoval;
        renderTable5.Cells[11, 0].Text = i_Tree_Eco_v6.Resources.Strings.PlantTreesInPollutedAreas;
        renderTable5.Cells[11, 1].Text = i_Tree_Eco_v6.Resources.Strings.MaximizesTreeAirQualityBenefits;
        renderTable5.Cells[12, 0].Text = i_Tree_Eco_v6.Resources.Strings.AvoidPollutantSensitiveSpecies;
        renderTable5.Cells[12, 1].Text = i_Tree_Eco_v6.Resources.Strings.ImproveTreeHealth;
        renderTable5.Cells[13, 0].Text = i_Tree_Eco_v6.Resources.Strings.UtilizeEvergreenTrees;
        renderTable5.Cells[13, 1].Text = i_Tree_Eco_v6.Resources.Strings.YearRoundRemovalOfParticles;
        ReportUtil.FormatRenderTableWrittenReport(renderTable5);
        this.ra.Children.Add((RenderObject) renderTable5);
        C1doc.Body.Children.Add((RenderObject) this.ra);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Appendix5);
        if (this.nation.Id == 219)
        {
          this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Appendix5P1, (object) this.state.Name, (object) this.resmanStateInvasives.GetString(this.state.Name + "_InText")));
          RenderTable renderTable6 = new RenderTable();
          int count6 = 2;
          renderTable6.RowGroups[0, count6].Header = TableHeaderEnum.Page;
          ReportUtil.AddWrittenReportTableHeaderFormat(renderTable6);
          this.rParagraph = new RenderParagraph();
          this.rParagraph.Style.Font = renderTable6.Style.Font;
          this.pText = new ParagraphText(i_Tree_Eco_v6.Resources.Strings.SpeciesName);
          this.rParagraph.Content.Add((ParagraphObject) this.pText);
          this.AddSubscript(this.rParagraph, "a");
          renderTable6.Cells[0, 0].RenderObject = (RenderObject) this.rParagraph;
          List<double> dataList4 = new List<double>();
          foreach (KeyValuePair<double, InvasiveSpecies> keyValuePair2 in invasiveSpeciesList)
            dataList4.Add(keyValuePair2.Value.LeafArea);
          dataScaler1.SetScaler(dataList4, Units.Squarekilometer, Units.None);
          renderTable6.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
          renderTable6.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.PercentOfTrees;
          renderTable6.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.LeafArea;
          renderTable6.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(dataScaler1.GetPrefixPlusUnitAbbreviation(ReportBase.EnglishUnits));
          renderTable6.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PercentLeafArea;
          int num22 = count6;
          foreach (KeyValuePair<double, InvasiveSpecies> keyValuePair3 in invasiveSpeciesList)
          {
            renderTable6.Cells[num22, 0].Text = this.GetSpeciesName(keyValuePair3.Value.SpeciesCVO);
            TableCell cell4 = renderTable6.Cells[num22, 1];
            double num23 = keyValuePair3.Value.TreeCount;
            string str55 = num23.ToString("N0");
            cell4.Text = str55;
            TableCell cell5 = renderTable6.Cells[num22, 2];
            num23 = keyValuePair3.Value.PercentTreeNumber;
            string str56 = num23.ToString("N1");
            cell5.Text = str56;
            TableCell cell6 = renderTable6.Cells[num22, 3];
            num23 = dataScaler1.GetScaledValue(keyValuePair3.Value.LeafArea, ReportBase.EnglishUnits);
            string str57 = num23.ToString("N1");
            cell6.Text = str57;
            TableCell cell7 = renderTable6.Cells[num22, 4];
            num23 = keyValuePair3.Value.PercentLeafArea;
            string str58 = num23.ToString("N1");
            cell7.Text = str58;
            ++num22;
          }
          renderTable6.Cells[num22, 0].Text = i_Tree_Eco_v6.Resources.Strings.Total;
          renderTable6.Cells[num22, 1].Text = invasiveSpeciesList.Sum<KeyValuePair<double, InvasiveSpecies>>((Func<KeyValuePair<double, InvasiveSpecies>, double>) (p => p.Value.TreeCount)).ToString("N0");
          renderTable6.Cells[num22, 2].Text = invasiveSpeciesList.Sum<KeyValuePair<double, InvasiveSpecies>>((Func<KeyValuePair<double, InvasiveSpecies>, double>) (p => p.Value.PercentTreeNumber)).ToString("N2");
          renderTable6.Cells[num22, 3].Text = dataScaler1.GetScaledValue(invasiveSpeciesList.Sum<KeyValuePair<double, InvasiveSpecies>>((Func<KeyValuePair<double, InvasiveSpecies>, double>) (p => p.Value.LeafArea)), ReportBase.EnglishUnits).ToString("N2");
          renderTable6.Cells[num22, 4].Text = invasiveSpeciesList.Sum<KeyValuePair<double, InvasiveSpecies>>((Func<KeyValuePair<double, InvasiveSpecies>, double>) (p => p.Value.PercentLeafArea)).ToString("N2");
          renderTable6.Rows[num22].Style.FontBold = true;
          ReportUtil.FormatRenderTableWrittenReport(renderTable6);
          C1doc.Body.Children.Add((RenderObject) renderTable6);
          this.rParagraph = new RenderParagraph();
          this.rParagraph.Style.Font = renderTable6.Style.Font;
          this.AddSubscript(this.rParagraph, "a");
          this.pText = new ParagraphText(i_Tree_Eco_v6.Resources.Strings.WR_Apendix5SuperscriptExplanation);
          this.rParagraph.Content.Add((ParagraphObject) this.pText);
          C1doc.Body.Children.Add((RenderObject) this.rParagraph);
        }
        else
          this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix5InternationalMessage);
        this.AddSectionTitleAndIndex(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6);
        if (this.nation.Id == 219)
        {
          this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6P1);
          RenderTable renderTable7 = new RenderTable();
          C1doc.Body.Children.Add((RenderObject) renderTable7);
          renderTable7.Style.Spacing.Top = (Unit) "1ls";
          renderTable7.BreakAfter = BreakEnum.Page;
          renderTable7.Style.GridLines.All = LineDef.Empty;
          renderTable7.CellStyle.TextAlignVert = C1.C1Preview.AlignVertEnum.Top;
          renderTable7.CellStyle.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
          renderTable7.CellStyle.JustifyEndOfLines = false;
          int count7 = 2;
          renderTable7.RowGroups[0, count7].Header = TableHeaderEnum.Page;
          renderTable7.RowGroups[0, count7].Style.Borders.Top = LineDef.Default;
          renderTable7.RowGroups[0, count7].Style.TextAlignVert = C1.C1Preview.AlignVertEnum.Center;
          renderTable7.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
          renderTable7.Cols[3].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
          renderTable7.Cols[4].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
          renderTable7.Width = (Unit) "100%";
          renderTable7.Cols[0].Width = (Unit) "10%";
          renderTable7.Cols[3].Width = (Unit) "15%";
          renderTable7.Cols[4].Width = (Unit) "15%";
          ReportUtil.FormatRenderTableHeader(renderTable7);
          List<double> dataList5 = new List<double>();
          foreach (PestAffected pestAffected in affectedAllPests)
            dataList5.Add(pestAffected.ReplacementValue);
          dataScaler1.SetScaler(dataList5, Units.Monetaryunit, Units.None, this.CurrencyName, this.CurrencySymbol);
          renderTable7.Cells[0, 0].Text = LocationSpecies.Domain.Properties.Strings.Species_Code;
          renderTable7.Cells[0, 1].Text = LocationSpecies.Domain.Properties.Strings.Species_ScientificName;
          renderTable7.Cells[0, 2].Text = LocationSpecies.Domain.Properties.Strings.Species_CommonName;
          renderTable7.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.TreesAtRisk;
          renderTable7.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(i_Tree_Eco_v6.Resources.Strings.NumberSymbol);
          renderTable7.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.Value;
          renderTable7.Cells[1, 4].Text = ReportUtil.FormatHeaderUnitsStr(this.CurrencySymbol);
          string scaledPrefix = dataScaler1.GetScaledPrefix(ReportBase.EnglishUnits);
          if (scaledPrefix != string.Empty)
            renderTable7.Cells[1, 4].Text = ReportUtil.FormatHeaderUnitsStr(string.Format("{0} {1}", (object) this.CurrencySymbol, (object) scaledPrefix));
          int row6 = count7;
          foreach (PestAffected pestAffected in affectedAllPests)
          {
            renderTable7.Cells[row6, 0].Text = pestAffected.Abb;
            renderTable7.Cells[row6, 1].Text = pestAffected.ScientificName;
            renderTable7.Cells[row6, 2].Text = pestAffected.CommonName;
            renderTable7.Cells[row6, 3].Text = pestAffected.PopulationAffected.ToString("N0");
            renderTable7.Cells[row6, 4].Text = dataScaler1.GetScaledValue(pestAffected.ReplacementValue, ReportBase.EnglishUnits).ToString("N2");
            ++row6;
          }
          this.StartOnNewPage(C1doc);
          this.AddDefaultParagraph(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Apendix6P2, (object) this.fourHundredKm, (object) this.twelveHundredKm));
          this.ra = new RenderArea();
          this.ra.Stacking = StackingRulesEnum.InlineLeftToRight;
          DataScaler pestTreeCountScaler = (DataScaler) null;
          DataScaler pestReplacementValueScaler = (DataScaler) null;
          string[] strArray = new string[4]
          {
            "Red",
            "Yellow",
            "Orange",
            "Green"
          };
          foreach (string Level in strArray)
            this.ra.Children.Add((RenderObject) ReportUtil.CreateChartRenderObject(this.CreateChartPestRiskAndDistance(this.locSpSession, this.county, Level, dictionary1, pestTreeCountScaler, pestReplacementValueScaler, structuralValues), this.formGraphics, C1doc, 0.5, 0.3));
          C1doc.Body.Children.Add((RenderObject) this.ra);
          this.rText = new RenderText();
          this.rText.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Center;
          this.rText.Text = i_Tree_Eco_v6.Resources.Strings.NotePotentialRiskOfPestsGraphs;
          C1doc.Body.Children.Add((RenderObject) this.rText);
          this.StartOnNewPage(C1doc);
          this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6P3);
          this.CreatePestRiskTable(C1doc, this.county, dictionary1, ReportBase.ScientificName);
          this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6Note);
          this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6NoteP1, true);
          this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6SpeciesRisk);
          this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6SpeciesRiskBLItem1);
          this.RenderListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Apendix6SpeciesRiskBLItem2, (object) this.fourHundredKm));
          this.RenderListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Apendix6SpeciesRiskBLItem3, (object) this.fourHundredKm, (object) this.fourHundredKmTwelveHundredKm));
          this.RenderListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Apendix6SpeciesRiskBLItem4, (object) this.twelveHundredKm, (object) this.twelveHundredKm));
          this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6RiskWeight);
          this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6RiskWeightp1, true);
          this.AddAppendix1Definition(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6PestColorCodes);
          this.RenderListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Apendix6PestColorCodesBLI1, (object) this.county.Name));
          this.RenderListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Apendix6PestColorCodesBLI1, (object) this.fourHundredKm, (object) this.county.Name));
          this.RenderListItem(C1doc, string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Apendix6PestColorCodesBLI3, (object) this.twelveHundredKm, (object) this.county.Name));
          this.RenderListItem(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6PestColorCodesBLI4);
        }
        else
          this.AddDefaultParagraph(C1doc, i_Tree_Eco_v6.Resources.Strings.WR_Apendix6InternationalMessage);
        this.AddSectionTitleAndIndex(C1doc, "References");
        List<string> values = new List<string>();
        values.Add("Abdollahi, K.K.; Ning, Z.H.; Appeaning, A., eds. 2000. Global climate change and the urban forest. Baton Rouge, LA: GCRCC and Franklin Press. 77 p.");
        values.Add("Baldocchi, D. 1988. A multi-layer model for estimating sulfur dioxide deposition to a deciduous oak forest canopy. Atmospheric Environment. 22: 869-884.");
        values.Add("Baldocchi, D.D.; Hicks, B.B.; Camara, P. 1987. A canopy stomatal resistance model for gaseous deposition to vegetated surfaces. Atmospheric Environment. 21: 91-101.");
        values.Add("Bidwell, R.G.S.; Fraser, D.E. 1972. Carbon monoxide uptake and metabolism by leaves. Canadian Journal of Botany. 50: 1435-1439.");
        values.Add("British Columbia Ministry of Water, Land, and Air Protection. 2005. Residential wood burning emissions in British Columbia. British Columbia.");
        values.Add("Broecker, W.S. 1970. Man's oxygen reserve. Science 168(3939): 1537-1538.");
        values.Add("Bureau of Transportation Statistics. 2010. Estimated National Average Vehicle Emissions Rates per Vehicle by Vehicle Type using Gasoline and Diesel. Washington, DC: Burea of Transportation Statistics, U.S. Department of Transportation. Table 4-43.");
        values.Add("California Air Resources Board. 2013. Methods to Find the Cost-Effectiveness of Funding Air Quality Projects. Table 3 Average Auto Emission Factors. CA: California Environmental Protection Agency, Air Resources Board.");
        values.Add("Carbon Dioxide Information Analysis Center. 2010. CO2 Emissions (metric tons per capita). Washington, DC: The World Bank.");
        values.Add("Cardelino, C.A.; Chameides, W.L. 1990.  Natural hydrocarbons, urbanization, and urban ozone. Journal of Geophysical Research. 95(D9): 13,971-13,979.");
        values.Add("Eastern Forest Environmental Threat Assessment Center. Dutch Elm Disease. http://threatsummary.forestthreats.org/threats/threatSummaryViewer.cfm?threatID=43");
        values.Add("Energy Information Administration. 1994. Energy Use and Carbon Emissions: Non-OECD Countries. Washington, DC: Energy Information Administration, U.S. Department of Energy.");
        values.Add("Energy Information Administration. 2013. CE2.1 Fuel consumption totals and averages, U.S. homes. Washington, DC: Energy Information Administration, U.S. Department of Energy.");
        values.Add("Energy Information Administration. 2014. CE5.2 Household wood consumption. Washington, DC: Energy Information Administration, U.S. Department of Energy.");
        values.Add("Federal Highway Administration. 2013. Highway Statistics 2011.Washington, DC: Federal Highway Administration, U.S. Department of Transportation. Table VM-1.");
        values.Add("Georgia Forestry Commission. 2009. Biomass Energy Conversion for Electricity and Pellets Worksheet. Dry Branch, GA: Georgia Forestry Commission.");
        values.Add("Heirigs, P.L.; Delaney, S.S.; Dulla, R.G. 2004. Evaluation of MOBILE Models: MOBILE6.1 (PM), MOBILE6.2 (Toxics), and MOBILE6/CNG. Sacramento, CA: National Cooperative Highway Research Program, Transportation Research Board.");
        values.Add("Hirabayashi, S. 2011. Urban Forest Effects-Dry Deposition (UFORE-D) Model Enhancements, http://www.itreetools.org/eco/resources/UFORE-D enhancements.pdf");
        values.Add("Hirabayashi, S. 2012. i-Tree Eco Precipitation Interception Model Descriptions, http://www.itreetools.org/eco/resources/iTree_Eco_Precipitation_Interception_Model_Descriptions_V1_2.pdf");
        values.Add("Hirabayashi, S.; Kroll, C.; Nowak, D. 2011. Component-based development and sensitivity analyses of an air pollutant dry deposition model. Environmental Modeling and Software. 26(6): 804-816.");
        values.Add("Hirabayashi, S.; Kroll, C.; Nowak, D. 2012. i-Tree Eco Dry Deposition Model Descriptions V 1.0");
        values.Add("Interagency Working Group on Social Cost of Carbon, United States Government. 2015. Technical Support Document: Technical Update of the Social Cost of Carbon for Regulatory Impact Analysis Under Executive Order 12866. http://www.whitehouse.gov/sites/default/files/omb/inforeg/scc-tsd-final-july-2015.pdf");
        values.Add("Layton, M. 2004. 2005 Electricity Environmental Performance Report: Electricity Generation and Air Emissions. CA: California Energy Commission.");
        values.Add("Leonardo Academy. 2011. Leonardo Academy's Guide to Calculating Emissions Including Emission Factors and Energy Prices. Madison, WI: Leonardo Academy Inc.");
        values.Add("Lovett, G.M. 1994. Atmospheric deposition of nutrients and pollutants in North America: an ecological perspective. Ecological Applications. 4: 629-650.");
        values.Add("McPherson, E.G.; Maco, S.E.; Simpson, J.R.; Peper, P.J.; Xiao, Q.; VanDerZanden, A.M.; Bell, N. 2002. Western Washington and Oregon Community Tree Guide: Benefits, Costs, and Strategic Planting. International Society of Arboriculture, Pacific Northwest, Silverton, OR.");
        values.Add("McPherson, E.G.; Simpson, J.R. 1999. Carbon dioxide reduction through urban forestry: guidelines for professional and volunteer tree planters. Gen. Tech. Rep. PSW-171. Albany, CA: U.S. Department of Agriculture, Forest Service, Pacific Southwest Research Station. 237 p.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Crowell, A.M.N.; Xiao, Q. 2010. Northern California coast community tree guide: benefits, costs, and strategic planting. PSW-GTR-228. Gen. Tech. Rep. PSW-GTR-228. U.S. Department of Agriculture, Forest Service, Pacific Southwest Research Station, Albany, CA.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Gardner, S.L.; Vargas, K.E.; Maco, S.E.; Xiao, Q. 2006a. Coastal Plain Community Tree Guide: Benefits, Costs, and Strategic Planting PSW-GTR-201. USDA Forest Service, Pacific Southwest Research Station, Albany, CA.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Gardner, S.L.; Vargas, K.E.; Xiao, Q. 2007. Northeast community tree guide: benefits, costs, and strategic planting.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Maco, S.E.; Gardner, S.L.; Cozad, S.K.; Xiao, Q. 2006b. Midwest Community Tree Guide: Benefits, Costs and Strategic Planting PSW-GTR-199. U.S. Department of Agriculture, Forest Service, Pacific Southwest Research Station, Albany, CA.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Maco, S.E.; Gardner, S.L.; Vargas, K.E.; Xiao, Q. 2006c. Piedmont Community Tree Guide: Benefits, Costs, and Strategic Planting PSW-GTR 200. U.S. Department of Agriculture, Forest Service, Pacific Southwest Research Station, Albany, CA.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Maco, S.E.; Xiao Q.; Mulrean, E. 2004. Desert Southwest Community Tree Guide: Benefits, Costs and Strategic Planting. Phoenix, AZ: Arizona Community Tree Council, Inc. 81 :81.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Scott, K.I.; Xiao, Q. 2000. Tree Guidelines for Coastal Southern California Communities. Local Government Commission, Sacramento, CA.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Xiao, Q. 1999. Tree Guidelines for San Joaquin Valley Communities. Local Government Commission, Sacramento, CA.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Xiao, Q.; Maco, S.E.; Hoefer, P.J. 2003. Northern Mountain and Prairie Community Tree Guide: Benefits, Costs and Strategic Planting. Center for Urban Forest Research, USDA Forest Service, Pacific Southwest Research Station, Albany, CA.");
        values.Add("McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Xiao, Q.; Pittenger, D.R.; Hodel, D.R. 2001. Tree Guidelines for Inland Empire Communities. Local Government Commission, Sacramento, CA.");
        values.Add("Murray, F.J.; Marsh L.; Bradford, P.A. 1994. New York State Energy Plan, vol. II: issue reports. Albany, NY: New York State Energy Office.");
        values.Add("Nowak, D.J. 1994. Atmospheric carbon dioxide reduction by Chicago’s urban forest. In: McPherson, E.G.; Nowak, D.J.; Rowntree, R.A., eds. Chicago’s urban forest ecosystem: results of the Chicago Urban Forest Climate Project. Gen. Tech. Rep. NE-186. Radnor, PA: U.S. Department of Agriculture, Forest Service, Northeastern Forest Experiment Station: 83-94.");
        values.Add("Nowak, D.J. 1995. Trees pollute? A \"TREE\" explains it all. In: Proceedings of the 7th National Urban Forestry Conference.  Washington, DC: American Forests: 28-30.");
        values.Add("Nowak, D.J. 2000. The interactions between urban forests and global climate change. In: Abdollahi, K.K.; Ning, Z.H.; Appeaning, A., eds.  Global Climate Change and the Urban Forest.  Baton Rouge, LA: GCRCC and Franklin Press: 31-44.");
        values.Add("Nowak, D.J.; Civerolo, K.L.; Rao, S.T.; Sistla, S.; Luley, C.J.; Crane, D.E. 2000. A modeling study of the impact of urban trees on ozone. Atmospheric Environment. 34: 1601-1613.");
        values.Add("Nowak, D.J.; Crane, D.E. 2000. The Urban Forest Effects (UFORE) Model: quantifying urban forest structure and functions. In: Hansen, M.; Burk, T., eds. Integrated tools for natural resources inventories in the 21st century. Proceedings of IUFRO conference. Gen. Tech. Rep. NC-212. St. Paul, MN: U.S. Department of Agriculture, Forest Service, North Central Research Station: 714-720.");
        values.Add("Nowak, D.J.; Crane, D.E.; Dwyer, J.F. 2002a. Compensatory value of urban trees in the United States. Journal of Arboriculture. 28(4): 194 - 199.");
        values.Add("Nowak, D.J.; Crane, D.E.; Stevens, J.C.; Ibarra, M. 2002b. Brooklyn’s urban forest. Gen. Tech. Rep. NE-290. Newtown Square, PA: U.S. Department of Agriculture, Forest Service, Northeastern Research Station. 107 p.");
        values.Add("Nowak, D.J.; Crane, D.E.; Stevens, J.C.; Hoehn, R.E. 2005. The urban forest effects (UFORE) model: field data collection manual. V1b. Newtown Square, PA: U.S. Department of Agriculture, Forest Service, Northeastern Research Station, 34 p. http://www.fs.fed.us/ne/syracuse/Tools/downloads/UFORE_Manual.pdf");
        values.Add("Nowak, D.J.; Dwyer, J.F. 2000. Understanding the benefits and costs of urban forest ecosystems. In: Kuser, John, ed. Handbook of urban and community forestry in the northeast.  New York, NY: Kluwer Academics/Plenum: 11-22.");
        values.Add("Nowak, D.J., Hirabayashi, S., Bodine, A., Greenfield, E. 2014. Tree and forest effects on air quality and human health in the United States. Environmental Pollution. 193:119-129.");
        values.Add("Nowak, D.J., Hirabayashi, S., Bodine, A., Hoehn, R. 2013. Modeled PM2.5 removal by trees in ten U.S. cities and associated health effects. Environmental Pollution. 178: 395-402.");
        values.Add("Nowak, D.J.; Hoehn, R.; Crane, D. 2007. Oxygen production by urban trees in the United States. Arboriculture & Urban Forestry. 33(3):220-226.");
        values.Add("Nowak, D.J.; Hoehn, R.E.; Crane, D.E.; Stevens, J.C.; Walton, J.T; Bond, J. 2008. A ground-based method of assessing urban forest structure and ecosystem services. Arboriculture and Urban Forestry. 34(6): 347-358.");
        values.Add("Nowak, D.J.; Stevens, J.C.; Sisinni, S.M.; Luley, C.J. 2002c. Effects of urban tree management and species selection on atmospheric carbon dioxide. Journal of Arboriculture. 28(3): 113-122.");
        values.Add("Peper, P.J.; McPherson, E.G.; Simpson, J.R.; Albers, S.N.; Xiao, Q. 2010. Central Florida community tree guide: benefits, costs, and strategic planting. Gen. Tech. Rep. PSW-GTR-230. U.S. Department of Agriculture, Forest Service, Pacific Southwest Research Station, Albany, CA.");
        values.Add("Peper, P.J.; McPherson, E.G.; Simpson, J.R.; Vargas, K.E.; Xiao Q. 2009. Lower Midwest community tree guide: benefits, costs, and strategic planting. PSW-GTR-219. Gen. Tech. Rep. PSW-GTR-219. U.S. Department of Agriculture, Forest Service, Pacific Southwest Research Station, Albany, CA.");
        values.Add("U.S. Environmental Protection Agency. 2010. Light-Duty Vehicle Greenhouse Gas Emission Standards and Corporate Average Fuel Economy Standards. Washington, DC: U.S. Environmental Protection Agency. EPA-420-R-10-012a");
        values.Add("U.S. Environmental Protection Agency. 2015. The social cost of carbon. http://www.epa.gov/climatechange/EPAactivities/economics/scc.html");
        values.Add("van Essen, H.; Schroten, A.; Otten, M.; Sutter, D.; Schreyer, C.; Zandonella, R.; Maibach, M.; Doll, C. 2011. External Costs of Transport in Europe. Netherlands: CE Delft. 161 p.");
        values.Add("Vargas, K.E.; McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Gardner, S.L.; Xiao, Q. 2007a. Interior West Tree Guide.");
        values.Add("Vargas, K.E.; McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Gardner, S.L.; Xiao, Q. 2007b. Temperate Interior West Community Tree Guide: Benefits, Costs, and Strategic Planting.");
        values.Add("Vargas, K.E.; McPherson, E.G.; Simpson, J.R.; Peper, P.J.; Gardner, S.L.; Xiao, Q. 2008. Tropical community tree guide: benefits, costs, and strategic planting. PSW-GTR-216. Gen. Tech. Rep. PSW-GTR-216. U.S. Department of Agriculture, Forest Service, Pacific Southwest Research Station, Albany, CA.");
        values.Add("Worrall, J.J. 2007. Chestnut Blight. Forest and Shade Tree Pathology.\r\nhttp://www.forestpathology.org/dis_chestnut.html");
        values.Add("Zinke, P.J. 1967. Forest interception studies in the United States. In: Sopper, W.E.; Lull, H.W., eds. Forest Hydrology. Oxford, UK: Pergamon Press: 137-161.");
        if (this.nation.Id == 219)
        {
          values.Add("National Invasive Species Information Center. 2011. Beltsville, MD: U.S. Department of Agriculture, National Invasive Species Information Center. http://www.invasivespeciesinfo.gov/plants/main.shtml");
          values.Add("Forest Health Technology Enterprise Team. 2014. 2012 National Insect & Disease Risk Maps/Data. Fort Collins, CO: U.S. Department of Agriculture, Forest Service. http://www.fs.fed.us/foresthealth/technology/nidrm2012.shtml");
          values.Add(this.resmanStateInvasives.GetString(this.state.Name + "_References"));
        }
        foreach (PestAffected pestAffected in PestsInLocation)
          values.Add(this.resmanPestReferences.GetString(pestAffected.Abb));
        values.Sort();
        this.AddDefaultParagraph(C1doc, string.Join(string.Format("{0}{1}", (object) Environment.NewLine, (object) Environment.NewLine), (IEnumerable<string>) values));
      }
    }

    private void DrawGradientOnGraphics(Graphics FormGraphics, RenderGraphics rg)
    {
      Unit unit1;
      ref Unit local1 = ref unit1;
      Unit unit2 = rg.Document.PageLayout.PageSettings.Width;
      double num1 = unit2.Value;
      unit2 = rg.Document.PageLayout.PageSettings.LeftMargin;
      double num2 = unit2.Value;
      unit2 = rg.Document.PageLayout.PageSettings.RightMargin;
      double num3 = unit2.Value;
      double num4 = num2 + num3;
      double num5 = num1 - num4;
      unit2 = rg.Document.PageLayout.PageSettings.Width;
      int units1 = (int) unit2.Units;
      local1 = new Unit(num5, (UnitTypeEnum) units1);
      Unit unit3;
      ref Unit local2 = ref unit3;
      Unit unit4 = rg.Document.PageLayout.PageSettings.Height;
      double num6 = unit4.Value;
      unit4 = rg.Document.PageLayout.PageSettings.TopMargin;
      double num7 = unit4.Value;
      unit4 = rg.Document.PageLayout.PageSettings.BottomMargin;
      double num8 = unit4.Value;
      double num9 = num7 + num8;
      double num10 = num6 - num9;
      unit4 = rg.Document.PageLayout.PageSettings.Width;
      int units2 = (int) unit4.Units;
      local2 = new Unit(num10, (UnitTypeEnum) units2);
      Rectangle rect = new Rectangle(0, 0, (int) unit1.ConvertUnit(rg.Document.CreationDpi, UnitTypeEnum.Pixel, FormGraphics.DpiX), (int) unit3.ConvertUnit(rg.Document.CreationDpi, UnitTypeEnum.Pixel, FormGraphics.DpiY));
      LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect, Color.FromArgb(102, 153, 204), Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 90f);
      rg.Graphics.FillRectangle((Brush) linearGradientBrush, rect);
    }

    private void GenerateComparisonOfUrbanForestsTables(C1PrintDocument C1doc)
    {
      this.rText = this.AddTableTitle(i_Tree_Eco_v6.Resources.Strings.WR_Appendix3Table1Title);
      this.rText.Style.Spacing.Top = (Unit) 0;
      C1doc.Body.Children.Add((RenderObject) this.rText);
      RenderTable renderTable1 = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable1);
      renderTable1.Cols[0].Width = (Unit) "24%";
      renderTable1.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
      renderTable1.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable1.Style.GridLines.All = LineDef.Default;
      renderTable1.Style.FontSize = 8f;
      int count1 = 2;
      renderTable1.RowGroups[0, count1].Header = TableHeaderEnum.Page;
      ReportUtil.AddWrittenReportTableHeaderFormat(renderTable1);
      renderTable1.Rows[0].Style.Borders.Bottom = LineDef.Empty;
      renderTable1.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.City;
      renderTable1.Cells[0, 1].Text = i_Tree_Eco_v6.Resources.Strings.PercentTreeCover;
      renderTable1.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.NumberOfTreesV;
      renderTable1.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable1.Cells[1, 3].Text = ReportUtil.FormatHeaderUnitsStr(ReportBase.TonnesUnits());
      renderTable1.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.CarbonSequestration;
      renderTable1.Cells[1, 4].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonnesUnits());
      renderTable1.Cells[0, 5].Text = i_Tree_Eco_v6.Resources.Strings.PollutionRemoval;
      renderTable1.Cells[1, 5].Text = ReportUtil.GetFormattedValuePerYrStr(ReportBase.TonnesUnits());
      int row1 = count1;
      renderTable1.Cells[row1, 0].Text = "Toronto, ON, Canada";
      renderTable1.Cells[row1, 1].Text = 26.6.ToString("N1");
      renderTable1.Cells[row1, 2].Text = 10220000.ToString("N0");
      renderTable1.Cells[row1, 3].Text = (!ReportBase.EnglishUnits ? 1108000 : 1221000).ToString("N0");
      renderTable1.Cells[row1, 4].Text = (!ReportBase.EnglishUnits ? 46700 : 51500).ToString("N0");
      renderTable1.Cells[row1, 5].Text = (!ReportBase.EnglishUnits ? 1904.7 : 2098.9794).ToString("N0");
      int row2 = row1 + 1;
      renderTable1.Cells[row2, 0].Text = "Atlanta, GA";
      renderTable1.Cells[row2, 1].Text = 36.7.ToString("N1");
      renderTable1.Cells[row2, 2].Text = 9415000.ToString("N0");
      renderTable1.Cells[row2, 3].Text = (!ReportBase.EnglishUnits ? 1220000 : 1344000).ToString("N0");
      renderTable1.Cells[row2, 4].Text = (!ReportBase.EnglishUnits ? 42100 : 46400).ToString("N0");
      renderTable1.Cells[row2, 5].Text = (!ReportBase.EnglishUnits ? 1509.0 : 1662.918).ToString("N0");
      int row3 = row2 + 1;
      renderTable1.Cells[row3, 0].Text = "Los Angeles, CA";
      renderTable1.Cells[row3, 1].Text = 11.1.ToString("N1");
      renderTable1.Cells[row3, 2].Text = 5993000.ToString("N0");
      renderTable1.Cells[row3, 3].Text = (!ReportBase.EnglishUnits ? 1151000 : 1269000).ToString("N0");
      renderTable1.Cells[row3, 4].Text = (!ReportBase.EnglishUnits ? 69800 : 77000).ToString("N0");
      renderTable1.Cells[row3, 5].Text = (!ReportBase.EnglishUnits ? 1792.20854901689 : 1975.01382101661).ToString("N0");
      int row4 = row3 + 1;
      renderTable1.Cells[row4, 0].Text = "New York, NY";
      renderTable1.Cells[row4, 1].Text = 20.9.ToString("N1");
      renderTable1.Cells[row4, 2].Text = 5212000.ToString("N0");
      renderTable1.Cells[row4, 3].Text = (!ReportBase.EnglishUnits ? 1225000 : 1350000).ToString("N0");
      renderTable1.Cells[row4, 4].Text = (!ReportBase.EnglishUnits ? 38400 : 42300).ToString("N0");
      renderTable1.Cells[row4, 5].Text = (!ReportBase.EnglishUnits ? 1521.0 : 1676.142).ToString("N0");
      int row5 = row4 + 1;
      renderTable1.Cells[row5, 0].Text = "London, ON, Canada";
      renderTable1.Cells[row5, 1].Text = 24.7.ToString("N1");
      renderTable1.Cells[row5, 2].Text = 4376000.ToString("N0");
      renderTable1.Cells[row5, 3].Text = (!ReportBase.EnglishUnits ? 360000 : 396000).ToString("N0");
      renderTable1.Cells[row5, 4].Text = (!ReportBase.EnglishUnits ? 12500 : 13700).ToString("N0");
      renderTable1.Cells[row5, 5].Text = (!ReportBase.EnglishUnits ? 369.792093885405 : 407.510887461716).ToString("N0");
      int row6 = row5 + 1;
      renderTable1.Cells[row6, 0].Text = "Chicago, IL";
      renderTable1.Cells[row6, 1].Text = 17.2.ToString("N1");
      renderTable1.Cells[row6, 2].Text = 3585000.ToString("N0");
      renderTable1.Cells[row6, 3].Text = (!ReportBase.EnglishUnits ? 649000 : 716000).ToString("N0");
      renderTable1.Cells[row6, 4].Text = (!ReportBase.EnglishUnits ? 22800 : 25200).ToString("N0");
      renderTable1.Cells[row6, 5].Text = (!ReportBase.EnglishUnits ? 806.00432371134 : 888.216764729897).ToString("N0");
      int row7 = row6 + 1;
      renderTable1.Cells[row7, 0].Text = "Phoenix, AZ";
      renderTable1.Cells[row7, 1].Text = 9.0.ToString("N1");
      renderTable1.Cells[row7, 2].Text = 3166000.ToString("N0");
      renderTable1.Cells[row7, 3].Text = (!ReportBase.EnglishUnits ? 286000 : 315000).ToString("N0");
      renderTable1.Cells[row7, 4].Text = (!ReportBase.EnglishUnits ? 29800 : 32800).ToString("N0");
      renderTable1.Cells[row7, 5].Text = (!ReportBase.EnglishUnits ? 511.0 : 562.86).ToString("N0");
      int row8 = row7 + 1;
      renderTable1.Cells[row8, 0].Text = "Baltimore, MD";
      renderTable1.Cells[row8, 1].Text = 21.ToString("N1");
      renderTable1.Cells[row8, 2].Text = 2479000.ToString("N0");
      renderTable1.Cells[row8, 3].Text = (!ReportBase.EnglishUnits ? 517000 : 570000).ToString("N0");
      renderTable1.Cells[row8, 4].Text = (!ReportBase.EnglishUnits ? 16700 : 18400).ToString("N0");
      renderTable1.Cells[row8, 5].Text = (!ReportBase.EnglishUnits ? 390.0 : 429.78).ToString("N0");
      int row9 = row8 + 1;
      renderTable1.Cells[row9, 0].Text = "Philadelphia, PA";
      renderTable1.Cells[row9, 1].Text = 15.7.ToString("N1");
      renderTable1.Cells[row9, 2].Text = 2113000.ToString("N0");
      renderTable1.Cells[row9, 3].Text = (!ReportBase.EnglishUnits ? 481000 : 530000).ToString("N0");
      renderTable1.Cells[row9, 4].Text = (!ReportBase.EnglishUnits ? 14600 : 16100).ToString("N0");
      renderTable1.Cells[row9, 5].Text = (!ReportBase.EnglishUnits ? 522.0 : 575.244).ToString("N0");
      int row10 = row9 + 1;
      renderTable1.Cells[row10, 0].Text = "Washington, DC";
      renderTable1.Cells[row10, 1].Text = 28.6.ToString("N1");
      renderTable1.Cells[row10, 2].Text = 1928000.ToString("N0");
      renderTable1.Cells[row10, 3].Text = (!ReportBase.EnglishUnits ? 477000 : 525000).ToString("N0");
      renderTable1.Cells[row10, 4].Text = (!ReportBase.EnglishUnits ? 14700 : 16200).ToString("N0");
      renderTable1.Cells[row10, 5].Text = (!ReportBase.EnglishUnits ? 379.0 : 417.658).ToString("N0");
      int row11 = row10 + 1;
      renderTable1.Cells[row11, 0].Text = "Oakville, ON , Canada";
      renderTable1.Cells[row11, 1].Text = 29.1.ToString("N1");
      renderTable1.Cells[row11, 2].Text = 1908000.ToString("N0");
      renderTable1.Cells[row11, 3].Text = (!ReportBase.EnglishUnits ? 133000 : 147000).ToString("N0");
      renderTable1.Cells[row11, 4].Text = (!ReportBase.EnglishUnits ? 6000 : 6600).ToString("N0");
      renderTable1.Cells[row11, 5].Text = (!ReportBase.EnglishUnits ? 172.1 : 189.6542).ToString("N0");
      int row12 = row11 + 1;
      renderTable1.Cells[row12, 0].Text = "Albuquerque, NM";
      renderTable1.Cells[row12, 1].Text = 14.3.ToString("N1");
      renderTable1.Cells[row12, 2].Text = 1846000.ToString("N0");
      renderTable1.Cells[row12, 3].Text = (!ReportBase.EnglishUnits ? 301000 : 332000).ToString("N0");
      renderTable1.Cells[row12, 4].Text = (!ReportBase.EnglishUnits ? 9600 : 10600).ToString("N0");
      renderTable1.Cells[row12, 5].Text = (!ReportBase.EnglishUnits ? 225.23 : 248.28).ToString("N0");
      int row13 = row12 + 1 + 1;
      renderTable1.Cells[row13, 0].Text = "Boston, MA";
      renderTable1.Cells[row13, 1].Text = 22.3.ToString("N1");
      renderTable1.Cells[row13, 2].Text = 1183000.ToString("N0");
      renderTable1.Cells[row13, 3].Text = (!ReportBase.EnglishUnits ? 290000 : 319000).ToString("N0");
      renderTable1.Cells[row13, 4].Text = (!ReportBase.EnglishUnits ? 9500 : 10500).ToString("N0");
      renderTable1.Cells[row13, 5].Text = (!ReportBase.EnglishUnits ? 257.0 : 283.214).ToString("N0");
      int row14 = row13 + 1;
      renderTable1.Cells[row14, 0].Text = "Syracuse, NY";
      renderTable1.Cells[row14, 1].Text = 26.9.ToString("N1");
      renderTable1.Cells[row14, 2].Text = 1088000.ToString("N0");
      renderTable1.Cells[row14, 3].Text = (!ReportBase.EnglishUnits ? 166000 : 183000).ToString("N0");
      renderTable1.Cells[row14, 4].Text = (!ReportBase.EnglishUnits ? 5300 : 5900).ToString("N0");
      renderTable1.Cells[row14, 5].Text = (!ReportBase.EnglishUnits ? 99.0 : 109.098).ToString("N0");
      int row15 = row14 + 1;
      renderTable1.Cells[row15, 0].Text = "Woodbridge, NJ";
      renderTable1.Cells[row15, 1].Text = 29.5.ToString("N1");
      renderTable1.Cells[row15, 2].Text = 986000.ToString("N0");
      renderTable1.Cells[row15, 3].Text = (!ReportBase.EnglishUnits ? 145000 : 160000).ToString("N0");
      renderTable1.Cells[row15, 4].Text = (!ReportBase.EnglishUnits ? 5000 : 5600).ToString("N0");
      renderTable1.Cells[row15, 5].Text = (!ReportBase.EnglishUnits ? 191.0 : 210.482).ToString("N0");
      int row16 = row15 + 1;
      renderTable1.Cells[row16, 0].Text = "Minneapolis, MN";
      renderTable1.Cells[row16, 1].Text = 26.4.ToString("N1");
      renderTable1.Cells[row16, 2].Text = 979000.ToString("N0");
      renderTable1.Cells[row16, 3].Text = (!ReportBase.EnglishUnits ? 227000 : 250000).ToString("N0");
      renderTable1.Cells[row16, 4].Text = (!ReportBase.EnglishUnits ? 8100 : 8900).ToString("N0");
      renderTable1.Cells[row16, 5].Text = (!ReportBase.EnglishUnits ? 277.0 : 305.254).ToString("N0");
      int row17 = row16 + 1;
      renderTable1.Cells[row17, 0].Text = "San Francisco, CA";
      renderTable1.Cells[row17, 1].Text = 11.9.ToString("N1");
      renderTable1.Cells[row17, 2].Text = 668000.ToString("N0");
      renderTable1.Cells[row17, 3].Text = (!ReportBase.EnglishUnits ? 176000 : 194000).ToString("N0");
      renderTable1.Cells[row17, 4].Text = (!ReportBase.EnglishUnits ? 4600 : 5100).ToString("N0");
      renderTable1.Cells[row17, 5].Text = (!ReportBase.EnglishUnits ? 128.0 : 141.056).ToString("N0");
      int row18 = row17 + 1;
      renderTable1.Cells[row18, 0].Text = "Morgantown, WV";
      renderTable1.Cells[row18, 1].Text = 35.5.ToString("N1");
      renderTable1.Cells[row18, 2].Text = 658000.ToString("N0");
      renderTable1.Cells[row18, 3].Text = (!ReportBase.EnglishUnits ? 84000 : 93000).ToString("N0");
      renderTable1.Cells[row18, 4].Text = (!ReportBase.EnglishUnits ? 2600 : 2900).ToString("N0");
      renderTable1.Cells[row18, 5].Text = (!ReportBase.EnglishUnits ? 65.17265 : 71.8202603).ToString("N0");
      int row19 = row18 + 1;
      renderTable1.Cells[row19, 0].Text = "Moorestown, NJ";
      renderTable1.Cells[row19, 1].Text = 28.ToString("N1");
      renderTable1.Cells[row19, 2].Text = 583000.ToString("N0");
      renderTable1.Cells[row19, 3].Text = (!ReportBase.EnglishUnits ? 106000 : 117000).ToString("N0");
      renderTable1.Cells[row19, 4].Text = (!ReportBase.EnglishUnits ? 3400 : 3800).ToString("N0");
      renderTable1.Cells[row19, 5].Text = (!ReportBase.EnglishUnits ? 107.0 : 117.914).ToString("N0");
      int row20 = row19 + 1;
      renderTable1.Cells[row20, 0].Text = "Hartford, CT";
      renderTable1.Cells[row20, 1].Text = 25.9.ToString("N1");
      renderTable1.Cells[row20, 2].Text = 568000.ToString("N0");
      renderTable1.Cells[row20, 3].Text = (!ReportBase.EnglishUnits ? 130000 : 143000).ToString("N0");
      renderTable1.Cells[row20, 4].Text = (!ReportBase.EnglishUnits ? 3900 : 4300).ToString("N0");
      renderTable1.Cells[row20, 5].Text = (!ReportBase.EnglishUnits ? 52.3269936196319 : 57.6643469688344).ToString("N0");
      int row21 = row20 + 1;
      renderTable1.Cells[row21, 0].Text = "Jersey City, NJ";
      renderTable1.Cells[row21, 1].Text = 11.5.ToString("N1");
      renderTable1.Cells[row21, 2].Text = 136000.ToString("N0");
      renderTable1.Cells[row21, 3].Text = (!ReportBase.EnglishUnits ? 19000 : 21000).ToString("N0");
      renderTable1.Cells[row21, 4].Text = (!ReportBase.EnglishUnits ? 800 : 890).ToString("N0");
      renderTable1.Cells[row21, 5].Text = (!ReportBase.EnglishUnits ? 37.0 : 40.774).ToString("N0");
      int row22 = row21 + 1;
      renderTable1.Cells[row22, 0].Text = "Casper, WY";
      renderTable1.Cells[row22, 1].Text = 8.9.ToString("N1");
      renderTable1.Cells[row22, 2].Text = 123000.ToString("N0");
      renderTable1.Cells[row22, 3].Text = (!ReportBase.EnglishUnits ? 34000 : 37000).ToString("N0");
      renderTable1.Cells[row22, 4].Text = (!ReportBase.EnglishUnits ? 1100 : 1200).ToString("N0");
      renderTable1.Cells[row22, 5].Text = (!ReportBase.EnglishUnits ? 34.0 : 37.468).ToString("N0");
      int row23 = row22 + 1;
      renderTable1.Cells[row23, 0].Text = "Freehold, NJ";
      renderTable1.Cells[row23, 1].Text = 34.4.ToString("N1");
      renderTable1.Cells[row23, 2].Text = 48000.ToString("N0");
      renderTable1.Cells[row23, 3].Text = (!ReportBase.EnglishUnits ? 18000 : 20000).ToString("N0");
      renderTable1.Cells[row23, 4].Text = (!ReportBase.EnglishUnits ? 500 : 540).ToString("N0");
      renderTable1.Cells[row23, 5].Text = (!ReportBase.EnglishUnits ? 20.0 : 22.04).ToString("N0");
      ReportUtil.FormatRenderTableWrittenReport(renderTable1);
      this.rText = this.AddTableTitle(string.Format(i_Tree_Eco_v6.Resources.Strings.WR_Appendix3Table2Title, (object) ReportBase.HectareUnits()));
      this.rText.Style.Spacing.Top = (Unit) 0;
      C1doc.Body.Children.Add((RenderObject) this.rText);
      RenderTable renderTable2 = new RenderTable();
      C1doc.Body.Children.Add((RenderObject) renderTable2);
      renderTable2.Cols[0].Width = (Unit) "24%";
      renderTable2.Cols[1].Width = (Unit) "22%";
      renderTable2.Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Right;
      renderTable2.Cols[0].Style.TextAlignHorz = C1.C1Preview.AlignHorzEnum.Left;
      renderTable2.Style.GridLines.All = LineDef.Default;
      renderTable2.Style.FontSize = 8f;
      int count2 = 2;
      renderTable2.RowGroups[0, count2].Header = TableHeaderEnum.Page;
      ReportUtil.AddWrittenReportTableHeaderFormat(renderTable2);
      renderTable2.Rows[0].Style.Borders.Bottom = LineDef.Empty;
      renderTable2.Cells[0, 0].Text = i_Tree_Eco_v6.Resources.Strings.City;
      renderTable2.Cells[0, 1].Text = string.Format(i_Tree_Eco_v6.Resources.Strings.NumberOfTreesPerArea, (object) ReportBase.HaUnits());
      string valuePerValueStr = ReportUtil.GetValuePerValueStr(ReportBase.TonnesUnits(), ReportBase.HaUnits());
      renderTable2.Cells[0, 2].Text = i_Tree_Eco_v6.Resources.Strings.CarbonStorage;
      renderTable2.Cells[1, 2].Text = ReportUtil.FormatHeaderUnitsStr(valuePerValueStr);
      renderTable2.Cells[0, 3].Text = i_Tree_Eco_v6.Resources.Strings.CarbonSequestration;
      renderTable2.Cells[1, 3].Text = ReportUtil.GetFormattedValuePerYrStr(valuePerValueStr);
      renderTable2.Cells[0, 4].Text = i_Tree_Eco_v6.Resources.Strings.PollutionRemoval;
      renderTable2.Cells[1, 4].Text = ReportUtil.GetFormattedValuePerYrStr(ReportUtil.GetValuePerValueStr(ReportBase.KgUnits(), ReportBase.HaUnits()));
      int row24 = count2;
      renderTable2.Cells[row24, 0].Text = "Toronto, ON, Canada";
      renderTable2.Cells[row24, 1].Text = (!ReportBase.EnglishUnits ? 160.4 : 64.9129906920275).ToString("N1");
      renderTable2.Cells[row24, 2].Text = (!ReportBase.EnglishUnits ? 17.3811 : 7.75150635370295).ToString("N1");
      renderTable2.Cells[row24, 3].Text = (!ReportBase.EnglishUnits ? 0.7335 : 0.327121408336706).ToString("N2");
      renderTable2.Cells[row24, 4].Text = (!ReportBase.EnglishUnits ? 29.8884725463701 : 26.6589208790772).ToString("N1");
      int row25 = row24 + 1;
      renderTable2.Cells[row25, 0].Text = "Atlanta, GA";
      renderTable2.Cells[row25, 1].Text = (!ReportBase.EnglishUnits ? 275.8 : 111.614730878187).ToString("N1");
      renderTable2.Cells[row25, 2].Text = (!ReportBase.EnglishUnits ? 35.7372 : 15.9378366653177).ToString("N1");
      renderTable2.Cells[row25, 3].Text = (!ReportBase.EnglishUnits ? 1.2339 : 0.550286442735735).ToString("N2");
      renderTable2.Cells[row25, 4].Text = (!ReportBase.EnglishUnits ? 44.2016203159903 : 39.4254840859744).ToString("N1");
      int row26 = row25 + 1;
      renderTable2.Cells[row26, 0].Text = "Los Angeles, CA";
      renderTable2.Cells[row26, 1].Text = (!ReportBase.EnglishUnits ? 48.4 : 19.5872116552003).ToString("N1");
      renderTable2.Cells[row26, 2].Text = (!ReportBase.EnglishUnits ? 9.4477 : 4.21342185350061).ToString("N1");
      renderTable2.Cells[row26, 3].Text = (!ReportBase.EnglishUnits ? 0.3633 : 0.16202209631728).ToString("N2");
      renderTable2.Cells[row26, 4].Text = (!ReportBase.EnglishUnits ? 14.7174975694063 : 13.1272216280743).ToString("N1");
      int row27 = row26 + 1;
      renderTable2.Cells[row27, 0].Text = "New York, NY";
      renderTable2.Cells[row27, 1].Text = (!ReportBase.EnglishUnits ? 65.2 : 26.3860785107244).ToString("N1");
      renderTable2.Cells[row27, 2].Text = (!ReportBase.EnglishUnits ? 15.3288 : 6.83623537029543).ToString("N1");
      renderTable2.Cells[row27, 3].Text = (!ReportBase.EnglishUnits ? 0.4799 : 0.214022581950627).ToString("N2");
      renderTable2.Cells[row27, 4].Text = (!ReportBase.EnglishUnits ? 19.02917193326 : 16.9730048324181).ToString("N1");
      int row28 = row27 + 1;
      renderTable2.Cells[row28, 0].Text = "London, ON, Canada";
      renderTable2.Cells[row28, 1].Text = (!ReportBase.EnglishUnits ? 185.5 : 75.070821529745).ToString("N1");
      renderTable2.Cells[row28, 2].Text = (!ReportBase.EnglishUnits ? 15.2502 : 6.80118186968839).ToString("N1");
      renderTable2.Cells[row28, 3].Text = (!ReportBase.EnglishUnits ? 0.5278 : 0.235384702549575).ToString("N2");
      renderTable2.Cells[row28, 4].Text = (!ReportBase.EnglishUnits ? 15.6753469541957 : 13.9815721113101).ToString("N1");
      int row29 = row28 + 1;
      renderTable2.Cells[row29, 0].Text = "Chicago, IL";
      renderTable2.Cells[row29, 1].Text = (!ReportBase.EnglishUnits ? 59.9 : 24.2411978955888).ToString("N1");
      renderTable2.Cells[row29, 2].Text = (!ReportBase.EnglishUnits ? 6786.0 / 625.0 : 4.84219959530555).ToString("N1");
      renderTable2.Cells[row29, 3].Text = (!ReportBase.EnglishUnits ? 0.3818 : 0.17027260218535).ToString("N2");
      renderTable2.Cells[row29, 4].Text = (!ReportBase.EnglishUnits ? 13.4772175833495 : 12.0209581358569).ToString("N1");
      int row30 = row29 + 1;
      renderTable2.Cells[row30, 0].Text = "Phoenix, AZ";
      renderTable2.Cells[row30, 1].Text = (!ReportBase.EnglishUnits ? 31.8 : 12.9).ToString("N1");
      renderTable2.Cells[row30, 2].Text = (!ReportBase.EnglishUnits ? 2.87 : 1.28).ToString("N1");
      renderTable2.Cells[row30, 3].Text = (!ReportBase.EnglishUnits ? 0.3 : 0.13).ToString("N2");
      renderTable2.Cells[row30, 4].Text = (!ReportBase.EnglishUnits ? 5.13 : 4.57).ToString("N1");
      int row31 = row30 + 1;
      renderTable2.Cells[row31, 0].Text = "Baltimore, MD";
      renderTable2.Cells[row31, 1].Text = (!ReportBase.EnglishUnits ? 118.5 : 47.9562929987859).ToString("N1");
      renderTable2.Cells[row31, 2].Text = (!ReportBase.EnglishUnits ? 24.9518 : 11.1278363415621).ToString("N1");
      renderTable2.Cells[row31, 3].Text = (!ReportBase.EnglishUnits ? 0.8038 : 0.35847333063537).ToString("N2");
      renderTable2.Cells[row31, 4].Text = (!ReportBase.EnglishUnits ? 18.644835954128 : 16.6301976701328).ToString("N1");
      int row32 = row31 + 1;
      renderTable2.Cells[row32, 0].Text = "Philadelphia, PA";
      renderTable2.Cells[row32, 1].Text = (!ReportBase.EnglishUnits ? 61.9 : 25.0505868069607).ToString("N1");
      renderTable2.Cells[row32, 2].Text = (!ReportBase.EnglishUnits ? 8803.0 / 625.0 : 6.28144459732902).ToString("N1");
      renderTable2.Cells[row32, 3].Text = (!ReportBase.EnglishUnits ? 0.4281 : 0.190921165520032).ToString("N2");
      renderTable2.Cells[row32, 4].Text = (!ReportBase.EnglishUnits ? 15.2842564253169 : 13.6327402514765).ToString("N1");
      int row33 = row32 + 1;
      renderTable2.Cells[row33, 0].Text = "Washington, DC";
      renderTable2.Cells[row33, 1].Text = (!ReportBase.EnglishUnits ? 121.1 : 49.0084985835694).ToString("N1");
      renderTable2.Cells[row33, 2].Text = (!ReportBase.EnglishUnits ? 29.8098 : 13.2943745851882).ToString("N1");
      renderTable2.Cells[row33, 3].Text = (!ReportBase.EnglishUnits ? 0.9205 : 0.410518413597734).ToString("N2");
      renderTable2.Cells[row33, 4].Text = (!ReportBase.EnglishUnits ? 23.8142663163854 : 21.2410534040119).ToString("N1");
      int row34 = row33 + 1;
      renderTable2.Cells[row34, 0].Text = "Oakville, ON , Canada";
      renderTable2.Cells[row34, 1].Text = (!ReportBase.EnglishUnits ? 192.9 : 78.0655605018211).ToString("N1");
      renderTable2.Cells[row34, 2].Text = (!ReportBase.EnglishUnits ? 8403.0 / 625.0 : 5.99602169162282).ToString("N1");
      renderTable2.Cells[row34, 3].Text = (!ReportBase.EnglishUnits ? 0.6055 : 0.270036827195467).ToString("N2");
      renderTable2.Cells[row34, 4].Text = (!ReportBase.EnglishUnits ? 12.3850550310958 : 11.0468074822076).ToString("N1");
      int row35 = row34 + 1;
      renderTable2.Cells[row35, 0].Text = "Albuquerque, NM";
      renderTable2.Cells[row35, 1].Text = (!ReportBase.EnglishUnits ? 53.9 : 21.8).ToString("N1");
      renderTable2.Cells[row35, 2].Text = (!ReportBase.EnglishUnits ? 8.78 : 3.92).ToString("N1");
      renderTable2.Cells[row35, 3].Text = (!ReportBase.EnglishUnits ? 0.28 : 0.12).ToString("N2");
      renderTable2.Cells[row35, 4].Text = (!ReportBase.EnglishUnits ? 6.58 : 5.87).ToString("N1");
      int row36 = row35 + 1;
      renderTable2.Cells[row36, 0].Text = "Boston, MA";
      renderTable2.Cells[row36, 1].Text = (!ReportBase.EnglishUnits ? 82.9 : 33.5491703763658).ToString("N1");
      renderTable2.Cells[row36, 2].Text = (!ReportBase.EnglishUnits ? 20.2955 : 9.05125091056253).ToString("N1");
      renderTable2.Cells[row36, 3].Text = (!ReportBase.EnglishUnits ? 0.6677 : 0.297776365843788).ToString("N2");
      renderTable2.Cells[row36, 4].Text = (!ReportBase.EnglishUnits ? 17.9981693690766 : 16.0534056209813).ToString("N1");
      int row37 = row36 + 1;
      renderTable2.Cells[row37, 0].Text = "Syracuse, NY";
      renderTable2.Cells[row37, 1].Text = (!ReportBase.EnglishUnits ? 167.4 : 67.7458518818292).ToString("N1");
      renderTable2.Cells[row37, 2].Text = (!ReportBase.EnglishUnits ? 23.0941 : 10.2993517604209).ToString("N1");
      renderTable2.Cells[row37, 3].Text = (!ReportBase.EnglishUnits ? 0.7672 : 0.342150708215297).ToString("N2");
      renderTable2.Cells[row37, 4].Text = (!ReportBase.EnglishUnits ? 15.2281921520973 : 13.5827339146995).ToString("N1");
      int row38 = row37 + 1;
      renderTable2.Cells[row38, 0].Text = "Woodbridge, NJ";
      renderTable2.Cells[row38, 1].Text = (!ReportBase.EnglishUnits ? 164.4 : 66.5317685147713).ToString("N1");
      renderTable2.Cells[row38, 2].Text = (!ReportBase.EnglishUnits ? 24.1641 : 10.7765431808984).ToString("N1");
      renderTable2.Cells[row38, 3].Text = (!ReportBase.EnglishUnits ? 0.8414 : 0.375241926345609).ToString("N2");
      renderTable2.Cells[row38, 4].Text = (!ReportBase.EnglishUnits ? 31.8543572090913 : 28.4123849813182).ToString("N1");
      int row39 = row38 + 1;
      renderTable2.Cells[row39, 0].Text = "Minneapolis, MN";
      renderTable2.Cells[row39, 1].Text = (!ReportBase.EnglishUnits ? 64.8 : 26.22420072845).ToString("N1");
      renderTable2.Cells[row39, 2].Text = (!ReportBase.EnglishUnits ? 15.0343 : 6.70489623634156).ToString("N1");
      renderTable2.Cells[row39, 3].Text = (!ReportBase.EnglishUnits ? 0.534 : 0.238149736948604).ToString("N2");
      renderTable2.Cells[row39, 4].Text = (!ReportBase.EnglishUnits ? 18.329925423011 : 16.3493142987925).ToString("N1");
      int row40 = row39 + 1;
      renderTable2.Cells[row40, 0].Text = "San Francisco, CA";
      renderTable2.Cells[row40, 1].Text = (!ReportBase.EnglishUnits ? 55.7 : 22.5414811817078).ToString("N1");
      renderTable2.Cells[row40, 2].Text = (!ReportBase.EnglishUnits ? 14.6873 : 6.55014350465399).ToString("N1");
      renderTable2.Cells[row40, 3].Text = (!ReportBase.EnglishUnits ? 0.3859 : 0.17210109267503).ToString("N2");
      renderTable2.Cells[row40, 4].Text = (!ReportBase.EnglishUnits ? 10.6773618240938 : 9.5236363659663).ToString("N1");
      int row41 = row40 + 1;
      renderTable2.Cells[row41, 0].Text = "Morgantown, WV";
      renderTable2.Cells[row41, 1].Text = (!ReportBase.EnglishUnits ? 294.5 : 119.182517199514).ToString("N1");
      renderTable2.Cells[row41, 2].Text = (!ReportBase.EnglishUnits ? 37.719 : 16.8216665317685).ToString("N1");
      renderTable2.Cells[row41, 3].Text = (!ReportBase.EnglishUnits ? 734.0 / 625.0 : 0.523751031970862).ToString("N2");
      renderTable2.Cells[row41, 4].Text = (!ReportBase.EnglishUnits ? 29.1600223713646 : 26.0091822365389).ToString("N1");
      int row42 = row41 + 1;
      renderTable2.Cells[row42, 0].Text = "Moorestown, NJ";
      renderTable2.Cells[row42, 1].Text = (!ReportBase.EnglishUnits ? 153.4 : 62.0801295022258).ToString("N1");
      renderTable2.Cells[row42, 2].Text = (!ReportBase.EnglishUnits ? 27.8543 : 12.4222738162687).ToString("N1");
      renderTable2.Cells[row42, 3].Text = (!ReportBase.EnglishUnits ? 0.8965 : 0.399815054633751).ToString("N2");
      renderTable2.Cells[row42, 4].Text = (!ReportBase.EnglishUnits ? 28.1401220281927 : 25.0994856131674).ToString("N1");
      int row43 = row42 + 1;
      renderTable2.Cells[row43, 0].Text = "Hartford, CT";
      renderTable2.Cells[row43, 1].Text = (!ReportBase.EnglishUnits ? 124.6 : 50.4249291784703).ToString("N1");
      renderTable2.Cells[row43, 2].Text = (!ReportBase.EnglishUnits ? 28.5222 : 12.7201393767705).ToString("N1");
      renderTable2.Cells[row43, 3].Text = (!ReportBase.EnglishUnits ? 0.8631 : 0.38491954674221).ToString("N2");
      renderTable2.Cells[row43, 4].Text = (!ReportBase.EnglishUnits ? 11.4752178990421 : 10.2352813636134).ToString("N1");
      int row44 = row43 + 1;
      renderTable2.Cells[row44, 0].Text = "Jersey City, NJ";
      renderTable2.Cells[row44, 1].Text = (!ReportBase.EnglishUnits ? 35.5 : 14.3666531768515).ToString("N1");
      renderTable2.Cells[row44, 2].Text = (!ReportBase.EnglishUnits ? 5.0249 : 2.24097118575476).ToString("N1");
      renderTable2.Cells[row44, 3].Text = (!ReportBase.EnglishUnits ? 0.2105 : 0.0938773775799272).ToString("N2");
      renderTable2.Cells[row44, 4].Text = (!ReportBase.EnglishUnits ? 9.64478516892711 : 8.60263314945987).ToString("N1");
      int row45 = row44 + 1;
      renderTable2.Cells[row45, 0].Text = "Casper, WY";
      renderTable2.Cells[row45, 1].Text = (!ReportBase.EnglishUnits ? 22.5 : 9.10562525293403).ToString("N1");
      renderTable2.Cells[row45, 2].Text = (!ReportBase.EnglishUnits ? 6.2056 : 2.76753184945366).ToString("N1");
      renderTable2.Cells[row45, 3].Text = (!ReportBase.EnglishUnits ? 0.1967 : 0.0877229461756374).ToString("N2");
      renderTable2.Cells[row45, 4].Text = (!ReportBase.EnglishUnits ? 6.22015696749053 : 5.54804773628051).ToString("N1");
      int row46 = row45 + 1;
      renderTable2.Cells[row46, 0].Text = "Freehold, NJ";
      renderTable2.Cells[row46, 1].Text = (!ReportBase.EnglishUnits ? 94.6 : 38.2840955078915).ToString("N1");
      renderTable2.Cells[row46, 2].Text = (!ReportBase.EnglishUnits ? 35.886 : 16.0041974908944).ToString("N1");
      renderTable2.Cells[row46, 3].Text = (!ReportBase.EnglishUnits ? 0.9791 : 0.436652448401457).ToString("N2");
      renderTable2.Cells[row46, 4].Text = (!ReportBase.EnglishUnits ? 39.6086663762031 : 35.3288145257595).ToString("N1");
      ReportUtil.FormatRenderTableWrittenReport(renderTable2);
    }

    private List<GroundCoverPercent> GetGroundCoversExcludingTreesAndShrubs()
    {
      IList<(int, double)> tupleList = this.estUtil.queryProvider.GetEstimateUtilProvider().GetGroundCoversExcludingTreesAndShrubs(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Cover, new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.GroundCover
      })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.GroundCover]).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CoverArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetParameter<short>("treeGroundCoverOrder", this.estUtil.GetClassValueOrderFromName(Classifiers.GroundCover, "TREE")).SetParameter<short>("shrubGroundCoverOrder", this.estUtil.GetClassValueOrderFromName(Classifiers.GroundCover, "SHRUB")).SetParameter<short>("plantSpaceCoverOrder", this.estUtil.GetClassValueOrderFromName(Classifiers.GroundCover, "PLANTABLE SPACE")).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double)>()).List<(int, double)>();
      List<GroundCoverPercent> excludingTreesAndShrubs = new List<GroundCoverPercent>();
      foreach ((int key, double num) in (IEnumerable<(int, double)>) tupleList)
        excludingTreesAndShrubs.Add(new GroundCoverPercent()
        {
          Name = this.estUtil.ClassValues[Classifiers.GroundCover][(short) key].Item1,
          PercentCover = num
        });
      return excludingTreesAndShrubs;
    }

    private List<ImportanceValueSpecies> GetImportanceValues(
      IList<(int SpeciesOrder, double TreeNo, double LeafArea)> data,
      double totalTreeCount,
      double totalLeafArea)
    {
      return data.Select<(int, double, double), ImportanceValueSpecies>((Func<(int, double, double), ImportanceValueSpecies>) (r => new ImportanceValueSpecies()
      {
        SpeciesCVO = (short) r.SpeciesOrder,
        TreeCount = r.TreeNo,
        LeafArea = r.LeafArea,
        PercentTreeNumber = r.TreeNo / totalTreeCount * 100.0,
        PercentLeafArea = r.LeafArea / totalLeafArea * 100.0
      })).ToList<ImportanceValueSpecies>();
    }

    private IQuery GetSpeciesCountsWithLeafAreaQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetSpeciesCountsWithLeafArea(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    private IList<(int SpeciesOrder, double TreeNo, double LeafArea)> GetSpeciesCountsWithLeafArea() => this.GetSpeciesCountsWithLeafAreaQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "TOTAL")).SetParameter<int>("estTypeNumTrees", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<EquationTypes>("eqTypeNone", EquationTypes.None).SetParameter<int>("countUnit", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<int>("estTypeLeafArea", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<EquationTypes>("eqTypeNoneLA", EquationTypes.None).SetParameter<int>("estUnitLeafArea", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)]).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double, double)>()).List<(int, double, double)>();

    private SortedList<double, InvasiveSpecies> GetInvasiveSpeciesList(
      IList<(int SpeciesOrder, double TreeNo, double LeafArea)> data,
      double totalTreeCount,
      double totalLeafArea)
    {
      SortedList<double, InvasiveSpecies> invasiveSpeciesList = new SortedList<double, InvasiveSpecies>((IComparer<double>) new ReverseComparer<double>());
      List<string> list = this.LocationService.GetInvasiveSpecies(this.state.Id).Select<Species, string>((Func<Species, string>) (sp => sp.CommonName)).ToList<string>();
      double key = 0.0;
      foreach ((int SpeciesOrder, double TreeNo, double LeafArea) tuple in (IEnumerable<(int SpeciesOrder, double TreeNo, double LeafArea)>) data)
      {
        string str = this.estUtil.ClassValues[Classifiers.Species][(short) tuple.SpeciesOrder].Item1;
        if (list.Contains<string>(str, (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase))
        {
          --key;
          invasiveSpeciesList.Add(key, new InvasiveSpecies()
          {
            SpeciesCVO = (short) tuple.SpeciesOrder,
            TreeCount = tuple.TreeNo,
            LeafArea = tuple.LeafArea,
            PercentTreeNumber = tuple.TreeNo / totalTreeCount * 100.0,
            PercentLeafArea = tuple.LeafArea / totalLeafArea * 100.0
          });
        }
      }
      return invasiveSpeciesList;
    }

    private List<LanduseLeafArea> GetLanduseLeafAreas() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new TupleResultTransformer<(int, double)>()).List<(int, double)>().Select<(int, double), LanduseLeafArea>((Func<(int, double), LanduseLeafArea>) (r => new LanduseLeafArea()
    {
      Landuse = this.estUtil.ClassValues[Classifiers.Strata][(short) r.landuse].Item1,
      LeafAreaKm = r.estimatedValue
    })).ToList<LanduseLeafArea>();

    private double GetCarbonStorage() => ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatedValueSumForStrata().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CarbonStorage]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>());

    private double GetTreeCounts() => ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatedValueSumForStrata().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>());

    private double GetGrossCarbonSequestration() => ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatedValueSumForStrata().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>());

    private double GetStructuralValue()
    {
      double num = ReportUtil.ConvertFromDBVal<double>((object) this.GetEstimatedValuesSum().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CompensatoryValue]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Monetaryunit, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "TOTAL")).UniqueResult<double>());
      return !NationFeatures.IsUSorUSlikeNation(this.nation.Id) ? num * this.currencyExchangeRate : num;
    }

    private double GetOverallTreeDensity() => ReportUtil.ConvertFromDBVal<double>((object) this.GetStrataEstimatedValuesByClassifier().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.Hectare, Units.None)]).SetParameter<short>("classifier", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>());

    public DataTable GetLanduseTopThreeTreeDensities() => this.GetStrataEstimatedValues().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.Hectare, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>().AsEnumerable().Take<DataRow>(3).CopyToDataTable<DataRow>();

    public string GetLanduseTopThreeTreeDensitiesString(DataTable data)
    {
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Strata];
      string treeDensitiesString = string.Empty;
      int num = 0;
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        string str = this.estUtil.ClassValues[Classifiers.Strata][(short) (int) row[classifierName]].Item1;
        if (num == 0)
          treeDensitiesString = str;
        if (num == 1)
          treeDensitiesString += string.Format(" {0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.WR_FollowedBy, (object) str);
        if (num == 2)
          treeDensitiesString += string.Format(" {0} {1}", (object) i_Tree_Eco_v6.Resources.Strings.And, (object) str);
        ++num;
      }
      return treeDensitiesString;
    }

    private double GetPercentNativeContinent() => this.GetPercentNativeContinentQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<short>("classifier", this.estUtil.GetClassValueOrderFromName(Classifiers.Continent, this.continent.Name)).SetParameter<short>("totCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>();

    private double GetPercentNativeState() => this.GetPercentNativeContinentQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<short>("classifier", this.estUtil.GetClassValueOrderFromName(Classifiers.Continent, "STATE")).SetParameter<short>("totCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>();

    private DataTable GetSpeciesPercentExoticToContinent() => this.GetTotalsExoticToContinentQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<short>("continent", this.estUtil.GetClassValueOrderFromName(Classifiers.Continent, this.continent.Name)).SetParameter<short>("state", this.estUtil.GetClassValueOrderFromName(Classifiers.Continent, "STATE")).SetParameter<short>("totCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private double GetPercentExoticToContinent(DataTable data) => data.AsEnumerable().Sum<DataRow>((Func<DataRow, double>) (r => r.Field<double>("EstimateValue")));

    private double GetPercentExoticHighestSourceContinent(DataTable data) => data.Rows[0].Field<double>("EstimateValue");

    private IQuery GetPercentNativeContinentQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetTotalsSumEstimateValueByClassifier(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Continent
    })], this.estUtil.ClassifierNames[Classifiers.Continent], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private IQuery GetTotalsExoticToContinentQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetTotalsExoticToContinent(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.Continent
    })], this.estUtil.ClassifierNames[Classifiers.Continent], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private IQuery GetEstimatedValueSumForStrata() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValueSum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private double GetAvgCoverPercent() => ReportUtil.ConvertFromDBVal<double>((object) this.GetAvgCoverPercentQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CoverArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<short>("c1", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetParameter<short>("c2", this.estUtil.GetClassValueOrderFromName(Classifiers.GroundCover, "TREE")).UniqueResult<double>());

    private double GetShrubCoverPercent() => ReportUtil.ConvertFromDBVal<double>((object) this.GetAvgCoverPercentQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CoverArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<short>("c1", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetParameter<short>("c2", this.estUtil.GetClassValueOrderFromName(Classifiers.GroundCover, "SHRUB")).UniqueResult<double>());

    private IQuery GetAvgCoverPercentQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesForClassifier2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Cover, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.GroundCover
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.GroundCover]);

    private SortedList<int, double> GetDbhTreeCount(
      Dictionary<int, double> SpeciesCountDictionary)
    {
      Dictionary<int, Dictionary<int, double>> compositionByStrata = this.GetLandUseCompositionByStrata();
      SortedList<int, double> dbhTreeCount = new SortedList<int, double>();
      SortedList<short, Tuple<string, string>> classValue = this.estUtil.ClassValues[Classifiers.CDBH];
      foreach (KeyValuePair<int, double> speciesCount in SpeciesCountDictionary)
      {
        foreach (KeyValuePair<short, Tuple<string, string>> keyValuePair in classValue)
        {
          if (!dbhTreeCount.ContainsKey((int) keyValuePair.Key))
            dbhTreeCount.Add((int) keyValuePair.Key, 0.0);
          if (compositionByStrata.ContainsKey(speciesCount.Key))
          {
            Dictionary<int, double> dictionary = compositionByStrata[speciesCount.Key];
            if (dictionary.ContainsKey((int) keyValuePair.Key))
              dbhTreeCount[(int) keyValuePair.Key] += dictionary[(int) keyValuePair.Key] / 100.0 * speciesCount.Value;
          }
        }
      }
      return dbhTreeCount;
    }

    private Dictionary<int, Dictionary<int, double>> GetLandUseCompositionByStrata()
    {
      DataTable dataTable = this.GetEstimatedValues2().SetParameter<Guid?>("y", ReportBase.m_ps.InputSession.YearKey).SetParameter<int>("estType", 1).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Percent, Units.None, Units.None)]).SetParameter<EquationTypes>("eqType", EquationTypes.None).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      Dictionary<int, Dictionary<int, double>> compositionByStrata = new Dictionary<int, Dictionary<int, double>>();
      string classifierName1 = this.estUtil.ClassifierNames[Classifiers.CDBH];
      string classifierName2 = this.estUtil.ClassifierNames[Classifiers.Species];
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        int key = (int) row[classifierName2];
        if (!compositionByStrata.ContainsKey(key))
          compositionByStrata.Add(key, new Dictionary<int, double>());
        if (!compositionByStrata[key].ContainsKey((int) row[classifierName1]))
          compositionByStrata[key][(int) row[classifierName1]] = 0.0;
        compositionByStrata[key][(int) row[classifierName1]] += (double) row["EstimateValue"];
      }
      return compositionByStrata;
    }

    private IQuery GetEstimatedValues2() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValues2(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species,
      Classifiers.CDBH
    })], this.estUtil.ClassifierNames[Classifiers.Species], this.estUtil.ClassifierNames[Classifiers.CDBH]);

    private Rectangle GetPageRectanglePixels(C1PrintDocument C1doc, Graphics FormGraphics)
    {
      Unit unit1;
      ref Unit local1 = ref unit1;
      Unit unit2 = C1doc.PageLayout.PageSettings.Width;
      double num1 = unit2.Value;
      unit2 = C1doc.PageLayout.PageSettings.LeftMargin;
      double num2 = unit2.Value;
      unit2 = C1doc.PageLayout.PageSettings.RightMargin;
      double num3 = unit2.Value;
      double num4 = num2 + num3;
      double num5 = num1 - num4;
      unit2 = C1doc.PageLayout.PageSettings.Width;
      int units1 = (int) unit2.Units;
      local1 = new Unit(num5, (UnitTypeEnum) units1);
      Unit unit3;
      ref Unit local2 = ref unit3;
      Unit unit4 = C1doc.PageLayout.PageSettings.Height;
      double num6 = unit4.Value;
      unit4 = C1doc.PageLayout.PageSettings.TopMargin;
      double num7 = unit4.Value;
      unit4 = C1doc.PageLayout.PageSettings.BottomMargin;
      double num8 = unit4.Value;
      double num9 = num7 + num8;
      double num10 = num6 - num9;
      unit4 = C1doc.PageLayout.PageSettings.Width;
      int units2 = (int) unit4.Units;
      local2 = new Unit(num10, (UnitTypeEnum) units2);
      return new Rectangle(0, 0, (int) unit1.ConvertUnit(C1doc.CreationDpi, UnitTypeEnum.Pixel, FormGraphics.DpiX), (int) unit3.ConvertUnit(C1doc.CreationDpi, UnitTypeEnum.Pixel, FormGraphics.DpiY));
    }

    private List<KeyValuePair<int, double>> GetTopAvoidedRunoffBySpecies()
    {
      List<KeyValuePair<int, double>> avoidedRunoffBySpecies = new List<KeyValuePair<int, double>>();
      DataTable dataTable = this.GetMultipleFieldsData().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.AvoidedRunoff]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.Year)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "TOTAL")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();
      if (dataTable != null)
      {
        dataTable.DefaultView.Sort = "EstimateValue DESC";
        DataTable table = dataTable.DefaultView.ToTable();
        string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
        int num = 0;
        foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        {
          if (num < 10)
          {
            avoidedRunoffBySpecies.Add(new KeyValuePair<int, double>((int) row[classifierName], (double) row["EstimateValue"]));
            ++num;
          }
          else
            break;
        }
      }
      return avoidedRunoffBySpecies;
    }

    private double GetTotalLeafAreaSquareKilometers() => this.GetEstimatedValuesSum().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.LeafArea]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "TOTAL")).UniqueResult<double>();

    private Tuple<double, double> GetAvoidedRunoff(double CustomizedRainfallWaterPrice)
    {
      double num = this.GetMultipleFieldsStratumData().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.AvoidedRunoff]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.Year)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).UniqueResult<double>();
      return Tuple.Create<double, double>(num, num * CustomizedRainfallWaterPrice);
    }

    private double GetYearlyPollutionRemovalAmount(EstimateDataTypes PollEstType, int PollCVO) => this.GetPollutionEstimateQuery(PollEstType).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.PollutionRemoval]).SetParameter<int>("pollCVO", PollCVO).UniqueResult<double>();

    private double GetTotalTreeCoverHectare(double avgCoverPercent)
    {
      if (!this.curYear.RecordStrata)
        return this.curInputISession.GetNamedQuery("TotalGroundArea").SetParameter<Guid>("y", this.YearGuid).UniqueResult<double>() / 10000.0;
      double num = this.curInputISession.GetNamedQuery("TotalStratum").SetParameter<Guid>("y", this.YearGuid).UniqueResult<double>();
      if (this.curYear.Unit == YearUnit.English)
        num *= 0.404686;
      return avgCoverPercent * num / 100.0;
    }

    private DataTable GetSpeciesCounts() => this.GetEstimatedValues().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.NumberofTrees]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Count, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "Total")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private double GetYearlyTotalPollutionRemovalAmount(EstimateDataTypes PollEstType) => 0.0 + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "CO")) + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "NO2")) + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "SO2")) + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "O3")) + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM2.5")) + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM10*"));

    private double GetYearlyTotalPollutionRemovalDollars(
      EstimateDataTypes PollEstType,
      double CustomizedCoDollarsPerTon,
      double CustomizedO3DollarsPerTon,
      double customizedNO2DollarsPerTon,
      double CustomizedSO2DollarsPerTon,
      double CustomizedPM25DollarsPerTon,
      double CustomizedPM10DollarsPerTon)
    {
      return 0.0 + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "CO")) * CustomizedCoDollarsPerTon + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "NO2")) * customizedNO2DollarsPerTon + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "SO2")) * CustomizedSO2DollarsPerTon + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "O3")) * CustomizedO3DollarsPerTon + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM2.5")) * CustomizedPM25DollarsPerTon + this.GetYearlyPollutionRemovalAmount(PollEstType, (int) this.estUtil.GetClassValueOrderFromName(Classifiers.Pollutant, "PM10*")) * CustomizedPM10DollarsPerTon;
    }

    private IQuery GetPollutionEstimateQuery(EstimateDataTypes pollEstType) => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedPollution(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(pollEstType, new List<Classifiers>()
    {
      Classifiers.Month,
      Classifiers.Pollutant
    })], this.estUtil.ClassifierNames[Classifiers.Pollutant]);

    private IQuery GetMultipleFieldsStratumData() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValueSum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.TreeShrubCombined, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private IQuery GetMultipleFieldsData() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetSignificantEstimateValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.TreeShrubCombined, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    private IQuery GetEstimatedValues() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    private IQuery GetStrataEstimatedValues() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValues(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private IQuery GetStrataEstimatedValuesByClassifier() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimateValuesForClassifier(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata
    })], this.estUtil.ClassifierNames[Classifiers.Strata]);

    private IQuery GetEstimatedValuesSum() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedValueSum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    private DataTable GetPollutionByStratum() => this.GetPollutionByStratumQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.VOCEmissions]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.None, Units.Year)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Strata, "Study Area")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetPollutionByStratumQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedPollutantsValuesByStratum(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Strata,
      Classifiers.VOCs
    })], this.estUtil.ClassifierNames[Classifiers.Strata], this.estUtil.ClassifierNames[Classifiers.VOCs]);

    private DataTable GetTopTwoPollutantSpecies() => this.GetPollutionBySpeciesQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.VOCEmissions]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.None, Units.Year)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "TOTAL")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>().AsEnumerable().Take<DataRow>(2).CopyToDataTable<DataRow>();

    private IQuery GetPollutionBySpeciesQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedPollutantsValuesBySpecies(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species,
      Classifiers.VOCs
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    public DataTable GetCarbonData() => this.GetCarbonQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.series.SampleType == SampleType.Inventory ? this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration] : this.estUtil.EstTypes[EstimateTypeEnum.NetCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "TOTAL")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    public DataTable GetGrossCarbonSequestrationData() => this.GetCarbonQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.GrossCarbonSequestration]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "TOTAL")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    public DataTable CarbonStorage() => this.GetCarbonQuery().SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estType", this.estUtil.EstTypes[EstimateTypeEnum.CarbonStorage]).SetParameter<int>("estUnits", this.estUtil.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<short>("totalsCVO", this.estUtil.GetClassValueOrderFromName(Classifiers.Species, "TOTAL")).SetResultTransformer((IResultTransformer) new DataTableResultTransformer()).UniqueResult<DataTable>();

    private IQuery GetCarbonQuery() => this.estUtil.queryProvider.GetEstimateUtilProvider().GetEstimatedPollutantsValuesBySpecies(this.estUtil.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
    {
      Classifiers.Species
    })], this.estUtil.ClassifierNames[Classifiers.Species]);

    private List<KeyValuePair<int, double>> GetTopN(DataTable data, int n)
    {
      List<KeyValuePair<int, double>> topN = new List<KeyValuePair<int, double>>();
      string classifierName = this.estUtil.ClassifierNames[Classifiers.Species];
      int num = 0;
      foreach (DataRow row in (InternalDataCollectionBase) data.Rows)
      {
        if (num++ < n)
          topN.Add(new KeyValuePair<int, double>((int) row[classifierName], (double) row["sumEstimate"]));
      }
      return topN;
    }

    protected override void SetLayout(C1PrintDocument C1doc)
    {
      base.SetLayout(C1doc);
      C1doc.PageLayout.PageSettings.Landscape = false;
    }
  }
}
