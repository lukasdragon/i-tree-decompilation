// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.Series
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using System.Collections.Generic;

namespace Eco.Domain.v5
{
  public class Series
  {
    private int? m_hash;
    private SeriesId m_compositeId;
    private ISet<Project> m_projects;
    private Location m_location;

    public Series() => this.Init();

    public Series(SeriesId id)
    {
      this.m_compositeId = id;
      this.Init();
    }

    public virtual string Id => this.IsTransient ? (string) null : this.m_compositeId.Series;

    public virtual SeriesId CompositeId => this.m_compositeId;

    public virtual Location Location => this.m_location;

    public virtual ISet<Project> Projects => this.m_projects;

    public virtual bool IsPermanent { get; set; }

    public virtual char SampleType { get; set; }

    public virtual int SampleMethod { get; set; }

    public virtual float DefaultSubplotSize { get; set; }

    public virtual char DefaultSubplotSizeUnit { get; set; }

    public virtual int NextAvailabPlotID { get; set; }

    public virtual string GISProjection { get; set; }

    public virtual short GISUnit { get; set; }

    public virtual string Comments { get; set; }

    private void Init() => this.m_projects = (ISet<Project>) new HashSet<Project>();

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual SeriesDTO GetDTO() => new SeriesDTO()
    {
      Id = this.Id,
      IsPermanent = this.IsPermanent,
      SampleType = this.SampleType,
      SampleMethod = this.SampleMethod,
      DefaultSubplotSize = this.DefaultSubplotSize,
      DefaultSubplotSizeUnit = this.DefaultSubplotSizeUnit,
      NextAvailabPlotID = this.NextAvailabPlotID,
      GISProjection = this.GISProjection,
      GISUnit = this.GISUnit,
      Comments = this.Comments,
      Location = this.Location.GetDTO(),
      Projects = new List<ProjectDTO>()
    };

    public override bool Equals(object obj)
    {
      if (!(obj is Series series) || this.IsTransient ^ series.IsTransient)
        return false;
      return this.IsTransient && series.IsTransient ? this == series : this.CompositeId.Equals((object) series.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    public virtual bool IsSample() => this.SampleType != 'I';
  }
}
