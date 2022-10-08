// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Stem
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.DTO;
using Eco.Domain.DTO.v6;
using System;

namespace Eco.Domain.v6
{
  public class Stem : Entity<Stem>
  {
    public Stem()
    {
      this.Diameter = -1.0;
      this.DiameterHeight = -1.0;
      this.Measured = true;
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

    public virtual double Diameter
    {
      get => this.\u003CDiameter\u003Ek__BackingField;
      set
      {
        if (this.\u003CDiameter\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Diameter));
        this.\u003CDiameter\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Diameter);
      }
    }

    public virtual double DiameterHeight
    {
      get => this.\u003CDiameterHeight\u003Ek__BackingField;
      set
      {
        if (this.\u003CDiameterHeight\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DiameterHeight));
        this.\u003CDiameterHeight\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DiameterHeight);
      }
    }

    public virtual bool Measured
    {
      get => this.\u003CMeasured\u003Ek__BackingField;
      set
      {
        if (this.\u003CMeasured\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Measured));
        this.\u003CMeasured\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Measured);
      }
    }

    public virtual StemDTO GetDTO()
    {
      StemDTO dto = new StemDTO();
      dto.Guid = this.IsTransient ? new Guid?() : new Guid?(this.Guid);
      dto.Id = this.Id;
      dto.Diameter = Util.NullIfDefault(this.Diameter, -1.0);
      dto.DiameterHeight = Util.NullIfDefault(this.DiameterHeight, -1.0);
      dto.Measured = Util.NullIfDefault<bool>(this.Measured, true);
      dto.Revision = this.Revision;
      return dto;
    }

    public override Stem Clone(bool deep) => Stem.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) Stem.Clone(this, new EntityMap());

    internal static Stem Clone(Stem s, EntityMap map, bool deep = true)
    {
      Stem eNew;
      if (map.Contains((Entity) s))
      {
        eNew = map.GetEntity<Stem>(s);
      }
      else
      {
        eNew = new Stem();
        eNew.Id = s.Id;
        eNew.Diameter = s.Diameter;
        eNew.DiameterHeight = s.DiameterHeight;
        eNew.Measured = s.Measured;
        map.Add((Entity) s, (Entity) eNew);
      }
      if (deep)
        eNew.Tree = map.GetEntity<Tree>(s.Tree);
      return eNew;
    }
  }
}
