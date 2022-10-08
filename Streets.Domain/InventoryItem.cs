// Decompiled with JetBrains decompiler
// Type: Streets.Domain.InventoryItem
// Assembly: Streets.Domain, Version=1.1.5560.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C8C80D5-C46C-4F96-B160-AECDF3D99BE1
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Streets.Domain.dll

using System;

namespace Streets.Domain
{
  public class InventoryItem
  {
    public virtual int Id { get; set; }

    public virtual Project Project { get; set; }

    public virtual int StreetSeg { get; set; }

    public virtual Zone Zone { get; set; }

    public virtual SpListItem SpCode { get; set; }

    public virtual short? CityManaged { get; set; }

    public virtual double DBH { get; set; }

    public virtual FieldLandUse LandUse { get; set; }

    public virtual SiteType SiteType { get; set; }

    public virtual LocSite LocSite { get; set; }

    public virtual int? LocNo { get; set; }

    public virtual MaintRec MaintRec { get; set; }

    public virtual MaintTask MaintTask { get; set; }

    public virtual Sidewalk SidewalkDamage { get; set; }

    public virtual WireConflict WireConflict { get; set; }

    public virtual Condition WoodyCondition { get; set; }

    public virtual Condition LeavesCondition { get; set; }

    public virtual OtherOne OtherOne { get; set; }

    public virtual OtherTwo OtherTwo { get; set; }

    public virtual OtherThree OtherThree { get; set; }

    public virtual double Latitude { get; set; }

    public virtual double Longitude { get; set; }

    public virtual Street Street { get; set; }

    public virtual string StreetNumber { get; set; }

    public virtual string SurveyorId { get; set; }

    public virtual DateTime? SurveyDate { get; set; }

    public virtual bool NoteThisTree { get; set; }

    public virtual string Comments { get; set; }

    public virtual IPED IPED { get; set; }

    public virtual bool IsTransient => this.Id == 0;

    public override bool Equals(object obj) => obj is InventoryItem inventoryItem && !(this.GetType() != inventoryItem.GetType()) && !(this.IsTransient ^ inventoryItem.IsTransient) && this == inventoryItem && this.Id.Equals(inventoryItem.Id);

    public override int GetHashCode() => 11 * this.Id;
  }
}
