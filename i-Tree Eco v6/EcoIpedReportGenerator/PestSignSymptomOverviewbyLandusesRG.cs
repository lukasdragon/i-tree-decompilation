// Decompiled with JetBrains decompiler
// Type: EcoIpedReportGenerator.PestSignSymptomOverviewbyLandusesRG
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using i_Tree_Eco_v6.Reports;
using System.Collections.Generic;
using System.Drawing;

namespace EcoIpedReportGenerator
{
  internal class PestSignSymptomOverviewbyLandusesRG : ReportGenerator, IBindable
  {
    private List<DataBinding> m_bindings = new List<DataBinding>();
    private PestData m_output;

    public List<DataBinding> Bindings() => this.m_bindings;

    public PestSignSymptomOverviewbyLandusesRG() => this.m_output = new PestData();

    public override object Generate()
    {
      PestSignSymptomOverviewbyLanduses overviewbyLanduses = new PestSignSymptomOverviewbyLanduses(staticData.IsSample);
      C1PrintDocument c1PrintDocument = new C1PrintDocument();
      C1PrintDocument C1doc = c1PrintDocument;
      overviewbyLanduses.RenderBody(C1doc, (Graphics) null);
      return (object) c1PrintDocument;
    }
  }
}
