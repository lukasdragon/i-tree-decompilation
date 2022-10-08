// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v5.ProjectDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System.Collections.Generic;

namespace Eco.Domain.DTO.v5
{
  public class ProjectDTO
  {
    public SeriesDTO Series;
    public List<PlotDTO> Plots;
    public List<MapLandUseDTO> MapLandUses;
    public List<CoverTypeDTO> CoverTypes;
    public int Id;
    public char Unit;
    public bool IsInitialMeasurement;
    public bool RecordHydro;
    public bool RecordShrub;
    public bool RecordEnergy;
    public bool RecordPlantableSpace;
    public bool RecordCLE;
    public bool RecordIPED;
  }
}
