// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v5.ElementPriceId
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v5
{
  public class ElementPriceId : ProjectId
  {
    private string m_element;

    public virtual string Element => this.m_element;

    public override bool Equals(object obj)
    {
      if (this == obj)
        return true;
      return obj is ElementPriceId elementPriceId && !(this.GetType() != elementPriceId.GetType()) && !(this.Location != elementPriceId.Location) && !(this.Series != elementPriceId.Series) && (int) this.Year != (int) elementPriceId.Year;
    }

    public override int GetHashCode()
    {
      int num = 43;
      return ((num * this.Location.GetHashCode() * num + this.Series.GetHashCode()) * num + this.Year.GetHashCode()) * num + this.Element.GetHashCode();
    }
  }
}
