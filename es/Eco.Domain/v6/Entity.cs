// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Entity
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;
using System.ComponentModel;

namespace Eco.Domain.v6
{
  public abstract class Entity : ICloneable, INotifyPropertyChanged, INotifyPropertyChanging
  {
    private int? m_hash;

    public virtual event PropertyChangedEventHandler PropertyChanged;

    public virtual Guid Guid { get; protected set; }

    [Browsable(false)]
    public virtual int Revision { get; set; }

    [Browsable(false)]
    public virtual bool IsTransient => this.Guid.Equals(new Guid());

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, eventArgs);
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Entity entity) || this.GetType() != entity.GetUnproxiedType() || this.IsTransient ^ entity.IsTransient)
        return false;
      return this.IsTransient && entity.IsTransient ? this == entity : this.Guid.Equals(entity.Guid);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.Guid.GetHashCode());
      return this.m_hash.Value;
    }

    protected virtual Type GetUnproxiedType() => this.GetType();

    public virtual object Clone() => (object) null;

    public virtual event PropertyChangingEventHandler PropertyChanging;

    public virtual void OnPropertyChanging(string propertyName)
    {
      PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
      if (propertyChanging == null)
        return;
      propertyChanging((object) this, new PropertyChangingEventArgs(propertyName));
    }
  }
}
