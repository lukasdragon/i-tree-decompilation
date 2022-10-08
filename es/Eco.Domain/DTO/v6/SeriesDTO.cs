// Decompiled with JetBrains decompiler
// Type: Eco.Domain.DTO.v6.SeriesDTO
// Assembly: Eco.Domain, Version=10.0.17.6714, Culture=neutral, PublicKeyToken=null
// MVID: 01C26179-0456-4F8E-BC0F-741F177F6574
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\Eco.Domain.dll

namespace Eco.Domain.DTO.v6
{
  public class SeriesDTO : EntityDTO
  {
    public string Id;
    public string Comments;
    public float? DefaultPlotSize;
    public char DefaultPlotSizeUnit;
    public bool? IsPermanent;
    public int? SampleMethod;
    public char SampleType;
    public ProjectDTO Project;
  }
}
