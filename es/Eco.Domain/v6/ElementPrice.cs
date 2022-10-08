// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.ElementPrice
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.v6
{
  public abstract class ElementPrice : Entity<ElementPrice>
  {
    public virtual double Price
    {
      get => this.\u003CPrice\u003Ek__BackingField;
      set
      {
        if (this.\u003CPrice\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Price));
        this.\u003CPrice\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Price);
      }
    }

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

    public static T Clone<T>(T ep, EntityMap map, bool deep = true) where T : ElementPrice
    {
      T eNew = default (T);
      if ((object) ep != null)
      {
        if (map.Contains((Entity) ep))
        {
          eNew = map.GetEntity<T>(ep);
        }
        else
        {
          eNew = Activator.CreateInstance<T>();
          eNew.Price = ep.Price;
          map.Add((Entity) ep, (Entity) eNew);
        }
        if (deep)
          eNew.Year = map.GetEntity<Year>(ep.Year);
      }
      return eNew;
    }
  }
}
