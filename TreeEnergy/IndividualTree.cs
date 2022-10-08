// Decompiled with JetBrains decompiler
// Type: TreeEnergy.IndividualTree
// Assembly: TreeEnergy, Version=1.1.6718.0, Culture=neutral, PublicKeyToken=null
// MVID: 999CE94F-70B0-42EB-8D64-3ADB949CAFD0
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\TreeEnergy.dll

using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;

namespace TreeEnergy
{
  public class IndividualTree
  {
    private ISession _sessLocSpec;
    private Location _location;
    private ClimateRegion _climateRegion;
    private Dictionary<BuildingVintage, ClimateRegionBuilding> _dVintageClimateRegionBuilding;
    private Dictionary<EnergyEffect, Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>>>> _dShadingFactors;
    private Dictionary<BuildingVintage, Dictionary<EnergyEffect, Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>>>> _dClimateFactors;
    private Dictionary<BuildingVintage, Dictionary<EnergyEffect, BuildingVintageFactor>> _dBuildingVintageFactors;
    private Dictionary<BuildingVintage, Dictionary<TreeHeightClass, Dictionary<LeafType, WindbreakFactor>>> _dWindbreakFactors;
    private LocationEnvironmentalEffect _locEnvEffect;
    private EnvironmentalEffectDollarAdjustments _locEnvEffectDollarAdjust;
    private LocationCost _locationCost;
    private LocationSpecificValues _locVal;
    private CalculateTreeParameter _calcTreeParam;
    private bool _metric;
    private bool _heightRecorded;
    private double _percentTreeBuildingCover;

    public IndividualTree(
      ISession sessLocSpec,
      Location location,
      double exchangeRate,
      bool metric,
      bool heightRecorded)
    {
      if (exchangeRate <= 0.0)
        throw new ArgumentException(nameof (exchangeRate));
      this._sessLocSpec = sessLocSpec;
      this._location = location;
      this._metric = metric;
      this._heightRecorded = heightRecorded;
      this._locVal = new LocationSpecificValues(sessLocSpec, exchangeRate);
      this._locVal.GetLocationSpecificValues(location);
      this._climateRegion = this._locVal.climRegion;
      if (this._climateRegion != null)
      {
        this.InitClimateRegionBuildings();
        this.InitShadingFactors(sessLocSpec);
        this.InitClimateFactors(sessLocSpec);
        this.InitBuildingVintageFactors(sessLocSpec);
        this.InitWindbreakFactors(sessLocSpec);
      }
      this._locEnvEffect = this._locVal.locEnvEffect;
      this._locEnvEffectDollarAdjust = this._locVal.locEnvEffectDollarAdjust;
      this._locationCost = this._locVal.locationCost;
      this._calcTreeParam = new CalculateTreeParameter(this._sessLocSpec);
    }

    private void InitWindbreakFactors(ISession sessLocSpec)
    {
      IList<WindbreakFactor> windbreakFactorList = sessLocSpec.CreateCriteria<WindbreakFactor>().Add((ICriterion) Restrictions.Eq("ClimateRegion", (object) this._climateRegion)).Add((ICriterion) Restrictions.Eq("EnergyEffect", (object) EnergyEffect.Heating)).SetCacheable(true).List<WindbreakFactor>();
      this._dWindbreakFactors = new Dictionary<BuildingVintage, Dictionary<TreeHeightClass, Dictionary<LeafType, WindbreakFactor>>>();
      foreach (WindbreakFactor windbreakFactor in (IEnumerable<WindbreakFactor>) windbreakFactorList)
      {
        Dictionary<TreeHeightClass, Dictionary<LeafType, WindbreakFactor>> dictionary1 = (Dictionary<TreeHeightClass, Dictionary<LeafType, WindbreakFactor>>) null;
        Dictionary<LeafType, WindbreakFactor> dictionary2 = (Dictionary<LeafType, WindbreakFactor>) null;
        if (!this._dWindbreakFactors.TryGetValue(windbreakFactor.BuildingVintage, out dictionary1))
        {
          dictionary1 = new Dictionary<TreeHeightClass, Dictionary<LeafType, WindbreakFactor>>();
          this._dWindbreakFactors[windbreakFactor.BuildingVintage] = dictionary1;
        }
        if (!dictionary1.TryGetValue(windbreakFactor.TreeHeightClass, out dictionary2))
        {
          dictionary2 = new Dictionary<LeafType, WindbreakFactor>();
          dictionary1[windbreakFactor.TreeHeightClass] = dictionary2;
        }
        dictionary2[windbreakFactor.LeafType] = windbreakFactor;
      }
    }

