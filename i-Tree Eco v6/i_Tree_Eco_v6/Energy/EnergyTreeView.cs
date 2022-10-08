// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Energy.EnergyTreeView
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using LocationSpecies.Domain;
using System.Collections.Generic;
using TreeEnergy;

namespace i_Tree_Eco_v6.Energy
{
  public class EnergyTreeView : EnergyTree
  {
    private Tree _tree;
    private Species _species;

    public EnergyTreeView(Tree tree, Species species)
    {
      this._tree = tree;
      this._species = species;
    }

    public override double Dieback => this._tree.Crown != null && this._tree.Crown.Condition != null ? this._tree.Crown.Condition.PctDieback : 13.0;

    public override int PercentCrownMissing => this._tree.Plot.Year.RecordCrownSize && this._tree.Crown != null ? (int) this._tree.Crown.PercentMissing : 0;

    public override Species Species => this._species;

    public override double TreeHeight
    {
      get => (double) this._tree.TreeHeight;
      set => base.TreeHeight = value;
    }

    public override List<double> StemDiameters
    {
      get
      {
        if (this._tree.Stems == null)
          return new List<double>();
        List<double> stemDiameters = new List<double>();
        foreach (Stem stem in (IEnumerable<Stem>) this._tree.Stems)
          stemDiameters.Add(stem.Diameter);
        return stemDiameters;
      }
    }
  }
}
