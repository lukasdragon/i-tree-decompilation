// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Reports.ErrorReport
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using C1.C1Preview;
using i_Tree_Eco_v6.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace i_Tree_Eco_v6.Reports
{
  public class ErrorReport : Report
  {
    public C1PrintDocument C1doc;

    public string ExceptionMsg { get; set; }

    public string InnerExceptionMsg { get; set; }

    public string StackTrace { get; set; }

    public bool IsExport { get; set; }

    public ErrorReport() => this.C1doc = new C1PrintDocument();

    public override C1PrintDocument GenerateReport(Graphics g)
    {
      RenderText ro1 = new RenderText(i_Tree_Eco_v6.Resources.Strings.ErrorReport);
      ro1.Style.Font = new Font("Calibri", 14f, FontStyle.Bold);
      ro1.Style.FontUnderline = true;
      this.C1doc.Body.Children.Add((RenderObject) ro1);
      RenderText renderText = new RenderText();
      RenderText ro2 = new RenderText(string.Format(i_Tree_Eco_v6.Resources.Strings.MsgErrorReport, this.IsExport ? (object) i_Tree_Eco_v6.Resources.Strings.Export : (object) i_Tree_Eco_v6.Resources.Strings.Report));
      ro2.Style.Spacing.Top = (Unit) "5mm";
      this.C1doc.Body.Children.Add((RenderObject) ro2);
      RenderTable ro3 = new RenderTable();
      ro3.Style.Spacing.Top = (Unit) "5mm";
      ro3.Style.Borders.All = LineDef.Default;
      ro3.Cols[0].Width = (Unit) "20%";
      ro3.Cols[0].Style.Borders.Right = LineDef.Default;
      int num1 = 0;
      ro3.Cells[num1, 0].Text = i_Tree_Eco_v6.Resources.Strings.ReportTitle;
      ro3.Cells[num1, 1].Text = this.ReportTitle;
      ro3.Rows[num1].Style.Borders.Bottom = LineDef.Default;
      int num2 = num1 + 1;
      ro3.Cells[num2, 0].Text = i_Tree_Eco_v6.Resources.Strings.ExceptionMessage;
      ro3.Cells[num2, 1].Text = this.ExceptionMsg;
      ro3.Rows[num2].Style.Borders.Bottom = LineDef.Default;
      int num3 = num2 + 1;
      if (!string.IsNullOrEmpty(this.InnerExceptionMsg))
      {
        ro3.Cells[num3, 0].Text = i_Tree_Eco_v6.Resources.Strings.InnerExceptionMessage;
        ro3.Cells[num3, 1].Text = this.InnerExceptionMsg;
        ro3.Rows[num3].Style.Borders.Bottom = LineDef.Default;
        ++num3;
      }
      ro3.Cells[num3, 0].Text = i_Tree_Eco_v6.Resources.Strings.StackTrace;
      ro3.Cells[num3, 1].Text = this.StackTrace;
      this.C1doc.Body.Children.Add((RenderObject) ro3);
      return this.C1doc;
    }

    public override C1PrintDocument ExportCSV(Graphics g, string csvFileName) => throw new NotImplementedException();

    public override Dictionary<string, object> GenerateMapData() => throw new NotImplementedException();

    public override bool CanExport(ExportFormat format) => false;

    public override void Export(ExportFormat format, string file) => throw new NotImplementedException();

    public override DataTable GetData() => throw new NotImplementedException();
  }
}
