// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ExistingPlotsForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using i_Tree_Eco_v6.Enums;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ExistingPlotsForm : Form
  {
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel1;
    private Label label1;
    private RadioButton rdoDelete;
    private RadioButton rdoMerge;
    private Button btnCancel;
    private Button btnOK;
    private Panel panel1;

    public ExistingPlotsForm() => this.InitializeComponent();

    private void rdoOption_CheckedChanged(object sender, EventArgs e) => this.btnOK.Enabled = this.rdoDelete.Checked || this.rdoMerge.Checked;

    public PlotImportActionEnum ImportAction { get; private set; }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoDelete.Checked)
        this.ImportAction = PlotImportActionEnum.Delete;
      else if (this.rdoMerge.Checked)
        this.ImportAction = PlotImportActionEnum.Merge;
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ExistingPlotsForm));
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.label1 = new Label();
      this.panel1 = new Panel();
      this.rdoMerge = new RadioButton();
      this.rdoDelete = new RadioButton();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnOK, 1, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.btnCancel, 2, 2);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.label1, 3);
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.tableLayoutPanel1.SetColumnSpan((Control) this.panel1, 3);
      this.panel1.Controls.Add((Control) this.rdoMerge);
      this.panel1.Controls.Add((Control) this.rdoDelete);
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.rdoMerge, "rdoMerge");
      this.rdoMerge.Name = "rdoMerge";
      this.rdoMerge.TabStop = true;
      this.rdoMerge.UseVisualStyleBackColor = true;
      this.rdoMerge.CheckedChanged += new EventHandler(this.rdoOption_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.rdoDelete, "rdoDelete");
      this.rdoDelete.Name = "rdoDelete";
      this.rdoDelete.TabStop = true;
      this.rdoDelete.UseVisualStyleBackColor = true;
      this.rdoDelete.CheckedChanged += new EventHandler(this.rdoOption_CheckedChanged);
      componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ExistingPlotsForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
