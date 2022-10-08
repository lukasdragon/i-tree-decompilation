// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ResolveConflictForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ResolveConflictForm : Form
  {
    private Resolution _resolution;
    private IContainer components;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnUseMine;
    private Button btnUseTheirs;
    private Panel panel1;
    private Label lblMessage;
    private Button btnAbort;

    public ResolveConflictForm(string message)
    {
      this.InitializeComponent();
      this.lblMessage.Text = message;
      this._resolution = Resolution.Abort;
    }

    public Resolution Resolution
    {
      get => this._resolution;
      private set
      {
        this._resolution = value;
        this.Close();
      }
    }

    private void btnUseTheirs_Click(object sender, EventArgs e) => this.Resolution = Resolution.UseTheirs;

    private void btnUseMine_Click(object sender, EventArgs e) => this.Resolution = Resolution.UseMine;

    private void btnAbort_Click(object sender, EventArgs e) => this.Resolution = Resolution.Abort;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ResolveConflictForm));
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnAbort = new Button();
      this.btnUseMine = new Button();
      this.btnUseTheirs = new Button();
      this.panel1 = new Panel();
      this.lblMessage = new Label();
      this.flowLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.flowLayoutPanel1, "flowLayoutPanel1");
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAbort);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnUseMine);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnUseTheirs);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.btnAbort, "btnAbort");
      this.btnAbort.Name = "btnAbort";
      this.btnAbort.UseVisualStyleBackColor = true;
      this.btnAbort.Click += new EventHandler(this.btnAbort_Click);
      componentResourceManager.ApplyResources((object) this.btnUseMine, "btnUseMine");
      this.btnUseMine.Name = "btnUseMine";
      this.btnUseMine.UseVisualStyleBackColor = true;
      this.btnUseMine.Click += new EventHandler(this.btnUseMine_Click);
      componentResourceManager.ApplyResources((object) this.btnUseTheirs, "btnUseTheirs");
      this.btnUseTheirs.Name = "btnUseTheirs";
      this.btnUseTheirs.UseVisualStyleBackColor = true;
      this.btnUseTheirs.Click += new EventHandler(this.btnUseTheirs_Click);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Controls.Add((Control) this.lblMessage);
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.lblMessage, "lblMessage");
      this.lblMessage.Name = "lblMessage";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ResolveConflictForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.TopMost = true;
      this.flowLayoutPanel1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
