// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.NewProjectType
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using Eco.Domain.v6;
using i_Tree_Eco_v6.Forms.Resources;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class NewProjectType : Form
  {
    public bool ReinventoryProject;
    private IContainer components;
    private Label label1;
    private ComboBox cmbProjectType;
    private Button cmdCancel;
    private Button cmdOK;
    private TableLayoutPanel tableLayoutPanel1;
    private Panel panel1;
    private RichTextLabel richTextLabel1;

    public NewProjectType() => this.InitializeComponent();

    public SampleType NewProjectSampleType { get; set; }

    private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (this.cmbProjectType.SelectedIndex)
      {
        case 0:
          this.richTextLabel1.RichText = ApplicationHelp.NewProjectTypeCompleteInventory;
          break;
        case 1:
          this.richTextLabel1.RichText = ApplicationHelp.NewProjectTypePlotSample;
          break;
        case 2:
          this.richTextLabel1.RichText = ApplicationHelp.NewProjectTypeReinventory;
          break;
        default:
          this.richTextLabel1.Clear();
          break;
      }
    }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      if (this.cmbProjectType.SelectedIndex == -1)
        return;
      switch (this.cmbProjectType.SelectedIndex)
      {
        case 0:
          this.NewProjectSampleType = SampleType.Inventory;
          break;
        case 1:
          this.NewProjectSampleType = SampleType.RegularPlot;
          break;
        case 2:
          this.ReinventoryProject = true;
          break;
      }
      this.DialogResult = DialogResult.OK;
    }

    private void cmdCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    private void NewProjectType_Load(object sender, EventArgs e) => this.cmbProjectType.SelectedIndex = 0;

    private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (NewProjectType));
      this.label1 = new Label();
      this.cmbProjectType = new ComboBox();
      this.cmdCancel = new Button();
      this.cmdOK = new Button();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.panel1 = new Panel();
      this.richTextLabel1 = new RichTextLabel();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      this.tableLayoutPanel1.SetColumnSpan((Control) this.cmbProjectType, 2);
      this.cmbProjectType.DropDownStyle = ComboBoxStyle.DropDownList;
      componentResourceManager.ApplyResources((object) this.cmbProjectType, "cmbProjectType");
      this.cmbProjectType.FormattingEnabled = true;
      this.cmbProjectType.Items.AddRange(new object[2]
      {
        (object) componentResourceManager.GetString("cmbProjectType.Items"),
        (object) componentResourceManager.GetString("cmbProjectType.Items1")
      });
      this.cmbProjectType.Name = "cmbProjectType";
      this.cmbProjectType.SelectedIndexChanged += new EventHandler(this.cmbProject_SelectedIndexChanged);
      componentResourceManager.ApplyResources((object) this.cmdCancel, "cmdCancel");
      this.cmdCancel.DialogResult = DialogResult.Cancel;
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.UseVisualStyleBackColor = true;
      this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
      componentResourceManager.ApplyResources((object) this.cmdOK, "cmdOK");
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.UseVisualStyleBackColor = true;
      this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
      componentResourceManager.ApplyResources((object) this.tableLayoutPanel1, "tableLayoutPanel1");
      this.tableLayoutPanel1.Controls.Add((Control) this.label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.cmdOK, 1, 2);
      this.tableLayoutPanel1.Controls.Add((Control) this.cmbProjectType, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.panel1, 0, 1);
      this.tableLayoutPanel1.Controls.Add((Control) this.cmdCancel, 2, 2);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.Paint += new PaintEventHandler(this.tableLayoutPanel1_Paint);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.panel1, 3);
      this.panel1.Controls.Add((Control) this.richTextLabel1);
      this.panel1.Name = "panel1";
      componentResourceManager.ApplyResources((object) this.richTextLabel1, "richTextLabel1");
      this.richTextLabel1.Name = "richTextLabel1";
      this.richTextLabel1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.cmdOK;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.CancelButton = (IButtonControl) this.cmdCancel;
      this.ControlBox = false;
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (NewProjectType);
      this.ShowInTaskbar = false;
      this.Load += new EventHandler(this.NewProjectType_Load);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
