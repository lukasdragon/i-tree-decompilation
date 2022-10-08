// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.ReferenceObjectId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class ReferenceObjectId : SubPlotId
  {
    private int m_direction;

    public ReferenceObjectId()
    {
    }

    public ReferenceObjectId(
      string location,
      string series,
      short year,
      int plot,
      int subplot,
      int direction)
      : base(location, series, year, plot, subplot)
    {
      this.m_direction = direction;
    }

    public virtual int Direction => this.m_direction;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      bool flag = base.Equals(obj);
      if (flag)
      {
        if (!(obj is ReferenceObjectId referenceObjectId))
          return false;
        flag = this.Direction == referenceObjectId.Direction;
      }
      return flag;
    }

    public override int GetHashCode()
    {
      int num = 17;
      return ((((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.Plot.GetHashCode()) * num + this.SubPlot.GetHashCode()) * num + this.Direction.GetHashCode();
    }
  }
}
