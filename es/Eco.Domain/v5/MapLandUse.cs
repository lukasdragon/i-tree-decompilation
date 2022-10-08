// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.MapLandUse
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v5;
using System.Collections.Generic;

namespace Eco.Domain.v5
{
  public class MapLandUse
  {
    private int? m_hash;
    private MapLandUseId m_compositeId;
    private Project m_project;
    private ISet<Plot> m_plots;

    public MapLandUse() => this.Init();

    public MapLandUse(MapLandUseId id)
    {
      this.m_compositeId = id;
      this.Init();
    }

    public virtual MapLandUseId CompositeId => this.m_compositeId;

    public virtual Project Project => this.m_project;

    public virtual int Id => this.IsTransient ? 0 : this.m_compositeId.MapLandUse;

    public virtual ISet<Plot> Plots => this.m_plots;

    public virtual string Description { get; set; }

    public virtual string Abbreviation { get; set; }

    public virtual float Size { get; set; }

    private void Init() => this.m_plots = (ISet<Plot>) new HashSet<Plot>();

    public virtual bool IsTransient => this.m_compositeId == null;

    public virtual MapLandUseDTO GetDTO() => new MapLandUseDTO()
    {
      Id = (long) this.Id,
      Abbreviation = this.Abbreviation,
      Description = this.Description,
      Size = this.Size
    };

    public override bool Equals(object obj)
    {
      if (!(obj is MapLandUse mapLandUse) || this.IsTransient ^ mapLandUse.IsTransient)
        return false;
      return this.IsTransient && mapLandUse.IsTransient ? this == mapLandUse : this.CompositeId.Equals((object) mapLandUse.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }
  }
}
