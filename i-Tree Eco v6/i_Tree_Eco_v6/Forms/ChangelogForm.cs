// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ChangelogForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Forms.Resources;
using System.ComponentModel;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ChangelogForm : CustomWebBrowserForm
  {
    private IContainer components;

    public ChangelogForm() => this.InitializeComponent();

    protected override string Url => string.Format(UserGuideRes.ViewPdfURL, (object) UserGuideRes.ChangeLogURL);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Text = nameof (ChangelogForm);
    }
  }
}
