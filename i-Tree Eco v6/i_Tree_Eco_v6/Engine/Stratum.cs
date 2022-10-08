// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Engine.Stratum
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

namespace i_Tree_Eco_v6.Engine
{
  internal class Stratum
  {
    public int ClassValue;
    public string Abbreviation;
    public string Description;
    public double AreaHectare;
    public double AreaHectareBasedEngineInventoryDB;
    public double TreeCoverPercentageBasedEngineInventoryDB;
    public double ShrubCoverPercentageBasedEngineInventoryDB;
    public double TreeLeafAreaHectare;
    public double ShrubLeafAreaHectare;
    public double TreeLAI;
    public double ShrubLAI;
    public double sumPlotAreaHectare;
    public double sumPlotTreeCoverSquareMeters;
    public double sumPlotTreeCoverEvergreenSquareMeters;
    public double sumPlotShrubCoverSquareMeters;
    public double sumPlotShrubEvergreenSquareMeters;

    public Stratum()
    {
      this.ClassValue = 0;
      this.Abbreviation = "";
      this.Description = "";
      this.AreaHectare = 0.0;
      this.AreaHectareBasedEngineInventoryDB = 0.0;
      this.TreeCoverPercentageBasedEngineInventoryDB = 0.0;
      this.ShrubCoverPercentageBasedEngineInventoryDB = 0.0;
      this.TreeLeafAreaHectare = 0.0;
      this.ShrubLeafAreaHectare = 0.0;
      this.TreeLAI = 0.0;
      this.ShrubLAI = 0.0;
      this.sumPlotAreaHectare = 0.0;
      this.sumPlotTreeCoverSquareMeters = 0.0;
      this.sumPlotTreeCoverEvergreenSquareMeters = 0.0;
      this.sumPlotShrubCoverSquareMeters = 0.0;
      this.sumPlotShrubEvergreenSquareMeters = 0.0;
    }

    public void Copy(Stratum fromStratum)
    {
      this.ClassValue = fromStratum.ClassValue;
      this.Abbreviation = fromStratum.Abbreviation;
      this.Description = fromStratum.Description;
      this.AreaHectare = fromStratum.AreaHectare;
      this.AreaHectareBasedEngineInventoryDB = fromStratum.AreaHectareBasedEngineInventoryDB;
      this.TreeCoverPercentageBasedEngineInventoryDB = fromStratum.TreeCoverPercentageBasedEngineInventoryDB;
      this.ShrubCoverPercentageBasedEngineInventoryDB = fromStratum.ShrubCoverPercentageBasedEngineInventoryDB;
      this.TreeLeafAreaHectare = fromStratum.TreeLeafAreaHectare;
      this.ShrubLeafAreaHectare = fromStratum.ShrubLeafAreaHectare;
      this.TreeLAI = fromStratum.TreeLAI;
      this.ShrubLAI = fromStratum.ShrubLAI;
      this.sumPlotAreaHectare = fromStratum.sumPlotAreaHectare;
      this.sumPlotTreeCoverSquareMeters = fromStratum.sumPlotTreeCoverSquareMeters;
      this.sumPlotTreeCoverEvergreenSquareMeters = fromStratum.sumPlotTreeCoverEvergreenSquareMeters;
      this.sumPlotShrubCoverSquareMeters = fromStratum.sumPlotShrubCoverSquareMeters;
      this.sumPlotShrubEvergreenSquareMeters = fromStratum.sumPlotShrubEvergreenSquareMeters;
    }
  }
}
