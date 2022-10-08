// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.TreeConditionBySpecies
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Core;
using Eco.Domain.v6;
using Eco.Util;
using System;

namespace i_Tree_Eco_v6.Reports
{
  internal class TreeConditionBySpecies : TreeConditionBase
  {
    public TreeConditionBySpecies()
    {
      this.ReportTitle = i_Tree_Eco_v6.Resources.Strings.ReportTitleConditionOfTreesBySpecies;
      this.diebackClass = Classifiers.Dieback;
      this.diebackOdersAndDescriptions = this.GetSortedDiebackOdersWithDescriptions();
    }

    protected override string GetTranslatedName(string name) => EnumHelper.GetDescription<ConditionCategory>((ConditionCategory) Enum.Parse(typeof (ConditionCategory), name));
  }
}
