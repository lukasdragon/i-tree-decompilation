// Decompiled with JetBrains decompiler
// Type: Eco.Util.Convert.YearData
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using System.Collections.Generic;

namespace Eco.Util.Convert
{
  public class YearData
  {
    private Dictionary<int, GroundCover> m_gc;
    private Dictionary<int, Eco.Domain.v6.Strata> m_strata;
    private Dictionary<char, LandUse> m_lu;
    private Dictionary<double, Condition> m_cond;

    public YearData()
    {
      this.m_gc = new Dictionary<int, GroundCover>();
      this.m_strata = new Dictionary<int, Eco.Domain.v6.Strata>();
      this.m_lu = new Dictionary<char, LandUse>();
      this.m_cond = new Dictionary<double, Condition>();
    }

    public Dictionary<int, GroundCover> GroundCovers => this.m_gc;

    public Dictionary<int, Eco.Domain.v6.Strata> Strata => this.m_strata;

    public Dictionary<char, LandUse> LandUses => this.m_lu;

    public Dictionary<double, Condition> Conditions => this.m_cond;
  }
}
