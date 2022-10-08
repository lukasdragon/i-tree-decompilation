// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.UnitsHelper
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

namespace i_Tree_Eco_v6.Reports
{
  internal class UnitsHelper
  {
    public static double CarbonDioxideToCarbon(double val) => val / (11.0 / 3.0);

    public static double CarbonStorageToBiomass(double carbonStorage) => carbonStorage * 2.0;

    public static double BiomassToAboveGroundBiomass(double biomass) => biomass / 1.26;

    public static double CarbonToCarbonDioxide(double c) => c * (11.0 / 3.0);

    public static double ToPercent(double val) => val * 100.0;

    public static double TonneToKg(double val) => val * 1000.0;

    public static double KgToTonne(double val) => val / 1000.0;

    public static double CubicMetersToCubicFeet(double val) => val / 35.3147;

    public static double MetersToInches(double val) => val * 39.3700787559;

    public static double MetersToCentimeters(double val) => val * 100.0;
  }
}
