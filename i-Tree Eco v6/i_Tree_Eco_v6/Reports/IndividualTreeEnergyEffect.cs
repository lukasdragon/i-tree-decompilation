// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeEnergyEffect
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using Eco.Util;
using i_Tree_Eco_v6.Energy;
using LocationSpecies.Domain;
using System;
using System.Collections.Generic;
using TreeEnergy;

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeEnergyEffect : IndividualTreeEnergyFieldsBase
  {
    public double ElectricityAvoidedHeating { get; set; }

    public double HeatingKwhDollars { get; set; }

    public double FuelsAvoidedHeating { get; set; }

    public double HeatingMBTUDollars { get; set; }

    public double ElectricityAvoidedCooling { get; set; }

    public double CoolingKwhDollars { get; set; }

    public double CarbonAvoidedKgPerYear { get; set; }

    public double CarbonAvoidedValuePerYear { get; set; }

    public override IndividualTreeEnergyFieldsBase GetEnergyImpactValues(
      DatabaseReport dbr,
      Tree tree,
      Species species,
      IndividualTree indivTree,
      double? percentGroundCover)
    {
      IndividualTreeEnergyEffectsAgregateValues agregateValues = new IndividualTreeEnergyEffectsAgregateValues();
      EnergyTreeView tree1 = new EnergyTreeView(tree, species);
      foreach (Building building in (IEnumerable<Building>) tree.Buildings)
      {
        if (building.Direction != (short) -1)
        {
          if ((double) Math.Abs(building.Distance + 1f) >= 1.4012984643248171E-45)
          {
            try
            {
              agregateValues += indivTree.CalculateEnergyBenefits((EnergyBuilding) new EnergyBuildingView(building), (EnergyTree) tree1, percentGroundCover);
            }
            catch (Exception ex)
            {
              throw ex;
            }
          }
        }
      }
      return (IndividualTreeEnergyFieldsBase) this.AssignIndividualTreeEnergyEffectValues(dbr, agregateValues);
    }

    private IndividualTreeEnergyEffect AssignIndividualTreeEnergyEffectValues(
      DatabaseReport dbr,
      IndividualTreeEnergyEffectsAgregateValues agregateValues)
    {
      this.ElectricityAvoidedHeating = agregateValues.treeElectricityAvoidedHeating;
      this.HeatingKwhDollars = this.ElectricityAvoidedHeating * dbr.customizedElectricityDollarsPerKwh;
      this.FuelsAvoidedHeating = agregateValues.treeFuelsAvoidedHeating;
      this.HeatingMBTUDollars = this.FuelsAvoidedHeating * dbr.customizedHeatingDollarsPerTherm * 10.0023877;
      this.ElectricityAvoidedCooling = agregateValues.treeElectricityAvoidedCooling;
      this.CoolingKwhDollars = this.ElectricityAvoidedCooling * dbr.customizedElectricityDollarsPerKwh;
      this.Total = this.HeatingKwhDollars + this.HeatingMBTUDollars + this.CoolingKwhDollars;
      this.CarbonAvoidedKgPerYear = EstimateUtil.ConvertToMetric(agregateValues.treeCarbonAvoidedLbs, Units.Pounds, true);
      this.CarbonAvoidedValuePerYear = this.CarbonAvoidedKgPerYear * (dbr.customizedCarbonDollarsPerTon / 1000.0);
      return this;
    }
  }
}
