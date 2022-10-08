// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.CohortResult
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v6
{
  public class CohortResult : Entity<CohortResult>
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

    public virtual Strata Stratum
    {
      get => this.\u003CStratum\u003Ek__BackingField;
      set
      {
        if (this.\u003CStratum\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Stratum));
        this.\u003CStratum\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Stratum);
      }
    }

    public virtual CohortResultDataType DataType
    {
      get => this.\u003CDataType\u003Ek__BackingField;
      set
      {
        if (this.\u003CDataType\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DataType));
        this.\u003CDataType\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DataType);
      }
    }

    public virtual double DBHRangeStart
    {
      get => this.\u003CDBHRangeStart\u003Ek__BackingField;
      set
      {
        if (this.\u003CDBHRangeStart\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DBHRangeStart));
        this.\u003CDBHRangeStart\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DBHRangeStart);
      }
    }

    public virtual double DBHRangeEnd
    {
      get => this.\u003CDBHRangeEnd\u003Ek__BackingField;
      set
      {
        if (this.\u003CDBHRangeEnd\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DBHRangeEnd));
        this.\u003CDBHRangeEnd\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DBHRangeEnd);
      }
    }

    public virtual double DataValue
    {
      get => this.\u003CDataValue\u003Ek__BackingField;
      set
      {
        if (this.\u003CDataValue\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (DataValue));
        this.\u003CDataValue\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DataValue);
      }
    }

    public override object Clone() => (object) CohortResult.Clone(this, new EntityMap());

    public override CohortResult Clone(bool deep) => CohortResult.Clone(this, new EntityMap(), deep);

    internal static CohortResult Clone(CohortResult c, EntityMap map, bool deep = true)
    {
      CohortResult eNew;
      if (map.Contains((Entity) c))
      {
        eNew = map.GetEntity<CohortResult>(c);
      }
      else
      {
        eNew = new CohortResult();
        eNew.ForecastedYear = c.ForecastedYear;
        eNew.DataType = c.DataType;
        eNew.DBHRangeStart = c.DBHRangeStart;
        eNew.DBHRangeEnd = c.DBHRangeEnd;
        eNew.DataValue = c.DataValue;
        map.Add((Entity) c, (Entity) eNew);
      }
      if (deep)
      {
        eNew.Forecast = map.GetEntity<Forecast>(c.Forecast);
        eNew.Stratum = map.GetEntity<Strata>(c.Stratum);
      }
      return eNew;
    }
  }
}
