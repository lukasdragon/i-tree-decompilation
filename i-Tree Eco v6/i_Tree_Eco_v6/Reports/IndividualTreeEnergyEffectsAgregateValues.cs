// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeEnergyEffectsAgregateValues
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using TreeEnergy;

namespace i_Tree_Eco_v6.Reports
{
  internal class IndividualTreeEnergyEffectsAgregateValues
  {
    public double treeElectricityAvoidedHeating { get; set; }

    public double treeElectricityAvoidedCooling { get; set; }

    public double treeFuelsAvoidedHeating { get; set; }

    public double treeCarbonAvoidedLbs { get; set; }

    public IndividualTreeEnergyEffectsAgregateValues()
    {
      this.treeElectricityAvoidedHeating = 0.0;
      this.treeElectricityAvoidedCooling = 0.0;
      this.treeFuelsAvoidedHeating = 0.0;
      this.treeCarbonAvoidedLbs = 0.0;
    }

    public static IndividualTreeEnergyEffectsAgregateValues operator +(
      IndividualTreeEnergyEffectsAgregateValues l,
      TreeBenefit r)
    {
      return new IndividualTreeEnergyEffectsAgregateValues()
      {
        treeElectricityAvoidedCooling = l.treeElectricityAvoidedCooling + (r.KWhElectricityCoolClimate + r.KWhElectricityCoolShade),
        treeElectricityAvoidedHeating = l.treeElectricityAvoidedHeating + (r.KWhElectricityHeatClimate + r.KwhElectricityHeatShade + r.KWhElectricityHeatWind),
        treeFuelsAvoidedHeating = l.treeFuelsAvoidedHeating + r.MbtuTotal,
        treeCarbonAvoidedLbs = l.treeCarbonAvoidedLbs + UnitsHelper.CarbonDioxideToCarbon(r.CarbonDioxideLbsMbtuHeatShade + r.CarbonDioxideLbsMbtuHeatClimate + r.CarbonDioxideLbsMbtuHeatWind + r.CarbonDioxideLbsElectricHeatShade + r.CarbonDioxideLbsElectricCoolShade + r.CarbonDioxideLbsElectricHeatClimate + r.CarbonDioxideLbsElectricCoolClimate + r.CarbonDioxideLbsElectricHeatWind)
      };
    }
  }
}
