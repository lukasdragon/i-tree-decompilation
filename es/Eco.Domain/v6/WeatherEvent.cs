// Decompiled with JetBrains decompiler
// Type: Eco.Domain.v6.WeatherEvent
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using Eco.Domain.Attributes;
using Eco.Domain.Properties;

namespace Eco.Domain.v6
{
  public enum WeatherEvent
  {
    [LocalizedDescription(typeof (v6Strings), "WeatherEvent_Storm")] Storm = 1,
    [LocalizedDescription(typeof (v6Strings), "WeatherEvent_Class1Huricane")] Class1Huricane = 2,
    [LocalizedDescription(typeof (v6Strings), "WeatherEvent_Class2Huricane")] Class2Huricane = 3,
    [LocalizedDescription(typeof (v6Strings), "WeatherEvent_Class3Huricane")] Class3Huricane = 4,
    [LocalizedDescription(typeof (v6Strings), "WeatherEvent_Class4Huricane")] Class4Huricane = 5,
    [LocalizedDescription(typeof (v6Strings), "WeatherEvent_Class5Huricane")] Class5Huricane = 6,
    [LocalizedDescription(typeof (v6Strings), "WeatherEvent_TropicalDepression")] TropicalDepression = 7,
    [LocalizedDescription(typeof (v6Strings), "WeatherEvent_TropicalStorm")] TropicalStorm = 8,
  }
}
