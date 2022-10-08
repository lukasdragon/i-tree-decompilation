// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.Chart.UVIndex
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;

namespace i_Tree_Eco_v6.Reports.Chart
{
  internal class UVIndex : SimpleDateValueChart
  {
    public UVIndex(string colName = "", string Y = "")
    {
      this.item1 = colName;
      string date = Strings.Date;
      this.XAxisTitle = date;
      this.YAxisTitle = Y;
      this.TableXTitle = date;
      this.TableYTitle = Y;
    }

    internal class UVIndexData : UVIndex
    {
      public UVIndexData()
        : base(nameof (UVIndex), Strings.UVIndexAtSolarNoon)
      {
        this.MainTitle = Strings.ReportTitleUVIndexData;
      }
    }

    internal class UVIndexReductionByTreesForUnderTrees : UVIndex
    {
      public UVIndexReductionByTreesForUnderTrees()
        : base("ShadedUVReduction", Strings.ReductionInUVIndexAtSolarNoon)
      {
        this.MainTitle = Strings.ReportTitleUVEffectsInTreeShade;
      }
    }

    internal class UVIndexReductionByTreesForOverall : UVIndex
    {
      public UVIndexReductionByTreesForOverall()
        : base("OverallUVReduction", Strings.ReductionInUVIndexAtSolarNoon)
      {
        this.MainTitle = Strings.ReportTitleUVEffectsOverall;
      }
    }
  }
}
