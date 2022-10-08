// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.ForecastPestEvent
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v6
{
  public class ForecastPestEvent : ForecastEvent<ForecastPestEvent>
  {
    public virtual int PestId
    {
      get => this.\u003CPestId\u003Ek__BackingField;
      set
      {
        if (this.\u003CPestId\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PestId));
        this.\u003CPestId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PestId);
      }
    }

    public virtual short Duration
    {
      get => this.\u003CDuration\u003Ek__BackingField;
      set
      {
        if ((int) this.\u003CDuration\u003Ek__BackingField == (int) value)
          return;
        this.OnPropertyChanging(nameof (Duration));
        this.\u003CDuration\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Duration);
      }
    }

    public virtual bool PlantPestHosts
    {
      get => this.\u003CPlantPestHosts\u003Ek__BackingField;
      set
      {
        if (this.\u003CPlantPestHosts\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PlantPestHosts));
        this.\u003CPlantPestHosts\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PlantPestHosts);
      }
    }

    protected internal override ForecastPestEvent Clone(EntityMap map, bool deep = true)
    {
      ForecastPestEvent forecastPestEvent = ForecastEvent.Clone<ForecastPestEvent>(this, map);
      forecastPestEvent.PestId = this.PestId;
      forecastPestEvent.Duration = this.Duration;
      forecastPestEvent.PlantPestHosts = this.PlantPestHosts;
      return forecastPestEvent;
    }
  }
}
