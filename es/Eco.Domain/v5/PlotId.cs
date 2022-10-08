// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.PlotId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class PlotId : ProjectId
  {
    private int m_plot;

    public PlotId()
    {
    }

    public PlotId(string location, string series, short year, int plot)
      : base(location, series, year)
    {
      this.m_plot = plot;
    }

    public virtual int Plot => this.m_plot;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is PlotId plotId && !(this.Location != plotId.Location) && !(this.Series != plotId.Series) && (int) this.Year == (int) plotId.Year && this.Plot == plotId.Plot;
    }

    public override int GetHashCode()
    {
      int num = 11;
      return ((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.Plot.GetHashCode();
    }
  }
}
