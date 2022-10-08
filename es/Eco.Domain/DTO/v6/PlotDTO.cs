// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v6.PlotDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;
using System.Collections.Generic;

namespace Eco.Domain.DTO.v6
{
  public class PlotDTO : EntityDTO
  {
    public int Id;
    public Guid? Street;
    public string Address;
    public string Comments;
    public string ContactInfo;
    public string Crew;
    public DateTime? Date;
    public double? Latitude;
    public double? Longitude;
    public int? OffsetPoint;
    public int? PercentMeasured;
    public short? PercentPlantable;
    public short? PercentShrubCover;
    public short? PercentTreeCover;
    public string Photo;
    public float? Size;
    public bool? Stake;
    public bool? IsComplete;
    public Guid? Strata;
    public List<PlotGroundCoverDTO> PlotGroundCovers;
    public List<PlotLandUseDTO> LandUses;
    public List<ReferenceObjectDTO> ReferenceObjects;
    public List<ShrubDTO> Shrubs;
    public List<TreeDTO> Trees;
    public List<PlantingSiteDTO> PlantingSites;
    public Guid? PriorYear;
  }
}
