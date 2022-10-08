// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.Project
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using System.Collections.Generic;

namespace Eco.Domain.v5
{
  public class Project
  {
    private int? m_hash;
    private ProjectId m_compositeId;
    private Series m_series;
    private ISet<Plot> m_plots;
    private ISet<MapLandUse> m_mapLandUses;
    private ISet<CoverType> m_coverTypes;

    public Project() => this.Init();

    public Project(ProjectId id)
    {
      this.m_compositeId = id;
      this.Init();
    }

    public virtual ProjectId CompositeId => this.m_compositeId;

    public virtual short Id => this.IsTransient ? (short) 0 : this.m_compositeId.Year;

    public virtual ISet<Plot> Plots => this.m_plots;

    public virtual Series Series => this.m_series;

    public virtual ISet<MapLandUse> MapLandUses => this.m_mapLandUses;

    public virtual ISet<CoverType> CoverTypes => this.m_coverTypes;

    public virtual char Unit { get; set; }

    public virtual bool IsInitialMeasurement { get; set; }

    public virtual bool RecordHydro { get; set; }

    public virtual bool RecordShrub { get; set; }

    public virtual bool RecordEnergy { get; set; }

    public virtual bool RecordPlantableSpace { get; set; }

    public virtual string ProgGenVersion { get; set; }

    public virtual int DataEntrySoftware { get; set; }

    public virtual string DataEntryVersion { get; set; }

    public virtual bool RecordCLE { get; set; }

    public virtual bool RecordIPED { get; set; }

    public virtual string MobileKey { get; set; }

    public virtual string MobileEmail { get; set; }

    public virtual string Comments { get; set; }

    public virtual Carbon Carbon { get; set; }

    public virtual CO CO { get; set; }

    public virtual Electricity Electricity { get; set; }

    public virtual NO2 NO2 { get; set; }

    public virtual O3 O3 { get; set; }

    public virtual PM10 PM10 { get; set; }

    public virtual PM25 PM25 { get; set; }

    public virtual SO2 SO2 { get; set; }

    public virtual Gas Gas { get; set; }

    public virtual H2O H2O { get; set; }

    public virtual ExchangeRate ExchangeRate { get; set; }

    private void Init()
    {
      this.m_plots = (ISet<Plot>) new HashSet<Plot>();
      this.m_mapLandUses = (ISet<MapLandUse>) new HashSet<MapLandUse>();
      this.m_coverTypes = (ISet<CoverType>) new HashSet<CoverType>();
    }

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual ProjectDTO GetDTO()
    {
      ProjectDTO dto = new ProjectDTO()
      {
        Id = (int) this.Id,
        Unit = this.Unit,
        IsInitialMeasurement = this.IsInitialMeasurement,
        RecordHydro = this.RecordHydro,
        RecordShrub = this.RecordShrub,
        RecordEnergy = this.RecordEnergy,
        RecordPlantableSpace = this.RecordPlantableSpace,
        RecordCLE = this.RecordCLE,
        RecordIPED = this.RecordIPED,
        Series = this.Series.GetDTO(),
        MapLandUses = this.MapLandUses.Count > 0 ? new List<MapLandUseDTO>() : (List<MapLandUseDTO>) null,
        CoverTypes = this.CoverTypes.Count > 0 ? new List<CoverTypeDTO>() : (List<CoverTypeDTO>) null
      };
      foreach (MapLandUse mapLandUse in (IEnumerable<MapLandUse>) this.MapLandUses)
        dto.MapLandUses.Add(mapLandUse.GetDTO());
      foreach (CoverType coverType in (IEnumerable<CoverType>) this.CoverTypes)
        dto.CoverTypes.Add(coverType.GetDTO());
      return dto;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Project project) || this.IsTransient ^ project.IsTransient)
        return false;
      return this.IsTransient && project.IsTransient ? this == project : this.CompositeId.Equals((object) project.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }
  }
}
