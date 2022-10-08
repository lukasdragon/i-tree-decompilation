// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v6.YearlyCostDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.DTO.v6
{
  public class YearlyCostDTO : EntityDTO
  {
    public bool Public;
    public Decimal? Planting;
    public Decimal? Pruning;
    public Decimal? TreeRemoval;
    public Decimal? PestControl;
    public Decimal? Irrigation;
    public Decimal? Repair;
    public Decimal? CleanUp;
    public Decimal? Legal;
    public Decimal? Administrative;
    public Decimal? Inspection;
    public Decimal? Other;
  }
}
