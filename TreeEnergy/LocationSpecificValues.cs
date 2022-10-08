// Decompiled with JetBrains decompiler
// Type: TreeEnergy.LocationSpecificValues
// Assembly: TreeEnergy, Version=1.1.6718.0, Culture=neutral, PublicKeyToken=null
// MVID: 999CE94F-70B0-42EB-8D64-3ADB949CAFD0
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\TreeEnergy.dll

using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeEnergy
{
  public class LocationSpecificValues
  {
    private ISession _sessLocSpec;
    private double _exchangeRate;
    private ClimateRegion _climRegion;
    private LocationEnvironmentalEffect _locEnvEffect;
    private EnvironmentalEffectDollarAdjustments _locEnvEffectDollarAdjust;
    private LocationCost _locationCost;

    public int NationId { get; set; }

    public int StateId { get; set; }

    public int CountyId { get; set; }

    public int PlaceId { get; set; }

    public ClimateRegion climRegion => this._climRegion;

    public LocationEnvironmentalEffect locEnvEffect => this._locEnvEffect;

    public EnvironmentalEffectDollarAdjustments locEnvEffectDollarAdjust => this._locEnvEffectDollarAdjust;

    public LocationCost locationCost => this._locationCost;

    public LocationSpecificValues(ISession sessLocSpec, double exchangeRate)
    {
      if (exchangeRate <= 0.0)
        throw new ArgumentException(nameof (exchangeRate));
      this._sessLocSpec = sessLocSpec;
      this._exchangeRate = exchangeRate;
    }

    public void GetLocationSpecificValues(Location loc)
    {
      LocationRelation locationRelation1 = this._sessLocSpec.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) loc)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>();
      if (locationRelation1.Level == (short) 5)
        this.PlaceId = locationRelation1.Location.Id;
      else if (locationRelation1.Level == (short) 4)
        this.CountyId = locationRelation1.Location.Id;
      else if (locationRelation1.Level == (short) 3)
        this.StateId = locationRelation1.Location.Id;
      else if (locationRelation1.Level == (short) 2)
        this.NationId = locationRelation1.Location.Id;
      while (locationRelation1.Level > (short) 2)
      {
        locationRelation1 = this._sessLocSpec.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location.Id", (object) locationRelation1.Parent.Id)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>();
        if (locationRelation1.Level == (short) 4)
          this.CountyId = locationRelation1.Location.Id;
        else if (locationRelation1.Level == (short) 3)
          this.StateId = locationRelation1.Location.Id;
        else if (locationRelation1.Level == (short) 2)
          this.NationId = locationRelation1.Location.Id;
      }
      this._locationCost = this._sessLocSpec.CreateCriteria<LocationCost>().Add((ICriterion) Restrictions.Eq("Location.Id", (object) this.StateId)).SetCacheable(true).UniqueResult<LocationCost>();
      if (this._locationCost == null)
        this._locationCost = this._sessLocSpec.CreateCriteria<LocationCost>().Add((ICriterion) Restrictions.Eq("Location.Id", (object) this.NationId)).SetCacheable(true).UniqueResult<LocationCost>();
      this._climRegion = loc.ClimateRegion;
      if (this._climRegion == null)
      {
        for (LocationRelation locationRelation2 = this._sessLocSpec.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) loc)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>(); this._climRegion == null && locationRelation2.Level > (short) 2; this._climRegion = locationRelation2.Location.ClimateRegion)
          locationRelation2 = this._sessLocSpec.CreateCriteria<LocationRelation>().Add((ICriterion) Restrictions.Eq("Location", (object) locationRelation2.Parent)).Add((ICriterion) Restrictions.IsNotNull("Code")).SetCacheable(true).UniqueResult<LocationRelation>();
      }
      this._locEnvEffect = this.GetLocationEnvironmentalEffect(loc.Id);
      EnvironmentalEffectDollarAdjustments dollarAdjustments = new EnvironmentalEffectDollarAdjustments();
      LocationEnvironmentalValue environmentalValue = this._sessLocSpec.CreateCriteria<LocationEnvironmentalValue>().Add((ICriterion) Restrictions.Eq("Location.Id", (object) this.NationId)).SetCacheable(true).UniqueResult<LocationEnvironmentalValue>() ?? this._sessLocSpec.CreateCriteria<LocationEnvironmentalValue>().Add((ICriterion) Restrictions.Eq("Location.Id", (object) 219)).SetCacheable(true).UniqueResult<LocationEnvironmentalValue>();
      dollarAdjustments.CarbonDioxideDollarValueAdjustment = (double) environmentalValue.Carbon * 12.0 / 44.0;
      LocationPpiAdjustment ppiAdj = this._sessLocSpec.Query<LocationPpiAdjustment>().WithOptions<LocationPpiAdjustment>((Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Where<LocationPpiAdjustment>((System.Linq.Expressions.Expression<Func<LocationPpiAdjustment, bool>>) (ppi => ppi.Location.Id == this.NationId)).OrderByDescending<LocationPpiAdjustment, int>((System.Linq.Expressions.Expression<Func<LocationPpiAdjustment, int>>) (ppi => ppi.PpiYear)).FirstOrDefault<LocationPpiAdjustment>();
      if (ppiAdj == null)
        ppiAdj = this._sessLocSpec.Query<LocationPpiAdjustment>().WithOptions<LocationPpiAdjustment>((Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Where<LocationPpiAdjustment>((System.Linq.Expressions.Expression<Func<LocationPpiAdjustment, bool>>) (ppi => ppi.Location.Id == 219)).OrderByDescending<LocationPpiAdjustment, int>((System.Linq.Expressions.Expression<Func<LocationPpiAdjustment, int>>) (ppi => ppi.PpiYear)).FirstOrDefault<LocationPpiAdjustment>();
      PollutantBaseCost pollutantBaseCost1 = this._sessLocSpec.Query<PollutantBaseCost>().WithOptions<PollutantBaseCost>((Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Where<PollutantBaseCost>((System.Linq.Expressions.Expression<Func<PollutantBaseCost, bool>>) (pbc => pbc.Location.Id == 219 && pbc.Pollutant.Id == 7)).Single<PollutantBaseCost>();
      double ppiValue = pollutantBaseCost1.PpiAdjustment.PpiValue;
      dollarAdjustments.VOCDollarValueAdjustment = pollutantBaseCost1.BaseCost * (ppiAdj.PpiValue / ppiValue);
      if (this.NationId == 219 && this.StateId != 236 && this.StateId != 256 && this.StateId != 303)
      {
        Location location = this._sessLocSpec.Get<Location>((object) this.PlaceId) ?? this._sessLocSpec.Get<Location>((object) this.CountyId);
        IList<LocationBenefit> locationBenefitList = this._sessLocSpec.CreateCriteria<LocationBenefit>().Add((ICriterion) Restrictions.Eq("Location.Id", (object) this.CountyId)).SetCacheable(true).List<LocationBenefit>() ?? this._sessLocSpec.CreateCriteria<LocationBenefit>().Add((ICriterion) Restrictions.Eq("Location.Id", (object) this.StateId)).SetCacheable(true).List<LocationBenefit>();
        dollarAdjustments.CarbonMonoxideDollarValueAdjustment = 0.0;
        dollarAdjustments.NitrogenDioxideDollarValueAdjustment = 0.0;
        dollarAdjustments.SulphurDioxideDollarValueAdjustment = 0.0;
        dollarAdjustments.OzoneDollarValueAdjustment = 0.0;
        dollarAdjustments.PM25DollarValueAdjustment = 0.0;
        foreach (LocationBenefit locationBenefit in (IEnumerable<LocationBenefit>) locationBenefitList)
        {
          double num = !locationBenefit.IsUrban ? 1.0 - location.PercentUrban : location.PercentUrban;
          if (locationBenefit.Pollutant.Id == 1)
            dollarAdjustments.CarbonMonoxideDollarValueAdjustment += locationBenefit.BenefitValue / locationBenefit.RemovalRate * num;
          else if (locationBenefit.Pollutant.Id == 2)
            dollarAdjustments.NitrogenDioxideDollarValueAdjustment += locationBenefit.BenefitValue / locationBenefit.RemovalRate * num;
          else if (locationBenefit.Pollutant.Id == 6)
            dollarAdjustments.SulphurDioxideDollarValueAdjustment += locationBenefit.BenefitValue / locationBenefit.RemovalRate * num;
          else if (locationBenefit.Pollutant.Id == 3)
            dollarAdjustments.OzoneDollarValueAdjustment += locationBenefit.BenefitValue / locationBenefit.RemovalRate * num;
          else if (locationBenefit.Pollutant.Id == 5)
            dollarAdjustments.PM25DollarValueAdjustment += locationBenefit.BenefitValue / locationBenefit.RemovalRate * num;
        }
        PollutantBaseCost pollutantBaseCost2 = this._sessLocSpec.Query<PollutantBaseCost>().WithOptions<PollutantBaseCost>((Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Where<PollutantBaseCost>((System.Linq.Expressions.Expression<Func<PollutantBaseCost, bool>>) (pbc => pbc.Location == ppiAdj.Location && pbc.Pollutant.Id == 4)).Single<PollutantBaseCost>();
        LocationPpiAdjustment ppiAdjustment = pollutantBaseCost2.PpiAdjustment;
        dollarAdjustments.PM10DollarValueAdjustment = pollutantBaseCost2.BaseCost * (ppiAdj.PpiValue / ppiAdjustment.PpiValue) * location.PercentUrban + pollutantBaseCost2.BaseCost * (ppiAdj.PpiValue / ppiAdjustment.PpiValue) * 0.018209307 * (1.0 - location.PercentUrban);
      }
      else
      {
        List<PollutantBaseCost> list = this._sessLocSpec.Query<PollutantBaseCost>().WithOptions<PollutantBaseCost>((Action<NhQueryableOptions>) (o => o.SetCacheable(true))).Where<PollutantBaseCost>((System.Linq.Expressions.Expression<Func<PollutantBaseCost, bool>>) (pbc => pbc.Location == ppiAdj.Location)).ToList<PollutantBaseCost>();
        LocationUrban locationUrban = this._sessLocSpec.CreateCriteria<LocationUrban>().Add((ICriterion) Restrictions.Eq("Location.Id", (object) this.StateId)).SetCacheable(true).UniqueResult<LocationUrban>();
        foreach (PollutantBaseCost pollutantBaseCost3 in (IEnumerable<PollutantBaseCost>) list)
        {
          LocationPpiAdjustment ppiAdjustment = pollutantBaseCost3.PpiAdjustment;
          if (ppiAdjustment != null)
          {
            double num = pollutantBaseCost3.BaseCost * (ppiAdj.PpiValue / ppiAdjustment.PpiValue) * this._exchangeRate;
            if (pollutantBaseCost3.Pollutant.Id == 1)
              dollarAdjustments.CarbonMonoxideDollarValueAdjustment = num;
            else if (pollutantBaseCost3.Pollutant.Id == 2)
              dollarAdjustments.NitrogenDioxideDollarValueAdjustment = num;
            else if (pollutantBaseCost3.Pollutant.Id == 3)
              dollarAdjustments.OzoneDollarValueAdjustment = num;
            else if (pollutantBaseCost3.Pollutant.Id == 4)
              dollarAdjustments.PM10DollarValueAdjustment = num;
            else if (pollutantBaseCost3.Pollutant.Id == 5)
              dollarAdjustments.PM25DollarValueAdjustment = num;
            else if (pollutantBaseCost3.Pollutant.Id == 6)
              dollarAdjustments.SulphurDioxideDollarValueAdjustment = num;
          }
        }
        if (locationUrban != null)
        {
          double num = locationUrban.UrbanPopulation / locationUrban.UrbanArea;
          RegressionBenefit regressionBenefit1 = this._sessLocSpec.Get<RegressionBenefit>((object) 2);
          dollarAdjustments.NitrogenDioxideDollarValueAdjustment = regressionBenefit1.ACoefficient + regressionBenefit1.BCoefficient * num;
          RegressionBenefit regressionBenefit2 = this._sessLocSpec.Get<RegressionBenefit>((object) 3);
          dollarAdjustments.OzoneDollarValueAdjustment = regressionBenefit2.ACoefficient + regressionBenefit2.BCoefficient * num;
          RegressionBenefit regressionBenefit3 = this._sessLocSpec.Get<RegressionBenefit>((object) 6);
          dollarAdjustments.SulphurDioxideDollarValueAdjustment = regressionBenefit3.ACoefficient + regressionBenefit3.BCoefficient * num;
          RegressionBenefit regressionBenefit4 = this._sessLocSpec.Get<RegressionBenefit>((object) 5);
          dollarAdjustments.PM25DollarValueAdjustment = regressionBenefit4.ACoefficient + regressionBenefit4.BCoefficient * num;
        }
      }
      this._locEnvEffectDollarAdjust = dollarAdjustments;
    }

    public LocationEnvironmentalEffect GetLocationEnvironmentalEffect(
      int locationId)
    {
      LocationEnvironmentalEffect dest = new LocationEnvironmentalEffect();
      using (this._sessLocSpec.BeginTransaction())
      {
        while (true)
        {
          LocationEnvironmentalEffect source = this._sessLocSpec.Query<LocationEnvironmentalEffect>().WithOptions<LocationEnvironmentalEffect>((Action<NhQueryableOptions>) (opt => opt.SetCacheable(true))).Where<LocationEnvironmentalEffect>((System.Linq.Expressions.Expression<Func<LocationEnvironmentalEffect, bool>>) (e => e.Id == locationId)).SingleOrDefault<LocationEnvironmentalEffect>();
          if (source != null)
            LocationSpecificValues.MergeEnvironmentalEffects(dest, source);
          LocationRelation locationRelation = this._sessLocSpec.Query<LocationRelation>().WithOptions<LocationRelation>((Action<NhQueryableOptions>) (opt => opt.SetCacheable(true))).Where<LocationRelation>((System.Linq.Expressions.Expression<Func<LocationRelation, bool>>) (r => r.Location.Id == locationId && r.Code != default (string))).SingleOrDefault<LocationRelation>();
          if (locationRelation != null && locationRelation.Level >= (short) 2)
            locationId = locationRelation.Parent.Id;
          else
            break;
        }
        LocationEnvironmentalEffect source1 = this._sessLocSpec.Query<LocationEnvironmentalEffect>().WithOptions<LocationEnvironmentalEffect>((Action<NhQueryableOptions>) (opt => opt.SetCacheable(true))).Where<LocationEnvironmentalEffect>((System.Linq.Expressions.Expression<Func<LocationEnvironmentalEffect, bool>>) (e => e.Id == 219)).SingleOrDefault<LocationEnvironmentalEffect>();
        LocationSpecificValues.MergeEnvironmentalEffects(dest, source1);
      }
      return dest;
    }

    private static void MergeEnvironmentalEffects(
      LocationEnvironmentalEffect dest,
      LocationEnvironmentalEffect source)
    {
      dest.CarbonSequestration = dest.CarbonSequestration ?? source.CarbonSequestration;
      dest.CO2MMBtu = dest.CO2MMBtu ?? source.CO2MMBtu;
      dest.CO2MWh = dest.CO2MWh ?? source.CO2MWh;
      dest.COMWh = dest.COMWh ?? source.COMWh;
      dest.Electricity = dest.Electricity ?? source.Electricity;
      dest.NOxMMbtu = dest.NOxMMbtu ?? source.NOxMMbtu;
      dest.NoxMWh = dest.NoxMWh ?? source.NoxMWh;
      dest.PM10MWh = dest.PM10MWh ?? source.PM10MWh;
      dest.PM25MWh = dest.PM25MWh ?? source.PM25MWh;
      dest.SO2MMbtu = dest.SO2MMbtu ?? source.SO2MMbtu;
      dest.SO2MWh = dest.SO2MWh ?? source.SO2MWh;
      dest.TransmissionLoss = dest.TransmissionLoss ?? source.TransmissionLoss;
      dest.VOCMWh = dest.VOCMWh ?? source.VOCMWh;
    }
  }
}
