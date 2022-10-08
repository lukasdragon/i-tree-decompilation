// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.MobileLogEntry
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.v6
{
  public class MobileLogEntry : Entity<MobileLogEntry>
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

    public virtual string MobileKey
    {
      get => this.\u003CMobileKey\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CMobileKey\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (MobileKey));
        this.\u003CMobileKey\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.MobileKey);
      }
    }

    public virtual string Description
    {
      get => this.\u003CDescription\u003Ek__BackingField;
      set
      {
        if (string.Equals(this.\u003CDescription\u003Ek__BackingField, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Description));
        this.\u003CDescription\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Description);
      }
    }

    public virtual DateTime DateTime
    {
      get => this.\u003CDateTime\u003Ek__BackingField;
      set
      {
        if (DateTime.Equals(this.\u003CDateTime\u003Ek__BackingField, value))
          return;
        this.OnPropertyChanging(nameof (DateTime));
        this.\u003CDateTime\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.DateTime);
      }
    }

    public virtual bool Submitted
    {
      get => this.\u003CSubmitted\u003Ek__BackingField;
      set
      {
        if (this.\u003CSubmitted\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Submitted));
        this.\u003CSubmitted\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Submitted);
      }
    }

    public override object Clone() => base.Clone();

    public override MobileLogEntry Clone(bool deep) => MobileLogEntry.Clone(this, new EntityMap(), deep);

    internal static MobileLogEntry Clone(
      MobileLogEntry entry,
      EntityMap map,
      bool deep = true)
    {
      MobileLogEntry mobileLogEntry = (MobileLogEntry) null;
      if (entry != null)
      {
        if (map.Contains((Entity) entry))
        {
          mobileLogEntry = map.GetEntity<MobileLogEntry>(entry);
        }
        else
        {
          mobileLogEntry = new MobileLogEntry();
          mobileLogEntry.MobileKey = entry.MobileKey;
          mobileLogEntry.Description = entry.Description;
          mobileLogEntry.DateTime = entry.DateTime;
          mobileLogEntry.Submitted = entry.Submitted;
        }
        if (deep)
          mobileLogEntry.Year = map.GetEntity<Year>(entry.Year);
      }
      return mobileLogEntry;
    }
  }
}
