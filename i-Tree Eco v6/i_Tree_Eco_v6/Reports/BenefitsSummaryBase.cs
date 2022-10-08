// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.BenefitsSummaryBase
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;
using System;
using System.ComponentModel;
using System.Text;

namespace i_Tree_Eco_v6.Reports
{
  [DesignerCategory("Code")]
  internal class BenefitsSummaryBase : DatabaseReport
  {
    public BenefitsSummaryBase() => this.ReportTitle = Strings.ReportTItleBenefitsSummaryBySpecies;

    protected override string ReportMessage()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(this.Carbon_MetricTon_ShortTon_Footer(Strings.NoteCarbonStorageAndgrossCarbonSequestrationValue));
      if (this.ProjectIsUsingTropicalEquations())
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(Strings.MsgTropicalEquations);
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Strings.NoteCarbonStorageLimit);
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(this.AvoidedRunoff_m3_ft3_Footer());
      if (this.pollutionIsAvailable)
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(this.PollutionRemoval_MetricTon_ShortTon_Footer());
      }
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(this.ReplacementValue_Footer());
      stringBuilder.Append(Environment.NewLine);
      stringBuilder.Append(Strings.NoteZeroValuesMeaning);
      return stringBuilder.ToString();
    }

    protected class PollutionRemoval
    {
      public double PollutionAmountCo;
      public double PollutionAmountO3;
      public double PollutionAmountNO2;
      public double PollutionAmountSO2;
      public double PollutionAmountPM25;
      public double PollutionAmountPM10;

      public double PollutionTotal => this.PollutionAmountCo + this.PollutionAmountO3 + this.PollutionAmountNO2 + this.PollutionAmountSO2 + this.PollutionAmountPM25 + this.PollutionAmountPM10;
    }

    protected class BenefitsSummary
    {
      public double NumTrees { get; set; }

      public double NumTreesSE { get; set; }

      public double CarbonStorage { get; set; }

      public double CarbonStorageSE { get; set; }

      public double GrossCarbonSeq { get; set; }

      public double GrossCarbonSeqSE { get; set; }

      public double AvoidedRunoff { get; set; }

      public BenefitsSummaryBase.PollutionRemoval PollutionRemoval { get; set; }

      public double StructuralValue { get; set; }

      public double StructuralValueSE { get; set; }
    }
  }
}
