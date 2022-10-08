// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.ElementPrice
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;

namespace Eco.Domain.v5
{
  public abstract class ElementPrice
  {
    private int? m_hash;
    private ElementPriceId m_compositeId;
    private Project m_project;

    public virtual ElementPriceId CompositeId => this.m_compositeId;

    public virtual Project Project => this.m_project;

    public virtual double Price { get; set; }

    public virtual bool IsTransient => this.m_compositeId == null;

    public override bool Equals(object obj)
    {
      if (!(obj is ElementPrice elementPrice) || this.GetType() != elementPrice.GetUnproxiedType() || this.IsTransient ^ elementPrice.IsTransient)
        return false;
      return this.IsTransient && elementPrice.IsTransient ? this == elementPrice : this.CompositeId.Equals((object) elementPrice.CompositeId);
    }

    public override int GetHashCode()
    {
      if (!this.m_hash.HasValue)
        this.m_hash = new int?(this.IsTransient ? base.GetHashCode() : this.CompositeId.GetHashCode());
      return this.m_hash.Value;
    }

    protected virtual Type GetUnproxiedType() => this.GetType();
  }
}
