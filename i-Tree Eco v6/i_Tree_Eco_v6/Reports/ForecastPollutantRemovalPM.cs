// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ForecastPollutantRemovalPM
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;
using System.ComponentModel;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  public class ForecastPollutantRemovalPM : ForecastPollutantRemoval
  {
    public ForecastPollutantRemovalPM()
      : base("PM2.5", Strings.ParticulateMatter25Furmula)
    {
    }
  }
}
