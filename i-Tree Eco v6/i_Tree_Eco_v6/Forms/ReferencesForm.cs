// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReferencesForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class ReferencesForm : DockContent
  {
    private IContainer components;
    private RichTextLabel rtlReferences;

    public ReferencesForm()
    {
      this.InitializeComponent();
      RichTextLabel rtlReferences = this.rtlReferences;
      Size clientSize = this.ClientSize;
      int width = clientSize.Width - 6;
      clientSize = this.ClientSize;
      int height = clientSize.Height;
      Size size = new Size(width, height);
      rtlReferences.Size = size;
      this.rtlReferences.Location = new Point(6, 0);
      this.rtlReferences.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    }

    private void ReferencesForm_Resize(object sender, EventArgs e)
    {
      if (this.rtlReferences.ClientSize.Width - (13 + SystemInformation.VerticalScrollBarWidth) < 0)
        return;
      this.rtlReferences.RightMargin = this.rtlReferences.ClientSize.Width - (13 + SystemInformation.VerticalScrollBarWidth);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ReferencesForm));
      this.rtlReferences = new RichTextLabel();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.rtlReferences, "rtlReferences");
      this.rtlReferences.Name = "rtlReferences";
      this.rtlReferences.TabStop = false;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.CloseButton = false;
      this.CloseButtonVisible = false;
      this.ControlBox = false;
      this.Controls.Add((Control) this.rtlReferences);
      this.DockAreas = DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight;
      this.Name = nameof (ReferencesForm);
      this.ShowHint = DockState.DockLeft;
      this.Resize += new EventHandler(this.ReferencesForm_Resize);
      this.ResumeLayout(false);
    }
  }
}
