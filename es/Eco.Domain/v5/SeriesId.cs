// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.SeriesId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class SeriesId : LocationId
  {
    private string m_series;

    public SeriesId()
    {
    }

    public SeriesId(string location, string series)
      : base(location)
    {
      this.m_series = series;
    }

    public virtual string Series => this.m_series;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is SeriesId seriesId && !(this.Series != seriesId.Series) && this.Location == seriesId.Location;
    }

    public override int GetHashCode()
    {
      int num = 19;
      return num * this.Location.GetHashCode() * num + this.Series.GetHashCode();
    }
  }
}
