// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.YearLocationData
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.v6
{
  public class YearLocationData : Entity<YearLocationData>
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

    public virtual short WeatherYear
    {
      get => this.\u003CWeatherYear\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CWeatherYear\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (WeatherYear));
        this.\u003CWeatherYear\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.WeatherYear);
      }
    }

    public virtual string WeatherStationId
    {
      get => this.\u003CWeatherStationId\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CWeatherStationId\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (WeatherStationId));
        this.\u003CWeatherStationId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.WeatherStationId);
      }
    }

    public virtual short PollutionYear
    {
      get => this.\u003CPollutionYear\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CPollutionYear\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (PollutionYear));
        this.\u003CPollutionYear\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PollutionYear);
      }
    }

    public virtual int Population
    {
      get => this.\u003CPopulation\u003Ek__BackingField;
      set
      {
        if (this.\u003CPopulation\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Population));
        this.\u003CPopulation\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Population);
      }
    }

    public override YearLocationData Clone(bool deep) => YearLocationData.Clone(this, new EntityMap(), deep);

    public override object Clone() => (object) YearLocationData.Clone(this, new EntityMap());

    internal static YearLocationData Clone(
      YearLocationData wd,
      EntityMap map,
      bool deep = true)
    {
      YearLocationData eNew;
      if (map.Contains((Entity) wd))
      {
        eNew = map.GetEntity<YearLocationData>(wd);
      }
      else
      {
        eNew = new YearLocationData();
        eNew.WeatherYear = wd.WeatherYear;
        eNew.WeatherStationId = wd.WeatherStationId;
        eNew.PollutionYear = wd.PollutionYear;
        eNew.Population = wd.Population;
        map.Add((Entity) wd, (Entity) eNew);
      }
      if (deep)
      {
        eNew.Year = map.GetEntity<Year>(wd.Year);
        eNew.ProjectLocation = map.GetEntity<ProjectLocation>(wd.ProjectLocation);
      }
      return eNew;
    }
  }
}
