// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Building
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class Building : Entity<Building>
  {
    public Building()
    {
      this.Direction = (short) -1;
      this.Distance = -1f;
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

    public virtual Tree Tree
    {
      get => this.\u003CTree\u003Ek__BackingField;
      set
      {
        if (this.\u003CTree\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Tree));
        this.\u003CTree\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Tree);
      }
    }

    public virtual short Direction
    {
      get => this.\u003CDirection\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CDirection\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (Direction));
        this.\u003CDirection\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Direction);
      }
    }

    public virtual float Distance
    {
      get => this.\u003CDistance\u003Ek__BackingField;
      set
      {
        if (this.\u003CDistance\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Distance));
        this.\u003CDistance\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Distance);
      }
    }

    public virtual BuildingDTO GetDTO()
    {
      BuildingDTO dto = new BuildingDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Direction = Util.NullIfDefault<short>(this.Direction, (short) -1);
      dto.Distance = Util.NullIfDefault(this.Distance, -1f);
      dto.Revision = this.Revision;
      return dto;
    }

    public override Building Clone(bool deep) => Building.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) Building.Clone(this, new EntityMap());

    internal static Building Clone(Building b, EntityMap map, bool deep = true)
    {
      Building eNew;
      if (map.Contains((Entity) b))
      {
        eNew = map.GetEntity<Building>(b);
      }
      else
      {
        eNew = new Building();
        eNew.Id = b.Id;
        eNew.Direction = b.Direction;
        eNew.Distance = b.Distance;
        map.Add((Entity) b, (Entity) eNew);
      }
      if (deep)
        eNew.Tree = map.GetEntity<Tree>(b.Tree);
      return eNew;
    }
  }
}
