// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.Report`1
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using i_Tree_Eco_v6.Enums;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class Report<T> : Report where T : ReportBase, new()
  {
    private ReportBase _reportObj;
    public DataTable _data;

    public override DataTable GetData()
    {
      if (this._data != null)
        return this._data;
      this._data = this.ReportObj.GetData() as DataTable;
      return this._data;
    }

    public Report() => this.HelpTopic = typeof (T).Name;

    public override string HelpTopic => !string.IsNullOrEmpty(this.ReportObj.HelpTopic) ? this.ReportObj.HelpTopic : base.HelpTopic;

    public override bool hasConvertableUnits => this.ReportObj.hasConvertableUnits;

    public override bool hasSpecies => this.ReportObj.hasSpecies;

    public override bool hasCoordinates => this.ReportObj.hasCoordinates;

    public override bool hasComments => this.ReportObj.hasComments;

    public override bool hasUID => this.ReportObj.hasUID;

    public override bool hasZeros => this.ReportObj.hasZeros;

    public override string ReportTitle => this.ReportObj.ReportTitle;

    public ReportBase ReportObj
    {
      get
      {
        if (this._reportObj == null)
          this._reportObj = (ReportBase) new T();
        return this._reportObj;
      }
    }

    public override C1PrintDocument GenerateReport(Graphics g)
    {
      ReportBase reportObj = this.ReportObj;
      C1PrintDocument C1doc = new C1PrintDocument();
      reportObj.InitDocument(C1doc);
      reportObj.RenderHeader(C1doc);
      reportObj.RenderBody(C1doc, g);
      reportObj.RenderFooter(C1doc);
      return C1doc;
    }

    public override C1PrintDocument ExportCSV(Graphics g, string csvFileName)
    {
      ReportBase reportObj = this.ReportObj;
      C1PrintDocument C1doc = new C1PrintDocument();
      reportObj.InitDocument(C1doc);
      reportObj.RenderHeader(C1doc);
      reportObj.ExportCSVandRenderBody(C1doc, g, csvFileName);
      reportObj.RenderFooter(C1doc);
      return C1doc;
    }

    public override Dictionary<string, object> GenerateMapData()
    {
      ReportBase reportObj = this.ReportObj;
      DataTable data = reportObj.GetData() as DataTable;
      List<ColumnFormat> columnFormatList = (List<ColumnFormat>) null;
      if (data != null)
        columnFormatList = reportObj.ColumnsFormat(data);
      return new Dictionary<string, object>()
      {
        {
          "data",
          (object) data
        },
        {
          "data_format",
          (object) columnFormatList
        }
      };
    }

    public override void Export(ExportFormat format, string file)
    {
      if (!this.CanExport(format))
        return;
      if (format != ExportFormat.CSV)
      {
        if (format != ExportFormat.KML)
          return;
        this.ReportObj.ExportKML(file);
      }
      else
        this.ReportObj.ExportCSV(file);
    }

    public override bool CanExport(ExportFormat format)
    {
      object data = (object) this.GetData();
      switch (format)
      {
        case ExportFormat.CSV:
          return data is DataTable && ((DataTable) data).Rows.Count > 0 && this.ReportObj.ColumnsFormat((DataTable) data) != null;
        case ExportFormat.KML:
          return data is DataTable && ((DataTable) data).Rows.Count > 0 && this.ReportObj.ColumnsFormat((DataTable) data) != null && this.hasCoordinates;
        default:
          return false;
      }
    }
  }
}
