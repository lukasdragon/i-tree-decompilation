// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeEnergyAvoidedPollutant
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using i_Tree_Eco_v6.Energy;
using LocationSpecies.Domain;
using System;
using System.Collections.Generic;
using TreeEnergy;

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeEnergyAvoidedPollutant : IndividualTreeEnergyFieldsBase
  {
    public double CarbonMonoxideLbs { get; set; }

    public double NitrogenDioxideLbs { get; set; }

    public double SulphurDioxideLbs { get; set; }

    public double PM10Lbs { get; set; }

    public double PM25Lbs { get; set; }

    public double VOCLbs { get; set; }

    public double CarbonMonoxideDollarValue { get; set; }

    public double NitrogenDioxideDollarValue { get; set; }

    public double SulphurDioxideDollarValue { get; set; }

    public double PM10DollarValue { get; set; }

    public double PM25DollarValue { get; set; }

    public double VOCDollarValue { get; set; }

    public override IndividualTreeEnergyFieldsBase GetEnergyImpactValues(
      DatabaseReport dbr,
      Tree tree,
      Species species,
      IndividualTree indivTree,
      double? percentGroundCover)
    {
      IndividualTreeEnergyAvoidedPollutant energyImpactValues = this;
      EnergyTreeView tree1 = new EnergyTreeView(tree, species);
      foreach (Building building in (IEnumerable<Building>) tree.Buildings)
      {
        if (building.Direction != (short) -1)
        {
          if ((double) Math.Abs(building.Distance + 1f) >= 1.4012984643248171E-45)
          {
            try
            {
              energyImpactValues += indivTree.CalculateEnergyBenefits((EnergyBuilding) new EnergyBuildingView(building), (EnergyTree) tree1, percentGroundCover);
            }
            catch (Exception ex)
            {
              throw ex;
            }
          }
        }
      }
      return (IndividualTreeEnergyFieldsBase) energyImpactValues;
    }

    public static IndividualTreeEnergyAvoidedPollutant operator +(
      IndividualTreeEnergyAvoidedPollutant l,
      TreeBenefit r)
    {
      l.CarbonMonoxideLbs += r.CarbonMonoxideLbs;
      l.NitrogenDioxideLbs += r.NitrogenDioxideLbs;
      l.SulphurDioxideLbs += r.SulphurDioxideLbs;
      l.PM10Lbs += r.PM10Lbs;
      l.PM25Lbs += r.PM25Lbs;
      l.VOCLbs += r.VOCLbs;
      l.CarbonMonoxideDollarValue += r.CarbonMonoxideDollarValue;
      l.NitrogenDioxideDollarValue += r.NitrogenDioxideDollarValue;
      l.SulphurDioxideDollarValue += r.SulphurDioxideDollarValue;
      l.PM10DollarValue += r.PM10DollarValue;
      l.PM25DollarValue += r.PM25DollarValue;
      l.VOCDollarValue += r.VOCDollarValue;
      l.Total += r.CarbonMonoxideDollarValue + r.NitrogenDioxideDollarValue + r.SulphurDioxideDollarValue + r.PM10DollarValue + r.PM25DollarValue + r.VOCDollarValue;
      return l;
    }
  }
}
