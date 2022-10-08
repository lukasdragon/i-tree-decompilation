// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v6.TreeDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;
using System.Collections.Generic;

namespace Eco.Domain.DTO.v6
{
  public class TreeDTO : EntityDTO
  {
    public int Id;
    public string UserId;
    public short? CityManaged;
    public Guid? PlotLandUse;
    public int? Direction;
    public float? Distance;
    public char? Status;
    public string Species;
    public float? TreeHeight;
    public int? PercentImpervious;
    public int? PercentShrub;
    public bool? StreetTree;
    public Guid? SiteType;
    public string Address;
    public Guid? Street;
    public Guid? LocSite;
    public int? LocNo;
    public Guid? MaintRec;
    public Guid? MaintTask;
    public Guid? SidewalkDamage;
    public Guid? WireConflict;
    public Guid? LeafCondition;
    public Guid? OtherOne;
    public Guid? OtherTwo;
    public Guid? OtherThree;
    public double? Latitude;
    public double? Longitude;
    public DateTime? SurveyDate;
    public bool? NoteThisTree;
    public string Comments;
    public CrownDTO Crown;
    public IPEDDTO IPED;
    public List<StemDTO> Stems;
    public List<BuildingDTO> Buildings;
    public Guid? PriorYear;
  }
}
