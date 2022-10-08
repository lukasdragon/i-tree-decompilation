// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.StratumSAS
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

namespace i_Tree_Eco_v6.SAS
{
  internal class StratumSAS
  {
    public int ClassValue;
    public string Abbreviation;
    public string Description;
    public double TreeLeafAreaHectare;
    public double TreeCoverAreaHectare;
    public double ShrubLeafAreaHectare;
    public double ShrubCoverAreaHectare;

    public StratumSAS()
    {
      this.ClassValue = 0;
      this.Abbreviation = "";
      this.Description = "";
      this.TreeLeafAreaHectare = 0.0;
      this.TreeCoverAreaHectare = 0.0;
      this.ShrubLeafAreaHectare = 0.0;
      this.ShrubCoverAreaHectare = 0.0;
    }

    public void Copy(StratumSAS fromStratumSAS)
    {
      this.ClassValue = fromStratumSAS.ClassValue;
      this.Abbreviation = fromStratumSAS.Abbreviation;
      this.Description = fromStratumSAS.Description;
      this.TreeLeafAreaHectare = fromStratumSAS.TreeLeafAreaHectare;
      this.TreeCoverAreaHectare = fromStratumSAS.TreeCoverAreaHectare;
      this.ShrubLeafAreaHectare = fromStratumSAS.ShrubLeafAreaHectare;
      this.ShrubCoverAreaHectare = fromStratumSAS.ShrubCoverAreaHectare;
    }
  }
}