    private void InitBuildingVintageFactors(ISession sessLocSpec)
    {
      IList<BuildingVintageFactor> buildingVintageFactorList = sessLocSpec.CreateCriteria<BuildingVintageFactor>().Add((ICriterion) Restrictions.Eq("ClimateRegion", (object) this._climateRegion)).SetCacheable(true).List<BuildingVintageFactor>();
      this._dBuildingVintageFactors = new Dictionary<BuildingVintage, Dictionary<EnergyEffect, BuildingVintageFactor>>();
      foreach (BuildingVintageFactor buildingVintageFactor in (IEnumerable<BuildingVintageFactor>) buildingVintageFactorList)
      {
        Dictionary<EnergyEffect, BuildingVintageFactor> dictionary = (Dictionary<EnergyEffect, BuildingVintageFactor>) null;
        if (!this._dBuildingVintageFactors.TryGetValue(buildingVintageFactor.BuildingVintage, out dictionary))
        {
          dictionary = new Dictionary<EnergyEffect, BuildingVintageFactor>();
          this._dBuildingVintageFactors[buildingVintageFactor.BuildingVintage] = dictionary;
        }
        dictionary[buildingVintageFactor.EnergyEffect] = buildingVintageFactor;
      }
    }

    private void InitClimateFactors(ISession sessLocSpec)
    {
      IList<ClimateFactor> climateFactorList = sessLocSpec.CreateCriteria<ClimateFactor>().Add((ICriterion) Restrictions.Eq("ClimateRegion", (object) this._climateRegion)).SetCacheable(true).List<ClimateFactor>();
      this._dClimateFactors = new Dictionary<BuildingVintage, Dictionary<EnergyEffect, Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>>>>();
      foreach (ClimateFactor climateFactor in (IEnumerable<ClimateFactor>) climateFactorList)
      {
        Dictionary<EnergyEffect, Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>>> dictionary1 = (Dictionary<EnergyEffect, Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>>>) null;
        Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>> dictionary2 = (Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>>) null;
        Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>> dictionary3 = (Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>) null;
        Dictionary<TreeBuildingCover, ClimateFactor> dictionary4 = (Dictionary<TreeBuildingCover, ClimateFactor>) null;
        if (!this._dClimateFactors.TryGetValue(climateFactor.BuildingVintage, out dictionary1))
        {
          dictionary1 = new Dictionary<EnergyEffect, Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>>>();
          this._dClimateFactors[climateFactor.BuildingVintage] = dictionary1;
        }
        if (!dictionary1.TryGetValue(climateFactor.EnergyEffect, out dictionary2))
        {
          dictionary2 = new Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>>();
          dictionary1[climateFactor.EnergyEffect] = dictionary2;
        }
        if (!dictionary2.TryGetValue(climateFactor.TreeHeightClass, out dictionary3))
        {
          dictionary3 = new Dictionary<LeafType, Dictionary<TreeBuildingCover, ClimateFactor>>();
          dictionary2[climateFactor.TreeHeightClass] = dictionary3;
        }
        if (!dictionary3.TryGetValue(climateFactor.LeafType, out dictionary4))
        {
          dictionary4 = new Dictionary<TreeBuildingCover, ClimateFactor>();
          dictionary3[climateFactor.LeafType] = dictionary4;
        }
        dictionary4[climateFactor.TreeBuildingCover] = climateFactor;
      }
    }

