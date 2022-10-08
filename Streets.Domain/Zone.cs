// Decompiled with JetBrains decompiler
// Type: Streets.Domain.Zone
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public class Zone
  {
    public virtual Project Project { get; set; }

    public virtual int Number { get; set; }

    public virtual string Name { get; set; }

    public virtual int Segments { get; set; }

    public virtual bool IsTransient => this.Project == null;

    public override bool Equals(object obj) => obj is Zone zone && !(this.GetType() != zone.GetType()) && !(this.IsTransient ^ zone.IsTransient) && this == zone && this.Project.Equals((object) zone.Project);

    public override int GetHashCode() => 19 * this.Project.Id;
  }
}
