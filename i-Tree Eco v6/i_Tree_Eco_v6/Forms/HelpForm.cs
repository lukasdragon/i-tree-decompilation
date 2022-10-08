// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.HelpForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using i_Tree_Eco_v6.Forms.Resources;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace i_Tree_Eco_v6.Forms
{
  public class HelpForm : DockContent
  {
    private IContainer components;
    private RichTextLabel rtlHelp;
    private Panel pnlHelp;

    public HelpForm() => this.InitializeComponent();

    internal void DisplayHelp(string topic)
    {
      ResourceManager resourceManager = ApplicationHelp.ResourceManager;
      this.rtlHelp.RichText = resourceManager.GetString(topic) ?? resourceManager.GetString("defaultDummyHelp");
      this.rtlHelp.Select(this.rtlHelp.TextLength, 0);
      this.rtlHelp.SelectedRtf = resourceManager.GetString("_footer");
      this.rtlHelp.Select(0, 0);
    }

    private void rtlHelp_LinkClicked(object sender, LinkClickedEventArgs e)
    {
      try
      {
        Process.Start(e.LinkText);
      }
      catch (Win32Exception ex)
      {
        int num = (int) MessageBox.Show((IWin32Window) this, string.Format(i_Tree_Eco_v6.Resources.Strings.ErrOpenRTFLink, (object) ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (HelpForm));
      this.rtlHelp = new RichTextLabel();
      this.pnlHelp = new Panel();
      this.pnlHelp.SuspendLayout();
      this.SuspendLayout();
      this.rtlHelp.Cursor = Cursors.Default;
      componentResourceManager.ApplyResources((object) this.rtlHelp, "rtlHelp");
      this.rtlHelp.Name = "rtlHelp";
      this.rtlHelp.TabStop = false;
      this.rtlHelp.LinkClicked += new LinkClickedEventHandler(this.rtlHelp_LinkClicked);
      this.pnlHelp.Controls.Add((Control) this.rtlHelp);
      componentResourceManager.ApplyResources((object) this.pnlHelp, "pnlHelp");
      this.pnlHelp.Name = "pnlHelp";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.CloseButton = false;
      this.CloseButtonVisible = false;
      this.ControlBox = false;
      this.Controls.Add((Control) this.pnlHelp);
      this.DockAreas = DockAreas.DockLeft;
      this.Name = nameof (HelpForm);
      this.ShowHint = DockState.DockLeft;
      this.pnlHelp.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
