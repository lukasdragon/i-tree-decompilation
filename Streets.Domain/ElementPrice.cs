// Decompiled with JetBrains decompiler
// Type: Streets.Domain.ElementPrice
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public class ElementPrice
  {
    public virtual double CO2 { get; set; }

    public virtual double Electricity { get; set; }

    public virtual double H2O { get; set; }

    public virtual double Home { get; set; }

    public virtual double Gas { get; set; }

    public virtual double NO2 { get; set; }

    public virtual double O3 { get; set; }

    public virtual double PM10 { get; set; }

    public virtual double SO2 { get; set; }

    public virtual double VOC { get; set; }
  }
}
