// Decompiled with JetBrains decompiler
// Type: Streets.Domain.LookupId
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

namespace Streets.Domain
{
  public class LookupId
  {
    public virtual int Code { get; set; }

    public virtual Project Project { get; set; }

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is LookupId lookupId && this.Project == lookupId.Project && this.Code == lookupId.Code;
    }

    public override int GetHashCode()
    {
      int num = 5;
      return num * this.Project.GetHashCode() + num * this.Code.GetHashCode() + base.GetHashCode();
    }
  }
}
