// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Lookup
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eco.Domain.v6
{
  public abstract class Lookup : Entity<Lookup>
  {
    public Lookup() => this.Trees = (ISet<Tree>) new HashSet<Tree>();

    [Browsable(false)]
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

    [LocalizedDescription(typeof (v6Strings), "Entity_Id")]
    public virtual int Id
    {
      get => this.\u003CId\u003Ek__BackingField;
      set
      {
        if (this.\u003CId\u003Ek__BackingField == value)
          return;
        this.OnPropertyChanging(nameof (Id));
        this.\u003CId\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Id);
      }
    }

    [LocalizedDescription(typeof (v6Strings), "Lookup_Description")]
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

    public virtual ISet<Tree> Trees
    {
      get => this.\u003CTrees\u003Ek__BackingField;
      protected set
      {
        if (this.Trees == value)
          return;
        this.OnPropertyChanging(nameof (Trees));
        this.\u003CTrees\u003Ek__BackingField = value;
        this.OnPropertyChanged(\u003C\u003EPropertyChangedEventArgs.Trees);
      }
    }
  }
}
