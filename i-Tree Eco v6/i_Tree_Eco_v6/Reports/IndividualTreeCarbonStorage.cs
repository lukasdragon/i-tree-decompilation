// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.IndividualTreeCarbonStorage
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;
using System;
using System.Text;

namespace i_Tree_Eco_v6.Reports
{
  public class IndividualTreeCarbonStorage : IndividualTreeCarbonBase
  {
    public IndividualTreeCarbonStorage()
    {
      this.ReportTitle = Strings.ReportTitleCarbonStorageOfMeasuredTrees;
      this._column = "CarbonStorage";
      this.hasComments = true;
      this.hasCoordinates = this.curYear.RecordTreeGPS;
      this.hasUID = this.curYear.RecordTreeUserId;
    }

    protected override string GetColumnTitle() => ReportUtil.FormatInlineHeaderUnitsStr(Strings.CarbonStorage, ReportBase.KgUnits());

    protected override string ReportMessage()
    {
      StringBuilder stringBuilder = new StringBuilder(Strings.NoteCarbonStorageLimit);
      if (this.ProjectIsUsingTropicalEquations())
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(Strings.MsgTropicalEquations);
      }
      return stringBuilder.ToString();
    }
  }
}
