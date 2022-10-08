// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.Report
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using i_Tree_Eco_v6.Enums;
using i_Tree_Eco_v6.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public abstract class Report : IExportable
  {
    public Report()
    {
      this.ReportTitle = string.Empty;
      this.hasConvertableUnits = true;
      this.hasSpecies = true;
      this.hasCoordinates = false;
      this.hasComments = false;
    }

    public virtual string ReportTitle { get; set; }

    public virtual string HelpTopic { get; protected set; }

    public abstract C1PrintDocument GenerateReport(Graphics g);

    public abstract C1PrintDocument ExportCSV(Graphics g, string csvFileName);

    public abstract DataTable GetData();

    public abstract Dictionary<string, object> GenerateMapData();

    public abstract bool CanExport(ExportFormat format);

    public abstract void Export(ExportFormat format, string file);

    public virtual bool hasConvertableUnits { get; protected set; }

    public virtual bool hasSpecies { get; protected set; }

    public virtual bool hasCoordinates { get; protected set; }

    public virtual bool hasComments { get; protected set; }

    public virtual bool hasUID { get; protected set; }

    public virtual bool hasZeros { get; protected set; }
  }
}
