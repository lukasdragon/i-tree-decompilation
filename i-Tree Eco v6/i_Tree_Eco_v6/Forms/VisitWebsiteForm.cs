// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.VisitWebsiteForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class VisitWebsiteForm : CustomWebBrowserForm
  {
    private IContainer components;

    public VisitWebsiteForm() => this.InitializeComponent();

    protected override string Url => Strings.UrlWebsite;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(284, 29);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(284, 261);
      this.Name = nameof (VisitWebsiteForm);
      this.Text = "i-Tree Website";
      this.ResumeLayout(false);
    }
  }
}
