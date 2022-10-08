// Decompiled with JetBrains decompiler
// Type: Eco.Util.Convert.PlotData
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using System.Collections.Generic;

namespace Eco.Util.Convert
{
  public class PlotData
  {
    private Dictionary<char, PlotLandUse> m_lu;

    public PlotData() => this.m_lu = new Dictionary<char, PlotLandUse>();

    public Dictionary<char, PlotLandUse> LandUses => this.m_lu;
  }
}
