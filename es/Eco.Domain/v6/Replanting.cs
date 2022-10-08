// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Replanting
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.v6
{
  public class Replanting : Entity<Replanting>
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

    public virtual double DBH
    {
      get => this.\u003CDBH\u003Ek__BackingField;
      set
      {
        if (this.\u003CDBH\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DBH));
        this.\u003CDBH\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DBH);
      }
    }

    public virtual string StratumDesc
    {
      get => this.\u003CStratumDesc\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CStratumDesc\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (StratumDesc));
        this.\u003CStratumDesc\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.StratumDesc);
      }
    }

    public virtual int Number
    {
      get => this.\u003CNumber\u003Ek__BackingField;
      set
      {
        if (this.\u003CNumber\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Number));
        this.\u003CNumber\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Number);
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

    public override object Clone() => (object) Replanting.Clone(this, new EntityMap());

    public override Replanting Clone(bool deep) => Replanting.Clone(this, new EntityMap(), deep);

    internal static Replanting Clone(Replanting i, EntityMap map, bool deep = true)
    {
      Replanting eNew;
      if (map.Contains((Entity) i))
      {
        eNew = map.GetEntity<Replanting>(i);
      }
      else
      {
        eNew = new Replanting();
        eNew.DBH = i.DBH;
        eNew.StratumDesc = i.StratumDesc;
        eNew.Number = i.Number;
        eNew.Forecast = i.Forecast;
        map.Add((Entity) i, (Entity) eNew);
      }
      eNew.Forecast = map.GetEntity<Forecast>(i.Forecast);
      return eNew;
    }
  }
}
