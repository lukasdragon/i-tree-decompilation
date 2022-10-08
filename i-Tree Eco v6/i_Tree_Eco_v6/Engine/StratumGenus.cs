// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Engine.StratumGenus
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

namespace i_Tree_Eco_v6.Engine
{
  internal class StratumGenus
  {
    public int StratumClassValue;
    public string StratumAbbreviation;
    public string StratumDescription;
    public string GenusScientificName;
    public string LeafType;
    public double LeafBiomassKilograms;
    public string Form_Ind;

    public StratumGenus()
    {
      this.StratumClassValue = 0;
      this.StratumAbbreviation = "";
      this.StratumDescription = "";
      this.GenusScientificName = "";
      this.LeafType = "DECIDUOUS";
      this.LeafBiomassKilograms = 0.0;
      this.Form_Ind = "TREE";
    }

    public void Copy(StratumGenus fromStratumGenus)
    {
      this.StratumClassValue = fromStratumGenus.StratumClassValue;
      this.StratumAbbreviation = fromStratumGenus.StratumAbbreviation;
      this.StratumDescription = fromStratumGenus.StratumDescription;
      this.GenusScientificName = fromStratumGenus.GenusScientificName;
      this.LeafType = fromStratumGenus.LeafType;
      this.LeafBiomassKilograms = fromStratumGenus.LeafBiomassKilograms;
      this.Form_Ind = fromStratumGenus.Form_Ind;
    }
  }
}
