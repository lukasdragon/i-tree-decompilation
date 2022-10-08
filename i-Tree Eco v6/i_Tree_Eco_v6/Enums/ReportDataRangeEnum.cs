// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Enums.ReportDataRangeEnum
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.Properties;
using System.ComponentModel;

namespace i_Tree_Eco_v6.Enums
{
  public enum ReportDataRangeEnum
  {
    [LocalizedDescription(typeof (v6Strings), "Millionth")] Millionth = -6, // 0xFFFFFFFA
    [LocalizedDescription(typeof (v6Strings), "Thousandth")] Thousandth = -3, // 0xFFFFFFFD
    [Description("")] Singles = 0,
    [LocalizedDescription(typeof (v6Strings), "Thousand")] Thousands = 3,
    [LocalizedDescription(typeof (v6Strings), "Million")] Millions = 6,
    [LocalizedDescription(typeof (v6Strings), "Billion")] Billions = 9,
    [LocalizedDescription(typeof (v6Strings), "Trillion")] Trillions = 12, // 0x0000000C
  }
}
