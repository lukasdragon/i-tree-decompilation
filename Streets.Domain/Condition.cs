// Decompiled with JetBrains decompiler
// Type: Streets.Domain.Condition
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public class Condition : Lookup
  {
    public virtual double WoodyConditionFactor { get; set; }

    public virtual double LeavesConditionFactor { get; set; }
  }
}
