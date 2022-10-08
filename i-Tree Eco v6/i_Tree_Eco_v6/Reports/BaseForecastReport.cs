// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.BaseForecastReport
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class BaseForecastReport : IReport
  {
    private bool _byTotal;

    public BaseForecastReport(bool byTotal) => this._byTotal = byTotal;

    public virtual C1PrintDocument GenerateReport(
      Graphics FormGraphics,
      bool EnglishUnits,
      bool ScientificName,
      bool byTotal)
    {
      return new C1PrintDocument();
    }
  }
}
