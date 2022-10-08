// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.Entity`1
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v6
{
  public abstract class Entity<T> : Entity where T : Entity
  {
    public virtual T Clone(bool deep) => this.Clone() as T;

    public virtual T Self => this as T;
  }
}
