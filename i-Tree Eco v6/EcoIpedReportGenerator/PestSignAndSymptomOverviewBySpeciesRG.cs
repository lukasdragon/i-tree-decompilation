// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.PestSignAndSymptomOverviewBySpeciesRG
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using i_Tree_Eco_v6.Reports;
using System.Collections.Generic;
using System.Drawing;

namespace EcoIpedReportGenerator
{
  internal class PestSignAndSymptomOverviewBySpeciesRG : ReportGenerator, IBindable
  {
    private List<DataBinding> m_bindings = new List<DataBinding>();

    public List<DataBinding> Bindings() => this.m_bindings;

    public override object Generate()
    {
      C1PrintDocument C1doc = new C1PrintDocument();
      (staticData.IsSample ? (ReportBase) new PestSignAndSymptomOverviewBySpeciesSample() : (ReportBase) new PestSignAndSymptomOverviewBySpeciesInventory()).RenderBody(C1doc, (Graphics) null);
      return (object) C1doc;
    }
  }
}
