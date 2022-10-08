// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.DataScaler
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Core;
using Eco.Util;
using i_Tree_Eco_v6.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace i_Tree_Eco_v6.Reports
{
  public class DataScaler
  {
    private Units _OriginalUnit = Units.None;
    private Units _OriginalPerAreaUnit = Units.None;
    private Units _ScaledMetricUnit = Units.None;
    private Units _ScaledMetricPerAreaUnit = Units.None;
    private Units _ScaledEnglishUnit = Units.None;
    private Units _ScaledEnglishPerAreaUnit = Units.None;
    private string _ScaledMetricUnitDescription = "";
    private string _ScaledMetricUnitAbbr = "";
    private string _ScaledEnglishUnitDescription = "";
    private string _ScaledEnglishUnitAbbr = "";
    private string _ScaledPerAreaMetricUnitDescription = "";
    private string _ScaledPerAreaEnglishUnitDescription = "";
    private string _ScaledMetricPrefix = "";
    private string _ScaledEnglishPrefix = "";
    private double _MetricConvertMultiplier = 1.0;
    private double _EnglishConvertMultiplier = 1.0;
    private double _EnglishPerAreaRatio = 1.0;

    public DataScaler()
    {
    }

    public DataScaler(double value, Units inputUnit, Units inputPerAreaUnit) => this.SetScaler(value, inputUnit, inputPerAreaUnit);

    public DataScaler(List<double> dataList, Units inputUnit, Units inputPerAreaUnit) => this.SetScaler(dataList, inputUnit, inputPerAreaUnit);

    public DataScaler(
      List<double> dataList,
      Units inputUnit,
      Units inputPerAreaUnit,
      string currencyName,
      string currencySymbol)
    {
      this.SetScaler(dataList, inputUnit, inputPerAreaUnit, currencyName, currencySymbol);
    }

    private ReportDataRangeEnum GetDataRange(double value)
    {
      value = Math.Abs(value);
      ReportDataRangeEnum dataRange = ReportDataRangeEnum.Singles;
      if (value == 0.0)
        return dataRange;
      if (value > 1.0 && value <= 1000.0)
        dataRange = ReportDataRangeEnum.Singles;
      else if (value > 1000.0 && value <= 1000000.0)
        dataRange = ReportDataRangeEnum.Thousands;
      else if (value > 1000000.0 && value <= 1000000000.0)
        dataRange = ReportDataRangeEnum.Millions;
      else if (value > 1000000000.0 && value <= 1000000000000.0)
        dataRange = ReportDataRangeEnum.Billions;
      else if (value > 1000000000000.0)
        dataRange = ReportDataRangeEnum.Trillions;
      else if (value > 0.001)
        dataRange = ReportDataRangeEnum.Thousandth;
      else if (value > 1E-06)
        dataRange = ReportDataRangeEnum.Millionth;
      return dataRange;
    }

    private void SetMultipliers(double multiplierM, double englishRatio = 1.0)
    {
      this._MetricConvertMultiplier = 1.0 / multiplierM;
      this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * englishRatio * this._MetricConvertMultiplier;
    }

    private void SetMultipliersAdjusted(double multiplierM, double englishRatio = 1.0)
    {
      this._MetricConvertMultiplier = 1.0 / multiplierM;
      this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * englishRatio;
    }

    private void AdjustEnglishDataScaleDown(double value, ReportDataRangeEnum aDataRange)
    {
      if (Math.Abs(value * this._EnglishConvertMultiplier) <= 1000.0)
        return;
      this._EnglishConvertMultiplier /= 1000.0;
      this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(aDataRange + 3).ToLower();
    }

    private void AdjustEnglishDataScaleUp(double value, ReportDataRangeEnum aDataRange, double min = 1.0)
    {
      if (Math.Abs(value * this._EnglishConvertMultiplier) >= min)
        return;
      this._EnglishConvertMultiplier *= 1000.0;
      this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(aDataRange - 3).ToLower();
    }

    public void SetScaler(List<double> dataList, Units inputUnit, Units inputPerAreaUnit)
    {
      double num = 0.0;
      if (dataList.Count != 0)
        num = Math.Abs(Enumerable.Average(dataList));
      this.SetScaler(num, inputUnit, inputPerAreaUnit);
    }

    public void SetScaler(double value, Units inputUnit, Units inputPerAreaUnit)
    {
      if (inputPerAreaUnit != Units.None && inputPerAreaUnit != Units.Hectare)
        throw new Exception(i_Tree_Eco_v6.Resources.Strings.DataScalerUnitErrorMessage2);
      this._OriginalUnit = inputUnit;
      this._OriginalPerAreaUnit = inputPerAreaUnit;
      this._ScaledMetricPerAreaUnit = inputPerAreaUnit;
      this._ScaledEnglishPerAreaUnit = inputPerAreaUnit;
      this._ScaledPerAreaMetricUnitDescription = this._ScaledPerAreaEnglishUnitDescription = string.Empty;
      if (inputPerAreaUnit == Units.Hectare)
      {
        this._EnglishPerAreaRatio = 0.40468626697153032;
        this._ScaledEnglishPerAreaUnit = Units.Acre;
        this._ScaledPerAreaMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitHectare;
        this._ScaledPerAreaEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitAcre;
      }
      ReportDataRangeEnum dataRange = this.GetDataRange(value);
      switch (inputUnit)
      {
        case Units.Count:
        case Units.Monetaryunit:
          this._ScaledMetricUnit = this._ScaledEnglishUnit = inputUnit;
          this._ScaledMetricUnitDescription = this._ScaledMetricUnitAbbr = this._ScaledEnglishUnitDescription = this._ScaledEnglishUnitAbbr = string.Empty;
          this.SetMultipliers(Math.Pow(10.0, (double) dataRange));
          this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange).ToLower();
          break;
        case Units.Centimeters:
          switch (dataRange)
          {
            case ReportDataRangeEnum.Thousands:
              this._ScaledMetricUnit = Units.Meters;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitMeter;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitMetersAbbr;
              this._ScaledEnglishUnit = Units.Feet;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitFeet;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitFeetAbbr;
              this.SetMultipliersAdjusted(100.0, 25.0 / 762.0);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              return;
            case ReportDataRangeEnum.Millions:
            case ReportDataRangeEnum.Billions:
            case ReportDataRangeEnum.Trillions:
              this._ScaledMetricUnit = Units.Kilometer;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitKilometer;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitKilometersAbbr;
              this._ScaledEnglishUnit = Units.Miles;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitMile;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitMilesAbbr;
              this.SetMultipliersAdjusted(Math.Pow(10.0, (double) (dataRange - 6)) * 100000.0, 1.0 / Math.Pow(10.0, (double) (dataRange - 6)) * 6.2137119223733393E-06);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange - 6).ToLower();
              return;
            default:
              this._ScaledMetricUnit = Units.Centimeters;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitCentimeter;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr;
              this._ScaledEnglishUnit = Units.Inches;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitInch;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr;
              this.SetMultipliersAdjusted(1.0, 50.0 / (double) sbyte.MaxValue);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              return;
          }
        case Units.Meters:
          switch (dataRange)
          {
            case ReportDataRangeEnum.Millionth:
            case ReportDataRangeEnum.Thousandth:
              this._ScaledMetricUnit = Units.Centimeters;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitCentimeter;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr;
              this._ScaledEnglishUnit = Units.Inches;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitInch;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr;
              this.SetMultipliersAdjusted(0.0 / Math.Pow(10.0, (double) (dataRange + 3)), 1.0 / Math.Pow(10.0, (double) (dataRange + 3)) * 39.3701);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange + 3).ToLower();
              return;
            case ReportDataRangeEnum.Thousands:
            case ReportDataRangeEnum.Millions:
              this._ScaledMetricUnit = Units.Kilometer;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitKilometer;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitKilometersAbbr;
              this._ScaledEnglishUnit = Units.Miles;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitMile;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitMilesAbbr;
              this.SetMultipliersAdjusted(Math.Pow(10.0, (double) dataRange), 0.00062137273664980683);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange - 3).ToLower();
              return;
            default:
              this._ScaledMetricUnit = Units.Meters;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitMeter;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitMetersAbbr;
              this._ScaledEnglishUnit = Units.Feet;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitFeet;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitFeetAbbr;
              this.SetMultipliersAdjusted(1.0, 3.28084);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              return;
          }
        case Units.Kilometer:
          if (dataRange == ReportDataRangeEnum.Thousandth)
          {
            this._ScaledMetricUnit = Units.Meters;
            this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitMeter;
            this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitMetersAbbr;
            this._ScaledEnglishUnit = Units.Feet;
            this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitFeet;
            this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitFeetAbbr;
            this.SetMultipliersAdjusted(0.001, 3.28084);
            this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange + 3).ToLower();
            this.AdjustEnglishDataScaleDown(value, dataRange + 3);
            break;
          }
          if (dataRange == ReportDataRangeEnum.Millionth)
          {
            this._ScaledMetricUnit = Units.Centimeters;
            this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitCentimeter;
            this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitCentimetersAbbr;
            this._ScaledEnglishUnit = Units.Inches;
            this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitInch;
            this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitInchesAbbr;
            this.SetMultipliersAdjusted(1E-05, 39370.08);
            this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange + 6).ToLower();
            break;
          }
          this._ScaledMetricUnit = Units.Kilometer;
          this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitKilometer;
          this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitKilometersAbbr;
          this._ScaledEnglishUnit = Units.Miles;
          this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitMile;
          this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitMilesAbbr;
          this.SetMultipliers(Math.Pow(10.0, (double) dataRange), 0.62137273664980675);
          this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange).ToLower();
          break;
        case Units.Squaremeter:
          switch (dataRange)
          {
            case ReportDataRangeEnum.Thousands:
              this._ScaledMetricUnit = Units.Hectare;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitHectare;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr;
              this._ScaledEnglishUnit = Units.Acre;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitAcre;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr;
              this.SetMultipliersAdjusted(10000.0, 0.00024710516301527604);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange - 3).ToLower();
              this.AdjustEnglishDataScaleDown(value, dataRange - 3);
              return;
            case ReportDataRangeEnum.Millions:
              this._ScaledMetricUnit = Units.Squarekilometer;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometer;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometersAbbr;
              this._ScaledEnglishUnit = Units.Squaremiles;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareMile;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareMilesAbbr;
              this.SetMultipliersAdjusted(1000000.0, 3.8610407785167015E-07);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              if (Math.Abs(value * this._EnglishConvertMultiplier) >= 1.0)
                return;
              this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 1.0 / 4046.86;
              this._ScaledEnglishUnit = Units.Acre;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitAcre;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr;
              this._ScaledEnglishPrefix = string.Empty;
              return;
            case ReportDataRangeEnum.Billions:
            case ReportDataRangeEnum.Trillions:
              this._ScaledMetricUnit = Units.Squarekilometer;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometer;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometersAbbr;
              this._ScaledEnglishUnit = Units.Squaremiles;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareMile;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareMilesAbbr;
              this.SetMultipliersAdjusted(Math.Pow(10.0, (double) dataRange), 1.0 / Math.Pow(10.0, (double) (dataRange - 6)) * 1.0 / 1609.34 / 1609.34);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange - 6).ToLower();
              this.AdjustEnglishDataScaleUp(value, dataRange - 6);
              return;
            default:
              this._ScaledMetricUnit = Units.Squaremeter;
              this._ScaledEnglishUnit = Units.Squarefeet;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareMeter;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareMetersAbbr;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareFeet;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareFeetAbbr;
              this.SetMultipliersAdjusted(1.0, 10.7639);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              return;
          }
        case Units.Squarekilometer:
          switch (dataRange)
          {
            case ReportDataRangeEnum.Millionth:
              this._ScaledMetricUnit = Units.Squaremeter;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareMeter;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareMetersAbbr;
              this._ScaledEnglishUnit = Units.Squarefeet;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareFeet;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareFeetAbbr;
              this.SetMultipliersAdjusted(Math.Pow(10.0, (double) (dataRange + 6)), 1.0 / Math.Pow(10.0, (double) (dataRange + 6)) * 10.763910416);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange + 6).ToLower();
              this.AdjustEnglishDataScaleDown(value, dataRange + 6);
              return;
            case ReportDataRangeEnum.Thousandth:
              this._ScaledMetricUnit = Units.Hectare;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitHectare;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr;
              this._ScaledEnglishUnit = Units.Acre;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitAcre;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr;
              this._MetricConvertMultiplier = 100.0;
              this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 247.1053814;
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              return;
            case ReportDataRangeEnum.Thousands:
            case ReportDataRangeEnum.Millions:
            case ReportDataRangeEnum.Billions:
            case ReportDataRangeEnum.Trillions:
              this._ScaledMetricUnit = Units.Squarekilometer;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometer;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometersAbbr;
              this._ScaledEnglishUnit = Units.Squaremiles;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareMile;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareMilesAbbr;
              this.SetMultipliers(Math.Pow(10.0, (double) dataRange), 0.386102158);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange).ToLower();
              this.AdjustEnglishDataScaleUp(value, dataRange);
              return;
            default:
              this._ScaledMetricUnit = Units.Squarekilometer;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometer;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometersAbbr;
              this._ScaledEnglishUnit = Units.Squaremiles;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareMile;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareMilesAbbr;
              this.SetMultipliersAdjusted(1.0, 0.386102158);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              if (Math.Abs(value * this._MetricConvertMultiplier) < 10.0)
              {
                this._ScaledMetricUnit = Units.Hectare;
                this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitHectare;
                this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr;
                this._MetricConvertMultiplier = 100.0;
                this._ScaledMetricPrefix = string.Empty;
              }
              if (Math.Abs(value * this._EnglishConvertMultiplier) >= 2.0)
                return;
              this._ScaledEnglishUnit = Units.Acre;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitAcre;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr;
              this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 247.1053814;
              this._ScaledEnglishPrefix = string.Empty;
              return;
          }
        case Units.Grams:
          switch (dataRange)
          {
            case ReportDataRangeEnum.Thousands:
              this._ScaledMetricUnit = Units.Kilograms;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitKilogram;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitKilogramsAbbr;
              this._ScaledEnglishUnit = Units.Pounds;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitPound;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitPoundsAbbr;
              this.SetMultipliersAdjusted(Math.Pow(10.0, (double) dataRange), 0.0022046226);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange - 3).ToLower();
              return;
            case ReportDataRangeEnum.Millions:
            case ReportDataRangeEnum.Billions:
            case ReportDataRangeEnum.Trillions:
              this._ScaledMetricUnit = Units.MetricTons;
              this._ScaledMetricUnitDescription = this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitTonne;
              this._ScaledEnglishUnit = Units.Ton;
              this._ScaledEnglishUnitDescription = this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitTon;
              this.SetMultipliers(Math.Pow(10.0, (double) dataRange), 1.1023113);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange - 6).ToLower();
              return;
            default:
              this._ScaledMetricUnit = Units.Grams;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitGram;
              this._ScaledMetricUnitAbbr = "g";
              this._ScaledEnglishUnit = Units.Ounces;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitOunce;
              this._ScaledEnglishUnitAbbr = "oz";
              this._MetricConvertMultiplier = 1.0;
              this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 0.0352739609;
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              if (Math.Abs(value * this._EnglishConvertMultiplier) >= 0.1)
                return;
              this._EnglishConvertMultiplier *= 1000.0;
              this._ScaledEnglishPrefix = i_Tree_Eco_v6.Resources.Strings.DS_Thousandth;
              return;
          }
        case Units.Kilograms:
          switch (dataRange)
          {
            case ReportDataRangeEnum.Millionth:
            case ReportDataRangeEnum.Thousandth:
              this._ScaledMetricUnit = Units.Grams;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitGram;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitGramsAbbr;
              this._ScaledEnglishUnit = Units.Ounces;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitOunce;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitOuncesAbbr;
              this.SetMultipliersAdjusted(1.0 / (1000.0 * Math.Pow(10.0, (double) (dataRange + 3))), Math.Pow(10.0, (double) (dataRange + 3)) * 35.27396194);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange + 3).ToLower();
              this.AdjustEnglishDataScaleUp(value, dataRange + 3, 0.1);
              return;
            case ReportDataRangeEnum.Thousands:
            case ReportDataRangeEnum.Millions:
            case ReportDataRangeEnum.Billions:
            case ReportDataRangeEnum.Trillions:
              this._ScaledMetricUnit = Units.MetricTons;
              this._ScaledMetricUnitDescription = this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitTonne;
              this._ScaledEnglishUnit = Units.Ton;
              this._ScaledEnglishUnitDescription = this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitTon;
              this.SetMultipliersAdjusted(1000.0 * Math.Pow(10.0, (double) (dataRange - 3)), 1.0 / Math.Pow(10.0, (double) (dataRange - 3)) * 0.0011023113);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange - 3).ToLower();
              return;
            default:
              this._ScaledMetricUnit = Units.Kilograms;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitKilogram;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitKilogramsAbbr;
              this._ScaledEnglishUnit = Units.Pounds;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitPound;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitPoundsAbbr;
              this._MetricConvertMultiplier = 1.0;
              this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 2.204622621;
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              return;
          }
        case Units.MetricTons:
          switch (dataRange)
          {
            case ReportDataRangeEnum.Millionth:
              this._ScaledMetricUnit = Units.Grams;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitGram;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitGramsAbbr;
              this._ScaledEnglishUnit = Units.Ounces;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitOunce;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitOuncesAbbr;
              this._MetricConvertMultiplier = 1000000.0;
              this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 35273.96194;
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange + 6).ToLower();
              this.AdjustEnglishDataScaleUp(value, dataRange + 6, 0.1);
              return;
            case ReportDataRangeEnum.Thousandth:
              this._ScaledMetricUnit = Units.Kilograms;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitKilogram;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitKilogramsAbbr;
              this._ScaledEnglishUnit = Units.Pounds;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitPound;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitPoundsAbbr;
              this._MetricConvertMultiplier = 1000.0;
              this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 2204.6226;
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange + 3).ToLower();
              return;
            case ReportDataRangeEnum.Thousands:
            case ReportDataRangeEnum.Millions:
            case ReportDataRangeEnum.Billions:
            case ReportDataRangeEnum.Trillions:
              this._ScaledMetricUnit = Units.MetricTons;
              this._ScaledMetricUnitDescription = this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitTonne;
              this._ScaledEnglishUnit = Units.Ton;
              this._ScaledEnglishUnitDescription = this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitTon;
              this.SetMultipliers(Math.Pow(10.0, (double) dataRange), 1.10231131);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange).ToLower();
              return;
            default:
              this._ScaledMetricUnit = Units.MetricTons;
              this._ScaledMetricUnitDescription = this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitTonne;
              this._ScaledEnglishUnit = Units.Ton;
              this._ScaledEnglishUnitDescription = this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitTon;
              this._MetricConvertMultiplier = 1.0;
              this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 1.10231131;
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              return;
          }
        case Units.Hectare:
          switch (dataRange)
          {
            case ReportDataRangeEnum.Thousandth:
              this._ScaledMetricUnit = Units.Squaremeter;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareMeter;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareMetersAbbr;
              this._ScaledEnglishUnit = Units.Squarefeet;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareFeet;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareFeetAbbr;
              this.SetMultipliersAdjusted(0.0001, 107639.104167);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              if (Math.Abs(value * this._EnglishConvertMultiplier) > 43560.0)
              {
                this._ScaledEnglishUnit = Units.Acre;
                this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitAcre;
                this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr;
                this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 2.47105;
                this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange + 3).ToLower();
                return;
              }
              this.AdjustEnglishDataScaleDown(value, dataRange + 3);
              return;
            case ReportDataRangeEnum.Thousands:
            case ReportDataRangeEnum.Millions:
            case ReportDataRangeEnum.Billions:
            case ReportDataRangeEnum.Trillions:
              this._ScaledMetricUnit = Units.Squarekilometer;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometer;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareKilometersAbbr;
              this._ScaledEnglishUnit = Units.Squaremiles;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitSquareMile;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitSquareMilesAbbr;
              this.SetMultipliersAdjusted(100.0 * Math.Pow(10.0, (double) (dataRange - 3)), 1.0 / Math.Pow(10.0, (double) (dataRange - 3)) * 0.0038610215854);
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange - 3).ToLower();
              return;
            default:
              this._ScaledMetricUnit = Units.Hectare;
              this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitHectare;
              this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitHectaresAbbr;
              this._ScaledEnglishUnit = Units.Acre;
              this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitAcre;
              this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitAcresAbbr;
              this._MetricConvertMultiplier = 1.0;
              this._EnglishConvertMultiplier = this._EnglishPerAreaRatio * 2.47105;
              this._ScaledMetricPrefix = this._ScaledEnglishPrefix = string.Empty;
              return;
          }
        case Units.CubicMeter:
          this._ScaledMetricUnit = this._ScaledEnglishUnit = inputUnit;
          this._ScaledMetricUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitCubicMeter;
          this._ScaledMetricUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitCubicMetersAbbr;
          this._ScaledEnglishUnitDescription = i_Tree_Eco_v6.Resources.Strings.UnitCubicFeet;
          this._ScaledEnglishUnitAbbr = i_Tree_Eco_v6.Resources.Strings.UnitCubicFeetAbbr;
          this.SetMultipliers(Math.Pow(10.0, (double) dataRange), 35.3146667);
          this._ScaledMetricPrefix = this._ScaledEnglishPrefix = EnumHelper.GetDescription<ReportDataRangeEnum>(dataRange).ToLower();
          this.AdjustEnglishDataScaleDown(value, dataRange);
          break;
        default:
          throw new Exception(string.Format(i_Tree_Eco_v6.Resources.Strings.DataScalerUnitErrorMessage1, (object) inputUnit.ToString()));
      }
    }

    public void SetScaler(
      double value,
      Units inputUnit,
      Units inputPerAreaUnit,
      string currencyName,
      string currencySymbol)
    {
      if (inputUnit != Units.Monetaryunit)
        throw new Exception(i_Tree_Eco_v6.Resources.Strings.DataScalerUnitErrorMessage3);
      this.SetScaler(value, inputUnit, inputPerAreaUnit);
      this._ScaledMetricUnitDescription = currencyName;
      this._ScaledMetricUnitAbbr = currencySymbol;
      this._ScaledEnglishUnitDescription = currencyName;
      this._ScaledEnglishUnitAbbr = currencySymbol;
    }

    public void SetScaler(
      List<double> dataList,
      Units inputUnit,
      Units inputPerAreaUnit,
      string currencyName,
      string currencySymbol)
    {
      double num = 0.0;
      if (dataList.Count != 0)
        num = Math.Abs(Enumerable.Average(dataList));
      this.SetScaler(num, inputUnit, inputPerAreaUnit, currencyName, currencySymbol);
    }

    public string GetScaledPrefix(bool English)
    {
      string s = English ? this._ScaledEnglishPrefix : this._ScaledMetricPrefix;
      if (!string.IsNullOrEmpty(s))
        s = ReportUtil.PluralizeLast(s);
      return s;
    }

    public string GetPrefixPlusUnitDescription(bool English)
    {
      string str = English ? this._ScaledEnglishPrefix : this._ScaledMetricPrefix;
      string plusUnitDescription = English ? this._ScaledEnglishUnitDescription : this._ScaledMetricUnitDescription;
      if (!string.IsNullOrEmpty(str))
        plusUnitDescription = str + " " + plusUnitDescription;
      return plusUnitDescription;
    }

    public string GetPrefixPlusUnitAbbreviation(bool English)
    {
      string str = English ? this._ScaledEnglishPrefix : this._ScaledMetricPrefix;
      string unitAbbreviation = English ? this._ScaledEnglishUnitAbbr : this._ScaledMetricUnitAbbr;
      if (!string.IsNullOrEmpty(str))
        unitAbbreviation = this._OriginalUnit == Units.Monetaryunit ? unitAbbreviation + " " + str : str + " " + unitAbbreviation;
      return unitAbbreviation;
    }

    public double GetScaledValue(double valueWithOriginalUnit, bool English) => !English ? valueWithOriginalUnit * this._MetricConvertMultiplier : valueWithOriginalUnit * this._EnglishConvertMultiplier;

    public string GetPhraseOfScaledValueWithUnit(double valueWithOriginalUnit, bool English)
    {
      string s = ReportUtil.RoundToSignificantFigures(this.GetScaledValue(valueWithOriginalUnit, English), 4).ToString();
      string str1 = English ? this._ScaledEnglishPrefix : this._ScaledMetricPrefix;
      if (!string.IsNullOrEmpty(str1))
        s = s + " " + str1;
      if (this._OriginalUnit == Units.Monetaryunit)
      {
        s = this._ScaledMetricUnitAbbr + s;
      }
      else
      {
        string str2 = English ? this._ScaledEnglishUnitDescription : this._ScaledMetricUnitDescription;
        if (!string.IsNullOrEmpty(str2))
          s = s + " " + str2;
      }
      string scaledValueWithUnit = ReportUtil.PluralizeLast(s);
      string str3 = English ? this._ScaledPerAreaEnglishUnitDescription : this._ScaledPerAreaMetricUnitDescription;
      if (!string.IsNullOrEmpty(str3))
        scaledValueWithUnit = scaledValueWithUnit + "/" + str3;
      return scaledValueWithUnit;
    }
  }
}
