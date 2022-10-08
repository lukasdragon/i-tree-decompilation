// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Street
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class Street : Entity<Street>
  {
    public virtual ProjectLocation ProjectLocation
    {
      get => this.\u003CProjectLocation\u003Ek__BackingField;
      set
      {
        if (this.\u003CProjectLocation\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (ProjectLocation));
        this.\u003CProjectLocation\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.ProjectLocation);
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

    public virtual StreetDTO GetDTO()
    {
      StreetDTO dto = new StreetDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Name = this.Name;
      dto.Revision = this.Revision;
      return dto;
    }

    public override Street Clone(bool deep) => Street.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) Street.Clone(this, new EntityMap());

    internal static Street Clone(Street s, EntityMap map, bool deep = true)
    {
      Street eNew;
      if (map.Contains((Entity) s))
      {
        eNew = map.GetEntity<Street>(s);
      }
      else
      {
        eNew = new Street();
        eNew.Name = s.Name;
        map.Add((Entity) s, (Entity) eNew);
      }
      if (deep)
        eNew.ProjectLocation = map.GetEntity<ProjectLocation>(s.ProjectLocation);
      return eNew;
    }
  }
}
