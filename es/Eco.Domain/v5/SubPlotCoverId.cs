// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.SubPlotCoverId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class SubPlotCoverId : SubPlotId
  {
    private int m_cover;

    public SubPlotCoverId()
    {
    }

    public SubPlotCoverId(
      string location,
      string series,
      short year,
      int plot,
      int subplot,
      int cover)
      : base(location, series, year, plot, subplot)
    {
      this.m_cover = cover;
    }

    public virtual int Cover => this.m_cover;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      bool flag = base.Equals(obj);
      if (flag)
      {
        if (!(obj is SubPlotCoverId subPlotCoverId))
          return false;
        flag = this.Cover == subPlotCoverId.Cover;
      }
      return flag;
    }

    public override int GetHashCode()
    {
      int num = 31;
      return ((((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.Plot.GetHashCode()) * num + this.SubPlot.GetHashCode()) * num + this.Cover.GetHashCode();
    }
  }
}
