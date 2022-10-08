// Decompiled with JetBrains decompiler
// Type: Eco.Util.Forecast.ModelEquation
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using LocationSpecies.Domain;
using System;
using System.Collections.Generic;

namespace Eco.Util.Forecast
{
  public static class ModelEquation
  {
    public const double FEET_PER_METER = 3.280839895;
    public const double INCHES_PER_CM = 0.393700787;
    public const double POUNDS_PER_KG = 2.204622621845;
    public const double HECTATES_PER_ACRE = 0.40468564224;
    public const double SQ_M_PER_HECTARE = 10000.0;
    private static bool? _initialized;
    private static Dictionary<string, Func<SpeciesDimension, double, double>> _dimensionModel;

    public static double Evaluate(SpeciesDimension dim, double dbh)
    {
      if (!ModelEquation._initialized.HasValue)
        ModelEquation.initialize();
      dbh = ModelEquation.dbhAdj(dbh, dim);
      return ModelEquation._dimensionModel[dim.Model](dim, dbh);
    }

    public static double CalculateHeight(SpeciesHeight dim, double dbh)
    {
      double height = ModelEquation.Evaluate((SpeciesDimension) dim, dbh);
      if (dbh > dim.DbhMax)
        height += (dbh - dim.DbhMax) * 10.0 * dim.HeightGrowth;
      return height;
    }

    public static double calculateBiomass(
      IList<SplinedBiomassEquationNew> eqns,
      Species aSpecies,
      double dbh,
      double height,
      double crownHeight,
      double leafBiomass)
    {
      double num1 = 0.0;
      double? woodDensity;
      for (int index = 0; index < eqns.Count; ++index)
      {
        double num2 = 0.0;
        double x1 = 0.0;
        double x2 = 0.0;
        SplinedBiomassEquationNew eqn = eqns[index];
        double num3;
        if (dbh > eqn.DBHCap)
        {
          num3 = !eqn.Species.IsPalm() ? eqn.BiomassCap + (dbh - eqn.DBHCap) * 40.0 / 0.5 : eqn.BiomassCap + (dbh - eqn.DBHCap) * 40.0 / 0.4;
        }
        else
        {
          double num4;
          if (eqn.DBHUnits == 1)
          {
            num4 = dbh;
          }
          else
          {
            if (eqn.DBHUnits != 2)
              throw new Exception("DBHUnits " + eqn.DBHUnits.ToString() + " is invalid");
            num4 = dbh * 0.393701;
          }
          switch (eqn.XVariable)
          {
            case 1:
              num2 = num4;
              break;
            case 2:
              num2 = num4 * num4 * height;
              break;
            case 3:
              num2 = height - crownHeight + 1.0;
              break;
            case 4:
              num2 = num4 * num4 * (height - crownHeight);
              break;
            case 5:
              x1 = num4;
              x2 = height;
              break;
            case 7:
              num2 = height;
              break;
            case 8:
              num2 = height - crownHeight;
              break;
            default:
              throw new Exception("Xvariable " + eqn.XVariable.ToString() + " is invalid");
          }
          double num5;
          switch (eqn.Form)
          {
            case 1:
              num5 = Math.Exp(eqn.A + eqn.B * Math.Log(num2) + eqn.MeanSquaredError / 2.0);
              break;
            case 2:
              num5 = eqn.A * Math.Pow(num2, eqn.B);
              break;
            case 3:
              num5 = Math.Pow(10.0, eqn.A) * Math.Pow(num2, eqn.B);
              break;
            case 4:
              num5 = eqn.A * num2 * num2 + eqn.B;
              break;
            case 5:
              num5 = eqn.A * num2 * num2;
              break;
            case 6:
              num5 = Math.Pow(10.0, eqn.A + eqn.B * Math.Log10(num2));
              break;
            case 8:
              num5 = Math.Pow(10.0, eqn.A * Math.Log10(num2) - eqn.B);
              break;
            case 9:
              num5 = eqn.A * Math.Pow(num2 * num2, eqn.B);
              break;
            case 10:
              num5 = eqn.A + num2 * eqn.B;
              break;
            case 11:
              num5 = eqn.A + eqn.B * Math.Sqrt(num2) * Math.Log(num2);
              break;
            case 12:
              num5 = Math.Exp(eqn.A + eqn.B * Math.Log(num2 * num2) * eqn.C);
              break;
            case 13:
              num5 = eqn.A * Math.Pow(num2, eqn.B) * eqn.C;
              break;
            case 14:
              num5 = Math.Exp(eqn.A + eqn.B * Math.Log(num2));
              break;
            case 15:
              num5 = eqn.A / 1000.0 * Math.Pow(x1, eqn.B) * Math.Pow(x2, eqn.C);
              break;
            case 16:
              num5 = eqn.A * Math.Pow(num2, 6.0) + eqn.B * Math.Pow(num2, 5.0) + eqn.C * Math.Pow(num2, 4.0) + eqn.D * Math.Pow(num2, 3.0) + eqn.E * Math.Pow(num2, 2.0) + eqn.F * num2 + eqn.G;
              break;
            default:
              throw new Exception("Equation Form " + eqn.Form.ToString() + " is not valid");
          }
          if (eqn.Output == 1)
          {
            if (eqn.Species.IsPalm())
              num5 *= 1.3;
            else
              num5 *= 1.26;
          }
          num3 = num5 * eqn.ConversionFactor;
        }
        woodDensity = eqn.Species.WoodDensity;
        if (woodDensity.HasValue)
        {
          double num6 = num1;
          double num7 = num3;
          woodDensity = eqn.Species.WoodDensity;
          double num8 = woodDensity.Value;
          double num9 = num7 / num8;
          num1 = num6 + num9;
        }
        else
        {
          if (eqn.Species.Rank == SpeciesRank.Species)
          {
            woodDensity = eqn.Species.Parent.WoodDensity;
            if (woodDensity.HasValue)
            {
              double num10 = num1;
              double num11 = num3;
              woodDensity = eqn.Species.Parent.WoodDensity;
              double num12 = woodDensity.Value;
              double num13 = num11 / num12;
              num1 = num10 + num13;
              continue;
            }
          }
          num1 += num3;
        }
      }
      double num14 = 1.0;
      woodDensity = aSpecies.WoodDensity;
      if (woodDensity.HasValue)
      {
        woodDensity = aSpecies.WoodDensity;
        num14 = woodDensity.Value;
      }
      else if (aSpecies.Rank == SpeciesRank.Species)
      {
        woodDensity = aSpecies.Parent.WoodDensity;
        if (woodDensity.HasValue)
        {
          woodDensity = aSpecies.Parent.WoodDensity;
          num14 = woodDensity.Value;
        }
      }
      double biomass = num1 / (double) eqns.Count * num14;
      if (aSpecies.LeafType.Id == 2 || aSpecies.LeafType.Id == 6)
        biomass += leafBiomass;
      return biomass;
    }

