// Decompiled with JetBrains decompiler
// Type: TreeEnergy.TreeBenefit
// Assembly: TreeEnergy, Version=1.1.6718.0, Culture=neutral, PublicKeyToken=null
// MVID: 999CE94F-70B0-42EB-8D64-3ADB949CAFD0
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\TreeEnergy.dll

namespace TreeEnergy
{
  public class TreeBenefit : VintageBenefit
  {
    public double ThermsHeatClimateDollars;
    public double KWhPrice;
    public double ThermPrice;
    public double CarbonDioxideLbsElectricHeatClimate;
    public double CarbonDioxideLbsElectricHeatShade;
    public double CarbonDioxideLbsElectricHeatWind;
    public double CarbonDioxideLbsElectricCoolClimate;
    public double CarbonDioxideLbsElectricCoolShade;
    public double CarbonDioxideLbsMbtuHeatClimate;
    public double CarbonDioxideLbsMbtuHeatShade;
    public double CarbonDioxideLbsMbtuHeatWind;
    public double NitrogenDioxideLbs;
    public double SulphurDioxideLbs;
    public double CarbonMonoxideLbs;
    public double PM10Lbs;
    public double PM25Lbs;
    public double VOCLbs;
    public double CarbonDioxideDollarValue;
    public double NitrogenDioxideDollarValue;
    public double SulphurDioxideDollarValue;
    public double CarbonMonoxideDollarValue;
    public double PM10DollarValue;
    public double PM25DollarValue;
    public double VOCDollarValue;
    public double CalculatedTreeHeightFeet;
    public double CalculatedTreeCrownWidthFeet;
    public double CalculatedTreeCrownHeightFeet;
    public double DBHInchCalculated;

