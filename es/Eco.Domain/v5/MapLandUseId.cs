// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.MapLandUseId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class MapLandUseId : ProjectId
  {
    private int m_mapLandUse;

    public MapLandUseId()
    {
    }

    public MapLandUseId(string location, string series, short year, int mapLandUse)
      : base(location, series, year)
    {
      this.m_mapLandUse = mapLandUse;
    }

    public virtual int MapLandUse => this.m_mapLandUse;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      bool flag = base.Equals(obj);
      if (flag)
      {
        if (!(obj is MapLandUseId mapLandUseId))
          return false;
        flag = this.MapLandUse == mapLandUseId.MapLandUse;
      }
      return flag;
    }

    public override int GetHashCode()
    {
      int num = 7;
      return ((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.MapLandUse.GetHashCode();
    }
  }
}
