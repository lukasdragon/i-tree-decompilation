// Decompiled with JetBrains decompiler
// Type: TreeEnergy.CalculateTreeParameter
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
  public class CalculateTreeParameter
  {
    private ISession locSpecSess;
    private Dictionary<int, SpeciesHeight> dictSpeciesHeight = new Dictionary<int, SpeciesHeight>();

    public CalculateTreeParameter(ISession sess) => this.locSpecSess = sess;

    public double CalculateTreeHeightFeet1(double DBHinches, Species species) => this.CalculateTreeParametersFeet1(CalculateTreeParameter.TreeParameters.Height, "ht", DBHinches, species);

    public double CalculateTreeCrownWidthFeet1(double DBHinches, Species species) => this.CalculateTreeParametersFeet1(CalculateTreeParameter.TreeParameters.CrownWidth, "crw", DBHinches, species);

    public double CalculateTreeCrownHeightFeet1(double DBHinches, Species species) => this.CalculateTreeParametersFeet1(CalculateTreeParameter.TreeParameters.CrownHeight, "crwht", DBHinches, species);

    private double CalculateTreeParametersFeet1(
      CalculateTreeParameter.TreeParameters parameter,
      string EquationParam,
      double DBHinches,
      Species species)
    {
      SpeciesDimension speciesDimension;
      switch (parameter)
      {
        case CalculateTreeParameter.TreeParameters.Height:
          if (this.dictSpeciesHeight.ContainsKey(species.Id))
          {
            speciesDimension = (SpeciesDimension) this.dictSpeciesHeight[species.Id];
            break;
          }
          speciesDimension = this.locSpecSess.CreateCriteria<SpeciesHeight>().Add((ICriterion) Restrictions.Eq("Species.Id", (object) species.Id)).SetCacheable(true).UniqueResult<SpeciesDimension>();
          if (speciesDimension == null)
          {
            Species parent = species.Parent;
            while (speciesDimension == null && parent != null)
            {
              if (this.dictSpeciesHeight.ContainsKey(parent.Id))
              {
                speciesDimension = (SpeciesDimension) this.dictSpeciesHeight[parent.Id];
              }
              else
              {
                speciesDimension = this.locSpecSess.CreateCriteria<SpeciesHeight>().Add((ICriterion) Restrictions.Eq("Species.Id", (object) parent.Id)).SetCacheable(true).UniqueResult<SpeciesDimension>();
                if (speciesDimension != null)
                {
                  if (!this.dictSpeciesHeight.ContainsKey(parent.Id))
                    this.dictSpeciesHeight.Add(parent.Id, (SpeciesHeight) speciesDimension);
                }
                else
                  parent = parent.Parent;
              }
            }
            break;
          }
          this.dictSpeciesHeight.Add(species.Id, (SpeciesHeight) speciesDimension);
          break;
        case CalculateTreeParameter.TreeParameters.CrownWidth:
          speciesDimension = this.locSpecSess.CreateCriteria<SpeciesCrownWidth>().Add((ICriterion) Restrictions.Eq("Species.Id", (object) species.Id)).SetCacheable(true).UniqueResult<SpeciesDimension>();
          break;
        case CalculateTreeParameter.TreeParameters.CrownHeight:
          speciesDimension = this.locSpecSess.CreateCriteria<SpeciesCrownHeight>().Add((ICriterion) Restrictions.Eq("Species.Id", (object) species.Id)).SetCacheable(true).UniqueResult<SpeciesDimension>();
          break;
        default:
          return 0.0;
      }
      double d = DBHinches <= speciesDimension.DbhMax ? (DBHinches >= speciesDimension.DbhMin ? DBHinches : speciesDimension.DbhMin) : speciesDimension.DbhMax;
      if (speciesDimension.Model == EquationParam + " = dbh")
        return speciesDimension.B0 + d * speciesDimension.B1;
      if (speciesDimension.Model == EquationParam + " = dbh dbh*dbh")
        return speciesDimension.B0 + d * speciesDimension.B1 + d * d * speciesDimension.B2;
      if (speciesDimension.Model == EquationParam + " = ldbh")
        return speciesDimension.B0 + Math.Log(d) * speciesDimension.B1;
      return speciesDimension.Model == "l" + EquationParam + " = ldbh" ? Math.Exp(speciesDimension.B0 + Math.Log(d) * speciesDimension.B1) : 0.0;
    }

    public enum TreeParameters
    {
      Height = 1,
      CrownWidth = 2,
      CrownHeight = 3,
    }
  }
}
