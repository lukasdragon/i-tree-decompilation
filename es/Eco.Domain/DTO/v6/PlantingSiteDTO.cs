// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v6.PlantingSiteDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.DTO.v6
{
  public class PlantingSiteDTO : EntityDTO
  {
    public Guid? PlantingSiteType;
    public Guid? PlotLandUse;
    public Guid? Street;
    public string StreetAddress;
    public double? xCoordinate;
    public double? yCoordinate;
  }
}
