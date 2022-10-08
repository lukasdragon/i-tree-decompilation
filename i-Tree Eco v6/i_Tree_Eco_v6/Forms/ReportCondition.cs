// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReportCondition
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System.Collections.Generic;

namespace i_Tree_Eco_v6.Forms
{
  public class ReportCondition
  {
    public bool IsAvailable { get; set; }

    public List<ConditionBase> Conditions { get; set; }

    public bool Enabled
    {
      get
      {
        bool enabled = true;
        foreach (ConditionBase condition in this.Conditions)
        {
          if (!condition.Test())
          {
            enabled = false;
            condition.Result = false;
          }
          else
            condition.Result = true;
        }
        return enabled;
      }
    }
  }
}
