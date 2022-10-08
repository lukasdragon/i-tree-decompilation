// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Project
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;
using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class Project : Entity<Project>
  {
    public Project() => this.Init();

    public Project(ProjectDTO project)
    {
      this.Init();
      this.Guid = project.Guid.GetValueOrDefault();
    }

    public Project(string name)
    {
      this.Name = name;
      this.Init();
    }

    public virtual ISet<Eco.Domain.v6.Series> Series
    {
      get => this.\u003CSeries\u003Ek__BackingField;
      protected internal set
      {
        if (this.Series == value)
          return;
        this.OnPropertyChanging(nameof (Series));
        this.\u003CSeries\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Series);
      }
    }

    public virtual ISet<ProjectLocation> Locations
    {
      get => this.\u003CLocations\u003Ek__BackingField;
      protected internal set
      {
        if (this.Locations == value)
          return;
        this.OnPropertyChanging(nameof (Locations));
        this.\u003CLocations\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Locations);
      }
    }

    public virtual string Name
    {
      get => this.\u003CName\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CName\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Name));
        this.\u003CName\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Name);
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

    public virtual string UFOREVersion
    {
      get => this.\u003CUFOREVersion\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CUFOREVersion\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (UFOREVersion));
        this.\u003CUFOREVersion\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.UFOREVersion);
      }
    }

    public virtual string Comments
    {
      get => this.\u003CComments\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CComments\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Comments));
        this.\u003CComments\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Comments);
      }
    }

    private void Init()
    {
      this.Series = (ISet<Eco.Domain.v6.Series>) new HashSet<Eco.Domain.v6.Series>();
      this.Locations = (ISet<ProjectLocation>) new HashSet<ProjectLocation>();
    }

    public virtual ProjectDTO GetDTO()
    {
      ProjectDTO projectDto = new ProjectDTO();
      projectDto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      projectDto.Name = this.Name;
      projectDto.Locations = this.Locations.Count > 0 ? new List<ProjectLocationDTO>() : (List<ProjectLocationDTO>) null;
      projectDto.Revision = this.Revision;
      ProjectDTO dto = projectDto;
      foreach (ProjectLocation location in (IEnumerable<ProjectLocation>) this.Locations)
        dto.Locations.Add(location.GetDTO());
      return dto;
    }

    public override Project Clone(bool deep) => Project.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) Project.Clone(this, new EntityMap());

    internal static Project Clone(Project p, EntityMap map, bool deep = false)
    {
      Project eNew;
      if (map.Contains((Entity) p))
      {
        eNew = map.GetEntity<Project>(p);
      }
      else
      {
        eNew = new Project();
        eNew.Name = p.Name;
        eNew.LocationId = p.LocationId;
        eNew.NationCode = p.NationCode;
        eNew.PrimaryPartitionCode = p.PrimaryPartitionCode;
        eNew.SecondaryPartitionCode = p.SecondaryPartitionCode;
        eNew.TertiaryPartitionCode = p.TertiaryPartitionCode;
        map.Add((Entity) p, (Entity) eNew);
        if (deep)
        {
          foreach (ProjectLocation location in (IEnumerable<ProjectLocation>) p.Locations)
            eNew.Locations.Add(ProjectLocation.Clone(location, map));
          foreach (Eco.Domain.v6.Series s in (IEnumerable<Eco.Domain.v6.Series>) p.Series)
            eNew.Series.Add(Eco.Domain.v6.Series.Clone(s, map));
        }
      }
      return eNew;
    }
  }
}
