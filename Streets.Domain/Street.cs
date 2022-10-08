// Decompiled with JetBrains decompiler
// Type: Streets.Domain.Street
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public class Street
  {
    public virtual int Id { get; set; }

    public virtual Project Project { get; set; }

    public virtual string Name { get; set; }

    public virtual bool IsTransient => this.Id == 0;

    public override bool Equals(object obj) => obj is Street street && !(this.GetType() != street.GetType()) && !(this.IsTransient ^ street.IsTransient) && this == street && this.Id.Equals(street.Id);

    public override int GetHashCode() => 17 * this.Id;
  }
}
