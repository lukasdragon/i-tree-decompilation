// Decompiled with JetBrains decompiler
// Type: TreeEnergy.VintageBenefit
// Assembly: TreeEnergy, Version=1.1.6718.0, Culture=neutral, PublicKeyToken=null
// MVID: 999CE94F-70B0-42EB-8D64-3ADB949CAFD0
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\TreeEnergy.dll

namespace TreeEnergy
{
  public class VintageBenefit
  {
    public double MbtuHeatShade;
    public double MbtuHeatClimate;
    public double MbtuWindHeat;
    public double KwhElectricityHeatShade;
    public double KWhElectricityHeatClimate;
    public double KWhElectricityCoolShade;
    public double KWhElectricityCoolClimate;
    public double KWhElectricityHeatWind;
    public double FuelDollars;
    public double ElectricDollars;

    public static VintageBenefit operator +(VintageBenefit vb1, VintageBenefit vb2) => new VintageBenefit()
    {
      MbtuHeatShade = vb1.MbtuHeatShade + vb2.MbtuHeatShade,
      MbtuHeatClimate = vb1.MbtuHeatClimate + vb2.MbtuHeatClimate,
      MbtuWindHeat = vb1.MbtuWindHeat + vb2.MbtuWindHeat,
      KwhElectricityHeatShade = vb1.KwhElectricityHeatShade + vb2.KwhElectricityHeatShade,
      KWhElectricityHeatClimate = vb1.KWhElectricityHeatClimate + vb2.KWhElectricityHeatClimate,
      KWhElectricityCoolShade = vb1.KWhElectricityCoolShade + vb2.KWhElectricityCoolShade,
      KWhElectricityCoolClimate = vb1.KWhElectricityCoolClimate + vb2.KWhElectricityCoolClimate,
      KWhElectricityHeatWind = vb1.KWhElectricityHeatWind + vb2.KWhElectricityHeatWind,
      FuelDollars = vb1.FuelDollars + vb2.FuelDollars,
      ElectricDollars = vb1.ElectricDollars + vb2.ElectricDollars
    };

    public static VintageBenefit operator *(VintageBenefit vb, double value) => new VintageBenefit()
    {
      MbtuHeatShade = vb.MbtuHeatShade * value,
      MbtuHeatClimate = vb.MbtuHeatClimate * value,
      MbtuWindHeat = vb.MbtuWindHeat * value,
      KwhElectricityHeatShade = vb.KwhElectricityHeatShade * value,
      KWhElectricityHeatClimate = vb.KWhElectricityHeatClimate * value,
      KWhElectricityCoolShade = vb.KWhElectricityCoolShade * value,
      KWhElectricityCoolClimate = vb.KWhElectricityCoolClimate * value,
      KWhElectricityHeatWind = vb.KWhElectricityHeatWind * value,
      FuelDollars = vb.FuelDollars * value,
      ElectricDollars = vb.ElectricDollars * value
    };

    public double MbtuTotal => this.MbtuHeatClimate + this.MbtuHeatShade + this.MbtuWindHeat;

    public double ThermsTotal => this.MbtuTotal * 10.0;

    public double KWhTotal => this.KWhElectricityCoolClimate + this.KWhElectricityCoolShade + this.KWhElectricityHeatClimate + this.KwhElectricityHeatShade + this.KWhElectricityHeatWind;

    public double ThermsHeatClimate => this.MbtuHeatClimate * 10.0;
  }
}
