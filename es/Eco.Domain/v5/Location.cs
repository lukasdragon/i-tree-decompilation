// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.Location
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using System.Collections.Generic;

namespace Eco.Domain.v5
{
  public class Location
  {
    private int? m_hash;
    private LocationId m_compositeId;
    private ISet<Eco.Domain.v5.Series> m_series;

    public Location() => this.Init();

    public Location(LocationId id)
    {
      this.m_compositeId = id;
      this.Init();
    }

    public virtual string Id => this.IsTransient ? (string) null : this.m_compositeId.Location;

    public virtual LocationId CompositeId => this.m_compositeId;

    public virtual ISet<Eco.Domain.v5.Series> Series => this.m_series;

    public virtual string NationCode { get; set; }

    public virtual string StateCode { get; set; }

    public virtual string CountyCode { get; set; }

    public virtual string CityCode { get; set; }

    public virtual short WeatherYear { get; set; }

    public virtual string WeatherStationID { get; set; }

    public virtual string UFOREVersion { get; set; }

    public virtual string Comments { get; set; }

    private void Init() => this.m_series = (ISet<Eco.Domain.v5.Series>) new HashSet<Eco.Domain.v5.Series>();

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual LocationDTO GetDTO() => new LocationDTO()
    {
      Id = this.Id,
      NationCode = this.NationCode,
      StateCode = this.StateCode,
      CountyCode = this.CountyCode,
      CityCode = this.CityCode,
      WeatherYear = (int) this.WeatherYear,
      WeatherStationID = this.WeatherStationID,
      Comments = this.Comments
    };

    public override bool Equals(object obj)
    {
      if (!(obj is Location location) || this.IsTransient ^ location.IsTransient)
        return false;
      return this.IsTransient && location.IsTransient ? this == location : this.CompositeId.Equals((object) location.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }
  }
}
