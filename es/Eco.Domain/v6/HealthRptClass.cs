// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.HealthRptClass
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;
using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class HealthRptClass : Entity<HealthRptClass>
  {
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

    public virtual double Extent
    {
      get => this.\u003CExtent\u003Ek__BackingField;
      set
      {
        if (this.\u003CExtent\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Extent));
        this.\u003CExtent\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Extent);
      }
    }

    public override HealthRptClass Clone(bool deep) => HealthRptClass.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) HealthRptClass.Clone(this, new EntityMap());

    internal static HealthRptClass Clone(HealthRptClass c, EntityMap map, bool deep = true)
    {
      HealthRptClass eNew;
      if (map.Contains((Entity) c))
      {
        eNew = map.GetEntity<HealthRptClass>(c);
      }
      else
      {
        eNew = new HealthRptClass();
        eNew.Id = c.Id;
        eNew.Description = c.Description;
        eNew.Extent = c.Extent;
        map.Add((Entity) c, (Entity) eNew);
      }
      if (deep)
        eNew.Year = map.GetEntity<Year>(c.Year);
      return eNew;
    }

    public class HealthRptClassComparer : IComparer<HealthRptClass>
    {
      public virtual int Compare(HealthRptClass x, HealthRptClass y) => Comparer<int>.Default.Compare(y.Id, x.Id);
    }
  }
}
