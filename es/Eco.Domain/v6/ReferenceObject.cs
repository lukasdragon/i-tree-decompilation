// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.ReferenceObject
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class ReferenceObject : Entity<ReferenceObject>
  {
    public ReferenceObject()
    {
      this.Direction = -1;
      this.Distance = -1f;
      this.DBH = -1f;
      this.Object = ReferenceObjectType.Unknown;
    }

    public virtual Plot Plot
    {
      get => this.\u003CPlot\u003Ek__BackingField;
      set
      {
        if (this.\u003CPlot\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Plot));
        this.\u003CPlot\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Plot);
      }
    }

    public virtual int Direction
    {
      get => this.\u003CDirection\u003Ek__BackingField;
      set
      {
        if (this.\u003CDirection\u003Ek__BackingField == value)
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

    public virtual ReferenceObjectType Object
    {
      get => this.\u003CObject\u003Ek__BackingField;
      set
      {
        if (this.\u003CObject\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Object));
        this.\u003CObject\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Object);
      }
    }

    public virtual float DBH
    {
      get => this.\u003CDBH\u003Ek__BackingField;
      set
      {
        if (this.\u003CDBH\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DBH));
        this.\u003CDBH\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DBH);
      }
    }

    public virtual string Notes
    {
      get => this.\u003CNotes\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CNotes\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Notes));
        this.\u003CNotes\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Notes);
      }
    }

    public virtual ReferenceObjectDTO GetDTO()
    {
      ReferenceObjectDTO dto = new ReferenceObjectDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Direction = Util.NullIfDefault<int>(this.Direction, -1);
      dto.Distance = Util.NullIfDefault(this.Distance, -1f);
      dto.DBH = Util.NullIfDefault(this.DBH, -1f);
      dto.Notes = this.Notes;
      dto.Object = Util.NullIfDefault<int>((int) this.Object, -1);
      dto.Revision = this.Revision;
      return dto;
    }

    public override ReferenceObject Clone(bool deep) => ReferenceObject.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) ReferenceObject.Clone(this, new EntityMap());

    internal static ReferenceObject Clone(
      ReferenceObject ro,
      EntityMap map,
      bool deep = true)
    {
      ReferenceObject eNew;
      if (map.Contains((Entity) ro))
      {
        eNew = map.GetEntity<ReferenceObject>(ro);
      }
      else
      {
        eNew = new ReferenceObject();
        eNew.Direction = ro.Direction;
        eNew.Distance = ro.Distance;
        eNew.DBH = ro.DBH;
        eNew.Notes = ro.Notes;
        eNew.Object = ro.Object;
        map.Add((Entity) ro, (Entity) eNew);
      }
      if (deep)
        eNew.Plot = map.GetEntity<Plot>(ro.Plot);
      return eNew;
    }
  }
}
