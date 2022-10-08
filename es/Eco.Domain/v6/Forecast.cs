// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Forecast
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;
using System.Collections.Generic;

namespace Eco.Domain.v6
{
  public class Forecast : Entity<Forecast>
  {
    private const short YEARS = 30;
    private const short FF_DAYS = 149;
    private const double DIEBACK00_49 = 3.0;
    private const double DIEBACK50_74 = 13.08;
    private const double DIEBACK75_99 = 50.0;

    public Forecast() => this.Init();

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

    public virtual string Title
    {
      get => this.\u003CTitle\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CTitle\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Title));
        this.\u003CTitle\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Title);
      }
    }

    public virtual short NumYears
    {
      get => this.\u003CNumYears\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CNumYears\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (NumYears));
        this.\u003CNumYears\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.NumYears);
      }
    }

    public virtual short FrostFreeDays
    {
      get => this.\u003CFrostFreeDays\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CFrostFreeDays\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (FrostFreeDays));
        this.\u003CFrostFreeDays\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.FrostFreeDays);
      }
    }

    public virtual bool Changed
    {
      get => this.\u003CChanged\u003Ek__BackingField;
      set
      {
        if (this.\u003CChanged\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Changed));
        this.\u003CChanged\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Changed);
      }
    }

    public virtual ISet<Mortality> Mortalities
    {
      get => this.\u003CMortalities\u003Ek__BackingField;
      protected internal set
      {
        if (this.Mortalities == value)
          return;
        this.OnPropertyChanging(nameof (Mortalities));
        this.\u003CMortalities\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Mortalities);
      }
    }

    public virtual ISet<Eco.Domain.v6.Replanting> Replanting
    {
      get => this.\u003CReplanting\u003Ek__BackingField;
      protected internal set
      {
        if (this.Replanting == value)
          return;
        this.OnPropertyChanging(nameof (Replanting));
        this.\u003CReplanting\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Replanting);
      }
    }

    public virtual ISet<ForecastWeatherEvent> WeatherEvents
    {
      get => this.\u003CWeatherEvents\u003Ek__BackingField;
      protected internal set
      {
        if (this.WeatherEvents == value)
          return;
        this.OnPropertyChanging(nameof (WeatherEvents));
        this.\u003CWeatherEvents\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.WeatherEvents);
      }
    }

    public virtual ISet<ForecastPestEvent> PestEvents
    {
      get => this.\u003CPestEvents\u003Ek__BackingField;
      protected internal set
      {
        if (this.PestEvents == value)
          return;
        this.OnPropertyChanging(nameof (PestEvents));
        this.\u003CPestEvents\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PestEvents);
      }
    }

    private void Init()
    {
      this.Mortalities = (ISet<Mortality>) new HashSet<Mortality>();
      this.Replanting = (ISet<Eco.Domain.v6.Replanting>) new HashSet<Eco.Domain.v6.Replanting>();
      this.WeatherEvents = (ISet<ForecastWeatherEvent>) new HashSet<ForecastWeatherEvent>();
      this.PestEvents = (ISet<ForecastPestEvent>) new HashSet<ForecastPestEvent>();
    }

    public override object Clone() => (object) Forecast.Clone(this, new EntityMap());

    public override Forecast Clone(bool deep) => Forecast.Clone(this, new EntityMap(), deep);

    internal static Forecast Clone(Forecast f, EntityMap map, bool deep = true)
    {
      Forecast eNew;
      if (map.Contains((Entity) f))
      {
        eNew = map.GetEntity<Forecast>(f);
      }
      else
      {
        eNew = new Forecast();
        eNew.Title = f.Title;
        eNew.NumYears = f.NumYears;
        eNew.FrostFreeDays = f.FrostFreeDays;
        eNew.Changed = f.Changed;
        map.Add((Entity) f, (Entity) eNew);
        if (deep)
        {
          foreach (Mortality mortality in (IEnumerable<Mortality>) f.Mortalities)
            eNew.Mortalities.Add(Mortality.Clone(mortality, map));
          foreach (Eco.Domain.v6.Replanting i in (IEnumerable<Eco.Domain.v6.Replanting>) f.Replanting)
            eNew.Replanting.Add(Eco.Domain.v6.Replanting.Clone(i, map));
          foreach (ForecastWeatherEvent weatherEvent in (IEnumerable<ForecastWeatherEvent>) f.WeatherEvents)
            eNew.WeatherEvents.Add(weatherEvent.Clone(map, true));
          foreach (ForecastPestEvent pestEvent in (IEnumerable<ForecastPestEvent>) f.PestEvents)
            eNew.PestEvents.Add(pestEvent.Clone(map, true));
        }
      }
      if (deep)
        eNew.Year = map.GetEntity<Year>(f.Year);
      return eNew;
    }
  }
}
