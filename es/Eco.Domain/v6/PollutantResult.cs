// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.PollutantResult
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v6
{
  public class PollutantResult : Entity<PollutantResult>
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

    public virtual int ForecastedYear
    {
      get => this.\u003CForecastedYear\u003Ek__BackingField;
      set
      {
        if (this.\u003CForecastedYear\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (ForecastedYear));
        this.\u003CForecastedYear\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.ForecastedYear);
      }
    }

    public virtual int PollutantId
    {
      get => this.\u003CPollutantId\u003Ek__BackingField;
      set
      {
        if (this.\u003CPollutantId\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (PollutantId));
        this.\u003CPollutantId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.PollutantId);
      }
    }

    public virtual double Amount
    {
      get => this.\u003CAmount\u003Ek__BackingField;
      set
      {
        if (this.\u003CAmount\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Amount));
        this.\u003CAmount\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Amount);
      }
    }

    public virtual double Value
    {
      get => this.\u003CValue\u003Ek__BackingField;
      set
      {
        if (this.\u003CValue\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Value));
        this.\u003CValue\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Value);
      }
    }

    public override object Clone() => (object) PollutantResult.Clone(this, new EntityMap());

    public override PollutantResult Clone(bool deep) => PollutantResult.Clone(this, new EntityMap(), deep);

    internal static PollutantResult Clone(
      PollutantResult c,
      EntityMap map,
      bool deep = true)
    {
      PollutantResult eNew;
      if (map.Contains((Entity) c))
      {
        eNew = map.GetEntity<PollutantResult>(c);
      }
      else
      {
        eNew = new PollutantResult();
        eNew.ForecastedYear = c.ForecastedYear;
        eNew.PollutantId = c.PollutantId;
        eNew.Amount = c.Amount;
        eNew.Value = c.Value;
        map.Add((Entity) c, (Entity) eNew);
      }
      eNew.Forecast = map.GetEntity<Forecast>(c.Forecast);
      return eNew;
    }
  }
}
