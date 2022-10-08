// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.ConditionCategory
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.Properties;

namespace Eco.Domain.v6
{
  public enum ConditionCategory
  {
    [LocalizedDescription(typeof (v6Strings), "ConditionCategory_Excellent")] Excellent = 0,
    [LocalizedDescription(typeof (v6Strings), "ConditionCategory_Good")] Good = 10, // 0x0000000A
    [LocalizedDescription(typeof (v6Strings), "ConditionCategory_Fair")] Fair = 25, // 0x00000019
    [LocalizedDescription(typeof (v6Strings), "ConditionCategory_Poor")] Poor = 50, // 0x00000032
    [LocalizedDescription(typeof (v6Strings), "ConditionCategory_Critical")] Critical = 75, // 0x0000004B
    [LocalizedDescription(typeof (v6Strings), "ConditionCategory_Dying")] Dying = 99, // 0x00000063
    [LocalizedDescription(typeof (v6Strings), "ConditionCategory_Dead")] Dead = 100, // 0x00000064
  }
}
