// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.YearResult
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.v6
{
  public class YearResult : Entity<YearResult>
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

    public virtual int RevProcessed
    {
      get => this.\u003CRevProcessed\u003Ek__BackingField;
      set
      {
        if (this.\u003CRevProcessed\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (RevProcessed));
        this.\u003CRevProcessed\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.RevProcessed);
      }
    }

    public virtual System.DateTime? DateTime
    {
      get => this.\u003CDateTime\u003Ek__BackingField;
      set
      {
        if (Nullable.Equals<System.DateTime>(this.DateTime, value))
          return;
        this.OnPropertyChanging(nameof (DateTime));
        this.\u003CDateTime\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DateTime);
      }
    }

    public virtual bool Completed
    {
      get => this.\u003CCompleted\u003Ek__BackingField;
      set
      {
        if (this.\u003CCompleted\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Completed));
        this.\u003CCompleted\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Completed);
      }
    }

    public virtual string Email
    {
      get => this.\u003CEmail\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CEmail\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Email));
        this.\u003CEmail\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Email);
      }
    }

    public virtual string Data
    {
      get => this.\u003CData\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CData\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Data));
        this.\u003CData\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Data);
      }
    }

    public override object Clone() => (object) YearResult.Clone(this, new EntityMap());

    public override YearResult Clone(bool deep) => YearResult.Clone(this, new EntityMap(), deep);

    internal static YearResult Clone(YearResult yr, EntityMap map, bool deep = true)
    {
      YearResult yearResult = (YearResult) null;
      if (yr != null)
      {
        yearResult = new YearResult();
        yearResult.RevProcessed = yr.RevProcessed;
        yearResult.DateTime = yr.DateTime;
        yearResult.Completed = yr.Completed;
        yearResult.Email = yr.Email;
        yearResult.Data = yr.Data;
        if (deep)
          yearResult.Year = map.GetEntity<Year>(yr.Year);
      }
      return yearResult;
    }
  }
}
