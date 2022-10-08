// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.ShrubId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class ShrubId : SubPlotId
  {
    private int m_shrub;

    public ShrubId()
    {
    }

    public ShrubId(string location, string series, short year, int plot, int subplot, int shrub)
      : base(location, series, year, plot, subplot)
    {
      this.m_shrub = shrub;
    }

    public virtual int Shrub => this.m_shrub;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      bool flag = base.Equals(obj);
      if (flag)
      {
        if (!(obj is ShrubId shrubId))
          return false;
        flag = this.Shrub == shrubId.Shrub;
      }
      return flag;
    }

    public override int GetHashCode()
    {
      int num = 23;
      return ((((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.Plot.GetHashCode()) * num + this.SubPlot.GetHashCode()) * num + this.Shrub.GetHashCode();
    }
  }
}
