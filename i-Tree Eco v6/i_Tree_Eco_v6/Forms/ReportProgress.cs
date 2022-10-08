// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReportProgress
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System.ComponentModel;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ReportProgress : Form
  {
    private IContainer components;
    private Label label1;

    public ReportProgress(string message = "")
    {
      this.InitializeComponent();
      if (!(message != string.Empty))
        return;
      this.label1.Text = message;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReportProgress));
      this.label1 = new Label();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add((Control) this.label1);
      this.Cursor = Cursors.WaitCursor;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (ReportProgress);
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
