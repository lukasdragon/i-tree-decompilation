// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.GroundCover
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;
using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class GroundCover : Entity<GroundCover>
  {
    public GroundCover() => this.Init();

    public virtual Year Year
    {
      get => this.\u003CYear\u003Ek__BackingField;
      set
      {
        if (this.\u003CYear\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Year));
        this.\u003CYear\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Year);
      }
    }

    public virtual ISet<PlotGroundCover> PlotGroundCovers
    {
      get => this.\u003CPlotGroundCovers\u003Ek__BackingField;
      protected internal set
      {
        if (this.PlotGroundCovers == value)
          return;
        this.OnPropertyChanging(nameof (PlotGroundCovers));
        this.\u003CPlotGroundCovers\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlotGroundCovers);
      }
    }

    public virtual int Id
    {
      get => this.\u003CId\u003Ek__BackingField;
      set
      {
        if (this.\u003CId\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Id));
        this.\u003CId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
      }
    }

    public virtual string Description
    {
      get => this.\u003CDescription\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CDescription\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Description));
        this.\u003CDescription\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Description);
      }
    }

    public virtual int CoverTypeId
    {
      get => this.\u003CCoverTypeId\u003Ek__BackingField;
      set
      {
        if (this.\u003CCoverTypeId\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (CoverTypeId));
        this.\u003CCoverTypeId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.CoverTypeId);
      }
    }

    private void Init() => this.PlotGroundCovers = (ISet<PlotGroundCover>) new HashSet<PlotGroundCover>();

    public virtual GroundCoverDTO GetDTO()
    {
      GroundCoverDTO dto = new GroundCoverDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Description = this.Description;
      dto.Revision = this.Revision;
      return dto;
    }

    public override GroundCover Clone(bool deep) => GroundCover.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) GroundCover.Clone(this, new EntityMap());

    internal static GroundCover Clone(GroundCover gc, EntityMap map, bool deep = true)
    {
      GroundCover eNew;
      if (map.Contains((Entity) gc))
      {
        eNew = map.GetEntity<GroundCover>(gc);
      }
      else
      {
        eNew = new GroundCover();
        eNew.Id = gc.Id;
        eNew.Description = gc.Description;
        eNew.CoverTypeId = gc.CoverTypeId;
        map.Add((Entity) gc, (Entity) eNew);
      }
      if (deep)
        eNew.Year = map.GetEntity<Year>(gc.Year);
      return eNew;
    }
  }
}
