// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.CrownLightExposure
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.Properties;

namespace Eco.Domain.v6
{
  public enum CrownLightExposure
  {
    [LocalizedDescription(typeof (v6Strings), "CrownLightExposure_NotEntered")] NotEntered = -1, // 0xFFFFFFFF
    [LocalizedDescription(typeof (v6Strings), "CrownLightExposure_NoSides")] NoSides = 0,
    [LocalizedDescription(typeof (v6Strings), "CrownLightExposure_OneSide")] OneSide = 1,
    [LocalizedDescription(typeof (v6Strings), "CrownLightExposure_TwoSides")] TwoSides = 2,
    [LocalizedDescription(typeof (v6Strings), "CrownLightExposure_ThreeSides")] ThreeSides = 3,
    [LocalizedDescription(typeof (v6Strings), "CrownLightExposure_FourSides")] FourSides = 4,
    [LocalizedDescription(typeof (v6Strings), "CrownLightExposure_FiveSides")] FiveSides = 5,
  }
}
