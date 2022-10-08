// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.ForecastWeatherEvent
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v6
{
  public class ForecastWeatherEvent : ForecastEvent<ForecastWeatherEvent>
  {
    public virtual WeatherEvent WeatherEvent
    {
      get => this.\u003CWeatherEvent\u003Ek__BackingField;
      set
      {
        if (this.\u003CWeatherEvent\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (WeatherEvent));
        this.\u003CWeatherEvent\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.WeatherEvent);
      }
    }

    protected internal override ForecastWeatherEvent Clone(
      EntityMap map,
      bool deep = true)
    {
      ForecastWeatherEvent forecastWeatherEvent = ForecastEvent.Clone<ForecastWeatherEvent>(this, map);
      forecastWeatherEvent.WeatherEvent = this.WeatherEvent;
      return forecastWeatherEvent;
    }
  }
}
