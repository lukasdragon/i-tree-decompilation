// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ReinventoryDestinationForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ReinventoryDestinationForm : Form
  {
    private IContainer components;
    private RadioButton rdoNewFile;
    private RadioButton rdoSameFile;
    private Button cmdCancel;
    private Button cmdOK;

    public ReinventoryDestinationForm() => this.InitializeComponent();

    public bool SaveInNewFile { get; set; }

    private void cmdOK_Click(object sender, EventArgs e)
    {
      if (!this.rdoNewFile.Checked && !this.rdoSameFile.Checked)
      {
        int num = (int) MessageBox.Show("Please select where you would like the re-inventory to be saved");
      }
      else
      {
        this.SaveInNewFile = this.rdoNewFile.Checked;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void cmdCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rdoNewFile = new RadioButton();
      this.rdoSameFile = new RadioButton();
      this.cmdCancel = new Button();
      this.cmdOK = new Button();
      this.SuspendLayout();
      this.rdoNewFile.AutoSize = true;
      this.rdoNewFile.Font = new Font("Calibri", 10f);
      this.rdoNewFile.Location = new Point(12, 23);
      this.rdoNewFile.Name = "rdoNewFile";
      this.rdoNewFile.Size = new Size(216, 21);
      this.rdoNewFile.TabIndex = 0;
      this.rdoNewFile.TabStop = true;
      this.rdoNewFile.Text = "Save the new project to a new file";
      this.rdoNewFile.UseVisualStyleBackColor = true;
      this.rdoSameFile.AutoSize = true;
      this.rdoSameFile.Font = new Font("Calibri", 10f);
      this.rdoSameFile.Location = new Point(12, 46);
      this.rdoSameFile.Name = "rdoSameFile";
      this.rdoSameFile.Size = new Size(233, 21);
      this.rdoSameFile.TabIndex = 1;
      this.rdoSameFile.TabStop = true;
      this.rdoSameFile.Text = "Save the new project in this same file";
      this.rdoSameFile.UseVisualStyleBackColor = true;
      this.cmdCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cmdCancel.Font = new Font("Calibri", 10f);
      this.cmdCancel.Location = new Point(186, 106);
      this.cmdCancel.Margin = new Padding(3, 6, 3, 0);
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.Size = new Size(76, 25);
      this.cmdCancel.TabIndex = 6;
      this.cmdCancel.Text = "Cancel";
      this.cmdCancel.UseVisualStyleBackColor = true;
      this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
      this.cmdOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cmdOK.Font = new Font("Calibri", 10f);
      this.cmdOK.Location = new Point(104, 106);
      this.cmdOK.Margin = new Padding(3, 6, 3, 0);
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.Size = new Size(76, 25);
      this.cmdOK.TabIndex = 5;
      this.cmdOK.Text = "OK";
      this.cmdOK.UseVisualStyleBackColor = true;
      this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(274, 142);
      this.ControlBox = false;
      this.Controls.Add((Control) this.cmdCancel);
      this.Controls.Add((Control) this.cmdOK);
      this.Controls.Add((Control) this.rdoSameFile);
      this.Controls.Add((Control) this.rdoNewFile);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ReinventoryDestinationForm);
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "New Project";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
