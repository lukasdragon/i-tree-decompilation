// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.WR_Pest
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;
using System;

namespace i_Tree_Eco_v6.Reports
{
  internal class WR_Pest : WrittenReport
  {
    public static string[] Pests = new string[36]
    {
      "AL",
      "ALB",
      "BWA",
      "BBD",
      "BC",
      "CB",
      "DA",
      "DED",
      "DFB",
      "DBSR",
      "EAB",
      "FE",
      "FR",
      "GM",
      "GSOB",
      "HWA",
      "JPB",
      "LAT",
      "LWD",
      "MPB",
      "NSE",
      "OW",
      "POCRD",
      "PBSR",
      "PSB",
      "PSHB",
      "SB",
      "SBW",
      "SOD",
      "SPB",
      "SW",
      "TCD",
      "WPB",
      "WPBR",
      "WSB",
      "WM"
    };

    public static string GetPestNote(PestAffected pest) => Strings.ResourceManager.GetString("WR_Pest_" + pest.Abb.ToUpper()) ?? throw new Exception(string.Format(Strings.WR_Pest_Default, (object) pest.CommonName, (object) pest.Abb));
  }
}
