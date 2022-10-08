// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.YearOption
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.v6
{
  public class YearOption : Entity<YearOption>
  {
    private bool m_enabled;

    public YearOption()
    {
    }

    public YearOption(bool enabled) => this.m_enabled = enabled;

    public static implicit operator bool(YearOption o) => o.m_enabled;

    public static implicit operator YearOption(bool b) => new YearOption(b);
  }
}
