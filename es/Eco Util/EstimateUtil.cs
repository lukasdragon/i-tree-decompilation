// Decompiled with JetBrains decompiler
// Type: Eco.Util.EstimateUtil
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.NHibernate;
using Eco.Domain.v6;
using Eco.Util.Queries.Interfaces;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TreeEnergy;

namespace Eco.Util
{
  public class EstimateUtil
  {
    public double CO2LbPerMMBTU;
    public double CO2LbPerMwh;
    public double transmissionLossRate;
    private ISession curInputISession;
    public IQuerySupplier queryProvider;

    public Dictionary<Tuple<Units, Units, Units>, int> EstUnits { get; set; }

    public Dictionary<EstimateTypeEnum, int> EstTypes { get; set; }

    public Dictionary<Tuple<EstimateDataTypes, List<Classifiers>>, string> EstTableDictionary { get; set; }

    public Dictionary<Classifiers, string> ClassifierNames { get; set; }

    public Dictionary<Classifiers, SortedList<short, Tuple<string, string>>> ClassValues { get; set; }

    public Dictionary<short, Tuple<string, string, string>> SpeciesValues { get; set; }

    public Guid YearGuid { get; set; }

    public EstimateUtil(InputSession inputSession, ISessionFactory sfLocSp)
    {
      EstimateUtil estimateUtil = this;
      this.YearGuid = inputSession.YearKey.Value;
      this.curInputISession = inputSession.CreateSession();
      this.queryProvider = inputSession.GetQuerySupplier(this.curInputISession);
      this.EstTypes = this.GetEstimationTypes();
      this.EstUnits = this.getEstUnits();
      this.EstTableDictionary = this.GetEstTableDictionary();
      this.ClassifierNames = this.getClassifierNames();
      this.ClassValues = this.GetClassValues();
      this.SpeciesValues = this.GetSpeciesValues();
      LocationEnvironmentalEffect locEnvEffect = RetryExecutionHandler.Execute<LocationSpecificValues>((Func<LocationSpecificValues>) (() =>
      {
        using (ISession sessLocSpec = sfLocSp.OpenSession())
        {
          Location location1 = sessLocSpec.Load<Location>((object) estimateUtil.getLocationId());
          Location location2 = sessLocSpec.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) location1)).Add((ICriterion) Restrictions.IsNotNull("Code")).UniqueResult<LocationRelation>().Location;
          LocationSpecificValues locationSpecificValues = new LocationSpecificValues(sessLocSpec, 1.0);
          locationSpecificValues.GetLocationSpecificValues(location2);
          return locationSpecificValues;
        }
      })).locEnvEffect;
      this.CO2LbPerMMBTU = locEnvEffect.CO2MMBtu.GetValueOrDefault();
      this.CO2LbPerMwh = locEnvEffect.CO2MWh.GetValueOrDefault();
      this.transmissionLossRate = locEnvEffect.TransmissionLoss.GetValueOrDefault();
    }

    private Dictionary<Tuple<Units, Units, Units>, int> getEstUnits()
    {
      Dictionary<int, Units> units = this.getUnits();
      Dictionary<Tuple<Units, Units, Units>, int> estUnits = new Dictionary<Tuple<Units, Units, Units>, int>();
      foreach (IDictionary dictionary in (IEnumerable<IDictionary>) this.curInputISession.GetNamedQuery("GetEstUnits").SetResultTransformer(Transformers.AliasToEntityMap).List<IDictionary>())
        estUnits.Add(Tuple.Create<Units, Units, Units>(units[Convert.ToInt32(dictionary[(object) "PrimaryUnitsId"])], units[Convert.ToInt32(dictionary[(object) "SecondaryUnitsId"])], units[Convert.ToInt32(dictionary[(object) "TertiaryUnitsId"])]), Convert.ToInt32(dictionary[(object) "EstimationUnitsId"]));
      return estUnits;
    }

    private Dictionary<int, Units> getUnits()
    {
      Dictionary<int, Units> units = new Dictionary<int, Units>();
      foreach (Units key in Enum.GetValues(typeof (Units)))
        units.Add((int) key, key);
      return units;
    }

    private Dictionary<EstimateTypeEnum, int> GetEstimationTypes()
    {
      Dictionary<EstimateTypeEnum, int> estimationTypes = new Dictionary<EstimateTypeEnum, int>();
      foreach (EstimateTypeEnum key in Enum.GetValues(typeof (EstimateTypeEnum)))
        estimationTypes.Add(key, (int) key);
      return estimationTypes;
    }

    public Dictionary<Tuple<EstimateDataTypes, List<Classifiers>>, string> GetEstTableDictionary()
    {
      Dictionary<Tuple<EstimateDataTypes, List<Classifiers>>, string> estTableDictionary = new Dictionary<Tuple<EstimateDataTypes, List<Classifiers>>, string>((IEqualityComparer<Tuple<EstimateDataTypes, List<Classifiers>>>) new ClassifierPartitionTableComparer());
      IList<(short, string, int)> tupleList = this.curInputISession.GetNamedQuery("GetEstimateTableWithPartitions").SetResultTransformer((IResultTransformer) new TupleResultTransformer<(short, string, int)>()).List<(short, string, int)>();
      int num = -1;
      string str = "";
      List<Classifiers> classifiersList = new List<Classifiers>();
      foreach ((short, string, int) tuple in (IEnumerable<(short, string, int)>) tupleList)
      {
        if (num != (int) tuple.Item1 || str != tuple.Item2)
        {
          if (str != "")
            estTableDictionary.Add(Tuple.Create<EstimateDataTypes, List<Classifiers>>((EstimateDataTypes) num, classifiersList), str);
          num = (int) tuple.Item1;
          str = tuple.Item2;
          classifiersList = new List<Classifiers>();
          classifiersList.Add((Classifiers) tuple.Item3);
        }
        else
          classifiersList.Add((Classifiers) tuple.Item3);
      }
      if (str != "")
        estTableDictionary.Add(Tuple.Create<EstimateDataTypes, List<Classifiers>>((EstimateDataTypes) num, classifiersList), str);
      return estTableDictionary;
    }

    private int getLocationId() => this.curInputISession.CreateCriteria<Year>().CreateAlias("Series", "s").CreateAlias("s.Project", "p").Add((ICriterion) Restrictions.Eq("Guid", (object) this.YearGuid)).SetProjection((IProjection) Projections.Property("p.LocationId")).UniqueResult<int>();

    private Dictionary<Classifiers, string> getClassifierNames()
    {
      Dictionary<Classifiers, string> classifierNames = new Dictionary<Classifiers, string>();
      foreach (Classifiers key in Enum.GetValues(typeof (Classifiers)))
        classifierNames.Add(key, key.ToString());
      return classifierNames;
    }

    private Dictionary<short, Tuple<string, string, string>> GetSpeciesValues()
    {
      Dictionary<short, Tuple<string, string, string>> speciesValues = new Dictionary<short, Tuple<string, string, string>>();
      foreach (IDictionary dictionary in (IEnumerable<IDictionary>) this.curInputISession.GetNamedQuery(nameof (GetSpeciesValues)).SetParameter<Guid>("yearGuid", this.YearGuid).SetParameter<Classifiers>("classifier", Classifiers.Species).SetResultTransformer(Transformers.AliasToEntityMap).List<IDictionary>())
        speciesValues[(short) dictionary[(object) "ClassValueOrder"]] = Tuple.Create<string, string, string>(dictionary[(object) "ClassValueName"] as string, dictionary[(object) "ClassValueName1"] as string, dictionary[(object) "SppCode"] as string);
      return speciesValues;
    }

    public Dictionary<Classifiers, SortedList<short, Tuple<string, string>>> GetClassValues()
    {
      IList<IDictionary> dictionaryList = this.curInputISession.GetNamedQuery(nameof (GetClassValues)).SetParameter<Guid>("yearGuid", this.YearGuid).SetResultTransformer(Transformers.AliasToEntityMap).List<IDictionary>();
      Dictionary<Classifiers, SortedList<short, Tuple<string, string>>> classValues = new Dictionary<Classifiers, SortedList<short, Tuple<string, string>>>();
      foreach (IDictionary dictionary in (IEnumerable<IDictionary>) dictionaryList)
      {
        Classifiers int32 = (Classifiers) Convert.ToInt32(dictionary[(object) "ClassifierId"]);
        if (!classValues.ContainsKey(int32))
          classValues.Add(int32, new SortedList<short, Tuple<string, string>>());
        classValues[int32][Convert.ToInt16(dictionary[(object) "ClassValueOrder"])] = Tuple.Create<string, string>(dictionary[(object) "ClassValueName"] as string, dictionary[(object) "ClassValueName1"] as string);
      }
      return classValues;
    }

    public short GetClassValueOrderFromName(Classifiers classifier, string ClassValueName)
    {
      if (this.ClassValues.Count == 0)
        return -1;
      foreach (KeyValuePair<short, Tuple<string, string>> keyValuePair in this.ClassValues[classifier])
      {
        if (string.Equals(ClassValueName, keyValuePair.Value.Item1, StringComparison.InvariantCultureIgnoreCase))
          return keyValuePair.Key;
      }
      return -1;
    }

    public short GetClassValueOrderFromNameOne(Classifiers classifier, string ClassValueName)
    {
      foreach (KeyValuePair<short, Tuple<string, string>> keyValuePair in this.ClassValues[classifier])
      {
        if (string.Equals(ClassValueName, keyValuePair.Value.Item2, StringComparison.InvariantCultureIgnoreCase))
          return keyValuePair.Key;
      }
      return -1;
    }

    public static double ConvertToEnglish(double d, Units SourceUnit, bool ConvertToEnglish)
    {
      if (!ConvertToEnglish)
        return d;
      switch (SourceUnit)
      {
        case Units.Centimeters:
          return d * 0.393701;
        case Units.Meters:
          return d * 3.28084;
        case Units.Kilometer:
          return d * 0.621371;
        case Units.Squaremeter:
          return d * 10.7639;
        case Units.Squarekilometer:
          return d * 0.386102;
        case Units.Grams:
          return d * 0.035274;
        case Units.Kilograms:
          return d * 2.20462;
        case Units.MetricTons:
          return d * 1.10231;
        case Units.Hectare:
          return d * 2.47105;
        case Units.CubicMeter:
          return d * 35.3147;
        default:
          throw new ArgumentException(Strings.MsgArgumentNotDefined);
      }
    }

    public static double ConvertToMetric(double d, Units SourceUnit, bool ConvertToMetric)
    {
      if (!ConvertToMetric)
        return d;
      if (SourceUnit == Units.Feet)
        return d * 0.3048;
      if (SourceUnit == Units.Pounds)
        return d * 0.453592;
      if (SourceUnit == Units.Ton)
        return d * 0.907185;
      throw new ArgumentException(Strings.MsgArgumentNotDefined);
    }

    public static double ConvertToEnglish(
      double d,
      Tuple<Units, Units, Units> SourceEstUnit,
      bool ConvertToEnglish)
    {
      if (!ConvertToEnglish)
        return Tuple.Create<Units, Units, Units>(Units.TempFahrenheit, Units.None, Units.None).Equals((object) SourceEstUnit) ? (d - 32.0) * 5.0 / 9.0 : d;
      if (Tuple.Create<Units, Units, Units>(Units.Count, Units.Hectare, Units.None).Equals((object) SourceEstUnit))
        return d * 0.40468626697;
      if (Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.Hectare, Units.None).Equals((object) SourceEstUnit) || Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.Hectare, Units.Year).Equals((object) SourceEstUnit) || Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.Year, Units.Hectare).Equals((object) SourceEstUnit))
        return d * 0.89217943789;
      if (Tuple.Create<Units, Units, Units>(Units.Squaremeter, Units.Hectare, Units.None).Equals((object) SourceEstUnit))
        return d * 4.356;
      if (Tuple.Create<Units, Units, Units>(Units.Monetaryunit, Units.Hectare, Units.None).Equals((object) SourceEstUnit))
        return d * 0.404685642;
      if (Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None).Equals((object) SourceEstUnit))
        return d * 1.10231;
      if (Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.None, Units.None).Equals((object) SourceEstUnit))
        return d * 2.20462;
      if (Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.Year, Units.None).Equals((object) SourceEstUnit))
        return d * 1.10231;
      if (Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.Hectare, Units.None).Equals((object) SourceEstUnit))
        return d * (5.0 / 32.0);
      if (Tuple.Create<Units, Units, Units>(Units.Ton, Units.None, Units.None).Equals((object) SourceEstUnit))
        return d;
      if (Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.Year, Units.None).Equals((object) SourceEstUnit) || Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.Year).Equals((object) SourceEstUnit) || Tuple.Create<Units, Units, Units>(Units.CubicMeter, Units.None, Units.None).Equals((object) SourceEstUnit))
        return d * 35.3147;
      if (Tuple.Create<Units, Units, Units>(Units.Kilograms, Units.None, Units.Year).Equals((object) SourceEstUnit))
        return d * 2.20462;
      if (Tuple.Create<Units, Units, Units>(Units.Meters, Units.None, Units.None).Equals((object) SourceEstUnit))
        return d * 3.28084;
      if (Tuple.Create<Units, Units, Units>(Units.Grams, Units.Squaremeter, Units.Hour).Equals((object) SourceEstUnit))
        return d * 0.003277065;
      if (Tuple.Create<Units, Units, Units>(Units.Pounds, Units.Squaremeter, Units.None).Equals((object) SourceEstUnit))
        return d * 0.09290304;
      if (Tuple.Create<Units, Units, Units>(Units.TempFahrenheit, Units.None, Units.None).Equals((object) SourceEstUnit))
        return d;
      if (Tuple.Create<Units, Units, Units>(Units.Squarekilometer, Units.None, Units.None).Equals((object) SourceEstUnit))
        return d * 0.386102;
      if (Tuple.Create<Units, Units, Units>(Units.Centimeters, Units.None, Units.None).Equals((object) SourceEstUnit))
        return d * 0.393701;
      throw new ArgumentException(Strings.MsgArgumentNotDefined);
    }

    public double GetEnergyValues(
      int MBTU_MWH_CarbonAvoided,
      int CoolingOrHeating,
      int UnitsOrDollars,
      double DollarsPerMBTU,
      double DollarsPerMWH,
      double DollarsPerCarbonTon)
    {
      int valueOrderFromName1 = (int) this.GetClassValueOrderFromName(Classifiers.Strata, "Study Area");
      int valueOrderFromName2 = (int) this.GetClassValueOrderFromName(Classifiers.EnergyUse, "Cooling");
      int valueOrderFromName3 = (int) this.GetClassValueOrderFromName(Classifiers.EnergyUse, "Heating");
      switch (MBTU_MWH_CarbonAvoided)
      {
        case 1:
          if (CoolingOrHeating == 1)
            throw new ArgumentException("No cooling for MBTU");
          return UnitsOrDollars == 1 ? this.GetHeatingMBTU(valueOrderFromName1, valueOrderFromName3) : this.GetDollars(this.GetHeatingMBTU(valueOrderFromName1, valueOrderFromName3), DollarsPerMBTU);
        case 2:
          double quantity = CoolingOrHeating == 1 ? this.GetMWH(valueOrderFromName1, valueOrderFromName2) : this.GetMWH(valueOrderFromName1, valueOrderFromName3);
          return UnitsOrDollars == 1 ? quantity : this.GetDollars(quantity, DollarsPerMWH);
        case 3:
          return UnitsOrDollars == 1 ? this.GetCarbonAvioded(CoolingOrHeating, DollarsPerMBTU, DollarsPerMWH, DollarsPerCarbonTon) : this.GetDollars(this.GetCarbonAvioded(CoolingOrHeating, DollarsPerMBTU, DollarsPerMWH, DollarsPerCarbonTon), DollarsPerCarbonTon);
        default:
          throw new ArgumentException(string.Format("{0} GetEnergyValues", (object) Strings.MsgInvalidParameters));
      }
    }

    private double GetCarbonAvioded(
      int CoolingOrHeating,
      double DollarsPerMBTU,
      double DollarsPerMWH,
      double DollarsPerCarbonTon)
    {
      double carbonAvioded;
      if (CoolingOrHeating == 1)
      {
        carbonAvioded = this.GetEnergyValues(2, CoolingOrHeating, 1, DollarsPerMBTU, DollarsPerMWH, DollarsPerCarbonTon) * this.CO2LbPerMwh / 2204.62 / (11.0 / 3.0) / (1.0 - this.transmissionLossRate / 100.0);
      }
      else
      {
        double energyValues1 = this.GetEnergyValues(2, CoolingOrHeating, 1, DollarsPerMBTU, DollarsPerMWH, DollarsPerCarbonTon);
        double energyValues2 = this.GetEnergyValues(1, CoolingOrHeating, 1, DollarsPerMBTU, DollarsPerMWH, DollarsPerCarbonTon);
        double co2LbPerMwh = this.CO2LbPerMwh;
        carbonAvioded = energyValues1 * co2LbPerMwh / 2204.62 / (11.0 / 3.0) / (1.0 - this.transmissionLossRate / 100.0) + energyValues2 * this.CO2LbPerMMBTU / 2204.62 / (11.0 / 3.0);
      }
      return carbonAvioded;
    }

    private double GetMWH(int cityTotalCVO, int HeatOrCoolingCVO)
    {
      string estTable = this.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.EnergyUse
      })];
      return this.queryProvider.GetEstimateUtilProvider().GetEstimateValue(estTable, this.ClassifierNames[Classifiers.EnergyUse], this.ClassifierNames[Classifiers.Strata]).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estUnits", this.EstUnits[Tuple.Create<Units, Units, Units>(Units.Megawatthours, Units.None, Units.None)]).SetParameter<int>("heatOrCoolingCVO", HeatOrCoolingCVO).SetParameter<int>("estType", this.EstTypes[EstimateTypeEnum.ElectricityAvoided]).SetParameter<int>("totCVO", cityTotalCVO).UniqueResult<double>();
    }

    private double GetHeatingMBTU(int cityTotalCVO, int HeatingCVO)
    {
      string estTable = this.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.Tree, new List<Classifiers>()
      {
        Classifiers.Strata,
        Classifiers.EnergyUse
      })];
      return this.queryProvider.GetEstimateUtilProvider().GetEstimateValue(estTable, this.ClassifierNames[Classifiers.EnergyUse], this.ClassifierNames[Classifiers.Strata]).SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estUnits", this.EstUnits[Tuple.Create<Units, Units, Units>(Units.MillionBritishThermalUnits, Units.None, Units.None)]).SetParameter<int>("heatOrCoolingCVO", HeatingCVO).SetParameter<int>("estType", this.EstTypes[EstimateTypeEnum.EnergyFuels]).SetParameter<int>("totCVO", cityTotalCVO).UniqueResult<double>();
    }

    private double GetDollars(double quantity, double valueInDollars) => valueInDollars >= 0.0 ? quantity * valueInDollars : 0.0;

    public double GetYearlyPrecipitationMetersFromEstimateDb() => this.curInputISession.GetNamedQuery("GetRainMeters").SetParameter<Guid>("yearGuid", this.YearGuid).UniqueResult<double>();

    public double GetCalculatedPollutantDollarsPerTonFromEstimateDb(string pollutant)
    {
      if (!this.IsDataInDataTable(pollutant))
      {
        IQuery pollutionEstimateQuery = this.GetPollutionEstimateQuery();
        double pollutionEstimatedTons = this.GetPollutionEstimatedTons(pollutionEstimateQuery, pollutant);
        double estimatedDollarValue = this.GetPollutionEstimatedDollarValue(pollutionEstimateQuery, pollutant);
        if (pollutionEstimatedTons != 0.0)
          return estimatedDollarValue / pollutionEstimatedTons;
      }
      try
      {
        return 1000000.0 * this.curInputISession.GetNamedQuery("GetPollutantValuePerFlux").SetParameter<Guid>("y", this.YearGuid).SetParameter<string>("poll", pollutant).UniqueResult<double>();
      }
      catch
      {
        return 0.0;
      }
    }

    private bool IsDataInDataTable(string pollutant)
    {
      if (this.GetClassValueOrderFromName(Classifiers.Pollutant, pollutant) == (short) -1)
        return true;
      return !this.EstTableDictionary.ContainsKey(Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.TreeShrubCombined, new List<Classifiers>()
      {
        Classifiers.Month,
        Classifiers.Pollutant
      }));
    }

    private IQuery GetPollutionEstimateQuery() => this.queryProvider.GetEstimateUtilProvider().GetEstimatedPollution(this.EstTableDictionary[Tuple.Create<EstimateDataTypes, List<Classifiers>>(EstimateDataTypes.TreeShrubCombined, new List<Classifiers>()
    {
      Classifiers.Month,
      Classifiers.Pollutant
    })], this.ClassifierNames[Classifiers.Pollutant]);

    private double GetPollutionEstimatedTons(IQuery q, string pollutant) => q.SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estUnits", this.EstUnits[Tuple.Create<Units, Units, Units>(Units.MetricTons, Units.None, Units.None)]).SetParameter<int>("estType", this.EstTypes[EstimateTypeEnum.PollutionRemoval]).SetParameter<short>("pollCVO", this.GetClassValueOrderFromName(Classifiers.Pollutant, pollutant)).UniqueResult<double>();

    private double GetPollutionEstimatedDollarValue(IQuery q, string pollutant) => q.SetParameter<Guid>("y", this.YearGuid).SetParameter<int>("estUnits", this.EstUnits[Tuple.Create<Units, Units, Units>(Units.Monetaryunit, Units.None, Units.None)]).SetParameter<int>("estType", this.EstTypes[EstimateTypeEnum.PollutionRemoval]).SetParameter<short>("pollCVO", this.GetClassValueOrderFromName(Classifiers.Pollutant, pollutant)).UniqueResult<double>();

    public SortedList<short, string> ConvertDBHRangesToEnglish(bool EnglishUnits)
    {
      SortedList<short, Tuple<string, string>> classValue = this.ClassValues[Classifiers.CDBH];
      SortedList<short, string> source = new SortedList<short, string>();
      foreach (KeyValuePair<short, Tuple<string, string>> keyValuePair in classValue)
      {
        string[] strArray = keyValuePair.Value.Item1.Split(new string[1]
        {
          " - "
        }, StringSplitOptions.None);
        CultureInfo invariantCulture = CultureInfo.InvariantCulture;
        double d1 = double.Parse(strArray[0], (IFormatProvider) invariantCulture);
        double d2 = double.Parse(strArray[1], (IFormatProvider) invariantCulture);
        source.Add(keyValuePair.Key, string.Format("{0} - {1}", (object) Math.Round(EstimateUtil.ConvertToEnglish(d1, Units.Centimeters, EnglishUnits), 1), (object) Math.Round(EstimateUtil.ConvertToEnglish(d2, Units.Centimeters, EnglishUnits), 1)));
      }
      KeyValuePair<short, string> keyValuePair1 = source.Last<KeyValuePair<short, string>>();
      short key = keyValuePair1.Key;
      string str = string.Format("{0}+", (object) keyValuePair1.Value.Split(new string[1]
      {
        " - "
      }, StringSplitOptions.None)[0]);
      source[key] = str;
      return source;
    }

    public static double DivideOrZero(double dividend, double divisor)
    {
      if (divisor == 0.0)
        return 0.0;
      double val = dividend / divisor;
      return !EstimateUtil.IsFinate(val) ? 0.0 : val;
    }

    public static double DivideOrZero(double dividend, long divisor)
    {
      if (divisor == 0L)
        return 0.0;
      double val = dividend / (double) divisor;
      return !EstimateUtil.IsFinate(val) ? 0.0 : val;
    }

    public static Decimal DivideOrZero(Decimal numerator, Decimal denominator) => denominator != 0M ? numerator / denominator : 0M;

    public static T PopAt<T>(List<T> list, int index)
    {
      T obj = list[index];
      list.RemoveAt(index);
      return obj;
    }

    private static bool IsFinate(double val) => !double.IsNaN(val) && !double.IsInfinity(val) && !double.IsNegativeInfinity(val);
  }
}
