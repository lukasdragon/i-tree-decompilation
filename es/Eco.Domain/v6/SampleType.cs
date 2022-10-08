// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.SampleType
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.Properties;

namespace Eco.Domain.v6
{
  public enum SampleType
  {
    [LocalizedDescription(typeof (v6Strings), "SampleType_RegularStreetTree")] RegularStreetTree = 66, // 0x00000042
    [LocalizedDescription(typeof (v6Strings), "SampleType_ClusterPlot")] ClusterPlot = 67, // 0x00000043
    [LocalizedDescription(typeof (v6Strings), "SampleType_UforeStreetTree")] UforeStreetTree = 70, // 0x00000046
    [LocalizedDescription(typeof (v6Strings), "SampleType_Inventory")] Inventory = 73, // 0x00000049
    [LocalizedDescription(typeof (v6Strings), "SampleType_MctiInventory")] MctiInventory = 77, // 0x0000004D
    [LocalizedDescription(typeof (v6Strings), "SampleType_RegularPlot")] RegularPlot = 80, // 0x00000050
    [LocalizedDescription(typeof (v6Strings), "SampleType_StratumStreetTree")] StratumStreetTree = 83, // 0x00000053
    [LocalizedDescription(typeof (v6Strings), "SampleType_ClusterStreetTree")] ClusterStreetTree = 84, // 0x00000054
    [LocalizedDescription(typeof (v6Strings), "SampleType_UforeInventory")] UforeInventory = 85, // 0x00000055
  }
}
