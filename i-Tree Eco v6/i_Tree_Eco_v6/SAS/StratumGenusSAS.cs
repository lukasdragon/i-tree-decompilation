// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.StratumGenusSAS
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

namespace i_Tree_Eco_v6.SAS
{
  internal class StratumGenusSAS
  {
    public int StratumClassValue;
    public string GenusScientificName;
    public string LeafType;
    public double TreeLeafBiomassKilograms;
    public double ShrubLeafBiomassKilograms;

    public StratumGenusSAS()
    {
      this.StratumClassValue = 0;
      this.GenusScientificName = "";
      this.LeafType = "DECIDUOUS";
      this.TreeLeafBiomassKilograms = 0.0;
      this.ShrubLeafBiomassKilograms = 0.0;
    }

    public void Copy(StratumGenusSAS fromStratumGenusSAS)
    {
      this.StratumClassValue = fromStratumGenusSAS.StratumClassValue;
      this.GenusScientificName = fromStratumGenusSAS.GenusScientificName;
      this.LeafType = fromStratumGenusSAS.LeafType;
      this.TreeLeafBiomassKilograms = fromStratumGenusSAS.TreeLeafBiomassKilograms;
      this.ShrubLeafBiomassKilograms = fromStratumGenusSAS.ShrubLeafBiomassKilograms;
    }
  }
}
