// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.ReferenceObjectType
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.Properties;

namespace Eco.Domain.v6
{
  public enum ReferenceObjectType
  {
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_Unknown")] Unknown = -1, // 0xFFFFFFFF
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_Tree")] Tree = 1,
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_BuildingCorner")] BuildingCorner = 2,
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_BuildingWall")] BuildingWall = 3,
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_BuildingDoor")] BuildingDoor = 4,
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_RoofPeak")] RoofPeak = 5,
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_LightPost")] LightPost = 6,
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_StreetSign")] StreetSign = 7,
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_Fence")] Fence = 8,
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_SwingSet")] SwingSet = 9,
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_Curb")] Curb = 10, // 0x0000000A
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_DrivewayEdge")] DrivewayEdge = 11, // 0x0000000B
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_FireHydrant")] FireHydrant = 12, // 0x0000000C
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_UtilityBox")] UtilityBox = 13, // 0x0000000D
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_SewerOrDrainGrate")] SewerOrDrainGrate = 14, // 0x0000000E
    [LocalizedDescription(typeof (v6Strings), "ReferenceObjectType_Other")] Other = 15, // 0x0000000F
  }
}
