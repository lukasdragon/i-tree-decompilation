// Decompiled with JetBrains decompiler
// Type: Streets.Domain.Benefit
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public class Benefit
  {
    public virtual int Id { get; set; }

    public virtual Project Project { get; set; }

    public virtual ElementPrice ElementPrice { get; set; }

    public virtual float PropertyReductionFactor { get; set; }

    public virtual int AvgLargeLeafArea { get; set; }

    public virtual bool IsTransient => this.Project == null;

    public override bool Equals(object obj) => obj is Benefit benefit && !(this.GetType() != benefit.GetType()) && !(this.IsTransient ^ benefit.IsTransient) && this == benefit && this.Project.Equals((object) benefit.Project);

    public override int GetHashCode() => this.Project.Id;
  }
}
