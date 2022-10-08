// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.CoverTypeId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class CoverTypeId : ProjectId
  {
    private int m_cover;

    public CoverTypeId()
    {
    }

    public CoverTypeId(string location, string series, short year, int cover)
      : base(location, series, year)
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
        if (!(obj is CoverTypeId coverTypeId))
          return false;
        flag = this.Cover == coverTypeId.Cover;
      }
      return flag;
    }

    public override int GetHashCode()
    {
      int num = 2;
      return ((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.Cover.GetHashCode();
    }
  }
}
