// Decompiled with JetBrains decompiler
// Type: Streets.Domain.ManagementCost
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

using System;

namespace Streets.Domain
{
  public class ManagementCost
  {
    public virtual Project Project { get; set; }

    public virtual bool IsPublic { get; set; }

    public virtual Decimal TotalBudgetCost { get; set; }

    public virtual Decimal Planting { get; set; }

    public virtual Decimal Pruning { get; set; }

    public virtual Decimal TreeRemoval { get; set; }

    public virtual Decimal PestControl { get; set; }

    public virtual Decimal Irrigation { get; set; }

    public virtual Decimal Repair { get; set; }

    public virtual Decimal CleanUp { get; set; }

    public virtual Decimal Lawsuit { get; set; }

    public virtual Decimal Admin { get; set; }

    public virtual Decimal Inspection { get; set; }

    public virtual Decimal Other { get; set; }

    public virtual bool IsTransient => this.Project == null;

    public override bool Equals(object obj) => obj is ManagementCost managementCost && !(this.GetType() != managementCost.GetType()) && !(this.IsTransient ^ managementCost.IsTransient) && this == managementCost && this.Project.Equals((object) managementCost.Project);

    public override int GetHashCode() => 7 * this.Project.Id;
  }
}
