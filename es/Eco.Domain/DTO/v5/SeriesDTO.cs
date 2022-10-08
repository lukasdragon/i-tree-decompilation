// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v5.SeriesDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

using System.Collections.Generic;

namespace Eco.Domain.DTO.v5
{
  public class SeriesDTO
  {
    public LocationDTO Location;
    public List<ProjectDTO> Projects;
    public string Id;
    public bool IsPermanent;
    public char SampleType;
    public int SampleMethod;
    public float DefaultSubplotSize;
    public char DefaultSubplotSizeUnit;
    public int NextAvailabPlotID;
    public string GISProjection;
    public short GISUnit;
    public string Comments;
  }
}
