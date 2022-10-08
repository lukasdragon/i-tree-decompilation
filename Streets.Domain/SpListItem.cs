// Decompiled with JetBrains decompiler
// Type: Streets.Domain.SpListItem
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public class SpListItem
  {
    public virtual Project Project { get; set; }

    public virtual string Code { get; set; }

    public virtual string ScientificName { get; set; }

    public virtual string CommonName { get; set; }

    public virtual string TreeType { get; set; }

    public virtual string SppValueAssignment { get; set; }

    public virtual double Rating { get; set; }

    public virtual double BasicPrice { get; set; }

    public virtual double PalmTrunkCost { get; set; }

    public virtual double ReplacementCost { get; set; }

    public virtual double TAr { get; set; }

    public virtual bool IsTransient => this.Project == null;

    public override bool Equals(object obj) => obj is SpListItem spListItem && !(this.GetType() != spListItem.GetType()) && !(this.IsTransient ^ spListItem.IsTransient) && this == spListItem && this.Project == spListItem.Project;

    public override int GetHashCode() => 13 * this.Project.Id;
  }
}
