// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Mortality
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.v6
{
  public class Mortality : Entity<Mortality>
  {
    public virtual Forecast Forecast
    {
      get => this.\u003CForecast\u003Ek__BackingField;
      set
      {
        if (this.\u003CForecast\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Forecast));
        this.\u003CForecast\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Forecast);
      }
    }

    public virtual string Type
    {
      get => this.\u003CType\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CType\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Type));
        this.\u003CType\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Type);
      }
    }

    public virtual string Value
    {
      get => this.\u003CValue\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CValue\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Value));
        this.\u003CValue\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Value);
      }
    }

    public virtual double Percent
    {
      get => this.\u003CPercent\u003Ek__BackingField;
      set
      {
        if (this.\u003CPercent\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Percent));
        this.\u003CPercent\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Percent);
      }
    }

    public virtual bool IsPercentStarting
    {
      get => this.\u003CIsPercentStarting\u003Ek__BackingField;
      set
      {
        if (this.\u003CIsPercentStarting\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (IsPercentStarting));
        this.\u003CIsPercentStarting\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.IsPercentStarting);
      }
    }

    public override object Clone() => (object) Mortality.Clone(this, new EntityMap());

    public override Mortality Clone(bool deep) => Mortality.Clone(this, new EntityMap(), deep);

    internal static Mortality Clone(Mortality m, EntityMap map, bool deep = true)
    {
      Mortality eNew;
      if (map.Contains((Entity) m))
      {
        eNew = map.GetEntity<Mortality>(m);
      }
      else
      {
        eNew = new Mortality();
        eNew.Type = m.Type;
        eNew.Value = m.Value;
        eNew.Percent = m.Percent;
        eNew.IsPercentStarting = m.IsPercentStarting;
        eNew.Forecast = m.Forecast;
        map.Add((Entity) m, (Entity) eNew);
      }
      if (deep)
        eNew.Forecast = map.GetEntity<Forecast>(m.Forecast);
      return eNew;
    }
  }
}
