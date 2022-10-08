// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.COutput
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util.Views;
using i_Tree_Eco_v6;
using System.Collections.Generic;

namespace EcoIpedReportGenerator
{
  public class COutput
  {
    public int m_dbhRange;

    protected void IncTreeCount<KeyType>(
      Dictionary<KeyType, Dictionary<int, Dictionary<int, int>>> dict,
      bool condition,
      KeyType key,
      int zone,
      int segment)
    {
      Dictionary<int, Dictionary<int, int>> dictionary1 = (Dictionary<int, Dictionary<int, int>>) null;
      if (dict.TryGetValue(key, out dictionary1))
        return;
      Dictionary<int, Dictionary<int, int>> dictionary2 = new Dictionary<int, Dictionary<int, int>>();
      dict[key] = dictionary2;
    }

    protected string GetSpeciesName(string SpCode)
    {
      if (SpCode == null)
        return string.Empty;
      ProgramSession instance = ProgramSession.GetInstance();
      bool flag = instance.SpeciesDisplayName == SpeciesDisplayEnum.CommonName;
      SpeciesView speciesView;
      if (!instance.Species.TryGetValue(SpCode, out speciesView))
        return SpCode.ToUpper();
      return !flag ? speciesView.ScientificName : speciesView.CommonName;
    }

    protected void IncDictValue<KeyType>(
      Dictionary<KeyType, int> dict,
      bool condition,
      KeyType key)
    {
      int num = 0;
      dict.TryGetValue(key, out num);
      if (condition)
        ++num;
      dict[key] = num;
    }
  }
}