    private void InitShadingFactors(ISession sessLocSpec)
    {
      IList<ShadingFactor> shadingFactorList = sessLocSpec.CreateCriteria<ShadingFactor>().Add((ICriterion) Restrictions.Eq("ClimateRegion", (object) this._climateRegion)).SetCacheable(true).List<ShadingFactor>();
      this._dShadingFactors = new Dictionary<EnergyEffect, Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>>>>();
      foreach (ShadingFactor shadingFactor in (IEnumerable<ShadingFactor>) shadingFactorList)
      {
        Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>>> dictionary1 = (Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>>>) null;
        Dictionary<LeafType, Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>> dictionary2 = (Dictionary<LeafType, Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>>) null;
        Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>> dictionary3 = (Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>) null;
        Dictionary<Directions, ShadingFactor> dictionary4 = (Dictionary<Directions, ShadingFactor>) null;
        if (!this._dShadingFactors.TryGetValue(shadingFactor.EnergyEffect, out dictionary1))
        {
          dictionary1 = new Dictionary<TreeHeightClass, Dictionary<LeafType, Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>>>();
          this._dShadingFactors[shadingFactor.EnergyEffect] = dictionary1;
        }
        if (!dictionary1.TryGetValue(shadingFactor.TreeHeightClass, out dictionary2))
        {
          dictionary2 = new Dictionary<LeafType, Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>>();
          dictionary1[shadingFactor.TreeHeightClass] = dictionary2;
        }
        if (!dictionary2.TryGetValue(shadingFactor.LeafType, out dictionary3))
        {
          dictionary3 = new Dictionary<TreeLocation, Dictionary<Directions, ShadingFactor>>();
          dictionary2[shadingFactor.LeafType] = dictionary3;
        }
        if (!dictionary3.TryGetValue(shadingFactor.TreeLocation, out dictionary4))
        {
          dictionary4 = new Dictionary<Directions, ShadingFactor>();
          dictionary3[shadingFactor.TreeLocation] = dictionary4;
        }
        dictionary4[shadingFactor.Direction] = shadingFactor;
      }
    }

    private void InitClimateRegionBuildings()
    {
      IList<ClimateRegionBuilding> climateRegionBuildingList = this._sessLocSpec.CreateCriteria<ClimateRegionBuilding>().Add((ICriterion) Restrictions.Eq("ClimateRegion", (object) this._climateRegion)).SetCacheable(true).List<ClimateRegionBuilding>();
      this._dVintageClimateRegionBuilding = new Dictionary<BuildingVintage, ClimateRegionBuilding>();
      foreach (ClimateRegionBuilding climateRegionBuilding in (IEnumerable<ClimateRegionBuilding>) climateRegionBuildingList)
        this._dVintageClimateRegionBuilding[climateRegionBuilding.BuildingVintage] = climateRegionBuilding;
    }

