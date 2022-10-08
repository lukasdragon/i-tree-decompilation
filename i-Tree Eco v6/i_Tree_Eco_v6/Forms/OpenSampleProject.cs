// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.OpenSampleProject
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using Eco.Util;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class OpenSampleProject : Form
  {
    private IContainer components;
    private Label label1;
    private Button btnOK;
    private Button btnCancel;
    private GroupBox gpProject;
    private TableLayoutPanel pnlProjects;

    public OpenSampleProject()
    {
      this.InitializeComponent();
      string[] files = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Sample Projects"), "*.ieco", SearchOption.TopDirectoryOnly);
      this.pnlProjects.RowCount = files.Length;
      for (int row = 0; row < files.Length; ++row)
      {
        string str = files[row];
        if (!FileSignature.IsSqliteDatabase(str))
        {
          --this.pnlProjects.RowCount;
        }
        else
        {
          RadioButton radioButton = new RadioButton();
          radioButton.Text = Path.GetFileNameWithoutExtension(str);
          radioButton.Tag = (object) str;
          radioButton.AutoSize = true;
          radioButton.CheckedChanged += new EventHandler(this.rbProject_CheckChanged);
          if (row == 0)
          {
            radioButton.Checked = true;
            this.FileName = str;
          }
          this.pnlProjects.SetCellPosition((Control) radioButton, new TableLayoutPanelCellPosition(0, row));
          this.pnlProjects.Controls.Add((Control) radioButton);
        }
      }
    }

    public string FileName { get; private set; }

    private void rbProject_CheckChanged(object sender, EventArgs e)
    {
      if (!(sender is RadioButton radioButton))
        return;
      this.FileName = (string) radioButton.Tag;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (OpenSampleProject));
      this.label1 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.gpProject = new GroupBox();
      this.pnlProjects = new TableLayoutPanel();
      this.gpProject.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.Name = "label1";
      componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.gpProject, "gpProject");
      this.gpProject.Controls.Add((Control) this.pnlProjects);
      this.gpProject.Name = "gpProject";
      this.gpProject.TabStop = false;
      componentResourceManager.ApplyResources((object) this.pnlProjects, "pnlProjects");
      this.pnlProjects.Name = "pnlProjects";
      this.AcceptButton = (IButtonControl) this.btnOK;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.Controls.Add((Control) this.gpProject);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (OpenSampleProject);
      this.ShowInTaskbar = false;
      this.gpProject.ResumeLayout(false);
      this.gpProject.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
