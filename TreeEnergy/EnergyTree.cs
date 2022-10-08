// Decompiled with JetBrains decompiler
// Type: TreeEnergy.EnergyTree
// Assembly: TreeEnergy, Version=1.1.6718.0, Culture=neutral, PublicKeyToken=null
// MVID: 999CE94F-70B0-42EB-8D64-3ADB949CAFD0
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\TreeEnergy.dll

using LocationSpecies.Domain;
using System.Collections.Generic;

namespace TreeEnergy
{
  public class EnergyTree
  {
    public virtual double Dieback { get; set; }

    public virtual int PercentCrownMissing { get; set; }

    public virtual Species Species { get; set; }

    public virtual double TreeHeight { get; set; }

    public virtual List<double> StemDiameters { get; set; }
  }
}
