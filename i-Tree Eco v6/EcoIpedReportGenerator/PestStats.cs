// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.PestStats
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

namespace EcoIpedReportGenerator
{
  public class PestStats
  {
    public int TreeCount;
    public double PopEst;
    public double StdErr;
    public double Pct;
    public double PctAffected;
    public double PctTree;

    public PestStats()
    {
    }

    public PestStats(
      int PopulationEstimate,
      double SE,
      double Percent,
      double PercentTrees,
      double PercentPestAffected)
    {
      this.PopEst = (double) PopulationEstimate;
      this.StdErr = SE;
      this.Pct = Percent;
      this.PctTree = PercentTrees;
      this.PctAffected = PercentPestAffected;
    }
  }
}
