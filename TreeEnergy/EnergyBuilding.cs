// Decompiled with JetBrains decompiler
// Type: TreeEnergy.EnergyBuilding
// Assembly: TreeEnergy, Version=1.1.6718.0, Culture=neutral, PublicKeyToken=null
// MVID: 999CE94F-70B0-42EB-8D64-3ADB949CAFD0
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\TreeEnergy.dll

using LocationSpecies.Domain;

namespace TreeEnergy
{
  public class EnergyBuilding
  {
    public EnergyBuilding()
    {
      this.Heated = YesNoUnknown.Yes;
      this.AirConditioned = YesNoUnknown.Unknown;
    }

    public virtual short Direction { get; set; }

    public virtual float Distance { get; set; }

    public virtual BuildingVintage Vintage { get; set; }

    public virtual YesNoUnknown Heated { get; set; }

    public virtual YesNoUnknown AirConditioned { get; set; }
  }
}