    public TreeBenefit CalculateEnergyBenefits(
      EnergyBuilding building,
      EnergyTree tree,
      double? percentTreeBuildingCover)
    {
      TreeBenefit energyBenefits = new TreeBenefit();
      if (this._climateRegion == null)
        return energyBenefits;
      this._percentTreeBuildingCover = percentTreeBuildingCover.HasValue ? (percentTreeBuildingCover.Value > 100.0 ? 100.0 : percentTreeBuildingCover.Value) : (double) this._climateRegion.PercentTreeBuildingCover;
      int num1 = (int) building.Direction - 180 + 22;
      while (num1 >= 360)
        num1 -= 360;
      while (num1 < 0)
        num1 += 360;
      int num2 = num1 / 45;
      if (this._location.Latitude <= 0.0)
      {
        num2 = 4 - num2;
        if (num2 < 0)
          num2 += 8;
      }
      Directions direction = (Directions) (num2 + 1);
      double distance = (double) building.Distance;
      if (!this._metric)
        distance *= 0.3048;
      if (distance > 18.0)
        return energyBenefits;
      TreeLocation treelocation = distance <= 12.0 ? (distance <= 6.0 ? TreeLocation.Adjacent : TreeLocation.Near) : TreeLocation.Far;
      double treeHeightMeters;
      if (this._heightRecorded)
      {
        treeHeightMeters = this._metric ? tree.TreeHeight : tree.TreeHeight * 0.3048;
        if (treeHeightMeters <= 0.0)
          treeHeightMeters = 0.0;
      }
      else
      {
        double d = 0.0;
        foreach (double stemDiameter in tree.StemDiameters)
        {
          double x = stemDiameter;
          if (this._metric)
            x /= 2.54;
          d = Math.Pow(x, 2.0) + d;
        }
        treeHeightMeters = this._calcTreeParam.CalculateTreeHeightFeet1(Math.Sqrt(d), tree.Species) * 0.3048;
      }
      if (treeHeightMeters < 3.048)
        return energyBenefits;
      LeafType topClassification = tree.Species.LeafType.TopClassification;
      if (building.Vintage == BuildingVintage.Unknown)
      {
        foreach (KeyValuePair<BuildingVintage, ClimateRegionBuilding> keyValuePair in this._dVintageClimateRegionBuilding)
        {
          BuildingVintage key = keyValuePair.Key;
          ClimateRegionBuilding climateRegionBuilding = keyValuePair.Value;
          building.Vintage = key;
          VintageBenefit vintageEnergy = this.CalculateVintageEnergy(this._sessLocSpec, topClassification, tree, building, climateRegionBuilding, treelocation, direction, treeHeightMeters, distance);
          energyBenefits += vintageEnergy * (double) climateRegionBuilding.PercentOfBuildings;
        }
      }
      else
      {
        VintageBenefit vintageEnergy = this.CalculateVintageEnergy(this._sessLocSpec, topClassification, tree, building, this._dVintageClimateRegionBuilding[building.Vintage], treelocation, direction, treeHeightMeters, distance);
        energyBenefits += vintageEnergy;
      }
      double? nullable = this._locEnvEffect.CO2MWh;
      double valueOrDefault1 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.CO2MMBtu;
      double valueOrDefault2 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.COMWh;
      double valueOrDefault3 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.NoxMWh;
      double valueOrDefault4 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.NOxMMbtu;
      double valueOrDefault5 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.SO2MWh;
      double valueOrDefault6 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.SO2MMbtu;
      double valueOrDefault7 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.PM10MWh;
      double valueOrDefault8 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.PM25MWh;
      double valueOrDefault9 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.VOCMWh;
      double valueOrDefault10 = nullable.GetValueOrDefault();
      nullable = this._locEnvEffect.TransmissionLoss;
      double num3 = 1.0 - nullable.GetValueOrDefault() / 100.0;
      energyBenefits.CarbonDioxideLbsElectricCoolClimate = energyBenefits.KWhElectricityCoolClimate * 0.001 * valueOrDefault1 / num3;
      energyBenefits.CarbonDioxideLbsElectricCoolShade = energyBenefits.KWhElectricityCoolShade * 0.001 * valueOrDefault1 / num3;
      energyBenefits.CarbonDioxideLbsElectricHeatClimate = energyBenefits.KWhElectricityHeatClimate * 0.001 * valueOrDefault1 / num3;
      energyBenefits.CarbonDioxideLbsElectricHeatShade = energyBenefits.KwhElectricityHeatShade * 0.001 * valueOrDefault1 / num3;
      energyBenefits.CarbonDioxideLbsElectricHeatWind = energyBenefits.KWhElectricityHeatWind * 0.001 * valueOrDefault1 / num3;
      energyBenefits.CarbonDioxideLbsMbtuHeatClimate = valueOrDefault2 * energyBenefits.MbtuHeatClimate;
      energyBenefits.CarbonDioxideLbsMbtuHeatShade = valueOrDefault2 * energyBenefits.MbtuHeatShade;
      energyBenefits.CarbonDioxideLbsMbtuHeatWind = valueOrDefault2 * energyBenefits.MbtuWindHeat;
      double num4 = (energyBenefits.KWhTotal * 0.001 * valueOrDefault4 / num3 + valueOrDefault5 * energyBenefits.MbtuTotal) * 0.2;
      double num5 = energyBenefits.KWhTotal * 0.001 * valueOrDefault6 / num3 + valueOrDefault7 * energyBenefits.MbtuTotal;
      double num6 = energyBenefits.KWhTotal * 0.001 * valueOrDefault3 / num3 + 0.0392 * energyBenefits.MbtuTotal;
      double num7 = energyBenefits.KWhTotal * 0.001 * valueOrDefault8 / num3 + 0.00186 * energyBenefits.MbtuTotal;
      double num8 = energyBenefits.KWhTotal * 0.001 * valueOrDefault9 / num3 + 0.00122 * energyBenefits.MbtuTotal;
      double num9 = energyBenefits.KWhTotal * 0.001 * valueOrDefault10 / num3 + 0.0054 * energyBenefits.MbtuTotal;
      energyBenefits.CarbonDioxideDollarValue = 0.00045359237 * energyBenefits.CarbonDioxideLbs * this._locEnvEffectDollarAdjust.CarbonDioxideDollarValueAdjustment;
      energyBenefits.CarbonMonoxideDollarValue = 0.00045359237 * num6 * this._locEnvEffectDollarAdjust.CarbonMonoxideDollarValueAdjustment;
      energyBenefits.NitrogenDioxideDollarValue = 0.00045359237 * num4 * this._locEnvEffectDollarAdjust.NitrogenDioxideDollarValueAdjustment;
      energyBenefits.SulphurDioxideDollarValue = 0.00045359237 * num5 * this._locEnvEffectDollarAdjust.SulphurDioxideDollarValueAdjustment;
      energyBenefits.PM10DollarValue = 0.00045359237 * num7 * this._locEnvEffectDollarAdjust.PM10DollarValueAdjustment;
      energyBenefits.PM25DollarValue = 0.00045359237 * num8 * this._locEnvEffectDollarAdjust.PM25DollarValueAdjustment;
      energyBenefits.VOCDollarValue = 0.00045359237 * num9 * this._locEnvEffectDollarAdjust.VOCDollarValueAdjustment;
      energyBenefits.NitrogenDioxideLbs = num4;
      energyBenefits.SulphurDioxideLbs = num5;
      energyBenefits.CarbonMonoxideLbs = num6;
      energyBenefits.PM10Lbs = num7;
      energyBenefits.PM25Lbs = num8;
      energyBenefits.VOCLbs = num9;
      if (this._locationCost != null)
      {
        energyBenefits.KWhPrice = this._locationCost.Electricity;
        energyBenefits.ThermPrice = this._locationCost.Fuels / 10.0;
        energyBenefits.ThermsHeatClimateDollars = this._locationCost.Fuels * energyBenefits.MbtuHeatClimate;
      }
      return energyBenefits;
    }

