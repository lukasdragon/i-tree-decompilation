// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.ElementPrice`1
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v6
{
  public abstract class ElementPrice<T> : ElementPrice where T : ElementPrice<T>
  {
    public override object Clone() => (object) this.Clone(true);

    public override ElementPrice Clone(bool deep) => (ElementPrice) ElementPrice.Clone<T>((T) this, new EntityMap(), deep);
  }
}
