// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeEnergyFieldsBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using LocationSpecies.Domain;
using TreeEnergy;

namespace i_Tree_Eco_v6.Reports
{
  public abstract class IndividualTreeEnergyFieldsBase
  {
    public abstract IndividualTreeEnergyFieldsBase GetEnergyImpactValues(
      DatabaseReport dbr,
      Tree tree,
      Species species,
      IndividualTree indivTree,
      double? percentGroundCover);

    public object this[string propertyName]
    {
      get => this.GetType().GetProperty(propertyName).GetValue((object) this, (object[]) null);
      set => this.GetType().GetProperty(propertyName).SetValue((object) this, value, (object[]) null);
    }

    public string Name { get; set; }

    public int PlotID { get; set; }

    public int TreeID { get; set; }

    public double Total { get; set; }
  }
}
