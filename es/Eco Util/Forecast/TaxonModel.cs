// Decompiled with JetBrains decompiler
// Type: Eco.Util.Forecast.TaxonModel
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using DaveyTree.NHibernate;
using LocationSpecies.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eco.Util.Forecast
{
  public class TaxonModel
  {
    private Dictionary<LocationSpecies.Domain.GrowthRate, double> GROWTH = new Dictionary<LocationSpecies.Domain.GrowthRate, double>()
    {
      {
        LocationSpecies.Domain.GrowthRate.Slow,
        0.23
      },
      {
        LocationSpecies.Domain.GrowthRate.Moderate,
        0.33
      },
      {
        LocationSpecies.Domain.GrowthRate.Fast,
        0.43
      }
    };
    private ISession _ls;
    private Species _root;
    private Dictionary<SpeciesRank, Species> _taxonDict;
    private IList<SpeciesHeight> _heights;
    private IList<SpeciesCrownHeight> _crHeights;
    private IList<SpeciesCrownWidth> _crWdths;
    private Dictionary<SpeciesGroup, int> _sppGroups;
    private double? _matHeight;
    private double? _groRate;
    private double? _shFact;
    private double? _kFact;
    private double? _leafBiomassToAreaConversionFactor;
    private IList<SplinedBiomassEquationNew> _biomassEqns;

    public TaxonModel(Species taxon, ISession ls)
    {
      if (taxon == null)
        throw new NullReferenceException(nameof (taxon));
      this.construct(taxon, ls);
    }

    public TaxonModel(string code, ISession ls) => this.construct(ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Code == code)).Cacheable().SingleOrDefault() ?? throw new ArgumentOutOfRangeException(nameof (code), (object) code, "No record in the _Species table matches the provided code."), ls);

    public Species Root
    {
      get => this._root;
      set
      {
        this.nullEverything();
        this.construct(value, this._ls);
      }
    }

    public Species this[SpeciesRank rank] => this._taxonDict[rank];

    public SpeciesRank RootRank => this._root.Rank;

    public Species Species => this[SpeciesRank.Species];

    public Species Subgenus => this[SpeciesRank.Subgenus];

    public Species Genus => this[SpeciesRank.Genus];

    public Species Family => this[SpeciesRank.Family];

    public Species Order => this[SpeciesRank.Order];

    public Species Subclass => this[SpeciesRank.Subclass];

    public Species Class => this[SpeciesRank.Class];

    public double Height(double dbh)
    {
      if (this._heights == null)
        this._heights = this.TreeDimension<SpeciesHeight>();
      return this._heights.Average<SpeciesHeight>((Func<SpeciesHeight, double>) (h => ModelEquation.CalculateHeight(h, dbh)));
    }

    public double CrownHeight(double dbh)
    {
      if (this._crHeights == null)
        this._crHeights = this.TreeDimension<SpeciesCrownHeight>();
      return this._crHeights.Average<SpeciesCrownHeight>((Func<SpeciesCrownHeight, double>) (ch => ModelEquation.Evaluate((SpeciesDimension) ch, dbh)));
    }

    public double CrownWidth(double dbh)
    {
      if (this._crWdths == null)
        this._crWdths = this.TreeDimension<SpeciesCrownWidth>();
      return this._crWdths.Average<SpeciesCrownWidth>((Func<SpeciesCrownWidth, double>) (cw => ModelEquation.Evaluate((SpeciesDimension) cw, dbh)));
    }

    public double CrownWidthBySpeciesGroup(double DBHinches, double TreeHeightFeet)
    {
      if (this._sppGroups == null)
        this._sppGroups = this.getSpeciesGroups();
      double num1 = 0.0;
      int num2 = 0;
      foreach (KeyValuePair<SpeciesGroup, int> sppGroup in this._sppGroups)
      {
        num1 += sppGroup.Key.CrownWidthEquation.Calculate(DBHinches / 0.393700787, TreeHeightFeet / (1250.0 / 381.0)) * (double) sppGroup.Value;
        num2 += sppGroup.Value;
      }
      return 1250.0 / 381.0 * num1 / (double) num2;
    }

    public double GrowthRate
    {
      get
      {
        this._groRate = !this._groRate.HasValue ? new double?(this.getGrowthRate()) : this._groRate;
        return this._groRate.Value;
      }
    }

    public double MatureHeight
    {
      get
      {
        this._matHeight = !this._matHeight.HasValue ? new double?(this.getMatureHeight()) : this._matHeight;
        return this._matHeight.Value;
      }
    }

    public double MaximumDBH
    {
      get
      {
        if (this._root.Habit != null)
        {
          double? maximumDbh = this._root.Habit.MaximumDBH;
          if (maximumDbh.HasValue)
          {
            maximumDbh = this._root.Habit.MaximumDBH;
            return maximumDbh.Value;
          }
        }
        return 30.0;
      }
    }

    public double ShadeFactor
    {
      get
      {
        this._shFact = !this._shFact.HasValue ? new double?(this.getShadeFactor()) : this._shFact;
        return this._shFact.Value;
      }
    }

    public double KFactor
    {
      get
      {
        this._kFact = !this._kFact.HasValue ? new double?(this.getKFactor()) : this._kFact;
        return this._kFact.Value;
      }
    }

    public double LeafBiomassToAreaConversionFactor
    {
      get
      {
        this._leafBiomassToAreaConversionFactor = !this._leafBiomassToAreaConversionFactor.HasValue ? new double?(this.getLeafBiomassToAreaConversionFactor()) : this._leafBiomassToAreaConversionFactor;
        return this._leafBiomassToAreaConversionFactor.Value;
      }
    }

    public double Biomass(double dbh, double height, double crownHeight, double leafBiomass)
    {
      if (this._biomassEqns == null)
        this._biomassEqns = this.getBiomassEquations();
      return ModelEquation.calculateBiomass(this._biomassEqns, this._root, dbh, height, crownHeight, leafBiomass);
    }

    public double TropicalBiomass(TropicalClimate tropicalClimate, double dbh, double height)
    {
      double num1 = 1.0;
      if (this._root.WoodDensity.HasValue)
        num1 = this._root.WoodDensity.Value;
      else if (this._root.Rank == SpeciesRank.Species)
      {
        double? woodDensity = this._root.Parent.WoodDensity;
        if (woodDensity.HasValue)
        {
          woodDensity = this._root.Parent.WoodDensity;
          num1 = woodDensity.Value;
        }
      }
      double num2;
      switch (tropicalClimate)
      {
        case TropicalClimate.Dry:
          num2 = 0.112 * Math.Pow(num1 * dbh * dbh * height, 0.916);
          break;
        case TropicalClimate.Moist:
          num2 = 0.0509 * (num1 * dbh * dbh * height);
          break;
        default:
          num2 = 0.0776 * Math.Pow(num1 * dbh * dbh * height, 0.94);
          break;
      }
      return num2;
    }

    private void construct(Species taxon, ISession ls)
    {
      this._ls = ls;
      this._root = taxon;
      this._taxonDict = this.taxonomyOf(this._root);
    }

    private void nullEverything()
    {
      this._heights = (IList<SpeciesHeight>) null;
      this._crHeights = (IList<SpeciesCrownHeight>) null;
      this._crWdths = (IList<SpeciesCrownWidth>) null;
      this._sppGroups = (Dictionary<SpeciesGroup, int>) null;
      this._matHeight = new double?();
      this._groRate = new double?();
      this._shFact = new double?();
      this._kFact = new double?();
      this._biomassEqns = (IList<SplinedBiomassEquationNew>) null;
    }

    private Dictionary<SpeciesRank, Species> taxonomyOf(Species taxon)
    {
      Dictionary<SpeciesRank, Species> dictionary = new Dictionary<SpeciesRank, Species>();
      for (SpeciesRank key = SpeciesRank.Species; key >= SpeciesRank.Class; --key)
        dictionary.Add(key, (Species) null);
      for (; taxon.Rank > SpeciesRank.Class; taxon = taxon.Parent)
        dictionary[taxon.Rank] = taxon;
      dictionary[SpeciesRank.Class] = taxon;
      return dictionary;
    }

    private IList<D> TreeDimension<D>() where D : SpeciesDimension
    {
      IList<D> dList = (IList<D>) new List<D>();
      D d1 = default (D);
      for (SpeciesRank rank = SpeciesRank.Species; rank >= SpeciesRank.Order; --rank)
      {
        if (this[rank] != null)
        {
          d1 = this._ls.QueryOver<D>().Where((System.Linq.Expressions.Expression<Func<D, bool>>) (d => d.Species.Id == this[rank].Id)).Cacheable().SingleOrDefault();
          if ((object) d1 != null)
            break;
        }
      }
      if ((object) d1 != null)
      {
        dList.Add(d1);
        return dList;
      }
      if (this.Subclass != null)
      {
        IList<D> list = (IList<D>) this._ls.QueryOver<D>().Inner.JoinQueryOver<Species>((System.Linq.Expressions.Expression<Func<D, Species>>) (d => d.Species)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => (int) s.Rank == 3)).Cacheable().List().Where<D>((Func<D, bool>) (d => TaxonModel.ParentOfChild(this.Subclass, d.Species))).ToList<D>();
        if (list.Count > 0)
          return list;
      }
      IList<D> list1 = (IList<D>) this._ls.QueryOver<D>().Inner.JoinQueryOver<Species>((System.Linq.Expressions.Expression<Func<D, Species>>) (d => d.Species)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => (int) s.Rank == 3)).Cacheable().List().Where<D>((Func<D, bool>) (d => TaxonModel.ParentOfChild(this.Class, d.Species))).ToList<D>();
      if (list1.Count > 0)
        return list1;
      D d2 = this._ls.QueryOver<D>().Inner.JoinQueryOver<Species>((System.Linq.Expressions.Expression<Func<D, Species>>) (d => d.Species)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Id == this.Class.Id)).Cacheable().SingleOrDefault();
      list1.Add(d2);
      return list1;
    }

    private double getMatureHeight()
    {
      SpeciesHabit habit = this.Root.Habit;
      if (habit != null && habit.MatureHeight.HasValue)
        return (double) habit.MatureHeight.Value;
      double? nullable = new double?();
      if (this.Genus != null)
      {
        try
        {
          SpeciesHabit hAlias = (SpeciesHabit) null;
          Species sAlias = (Species) null;
          Species pAlias = (Species) null;
          nullable = this._ls.QueryOver<SpeciesHabit>((System.Linq.Expressions.Expression<Func<SpeciesHabit>>) (() => hAlias)).Where((System.Linq.Expressions.Expression<Func<SpeciesHabit, bool>>) (h => h.MatureHeight != (int?) 0)).JoinAlias((System.Linq.Expressions.Expression<Func<object>>) (() => hAlias.Species), (System.Linq.Expressions.Expression<Func<object>>) (() => sAlias)).JoinAlias((System.Linq.Expressions.Expression<Func<object>>) (() => sAlias.Parent), (System.Linq.Expressions.Expression<Func<object>>) (() => pAlias)).Where((System.Linq.Expressions.Expression<Func<bool>>) (() => (int) sAlias.Rank == 7 && (sAlias.Parent.Id == this.Genus.Id || pAlias.Parent.Id == this.Genus.Id))).Select((IProjection) Projections.Avg((System.Linq.Expressions.Expression<Func<object>>) (() => (object) hAlias.MatureHeight))).Cacheable().SingleOrDefault<double?>();
        }
        catch (InvalidOperationException ex)
        {
        }
      }
      return !nullable.HasValue ? 50.0 : nullable.Value;
    }

    private double getGrowthRate() => this.GROWTH[(LocationSpecies.Domain.GrowthRate) ((int) this._ls.QueryOver<SpeciesHabit>().JoinQueryOver<Species>((System.Linq.Expressions.Expression<Func<SpeciesHabit, Species>>) (h => h.Species)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Id == this[this.RootRank].Id)).Cacheable().SingleOrDefault()?.GrowthRate ?? 2)];

    public Dictionary<SpeciesGroup, int> getSpeciesGroups()
    {
      Dictionary<SpeciesGroup, int> speciesGroups = (Dictionary<SpeciesGroup, int>) null;
      if (this.Root.SpeciesGroup != null)
        return new Dictionary<SpeciesGroup, int>()
        {
          {
            this.Root.SpeciesGroup,
            1
          }
        };
      for (SpeciesRank rank = SpeciesRank.Subgenus; rank >= SpeciesRank.Class; --rank)
      {
        try
        {
          if (this[rank] != null)
          {
            IList<Species> list = (IList<Species>) this._ls.QueryOver<Species>().WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.SpeciesGroup)).IsNotNull.Cacheable().List().Where<Species>((Func<Species, bool>) (sp =>
            {
              if (TaxonModel.ParentOfChild(this[rank], sp))
                return true;
              return rank == SpeciesRank.Genus && this.Genus != null && sp.Id == this.Genus.Id;
            })).ToList<Species>();
            if (list != null)
            {
              if (list.Count > 0)
              {
                speciesGroups = new Dictionary<SpeciesGroup, int>();
                foreach (Species species in (IEnumerable<Species>) list)
                {
                  if (speciesGroups.ContainsKey(species.SpeciesGroup))
                    speciesGroups[species.SpeciesGroup]++;
                  else
                    speciesGroups.Add(species.SpeciesGroup, 1);
                }
              }
            }
          }
          else
            continue;
        }
        catch (InvalidOperationException ex)
        {
        }
        if (speciesGroups != null && speciesGroups.Count > 0)
          break;
      }
      if (speciesGroups == null || speciesGroups.Count == 0)
      {
        IList<(SpeciesGroup, int)> tupleList = this._ls.QueryOver<Species>().WhereRestrictionOn((System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.SpeciesGroup)).IsNotNull.Select((IProjection) Projections.Group<Species>((System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.SpeciesGroup)), (IProjection) Projections.Count<Species>((System.Linq.Expressions.Expression<Func<Species, object>>) (sp => sp.SpeciesGroup))).TransformUsing((IResultTransformer) new TupleResultTransformer<(SpeciesGroup, int)>()).Cacheable().List<(SpeciesGroup, int)>();
        speciesGroups = new Dictionary<SpeciesGroup, int>();
        foreach ((SpeciesGroup, int) tuple in (IEnumerable<(SpeciesGroup, int)>) tupleList)
          speciesGroups.Add(tuple.Item1, tuple.Item2);
      }
      return speciesGroups;
    }

    private double getShadeFactor()
    {
      if (this.Root.ShadingFactor.HasValue)
        return this.Root.ShadingFactor.Value;
      double shadeFactor = 0.0;
      for (SpeciesRank rank = SpeciesRank.Subgenus; rank >= SpeciesRank.Class; --rank)
      {
        try
        {
          if (this[rank] != null)
            shadeFactor = this._ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => (int) s.Rank == 7 || (int) s.Rank == 5)).Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.ShadingFactor != new double?())).Cacheable().List().Where<Species>((Func<Species, bool>) (s =>
            {
              if (TaxonModel.ParentOfChild(this[rank], s))
                return true;
              return rank == SpeciesRank.Genus && this.Genus != null && s.Id == this.Genus.Id;
            })).Average<Species>((Func<Species, double>) (s => s.ShadingFactor.Value));
          else
            continue;
        }
        catch (InvalidOperationException ex)
        {
        }
        if (shadeFactor != 0.0)
          break;
      }
      if (shadeFactor == 0.0)
        shadeFactor = this._ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.ShadingFactor != new double?())).Select((IProjection) Projections.Avg<Species>((System.Linq.Expressions.Expression<Func<Species, object>>) (s => (object) s.ShadingFactor))).Cacheable().SingleOrDefault<double>();
      return shadeFactor;
    }

    private double getKFactor() => this.Class != null && this.Class.KFactor.HasValue ? this.Class.KFactor.Value : 0.65;

    private double getLeafBiomassToAreaConversionFactor()
    {
      object conversionFactor1 = this._ls.CreateSQLQuery("SELECT Avg(ConversionFactor) AS AvgOfConversionFactor  FROM _LeafBiomassToArea  WHERE SpeciesId = :id").SetInt32("id", this.Root.Id).UniqueResult();
      if (conversionFactor1 != null)
        return (double) conversionFactor1;
      if (this.Genus != null)
      {
        object conversionFactor2 = this._ls.CreateSQLQuery("SELECT Avg(_LeafBiomassToArea.ConversionFactor) AS AvgOfConversionFactor  FROM _Species INNER JOIN _LeafBiomassToArea ON _Species.SpeciesId = _LeafBiomassToArea.SpeciesId  WHERE _Species.SpeciesId = :id OR _Species.ParentId = :id").SetInt32("id", this.Genus.Id).UniqueResult();
        if (conversionFactor2 != null)
          return (double) conversionFactor2;
      }
      if (this.Family != null)
      {
        object conversionFactor3 = this._ls.CreateSQLQuery("SELECT Avg(_LeafBiomassToArea.ConversionFactor) AS AvgOfConversionFactor  FROM(_Species INNER JOIN _LeafBiomassToArea ON _Species.SpeciesId = _LeafBiomassToArea.SpeciesId)  INNER JOIN _Species AS _Species_1 ON _Species.ParentId = _Species_1.SpeciesId  WHERE _Species_1.SpeciesId = :id OR _Species_1.ParentId = :id").SetInt32("id", this.Family.Id).UniqueResult();
        if (conversionFactor3 != null)
          return (double) conversionFactor3;
      }
      if (this.Order != null)
      {
        object conversionFactor4 = this._ls.CreateSQLQuery("SELECT Avg(_LeafBiomassToArea.ConversionFactor) AS AvgOfConversionFactor  FROM((_Species INNER JOIN _Species AS _Species_1 ON _Species.ParentId = _Species_1.SpeciesId)  INNER JOIN _LeafBiomassToArea ON _Species.SpeciesId = _LeafBiomassToArea.SpeciesId)  INNER JOIN _Species AS _Species_2 ON _Species_1.ParentId = _Species_2.SpeciesId WHERE _Species_2.SpeciesId = :id OR _Species_2.ParentId = :id").SetInt32("id", this.Order.Id).UniqueResult();
        if (conversionFactor4 != null)
          return (double) conversionFactor4;
      }
      if (this.Subclass != null)
      {
        object conversionFactor5 = this._ls.CreateSQLQuery("SELECT Avg(_LeafBiomassToArea.ConversionFactor) AS AvgOfConversionFactor  FROM(((_Species INNER JOIN _Species AS _Species_1 ON _Species.ParentId = _Species_1.SpeciesId)  INNER JOIN _Species AS _Species_2 ON _Species_1.ParentId = _Species_2.SpeciesId)  INNER JOIN _LeafBiomassToArea ON _Species.SpeciesId = _LeafBiomassToArea.SpeciesId) INNER JOIN _Species AS _Species_3 ON _Species_2.ParentId = _Species_3.SpeciesId  WHERE _Species_3.SpeciesId = :id OR _Species_3.ParentId = :id").SetInt32("id", this.Subclass.Id).UniqueResult();
        if (conversionFactor5 != null)
          return (double) conversionFactor5;
      }
      if (this.Class != null)
      {
        object conversionFactor6 = this._ls.CreateSQLQuery("SELECT Avg(_LeafBiomassToArea.ConversionFactor) AS AvgOfConversionFactor  FROM((((_Species INNER JOIN _Species AS _Species_1 ON _Species.ParentId = _Species_1.SpeciesId)  INNER JOIN _Species AS _Species_2 ON _Species_1.ParentId = _Species_2.SpeciesId)  INNER JOIN _Species AS _Species_3 ON _Species_2.ParentId = _Species_3.SpeciesId)  INNER JOIN _LeafBiomassToArea ON _Species.SpeciesId = _LeafBiomassToArea.SpeciesId)  LEFT JOIN _Species AS _Species_4 ON _Species_3.ParentId = _Species_4.SpeciesId  WHERE _Species_4.SpeciesId = :id OR _Species_4.ParentId = :id   OR _Species_3.SpeciesId = :id OR _Species_3.ParentId = :id").SetInt32("id", this.Class.Id).UniqueResult();
        if (conversionFactor6 != null)
          return (double) conversionFactor6;
      }
      return (double) this._ls.CreateSQLQuery("SELECT Avg(ConversionFactor) AS AvgOfConversionFactor  FROM _LeafBiomassToArea").UniqueResult();
    }

    private IList<SplinedBiomassEquationNew> getBiomassEquations()
    {
      int PalmFamilyId = 100;
      Species PalmFamilySpp = this._ls.CreateCriteria<Species>().Add((ICriterion) Restrictions.Eq("Id", (object) PalmFamilyId)).SetCacheable(true).UniqueResult<Species>();
      IList<SplinedBiomassEquationNew> biomassEquationNewList = (IList<SplinedBiomassEquationNew>) new List<SplinedBiomassEquationNew>();
      IList<SplinedBiomassEquationNew> biomassEquations = this._ls.CreateCriteria<SplinedBiomassEquationNew>().Add((ICriterion) Restrictions.Eq("Species.Id", (object) this._root.Id)).SetCacheable(true).List<SplinedBiomassEquationNew>();
      if (biomassEquations.Count > 0)
        return biomassEquations;
      if (this._root.Rank == SpeciesRank.Species)
      {
        biomassEquations = this._ls.CreateCriteria<SplinedBiomassEquationNew>().Add((ICriterion) Restrictions.Eq("Species.Id", (object) this._root.Parent.Id)).SetCacheable(true).List<SplinedBiomassEquationNew>();
        if (biomassEquations.Count > 0)
          return biomassEquations;
      }
      if (this._root.IsPalm())
        return (IList<SplinedBiomassEquationNew>) this._ls.QueryOver<SplinedBiomassEquationNew>().Inner.JoinQueryOver<Species>((System.Linq.Expressions.Expression<Func<SplinedBiomassEquationNew, Species>>) (e => e.Species)).Cacheable().List().Where<SplinedBiomassEquationNew>((Func<SplinedBiomassEquationNew, bool>) (e => TaxonModel.ParentOfChild(this[SpeciesRank.Family], e.Species) || e.Species.Id == PalmFamilyId)).ToList<SplinedBiomassEquationNew>();
      for (SpeciesRank rank = SpeciesRank.Subgenus; rank >= SpeciesRank.Class; --rank)
      {
        if (this[rank] != null)
        {
          biomassEquations = (IList<SplinedBiomassEquationNew>) this._ls.QueryOver<SplinedBiomassEquationNew>().Inner.JoinQueryOver<Species>((System.Linq.Expressions.Expression<Func<SplinedBiomassEquationNew, Species>>) (e => e.Species)).Cacheable().List().Where<SplinedBiomassEquationNew>((Func<SplinedBiomassEquationNew, bool>) (e => TaxonModel.ParentOfChild(this[rank], e.Species) && e.Species.Id != PalmFamilyId && !TaxonModel.ParentOfChild(PalmFamilySpp, e.Species))).ToList<SplinedBiomassEquationNew>();
          if (biomassEquations.Count > 0)
            break;
        }
      }
      if (biomassEquations.Count == 0)
        biomassEquations = (IList<SplinedBiomassEquationNew>) this._ls.QueryOver<SplinedBiomassEquationNew>().Inner.JoinQueryOver<Species>((System.Linq.Expressions.Expression<Func<SplinedBiomassEquationNew, Species>>) (e => e.Species)).Cacheable().List().Where<SplinedBiomassEquationNew>((Func<SplinedBiomassEquationNew, bool>) (e => e.Species.Id != PalmFamilyId && !TaxonModel.ParentOfChild(PalmFamilySpp, e.Species))).ToList<SplinedBiomassEquationNew>();
      return biomassEquations;
    }

    public static SpeciesRank RankOf(string spCode, ISession ls) => ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Code == spCode)).Select((System.Linq.Expressions.Expression<Func<Species, object>>) (s => (object) s.Rank)).Cacheable().SingleOrDefault<SpeciesRank>();

    public static bool ParentOfChild(Species parent, Species child)
    {
      SpeciesRank rank = parent.Rank;
      if (parent.Rank == SpeciesRank.Species || child.Rank == SpeciesRank.Class)
        return false;
      Species species = child;
      do
      {
        species = species.Parent;
      }
      while (species.Rank > rank);
      return species.Id == parent.Id;
    }

    public static bool ParentOfChild(Species parent, string childCode, ISession ls)
    {
      try
      {
        Species child = ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Code == childCode)).Cacheable().SingleOrDefault();
        return TaxonModel.ParentOfChild(parent, child);
      }
      catch (QueryException ex)
      {
        throw new ArgumentOutOfRangeException(nameof (childCode), (object) childCode, "No record in the _Species table matches the provided child code.");
      }
    }

    public static bool ParentOfChild(string parentCode, Species child, ISession ls)
    {
      try
      {
        return TaxonModel.ParentOfChild(ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Code == parentCode)).Cacheable().SingleOrDefault(), child);
      }
      catch (QueryException ex)
      {
        throw new ArgumentOutOfRangeException(nameof (parentCode), (object) parentCode, "No record in the _Species table matches the provided parent code.");
      }
    }

    public static bool ParentOfChild(string parentCode, string childCode, ISession ls)
    {
      Species parent;
      try
      {
        parent = ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Code == parentCode)).Cacheable().SingleOrDefault();
      }
      catch (QueryException ex)
      {
        throw new ArgumentOutOfRangeException(nameof (parentCode), (object) parentCode, "No record in the _Species table matches the provided parent code.");
      }
      Species child;
      try
      {
        child = ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Code == childCode)).Cacheable().SingleOrDefault();
      }
      catch (QueryException ex)
      {
        throw new ArgumentOutOfRangeException(nameof (childCode), (object) childCode, "No record in the _Species table matches the provided child code.");
      }
      return TaxonModel.ParentOfChild(parent, child);
    }

    public static ISet<Species> AllChildrenOf(Species parent)
    {
      if (parent.Rank == SpeciesRank.Genus)
        return parent.Children;
      ISet<Species> speciesSet = (ISet<Species>) new HashSet<Species>();
      foreach (Species child in (IEnumerable<Species>) parent.Children)
      {
        speciesSet.Add(child);
        foreach (Species species in (IEnumerable<Species>) TaxonModel.AllChildrenOf(child))
          speciesSet.Add(species);
      }
      return speciesSet;
    }

    public static ISet<Species> AllChildrenOf(string parentCode, ISession ls)
    {
      try
      {
        return TaxonModel.AllChildrenOf(ls.QueryOver<Species>().Where((System.Linq.Expressions.Expression<Func<Species, bool>>) (s => s.Code == parentCode)).Cacheable().SingleOrDefault());
      }
      catch (QueryException ex)
      {
        throw new ArgumentOutOfRangeException(nameof (parentCode), (object) parentCode, "No record in the _Species table matches the provided parent code.");
      }
    }
  }
}
