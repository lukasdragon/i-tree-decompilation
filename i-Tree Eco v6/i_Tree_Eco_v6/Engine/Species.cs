// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Engine.Species
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

namespace i_Tree_Eco_v6.Engine
{
  internal class Species
  {
    public string SppCode;
    public int SppID;
    public string SpeciesCommonName;
    public string SpeciesShortScientificName;
    public string SpeciesScientificName;
    public string LeafType;
    public string GenusCode;
    public string GenusScientificName;
    public string GenusCommonName;
    public string NativeToState;

    public void copyFrom(Species fromSpecies)
    {
      this.SppCode = fromSpecies.SppCode;
      this.SppID = fromSpecies.SppID;
      this.SpeciesCommonName = fromSpecies.SpeciesCommonName;
      this.SpeciesShortScientificName = fromSpecies.SpeciesShortScientificName;
      this.SpeciesScientificName = fromSpecies.SpeciesScientificName;
      this.LeafType = fromSpecies.LeafType;
      this.GenusCode = fromSpecies.GenusCode;
      this.GenusScientificName = fromSpecies.GenusScientificName;
      this.GenusCommonName = fromSpecies.GenusCommonName;
      this.NativeToState = fromSpecies.NativeToState;
    }
  }
}