    public static TreeBenefit operator +(TreeBenefit tb1, TreeBenefit tb2)
    {
      TreeBenefit treeBenefit = new TreeBenefit();
      treeBenefit.ThermsHeatClimateDollars = tb1.ThermsHeatClimateDollars + tb2.ThermsHeatClimateDollars;
      treeBenefit.KWhPrice = tb1.KWhPrice;
      treeBenefit.ThermPrice = tb1.ThermPrice;
      treeBenefit.CarbonDioxideLbsElectricHeatClimate = tb1.CarbonDioxideLbsElectricHeatClimate + tb2.CarbonDioxideLbsElectricHeatClimate;
      treeBenefit.CarbonDioxideLbsElectricHeatShade = tb1.CarbonDioxideLbsElectricHeatShade + tb2.CarbonDioxideLbsElectricHeatShade;
      treeBenefit.CarbonDioxideLbsElectricHeatWind = tb1.CarbonDioxideLbsElectricHeatWind + tb2.CarbonDioxideLbsElectricHeatWind;
      treeBenefit.CarbonDioxideLbsElectricCoolClimate = tb1.CarbonDioxideLbsElectricCoolClimate + tb2.CarbonDioxideLbsElectricCoolClimate;
      treeBenefit.CarbonDioxideLbsElectricCoolShade = tb1.CarbonDioxideLbsElectricCoolShade + tb2.CarbonDioxideLbsElectricCoolShade;
      treeBenefit.CarbonDioxideLbsMbtuHeatClimate = tb1.CarbonDioxideLbsMbtuHeatClimate + tb2.CarbonDioxideLbsMbtuHeatClimate;
      treeBenefit.CarbonDioxideLbsMbtuHeatShade = tb1.CarbonDioxideLbsMbtuHeatShade + tb2.CarbonDioxideLbsMbtuHeatShade;
      treeBenefit.CarbonDioxideLbsMbtuHeatWind = tb1.CarbonDioxideLbsMbtuHeatWind + tb2.CarbonDioxideLbsMbtuHeatWind;
      treeBenefit.NitrogenDioxideLbs = tb1.NitrogenDioxideLbs + tb2.NitrogenDioxideLbs;
      treeBenefit.SulphurDioxideLbs = tb1.SulphurDioxideLbs + tb2.SulphurDioxideLbs;
      treeBenefit.CarbonMonoxideLbs = tb1.CarbonMonoxideLbs + tb2.CarbonMonoxideLbs;
      treeBenefit.PM10Lbs = tb1.PM10Lbs + tb2.PM10Lbs;
      treeBenefit.PM25Lbs = tb1.PM25Lbs + tb2.PM25Lbs;
      treeBenefit.VOCLbs = tb1.VOCLbs + tb2.VOCLbs;
      treeBenefit.CarbonDioxideDollarValue = tb1.CarbonDioxideDollarValue + tb2.CarbonDioxideDollarValue;
      treeBenefit.NitrogenDioxideDollarValue = tb1.NitrogenDioxideDollarValue + tb2.NitrogenDioxideDollarValue;
      treeBenefit.SulphurDioxideDollarValue = tb1.SulphurDioxideDollarValue + tb2.SulphurDioxideDollarValue;
      treeBenefit.CarbonMonoxideDollarValue = tb1.CarbonMonoxideDollarValue + tb2.CarbonMonoxideDollarValue;
      treeBenefit.PM10DollarValue = tb1.PM10DollarValue + tb2.PM10DollarValue;
      treeBenefit.PM25DollarValue = tb1.PM25DollarValue + tb2.PM25DollarValue;
      treeBenefit.VOCDollarValue = tb1.VOCDollarValue + tb2.VOCDollarValue;
      treeBenefit.CalculatedTreeHeightFeet = tb1.CalculatedTreeHeightFeet + tb2.CalculatedTreeHeightFeet;
      treeBenefit.CalculatedTreeCrownWidthFeet = tb1.CalculatedTreeCrownWidthFeet + tb2.CalculatedTreeCrownWidthFeet;
      treeBenefit.CalculatedTreeCrownHeightFeet = tb1.CalculatedTreeCrownHeightFeet + tb2.CalculatedTreeCrownHeightFeet;
      treeBenefit.DBHInchCalculated = tb1.DBHInchCalculated + tb2.DBHInchCalculated;
      treeBenefit.MbtuHeatShade = tb1.MbtuHeatShade + tb2.MbtuHeatShade;
      treeBenefit.MbtuHeatClimate = tb1.MbtuHeatClimate + tb2.MbtuHeatClimate;
      treeBenefit.MbtuWindHeat = tb1.MbtuWindHeat + tb2.MbtuWindHeat;
      treeBenefit.KwhElectricityHeatShade = tb1.KwhElectricityHeatShade + tb2.KwhElectricityHeatShade;
      treeBenefit.KWhElectricityHeatClimate = tb1.KWhElectricityHeatClimate + tb2.KWhElectricityHeatClimate;
      treeBenefit.KWhElectricityCoolShade = tb1.KWhElectricityCoolShade + tb2.KWhElectricityCoolShade;
      treeBenefit.KWhElectricityCoolClimate = tb1.KWhElectricityCoolClimate + tb2.KWhElectricityCoolClimate;
      treeBenefit.KWhElectricityHeatWind = tb1.KWhElectricityHeatWind + tb2.KWhElectricityHeatWind;
      treeBenefit.FuelDollars = tb1.FuelDollars + tb2.FuelDollars;
      treeBenefit.ElectricDollars = tb1.ElectricDollars + tb2.ElectricDollars;
      return treeBenefit;
    }

