// Decompiled with JetBrains decompiler
// Type: Streets.Domain.CityData
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public class CityData
  {
    public virtual int Id { get; set; }

    public virtual Project Project { get; set; }

    public virtual double TotalBudget { get; set; }

    public virtual long Population { get; set; }

    public virtual double TotalLandArea { get; set; }

    public virtual double AvgStreetWidth { get; set; }

    public virtual double AvgSidewalkWidth { get; set; }

    public virtual double TotalLinearStreetMiles { get; set; }

    public virtual bool IsTransient => this.Project == null;

    public override bool Equals(object obj) => obj is CityData cityData && !(this.GetType() != cityData.GetType()) && !(this.IsTransient ^ cityData.IsTransient) && this == cityData && this.Project.Equals((object) cityData.Project);

    public override int GetHashCode() => 2 * this.Project.Id;
  }
}