    private VintageBenefit CalculateVintageEnergy(
      ISession sessLocSpec,
      LeafType leafType,
      EnergyTree tree,
      EnergyBuilding building,
      ClimateRegionBuilding climateRegionBuilding,
      TreeLocation treelocation,
      Directions direction,
      double treeHeightMeters,
      double distanceMeters)
    {
      VintageBenefit vintageEnergy = new VintageBenefit();
      double num1 = treeHeightMeters / 0.3048;
      double num2 = 0.5 + 0.5 * ((100.0 - (double) tree.PercentCrownMissing) / 100.0);
      double num3 = leafType.Id != 1 ? 0.5 + 0.5 * ((100.0 - (double) tree.PercentCrownMissing) / 100.0) : 1.0;
      TreeHeightClass key1;
      double num4;
      TreeHeightClass key2;
      double num5;
      if (num1 >= 50.0)
      {
        key1 = TreeHeightClass.Large;
        num4 = 50.0;
        key2 = TreeHeightClass.Large;
        num5 = 51.0;
      }
      else if (num1 >= 42.5)
      {
        key1 = TreeHeightClass.Medium;
        num4 = 42.5;
        key2 = TreeHeightClass.Large;
        num5 = 50.0;
      }
      else if (num1 >= 27.5)
      {
        key1 = TreeHeightClass.Small;
        num4 = 27.5;
        key2 = TreeHeightClass.Medium;
        num5 = 42.5;
      }
      else
      {
        key1 = TreeHeightClass.Small;
        num4 = 10.0;
        key2 = TreeHeightClass.Small;
        num5 = 27.5;
      }
      double num6;
      double num7;
      if (key2 == TreeHeightClass.Small)
      {
        num6 = 0.0;
        num7 = 0.0;
      }
      else
      {
        num6 = (double) this._dShadingFactors[EnergyEffect.Cooling][key1][leafType][treelocation][direction].Factor;
        num7 = (double) this._dShadingFactors[EnergyEffect.Heating][key1][leafType][treelocation][direction].Factor;
      }
      double factor1 = (double) this._dShadingFactors[EnergyEffect.Cooling][key2][leafType][treelocation][direction].Factor;
      double factor2 = (double) this._dShadingFactors[EnergyEffect.Heating][key2][leafType][treelocation][direction].Factor;
      double num8 = (factor1 - num6) / (num5 - num4);
      double num9 = num7;
      double num10 = (factor2 - num9) / (num5 - num4);
      double num11 = (num8 * (num1 - num4) + num6) * num2;
      double num12 = num1 - num4;
      double num13 = (num10 * num12 + num7) * num3;
      double num14 = this._dBuildingVintageFactors[building.Vintage][EnergyEffect.Cooling].Factor * num11;
      double num15 = this._dBuildingVintageFactors[building.Vintage][EnergyEffect.Heating].Factor * num13;
      double num16;
      double num17;
      if (treeHeightMeters < 6.0)
      {
        num16 = 0.0;
        num17 = 0.0;
      }
      else
      {
        TreeHeightClass key3 = treeHeightMeters <= 15.0 ? (treeHeightMeters <= 10.0 ? TreeHeightClass.Small : TreeHeightClass.Medium) : TreeHeightClass.Large;
        IList<TreeBuildingCover> treeBuildingCoverList = sessLocSpec.CreateCriteria<TreeBuildingCover>().AddOrder(Order.Asc("PercentCover")).SetCacheable(true).List<TreeBuildingCover>();
        TreeBuildingCover key4;
        TreeBuildingCover key5;
        if (this._percentTreeBuildingCover <= (double) treeBuildingCoverList[1].PercentCover)
        {
          key4 = treeBuildingCoverList[0];
          key5 = treeBuildingCoverList[1];
        }
        else
        {
          key4 = treeBuildingCoverList[1];
          key5 = treeBuildingCoverList[2];
        }
        double factor3 = this._dClimateFactors[building.Vintage][EnergyEffect.Cooling][key3][leafType][key4].Factor;
        double factor4 = this._dClimateFactors[building.Vintage][EnergyEffect.Cooling][key3][leafType][key5].Factor;
        double factor5 = this._dClimateFactors[building.Vintage][EnergyEffect.Heating][key3][leafType][key4].Factor;
        double factor6 = this._dClimateFactors[building.Vintage][EnergyEffect.Heating][key3][leafType][key5].Factor;
        double num18 = (factor4 - factor3) / (double) (key5.PercentCover - key4.PercentCover);
        double num19 = (factor6 - factor5) / (double) (key5.PercentCover - key4.PercentCover);
        double num20 = this._percentTreeBuildingCover - (double) key4.PercentCover;
        double num21 = num18 * num20 + factor3;
        double num22 = num19 * (this._percentTreeBuildingCover - (double) key4.PercentCover) + factor5;
        if (num21 < factor4 * 0.333)
          num21 = factor4 * 0.333;
        if (num22 < factor6 * 0.333)
          num22 = factor6 * 0.333;
        num16 = num21 * num2;
        num17 = num22 * num3;
      }
      double num23 = 0.0;
      if (leafType.Id == 2 && num1 >= 10.0)
      {
        double num24 = key2 != TreeHeightClass.Small ? (double) this._dWindbreakFactors[building.Vintage][key1][leafType].Factor : 0.0;
        num23 = (((double) this._dWindbreakFactors[building.Vintage][key2][leafType].Factor - num24) / (num5 - num4) * (num1 - num4) + num24) * num3;
        if (num23 != 0.0)
        {
          double num25 = distanceMeters / treeHeightMeters;
          if (num25 > 5.0)
          {
            if (num25 <= 15.0)
              num23 -= (num25 - 5.0) / 10.0 * 0.714286 * num23;
            else
              num23 = num25 > 35.0 ? 0.0 : num23 * 0.285714 - (num25 - 15.0) / 20.0 * (num23 * 0.285714);
          }
        }
      }
      double electricityEmissionsFactor = this._climateRegion.ElectricityEmissionsFactor;
      if (building.Heated != YesNoUnknown.No)
      {
        double percentNaturalGasHeat = (double) climateRegionBuilding.PercentNaturalGasHeat;
        double percentFuelOilHeat = (double) climateRegionBuilding.PercentFuelOilHeat;
        double percentOtherHeat = (double) climateRegionBuilding.PercentOtherHeat;
        double num26 = ((double) climateRegionBuilding.PercentElectricalHeat + (double) climateRegionBuilding.PercentHeatPump) * num15;
        double num27 = (double) climateRegionBuilding.PercentNaturalGasHeat * num15;
        double num28 = (double) climateRegionBuilding.PercentFuelOilHeat * num15;
        double num29 = (double) climateRegionBuilding.PercentOtherHeat * num15;
        double num30 = ((double) climateRegionBuilding.PercentElectricalHeat + (double) climateRegionBuilding.PercentHeatPump) * num17;
        double num31 = (double) climateRegionBuilding.PercentNaturalGasHeat * num17;
        double num32 = (double) climateRegionBuilding.PercentFuelOilHeat * num17;
        double num33 = (double) climateRegionBuilding.PercentOtherHeat * num17;
        double num34 = ((double) climateRegionBuilding.PercentElectricalHeat + (double) climateRegionBuilding.PercentHeatPump) * num23;
        double num35 = (double) climateRegionBuilding.PercentNaturalGasHeat * num23;
        double num36 = (double) climateRegionBuilding.PercentFuelOilHeat * num23;
        double num37 = (double) climateRegionBuilding.PercentOtherHeat * num23;
        double num38 = 0.072;
        double num39 = 0.0527;
        double num40 = 0.0527;
        vintageEnergy.MbtuHeatShade = (num27 / num39 + num28 / num38 + num29 / num40) / 1000.0;
        vintageEnergy.MbtuHeatClimate = (num31 / num39 + num32 / num38 + num33 / num40) / 1000.0;
        vintageEnergy.MbtuWindHeat = (num35 / num39 + num36 / num38 + num37 / num40) / 1000.0;
        vintageEnergy.KwhElectricityHeatShade = num26 / electricityEmissionsFactor;
        vintageEnergy.KWhElectricityHeatClimate = num30 / electricityEmissionsFactor;
        vintageEnergy.KWhElectricityHeatWind = num34 / electricityEmissionsFactor;
      }
      if (building.AirConditioned != YesNoUnknown.No)
      {
        double num41;
        double num42;
        if (building.AirConditioned == YesNoUnknown.Unknown)
        {
          num41 = (1.0 - (double) climateRegionBuilding.PercentNoCooling) * num14;
          num42 = (1.0 - (double) climateRegionBuilding.PercentNoCooling) * num16;
        }
        else
        {
          num41 = num14;
          num42 = num16;
        }
        vintageEnergy.KWhElectricityCoolShade = num41 / electricityEmissionsFactor;
        vintageEnergy.KWhElectricityCoolClimate = num42 / electricityEmissionsFactor;
      }
      if (this._locationCost != null)
      {
        vintageEnergy.FuelDollars = this._locationCost.Fuels * vintageEnergy.MbtuTotal;
        vintageEnergy.ElectricDollars = this._locationCost.Electricity * vintageEnergy.KWhTotal;
      }
      return vintageEnergy;
    }
  }
}
