// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.StemId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class StemId : TreeId
  {
    private int m_stem;

    public StemId()
    {
    }

    public StemId(
      string location,
      string series,
      short year,
      int plot,
      int subplot,
      int tree,
      int stem)
      : base(location, series, year, plot, subplot, tree)
    {
      this.m_stem = stem;
    }

    public virtual int Stem => this.m_stem;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      bool flag = base.Equals(obj);
      if (flag)
      {
        if (!(obj is StemId stemId))
          return false;
        flag = this.Stem == stemId.Stem;
      }
      return flag;
    }

    public override int GetHashCode()
    {
      int num = 29;
      return (((((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.Plot.GetHashCode()) * num + this.SubPlot.GetHashCode()) * num + this.Tree.GetHashCode()) * num + this.Stem.GetHashCode();
    }
  }
}
