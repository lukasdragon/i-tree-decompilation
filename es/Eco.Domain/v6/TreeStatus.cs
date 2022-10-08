// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.TreeStatus
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.Properties;

namespace Eco.Domain.v6
{
  public enum TreeStatus
  {
    [LocalizedDescription(typeof (v6Strings), "TreeStatus_HealthyRemoved")] HealthyRemoved = 67, // 0x00000043
    [LocalizedDescription(typeof (v6Strings), "TreeStatus_HazardRemoved")] HazardRemoved = 72, // 0x00000048
    [LocalizedDescription(typeof (v6Strings), "TreeStatus_Ingrowth")] Ingrowth = 73, // 0x00000049
    [LocalizedDescription(typeof (v6Strings), "TreeStatus_LandUseChangeRemoved")] LandUseChangeRemoved = 76, // 0x0000004C
    [LocalizedDescription(typeof (v6Strings), "TreeStatus_NoChange")] NoChange = 78, // 0x0000004E
    [LocalizedDescription(typeof (v6Strings), "TreeStatus_InitialSample")] InitialSample = 79, // 0x0000004F
    [LocalizedDescription(typeof (v6Strings), "TreeStatus_Planted")] Planted = 80, // 0x00000050
    [LocalizedDescription(typeof (v6Strings), "TreeStatus_UnknownRemoved")] UnknownRemoved = 82, // 0x00000052
    [LocalizedDescription(typeof (v6Strings), "TreeStatus_Unknown")] Unknown = 85, // 0x00000055
  }
}
