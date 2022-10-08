// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.FieldLandUseId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class FieldLandUseId : SubPlotId
  {
    private char m_fieldLandUse;

    public FieldLandUseId()
    {
    }

    public FieldLandUseId(
      string location,
      string series,
      short year,
      int plot,
      int subplot,
      char fieldLandUse)
      : base(location, series, year, plot, subplot)
    {
      this.m_fieldLandUse = fieldLandUse;
    }

    public virtual char FieldLandUse => this.m_fieldLandUse;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      bool flag = base.Equals(obj);
      if (flag)
      {
        if (!(obj is FieldLandUseId fieldLandUseId))
          return false;
        flag = (int) this.FieldLandUse == (int) fieldLandUseId.FieldLandUse;
      }
      return flag;
    }

    public override int GetHashCode()
    {
      int num = 3;
      return ((((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.Plot.GetHashCode()) * num + this.SubPlot.GetHashCode()) * num + this.FieldLandUse.GetHashCode();
    }
  }
}
