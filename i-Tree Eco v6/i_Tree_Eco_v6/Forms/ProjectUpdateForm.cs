// Decompiled with JetBrains decompiler
// Type: i_Tree_Eco_v6.Forms.ProjectUpdateForm
// Assembly: i-Tree Eco v6.x64, Version=6.0.29.0, Culture=neutral, PublicKeyToken=null
// MVID: E695D7ED-407B-4C7B-B817-3F1C5BB20211
// Assembly location: C:\Program Files (x86)\i-Tree\EcoV6\i-Tree Eco v6.x64.exe

using DaveyTree.Controls;
using i_Tree_Eco_v6.Resources;
using i_Tree_Eco_v6.Tasks;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace i_Tree_Eco_v6.Forms
{
  public class ProjectUpdateForm : Form
  {
    private bool m_error;
    private UpdateDatabaseTask m_task;
    private IContainer components;
    private TextBox txtUpdateStatus;
    private Button btnOK;
    private Label lblStatus;

    public ProjectUpdateForm(UpdateDatabaseTask task)
    {
      this.InitializeComponent();
      this.m_error = false;
      this.m_task = task;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.m_error)
        this.DialogResult = DialogResult.Abort;
      else
        this.DialogResult = DialogResult.OK;
    }

    private void ProjectUpdateForm_Load(object sender, EventArgs e)
    {
      Stream stream = (Stream) new TextBoxStream(this.txtUpdateStatus);
      StreamWriter sw = new StreamWriter(stream);
      sw.WriteLine(Strings.MsgUpdateBeginning);
      sw.Flush();
      int hdrLength = this.txtUpdateStatus.Text.Length;
      this.m_task.OutputStream = stream;
      this.m_task.DoWork().ContinueWith((Action<Task>) (task =>
      {
        if (task.IsFaulted)
        {
          sw.WriteLine(Strings.ErrUpdateProcess);
          foreach (object innerException in task.Exception.InnerExceptions)
            sw.WriteLine(innerException.ToString());
          sw.WriteLine();
        }
        this.m_error = this.txtUpdateStatus.Text.Length > hdrLength;
        sw.WriteLine(Strings.MsgUpdateComplete);
        if (this.m_error)
          sw.WriteLine(Strings.ErrUpdateFailed);
        else
          sw.WriteLine(Strings.MsgUpdateSuccess);
        sw.Flush();
        this.btnOK.Enabled = true;
      }), TaskScheduler.FromCurrentSynchronizationContext());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProjectUpdateForm));
      this.txtUpdateStatus = new TextBox();
      this.btnOK = new Button();
      this.lblStatus = new Label();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.txtUpdateStatus, "txtUpdateStatus");
      this.txtUpdateStatus.BackColor = Color.White;
      this.txtUpdateStatus.Name = "txtUpdateStatus";
      this.txtUpdateStatus.ReadOnly = true;
      componentResourceManager.ApplyResources((object) this.btnOK, "btnOK");
      this.btnOK.Name = "btnOK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      componentResourceManager.ApplyResources((object) this.lblStatus, "lblStatus");
      this.lblStatus.Name = "lblStatus";
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblStatus);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtUpdateStatus);
      this.Name = nameof (ProjectUpdateForm);
      this.ShowInTaskbar = false;
      this.Load += new EventHandler(this.ProjectUpdateForm_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
