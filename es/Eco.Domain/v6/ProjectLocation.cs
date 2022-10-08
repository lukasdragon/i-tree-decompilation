// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.ProjectLocation
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;
using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class ProjectLocation : Entity<ProjectLocation>
  {
    public ProjectLocation() => this.Init();

    public virtual ISet<Street> Streets
    {
      get => this.\u003CStreets\u003Ek__BackingField;
      protected internal set
      {
        if (this.Streets == value)
          return;
        this.OnPropertyChanging(nameof (Streets));
        this.\u003CStreets\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Streets);
      }
    }

    public virtual ISet<Eco.Domain.v6.YearLocationData> YearLocationData
    {
      get => this.\u003CYearLocationData\u003Ek__BackingField;
      protected internal set
      {
        if (this.YearLocationData == value)
          return;
        this.OnPropertyChanging(nameof (YearLocationData));
        this.\u003CYearLocationData\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.YearLocationData);
      }
    }

    public virtual ISet<Plot> Plots
    {
      get => this.\u003CPlots\u003Ek__BackingField;
      protected internal set
      {
        if (this.Plots == value)
          return;
        this.OnPropertyChanging(nameof (Plots));
        this.\u003CPlots\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Plots);
      }
    }

    public virtual Project Project
    {
      get => this.\u003CProject\u003Ek__BackingField;
      set
      {
        if (this.\u003CProject\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Project));
        this.\u003CProject\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Project);
      }
    }

    public virtual int LocationId
    {
      get => this.\u003CLocationId\u003Ek__BackingField;
      set
      {
        if (this.\u003CLocationId\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (LocationId));
        this.\u003CLocationId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.LocationId);
      }
    }

    public virtual string NationCode
    {
      get => this.\u003CNationCode\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CNationCode\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (NationCode));
        this.\u003CNationCode\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.NationCode);
      }
    }

    public virtual string PrimaryPartitionCode
    {
      get => this.\u003CPrimaryPartitionCode\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CPrimaryPartitionCode\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (PrimaryPartitionCode));
        this.\u003CPrimaryPartitionCode\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PrimaryPartitionCode);
      }
    }

    public virtual string SecondaryPartitionCode
    {
      get => this.\u003CSecondaryPartitionCode\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CSecondaryPartitionCode\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (SecondaryPartitionCode));
        this.\u003CSecondaryPartitionCode\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.SecondaryPartitionCode);
      }
    }

    public virtual string TertiaryPartitionCode
    {
      get => this.\u003CTertiaryPartitionCode\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CTertiaryPartitionCode\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (TertiaryPartitionCode));
        this.\u003CTertiaryPartitionCode\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.TertiaryPartitionCode);
      }
    }

    public virtual bool IsUrban
    {
      get => this.\u003CIsUrban\u003Ek__BackingField;
      set
      {
        if (this.\u003CIsUrban\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (IsUrban));
        this.\u003CIsUrban\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.IsUrban);
      }
    }

    public virtual bool UseTropical
    {
      get => this.\u003CUseTropical\u003Ek__BackingField;
      set
      {
        if (this.\u003CUseTropical\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (UseTropical));
        this.\u003CUseTropical\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.UseTropical);
      }
    }

    private void Init()
    {
      this.Streets = (ISet<Street>) new HashSet<Street>();
      this.YearLocationData = (ISet<Eco.Domain.v6.YearLocationData>) new HashSet<Eco.Domain.v6.YearLocationData>();
      this.Plots = (ISet<Plot>) new HashSet<Plot>();
    }

    public virtual ProjectLocationDTO GetDTO()
    {
      ProjectLocationDTO projectLocationDto = new ProjectLocationDTO();
      projectLocationDto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      projectLocationDto.LocationId = this.LocationId;
      projectLocationDto.NationCode = this.NationCode;
      projectLocationDto.PrimaryPartitionCode = this.PrimaryPartitionCode;
      projectLocationDto.SecondaryPartitionCode = this.SecondaryPartitionCode;
      projectLocationDto.TertiaryPartitionCode = this.TertiaryPartitionCode;
      projectLocationDto.Streets = this.Streets.Count > 0 ? new List<StreetDTO>() : (List<StreetDTO>) null;
      projectLocationDto.IsUrban = this.IsUrban;
      projectLocationDto.UseTropical = this.UseTropical;
      projectLocationDto.Revision = this.Revision;
      ProjectLocationDTO dto = projectLocationDto;
      foreach (Street street in (IEnumerable<Street>) this.Streets)
        dto.Streets.Add(street.GetDTO());
      return dto;
    }

    public override ProjectLocation Clone(bool deep) => ProjectLocation.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) ProjectLocation.Clone(this, new EntityMap());

    public static ProjectLocation Clone(
      ProjectLocation pl,
      EntityMap map,
      bool deep = true)
    {
      ProjectLocation eNew = (ProjectLocation) null;
      if (pl != null)
      {
        if (map.Contains((Entity) pl))
        {
          eNew = map.GetEntity<ProjectLocation>(pl);
        }
        else
        {
          eNew = new ProjectLocation();
          eNew.LocationId = pl.LocationId;
          eNew.NationCode = pl.NationCode;
          eNew.PrimaryPartitionCode = pl.PrimaryPartitionCode;
          eNew.SecondaryPartitionCode = pl.SecondaryPartitionCode;
          eNew.TertiaryPartitionCode = pl.TertiaryPartitionCode;
          eNew.IsUrban = pl.IsUrban;
          eNew.UseTropical = pl.UseTropical;
          map.Add((Entity) pl, (Entity) eNew);
          if (deep)
          {
            foreach (Street street in (IEnumerable<Street>) pl.Streets)
              eNew.Streets.Add(Street.Clone(street, map));
          }
        }
        if (deep)
          eNew.Project = map.GetEntity<Project>(pl.Project);
      }
      return eNew;
    }
  }
}
