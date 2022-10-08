// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v5.SubPlotDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System.Collections.Generic;

namespace Eco.Domain.DTO.v5
{
  public class SubPlotDTO
  {
    public int Id;
    public float Size = -1f;
    public int OffsetPoint = -1;
    public bool Stake;
    public short PercentTreeCover = -1;
    public short PercentShrubCover;
    public short PercentPlantable = -1;
    public short PercentMeasured = 100;
    public string Comments;
    public string Photo;
    public List<FieldLandUseDTO> LandUses;
    public List<TreeDTO> Trees;
    public List<ShrubDTO> Shrubs;
    public List<ReferenceObjectDTO> ReferenceObjects;
    public List<SubPlotCoverDTO> Covers;
  }
}
