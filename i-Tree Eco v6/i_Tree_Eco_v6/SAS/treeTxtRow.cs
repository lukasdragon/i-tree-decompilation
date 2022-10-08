// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.SAS.treeTxtRow
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Domain.v6;
using System;

namespace i_Tree_Eco_v6.SAS
{
  internal class treeTxtRow
  {
    public int PlotId { get; set; }

    public int TreeId { get; set; }

    public Guid TreeKey { get; set; }

    public string Tree_Species { get; set; }

    public float TreeHeightTotal { get; set; }

    public Crown Tree_Crown { get; set; }

    public bool Tree_StreetTree { get; set; }

    public char EcoLanduses_LandUseCode { get; set; }

    public double EcoConditions_PctDieback { get; set; }

    public int EcoConditions_ConditionId { get; set; }
  }
}
