// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.VideoLearningForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Resources;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class VideoLearningForm : CustomWebBrowserForm
  {
    private IContainer components;

    public VideoLearningForm() => this.InitializeComponent();

    protected override string Url => Strings.UrlVideoLearning;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.lblBreadcrumb.Size = new Size(284, 19);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(284, 261);
      this.Name = nameof (VideoLearningForm);
      this.Text = "Video Learning";
      this.ResumeLayout(false);
    }
  }
}
