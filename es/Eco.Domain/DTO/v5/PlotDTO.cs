// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v5.PlotDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System;
using System.Collections.Generic;

namespace Eco.Domain.DTO.v5
{
  public class PlotDTO
  {
    public int Id;
    public List<SubPlotDTO> SubPlots;
    public string Address;
    public double? Latitude;
    public double? Longitude;
    public int MapLandUse;
    public DateTime? Date;
    public string Crew;
    public string Comments;
    public State State;
    public string ContactInfo;
    public long Revision;
  }
}
