// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Energy.EnergyBuildingView
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using TreeEnergy;

namespace i_Tree_Eco_v6.Energy
{
  public class EnergyBuildingView : EnergyBuilding
  {
    private Building _building;

    public EnergyBuildingView(Building building) => this._building = building;

    public override short Direction => this._building.Direction;

    public override float Distance => this._building.Distance;
  }
}