    private static void initialize()
    {
      if (ModelEquation._initialized.HasValue)
        return;
      ModelEquation._dimensionModel = new Dictionary<string, Func<SpeciesDimension, double, double>>();
      ModelEquation._dimensionModel.Add("ht = dbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimLinear));
      ModelEquation._dimensionModel.Add("ht = dbh dbh*dbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimQuadratic));
      ModelEquation._dimensionModel.Add("ht = ldbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimLogarithmic));
      ModelEquation._dimensionModel.Add("lht = ldbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimLoglog));
      ModelEquation._dimensionModel.Add("crwht = dbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimLinear));
      ModelEquation._dimensionModel.Add("crwht = dbh dbh*dbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimQuadratic));
      ModelEquation._dimensionModel.Add("crwht = ldbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimLogarithmic));
      ModelEquation._dimensionModel.Add("lcrwht = ldbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimLoglog));
      ModelEquation._dimensionModel.Add("crw = dbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimLinear));
      ModelEquation._dimensionModel.Add("crw = dbh dbh*dbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimQuadratic));
      ModelEquation._dimensionModel.Add("crw = ldbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimLogarithmic));
      ModelEquation._dimensionModel.Add("lcrw = ldbh", new Func<SpeciesDimension, double, double>(ModelEquation.dimLoglog));
      ModelEquation._initialized = new bool?(true);
    }

    private static double dbhAdj(double dbh, SpeciesDimension dim)
    {
      if (dbh > dim.DbhMax)
        return dim.DbhMax;
      return dbh < dim.DbhMin ? dim.DbhMin : dbh;
    }

    private static double dimLinear(SpeciesDimension dim, double dbh) => dim.B0 + dim.B1 * dbh;

    private static double dimQuadratic(SpeciesDimension dim, double dbh) => dim.B0 + dim.B1 * dbh + dim.B2 * dbh * dbh;

    private static double dimLogarithmic(SpeciesDimension dim, double dbh) => dim.B0 + dim.B1 * Math.Log(dbh, Math.E);

    private static double dimLoglog(SpeciesDimension dim, double dbh) => Math.Exp(dim.B0 + dim.B1 * Math.Log(dbh, Math.E));
  }
}
