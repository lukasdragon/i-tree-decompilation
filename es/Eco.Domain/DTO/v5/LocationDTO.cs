// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v5.LocationDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System.Collections.Generic;

namespace Eco.Domain.DTO.v5
{
  public class LocationDTO
  {
    public string Id;
    public List<SeriesDTO> Series;
    public string NationCode;
    public string StateCode;
    public string CountyCode;
    public string CityCode;
    public int WeatherYear;
    public string WeatherStationID;
    public string ShellVersion;
    public string Comments;
  }
}