    public static TreeBenefit operator +(TreeBenefit tb, VintageBenefit vb)
    {
      TreeBenefit treeBenefit = new TreeBenefit();
      treeBenefit.ThermsHeatClimateDollars = tb.ThermsHeatClimateDollars;
      treeBenefit.KWhPrice = tb.KWhPrice;
      treeBenefit.ThermPrice = tb.ThermPrice;
      treeBenefit.CarbonDioxideLbsElectricHeatClimate = tb.CarbonDioxideLbsElectricHeatClimate;
      treeBenefit.CarbonDioxideLbsElectricHeatShade = tb.CarbonDioxideLbsElectricHeatShade;
      treeBenefit.CarbonDioxideLbsElectricHeatWind = tb.CarbonDioxideLbsElectricHeatWind;
      treeBenefit.CarbonDioxideLbsElectricCoolClimate = tb.CarbonDioxideLbsElectricCoolClimate;
      treeBenefit.CarbonDioxideLbsElectricCoolShade = tb.CarbonDioxideLbsElectricCoolShade;
      treeBenefit.CarbonDioxideLbsMbtuHeatClimate = tb.CarbonDioxideLbsMbtuHeatClimate;
      treeBenefit.CarbonDioxideLbsMbtuHeatShade = tb.CarbonDioxideLbsMbtuHeatShade;
      treeBenefit.CarbonDioxideLbsMbtuHeatWind = tb.CarbonDioxideLbsMbtuHeatWind;
      treeBenefit.NitrogenDioxideLbs = tb.NitrogenDioxideLbs;
      treeBenefit.SulphurDioxideLbs = tb.SulphurDioxideLbs;
      treeBenefit.CarbonMonoxideLbs = tb.CarbonMonoxideLbs;
      treeBenefit.PM10Lbs = tb.PM10Lbs;
      treeBenefit.PM25Lbs = tb.PM25Lbs;
      treeBenefit.VOCLbs = tb.VOCLbs;
      treeBenefit.CarbonDioxideDollarValue = tb.CarbonDioxideDollarValue;
      treeBenefit.NitrogenDioxideDollarValue = tb.NitrogenDioxideDollarValue;
      treeBenefit.SulphurDioxideDollarValue = tb.SulphurDioxideDollarValue;
      treeBenefit.CarbonMonoxideDollarValue = tb.CarbonMonoxideDollarValue;
      treeBenefit.PM10DollarValue = tb.PM10DollarValue;
      treeBenefit.PM25DollarValue = tb.PM25DollarValue;
      treeBenefit.VOCDollarValue = tb.VOCDollarValue;
      treeBenefit.CalculatedTreeHeightFeet = tb.CalculatedTreeHeightFeet;
      treeBenefit.CalculatedTreeCrownWidthFeet = tb.CalculatedTreeCrownWidthFeet;
      treeBenefit.CalculatedTreeCrownHeightFeet = tb.CalculatedTreeCrownHeightFeet;
      treeBenefit.DBHInchCalculated = tb.DBHInchCalculated;
      treeBenefit.MbtuHeatShade = tb.MbtuHeatShade + vb.MbtuHeatShade;
      treeBenefit.MbtuHeatClimate = tb.MbtuHeatClimate + vb.MbtuHeatClimate;
      treeBenefit.MbtuWindHeat = tb.MbtuWindHeat + vb.MbtuWindHeat;
      treeBenefit.KwhElectricityHeatShade = tb.KwhElectricityHeatShade + vb.KwhElectricityHeatShade;
      treeBenefit.KWhElectricityHeatClimate = tb.KWhElectricityHeatClimate + vb.KWhElectricityHeatClimate;
      treeBenefit.KWhElectricityCoolShade = tb.KWhElectricityCoolShade + vb.KWhElectricityCoolShade;
      treeBenefit.KWhElectricityCoolClimate = tb.KWhElectricityCoolClimate + vb.KWhElectricityCoolClimate;
      treeBenefit.KWhElectricityHeatWind = tb.KWhElectricityHeatWind + vb.KWhElectricityHeatWind;
      treeBenefit.FuelDollars = tb.FuelDollars + vb.FuelDollars;
      treeBenefit.ElectricDollars = tb.ElectricDollars + vb.ElectricDollars;
      return treeBenefit;
    }

    public double CarbonDioxideLbs => this.CarbonDioxideLbsElectricCoolClimate + this.CarbonDioxideLbsElectricCoolShade + this.CarbonDioxideLbsElectricHeatClimate + this.CarbonDioxideLbsElectricHeatShade + this.CarbonDioxideLbsElectricHeatWind + this.CarbonDioxideLbsMbtuHeatClimate + this.CarbonDioxideLbsMbtuHeatShade + this.CarbonDioxideLbsMbtuHeatWind;
  }
}
