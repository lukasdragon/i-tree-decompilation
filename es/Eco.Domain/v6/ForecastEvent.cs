// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.ForecastEvent
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.v6
{
  public abstract class ForecastEvent : Entity<ForecastEvent>
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

    public virtual short StartYear
    {
      get => this.\u003CStartYear\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CStartYear\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (StartYear));
        this.\u003CStartYear\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.StartYear);
      }
    }

    public virtual double MortalityPercent
    {
      get => this.\u003CMortalityPercent\u003Ek__BackingField;
      set
      {
        if (this.\u003CMortalityPercent\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (MortalityPercent));
        this.\u003CMortalityPercent\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MortalityPercent);
      }
    }

    internal static T Clone<T>(T s, EntityMap map, bool deep = true) where T : ForecastEvent
    {
      T obj = default (T);
      T eNew;
      if (map.Contains((Entity) s))
      {
        eNew = map.GetEntity<T>(s);
      }
      else
      {
        eNew = Activator.CreateInstance<T>();
        eNew.StartYear = s.StartYear;
        eNew.MortalityPercent = s.MortalityPercent;
        map.Add((Entity) s, (Entity) eNew);
      }
      if (deep)
        eNew.Forecast = map.GetEntity<Forecast>(s.Forecast);
      return eNew;
    }
  }
}
