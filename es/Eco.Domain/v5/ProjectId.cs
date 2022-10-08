// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.ProjectId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class ProjectId : SeriesId
  {
    private short m_year;

    public ProjectId()
    {
    }

    public ProjectId(string location, string series, short year)
      : base(location, series)
    {
      this.m_year = year;
    }

    public virtual short Year => this.m_year;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is ProjectId projectId && !(this.Location != projectId.Location) && !(this.Series != projectId.Series) && (int) this.Year == (int) projectId.Year;
    }

    public override int GetHashCode()
    {
      int num = 13;
      return (num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode();
    }
  }
}
