// Decompiled with JetBrains decompiler
// Type: Eco.Util.Views.PlotStrata
// Assembly: Eco Util, Version=1.1.6757.0, Culture=neutral, PublicKeyToken=null
// MVID: 8C6433F7-3274-4315-B6F4-90179805B344
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco Util.dll

using Eco.Domain.v6;
using Eco\u0020Util;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace Eco.Util.Views
{
  public class PlotStrata : ICloneable, INotifyPropertyChanged, INotifyPropertyChanging
  {
    public Strata Strata
    {
      get => this.\u003CStrata\u003Ek__BackingField;
      private set
      {
        if (this.\u003CStrata\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging("Id");
        this.OnPropertyChanging("Abbreviation");
        this.OnPropertyChanging("Description");
        this.OnPropertyChanging("Size");
        this.OnPropertyChanging("Year");
        this.OnPropertyChanging(nameof (Strata));
        this.\u003CStrata\u003Ek__BackingField = value;
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Abbreviation);
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Description);
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Size);
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Year);
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Strata);
      }
    }

    public int Id
    {
      get => this.Strata.Id;
      set
      {
        if (this.Id == value)
          return;
        this.OnPropertyChanging(nameof (Id));
        this.Strata.Id = value;
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
      }
    }

    public string Abbreviation
    {
      get => this.Strata.Abbreviation;
      set
      {
        if (string.Equals(this.Abbreviation, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Abbreviation));
        this.Strata.Abbreviation = value;
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Abbreviation);
      }
    }

    public string Description
    {
      get => this.Strata.Description;
      set
      {
        if (string.Equals(this.Description, value, StringComparison.Ordinal))
          return;
        this.OnPropertyChanging(nameof (Description));
        this.Strata.Description = value;
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Description);
      }
    }

    public float Size
    {
      get => this.Strata.Size;
      set
      {
        if (this.Size == value)
          return;
        this.OnPropertyChanging(nameof (Size));
        this.Strata.Size = value;
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Size);
      }
    }

    public Year Year
    {
      get => this.Strata.Year;
      set
      {
        if (this.Year == value)
          return;
        this.OnPropertyChanging(nameof (Year));
        this.Strata.Year = value;
        this.\u003C\u003EOnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Year);
      }
    }

    public int TotalPlots => this.Strata.Plots.Count;

    public int PlotsToAdd { get; set; }

    public bool isComplete => this.Abbreviation != null && this.Description != null && (double) this.Size > 0.0;

    public PlotStrata() => this.Strata = new Strata();

    public PlotStrata(Strata s) => this.Strata = s;

    public object Clone() => (object) new PlotStrata(this.Strata.Clone() as Strata);

    [field: NonSerialized]
    public event PropertyChangedEventHandler PropertyChanged;

    [GeneratedCode("PropertyChanged.Fody", "3.2.8.0")]
    [DebuggerNonUserCode]
    protected void \u003C\u003EOnPropertyChanged(PropertyChangedEventArgs eventArgs)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, eventArgs);
    }

    public event PropertyChangingEventHandler PropertyChanging;

    public virtual void OnPropertyChanging(string propertyName)
    {
      PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
      if (propertyChanging == null)
        return;
      propertyChanging((object) this, new PropertyChangingEventArgs(propertyName));
    }
  }
}
